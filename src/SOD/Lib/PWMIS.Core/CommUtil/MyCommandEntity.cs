using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PWMIS.Core
{
    /// <summary>
    /// 命令日志实体类
    /// </summary>
    public class MyCommandLogEntity:EntityBase
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MyCommandLogEntity()
        {
            TableName = "SOD_CmdLog";
            PrimaryKeys.Add("CmdID");
        }

        /// <summary>
        /// 命令ID，建议使用 CommonUtil.NewSequenceGUID() 获取分布式ID
        /// </summary>
        public long CommandID
        {
            get { return getProperty<long>("CmdID"); }
            set { setProperty("CmdID", value); }
        }

        /// <summary>
        /// 命令执行的时间
        /// </summary>
        public DateTime ExecuteTime
        {
            get { return getProperty<DateTime>("ExecuteTime"); }
            set { setProperty("ExecuteTime", value); }
        }

        /// <summary>
        /// 命令日志的使用标记（扩展备用）
        /// </summary>
        public int LogFlag
        {
            get { return getProperty<int>("Flag"); }
            set { setProperty("Flag", value); }
        }

        /// <summary>
        /// 执行的命令语句
        /// </summary>
        public string CommandText
        {
            get { return getProperty<string>("SQL"); }
            set { setProperty("SQL", value,4000); }
        }
        /// <summary>
        /// 命令类型
        /// </summary>
        public CommandType CommandType
        {
            get { return getProperty<CommandType>("CommandType"); }
            set { setProperty("CommandType", value); }
        }
        /// <summary>
        /// 命令参数信息
        /// </summary>
        public string ParameterInfo
        {
            get { return getProperty<string>("Parameters"); }
            set { setProperty("Parameters", value); }
        }
    }
}
