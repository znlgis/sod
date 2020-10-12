using PWMIS.Common;
using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PWMIS.Core
{
    /// <summary>
    /// 命令日志实体类(V3版本)
    /// </summary>
    public class MyCommandLogEntity:EntityBase
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MyCommandLogEntity()
        {
            TableName = "SOD_CmdLog_V3";
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
        /// 命令日志的使用标记0表示为源库未处理，1表示源库已经处理，2表示已经复制到目标库，其它值为用户自定义的状态标记
        /// </summary>
        public int LogFlag
        {
            get { return getProperty<int>("Flag"); }
            set { setProperty("Flag", value); }
        }

        /// <summary>
        /// 执行的命令语句（如果语句超过4000个字符，外面将无法直接设置此属性；在数据库获取此属性值的时候，会显示为[LonqSql]=lengch 字样）
        /// </summary>
        public string CommandText
        {
            get { return getProperty<string>("SQL"); }
            set { setProperty("SQL", value,4000); }
        }

        /// <summary>
        /// 语句类型，取值为SQLOperatType 枚举
        /// </summary>
        public SQLOperatType SQLType
        {
            get { return getProperty<SQLOperatType>("SQLType"); }
            set { setProperty("SQLType", value); }
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
        /// 命令名字，对应的表名称，存储过程名字或者其它分类名
        /// </summary>
        public string CommandName
        {
            get { return getProperty<string>("Name"); }
            set { setProperty("Name", value, 100); }
        }

        /// <summary>
        /// 命令参数信息
        /// </summary>
        public string ParameterInfo
        {
            get { return getProperty<string>("Parameters"); }
            set { setProperty("Parameters", value); }
        }

        /// <summary>
        /// 命令日志的主题，比如要附加操作的数据条件，数据版本号等
        /// </summary>
        public string LogTopic
        {
            get { return getProperty<string>("LogTopic"); }
            set { setProperty("LogTopic", value, 200); }
        }

        #region 实体操作方法定义

        /// <summary>
        /// 准备写入，设置最终写入的SQL语句和参数等信息
        /// </summary>
        protected internal void PrepairSQL(string sql,string parameterString)
        {
            if (sql.StartsWith("INSERT INTO"))
                this.SQLType = SQLOperatType.Insert;
            else if (sql.StartsWith("UPDATE "))
                this.SQLType = SQLOperatType.Update;
            else if (sql.StartsWith("DELETE FROM "))
                this.SQLType = SQLOperatType.Delete;
            else
                this.SQLType = SQLOperatType.Select;

            //处理SQL语句超长问题
            if (sql.Length >= 4000)
            {
                this.CommandText = string.Format("[LonqSql]={0}", sql.Length);
                this.ParameterInfo = sql + "\r\n\r\n" + parameterString;
            }
            else
            {
                this.CommandText = sql;
                this.ParameterInfo = parameterString;
            }
        }

        /// <summary>
        /// 准备读取，设置程序处理需要的真正的SQL语句和参数信息，如果不调用此方法，得到的是数据库原始存储的属性值
        /// </summary>
        public void PrepairRead()
        {
            if (this.CommandText.StartsWith("[LonqSql]"))
            {
                string[] arr = this.CommandText.Split('=');
                int length = int.Parse(arr[1]);
                string temp = this.ParameterInfo;
                this["CommandText"] = temp.Substring(0, length);
                this.ParameterInfo = temp.Substring(length + 4);
            }
        }

        /// <summary>
        /// 根据参数信息字符串，解析当前查询语句对应的参数化对象数组。如果没有参数信息将返回空
        /// </summary>
        /// <param name="db">数据访问对象</param>
        /// <returns></returns>
        public IDataParameter[] ParseParameter(AdoHelper db)
        {
            PrepairRead();
            if (!string.IsNullOrEmpty(this.ParameterInfo))
            {
                return DbParameterSerialize.DeSerialize(this.ParameterInfo, db);
            }
            return null;
        }

        #endregion
    }
}
