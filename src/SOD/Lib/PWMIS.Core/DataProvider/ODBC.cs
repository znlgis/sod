﻿/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap
 * ========================================================================
 * 该类的作用
 *
 * 作者：邓太华     时间：2008-10-12
 * 版本：V3.0
 *
 * 修改者：         时间：
 * 修改说明：
 * ========================================================================
 */

using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using PWMIS.Common;

namespace PWMIS.DataProvider.Data
{
    /// <summary>
    ///     ODBC 数据访问类
    /// </summary>
    public sealed class Odbc : AdoHelper
    {
        /// <summary>
        ///     默认构造函数
        /// </summary>
        public Odbc()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        ///     获取当前数据库类型的枚举
        /// </summary>
        public override DBMSType CurrentDBMSType => DBMSType.UNKNOWN;

        public override DbConnectionStringBuilder ConnectionStringBuilder =>
            new OdbcConnectionStringBuilder(ConnectionString);

        public override string ConnectionUserID
        {
            get
            {
                if (ConnectionStringBuilder.ContainsKey("User ID"))
                    return ConnectionStringBuilder["User ID"].ToString();
                return "";
            }
        }

        /// <summary>
        ///     创建并且打开数据库连接
        /// </summary>
        /// <returns>数据库连接</returns>
        protected override IDbConnection GetConnection()
        {
            var conn = base.GetConnection();
            if (conn == null) conn = new OdbcConnection(ConnectionString);
            //conn.Open ();
            return conn;
        }

        /// <summary>
        ///     获取数据适配器实例
        /// </summary>
        /// <returns>数据适配器</returns>
        protected override IDbDataAdapter GetDataAdapter(IDbCommand command)
        {
            IDbDataAdapter ada = new OdbcDataAdapter((OdbcCommand)command);
            return ada;
        }

        /// <summary>
        ///     获取一个新参数对象
        /// </summary>
        /// <returns>特定于数据源的参数对象</returns>
        public override IDataParameter GetParameter()
        {
            return new OdbcParameter();
        }

        /// <summary>
        ///     获取一个新参数对象
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="dbType">参数数据类型</param>
        /// <param name="size">参数大小</param>
        /// <returns>特定于数据源的参数对象</returns>
        public override IDataParameter GetParameter(string paraName, DbType dbType, int size)
        {
            var para = new OdbcParameter();
            para.ParameterName = paraName;
            para.DbType = dbType;
            para.Size = size;
            return para;
        }

        public override string GetNativeDbTypeName(IDataParameter para)
        {
            return para.DbType.ToString();
        }

        protected override string PrepareSQL(string sql, IDataParameter[] parameters = null)
        {
            sql = sql.Replace("[", "").Replace("]", "");
            if (parameters != null)
            {
                //必须倒序循环替换，感谢网友 北京-cool18 发现此问题
                var pcount = parameters.Length;
                for (var i = pcount - 1; i >= 0; i--)
                {
                    var paraName = parameters[i].ParameterName;
                    if (!paraName.StartsWith("@"))
                        paraName = "@" + paraName;
                    sql = sql.Replace(paraName, "?");
                }

                return sql;
            }

            return sql;
        }

        /// <summary>
        ///     返回此 OdbcConnection 的数据源的架构信息。
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="restrictionValues">请求的架构的一组限制值</param>
        /// <returns>数据库架构信息表</returns>
        public override DataTable GetSchema(string collectionName, string[] restrictionValues)
        {
            using (var conn = (OdbcConnection)GetConnection())
            {
                conn.Open();
                if (restrictionValues == null && string.IsNullOrEmpty(collectionName))
                    return conn.GetSchema();
                if (restrictionValues == null && !string.IsNullOrEmpty(collectionName))
                    return conn.GetSchema(collectionName);
                return conn.GetSchema(collectionName, restrictionValues);
            }
        }
    }
}