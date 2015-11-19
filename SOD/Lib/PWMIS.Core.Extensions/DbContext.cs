using System;
using System.Collections.Generic;
using System.Text;
using PWMIS.DataProvider.Adapter;
using System.IO;
using PWMIS.DataProvider.Data;
using PWMIS.DataMap.Entity;
using PWMIS.Core.Interface;

namespace PWMIS.Core.Extensions
{
    /// <summary>
    /// 数据上下文，可以实现自动检查数据库，创建表，获取EntityQuery 泛型实例对象等功能，封装了AdoHelper的使用。
    /// </summary>
    public abstract class DbContext:IDbContextProvider
    {
        private AdoHelper db;
        private IDbContextProvider provider ;
        private static object lock_obj = new object();
        private bool checkedDb = false;//数据库文件是否已经创建
        private static Dictionary<string, bool> dictCheckedDb = new Dictionary<string, bool>();
        /// <summary>
        /// 数据库文件，对于文件型数据库需要设置该字段，并且在CheckDB 实现类里面做适当的处理
        /// </summary>
        public string DBFilePath = string.Empty;
        /// <summary>
        /// 初始化数据访问上下文,程序会自动寻找合适的数据上下文提供程序
        /// </summary>
        /// <param name="connName">在应用程序配置文件的数据库连接配置的连接名称</param>
        public DbContext(string connName):this(connName,null)
        {
           
        }

        /// <summary>
        /// 使用数据库连接配置名字和数据上下文提供程序,初始化数据访问上下文
        /// </summary>
        /// <param name="connName">数据库连接配置名字</param>
        /// <param name="contextProvider">数据上下文提供程序</param>
        public DbContext(string connName, IDbContextProvider contextProvider)
        {
            db = MyDB.GetDBHelperByConnectionName(connName);
            //在这里初始化合适的 IDbContextProvider
            this.provider = contextProvider;
            dictCheckedDb.TryGetValue(connName, out checkedDb);
            if (!checkedDb)
                {
                    lock (lock_obj)
                    {
                        if (!checkedDb)
                        {
                            checkedDb = CheckDB();
                            dictCheckedDb[connName] = checkedDb;
                        }
                    }
                }
        }

        #region 接口实现
        /// <summary>
        /// 关联的当期数据库访问对象
        /// </summary>
        public AdoHelper CurrentDataBase
        {
            get { return db; }
        }

        public void CheckTableExists<T>() where T : EntityBase, new()
        {
            DbContextProvider.CheckTableExists<T>();
        }
        #endregion

        /// <summary>
        /// 获取数据上下文提供程序
        /// </summary>
        public IDbContextProvider DbContextProvider
        {
            get
            {
                if (this.provider == null)
                    InitContextProvider();
                return this.provider;
            }
        }

        /// <summary>
        /// 检查数据库和相关的表是否已经初始化。如果是Access 数据库，还会检查数据库文件是否存在。
        /// 如果需要更多的检查，可以重写该方法，但一定在方法第一行保留 base.CheckDB();这行代码。
        /// </summary>
        /// <returns>检查是否通过</returns>
        public  bool CheckDB()
        {
            if (this.DbContextProvider.CheckDB())
                return CheckAllTableExists();//其它类型的数据库，仅检查表是否存在
            else
                return false;
        }

        /// <summary>
        /// 检查所有的表是否存在，需要在子类里面实现。
        /// </summary>
        /// <returns></returns>
        protected abstract bool CheckAllTableExists();
        
       
        
        /// <summary>
        /// 创建一个新的EntityQuery泛型类实例对象
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <returns></returns>
        protected EntityQuery<T> NewQuery<T>() where T : EntityBase, new()
        {
            return new EntityQuery<T>(db);
        }
        /// <summary>
        /// 开启事务执行上下文，程序会自动提交或者回滚事务。
        /// </summary>
        /// <typeparam name="T">数据上下文类型</typeparam>
        /// <param name="instance">实例对象</param>
        /// <param name="action">操作的方法</param>
        /// <param name="errorMessage">出错信息</param>
        /// <returns>事务是否执行成功</returns>
        public static bool Transaction<T>(T instance, Action<T> action, out string errorMessage) where T : DbContext
        {
            try
            {
                instance.CurrentDataBase.BeginTransaction();

                action(instance);
                instance.CurrentDataBase.Commit();
                errorMessage = "";
                return true;
            }
            catch (Exception ex)
            {
                instance.CurrentDataBase.Rollback();
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 开启事务执行上下文，如果你的方法执行成功，自动提交事务，否则，回滚事务。
        /// <example>
        /// <![CDATA[
        /// string msg;
        /// User user=new User(){ /* init property value */ };
        /// Role role=new Role(){/* init property value */  }
        /// 
        /// LocalContext localDb=new LocalContext("local");
        /// bool result= localDb.Transaction( c=>{
        ///                      c.Add(user);
        ///                      c.Add(role);
        ///                                       },out msg);
        /// ]]>
        /// </example>
        /// </summary>
        /// <param name="action">自定义的操作方法</param>
        /// <param name="errorMessage">出错信息</param>
        /// <returns>事务是否执行成功</returns>
        public bool Transaction(Action<DbContext> action, out string errorMessage) 
        {
            return Transaction(this, action, out errorMessage);
        }

        #region 增，删，改公共方法
        private int ExecuteQuery<T>(T data, Func<EntityQuery, EntityBase, int> fun) where T : class
        {
            EntityBase entity = data as EntityBase;
            if (entity == null) //T 是接口类型，data 是一个实现了该接口的DTO
            {
                T temp = EntityBuilder.CreateEntity<T>();
                entity = temp as EntityBase;

                entity.MapFrom(data,true);//使用该重载方法，可以不用调用下面一行代码
                //entity.ResetChanges(true);
            }

            EntityQuery eq = new EntityQuery(CurrentDataBase);
           
            int accept = fun(eq, entity);
            return accept;
        }

        /// <summary>
        /// 增加一个数据到数据库中
        /// </summary>
        /// <typeparam name="T">实体类或者接口</typeparam>
        /// <param name="data">要增加的数据</param>
        /// <returns>操作受影响的行数</returns>
        public int Add<T>(T data) where T : class
        {
            //Oracle 处理自增
            if (CurrentDataBase.CurrentDBMSType == Common.DBMSType.Oracle)
            {
                EntityBase entity = data as EntityBase;
                if (entity == null) //T 是接口类型，data 是一个实现了该接口的DTO
                {
                    T temp = EntityBuilder.CreateEntity<T>();
                    entity = temp as EntityBase;
                }
                string seqName = entity.GetTableName() + "_" + entity.GetIdentityName() + "_SEQ";
                CurrentDataBase.InsertKey = "select " + seqName + ".currval as id from dual";

                int result = ExecuteQuery<T>(data, (q, e) => q.Insert(e));
                CurrentDataBase.InsertKey = "";//恢复
                return result;
            }
            else
            {
                return ExecuteQuery<T>(data, (q, e) => q.Insert(e));
            }
        }

        /// <summary>
        /// 修改一个数据到数据库中
        /// </summary>
        /// <typeparam name="T">实体类或者接口</typeparam>
        /// <param name="data">要修改的数据</param>
        /// <returns>操作受影响的行数</returns>
        public int Update<T>(T data) where T : class
        {
            return ExecuteQuery<T>(data, (q, e) => q.Update(e));
        }

        /// <summary>
        /// 从数据库中删除一个数据到，数据必须有主键
        /// </summary>
        /// <typeparam name="T">实体类或者接口</typeparam>
        /// <param name="data">要删除的数据</param>
        /// <returns>操作受影响的行数</returns>
        public int Remove<T>(T data) where T : class
        {
            return ExecuteQuery<T>(data, (q, e) => q.Delete(e));
        }

        /// <summary>
        /// 添加一个列表数据到数据库中
        /// </summary>
        /// <typeparam name="T">实体类或者接口</typeparam>
        /// <param name="list">数据列表</param>
        /// <returns>操作受影响的行数</returns>
        public int AddList<T>(IEnumerable<T> list) where T : class
        {
            List<EntityBase> objList = new List<EntityBase>();
            foreach (T data in list)
            {
                //根据接口创建实际的实体类对象
                T obj = EntityBuilder.CreateEntity<T>();
                EntityBase entity = obj as EntityBase;
                //为实体类属性赋值
                entity.MapFrom(data,true);//使用该重载，不用调用下面一行代码了
                //entity.ResetChanges(true);

                objList.Add(entity);
            }

            EntityQuery eq = new EntityQuery(CurrentDataBase);
            return eq.Insert(objList);
        }

        #endregion

        /// <summary>
        /// 初始化DbContextProvider ,如果是SqlServer,Oracle之外的数据库，需要按照约定，提供XXXDbContext
        /// <remarks>
        ///   约定，根据 CurrentDataBase 所在的程序集，来确定 XXXDbContext 的位置
        ///  XXXDbContext的名字，XXX总是CurrentDataBase的类型名字，(Name,not full Name)
        ///  XXXDbContext 可以在不同的命名空间中
        /// </remarks>
        /// </summary>
        private void InitContextProvider()
        {
            if (CurrentDataBase.CurrentDBMSType == Common.DBMSType.SqlServer)
            {
                provider = new SqlServerDbContext(CurrentDataBase);
            }
            else if (CurrentDataBase.CurrentDBMSType == Common.DBMSType.Oracle)
            {
                provider = new OracleDbContext(CurrentDataBase);
            }
            else if (CurrentDataBase.CurrentDBMSType == Common.DBMSType.Access)
            {
                var assembly = System.Reflection.Assembly.Load("PWMIS.Access.Extensions");
                string typeName = "PWMIS.AccessExtensions.AccessDbContext";
                var obj = assembly.CreateInstance(typeName, false,
                    System.Reflection.BindingFlags.Default, null, new object[] { CurrentDataBase }, null, null);
                provider = obj as IDbContextProvider;
                if (provider == null)
                    throw new Exception("类型 " + typeName + " 不是IDbContextProvider 的实例类型");
            }
            else
            {
                //约定，根据 CurrentDataBase 所在的程序集，来确定 XXXDbContext 的位置
                //XXXDbContext的名字，XXX总是CurrentDataBase的类型名字，(Name,not full Name)
                //XXXDbContext 可以在不同的命名空间中
                var assembly = System.Reflection.Assembly.GetAssembly(CurrentDataBase.GetType());

                string typeName = CurrentDataBase.GetType().Name + "DbContext";
                foreach (Type t in assembly.GetTypes())
                {
                    if (t.Name == typeName)
                    {
                        var obj = Activator.CreateInstance(t, CurrentDataBase);
                        provider = obj as IDbContextProvider;
                        if (provider == null)
                            throw new Exception("类型 " + typeName + " 不是IDbContextProvider 的实例类型");
                        break;
                    }
                }
                if (provider == null)
                    throw new Exception("未能在程序集 " + assembly.FullName + " 中找到约定的DbContext 类型： " + typeName);
            }
        }
    }
}
