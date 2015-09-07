using PWMIS.Core.Interface;
using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PWMIS.Core.Extensions
{
    /// <summary>
    /// SqlServer 数据上下文基类，可以自动创建实体类对应的表
    /// </summary>
    public sealed class SqlServerDbContext : IDbContextProvider
    {
       public  AdoHelper CurrentDataBase { get; private set; }
        /// <summary>
        /// 用连接字符串名字初始化本类
        /// </summary>
        /// <param name="connName"></param>
        public SqlServerDbContext(AdoHelper db)
        {
            if (db.CurrentDBMSType != Common.DBMSType.SqlServer)
                throw new Exception("当前数据库类型不是SqlServer ");
            this.CurrentDataBase = db;
        }
        /// <summary>
        /// 检查实体类对应的数据表是否在数据库中存在
        /// </summary>
        public void CheckTableExists<T>() where T : EntityBase, new()
        {
            //创建表
            var entity = new T();
            var dsScheme = CurrentDataBase.GetSchema("Tables", new string[] { null, null, null, "BASE TABLE" });
            var rows = dsScheme.Select("table_name='" + entity.GetTableName() + "'");
            if (rows.Length == 0)
            {
                EntityCommand ecmd = new EntityCommand(entity, CurrentDataBase);
                string sql = ecmd.CreateTableCommand;
                CurrentDataBase.ExecuteNonQuery(sql);
            }
        }


        public bool CheckDB()
        {
            var connBuilder = CurrentDataBase.ConnectionStringBuilder as System.Data.SqlClient.SqlConnectionStringBuilder;
            string database = connBuilder.InitialCatalog;
            if (!string.IsNullOrEmpty(database))
            {
                string sqlformat = @"
if not exists (select * From master.dbo.sysdatabases where name='{0}')
create database '{1}'
";
                string sql = string.Format(sqlformat, database, database);
                CurrentDataBase.ExecuteNonQuery(sql);
            }
            return true;
        }
    }
}
