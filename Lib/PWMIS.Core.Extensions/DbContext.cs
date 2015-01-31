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
            //if (CurrentDataBase.CurrentDBMSType == PWMIS.Common.DBMSType.Access)
            //{
            //    if (DBFilePath != string.Empty)
            //    {
            //        if (!File.Exists(DBFilePath))
            //        {
            //            //创建数据库文件
            //            PWMIS.AccessExtensions.AccessUility.CreateDataBase(DBFilePath);
            //        }
            //        return CheckAllTableExists();
            //    }
            //}
            //else
            //{
            //    //其它类型的数据库，仅检查表是否存在
            //    return CheckAllTableExists();
            //}
            //return false;

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
        protected virtual void CheckTableExists<T>() where T : EntityBase, new()
        {
            ////创建表
            //if (db.CurrentDBMSType == PWMIS.Common.DBMSType.Access)
            //{
            //    var entity = new T();
            //    Access access = (Access)db;
            //    var dsScheme = access.GetSchema("Tables", new string[] { null, null, null, "TABLE" });
            //    var rows = dsScheme.Select("table_name='" + entity.GetTableName() + "'");
            //    if (rows.Length == 0)
            //    {
            //        PWMIS.AccessExtensions.AccessUility.CreateTable(access, entity);
            //    }
            //}
        }
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
    }
}
