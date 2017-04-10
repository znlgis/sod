using MySql.Data.MySqlClient;
using PWMIS.Core.Interface;
using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PWMIS.DataProvider.Data
{
    /// <summary>
    /// MySQL 数据上下文提供程序
    /// </summary>
    public class MySQLDbContext:IDbContextProvider
    {
        public AdoHelper CurrentDataBase
        {
            get;
            private set;
        }

        public MySQLDbContext(AdoHelper db)
        {
            this.CurrentDataBase = db;
        }

        /// <summary>
        /// 检查表是否存在，如果不存在，则创建
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void CheckTableExists<T>() where T : DataMap.Entity.EntityBase, new()
        {
            //创建表
            if (CurrentDataBase.CurrentDBMSType == PWMIS.Common.DBMSType.MySql)
            {
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
        }

        /// <summary>
        /// 检查MySQL数据库是否存在，如果不存在会自动创建
        /// </summary>
        /// <returns></returns>
        public bool CheckDB()
        {
            var connBuilder = CurrentDataBase.ConnectionStringBuilder as MySqlConnectionStringBuilder;
            string database = connBuilder.Database;
            if (!string.IsNullOrEmpty(database))
            {
                string sqlformat = "CREATE DATABASE IF NOT EXISTS {0}  DEFAULT CHARSET utf8 COLLATE utf8_general_ci";
                string sql = string.Format(sqlformat, database);
                //移除初始化的数据库名称，否则下面的执行打不开数据库
                connBuilder.Database = "";
                CurrentDataBase.ConnectionString = connBuilder.ConnectionString;
                CurrentDataBase.ExecuteNonQuery(sql);
                //恢复连接字符串
                connBuilder.Database = database;
                CurrentDataBase.ConnectionString = connBuilder.ConnectionString;
            }
            return true;
        }
    }
}
