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
        /// 执行查询,并以指定的(具有数据架构的)数据集来填充数据
        /// </summary>
        /// <param name="SQL">查询语句</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">查询参数</param>
        /// <param name="schemaDataSet">指定的(具有数据架构的)数据集</param>
        /// <returns>具有数据的数据集</returns>
        public override DataSet ExecuteDataSetWithSchema(string SQL, CommandType commandType, IDataParameter[] parameters, DataSet schemaDataSet)
        {
            IDbConnection conn = GetConnection();
            IDbCommand cmd = conn.CreateCommand();
            CompleteCommand(cmd,  SQL,  commandType,  parameters);
            SqlDataAdapter ada = new SqlDataAdapter((SqlCommand)cmd);

            CommandLog cmdLog = new CommandLog(true);

            try
            {
                if (schemaDataSet.Tables.Count > 0)
                    ada.Fill(schemaDataSet, schemaDataSet.Tables[0].TableName);
                else
                    ada.Fill(schemaDataSet);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                bool inTransaction = cmd.Transaction == null ? false : true;
                cmdLog.WriteErrLog(cmd, "AdoHelper:" + ErrorMessage);
                if (OnErrorThrow)
                {
                    throw new QueryException(ex.Message, cmd.CommandText, commandType, parameters, inTransaction, conn.ConnectionString);
                }
            }
            finally
            {
                cmdLog.WriteLog(cmd, "AdoHelper", out _elapsedMilliseconds);
                CloseConnection(conn, cmd);
            }
            return schemaDataSet;
        }

        /// <summary>
        /// 执行强类型的数据集查询
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">查询参数</param>
        /// <param name="schemaDataSet">强类型的数据集</param>
        /// <param name="tableName">要填充的表名称</param>
        /// <returns></returns>
        public DataSet ExecuteTypedDataSet(string SQL, CommandType commandType, IDataParameter[] parameters, DataSet schemaDataSet, string tableName)
        {
            bool flag = false;
            for (int i = 0; i < schemaDataSet.Tables.Count; i++)
            {
                if (schemaDataSet.Tables[i].TableName == tableName)
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
                throw new ArgumentException("在强类型的数据集中，没有找到制定的数据表明称！");

            IDbConnection conn = GetConnection();
            IDbCommand cmd = conn.CreateCommand();
            CompleteCommand(cmd,  SQL,  commandType,  parameters);
            SqlDataAdapter ada = new SqlDataAdapter((SqlCommand)cmd);

            CommandLog cmdLog = new CommandLog(true);

            try
            {
                ada.Fill(schemaDataSet, tableName);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                bool inTransaction = cmd.Transaction == null ? false : true;
                cmdLog.WriteErrLog(cmd, "AdoHelper:" + ErrorMessage);
                if (OnErrorThrow)
                {
                    throw new QueryException(ex.Message, cmd.CommandText, commandType, parameters, inTransaction, conn.ConnectionString);
                }
            }
            finally
            {
                cmdLog.WriteLog(cmd, "AdoHelper", out _elapsedMilliseconds);
                CloseConnection(conn, cmd);
            }
            return schemaDataSet;
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
        public  override int ExecuteInsertQuery(string SQL, CommandType commandType, IDataParameter[] parameters, ref object ID,string insertKey="")
        {
            IDbConnection conn = GetConnection();
            IDbCommand cmd = conn.CreateCommand();
            CompleteCommand(cmd,  SQL,  commandType,  parameters);

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
