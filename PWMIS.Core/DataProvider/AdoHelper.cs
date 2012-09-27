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
    /// 公共数据访问抽象类 兼容 AdoHelper 类 ,实例使用方法参见 PWMIS.CommonDataProvider.Adapter.MyDB
	/// </summary>
	public abstract class AdoHelper:CommonDB
	{
		/// <summary>
		/// 默认构造函数
		/// </summary>
		public AdoHelper()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

//		/// <summary>
//		/// 获取数据适配器实例
//		/// </summary>
//		/// <returns>数据适配器</returns>
//		protected override IDbDataAdapter  GetDataAdapter(IDbCommand command)
//		{
//			return null;
//		}

//		/// <summary>
//		/// 获取一个新参数对象
//		/// </summary>
//		/// <returns>特定于数据源的参数对象</returns>
//		public override IDataParameter GetParameter()
//		{
//			return null;
//		}

		/// <summary>
		/// 创建公共数据访问类的实例
		/// </summary>
		/// <param name="providerAssembly">提供这程序集名称</param>
		/// <param name="providerType">提供者类型</param>
		/// <returns></returns>
		public static AdoHelper  CreateHelper(string providerAssembly, string providerType)
		{
			return (AdoHelper)CommonDB.CreateInstance(providerAssembly,providerType);
		}

		/// <summary>
		/// 执行不返回值的查询
		/// </summary>
		/// <param name="connectionString">连接字符串</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="SQL">SQL</param>
		/// <returns>受影响的行数</returns>
		public virtual int ExecuteNonQuery(string connectionString,CommandType commandType,string SQL)
		{
			base.ConnectionString=connectionString;
			return base.ExecuteNonQuery(SQL,commandType,null);
		}

		/// <summary>
		/// 执行不返回值的查询
		/// </summary>
		/// <param name="connectionString">连接字符串</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="SQL">SQL</param>
		/// <param name="parameters">参数数组</param>
		/// <returns>受影响的行数</returns>
        public virtual int ExecuteNonQuery(string connectionString, CommandType commandType, string SQL, IDataParameter[] parameters)
		{
			base.ConnectionString=connectionString;
			return base.ExecuteNonQuery(SQL,commandType,parameters);
		}

		/// <summary>
		/// 执行数据阅读器查询
		/// </summary>
		/// <param name="connectionString">连接字符串</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="SQL">SQL</param>
		/// <param name="parameters">参数数组</param>
		/// <returns>数据阅读器</returns>
		public IDataReader ExecuteReader(string connectionString,CommandType commandType,string SQL,IDataParameter[] parameters)
		{
			base.ConnectionString=connectionString;
			return base.ExecuteDataReader(SQL,commandType,parameters);
		}

		/// <summary>
		/// 执行返回数据集的查询
		/// </summary>
		/// <param name="connectionString">连接字符串</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="SQL">SQL</param>
		/// <param name="parameters">参数数组</param>
		/// <returns>数据集</returns>
		public DataSet ExecuteDataSet(string connectionString,CommandType commandType,string SQL,IDataParameter[] parameters)
		{
			base.ConnectionString=connectionString;
			return base.ExecuteDataSet(SQL,commandType,parameters);
		}

        /// <summary>
        /// 执行返回数据集的查询
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="SQL">SQL</param>
        /// <returns>数据集</returns>
        public DataSet ExecuteDataSet(string connectionString, CommandType commandType, string SQL)
        {
            base.ConnectionString = connectionString;
            return base.ExecuteDataSet(SQL, commandType, null);
        }

	
		/// <summary>
		/// 执行返回单一值得查询
		/// </summary>
		/// <param name="connectionString">连接字符串</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="SQL">SQL</param>
		/// <param name="parameters">参数数组</param>
		/// <returns>结果</returns>
		public object ExecuteScalar(string connectionString,CommandType commandType,string SQL,IDataParameter[] parameters)
		{
			base.ConnectionString=connectionString;
			return base.ExecuteScalar (SQL,commandType,parameters);
		}
		

	}
}
