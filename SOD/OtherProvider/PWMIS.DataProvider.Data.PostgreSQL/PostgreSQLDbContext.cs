using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Data;
using PWMIS.Core.Interface;

namespace PWMIS.PostgreSQLClient
{
    /// <summary>
    /// PostgreSQL数据库上下文，可以实现自动检查数据库，创建表，获取EntityQuery 泛型实例对象等功能，封装了AdoHelper的使用。
    /// </summary>
    public abstract class PostgreSQLDbContext : IDbContextProvider
    {
        public AdoHelper CurrentDataBase { get; private set; }
        /// <summary>
        /// 用连接字符串名字初始化本类
        /// </summary>
        /// <param name="connName"></param>
        public PostgreSQLDbContext(AdoHelper db)
        {
            this.CurrentDataBase = db;
        }
        /// <summary>
        /// 检查实体类对应的数据表是否在数据库中存在
        /// </summary>
        public void CheckTableExists<T>() where T : EntityBase, new()
        {
            //创建表
            if (CurrentDataBase.CurrentDBMSType == PWMIS.Common.DBMSType.PostgreSQL)
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
        /// 检查实体类和表名对应的数据表是否在数据库中存在
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <typeparam name="T">实体类</typeparam>
        public void CheckTableExists<T>(string tableName) where T : EntityBase, new()
        {
            //创建表
            if (CurrentDataBase.CurrentDBMSType == PWMIS.Common.DBMSType.PostgreSQL)
            {
                var entity = new T();
                entity.MapNewTableName(tableName);
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
    }
}
