/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap 
 * ========================================================================
 * 该类的作用
 * 
 * 使用下面的方法创建数据访问实例,可以在App.config中作如下配置：
 <add key="SqlServerConnectionString" value="Data Source=localhost;Initial catalog=DAABAF;user id=daab;password=daab" />
       <add key="SqlServerHelperAssembly" value="CommonDataProvider.Data"></add>
       <add key="SqlServerHelperType" value="CommonDataProvider.Data.SqlServer"></add>
       <add key="OleDbConnectionString" value="Provider=SQLOLEDB;Data Source=localhost;Initial catalog=DAABAF;user id=daab;password=daab" />
       <add key="OleDbHelperAssembly" value="CommonDataProvider.Data"></add>
       <add key="OleDbHelperType" value="CommonDataProvider.Data.OleDb"></add>
       <add key="OdbcConnectionString" value="DRIVER={SQL Server};SERVER=localhost;DATABASE=DAABAF;UID=daab;PWD=daab;" />
       <add key="OdbcHelperAssembly" value="CommonDataProvider.Data"></add>
       <add key="OdbcHelperType" value="CommonDataProvider.Data.Odbc"></add>
       <add key="OracleConnectionString" value="User ID=DAAB;Password=DAAB;Data Source=spinvis_flash;" />
       <add key="OracleHelperAssembly" value="CommonDataProvider.Data"></add>
       <add key="OracleHelperType" value="CommonDataProvider.Data.Oracle"></add>
        * 
       <add key="SQLiteConnectionString" value="Data Source=spinvis_flash;" />
       <add key="SQLiteHelperAssembly" value="CommonDataProvider.Data"></add>
       <add key="SQLiteHelperType" value="CommonDataProvider.Data.SQLite"></add>
 * 
 * 作者：邓太华     时间：2008-10-12
 * 版本：V4.5.12.1101
 * 
 * 修改者：         时间：2010-3-24                
 * 修改说明：在参数设置的时候，如果有null值的参数，将在数据库设置NULL值。
 * 
 *  * 修改者：         时间：2012-4-11                
 * 修改说明：处理SqlServer自增插入的问题,详见SqlServer.cs。
 * 
 * * 修改者：         时间：2012-5-11                
 * 修改说明：增加命令执行的超时时间设定。
 * 
 * 修改者：         时间：2012-10-12                
 * 修改说明：增加连接会话功能，以便在一个连接中执行多次查询（不同于事务）。
 * 
 * 修改者：         时间：2012-10-30                
 * 修改说明：使用MySQL等PDF.NET外部数据访问提供程序的时候，改进实例对象的创建效率。
 * 
 * 修改者：         时间：2012-11-01                
 * 修改说明：为支持扩展SQLite驱动，改进了本类的某些成员的访问级别。
 * 
 * 修改者：         时间：2012-11-06
 * 为提高效率，不再继续内部进行参数克隆处理，请多条SQL语句不要使用同名的参数对象
 * 
 * 修改者：         时间：2013-1-13
 * 增加获取本地数据库类型的参数名称的抽象方法，增加“读写分离”功能，只需要设置
 *   DataWriteConnectionString
 * 属性即可，注意设置该属性不改变当前使用的数据库类型。
 * 
 *  修改者：         时间：2013-2-24
 * 修改获取参数的方法为可重写，以解决Access 访问类的重写需求
 * 
 *  修改者：         时间：2013-3-8
 * 执行查询后，清除参数集合，避免参数重复占用的问题。
 * 
 * 修改者：         时间：2013-3-25
 * 为支持本类的读写分离功能，修复了在事务中执行ExecuteNoneQuery引起了一个Bug，感谢网友“长的没礼貌”发现此Bug。
 * 
 *  修改者：         时间：2013-4-16
 * 修复SQL日志中，没有记录参数化查询的参数值问题，感谢网友“GIV-顺德”发现此Bug。
 * 
 * *  修改者：         时间：2013-８-８
 * 在事务回滚方法中递减事务计数器，并且修改数据阅读器方法判断事务数量的方式
 * 
 * * 修改者：         时间：2015-1-29
 * 修正OpenSession 打开连接会话之后，DataReader关闭连接的问题
 * 
 *  修改者：         时间：2015-12-12
 * 增加命令处理管道，以便你管理框架的查询命令处理行为，比如插入命令执行日志处理器或者自定义的其他命令执行处理器，
 * 详细内容，请看 SampleORMTest 项目的 LocalDbContext 类的示例代码。
 * 
 * 修改者：         时间：2016-1-28
 * 增加通用日志接口，可以记录事务的日志。如果开启了事务，但没有设置日志对象，系统会使用默认的命令日志对象，但需要配置文件进行相应的配置，才会真正记录日志。
 * ========================================================================
*/


using System;
using System.Data;
using System.IO;
using System.Reflection;
using PWMIS.Common;
using PWMIS.Core;
using System.Data.Common;
using System.Collections.Generic;

namespace PWMIS.DataProvider.Data
{
    /// <summary>
    /// 通用数据访问对象，如果配置了日志记录，将同时初始化命令执行日志处理器到【命令处理管道】中。
    /// <remarks>
    /// 正常情况下CRUD方法都是使用一个局部的连接变量，因此是线程安全的原子操作；
    /// 如果开启了事务，或者开启了 opensession，将共享同一个连接对象，直到事务结束或者关闭会话，因此此时连接对象不是线程安全的。
    /// 所以，MyDB.Instance 不可以用于多线程环境下开启会话或者执行事务方法。
    /// </remarks>
    /// </summary>
    public abstract class CommonDB : IDisposable
    {
        private string _connString = string.Empty;
        private string _writeConnString =null;
        private string _errorMessage = string.Empty;
        private bool _onErrorRollback = true;
        private bool _onErrorThrow = true;
        private IDbConnection _connection = null;
        private IDbTransaction _transation = null;
        protected long _elapsedMilliseconds = 0;

        private string appRootPath = "";

        private int transCount;//事务计数器
        private IDbConnection sessionConnection = null;//会话使用的连接
        private bool disposed;

       /// <summary>
       /// 初始化通用数据访问对象，如果配置了日志记录，将同时初始化命令执行日志处理器到【命令处理管道】中。
       /// </summary>
        public CommonDB()
        {
            //CommandLog.SaveCommandLog 取决于应用程序配置文件的配置名称 "SaveCommandLog"，
            //如果是"true"，则 CommandLog.SaveCommandLog ==true;
            //开启记录SQL日志后，如果没有指定日志文件路径，日志文件将在应用程序的根目录下，文件名为 sql.log
            //加入配置了日志文件路径但是没有配置需要记录日志，当应用程序查询出错的时候，会在日志文件中记录错误信息。
            //
            //除了通过配置文件配置日志处理行为，也可以通过 RegisterCommandHandle 方法注册你的日志处理程序
            if (CommandLog.SaveCommandLog)
            {
                commandHandles = new List<ICommandHandle>();
                commandHandles.Add(new CommandExecuteLogHandle());
            }
        }

        /// <summary>
        /// 根据数据库实例获取数据库类型枚举
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static DBMSType GetDBMSType(CommonDB db)
        {
            if (db != null)
            {
                if (db is Access)
                    return DBMSType.Access;
                if (db is SqlServer)
                    return DBMSType.SqlServer;
                if (db is Oracle)
                    return DBMSType.Oracle;
                if (db is OleDb)
                    return DBMSType.UNKNOWN;
                if (db is Odbc)
                    return DBMSType.UNKNOWN;
            }
            return DBMSType.UNKNOWN;
        }

        private static Dictionary<string, Type> cacheHelper = null;
        /// <summary>
        /// 创建公共数据访问类的实例
        /// </summary>
        /// <param name="providerAssembly">提供这程序集名称</param>
        /// <param name="providerType">提供者类型</param>
        /// <returns></returns>
        public static AdoHelper CreateInstance(string providerAssembly, string providerType)
        {
            //使用Activator.CreateInstance 效率远高于assembly.CreateInstance
            //所以首先检查缓存里面是否数据访问实例对象的类型
            //详细内容请参看 http://www.cnblogs.com/leven/archive/2009/12/08/instanse_create_comparison.html
            //
            if (cacheHelper == null)
                cacheHelper = new Dictionary<string, Type>();
            string key = string.Format("{0}_{1}", providerAssembly, providerType);
            if (cacheHelper.ContainsKey(key))
            {
                return (AdoHelper)Activator.CreateInstance(cacheHelper[key]);
            }

            Assembly assembly = Assembly.Load(providerAssembly);
            object provider = assembly.CreateInstance(providerType);

            if (provider is AdoHelper)
            {
                AdoHelper result = provider as AdoHelper;
                cacheHelper[key] = result.GetType();//加入缓存
                return result;
            }
            else
            {
                throw new InvalidOperationException("当前指定的的提供程序不是 AdoHelper 抽象类的具体实现类，请确保应用程序进行了正确的配置（如connectionStrings 配置节的 providerName 属性）。");
            }
        }

        /// <summary>
        /// 执行SQL查询的超时时间，单位秒。不设置则取默认时间，详见MSDN。
        /// </summary>
        public int CommandTimeOut { get; set; }

        /// <summary>
        /// 当前数据库的类型枚举
        /// </summary>
        public abstract DBMSType CurrentDBMSType { get; }
        /// <summary>
        /// 获取最近一次执行查询的所耗费的时间，单位：毫秒
        /// </summary>
        public long ElapsedMilliseconds
        {
            get { return _elapsedMilliseconds; }
            protected set { _elapsedMilliseconds = value; }
        }

        private string _insertKey;
        /// <summary>
        /// 在插入具有自增列的数据后，获取刚才自增列的数据的方式，默认使用 @@IDENTITY，在其它具体数据库实现类可能需要重写该属性或者运行时动态指定。
        /// 在SqlServer，默认使用SCOPE_IDENTITY()，可根据情况设置。
        /// </summary>
        public virtual string InsertKey
        {
            get
            {
                if (string.IsNullOrEmpty(_insertKey))
                    return "SELECT @@IDENTITY";
                else
                    return _insertKey;
            }
            set
            {
                _insertKey = value;
            }
        }

        /// <summary>
        /// 获取或者设置数据连结字符串
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return _connString;
            }
            set
            {
                _connString = value;
                CommonUtil.ReplaceWebRootPath(ref _connString);
            }
        }

        /// <summary>
        /// 写入数据的连接字符串，ExecuteNoneQuery 方法将自动使用该连接
        /// </summary>
        public string DataWriteConnectionString
        {
            get {
                //if (_writeConnString == null)
                //    return this.ConnectionString;
                //else
                //    return _writeConnString;
                return _writeConnString ?? this.ConnectionString;
            }
            set {
                _writeConnString = value;
            }
        }

        /// <summary>
        /// 获取连接字符串构造类
        /// </summary>
        public abstract DbConnectionStringBuilder ConnectionStringBuilder { get; }
        /// <summary>
        /// 连接数据库用户的ID
        /// </summary>
        public abstract string ConnectionUserID { get; }

        /// <summary>
        /// 数据操作的错误信息，请在每次查询后检查该信息。
        /// </summary>
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if (value != null && value != "")
                    _errorMessage += ";" + value;
                else
                    _errorMessage = value;
            }
        }

        /// <summary>
        /// 在事务执行期间，更新过程如果出现错误，是否自动回滚事务。默认为是。
        /// </summary>
        public bool OnErrorRollback
        {
            get { return _onErrorRollback; }
            set { _onErrorRollback = value; }
        }

        /// <summary>
        /// 查询出现错误是否是将错误抛出。默认为是。
        /// 如果设置为否，将简化调用程序的异常处理，但是请检查每次更新后受影响的结果数和错误信息来决定你的程序逻辑。
        /// 如果在事务执行期间，期望出现错误后立刻结束处理，请设置本属性为 是。
        /// </summary>
        public bool OnErrorThrow
        {
            get { return _onErrorThrow; }
            set { _onErrorThrow = value; }
        }

        private PWMIS.Common.ICommandLog _logger;
        /// <summary>
        /// 获取或者设置日志组件
        /// </summary>
        public PWMIS.Common.ICommandLog Logger
        {
            get {
                if (_logger == null)
                {
                    if (commandHandles != null)
                    {
                        foreach (ICommandHandle handle in this.commandHandles)
                        {
                            if (handle is CommandExecuteLogHandle)
                            {
                                _logger = ((CommandExecuteLogHandle)handle).CurrCommandLog;
                                break;
                            }
                        }
                    }
                }
                if (_logger == null)
                    _logger = new CommandLog();
                return _logger; 
            }
            set { _logger = value; }
        }

        /// <summary>
        /// 获取事务的数据连结对象
        /// </summary>
        /// <returns>数据连结对象</returns>
        protected virtual IDbConnection GetConnection() //
        {
            //优先使用事务的连接
            if (Transaction != null)
            {
                IDbTransaction trans = Transaction;
                if (trans.Connection != null)
                    return trans.Connection;
            }
            //如果开启连接会话，则使用该连接
            if (sessionConnection != null)
            {
                return sessionConnection;
            }
            return null;
        }

        /// <summary>
        /// 获取数据库连接对象实例
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetDbConnection()
        {
            return this.GetConnection();
        }

        /// <summary>
        /// 获取数据连结对象实例
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns>数据连结对象</returns>
        public IDbConnection GetConnection(string connectionString)
        {
            this.ConnectionString = connectionString;
            return this.GetConnection();
        }

        /// <summary>
        /// 获取数据适配器实例
        /// </summary>
        /// <returns>数据适配器</returns>
        protected abstract IDbDataAdapter GetDataAdapter(IDbCommand command);

        /// <summary>
        /// 获取或者设置事务对象
        /// </summary>
        public IDbTransaction Transaction
        {
            get { return _transation; }
            set { _transation = value; }
        }

        /// <summary>
        /// 获取参数名的标识字符，默认为SQLSERVER格式，如果其它数据库则可能需要重写该属性
        /// </summary>
        public virtual string GetParameterChar
        {
            get { return "@"; }
        }

        /// <summary>
        /// 获取一个新参数对象
        /// </summary>
        /// <returns>特定于数据源的参数对象</returns>
        public abstract IDataParameter GetParameter();

        /// <summary>
        /// 获取一个新参数对象
        /// </summary>
        /// <param name="paraName">参数名字</param>
        /// <param name="dbType">数据库数据类型</param>
        /// <param name="size">字段大小</param>
        /// <returns>特定于数据源的参数对象</returns>
        public abstract IDataParameter GetParameter(string paraName, System.Data.DbType dbType, int size);

        /// <summary>
        /// 获取一个新参数对象
        /// </summary>
        /// <param name="paraName">参数名字</param>
        /// <param name="dbType">>数据库数据类型</param>
        /// <returns>特定于数据源的参数对象</returns>
        public virtual IDataParameter GetParameter(string paraName, DbType dbType)
        {
            IDataParameter para = this.GetParameter();
            para.ParameterName = paraName;
            para.DbType = dbType;
            return para;
        }

        /// <summary>
        /// 根据参数名和值返回参数一个新的参数对象
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="Value">参数值</param>
        /// <returns>特定于数据源的参数对象</returns>
        public virtual IDataParameter GetParameter(string paraName, object Value)
        {
            IDataParameter para = this.GetParameter();
            para.ParameterName = paraName;
            para.Value = Value;
            return para;
        }

        /// <summary>
        /// 获取一个新参数对象
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="dbType">参数值</param>
        /// <param name="size">参数大小</param>
        /// <param name="paraDirection">参数输出类型</param>
        /// <returns>特定于数据源的参数对象</returns>
        public IDataParameter GetParameter(string paraName, System.Data.DbType dbType, int size, System.Data.ParameterDirection paraDirection)
        {
            IDataParameter para = this.GetParameter(paraName, dbType, size);
            para.Direction = paraDirection;
            return para;
        }

        /// <summary>
        /// 获取一个新参数对象
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="size">参数值的长度</param>
        /// <param name="paraDirection">参数的输入输出类型</param>
        /// <param name="precision">参数值参数的精度</param>
        /// <param name="scale">参数的小数位位数</param>
        /// <returns></returns>
        public IDataParameter GetParameter(string paraName, System.Data.DbType dbType, int size, System.Data.ParameterDirection paraDirection, byte precision, byte scale)
        {
            IDbDataParameter para = (IDbDataParameter)this.GetParameter(paraName, dbType, size);
            para.Direction = paraDirection;
            para.Precision = precision;
            para.Scale = scale;
            return para;
        }

        
        

        /// <summary>
        /// 获取当前数据库类型的参数数据类型名称
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public abstract string GetNativeDbTypeName(IDataParameter para);
        /// <summary>
        /// 返回此 SqlConnection 的数据源的架构信息。
        /// </summary>
        /// <param name="collectionName">集合名称，可以为空</param>
        /// <param name="restrictionValues">请求的架构的一组限制值，可以为空</param>
        /// <returns>数据库架构信息表</returns>
        public abstract DataTable GetSchema(string collectionName, string[] restrictionValues);

        /// <summary>
        /// 获取存储过程、函数的定义内容，如果子类支持，需要在子类中重写
        /// </summary>
        /// <param name="spName">存储过程名称</param>
        /// <returns></returns>
        public virtual string GetSPDetail(string spName)
        {
            return "";
        }

        /// <summary>
        /// 获取视图定义，如果子类支持，需要在子类中重写
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <returns></returns>
        public virtual string GetViweDetail(string viewName)
        {
            return "";
        }

        /// <summary>
        /// 打开连接并开启事务
        /// </summary>
        public void BeginTransaction()
        {
            BeginTransaction(IsolationLevel.ReadCommitted);
        }

        /// <summary>
        /// 开启事务并指定事务隔离级别
        /// </summary>
        /// <param name="ilevel"></param>
        public void BeginTransaction(IsolationLevel ilevel)
        {
            transCount++;
            this.ErrorMessage = "";
            _connection = GetConnection();//在子类中将会获取连接对象实例
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
            if (transCount == 1)
                _transation = _connection.BeginTransaction(ilevel);
            Logger.WriteLog("打开连接并开启事务", "AdoHelper Transaction");
        }

        /// <summary>
        /// 提交事务并关闭连接
        /// </summary>
        public void Commit()
        {
            transCount--;
            if (_transation != null && _transation.Connection != null && transCount == 0)
                _transation.Commit();

            if (transCount <= 0)
            {
                CloseGlobalConnection();
                transCount = 0;            
            }
            Logger.WriteLog("提交事务并关闭连接", "AdoHelper Transaction");
        }

        /// <summary>
        /// 回滚事务并关闭连接
        /// </summary>
        public void Rollback()
        {
            transCount--;
            if (_transation != null && _transation.Connection != null)
                _transation.Rollback();
           
            if (transCount <= 0)
            {
                CloseGlobalConnection();
                transCount = 0;
            }
            Logger.WriteLog("回滚事务并关闭连接", "AdoHelper Transaction");
        }

        /// <summary>
        /// 打开一个数据库连接会话，你可以在其中执行一系列AdoHelper查询
        /// </summary>
        /// <returns>连接会话对象</returns>
        public ConnectionSession OpenSession()
        {
            this.ErrorMessage = "";
            sessionConnection = GetConnection();//在子类中将会获取连接对象实例
            if (sessionConnection.State != ConnectionState.Open)
                sessionConnection.Open();

            Logger.WriteLog("打开会话连接", "ConnectionSession");
            return new ConnectionSession(sessionConnection);
        }

        /// <summary>
        /// 关闭连接会话
        /// </summary>
        public void CloseSession()
        {
            if (sessionConnection != null && sessionConnection.State == ConnectionState.Open)
            {
                sessionConnection.Close();
                sessionConnection.Dispose();
                sessionConnection = null;
            }
        }



        private bool _sqlServerCompatible = true;
        /// <summary>
        /// SQL SERVER 兼容性设置，默认为兼容。该特性可以将SQLSERVER的语句移植到其它其它类型的数据库，例如字段分隔符号，日期函数等。
        /// 如果是拼接字符串方式的查询，建议设置为False，避免在拼接ＳＱＬ的时候过滤掉'@'等特殊字符
        /// </summary>
        public bool SqlServerCompatible
        {
            get { return _sqlServerCompatible; }
            set { _sqlServerCompatible = value; }
        }
        /// <summary>
        /// 对应SQL语句进行其它的处理，例如将SQLSERVER的字段名外的中括号替换成数据库特定的字符。该方法会在执行查询前调用，默认情况下不进行任何处理。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        protected virtual string PrepareSQL( string sql)
        {
            return sql;
        }

        /// <summary>
        /// 获取经过本地数据库类型处理过的SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string GetPreparedSQL(string sql)
        {
            return this.PrepareSQL( sql);
        }

        /// <summary>
        /// 完善命令对象,处理命令对象关联的事务和连接，如果未打开连接这里将打开它
        /// 注意：为提高效率，不再继续内部进行参数克隆处理，请多条SQL语句不要使用同名的参数对象
        /// </summary>
        /// <param name="cmd">命令对象</param>
        /// <param name="SQL">SQL</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">参数数组</param>
        protected void CompleteCommand(IDbCommand cmd,  string SQL,  CommandType commandType,  IDataParameter[] parameters)
        {
            //SQL 可能在OnExecuting 已经处理，因此PrepareSQL 对于某些Oracle大写的字段名，不会有影响
            cmd.CommandText = SqlServerCompatible ? PrepareSQL(  SQL) : SQL;
            cmd.CommandType = commandType;
            cmd.Transaction = this.Transaction;
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
                cmd.Connection.Open();
            //增加日志处理
            //dth,2008.4.8
            //
            //			if(SaveCommandLog )
            //				RecordCommandLog(cmd);
            //CommandLog.Instance.WriteLog(cmd,"AdoHelper");
        }

        #region 命令处理器
        private List<ICommandHandle> commandHandles;
        /// <summary>
        /// 注册一个命令处理器
        /// </summary>
        /// <param name="handles"></param>
        public void RegisterCommandHandle(ICommandHandle handle)
        {
            if (handle != null)
            {
                if (this.commandHandles == null)
                    this.commandHandles = new List<ICommandHandle>();
                this.commandHandles.Add(handle);
            }
        }

        private bool OnCommandExecuting(ref string sql, CommandType commandType, IDataParameter[] parameters)
        {
            if (commandHandles != null)
            {
                foreach (ICommandHandle handle in this.commandHandles)
                {
                    if (handle.ApplayDBMSType== DBMSType.UNKNOWN || handle.ApplayDBMSType == this.CurrentDBMSType)
                    {
                        bool flag = handle.OnExecuting(this,ref sql, commandType, parameters);
                        if (!flag)
                            return false;
                    }
                }
            }
            return true;
        }

        private void OnCommandExecuteError(IDbCommand cmd, string errorMessage)
        {
            if (commandHandles != null)
            {
                foreach (ICommandHandle handle in this.commandHandles)
                {
                    if (handle.ApplayDBMSType == DBMSType.UNKNOWN || handle.ApplayDBMSType == this.CurrentDBMSType)
                        handle.OnExecuteError(cmd, errorMessage);
                }
            }
        }

        private void OnCommandExected(IDbCommand cmd)
        {
            if (commandHandles != null)
            {
                foreach (ICommandHandle handle in this.commandHandles)
                {
                    if (handle.ApplayDBMSType == DBMSType.UNKNOWN || handle.ApplayDBMSType == this.CurrentDBMSType)
                    {
                        long result = handle.OnExected(cmd);
                        if (handle is CommandExecuteLogHandle)
                            this._elapsedMilliseconds = result;
                    }
                }
            }
        }

        #endregion


        /// <summary>
        /// 执行不返回值的查询，如果此查询出现了错误并且设置 OnErrorThrow 属性为 是，将抛出错误；否则将返回 -1，此时请检查ErrorMessage属性；
        /// 如果此查询在事务中并且出现了错误，将根据 OnErrorRollback 属性设置是否自动回滚事务。
        /// </summary>
        /// <param name="SQL">SQL</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>受影响的行数</returns>
        public virtual int ExecuteNonQuery(string SQL, CommandType commandType, IDataParameter[] parameters)
        {
            if (!OnCommandExecuting(ref SQL, commandType, parameters))
                return -1;

            ErrorMessage = "";
            IDbConnection conn = GetConnection();
            if (conn.State != ConnectionState.Open) //连接已经打开，不能切换连接字符串，感谢网友 “长的没礼貌”发现此Bug 
                conn.ConnectionString = this.DataWriteConnectionString;
            IDbCommand cmd = conn.CreateCommand();
            CompleteCommand(cmd,  SQL,  commandType,  parameters);

            int result = -1;
            try
            {
                result = cmd.ExecuteNonQuery();
                //如果开启事务，则由上层调用者决定何时提交事务
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                bool inTransaction = cmd.Transaction == null ? false : true;

                //如果开启事务，那么此处应该回退事务
                if (cmd.Transaction != null && OnErrorRollback)
                    cmd.Transaction.Rollback();

                OnCommandExecuteError(cmd, ErrorMessage);
                if (OnErrorThrow)
                {
                    throw new QueryException(ex.Message, cmd.CommandText, commandType, parameters, inTransaction, conn.ConnectionString);
                }
            }
            finally
            {
                OnCommandExected(cmd);
                CloseConnection(conn, cmd);
            }
            return result;
        }

        /// <summary>
        /// 执行不返回值的查询，如果此查询出现了错误，将返回 -1，此时请检查ErrorMessage属性；
        /// 如果此查询在事务中并且出现了错误，将根据 OnErrorRollback 属性设置是否自动回滚事务。
        /// </summary>
        /// <param name="SQL">SQL</param>
        /// <returns>受影响的行数</returns>
        public int ExecuteNonQuery(string SQL)
        {
            return ExecuteNonQuery(SQL, CommandType.Text, null);
        }

        /// <summary>
        /// 执行插入数据的查询，仅限于Access，SqlServer
        /// </summary>
        /// <param name="SQL">插入数据的SQL</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">参数数组</param>
        /// <param name="ID">要传出的本次操作的新插入数据行的自增主键ID值，如果没有获取到，则为-1</param>
        /// <returns>本次查询受影响的行数</returns>
        public virtual int ExecuteInsertQuery(string SQL, CommandType commandType, IDataParameter[] parameters, ref object ID,string insertKey)
        {
            if (insertKey == null) insertKey = "";
            if (!OnCommandExecuting(ref SQL, commandType, parameters))
                return -1;
            IDbConnection conn = GetConnection();
            if (conn.State != ConnectionState.Open) //连接已经打开，不能切换连接字符串，感谢网友 “长的没礼貌”发现此Bug 
                conn.ConnectionString = this.DataWriteConnectionString;
            IDbCommand cmd = conn.CreateCommand();
            CompleteCommand(cmd,  SQL,  commandType,  parameters);

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

                result = cmd.ExecuteNonQuery();
                //这里使用 参数而不是 this.InsertKey ，避免多线程插入不同实体类在Oracle数据库上的问题
                if (insertKey == "") insertKey = this.InsertKey;
                if (!string.IsNullOrEmpty(insertKey)) 
                {
                    //cmd.Parameters.Clear();
                    //不清除参数对象在Oracle会发生错误
                    //但是清除了参数，会让SQL日志没法记录参数信息，故下面创建一个新的命令对象
                    IDbCommand cmd2 = conn.CreateCommand();
                    cmd2.CommandText = insertKey;// "SELECT @@IDENTITY ";
                    cmd2.Transaction = cmd.Transaction;
                    ID = cmd2.ExecuteScalar();
                }
                else
                {
                    ID = -1;//表示未获取到自增列的值
                }
              
                //如果在内部开启了事务则提交事务，否则外部调用者决定何时提交事务
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

                OnCommandExecuteError(cmd, ErrorMessage);
                if (OnErrorThrow)
                {
                    throw new QueryException(ex.Message, cmd.CommandText, commandType, parameters, inTransaction, conn.ConnectionString);
                }

            }
            finally
            {
                OnCommandExected(cmd);
                CloseConnection(conn, cmd);
            }
            return result;
        }

        /// <summary>
        /// 执行插入数据的查询
        /// </summary>
        /// <param name="SQL">插入数据的SQL</param>
        /// <param name="ID">要传出的本次操作的新插入数据行的主键ID值</param>
        /// <returns>本次查询受影响的行数</returns>
        public int ExecuteInsertQuery(string SQL, ref object ID)
        {
            return ExecuteInsertQuery(SQL, CommandType.Text, null, ref ID,null);
        }

        /// <summary>
        /// 执行返回单一值得查询
        /// </summary>
        /// <param name="SQL">SQL</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>查询结果</returns>
        public virtual object ExecuteScalar(string SQL, CommandType commandType, IDataParameter[] parameters)
        {
            if (!OnCommandExecuting(ref SQL, commandType, parameters))
                return -1;
            IDbConnection conn = GetConnection();
            IDbCommand cmd = conn.CreateCommand();
            CompleteCommand(cmd,  SQL,  commandType,  parameters);

            CommandLog cmdLog = new CommandLog(true);

            object result = null;
            try
            {
                result = cmd.ExecuteScalar();
                //如果开启事务，则由上层调用者决定何时提交事务
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                //如果开启事务，那么此处应该回退事务
                //if(cmd.Transaction!=null)
                //    cmd.Transaction.Rollback ();

                bool inTransaction = cmd.Transaction == null ? false : true;
                OnCommandExecuteError(cmd, ErrorMessage);
                if (OnErrorThrow)
                {
                    throw new QueryException(ex.Message, cmd.CommandText, commandType, parameters, inTransaction, conn.ConnectionString);
                }
            }
            finally
            {
                OnCommandExected(cmd);
                CloseConnection(conn, cmd);
            }
            return result;
        }

        /// <summary>
        /// 执行返回单一值得查询
        /// </summary>
        /// <param name="SQL">SQL</param>
        /// <returns>查询结果</returns>
        public object ExecuteScalar(string SQL)
        {
            return ExecuteScalar(SQL, CommandType.Text, null);
        }

        /// <summary>
        /// 执行返回数据集的查询
        /// </summary>
        /// <param name="SQL">SQL</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>数据集</returns>
        public virtual DataSet ExecuteDataSet(string SQL, CommandType commandType, IDataParameter[] parameters)
        {
            DataSet ds = new DataSet();
            return ExecuteDataSetWithSchema(SQL, commandType, parameters, ds);
        }

        /// <summary>
        /// 执行返回数据架构的查询，注意，不返回任何行
        /// </summary>
        /// <param name="SQL">SQL</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>数据架构</returns>
        public virtual DataSet ExecuteDataSetSchema(string SQL, CommandType commandType, IDataParameter[] parameters)
        {
            if (!OnCommandExecuting(ref SQL, commandType, parameters))
                return null;
            IDbConnection conn = GetConnection();
            IDbCommand cmd = conn.CreateCommand();
            CompleteCommand(cmd,  SQL,  commandType,  parameters);
            IDataAdapter ada = GetDataAdapter(cmd);

            DataSet ds = new DataSet();
            try
            {
                ada.FillSchema(ds, SchemaType.Mapped);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                bool inTransaction = cmd.Transaction == null ? false : true;
                OnCommandExecuteError(cmd, ErrorMessage);
                if (OnErrorThrow)
                {
                    throw new QueryException(ex.Message, cmd.CommandText, commandType, parameters, inTransaction, conn.ConnectionString);
                }
            }
            finally
            {
                OnCommandExected(cmd);
                CloseConnection(conn, cmd);
            }
            return ds;
        }

        /// <summary>
        /// 执行查询,并返回具有数据架构的数据集(整个过程仅使用一次连接)
        /// </summary>
        /// <param name="SQL">查询语句</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">查询参数</param>
        /// <returns>具有数据架构的数据集</returns>
        public virtual DataSet ExecuteDataSetWithSchema(string SQL, CommandType commandType, IDataParameter[] parameters)
        {
            if (!OnCommandExecuting(ref SQL, commandType, parameters))
                return null;
            IDbConnection conn = GetConnection();
            IDbCommand cmd = conn.CreateCommand();
            CompleteCommand(cmd,  SQL,  commandType,  parameters);
            IDataAdapter ada = GetDataAdapter(cmd);

            CommandLog cmdLog = new CommandLog(true);
            DataSet ds = new DataSet();
            try
            {
                ada.FillSchema(ds, SchemaType.Mapped);
                ada.Fill(ds);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                bool inTransaction = cmd.Transaction == null ? false : true;
                OnCommandExecuteError(cmd, ErrorMessage);
                if (OnErrorThrow)
                {
                    throw new QueryException(ex.Message, cmd.CommandText, commandType, parameters, inTransaction, conn.ConnectionString);
                }
            }
            finally
            {
                OnCommandExected(cmd);
                CloseConnection(conn, cmd);
            }

            return ds;
        }

        /// <summary>
        /// 执行查询,并以指定的(具有数据架构的)数据集来填充数据
        /// </summary>
        /// <param name="SQL">查询语句</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">查询参数</param>
        /// <param name="schemaDataSet">指定的(具有数据架构的)数据集</param>
        /// <returns>具有数据的数据集</returns>
        public virtual DataSet ExecuteDataSetWithSchema(string SQL, CommandType commandType, IDataParameter[] parameters, DataSet schemaDataSet)
        {
            if (!OnCommandExecuting(ref SQL, commandType, parameters))
                return null;
            IDbConnection conn = GetConnection();
            IDbCommand cmd = conn.CreateCommand();
            CompleteCommand(cmd,  SQL,  commandType,  parameters);
            IDataAdapter ada = GetDataAdapter(cmd);

            try
            {
                //使用MyDB.Intance 连接不能及时关闭？待测试
                ada.Fill(schemaDataSet);//FillSchema(ds,SchemaType.Mapped )
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                bool inTransaction = cmd.Transaction == null ? false : true;
                OnCommandExecuteError(cmd, ErrorMessage);
                if (OnErrorThrow)
                {
                    throw new QueryException(ex.Message, cmd.CommandText, commandType, parameters, inTransaction, conn.ConnectionString);
                }
            }
            finally
            {
                OnCommandExected(cmd);
                CloseConnection(conn, cmd);
            }
            return schemaDataSet;
        }

        /// <summary>
        /// 执行返回数据集的查询
        /// </summary>
        /// <param name="SQL">SQL</param>
        /// <returns>数据集</returns>
        public DataSet ExecuteDataSet(string SQL)
        {
            return ExecuteDataSet(SQL, CommandType.Text, null);
        }


        /// <summary>
        /// 返回单一行的数据阅读器
        /// </summary>
        /// <param name="SQL">SQL</param>
        /// <returns>数据阅读器</returns>
        public IDataReader ExecuteDataReaderWithSingleRow(string SQL)
        {
            IDataParameter[] paras = { };
            //在有事务的时候不能关闭连接
            return ExecuteDataReaderWithSingleRow(SQL, paras);
        }

        /// <summary>
        /// 返回单一行的数据阅读器
        /// </summary>
        /// <param name="SQL">SQL</param>
        /// <param name="paras">参数</param>
        /// <returns>数据阅读器</returns>
        public IDataReader ExecuteDataReaderWithSingleRow(string SQL, IDataParameter[] paras)
        {
            //在有事务或者有连接会话的时候不能关闭连接
            if (this.transCount > 0 || this.sessionConnection != null)
                return ExecuteDataReader(ref SQL, CommandType.Text, CommandBehavior.SingleRow, ref paras);
            else
                return ExecuteDataReader(ref SQL, CommandType.Text, CommandBehavior.SingleRow | CommandBehavior.CloseConnection, ref paras);
        }

        /// <summary>
        /// 根据查询返回数据阅读器对象，在非事务过程中，阅读完以后会自动关闭数据库连接
        /// </summary>
        /// <param name="SQL">SQL</param>
        /// <returns>数据阅读器</returns>
        public IDataReader ExecuteDataReader(string SQL)
        {
            IDataParameter[] paras = { };
            //在有事务或者有会话的时候不能关闭连接 edit at 2012.7.23
            //this.Transaction == null 不安全
            CommandBehavior behavior = this.transCount > 0 || this.sessionConnection != null
                ? CommandBehavior.SingleResult
                : CommandBehavior.SingleResult | CommandBehavior.CloseConnection;
            return ExecuteDataReader(ref SQL, CommandType.Text, behavior, ref paras);
        }

        /// <summary>
        /// 根据查询返回数据阅读器对象
        /// </summary>
        /// <param name="SQL">SQL</param>
        /// <param name="cmdBehavior">对查询和返回结果有影响的说明</param>
        /// <returns>数据阅读器</returns>
        public IDataReader ExecuteDataReader(string SQL, CommandBehavior cmdBehavior)
        {
            IDataParameter[] paras = { };
            return ExecuteDataReader(ref SQL, CommandType.Text, cmdBehavior, ref paras);
        }

        /// <summary>
        /// 根据查询返回数据阅读器对象，但不可随机读取行内数据
        /// </summary>
        /// <param name="SQL">SQL</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>数据阅读器</returns>
        public IDataReader ExecuteDataReader(string SQL, CommandType commandType, IDataParameter[] parameters)
        {
            //在有事务或者有会话的时候不能关闭连接 edit at 2012.7.23
            //this.Transaction == null 不安全
            CommandBehavior behavior = this.transCount > 0 || this.sessionConnection != null
                ? CommandBehavior.SingleResult
                : CommandBehavior.SingleResult | CommandBehavior.CloseConnection;
            return ExecuteDataReader(ref SQL, commandType, behavior, ref parameters);
        }

        /// <summary>
        /// 根据查询返回数据阅读器对象,可以顺序读取行内的大数据列
        /// </summary>
        /// <param name="SQL">SQL</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>数据阅读器</returns>
        public IDataReader ExecuteDataReaderSequentialAccess(string SQL, CommandType commandType, IDataParameter[] parameters)
        {
            CommandBehavior behavior = this.transCount == 0 //this.Transaction == null 不安全
                ? CommandBehavior.SingleResult | CommandBehavior.CloseConnection | CommandBehavior.SequentialAccess
                : CommandBehavior.SingleResult | CommandBehavior.SequentialAccess;//新增SequentialAccess 2013.9.24
            return ExecuteDataReader(ref SQL, commandType, behavior, ref parameters);
        }

        /// <summary>
        /// 根据查询返回数据阅读器对象
        /// </summary>
        /// <param name="SQL">SQL</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="cmdBehavior">对查询和返回结果有影响的说明</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>数据阅读器</returns>
        protected virtual IDataReader ExecuteDataReader(ref string SQL, CommandType commandType, CommandBehavior cmdBehavior, ref IDataParameter[] parameters)
        {
            if (!OnCommandExecuting(ref SQL, commandType, parameters))
                return null;
            IDbConnection conn = GetConnection();
            IDbCommand cmd = conn.CreateCommand();
            CompleteCommand(cmd,  SQL,  commandType,  parameters);

            IDataReader reader = null;
            try
            {
                //如果命令对象的事务对象为空，那么强制在读取完数据后关闭阅读器的数据库连接 2008.3.20
                if (cmd.Transaction == null && cmdBehavior == CommandBehavior.Default)
                    cmdBehavior = CommandBehavior.CloseConnection;
                reader = cmd.ExecuteReader(cmdBehavior);
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
                    throw new QueryException(ex.Message, cmd.CommandText, commandType, parameters, inTransaction, conn.ConnectionString);
                }
            }

            OnCommandExected(cmd);
            cmd.Parameters.Clear();

            return reader;
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <param name="conn">连接对象</param>
        /// <param name="cmd">命令对象</param>
        protected void CloseConnection(IDbConnection conn, IDbCommand cmd)
        {
            if (cmd.Transaction == null && conn != sessionConnection && conn.State == ConnectionState.Open)
                conn.Close();
            cmd.Parameters.Clear();
        }

        private void CloseGlobalConnection()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
                _connection.Close();
        }

        //Dispose与GC的关系，参考 http://www.cnblogs.com/coolkiss/archive/2010/08/23/1806382.html

        void IDisposable.Dispose()
        {
            if (!disposed)
            {
                Dispose(true);
                disposed = true;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free other state (managed objects).
                CloseGlobalConnection();
                CloseSession();
            }
            // Free your own state (unmanaged objects).
            // Set large fields to null.
            _connection = null;
            _transation = null;
            sessionConnection = null;
        }

    }
}
