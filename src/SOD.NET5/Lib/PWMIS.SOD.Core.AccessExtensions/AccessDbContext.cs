using System;
using System.Data.OleDb;
using System.IO;
using PWMIS.Common;
using PWMIS.Core.Interface;
using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Data;

namespace PWMIS.AccessExtensions
{
    /// <summary>
    ///     Access数据库上下文，可以实现自动检查数据库，创建表，获取EntityQuery 泛型实例对象等功能，封装了AdoHelper的使用。
    /// </summary>
    public class AccessDbContext : IDbContextProvider
    {
        public string DBFilePath = string.Empty;

        public AccessDbContext(AdoHelper db)
        {
            if (db.CurrentDBMSType != DBMSType.Access)
                throw new Exception("当前数据库类型不是Access ");
            CurrentDataBase = db;
        }

        public AdoHelper CurrentDataBase { get; }

        /// <summary>
        ///     检查数据库，检查表是否已经初始化。如果是Access 数据库，还会检查数据库文件是否存在，可以在系统中设置DBFilePath 字段。
        ///     如果需要更多的检查，可以重写该方法，但一定请保留 base.CheckDB();这行代码。
        /// </summary>
        /// <returns>检查是否通过</returns>
        public bool CheckDB()
        {
            if (DBFilePath == string.Empty)
            {
                var objDataDir = AppDomain.CurrentDomain.GetData("DataDirectory");
                var dataSource = ((OleDbConnectionStringBuilder)CurrentDataBase.ConnectionStringBuilder).DataSource;
                if (objDataDir != null)
                {
                    var dbPath = objDataDir.ToString();
                    DBFilePath = dataSource.Replace("|DataDirectory|", dbPath);
                }
                else
                {
                    DBFilePath = dataSource;
                }
            }

            if (DBFilePath != string.Empty)
            {
                var directory = Path.GetDirectoryName(DBFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                if (!File.Exists(DBFilePath))
                    //创建数据库文件
                    AccessUility.CreateDataBase(DBFilePath,
                        CurrentDataBase.ConnectionStringBuilder as OleDbConnectionStringBuilder);
            }

            return true;
        }

        /// <summary>
        ///     检查实体类对应的数据表是否在数据库中存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public bool CheckTableExists<T>(T entity = null) where T : EntityBase, new()
        {
            //创建表
            if (entity == null) entity = new T();
            var access = (Access)CurrentDataBase;
            var dsScheme = access.GetSchema("Tables", new[] { null, null, null, "TABLE" });
            var rows = dsScheme.Select("table_name='" + entity.GetTableName() + "'");
            if (rows.Length == 0)
            {
                AccessUility.CreateTable(access, entity);
                return false;
            }

            return true;
        }

        /// <summary>
        ///     检查实体类对应的表是否存在，如果不存在则创建表并执行可选的SQL语句，比如为表增加索引等。
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="initSql">要初始化执行的SQL语句</param>
        /// <param name="entity">对应的实体类，可选</param>
        public void InitializeTable<T>(string initSql, T entity = null) where T : EntityBase, new()
        {
            if (!CheckTableExists(entity)) CurrentDataBase.ExecuteNonQuery(initSql);
        }
    }
}