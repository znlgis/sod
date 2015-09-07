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


        public bool CheckDB()
        {
            //throw new NotImplementedException();
            return true;
        }
    }
}
