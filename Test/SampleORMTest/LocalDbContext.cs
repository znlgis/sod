using PWMIS.Core.Extensions;
using PWMIS.DataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleORMTest
{
    /// <summary>
    /// 用来测试的本地 数据库上下文类
    /// </summary>
    public class LocalDbContext : DbContext  
    {
        public LocalDbContext()
            : base("local")
        {
            //local 是连接字符串名字
            //注册日志处理器和Oracle命令处理器
            base.CurrentDataBase.RegisterCommandHandle(new CommandExecuteLogHandle());
            //base.CurrentDataBase.RegisterCommandHandle(new OracleCommandHandle());
        }

        #region 父类抽象方法的实现

        protected override bool CheckAllTableExists()
        {
            //创建用户表
            CheckTableExists<User>();
            return true;
        }

        #endregion
    }

    /// <summary>
    /// 自定义的Oracle命令处理器，用于处理特殊的字段名大写问题
    /// </summary>
    public class OracleCommandHandle : ICommandHandle
    {

        public bool OnExecuting(CommonDB db, ref string sql, System.Data.CommandType commandType, System.Data.IDataParameter[] parameters)
        {
            sql= sql.Replace("[", "").Replace("]", "").Replace("@", ":").ToUpper();
            //设置SQLSERVER兼容性为假，避免命令对象真正执行的时候再进行Oracle的查询语句的预处理。
            db.SqlServerCompatible = false;
            //返回真，以便最终执行查询，否则将终止查询
            return true;
        }

        public void OnExecuteError(System.Data.IDbCommand cmd, string errorMessage)
        {
            
        }

        public long OnExected(System.Data.IDbCommand cmd, int recordAffected)
        {
            return 1;
        }

        public PWMIS.Common.DBMSType ApplayDBMSType
        {
            get { return PWMIS.Common.DBMSType.Oracle; }
        }
    }
}
