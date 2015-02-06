using PWMIS.Core.Extensions;
using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PWMIS.AccessExtensions
{
     /// <summary>
    /// Access数据库上下文，可以实现自动检查数据库，创建表，获取EntityQuery 泛型实例对象等功能，封装了AdoHelper的使用。
    /// </summary>
    public abstract class AccessDbContext : DbContext
    {
        public AccessDbContext(string connName):base(connName)
        { 
        
        }

        /// <summary>
        /// 检查数据库，检查表是否已经初始化。如果是Access 数据库，还会检查数据库文件是否存在，可以在系统中设置DBFilePath 字段。
        /// 如果需要更多的检查，可以重写该方法，但一定请保留 base.CheckDB();这行代码。
        /// </summary>
        /// <returns>检查是否通过</returns>
        protected  override bool CheckDB()
        {
            if (CurrentDataBase.CurrentDBMSType == PWMIS.Common.DBMSType.Access)
            {
                if (DBFilePath != string.Empty)
                {
                    if (!File.Exists(DBFilePath))
                    {
                        //创建数据库文件
                        PWMIS.AccessExtensions.AccessUility.CreateDataBase(DBFilePath);
                    }
                    return CheckAllTableExists();
                }
            }
            else
            {
                //其它类型的数据库，仅检查表是否存在
                return CheckAllTableExists();
            }
            return false;
        }

        /// <summary>
        /// 检查实体类对应的数据表是否在数据库中存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        protected override void CheckTableExists<T>() 
        {
            //创建表
            if (CurrentDataBase.CurrentDBMSType == PWMIS.Common.DBMSType.Access)
            {
                var entity = new T();
                Access access = (Access)CurrentDataBase;
                var dsScheme = access.GetSchema("Tables", new string[] { null, null, null, "TABLE" });
                var rows = dsScheme.Select("table_name='" + entity.GetTableName() + "'");
                if (rows.Length == 0)
                {
                    PWMIS.AccessExtensions.AccessUility.CreateTable(access, entity);
                }
            }
            else
                throw new Exception("当前数据库类型不是Access ");
        }
    }
}
