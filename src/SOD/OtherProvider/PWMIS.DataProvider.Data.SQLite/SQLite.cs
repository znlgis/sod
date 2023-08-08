/*
 * ========================================================================
 * Copyright(c) 2006-2012 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap
 * ========================================================================
 * 该类是一个Windows下的SQLite的PDF.NET驱动程序类，
 * 注意需要区分引用的SQLite版本和所在的操作系统版本（32位或者64位）。
 *
 * 作者：深蓝医生    时间：2012-11-1
 * 版本：V4.5
 *
 * 修改者：         时间：
 * 修改说明：
 * ========================================================================
 */

using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using PWMIS.Common;

namespace PWMIS.DataProvider.Data
{
    /// <summary>
    ///     SQLite 数据访问类 dth,2012.11.1
    /// </summary>
    public sealed class SQLite : AdoHelper
    {
        /// <summary>
        ///     默认构造函数
        /// </summary>
        public SQLite()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        //public override int ExecuteNonQuery(string SQL, CommandType commandType, IDataParameter[] parameters)
        //{
        //    ErrorMessage = "";
        //    IDbConnection conn = GetConnection();
        //    IDbCommand cmd = conn.CreateCommand();
        //    //CompleteCommand(cmd, ref SQL, ref commandType, ref parameters);
        //    cmd.CommandText = SqlServerCompatible ? PrepareSQL(ref  SQL) : SQL;
        //    cmd.CommandType = commandType;
        //    cmd.Transaction = this.Transaction;
        //    if (this.CommandTimeOut > 0)
        //        cmd.CommandTimeout = this.CommandTimeOut;

        //    if (parameters != null)
        //        for (int i = 0; i < parameters.Length; i++)
        //            if (parameters[i] != null)
        //            {
        //                IDataParameter para = (IDataParameter)((ICloneable)parameters[i]).Clone();
        //                //SQLite参数化查询，即可解决坑爹的“日期类型”字段问题。
        //                //采用拼接字符串的方式，需要将日期值 ToString(s)
        //                if (para.Value == null)
        //                {
        //                    //if (para.DbType == DbType.DateTime)
        //                    //    para.Value = new DateTime(1900, 1, 1).ToUniversalTime();
        //                    //else
        //                    para.Value = DBNull.Value;
        //                }
        //                else
        //                    //{
        //                    //    if (para.Value.GetType() == typeof(DateTime))
        //                    //    {
        //                    //        para.Value = ((DateTime)para.Value).ToUniversalTime();
        //                    //    }
        //                    //}
        //                    cmd.Parameters.Add(para);
        //            }

        //    if (cmd.Connection.State != ConnectionState.Open)
        //        cmd.Connection.Open();

        //    CommandLog cmdLog = new CommandLog(true);

        //    int result = -1;
        //    try
        //    {
        //        result = cmd.ExecuteNonQuery();
        //        //如果开启事务，则由上层调用者决定何时提交事务
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage = ex.Message;
        //        bool inTransaction = cmd.Transaction == null ? false : true;

        //        //如果开启事务，那么此处应该回退事务
        //        if (cmd.Transaction != null && OnErrorRollback)
        //            cmd.Transaction.Rollback();

        //        cmdLog.WriteErrLog(cmd, "AdoHelper:" + ErrorMessage);
        //        if (OnErrorThrow)
        //        {
        //            throw new QueryException(ex.Message, cmd.CommandText, commandType, parameters, inTransaction, conn.ConnectionString);
        //        }
        //    }
        //    finally
        //    {
        //        //if (cmd.Transaction == null && conn.State == ConnectionState.Open)
        //        //    conn.Close();
        //        CloseConnection(conn, cmd);
        //    }

        //    cmdLog.WriteLog(cmd, "AdoHelper", out base._elapsedMilliseconds);

        //    return result;
        //}

        public override DbConnectionStringBuilder ConnectionStringBuilder => throw new NotImplementedException();

        public override string ConnectionUserID => throw new NotImplementedException();

        public override DBMSType CurrentDBMSType => DBMSType.SQLite;

        public override string GetParameterChar => base.GetParameterChar;

        public override string InsertKey
        {
            get
            {
                base.InsertKey = "select last_insert_rowid();";
                return base.InsertKey;
            }
            set => base.InsertKey = value;
        }

        /// <summary>
        ///     创建并且打开数据库连接
        /// </summary>
        /// <returns>数据库连接</returns>
        protected override IDbConnection GetConnection()
        {
            var conn = base.GetConnection();
            if (conn == null) conn = new SQLiteConnection(ConnectionString);
            //conn.Open ();
            return conn;
        }

        /// <summary>
        ///     获取数据适配器实例
        /// </summary>
        /// <returns>数据适配器</returns>
        protected override IDbDataAdapter GetDataAdapter(IDbCommand command)
        {
            IDbDataAdapter ada = new SQLiteDataAdapter((SQLiteCommand)command);
            // ((SQLiteCommand )command).Connection
            return ada;
        }

        /// <summary>
        ///     获取一个新参数对象
        /// </summary>
        /// <returns>特定于数据源的参数对象</returns>
        public override IDataParameter GetParameter()
        {
            return new SQLiteParameter();
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
            var para = new SQLiteParameter();
            para.ParameterName = paraName;
            para.DbType = dbType;
            para.Size = size;
            return para;
        }

        /// <summary>
        ///     根据参数名和值返回参数一个新的参数对象
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="Value">参数值</param>
        /// <returns>特定于数据源的参数对象</returns>
        public override IDataParameter GetParameter(string paraName, object Value)
        {
            //SQLite 不会根据值自动设置数据库类型，有些类型需要设置下
            var para = GetParameter();
            para.ParameterName = paraName;
            para.Value = Value;
            if (Value != null)
                if (Value.GetType() == typeof(DateTime))
                    para.DbType = DbType.DateTime;
            return para;
        }

        /// <summary>
        ///     获取本地数据库类型名
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public override string GetNativeDbTypeName(IDataParameter para)
        {
            var mysqlPara = para as SQLiteParameter;
            var dbType = mysqlPara.DbType;
            if (dbType == DbType.Int32)
                return "INTEGER";
            return dbType.ToString();
        }

        /// <summary>
        ///     更新数据（为SQLite重写的支持多线程并发写入功能）
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandType"></param>
        /// <param name="SQL"></param>
        /// <returns></returns>
        public override int ExecuteNonQuery(string connectionString, CommandType commandType, string SQL)
        {
            //根据connectionString 缓存每一个写入锁
            return base.ExecuteNonQuery(connectionString, commandType, SQL);
        }

        /// <summary>
        ///     更新数据（为SQLite重写的支持多线程并发写入功能）
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandType"></param>
        /// <param name="SQL"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override int ExecuteNonQuery(string connectionString, CommandType commandType, string SQL,
            IDataParameter[] parameters)
        {
            return ExecuteNonQuery(connectionString, commandType, SQL, parameters);
        }
    }
}