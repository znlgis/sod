/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap 
 * ========================================================================
 * 该类的作用 管理SqlServer数据库上下文，比如创建数据库，检查并创建不存在的表，获取EntitQuery对象等
 * 
 * 作者：深蓝医生     时间：2015-9-1
 * 版本：V5.3.6
 * 
 * 修改者：         时间：2015-11-18                
 * 修改说明：修复不能自动根据连接字符串，创建 SqlServer数据库的问题
  * ========================================================================
*/
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
if not exists (select * From master.dbo.sysdatabases where name=N'[{0}]')
create database [{1}]
";
                string sql = string.Format(sqlformat, database, database);
                //移除初始化的数据库名称，否则下面的执行打不开数据库
                connBuilder.InitialCatalog = "";
                CurrentDataBase.ConnectionString = connBuilder.ConnectionString;
                CurrentDataBase.ExecuteNonQuery(sql);
                //恢复连接字符串
                connBuilder.InitialCatalog = database;
                CurrentDataBase.ConnectionString = connBuilder.ConnectionString;
            }
            return true;
        }
    }
}
