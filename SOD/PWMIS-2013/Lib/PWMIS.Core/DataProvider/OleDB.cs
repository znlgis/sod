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
using System.Data ;
using System.Data.OleDb ;

namespace PWMIS.DataProvider.Data
{
	/// <summary>
	/// OleDbServer 数据处理
	/// </summary>
	public  class OleDb:AdoHelper
	{
		/// <summary>
		/// 默认构造函数
		/// </summary>
		public OleDb()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

        /// <summary>
        /// 获取当前数据库类型的枚举
        /// </summary>
        public override PWMIS.Common.DBMSType CurrentDBMSType
        {
            get { return PWMIS.Common.DBMSType.UNKNOWN ; }
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
				conn=new OleDbConnection (base.ConnectionString );
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
			IDbDataAdapter ada=new OleDbDataAdapter ((OleDbCommand )command);
			return ada;
		}

		/// <summary>
		/// 获取一个新参数对象
		/// </summary>
		/// <returns>特定于数据源的参数对象</returns>
		public override IDataParameter GetParameter()
		{
			return new OleDbParameter ();
		}

        public override string GetNativeDbTypeName(IDataParameter para)
        {
            return ((OleDbParameter)para).OleDbType.ToString();
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
			OleDbParameter para=new OleDbParameter();
			para.ParameterName=paraName;
			para.DbType=dbType;
			para.Size=size;
			return para;
		}

        /// <summary>
        /// 返回此 OleDbConnection 的数据源的架构信息。
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="restrictionValues">请求的架构的一组限制值</param>
        /// <returns>数据库架构信息表</returns>
        public override DataTable GetSchema(string collectionName, string[] restrictionValues)
        {
            using (OleDbConnection conn = (OleDbConnection)this.GetConnection())
            {
                conn.Open();
                if (restrictionValues == null && string.IsNullOrEmpty(collectionName))
                    return conn.GetSchema();
                else if (restrictionValues == null && !string.IsNullOrEmpty(collectionName))
                    return conn.GetSchema(collectionName);
                else
                    return conn.GetSchema(collectionName, restrictionValues);
            }
            
        }

        public override System.Data.Common.DbConnectionStringBuilder ConnectionStringBuilder
        {
            get { return new OleDbConnectionStringBuilder(this.ConnectionString); }
        }

        public override string ConnectionUserID
        {
            get
            {
                if (ConnectionStringBuilder.ContainsKey("User ID"))
                    return ConnectionStringBuilder["User ID"].ToString();
                else
                    return "";
            }
        }

//		/// <summary>
//		/// 执行不返回值得查询
//		/// </summary>
//		/// <param name="SQL">SQL</param>
//		/// <returns>受影响的行数</returns>
//		public override int ExecuteNonQuery(string SQL)
//		{
//			OleDbConnection conn=new OleDbConnection (base.ConnectionString );
//			OleDbCommand cmd=new OleDbCommand (SQL,conn);
//			conn.Open ();
//			int result=0;
//			try
//			{
//				result=cmd.ExecuteNonQuery ();
//			}
//			catch(Exception ex)
//			{
//				base.ErrorMessage =ex.Message ;
//			}
//			finally
//			{
//				if(conn.State ==ConnectionState.Open )
//					conn.Close ();
//			}
//			return result;
//		}
//
//		/// <summary>
//		/// 执行插入数据的查询
//		/// </summary>
//		/// <param name="SQL">插入数据的SQL</param>
//		/// <param name="ID">要传出的本次操作的新插入数据行的主键ID值</param>
//		/// <returns>本次查询受影响的行数</returns>
//		public override int ExecuteInsertQuery(string SQL,ref int ID)
//		{
//			OleDbConnection conn=new OleDbConnection (base.ConnectionString );
//			OleDbCommand cmd=new OleDbCommand (SQL,conn);
//			OleDbTransaction trans=null;//=conn.BeginTransaction ();
//			conn.Open ();
//			int result=0;
//			ID=0;
//			try
//			{
//				trans=conn.BeginTransaction ();
//				cmd.Transaction =trans;
//				result=cmd.ExecuteNonQuery ();
//				cmd.CommandText ="SELECT @@IDENTITY";
//				//ID=(int)(cmd.ExecuteScalar ());//出错
//				object obj=cmd.ExecuteScalar ();
//				ID=Convert.ToInt32 (obj);
//				trans.Commit ();
//			}
//			catch(Exception ex)
//			{
//				base.ErrorMessage=ex.Message ;
//				if(trans!=null)
//					trans.Rollback ();
//			}
//			finally
//			{
//				if(conn.State ==ConnectionState.Open )
//					conn.Close ();
//			}
//			return result;
//		}
//
//		/// <summary>
//		/// 执行返回数据集的查询
//		/// </summary>
//		/// <param name="SQL">SQL</param>
//		/// <returns>数据集</returns>
//		public override DataSet ExecuteDataSet(string SQL)
//		{
//			OleDbConnection conn=new OleDbConnection (base.ConnectionString );
//			OleDbDataAdapter ada =new OleDbDataAdapter (SQL,conn);
//			DataSet ds=new DataSet ();
//			try
//			{
//				ada.Fill (ds);
//			}
//			catch(Exception ex)
//			{
//				base.ErrorMessage=ex.Message ;
//			}
//			finally
//			{
//				if(conn.State ==ConnectionState.Open )
//					conn.Close ();
//			}
//			return ds;
//		}
//
//		/// <summary>
//		/// 返回单一行的数据阅读器
//		/// </summary>
//		/// <param name="SQL">SQL</param>
//		/// <returns>数据阅读器</returns>
//		public override IDataReader ExecuteDataReaderWithSingleRow(string SQL)
//		{
//			OleDbConnection conn=new OleDbConnection (base.ConnectionString );
//			OleDbCommand cmd=new OleDbCommand (SQL,conn);
//			IDataReader reader=null;
//			try
//			{
//				conn.Open ();
//				return cmd.ExecuteReader (CommandBehavior.SingleRow | CommandBehavior.CloseConnection );
//			}
//			catch(Exception ex)
//			{
//				base.ErrorMessage=ex.Message ;
//				if(conn.State ==ConnectionState.Open )
//					conn.Close ();
//			}
//			return reader;
//			
//		}

	}
}
