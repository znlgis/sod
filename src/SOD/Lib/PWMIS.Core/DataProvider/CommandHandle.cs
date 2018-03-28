using PWMIS.Common;
using PWMIS.Core;
using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PWMIS.DataProvider.Data
{
    /// <summary>
    /// 查询命令处理器接口
    /// 可以参见 http://www.cnblogs.com/bluedoctor/p/5278995.html
    /// </summary>
    public interface ICommandHandle
    {
        /// <summary>
        /// 获取当前适用的数据库类型，如果通用，请设置为 UNKNOWN
        /// </summary>
        DBMSType ApplayDBMSType { get; }
        /// <summary>
        /// 执行前处理，比如预处理SQL，补充设定参数类型邓，返回是否继续进行查询执行
        /// </summary>
        /// <param name="db">数据库访问对象</param>
        /// <param name="SQL"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns>返回真，以便最终执行查询，否则将终止查询</returns>
        bool OnExecuting(CommonDB db, ref string SQL, CommandType commandType, IDataParameter[] parameters);

        /// <summary>
        /// 执行过程中出错情况处理
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="errorMessage"></param>
        void OnExecuteError(IDbCommand cmd, string errorMessage);
        /// <summary>
        /// 查询执行完成后的处理，不管是否执行出错都会进行的处理
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="recordAffected">命令执行的受影响记录行数</param>
        /// <returns>返回执行时间或者其它信息</returns>
        long OnExecuted(IDbCommand cmd, int recordAffected);
        /// <summary>
        /// 获取当前处理器要应用的命令执行类型，只有符合该类型才会应用当前命令处理器
        /// </summary>
        CommandExecuteType ApplayExecuteType { get; }
    }

    /// <summary>
    /// 命令执行日志处理器，可以记录SQL和参数，执行时间等信息
    /// </summary>
    public class CommandExecuteLogHandle :ICommandHandle
    {
        /// <summary>
        /// 初始化一个命令执行日志处理器
        /// </summary>
        public CommandExecuteLogHandle()
        {
            this.CurrCommandLog = new CommandLog(true);
            //这里需要进行一些初始化检查，设置日志路径等
            if (CommandLog.DataLogFile == null)
            {
                //直接记录在当前目录，可能没有写入权限
                //CommandLog.DataLogFile = "~/SOD_sql.log";
                //记录在公共程序目录
                string logFolder = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "SODLog");
                if (!System.IO.Directory.Exists(logFolder))
                    System.IO.Directory.CreateDirectory(logFolder);
                CommandLog.DataLogFile = System.IO.Path.Combine(logFolder, "sqllog_"+DateTime.Now.ToString("yyyyMMdd")+".txt");
            }

        }

        public CommandLog CurrCommandLog { get; private set; }


        public bool OnExecuting(CommonDB db, ref string SQL, CommandType commandType, IDataParameter[] parameters)
        {
            this.CurrCommandLog.ReSet();
            return true;
        }

        public void OnExecuteError(IDbCommand cmd, string errorMessage)
        {
            CurrCommandLog.WriteErrLog(cmd, "AdoHelper:" + errorMessage);
            CurrCommandLog.Flush();
        }

        public long OnExecuted(IDbCommand cmd, int recordAffected)
        {
            long elapsedMilliseconds;
            CurrCommandLog.WriteLog(cmd, "AdoHelper", out elapsedMilliseconds);
            CurrCommandLog.WriteLog("RecordAffected:"+recordAffected , "AdoHelper");
            CurrCommandLog.Flush();
            return elapsedMilliseconds;
        }

        public DBMSType ApplayDBMSType
        {
            get { return DBMSType.UNKNOWN; }
        }

        /// <summary>
        /// 获取当前处理器要应用的命令执行类型，只有符合该类型才会应用当前命令处理器
        /// </summary>
        public CommandExecuteType ApplayExecuteType {
            get {
                return CommandExecuteType.Any;
            }
        }
    }

    /// <summary>
    /// 事务日志处理器，将记录事务型查询（例如增删改操作）的详细信息到当前连接的数据库的命令日志数据表
    /// </summary>
    public class TransactionLogHandle : ICommandHandle
    {
        private CommonDB currDb;
        private MyCommandLogEntity logEntity;
        private bool enable = false;
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TransactionLogHandle()
        {
            logEntity = new MyCommandLogEntity();
            enable = true;
        }

        /// <summary>
        /// 应用的数据库类型，支持所有
        /// </summary>
        public DBMSType ApplayDBMSType
        {
            get { return DBMSType.UNKNOWN; }
        }

        /// <summary>
        /// 在记录事务日志之前的自定义处理
        /// </summary>
        public MyFunc<MyCommandLogEntity, bool> BeforLog { get; set; }

        /// <summary>
        /// 在主体查询执行成功后调用，插入命令日志记录
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="recordAffected"></param>
        /// <returns></returns>
        public long OnExecuted(IDbCommand cmd, int recordAffected)
        {
            if (this.enable)
            {
                logEntity.CommandID = CommonUtil.NewSequenceGUID();
                logEntity.ExecuteTime = DateTime.Now;
                //recordAffected > 0 表示非SELECT语句
                if (recordAffected > 0)
                {
                    //如果有日志分表逻辑，需要在 BeforLog 业务方法内处理。
                    if (BeforLog==null || ( BeforLog != null && BeforLog(logEntity)))
                    {
                        //下面一行必须禁用自身调用
                        this.enable = false;
                        //如果下面一行执行失败，会抛出异常并且回滚事务，不会执行后面的 Commit方法
                        EntityQuery<MyCommandLogEntity>.Instance.Insert(this.logEntity, this.currDb);
                        this.enable = true;
                    }
                }
                this.currDb.Commit();
            }
            return 1;
        }

        /// <summary>
        /// 在事务过程中，暂不记录相关消息，可以查看SQL日志
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="errorMessage"></param>
        public void OnExecuteError(IDbCommand cmd, string errorMessage)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 主体查询预备执行操作，这里会构造命令日志信息
        /// </summary>
        /// <param name="db">当前查询连接对象</param>
        /// <param name="SQL">当前主体要执行的查询命令</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>总是返回成功</returns>
        public bool OnExecuting(CommonDB db, ref string SQL, CommandType commandType, IDataParameter[] parameters)
        {
            if (this.enable)
            {
                this.currDb = db;
                db.BeginTransaction();
                //需要真实反映执行的语句顺序，CommandID的赋值推迟到执行后
                //logEntity.CommandID = CommonUtil.NewSequenceGUID();
                //logEntity.ExecuteTime = DateTime.Now;
                //使用 PrepairSQL 方法处理
                //logEntity.CommandText = SQL;
                logEntity.CommandType = commandType;
                logEntity.LogFlag = 0;
                //logEntity.ParameterInfo = DbParameterSerialize.Serialize(parameters);
                logEntity.PrepairSQL(SQL, DbParameterSerialize.Serialize(parameters));
                if (db.ContextObject != null)
                {
                    if (db.ContextObject is OQL)
                    {
                        logEntity.CommandName = ((OQL)db.ContextObject).currEntity.GetTableName();
                    }
                    else if (db.ContextObject is EntityBase)
                    {
                        logEntity.CommandName = ((EntityBase)db.ContextObject).GetTableName();
                    }
                    else
                    {
                        logEntity.CommandName = "";
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 获取当前处理器要应用的命令执行类型，只有符合该类型才会应用当前命令处理器
        /// </summary>
        public CommandExecuteType ApplayExecuteType
        {
            get
            {
                return CommandExecuteType.ExecuteNonQuery;
            }
        }
    }



}
