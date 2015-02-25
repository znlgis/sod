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
using System.Text;
using System.Data;

namespace PWMIS.DataProvider.Data
{
    /// <summary>
    /// 查询异常类
    /// </summary>
    public  class QueryException:Exception 
    {
        private string _sql;
        /// <summary>
        /// SQL
        /// </summary>
        public string Sql
        {
            get { return _sql; }
            set { _sql = value; }
        }
        private CommandType _cmdType;
        /// <summary>
        /// 命令类型
        /// </summary>
        public CommandType CmdType
        {
            get { return _cmdType; }
            set { _cmdType = value; }
        }
        private IDataParameter[] _parameters;
        /// <summary>
        /// 参数
        /// </summary>
        public IDataParameter[] Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }
        private bool _inTransaction;
        /// <summary>
        /// 是否在事务中
        /// </summary>
        public bool InTransaction
        {
            get { return _inTransaction; }
            set { _inTransaction = value; }
        }
        private string _connectionString;
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        //private string _message;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public QueryException(string message)
            : base(message)
        { }

        /// <summary>
        /// 使用命令信息初始化构造函数
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sql"></param>
        /// <param name="cmdType"></param>
        /// <param name="parameters"></param>
        /// <param name="inTransaction"></param>
        /// <param name="connectionString"></param>
        public QueryException(string message, string sql, CommandType cmdType, IDataParameter[] parameters,bool inTransaction,string connectionString):base(message)
        {
            //_message = message;
            this.Sql = sql;
            this.CmdType = cmdType;
            this.Parameters = parameters;
            this.InTransaction = inTransaction;
            this.ConnectionString = connectionString;
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public override string Message
        {
            get
            {
                string paraString = "";
                if (this.Parameters != null)
                {
                    foreach (IDataParameter p in this.Parameters)
                    {
                        if(p!=null)
                            paraString += "Parameter[\"" + p.ParameterName + "\"]\t=\t\"" + Convert.ToString(p.Value) + "\"  \t\t\t//DbType=" + p.DbType.ToString()+"\r\n";
                    }
                }
                if(!string.IsNullOrEmpty ( this.Sql) )
                    return "PDF.NET AdoHelper 查询错误：\r\nDataBase ErrorMessage:" + base.Message+"\r\n"+string.Format ("SQL:{0}\r\nCommandType:{1}\r\nParameters:\r\n{2}",this.Sql ,this.CmdType ,paraString );// _message;
                else
                    return "PDF.NET AdoHelper 查询错误：\r\nDataBase ErrorMessage:" + base.Message + "\r\n";// _message;
            }
        }
    }
}
