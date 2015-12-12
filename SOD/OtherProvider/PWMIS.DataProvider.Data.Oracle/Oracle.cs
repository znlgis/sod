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
using System;
using System.Data;
using Oracle.DataAccess.Client;


namespace PWMIS.DataProvider.Data.OracleDataAccess
{
    /// <summary>
    /// OracleServer 数据处理
    /// </summary>
    public sealed class Oracle : AdoHelper
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Oracle()
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
            get { return PWMIS.Common.DBMSType.Oracle; }
        }

        /// <summary>
        /// 创建并且打开数据库连接
        /// </summary>
        /// <returns>数据库连接</returns>
        protected override IDbConnection GetConnection()
        {
            IDbConnection conn = base.GetConnection();
            if (conn == null)
            {
                conn = new OracleConnection(base.ConnectionString);
                //conn.Open ();
            }
            return conn;
        }

        /// <summary>
        /// 获取数据适配器实例
        /// </summary>
        /// <returns>数据适配器</returns>
        protected override IDbDataAdapter GetDataAdapter(IDbCommand command)
        {
            IDbDataAdapter ada = new OracleDataAdapter((OracleCommand)command);
            return ada;
        }

        /// <summary>
        /// 获取一个新参数对象
        /// </summary>
        /// <returns>特定于数据源的参数对象</returns>
        public override IDataParameter GetParameter()
        {
            return new OracleParameter();
        }

        /*
         * 
                //Oracle 处理自增
                //string seqName = entity.GetTableName() + "_" + entity.GetIdentityName() + "_SEQ";
                //CurrentDataBase.InsertKey = "select " + seqName + ".currval from dual;";
         */ 
        /// <summary>
        /// Oracle 不支持自增，请自己创建触发器和序列
        /// </summary>
        public override string InsertKey
        {
            get;
            set;
        }

        /// <summary>
        ///  获取一个新参数对象
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="dbType">参数数据类型</param>
        /// <param name="size">参数大小</param>
        /// <returns>特定于数据源的参数对象</returns>
        public override IDataParameter GetParameter(string paraName, System.Data.DbType dbType, int size)
        {
            OracleParameter para = new OracleParameter();
            para.ParameterName = paraName;
            para.DbType = dbType;
            para.Size = size;
            return para;
        }

        public override string GetNativeDbTypeName(IDataParameter para)
        {
            OracleParameter oraPara = (OracleParameter)para;
            OracleDbType oraType = oraPara.OracleDbType;
            if (oraType == OracleDbType.Date)
                return "Date";
            else if (oraType == OracleDbType.Int32)
                return "INT";
            else
                return oraType.ToString();
           
        }

        /// <summary>
        /// 返回此 OracleConnection 的数据源的架构信息。
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="restrictionValues">请求的架构的一组限制值</param>
        /// <returns>数据库架构信息表</returns>
        public override DataTable GetSchema(string collectionName, string[] restrictionValues)
        {
            using (OracleConnection conn = (OracleConnection)this.GetConnection())
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
        /// 预处理SQL语句，语句中不能包含"["，"]"左右中括号，如果需要，请使用参数化查询。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        protected override string PrepareSQL( string sql)
        {
            return sql.Replace("[", "\"").Replace("]", "\"").Replace("@", ":");
        }

        public override System.Data.Common.DbConnectionStringBuilder ConnectionStringBuilder
        {
            get { return new OracleConnectionStringBuilder(this.ConnectionString); }
        }

        public override string ConnectionUserID
        {
            get { return ((OracleConnectionStringBuilder)ConnectionStringBuilder).UserID; }
        }

        /// <summary>
        /// 获取Oracle数据库参数前缀资费
        /// <remarks>由网友路人甲.aspx 补充</remarks>
        /// </summary>
        public override string GetParameterChar
        {
            get
            {
                return ":";
            }
        }


        //		/// <summary>
        //		/// 执行不返回值得查询
        //		/// </summary>
        //		/// <param name="SQL">SQL</param>
        //		/// <returns>受影响的行数</returns>
        //		public override int ExecuteNonQuery(string SQL)
        //		{
        //			OracleConnection conn=new OracleConnection (base.ConnectionString );
        //			OracleCommand cmd=new OracleCommand (SQL,conn);
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
        //			OracleConnection conn=new OracleConnection (base.ConnectionString );
        //			OracleCommand cmd=new OracleCommand (SQL,conn);
        //			OracleTransaction trans=null;//=conn.BeginTransaction ();
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
        //			OracleConnection conn=new OracleConnection (base.ConnectionString );
        //			OracleDataAdapter ada =new OracleDataAdapter (SQL,conn);
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
        //			OracleConnection conn=new OracleConnection (base.ConnectionString );
        //			OracleCommand cmd=new OracleCommand (SQL,conn);
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
