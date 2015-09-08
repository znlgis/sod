/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap 
 * ========================================================================
 * 该类的作用
 * 
 * 作者：邓太华     时间：2008-10-12
 * 版本：V5.2
 * 
 * 修改者：         时间：2013-1-13                
 * 修改说明：支持读写分离，详细见基类说明
 * 
 *  * 修改者：         时间：2013-5-19                
 * 修改说明：支持SQL 格式控制串，支持直接获得结果对象列表。
 * 注意不能将Format开头的几个方法命名成其它被重载的方法，感谢GIV-顺德　发现此问题。
 * 
 * 修改者：         时间：2015-2-15                
 * 修改说明：新增  List<T> QueryList<T>(string sqlFormat, params object[] parameters) 方法，以增强对“微型ORM”的支持。
 * ========================================================================
*/
using System;
using System.Data;
using System.Collections.Generic;
using PWMIS.Core;
using PWMIS.DataProvider.Adapter;
using PWMIS.Common;
namespace PWMIS.DataProvider.Data
{
	/// <summary>
    /// 公共数据访问抽象类 兼容 AdoHelper 类 ,实例使用方法参见 PWMIS.CommonDataProvider.Adapter.MyDB
	/// </summary>
	public abstract class AdoHelper:CommonDB
	{
        public delegate TResult Func<T,TResult>(T arg);

		/// <summary>
		/// 默认构造函数
		/// </summary>
		public AdoHelper()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

        /// <summary>
        /// 根据应用程序配置文件的connectionStrings 配置中的name，创建数据访问对象
        /// </summary>
        /// <param name="connectionName">连接字符串配置项的名字</param>
        /// <returns>数据访问对象</returns>
        public static AdoHelper CreateHelper(string connectionName)
        {
            return MyDB.GetDBHelperByConnectionName(connectionName);
        }


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
			base.DataWriteConnectionString=connectionString;
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

        #region 直接参数化查询扩展
        /// <summary>
        /// 执行不返回结果集的查询。
        /// 对于字符串类型的参数，最好指定参数的长度，格式例如是{0:50}；
        /// 对于Decimal类型，最好指定精度和小数位，例如{0:8.3}，表示精度为8，小数位为3
        /// </summary>
        /// <param name="sqlFormat">带格式的查询命令字符串，例如SELECT * FROM TABLE1 WHERE CLASSID={0} AND CLASSNAME={1:50} PRICE={2:8.3}</param>
        /// <param name="parameters">要替换的参数</param>
        /// <returns>查询受影响的行数</returns>
        public int FormatExecuteNonQuery(string sqlFormat, params object[] parameters)
        {
            DataParameterFormat formater = new DataParameterFormat(this);
            string sql = string.Format(formater,sqlFormat, parameters);
            return base.ExecuteNonQuery(sql,CommandType.Text,formater.DataParameters);
        }
        /// <summary>
        /// 执行返回数据阅读器的查询
        /// 对于字符串类型的参数，最好指定参数的长度，格式例如是{0:50}；
        /// 对于Decimal类型，最好指定精度和小数位，例如{0:8.3}，表示精度为8，小数位为3
        /// </summary>
        /// <param name="sqlFormat">带格式的查询命令字符串，例如SELECT * FROM TABLE1 WHERE CLASSID={0} AND CLASSNAME={1:50} PRICE={2:8.3}</param>
        /// <param name="parameters">要替换的参数</param>
        /// <returns>数据阅读器</returns>
        public IDataReader FormatExecuteDataReader(string sqlFormat, params object[] parameters)
        {
            if (parameters == null)
            {
                return base.ExecuteDataReader(sqlFormat);
            }
            else
            {
                DataParameterFormat formater = new DataParameterFormat(this);
                string sql = string.Format(formater, sqlFormat, parameters);
                return base.ExecuteDataReader(sql, CommandType.Text, formater.DataParameters);
            }
        }
        /// <summary>
        /// 执行返回数据集的查询
        /// 对于字符串类型的参数，最好指定参数的长度，格式例如是{0:50}；
        /// 对于Decimal类型，最好指定精度和小数位，例如{0:8.3}，表示精度为8，小数位为3
        /// </summary>
        /// <param name="sqlFormat">带格式的查询命令字符串，例如SELECT * FROM TABLE1 WHERE CLASSID={0} AND CLASSNAME={1:50} PRICE={2:8.3}</param>
        /// <param name="parameters">要替换的参数</param>
        /// <returns>数据集</returns>
        public DataSet FormatExecuteDataSet(string sqlFormat, params object[] parameters)
        {
            DataParameterFormat formater = new DataParameterFormat(this);
            string sql = string.Format(formater, sqlFormat, parameters);
            return base.ExecuteDataSet(sql, CommandType.Text, formater.DataParameters);
        }
        /// <summary>
        /// 执行返回单值结果的查询
        /// 对于字符串类型的参数，最好指定参数的长度，格式例如是{0:50}；
        /// 对于Decimal类型，最好指定精度和小数位，例如{0:8.3}，表示精度为8，小数位为3
        /// </summary>
        /// <param name="sqlFormat">带格式的查询命令字符串，例如SELECT * FROM TABLE1 WHERE CLASSID={0} AND CLASSNAME={1:50} PRICE={2:8.3}</param>
        /// <param name="parameters">要替换的参数</param>
        /// <returns>返回的单值结果</returns>
        public object FormatExecuteScalar(string sqlFormat, params object[] parameters)
        {
            DataParameterFormat formater = new DataParameterFormat(this);
            string sql = string.Format(formater, sqlFormat, parameters);
            return base.ExecuteScalar(sql, CommandType.Text, formater.DataParameters);
        }
        #endregion

        /// <summary>
        /// 根据查询，获取对象列表
        /// <example>
        /// <code>
        /// <![CDATA[
        /// AdoHelper dbLocal = new SqlServer();
        /// dbLocal.ConnectionString = "Data Source=.;Initial Catalog=LocalDB;Integrated Security=True";
        /// var dataList = dbLocal.GetList(reader =>
        /// {
        ///     return new
        ///    {
        ///        UID=reader.GetInt32(0),
        ///        Name=reader.GetString(1)
        ///    };
        /// }, "SELECT UID,Name FROM Table_User WHERE Sex={0} And Height>={1:5.2}",1, 1.60M);
        /// 
        /// ]]>
        /// </code>
        /// </example>
        /// </summary>
        /// <typeparam name="TResult">返回的对象的类型</typeparam>
        /// <param name="fun">要处理数据阅读器的具体方法，该方法将返回一个对象</param>
        /// <param name="sqlFormat">SQL 格式控制语句</param>
        /// <param name="parameters">用于替换的参数</param>
        /// <returns>对象列表</returns>
        public IList<TResult> GetList<TResult>(Func<IDataReader, TResult> fun, string sqlFormat, params object[] parameters) where TResult : class
        {
            List<TResult> resultList = new List<TResult>();
            using (IDataReader reader = FormatExecuteDataReader(sqlFormat, parameters))
            {
                while (reader.Read())
                {
                    TResult t = fun(reader);
                    resultList.Add(t);
                }
            }
            return resultList;
        }

        /// <summary>
        /// 采用快速的方法，将数据阅读器的结果映射到一个POCO类的列表上
        /// </summary>
        /// <typeparam name="T">POCO类类型</typeparam>
        /// <param name="reader">抽象数据阅读器</param>
        /// <returns>POCO类的列表</returns>
        public static List<T> QueryList<T>(IDataReader reader) where T : class, new()
        {
            List<T> list = new List<T>();
            using (reader)
            {
                if (reader.Read())
                {
                    int fcount = reader.FieldCount;
                    //使用类型化委托读取正确的数据，解决MySQL等数据库可能的问题，感谢网友 @卖女孩的小肥羊 发现此问题
                    Dictionary<Type, MyFunc<IDataReader, int, object>> readerDelegates = DataReaderDelegate();
                    MyFunc<IDataReader, int, object>[] getDataMethods = new MyFunc<IDataReader, int, object>[fcount];

                    INamedMemberAccessor[] accessors = new INamedMemberAccessor[fcount];
                    DelegatedReflectionMemberAccessor accessorMethod = new DelegatedReflectionMemberAccessor();
                    for (int i = 0; i < fcount; i++)
                    {
                        accessors[i] = accessorMethod.FindAccessor<T>(reader.GetName(i));
                        getDataMethods[i] = readerDelegates[reader.GetFieldType(i)];
                    }
                    

                    do
                    {
                        T t = new T();
                        for (int i = 0; i < fcount; i++)
                        {
                            if (!reader.IsDBNull(i))
                            {
                                MyFunc<IDataReader, int, object> read = getDataMethods[i];
                                object value=read(reader,i);
                                accessors[i].SetValue(t, value);
                            }
                                
                        }
                        list.Add(t);
                    } while (reader.Read());
                }
            }
            return list;
        }

        private static Dictionary<Type, MyFunc<IDataReader, int, object>> DataReaderDelegate()
        {
            Dictionary<Type, MyFunc<IDataReader, int, object>> dictReader = new Dictionary<Type, MyFunc<IDataReader, int, object>>();
            dictReader.Add(typeof(int), (reader,i) => reader.GetInt32(i));
            dictReader.Add(typeof(bool), (reader, i) => reader.GetBoolean(i));
            dictReader.Add(typeof(byte), (reader, i) => reader.GetByte(i));
            dictReader.Add(typeof(char), (reader, i) => reader.GetChar(i));
            dictReader.Add(typeof(DateTime), (reader, i) => reader.GetDateTime(i));
            dictReader.Add(typeof(decimal), (reader, i) => reader.GetDecimal(i));
            dictReader.Add(typeof(double), (reader, i) => reader.GetDouble(i));
            dictReader.Add(typeof(float), (reader, i) => reader.GetFloat(i));
            dictReader.Add(typeof(Guid), (reader, i) => reader.GetGuid(i));
            dictReader.Add(typeof(System.Int16), (reader, i) => reader.GetInt16(i));
            dictReader.Add(typeof(System.Int64), (reader, i) => reader.GetInt64(i));
            dictReader.Add(typeof(string), (reader, i) => reader.GetString(i));
            dictReader.Add(typeof(object), (reader, i) => reader.GetValue(i));
            return dictReader;
        }

        /// <summary>
        /// 根据SQL格式化串和可选的参数，直接查询结果并映射到POCO 对象
        /// <example>
        /// <code>
        /// <![CDATA[
        /// //假设UserPoco 对象跟 Table_User 表是映射的相同结构
        /// AdoHelper dbLocal = new SqlServer();
        /// dbLocal.ConnectionString = "Data Source=.;Initial Catalog=LocalDB;Integrated Security=True";
        /// var list=dbLoal.QueryList<UserPoco>("SELECT UID,Name FROM Table_User WHERE Sex={0} And Height>={1:5.2}",1, 1.60M);
        /// ]]>
        /// </code>
        /// </example>
        /// </summary>
        /// <typeparam name="T">POCO 对象类型</typeparam>
        /// <param name="sqlFormat">SQL格式化串</param>
        /// <param name="parameters">可选的参数</param>
        /// <returns>POCO 对象列表</returns>
        public  List<T> QueryList<T>(string sqlFormat, params object[] parameters) where T : class, new()
        {
            IDataReader reader = FormatExecuteDataReader(sqlFormat, parameters);
            return QueryList<T>(reader);
        }
    }
}
