using System;
using System.Collections.Generic;
using System.Text;
using PWMIS.DataProvider.Adapter;
using System.IO;
using PWMIS.DataProvider.Data;
using PWMIS.DataMap.Entity;

namespace PWMIS.Core.Extensions
{
    /// <summary>
    /// 数据上下文，可以实现自动检查数据库，创建表，获取EntityQuery 泛型实例对象等功能，封装了AdoHelper的使用。
    /// </summary>
    public abstract class DbContext
    {
        private AdoHelper db;
        private static object lock_obj = new object();
        private static bool checkedDb = false;//数据库文件是否已经创建
        /// <summary>
        /// 数据库文件，对于文件型数据库需要设置该字段，并且在CheckDB 实现类里面做适当的处理
        /// </summary>
        public static string DBFilePath = string.Empty;
        /// <summary>
        /// 初始化数据访问上下文
        /// </summary>
        /// <param name="connName">在应用程序配置文件的数据库连接配置的连接名称</param>
        public DbContext(string connName)
        {
            db = MyDB.GetDBHelperByConnectionName(connName);
            if (!checkedDb)
            {
                lock (lock_obj)
                {
                    if (!checkedDb)
                        checkedDb = CheckDB();
                }
            }
        }

        /// <summary>
        /// 关联的当期数据库访问对象
        /// </summary>
        public AdoHelper CurrentDataBase
        {
            get { return db; }
        }

        /// <summary>
        /// 检查数据库，检查表是否已经初始化。如果是Access 数据库，还会检查数据库文件是否存在，可以在系统中设置DBFilePath 字段。
        /// 如果需要更多的检查，可以重写该方法，但一定请保留 base.CheckDB();这行代码。
        /// </summary>
        /// <returns>检查是否通过</returns>
        protected virtual bool CheckDB()
        {
            //其它类型的数据库，仅检查表是否存在
            return CheckAllTableExists();
        }

        /// <summary>
        /// 检查所有的表是否存在，需要在子类里面实现。
        /// </summary>
        /// <returns></returns>
        protected abstract bool CheckAllTableExists();
        /// <summary>
        /// 检查实体类对应的数据表是否在数据库中存在，需要在子类中实现
        /// </summary>
        /// <typeparam name="T"></typeparam>
        protected abstract void CheckTableExists<T>() where T : EntityBase, new();
        
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
        /// <returns>事务是否执行成功。</returns>
        public static bool Tansaction<T>(T instance, Action<T> action, out string errorMessage) where T : DbContext
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

        #region 增，删，改公共方法
        private int ExecuteQuery<T>(T data, Func<EntityQuery, EntityBase, int> fun) where T : class
        {
            EntityBase entity = data as EntityBase;
            if (entity == null) //T 是接口类型，data 是一个实现了该接口的DTO
            {
                T temp = EntityBuilder.CreateEntity<T>();
                entity = temp as EntityBase;

                entity.MapFrom(data);
                entity.ResetChanges(true);
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
            return ExecuteQuery<T>(data, (q, e) => q.Insert(e));
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
                entity.MapFrom(data);
                entity.ResetChanges(true);

                objList.Add(entity);
            }

            EntityQuery eq = new EntityQuery(CurrentDataBase);
            return eq.Insert(objList);
        }

        #endregion
    }
}
