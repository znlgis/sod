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

namespace PWMIS.DataProvider.Data
{
    /// <summary>
    ///     查询异常类
    /// </summary>
    public class QueryException : Exception
    {
        //private string _message;

        /// <summary>
        ///     默认构造函数
        /// </summary>
        public QueryException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     使用命令信息初始化构造函数
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sql"></param>
        /// <param name="cmdType"></param>
        /// <param name="parameters"></param>
        /// <param name="inTransaction"></param>
        /// <param name="connectionString"></param>
        public QueryException(string message, string sql, CommandType cmdType, IDataParameter[] parameters,
            bool inTransaction, string connectionString, Exception originalException) : base(message, originalException)
        {
            //_message = message;
            Sql = sql;
            CmdType = cmdType;
            Parameters = parameters;
            InTransaction = inTransaction;
            ConnectionString = connectionString;
        }

        /// <summary>
        ///     SQL
        /// </summary>
        public string Sql { get; set; }

        /// <summary>
        ///     命令类型
        /// </summary>
        public CommandType CmdType { get; set; }

        /// <summary>
        ///     参数
        /// </summary>
        public IDataParameter[] Parameters { get; set; }

        /// <summary>
        ///     是否在事务中
        /// </summary>
        public bool InTransaction { get; set; }

        /// <summary>
        ///     连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        ///     错误信息
        /// </summary>
        public override string Message
        {
            get
            {
                var paraString = "";
                if (Parameters != null)
                    foreach (var p in Parameters)
                        if (p != null)
                            paraString += "Parameter[\"" + p.ParameterName + "\"]\t=\t\"" + Convert.ToString(p.Value) +
                                          "\"  \t\t\t//DbType=" + p.DbType + "\r\n";
                if (!string.IsNullOrEmpty(Sql))
                    return "PDF.NET AdoHelper Query Error：\r\nDataBase ErrorMessage:" + base.Message + "\r\n" +
                           string.Format("SQL:{0}\r\nCommandType:{1}\r\nParameters:\r\n{2}", Sql, CmdType,
                               paraString); // _message;
                return "PDF.NET AdoHelper Query Error：\r\nDataBase ErrorMessage:" + base.Message + "\r\n"; // _message;
            }
        }
    }
}