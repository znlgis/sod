/*
 * ========================================================================
 * Copyright(c) 2006-2012 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap 
 * ========================================================================
 * 该类是一个Linux下的SQLite的PDF.NET驱动程序类，不可以在Windows系统下使用。
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
using Mono.Data.Sqlite;
using PWMIS.DataProvider.Data;

namespace PWMIS.DataProvider.Data
{
    /// <summary>
    /// Sqlite 数据访问类 dth,2009.4.1
    /// </summary>
    public sealed class Sqlite:AdoHelper   
    {
    /// <summary>
		/// 默认构造函数
		/// </summary>
		public Sqlite()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		/// <summary>
		/// 创建并且打开数据库连接
		/// </summary>
		/// <returns>数据库连接</returns>
		protected override IDbConnection GetConnection()
		{
			IDbConnection conn=base.GetConnection ();
			if(conn==null)
			{
				conn=new SqliteConnection (base.ConnectionString );
				//conn.Open ();
			}
			return conn;
		}

		/// <summary>
		/// 获取数据适配器实例
		/// </summary>
		/// <returns>数据适配器</returns>
		protected override IDbDataAdapter  GetDataAdapter(IDbCommand command)
		{
			IDbDataAdapter ada=new SqliteDataAdapter ((SqliteCommand )command);
			return ada;
		}

		/// <summary>
		/// 获取一个新参数对象
		/// </summary>
		/// <returns>特定于数据源的参数对象</returns>
		public override IDataParameter GetParameter()
		{
			return new SqliteParameter ();
		}

		/// <summary>
		///  获取一个新参数对象
		/// </summary>
		/// <param name="paraName">参数名</param>
		/// <param name="dbType">参数数据类型</param>
		/// <param name="size">参数大小</param>
		/// <returns>特定于数据源的参数对象</returns>
		public override IDataParameter GetParameter(string paraName,System.Data.DbType dbType,int size)
		{
			SqliteParameter para=new SqliteParameter();
			para.ParameterName=paraName;
			para.DbType=dbType;
			para.Size=size;
			return para;
		}

        /// <summary>
        /// 更新数据（为Sqlite重写的支持多线程并发写入功能）
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
        /// 更新数据（为Sqlite重写的支持多线程并发写入功能）
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandType"></param>
        /// <param name="SQL"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override int ExecuteNonQuery(string connectionString, CommandType commandType, string SQL, IDataParameter[] parameters)
        {
            return base.ExecuteNonQuery(connectionString, commandType, SQL, parameters);
        }

        public override System.Data.Common.DbConnectionStringBuilder ConnectionStringBuilder
        {
            get { throw new NotImplementedException(); }
        }

        public override string ConnectionUserID
        {
            get { throw new NotImplementedException(); }
        }

        public override PWMIS.Common.DBMSType CurrentDBMSType
        {
            get { return PWMIS.Common.DBMSType.SQLite; }
        }

        public override string GetParameterChar
        {
            get
            {
                return base.GetParameterChar;
            }
        }

        public override DataTable GetSchema(string collectionName, string[] restrictionValues)
        {
            throw new NotImplementedException();
        }

        public override string InsertKey
        {
            get
            {
                base.InsertKey = "select last_insert_rowid();";
                return base.InsertKey;
            }
            set
            {
                base.InsertKey = value;
            }
        }
	}
}

