/*
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

using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.IO;
using PWMIS.Common;

namespace PWMIS.DataProvider.Data
{
    /// <summary>
    ///     OleDbServer 数据处理
    /// </summary>
    public class OleDb : AdoHelper
    {
        /// <summary>
        ///     获取当前数据库类型的枚举
        /// </summary>
        public override DBMSType CurrentDBMSType
        {
            get { return DBMSType.UNKNOWN; }
        }

        public override DbConnectionStringBuilder ConnectionStringBuilder
        {
            get { return new OleDbConnectionStringBuilder(ConnectionString); }
        }

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
        ///     获取当前连接字符串中的数据源字符串，如果是|DataDirectory|，将返回数据源文件对应的绝对路径。
        /// </summary>
        public string ConnectionDataSource
        {
            get
            {
                if (ConnectionStringBuilder.ContainsKey("Data Source"))
                {
                    var path = ConnectionStringBuilder["Data Source"].ToString();
                    if (path.StartsWith("|DataDirectory|", StringComparison.OrdinalIgnoreCase))
                    {
                        var obj = AppDomain.CurrentDomain.GetData("DataDirectory");
                        if (obj == null)
                            throw new InvalidOperationException("当前应用程序域未设置 DataDirectory");
                        var dataPath = obj.ToString();
                        var fileName = path.Substring("|DataDirectory|".Length);
                        var dbFilePath = Path.Combine(dataPath, fileName);
                        return dbFilePath;
                    }
                    return path;
                }
                return null;
            }
        }

        /// <summary>
        ///     创建并且打开数据库连接
        /// </summary>
        /// <returns>数据库连接</returns>
        protected override IDbConnection GetConnection()
        {
            var conn = base.GetConnection();
            if (conn == null)
            {
                conn = new OleDbConnection(ConnectionString);
                //conn.Open ();
            }
            return conn;
        }

        /// <summary>
        ///     获取数据适配器实例
        /// </summary>
        /// <returns>数据适配器</returns>
        protected override IDbDataAdapter GetDataAdapter(IDbCommand command)
        {
            IDbDataAdapter ada = new OleDbDataAdapter((OleDbCommand) command);
            return ada;
        }

        /// <summary>
        ///     获取一个新参数对象
        /// </summary>
        /// <returns>特定于数据源的参数对象</returns>
        public override IDataParameter GetParameter()
        {
            return new OleDbParameter();
        }

        public override string GetNativeDbTypeName(IDataParameter para)
        {
            return ((OleDbParameter) para).OleDbType.ToString();
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
            var para = new OleDbParameter();
            para.ParameterName = paraName;
            para.DbType = dbType;
            para.Size = size;
            return para;
        }

        /// <summary>
        ///     返回此 OleDbConnection 的数据源的架构信息。
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="restrictionValues">请求的架构的一组限制值</param>
        /// <returns>数据库架构信息表</returns>
        public override DataTable GetSchema(string collectionName, string[] restrictionValues)
        {
            using (var conn = (OleDbConnection) GetConnection())
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