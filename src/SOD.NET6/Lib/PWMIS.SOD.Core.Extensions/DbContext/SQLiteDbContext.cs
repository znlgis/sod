using System;
using System.Collections.Generic;
using System.Text;
using PWMIS.Core.Interface;
using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Data;

namespace PWMIS.Core.Extensions
{
    /// <summary>
    /// SQLite数据库上下文，可以实现自动检查数据库，创建表，获取EntityQuery 泛型实例对象等功能，封装了AdoHelper的使用。
    /// </summary>
    public class SQLiteDbContext: IDbContextProvider
    {
        public AdoHelper CurrentDataBase { get; private set; }

        /// <summary>
        /// 用连接字符串名字初始化本类
        /// </summary>
        public SQLiteDbContext(AdoHelper db)
        {
            this.CurrentDataBase = db;
        }
        /// <summary>
        /// 检查实体类对应的数据表是否在数据库中存在
        /// </summary>
        public bool CheckTableExists<T>(T entity = null) where T : EntityBase, new()
        {
            //创建表
            if (CurrentDataBase.CurrentDBMSType == PWMIS.Common.DBMSType.SQLite)
            {
                if (entity == null) entity = new T();
                var dsScheme = CurrentDataBase.GetSchema("Tables", null);
                var rows = dsScheme.Select("table_name='" + entity.GetTableName() + "'");
                if (rows.Length == 0)
                {
                    EntityCommand ecmd = new EntityCommand(entity, CurrentDataBase);
                    string sql = ecmd.CreateTableCommand;
                    CurrentDataBase.ExecuteNonQuery(sql);
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检查实体类和表名对应的数据表是否在数据库中存在
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <typeparam name="T">实体类</typeparam>
        public bool CheckTableExists<T>(string tableName) where T : EntityBase, new()
        {
            //创建表
            if (CurrentDataBase.CurrentDBMSType == PWMIS.Common.DBMSType.SQLite)
            {
                var entity = new T();
                entity.MapNewTableName(tableName);
                var dsScheme = CurrentDataBase.GetSchema("Tables", null);
                var rows = dsScheme.Select("table_name='" + entity.GetTableName() + "'");
                if (rows.Length == 0)
                {
                    EntityCommand ecmd = new EntityCommand(entity, CurrentDataBase);
                    string sql = ecmd.CreateTableCommand;
                    CurrentDataBase.ExecuteNonQuery(sql);
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检查实体类对应的表是否存在，如果不存在则创建表并执行可选的SQL语句，比如为表增加索引等。
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="initSql">要初始化执行的SQL语句</param>
        public void InitializeTable<T>(string initSql, T entity = null) where T : EntityBase, new()
        {
            if (!CheckTableExists<T>(entity))
            {
                CurrentDataBase.ExecuteNonQuery(initSql);
            }
        }


        public bool CheckDB()
        {
            //throw new NotImplementedException();
            return true;
        }
    }
}
