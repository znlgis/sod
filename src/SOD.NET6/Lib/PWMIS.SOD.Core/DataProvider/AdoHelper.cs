/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap 
 * ========================================================================
 * 该类的作用
 * 
 * 作者：深蓝医生     时间：2008-10-12
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
 * 
 * 修改者：         时间：2016-5-7                
 * 修改说明：新增  ExecuteMapper 方法，以增强对“微型ORM”的支持。
 * 
 * ========================================================================
*/
using System;
using System.Data;
using System.Collections.Generic;
using PWMIS.Core;
using PWMIS.DataProvider.Adapter;
using PWMIS.Common;
using System.Data.Common;
using System.Threading.Tasks;

namespace PWMIS.DataProvider.Data
{
    /// <summary>
    /// 初始化通用数据访问对象，如果配置了日志记录，将同时初始化命令执行日志处理器到【命令处理管道】中。
    /// </summary>
	public abstract class AdoHelper:CommonDB
	{
        //public delegate TResult Func<T,TResult>(T arg);

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
        /// 根据查询，获取对象列表（已经过时）
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
        [Obsolete("该方法已经过时，请使用 ExecuteMapper方法")]
        public IList<TResult> GetList<TResult>(MyFunc<IDataReader, TResult> fun, string sqlFormat, params object[] parameters) where TResult : class
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
        /// 根据查询语句和参数，执行数据读取映射器，以便将结果映射到一个列表，支持匿名类型
        /// <example>
        /// <code>
        /// <![CDATA[
        ///   AdoHelper dbLocal = new SqlServer();
        ///   dbLocal.ConnectionString = "Data Source=.;Initial Catalog=LocalDB;Integrated Security=True";
        ///   var dataList = dbLocal.ExecuteMapper("SELECT UID,Name FROM Table_User WHERE Sex={0} And Height>={0:5.2}", 1, 1.60)
        ///                          .MapToList(reader => new
        ///                          {
        ///                              UID = reader.GetInt32(0),
        ///                              Name = reader.GetString(1)
        ///                          });
        /// ]]>
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="sqlFormat">带格式化占位符的SQL语句</param>
        /// <param name="parameters">SQL语句中的参数</param>
        /// <returns>数据阅读器</returns>
        public DataReaderMapper ExecuteMapper(string sqlFormat, params object[] parameters) 
        {
            IDataReader reader = FormatExecuteDataReader(sqlFormat, parameters);
            return new DataReaderMapper(reader);
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
                        //修改成从POCO实体类的属性上来获取DataReader类型化数据访问的方法，而不是之前的DataReader 的字段的类型
                        if (!readerDelegates.TryGetValue(accessors[i].MemberType, out getDataMethods[i]))
                        {
                            getDataMethods[i] = (rd, ii) => rd.GetValue(ii);
                        }
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


        private static Dictionary<Type, MyFunc<IDataReader, int, object>> dictReaderDelegate = null;
        private static Dictionary<Type, MyFunc<IDataReader, int, object>> DataReaderDelegate()
        {
            if (dictReaderDelegate == null)
            {
                Dictionary<Type, MyFunc<IDataReader, int, object>> dictReader = new Dictionary<Type, MyFunc<IDataReader, int, object>>();
                dictReader.Add(typeof(int), (reader, i) => reader.GetInt32(i));
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

                dictReaderDelegate = dictReader;
            }
            return dictReaderDelegate;
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

        //异步方法

        /// <summary>
        /// 执行不返回值的查询，如果此查询出现了错误并且设置 OnErrorThrow 属性为 是，将抛出错误；否则将返回 -1，此时请检查ErrorMessage属性；
        /// 如果此查询在事务中并且出现了错误，将根据 OnErrorRollback 属性设置是否自动回滚事务。
        /// </summary>
        /// <param name="SQL">SQL</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>受影响的行数</returns>
        public async Task<int> ExecuteNonQueryAsync(string SQL, CommandType commandType, IDataParameter[] parameters)
        {
            if (!OnCommandExecuting(ref SQL, commandType, parameters, CommandExecuteType.ExecuteNonQuery))
                return -1;

            ErrorMessage = "";
            DbConnection conn = (DbConnection)GetConnection();
            if (conn.State != ConnectionState.Open) //连接已经打开，不能切换连接字符串，感谢网友 “长的没礼貌”发现此Bug 
                conn.ConnectionString = this.DataWriteConnectionString;
            DbCommand cmd = conn.CreateCommand();
            await CompleteCommandAsync(cmd, SQL, commandType, parameters);

            int result = -1;
            try
            {
                result =await cmd.ExecuteNonQueryAsync();
                //如果开启事务，则由上层调用者决定何时提交事务
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                bool inTransaction = cmd.Transaction == null ? false : true;

                //如果开启事务，那么此处应该回退事务
                if (cmd.Transaction != null && OnErrorRollback)
                    cmd.Transaction.Rollback();

                OnCommandExecuteError(cmd, ErrorMessage, CommandExecuteType.ExecuteNonQuery);
                if (OnErrorThrow)
                {
                    throw new QueryException(ErrorMessage, cmd.CommandText, commandType, parameters, inTransaction, conn.ConnectionString, ex);
                }
            }
            finally
            {
                OnCommandExected(cmd, result, CommandExecuteType.ExecuteNonQuery);
                CloseConnection(conn, cmd);
            }
            return result;
        }

        /// <summary>
        /// 完善命令对象,处理命令对象关联的事务和连接，如果未打开连接这里将打开它
        /// 注意：为提高效率，不再继续内部进行参数克隆处理，请多条SQL语句不要使用同名的参数对象
        /// </summary>
        /// <param name="cmd">命令对象</param>
        /// <param name="SQL">SQL</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">参数数组</param>
        protected async Task CompleteCommandAsync(DbCommand cmd, string SQL, CommandType commandType, IDataParameter[] parameters)
        {
            //SQL 可能在OnExecuting 已经处理，因此PrepareSQL 对于某些Oracle大写的字段名，不会有影响
            cmd.CommandText = SqlServerCompatible ? PrepareSQL(SQL, parameters) : SQL;
            cmd.CommandType = commandType;
            cmd.Transaction = (DbTransaction)this.Transaction;
            if (this.CommandTimeOut > 0)
                cmd.CommandTimeout = this.CommandTimeOut;

            if (parameters != null)
                for (int i = 0; i < parameters.Length; i++)
                    if (parameters[i] != null)
                    {
                        if (commandType != CommandType.StoredProcedure)
                        {
                            //IDataParameter para = (IDataParameter)((ICloneable)parameters[i]).Clone();
                            IDataParameter para = parameters[i];
                            if (para.Value == null)
                                para.Value = DBNull.Value;
                            cmd.Parameters.Add(para);
                        }
                        else
                        {
                            //为存储过程带回返回值
                            cmd.Parameters.Add(parameters[i]);
                        }
                    }

            if (cmd.Connection.State != ConnectionState.Open)
               await cmd.Connection.OpenAsync();
            //增加日志处理
            //dth,2008.4.8
            //
            //			if(SaveCommandLog )
            //				RecordCommandLog(cmd);
            //CommandLog.Instance.WriteLog(cmd,"AdoHelper");
        }

        /// <summary>
        /// 根据查询返回数据阅读器对象
        /// </summary>
        /// <param name="SQL">SQL</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="cmdBehavior">对查询和返回结果有影响的说明</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>数据阅读器</returns>
        protected async Task<DbDataReader> ExecuteDataReaderAsync(string SQL, CommandType commandType, CommandBehavior cmdBehavior,  IDataParameter[] parameters)
        {
            if (!OnCommandExecuting(ref SQL, commandType, parameters))
                return null;
            DbConnection conn = (DbConnection)GetConnection();
            DbCommand cmd = conn.CreateCommand();
            await CompleteCommandAsync(cmd, SQL, commandType, parameters);

            DbDataReader reader = null;
            try
            {
                //如果命令对象的事务对象为空，那么强制在读取完数据后关闭阅读器的数据库连接 2008.3.20
                if (cmd.Transaction == null && cmdBehavior == CommandBehavior.Default)
                    cmdBehavior = CommandBehavior.CloseConnection;
                reader =await cmd.ExecuteReaderAsync(cmdBehavior);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                //只有出现了错误而且没有开启事务，可以关闭连结
                //if (cmd.Transaction == null && conn.State == ConnectionState.Open)
                //    conn.Close();
                CloseConnection(conn, cmd);

                bool inTransaction = cmd.Transaction == null ? false : true;
                OnCommandExecuteError(cmd, ErrorMessage);
                if (OnErrorThrow)
                {
                    throw new QueryException(ErrorMessage, cmd.CommandText, commandType, parameters, inTransaction, conn.ConnectionString, ex);
                }
            }
            //必须等到 Reader关闭后才能得到记录行数，这里返回-1
            OnCommandExected(cmd, -1);
            cmd.Parameters.Clear();

            return reader;
        }


    }

    /// <summary>
    /// 数据阅读映射器
    /// </summary>
    public class DataReaderMapper
    {
        private IDataReader dataReader;
        /// <summary>
        /// 以一个数据阅读器初始化本类
        /// </summary>
        /// <param name="reader"></param>
        public DataReaderMapper(IDataReader reader)
        {
            dataReader = reader;
        }

        /// <summary>
        /// 将数据阅读器的结果映射到列表
        /// </summary>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public IList<TResult> MapToList<TResult>(MyFunc<IDataReader, TResult> fun) where TResult : class
        {
            List<TResult> resultList = new List<TResult>();
            using (this.dataReader)
            {
                while (dataReader.Read())
                {
                    TResult t = fun(dataReader);
                    resultList.Add(t);
                }
            }
            return resultList;
        }
    }
}
