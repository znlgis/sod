using PWMIS.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PWMIS.DataProvider.Data
{
    /// <summary>
    /// 查询命令处理器接口
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
        long OnExecuted(IDbCommand cmd, int recordAffected);
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
                CommandLog.DataLogFile = "~/sql.log";
            CommandLog.SaveCommandLog = true;
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
        }

        public long OnExecuted(IDbCommand cmd, int recordAffected)
        {
            long elapsedMilliseconds;
            CurrCommandLog.WriteLog(cmd, "AdoHelper", out elapsedMilliseconds);
            CurrCommandLog.WriteLog("RecordAffected:"+recordAffected , "AdoHelper");
            return elapsedMilliseconds;
        }

        public DBMSType ApplayDBMSType
        {
            get { return DBMSType.UNKNOWN; }
        }
    }

   
}
