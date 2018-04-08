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
    /// 实体查询增删改执行事件参数类
    /// </summary>
    public class EntityQueryExecuteEventArgs : EventArgs
    {
        /// <summary>
        /// 操作的实体类
        /// </summary>
        public EntityBase Entity { get; private set; }
        /// <summary>
        /// 执行类型
        /// </summary>
        public EntityQueryExecuteType ExecuteType { get; private set; }
        /// <summary>
        /// 执行是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 使用实体类和查询类型初始化本类
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="executeType"></param>
        public EntityQueryExecuteEventArgs(EntityBase entity,EntityQueryExecuteType executeType)
        {
            this.Entity = entity;
            this.ExecuteType = executeType;
        }
    }

    /// <summary>
    /// 实体查询类型
    /// </summary>
    public enum EntityQueryExecuteType
    { 
        /// <summary>
        /// 仅查询
        /// </summary>
        Select,
        /// <summary>
        /// 插入
        /// </summary>
        Insert,
        /// <summary>
        /// 更新
        /// </summary>
        Update,
        /// <summary>
        /// 删除
        /// </summary>
        Delete
    }

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
        //存储已经校验过的实体类的字典，Key为当前DbContext的类型的 RuntimeTypeHandle
        private static Dictionary<RuntimeTypeHandle, List<RuntimeTypeHandle>> dictCheckedEntitys = new Dictionary<RuntimeTypeHandle, List<RuntimeTypeHandle>>();
        /// <summary>
        /// 数据库文件，对于文件型数据库需要设置该字段，并且在CheckDB 实现类里面做适当的处理
        /// </summary>
        public string DBFilePath = string.Empty;
        /// <summary>
        /// 执行增删改操作前的事件
        /// </summary>
        public event EventHandler<EntityQueryExecuteEventArgs> OnBeforeExecute;
        /// <summary>
        /// 执行增删改操作之后的事件
        /// </summary>
        public event EventHandler<EntityQueryExecuteEventArgs> OnAfterExecute;
        
        
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
            string key = GetContextKey(connName);
            this.provider = contextProvider;
            dictCheckedDb.TryGetValue(key, out checkedDb);
            if (!checkedDb)
                {
                    lock (lock_obj)
                    {
                        if (!checkedDb)
                        {
                            checkedDb = CheckDB();
                            dictCheckedDb[key] = checkedDb;
                        }
                    }
                }
        }

        /// <summary>
        /// 以一个数据访问对象初始化数据上下文
        /// </summary>
        /// <param name="db">数据访问对象</param>
        public DbContext(AdoHelper db)
        {
            string key = GetContextKey(db.ConnectionString);
            dictCheckedDb.TryGetValue(key, out checkedDb);
            this.db = db;
            if (!checkedDb)
            {
                lock (lock_obj)
                {
                    if (!checkedDb)
                    {
                        checkedDb = CheckDB();//可能会改变连接字符串
                        dictCheckedDb[key] = checkedDb;
                    }
                }
            }
        }

        /// <summary>
        /// 根据连接名称或者连接字符串，结合当前实例类型名称，生成检查数据库的Key
        /// </summary>
        /// <remarks>同一个DbContext类型可能使用不同的连接初始化，原因是不同的库可能有相同的表；
        /// 同样，不同的DbConext对象也可能使用相同的连接字符串，比如某个DbContext 初始化了一个数据库的部分表清单
        /// </remarks>
        /// <param name="connNameOrString">连接名称或者连接字符串</param>
        /// <returns></returns>
        private string GetContextKey(string connNameOrString)
        {
            string typeName = GetType().FullName;
            return string.Format("CK_{0}_{1}", typeName, connNameOrString);
        }

        #region 接口实现
        /// <summary>
        /// 关联的当期数据库访问对象
        /// </summary>
        public AdoHelper CurrentDataBase
        {
            get { return db; }
        }

        /// <summary>
        /// 在数据库中检查指定的实体类映射的数据表是否存在，如果不存在，将创建表
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        public bool CheckTableExists<T>() where T : EntityBase, new()
        {
            bool flag=  DbContextProvider.CheckTableExists<T>();
            //这里记录下所有检查的表，供需要的时候使用
            RuntimeTypeHandle thisHandle = GetType().TypeHandle;
            RuntimeTypeHandle entityHandle = typeof(T).TypeHandle;
            List<RuntimeTypeHandle> list = null;
            if (!dictCheckedEntitys.ContainsKey(thisHandle))
            {
                list = new List<RuntimeTypeHandle>();
                list.Add(entityHandle);
                dictCheckedEntitys.Add(thisHandle, list);
            }
            else
            {
                list = dictCheckedEntitys[thisHandle];
                if(!list.Contains(entityHandle))
                    list.Add(entityHandle);
            }
            return flag;
        }

        /// <summary>
        /// 检查实体类对应的表是否存在，如果不存在则创建表并执行可选的SQL语句，比如为表增加索引等。
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="initSql">要初始化执行的SQL语句，为空则忽略，支持{0} 占位符，者将会用表名称替换。</param>
        public void InitializeTable<T>(string initSql) where T : EntityBase, new()
        {
            if (!CheckTableExists<T>())
            {
                if (!string.IsNullOrEmpty(initSql))
                {
                    T entity = new T();
                    string tableName = entity.GetTableName();
                    string sql = string.Format(initSql, tableName);
                    CurrentDataBase.ExecuteNonQuery(sql);
                }
            }
        }
        #endregion

        /// <summary>
        /// 解析出所有检验过表存在的实体类（当前类注册的实体类）
        /// </summary>
        /// <returns></returns>
        public List<EntityBase> ResolveAllEntitys()
        {
            List<EntityBase> listEntity = new List<EntityBase>();
            RuntimeTypeHandle thisHandle = GetType().TypeHandle;
            foreach (RuntimeTypeHandle handle in dictCheckedEntitys[thisHandle])
            {
                Type entityType = Type.GetTypeFromHandle(handle);
                EntityBase entity = (EntityBase)Activator.CreateInstance(entityType);
                listEntity.Add(entity);
            }
            return listEntity;
        }

        /// <summary>
        /// 在数据库中检查指定的接口类型映射的数据表是否存在，如果不存在，将创建表
        /// </summary>
        /// <typeparam name="T">实体类接口类型</typeparam>
        public void CheckTableExistsOf<T>() where T:class   
        {
            //DbContextProvider.CheckTableExists<T>();
            T obj = EntityBuilder.CreateEntity<T>();
            var mi = DbContextProvider.GetType().GetMethod("CheckTableExists");
            var g_mi= mi.MakeGenericMethod(obj.GetType());
            g_mi.Invoke(DbContextProvider,null);
        }


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
            //可能会开启事务日志，这里需要回复事务连接的连接字符串
            bool result = false;
            bool oldEnable = CurrentDataBase.EnableCommandHandle;
            //检查数据库的时候不能开启命令管道，否则在事务日志处理器里面，可能尝试记录命令消息取找不到对应的数据库
            CurrentDataBase.EnableCommandHandle = false;
            if (this.DbContextProvider.CheckDB())
            {
                if (CurrentDataBase.Transaction != null)
                    CurrentDataBase.Transaction.Connection.ConnectionString = CurrentDataBase.ConnectionString;
                result = CheckAllTableExists();//其它类型的数据库，仅检查表是否存在
            }
            else
            {
                if (CurrentDataBase.Transaction != null)
                    CurrentDataBase.Transaction.Connection.ConnectionString = CurrentDataBase.ConnectionString;
            }
            CurrentDataBase.EnableCommandHandle = oldEnable;
            return result;
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
        /// 查询一个实体类对象
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="q">查询表达式</param>
        /// <returns>实体类</returns>
        public T QueryObject<T>(OQL q) where T : EntityBase, new()
        {
            return EntityQuery<T>.QueryObject(q, this.CurrentDataBase);
        }

        /// <summary>
        /// 查询实体类列表
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="q">OQL查询表达式</param>
        /// <returns>实体类列表</returns>
        public List<T> QueryList<T>(OQL q) where T : EntityBase, new()
        {
            var list= this.NewQuery<T>().GetList(q);
            db.Logger.WriteLog("记录条数：" + list.Count, "DbContext");
            return list;
        }

        /// <summary>
        /// 查询指定实体类类型的全部数据
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <returns>实体类列表</returns>
        public List<T> QueryAllList<T>() where T : EntityBase, new()
        {
            return OQL.From<T>().ToList(this.CurrentDataBase);
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
        /// Role role=new Role(){/* init property value */  };
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
        private int ExecuteQuery<T>(T data, Func<EntityQuery, EntityBase, int> fun ,EntityQueryExecuteType executeType) where T : class
        {
            EntityBase entity = data as EntityBase;
            if (entity == null) //T 是接口类型，data 是一个实现了该接口的DTO
            {
                T temp = EntityBuilder.CreateEntity<T>();
                entity = temp as EntityBase;

                entity.MapFrom(data,true);//使用该重载方法，可以不用调用下面一行代码
                //entity.ResetChanges(true);
            }

            EntityQueryExecuteEventArgs args = new EntityQueryExecuteEventArgs(entity, executeType);
            if (this.OnBeforeExecute != null)
                this.OnBeforeExecute(this, args);

            EntityQuery eq = new EntityQuery(CurrentDataBase);
           
            int accept = fun(eq, entity);
            args.Success = accept > 0;

            if (this.OnAfterExecute != null)
                this.OnAfterExecute(this, args);
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
            //已经在EntityQuery 处理，下面的代码注释
            //Oracle 处理自增 
            //if (CurrentDataBase.CurrentDBMSType == Common.DBMSType.Oracle)
            //{
            //    //EntityBase entity = data as EntityBase;
            //    //if (entity == null) //T 是接口类型，data 是一个实现了该接口的DTO
            //    //{
            //    //    T temp = EntityBuilder.CreateEntity<T>();
            //    //    entity = temp as EntityBase;
            //    //}
            //    ////string seqName = entity.GetTableName() + "_" + entity.GetIdentityName() + "_SEQ";
            //    ////CurrentDataBase.InsertKey = "select " + seqName + ".currval as id from dual";
            //    //int result = ExecuteQuery<T>(data, (q, e) => q.Insert(e));
            //    //return result;
            //}
            //else
            //{
                return ExecuteQuery<T>(data, (q, e) => q.Insert(e), EntityQueryExecuteType.Insert);
            //}

        }

        /// <summary>
        /// 修改一个数据到数据库中
        /// </summary>
        /// <typeparam name="T">实体类或者接口</typeparam>
        /// <param name="data">要修改的数据</param>
        /// <returns>操作受影响的行数</returns>
        public int Update<T>(T data) where T : class
        {
            return ExecuteQuery<T>(data, (q, e) => q.Update(e), EntityQueryExecuteType.Update);
        }

        /// <summary>
        /// 从数据库中删除一个数据到，数据必须有主键
        /// </summary>
        /// <typeparam name="T">实体类或者接口</typeparam>
        /// <param name="data">要删除的数据</param>
        /// <returns>操作受影响的行数</returns>
        public int Remove<T>(T data) where T : class
        {
            return ExecuteQuery<T>(data, (q, e) => q.Delete(e), EntityQueryExecuteType.Delete);
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
            if (typeof(T).BaseType == typeof(EntityBase))
            {
                foreach (T data in list)
                {
                    EntityBase entity = data as EntityBase;
                    objList.Add(entity);
                }
            }
            else
            {
                foreach (T data in list)
                {
                    //根据接口创建实际的实体类对象
                    T obj = EntityBuilder.CreateEntity<T>();
                    EntityBase entity = obj as EntityBase;
                    //为实体类属性赋值
                    entity.MapFrom(data, true);//使用该重载，不用调用下面一行代码了
                    //entity.ResetChanges(true);

                    objList.Add(entity);
                }
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
