using PWMIS.Core.Interface;
using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PWMIS.Core.Extensions
{
    public sealed class OracleDbContext : IDbContextProvider
    {
        public AdoHelper CurrentDataBase { get; private set; }

        /// <summary>
        /// 用连接字符串名字初始化本类
        /// </summary>
        /// <param name="connName"></param>
        public OracleDbContext(AdoHelper db)
        {
            if (db.CurrentDBMSType != Common.DBMSType.Oracle)
                throw new Exception("当前数据库类型不是Oracle ");
            this.CurrentDataBase = db;
        }
        /// <summary>
        /// 检查实体类对应的数据表是否在数据库中存在
        /// </summary>
        public  void CheckTableExists<T>() where T : EntityBase, new()
        {
            //创建表
            var entity = new T();
            var dsScheme = CurrentDataBase.GetSchema("Tables", null);
            string owner = CurrentDataBase.ConnectionUserID;
            var rows = dsScheme.Select("OWNER='" + owner + "' and table_name='" + entity.GetTableName() + "'");
            if (rows.Length == 0)
            {
                EntityCommand ecmd = new EntityCommand(entity, CurrentDataBase);
                string sql = ecmd.CreateTableCommand;
                //OracleClient 不能批量执行多条SQL语句
                string[] sqlArr = sql.Split(new string[] { ";--" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in sqlArr)
                {
                    if (item.Length > 10) //去除回车行
                        CurrentDataBase.ExecuteNonQuery(item);
                }

            }
        }


        public bool CheckDB()
        {
            //带实现详细的创建数据库的过程
            return true;
        }
    }
}
