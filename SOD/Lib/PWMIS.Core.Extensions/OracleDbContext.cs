using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PWMIS.Core.Extensions
{
    public abstract class OracleDbContext :DbContext
    {
        /// <summary>
        /// 用连接字符串名字初始化本类
        /// </summary>
        /// <param name="connName"></param>
        public OracleDbContext(string connName)
            : base(connName)
        { 
        
        }
        /// <summary>
        /// 检查实体类对应的数据表是否在数据库中存在
        /// </summary>
        protected override void CheckTableExists<T>()
        {
            //创建表
            if (CurrentDataBase.CurrentDBMSType == PWMIS.Common.DBMSType.Oracle)
            {
                var entity = new T();
                var dsScheme = CurrentDataBase.GetSchema("Tables", null);
                string owner = CurrentDataBase.ConnectionUserID;
                var rows = dsScheme.Select("OWNER='"+ owner +"' and table_name='" + entity.GetTableName() + "'");
                if (rows.Length == 0)
                {
                    EntityCommand ecmd = new EntityCommand(entity, CurrentDataBase);
                    string sql = ecmd.CreateTableCommand;
                    //OracleClient 不能批量执行多条SQL语句
                    string[] sqlArr = sql.Split(new string[] {";--" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in sqlArr)
                    {
                        if(item.Length >10) //去除回车行
                            CurrentDataBase.ExecuteNonQuery(item);
                    }
                   
                }
            }
        }
    }
}
