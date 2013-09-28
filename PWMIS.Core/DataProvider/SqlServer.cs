/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap 
 * ========================================================================
 * 该类的作用
 * 
 * 作者：邓太华     时间：2008-10-12
 * 版本：V4.3
 * 
 * 修改者：         时间：2012-4-11                
 * 修改说明：在获取自增数据的时候,使用 SCOPE_IDENTITY 代替默认的方式
 * ========================================================================
*/
using System;
using System.Data ;
using System.Data.SqlClient ;

namespace PWMIS.DataProvider.Data
{
	/// <summary>
	/// SqlServer 数据处理
	/// </summary>
	public sealed class SqlServer:AdoHelper
	{
		/// <summary>
		/// 默认构造函数
		/// </summary>
		public SqlServer()
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
            get { return PWMIS.Common.DBMSType.SqlServer ; }
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
				conn=new SqlConnection (base.ConnectionString );
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
			IDbDataAdapter ada=new SqlDataAdapter ((SqlCommand )command);
			return ada;
		}

		/// <summary>
		/// 获取一个新参数对象
		/// </summary>
		/// <returns>特定于数据源的参数对象</returns>
		public override IDataParameter GetParameter()
		{
			return new SqlParameter ();
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
			SqlParameter para=new SqlParameter();
			para.ParameterName=paraName;
			para.DbType=dbType;
			para.Size=size;
			return para;
		}

        public override string GetNativeDbTypeName(IDataParameter para)
        {
            return ((SqlParameter)para).SqlDbType.ToString();
        }

        /// <summary>
        /// 返回此 SqlConnection 的数据源的架构信息。
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="restrictionValues">请求的架构的一组限制值</param>
        /// <returns>数据库架构信息表</returns>
        public override DataTable GetSchema(string collectionName, string[] restrictionValues)
        {
            using (SqlConnection conn = (SqlConnection)this.GetConnection())
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

        /// <summary>
        /// 获取存储过程的定义内容
        /// </summary>
        /// <param name="spName">存储过程名称</param>
        public override  string GetSPDetail(string spName)
        {
            string value = "";
            DataSet ds = this.ExecuteDataSet("sp_helptext", CommandType.StoredProcedure, 
                new IDataParameter[] { this.GetParameter("@objname", spName) });
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    value += dt.Rows[i][0].ToString();
                }
            }
            else
                value = "nothing";
            return value;
        }

        /// <summary>
        /// 获取视图定义，如果子类支持，需要在子类中重写
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <returns></returns>
        public override  string GetViweDetail(string viewName)
        {
            return GetSPDetail(viewName);
        }
		
		#region 不带事物的查询

//		/// <summary>
//		/// 执行不返回值得查询
//		/// </summary>
//		/// <param name="SQL">SQL</param>
//		/// <returns>受影响的行数</returns>
//		public override int ExecuteNonQuery(string SQL)
//		{
//			SqlConnection conn=(SqlConnection)GetConnection();// new SqlConnection (base.ConnectionString );
//			SqlCommand cmd=new SqlCommand (SQL,conn);
//			
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
//			SqlConnection conn=new SqlConnection (base.ConnectionString );
//			SqlCommand cmd=new SqlCommand (SQL,conn);
//			SqlTransaction trans=null;//=conn.BeginTransaction ();
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
//			SqlConnection conn=new SqlConnection (base.ConnectionString );
//			SqlDataAdapter ada =new SqlDataAdapter (SQL,conn);
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
//			SqlConnection conn=new SqlConnection (base.ConnectionString );
//			SqlCommand cmd=new SqlCommand (SQL,conn);
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

#endregion

        /// <summary>
        /// SQL批量复制
        /// </summary>
        /// <param name="sourceReader">数据源的DataReader</param>
        /// <param name="connectionString">目标数据库的连接字符串</param>
        /// <param name="destinationTableName">要导入的目标表名称</param>
        /// <param name="batchSize">每次批量处理的大小</param>
        public static void BulkCopy(IDataReader sourceReader,string connectionString, string destinationTableName,int batchSize)
        {
            // 目的 
            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                // 打开连接 
                destinationConnection.Open();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection))
                {
                    bulkCopy.BatchSize = batchSize;

                    bulkCopy.DestinationTableName = destinationTableName;
                    bulkCopy.WriteToServer(sourceReader);
                }
            }
            sourceReader.Close();
        }

        /// <summary>
        /// SQL批量复制
        /// </summary>
        /// <param name="sourceTable">数据源表</param>
        /// <param name="connectionString">目标数据库的连接字符串</param>
        /// <param name="destinationTableName">要导入的目标表名称</param>
        /// <param name="batchSize">每次批量处理的大小</param>
        public static void BulkCopy(DataTable sourceTable, string connectionString, string destinationTableName, int batchSize)
        {
            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                // 打开连接 
                destinationConnection.Open();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection))
                {
                    
                    bulkCopy.BatchSize = batchSize;
                    
                    bulkCopy.DestinationTableName = destinationTableName;
                    bulkCopy.WriteToServer(sourceTable);
                }
            }
        }


        /// <summary>
        /// SqlServer 执行插入数据的查询，如果执行成功，受影响的行数只会返回1
        /// </summary>
        /// <param name="SQL">插入数据的SQL</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">参数数组</param>
        /// <param name="ID">要传出的本次操作的新插入数据行的主键ID值</param>
        /// <returns>本次查询受影响的行数</returns>
        public  override int ExecuteInsertQuery(string SQL, CommandType commandType, IDataParameter[] parameters, ref object ID)
        {
            IDbConnection conn = GetConnection();
            IDbCommand cmd = conn.CreateCommand();
            CompleteCommand(cmd, ref SQL, ref commandType, ref parameters);

            CommandLog cmdLog = new CommandLog(true);

            bool inner = false;
            int result = -1;
            ID = 0;
            try
            {
                if (cmd.Transaction == null)
                {
                    inner = true;
                    cmd.Transaction = conn.BeginTransaction();
                }
                cmd.CommandText = SQL + " ;SELECT SCOPE_IDENTITY();";
               
                ID = cmd.ExecuteScalar();
                //如果在内部开启了事务则提交事务，否则外部调用者决定何时提交事务
                result = 1;

                if (inner)
                {
                    cmd.Transaction.Commit();
                    cmd.Transaction = null;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                bool inTransaction = cmd.Transaction == null ? false : true;
                if (cmd.Transaction != null)
                    cmd.Transaction.Rollback();
                if (inner)
                    cmd.Transaction = null;

                cmdLog.WriteErrLog(cmd, "AdoHelper:" + ErrorMessage);
                if (OnErrorThrow)
                {
                    throw new QueryException(ex.Message, cmd.CommandText, commandType, parameters, inTransaction, conn.ConnectionString);
                }

            }
            finally
            {
                if (cmd.Transaction == null && conn.State == ConnectionState.Open)
                    conn.Close();
            }

            long _elapsedMilliseconds;
            cmdLog.WriteLog(cmd, "AdoHelper", out _elapsedMilliseconds);
            base.ElapsedMilliseconds = _elapsedMilliseconds;
            return result;
        }

        public override System.Data.Common.DbConnectionStringBuilder ConnectionStringBuilder
        {
            get { return new SqlConnectionStringBuilder(this.ConnectionString); }
        }

        public override string ConnectionUserID
        {
            get { return ((SqlConnectionStringBuilder)ConnectionStringBuilder).UserID; }
        }
	}
}
