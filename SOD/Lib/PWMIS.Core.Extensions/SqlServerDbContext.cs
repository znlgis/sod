using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PWMIS.Core.Extensions
{
    /// <summary>
    /// SqlServer 数据上下文基类，可以自动创建实体类对应的表
    /// </summary>
    public abstract class SqlServerDbContext : DbContext
    {
        /// <summary>
        /// 用连接字符串名字初始化本类
        /// </summary>
        /// <param name="connName"></param>
        public SqlServerDbContext(string connName) :base(connName)
        { 
        
        }
        /// <summary>
        /// 检查实体类对应的数据表是否在数据库中存在
        /// </summary>
        protected override void CheckTableExists<T>()
        {
            //创建表
            if (CurrentDataBase.CurrentDBMSType == PWMIS.Common.DBMSType.SqlServer)
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


    }
}
