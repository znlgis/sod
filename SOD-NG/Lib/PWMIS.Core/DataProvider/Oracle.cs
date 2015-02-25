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
 * 修改者：         时间：2012-11-6                
 * 修改说明：补充Oracle参数名前缀
 * ========================================================================
*/

using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using PWMIS.Common;

namespace PWMIS.DataProvider.Data
{
    /// <summary>
    ///     OracleServer 数据处理
    /// </summary>
    public sealed class Oracle : AdoHelper
    {
        /// <summary>
        ///     获取当前数据库类型的枚举
        /// </summary>
        public override DBMSType CurrentDBMSType
        {
            get { return DBMSType.Oracle; }
        }

        public override DbConnectionStringBuilder ConnectionStringBuilder
        {
            get { return new OracleConnectionStringBuilder(ConnectionString); }
        }

        public override string ConnectionUserID
        {
            get { return ((OracleConnectionStringBuilder) ConnectionStringBuilder).UserID; }
        }

        /// <summary>
        ///     获取Oracle数据库参数前缀资费
        ///     <remarks>由网友路人甲.aspx 补充</remarks>
        /// </summary>
        public override string GetParameterChar
        {
            get { return ":"; }
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
                conn = new OracleConnection(ConnectionString);
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
            IDbDataAdapter ada = new OracleDataAdapter((OracleCommand) command);
            return ada;
        }

        /// <summary>
        ///     获取一个新参数对象
        /// </summary>
        /// <returns>特定于数据源的参数对象</returns>
        public override IDataParameter GetParameter()
        {
            return new OracleParameter();
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
            var para = new OracleParameter();
            para.ParameterName = paraName;
            para.DbType = dbType;
            para.Size = size;
            return para;
        }

        public override string GetNativeDbTypeName(IDataParameter para)
        {
            return ((OracleParameter) para).OracleType.ToString();
        }

        /// <summary>
        ///     返回此 OracleConnection 的数据源的架构信息。
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="restrictionValues">请求的架构的一组限制值</param>
        /// <returns>数据库架构信息表</returns>
        public override DataTable GetSchema(string collectionName, string[] restrictionValues)
        {
            using (var conn = (OracleConnection) GetConnection())
            {
                conn.Open();
                if (restrictionValues == null && string.IsNullOrEmpty(collectionName))
                    return conn.GetSchema();
                if (restrictionValues == null && !string.IsNullOrEmpty(collectionName))
                    return conn.GetSchema(collectionName);
                return conn.GetSchema(collectionName, restrictionValues);
            }
        }

        /// <summary>
        ///     预处理SQL语句，语句中不能包含"["，"]"左右中括号，如果需要，请使用参数化查询。
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns></returns>
        protected override string PrepareSQL(ref string SQL)
        {
            return SQL.Replace("[", "\"").Replace("]", "\"").Replace("@", ":");
        }
    }
}