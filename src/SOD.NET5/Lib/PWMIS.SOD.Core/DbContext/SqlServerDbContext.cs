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

using System;
using System.Data.SqlClient;
using PWMIS.Common;
using PWMIS.Core.Interface;
using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Data;

namespace PWMIS.Core.Extensions
{
    /// <summary>
    ///     SqlServer 数据上下文基类，可以自动创建实体类对应的表
    /// </summary>
    public sealed class SqlServerDbContext : IDbContextProvider
    {
        /// <summary>
        ///     用连接字符串名字初始化本类
        /// </summary>
        /// <param name="db">数据访问对象</param>
        public SqlServerDbContext(AdoHelper db)
        {
            if (db.CurrentDBMSType != DBMSType.SqlServer)
                throw new Exception("当前数据库类型不是SqlServer ");
            CurrentDataBase = db;
        }

        public AdoHelper CurrentDataBase { get; }

        /// <summary>
        ///     检查实体类对应的数据表是否在数据库中存在
        /// </summary>
        public bool CheckTableExists<T>(T entity = null) where T : EntityBase, new()
        {
            //创建表
            if (entity == null) entity = new T();
            var dsScheme = CurrentDataBase.GetSchema("Tables", new[] { null, null, null, "BASE TABLE" });
            var rows = dsScheme.Select("table_name='" + entity.GetTableName() + "'");
            if (rows.Length == 0)
            {
                var ecmd = new EntityCommand(entity, CurrentDataBase);
                var sql = ecmd.CreateTableCommand;
                CurrentDataBase.ExecuteNonQuery(sql);
                return false;
            }

            return true;
        }

        /// <summary>
        ///     检查实体类对应的表是否存在，如果不存在则创建表并执行可选的SQL语句，比如为表增加索引等。
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="entity">对应的实体类，可选</param>
        /// <param name="initSql">要初始化执行的SQL语句</param>
        public void InitializeTable<T>(string initSql, T entity = null) where T : EntityBase, new()
        {
            if (!CheckTableExists(entity)) CurrentDataBase.ExecuteNonQuery(initSql);
        }


        public bool CheckDB()
        {
            var connBuilder = CurrentDataBase.ConnectionStringBuilder as SqlConnectionStringBuilder;
            var database = connBuilder.InitialCatalog;
            if (!string.IsNullOrEmpty(database))
            {
                var sqlformat = @"
if not exists (select * From master.dbo.sysdatabases where name=N'{0}')
create database [{1}]
";
                var sql = string.Format(sqlformat, database, database);
                //移除初始化的数据库名称，否则下面的执行打不开数据库
                connBuilder.InitialCatalog = "";
                AdoHelper db = new SqlServer();
                db.ConnectionString = connBuilder.ConnectionString;
                db.ExecuteNonQuery(sql);
            }

            return true;
        }
    }
}