using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PWMIS.Core.Extensions
{
    /// <summary>
    /// 复制状态
    /// </summary>
    public enum ReplicationStatus
    {
        /// <summary>
        /// 未知状态
        /// </summary>
        UnKnown,
        /// <summary>
        /// 日志已经存在
        /// </summary>
        LogExists,
        /// <summary>
        /// 复制成功
        /// </summary>
        Succeed,
        /// <summary>
        /// 复制过程发生错误，请查看错误信息
        /// </summary>
        Error
    }

    /// <summary>
    /// 管理事务日志的数据访问上下文对象
    /// </summary>
    public class TransLogDbContext : DbContext
    {
        /// <summary>
        /// 操作过程中的错误信息
        /// </summary>
        public string ErrorMessage { get; private set; }
        /// <summary>
        /// 当前操作的复制状态
        /// </summary>
        public ReplicationStatus CurrentStatus { get; private set; }

        /// <summary>
        /// 以一个数据上下文对象初始化本类
        /// </summary>
        /// <param name="db"></param>
        public TransLogDbContext(AdoHelper db) : base(db)
        { }

        /// <summary>
        /// 以连接字符串名字来初始化本类
        /// </summary>
        /// <param name="connName">应用程序配置文件中的连接字符串名字</param>
        public TransLogDbContext(string connName) : base(connName)
        { }

        protected override bool CheckAllTableExists()
        {
            base.CheckTableExists<MyCommandLogEntity>();
            return true;
        }

        /// <summary>
        /// 按照顺序读取事务日志并进行处理
        /// </summary>
        /// <param name="pageSize">每次要读取的日志页大小</param>
        /// <param name="func">处理日志的自定义方法，如果返回假则不再继续读取处理</param>
        /// <returns>返回已经读取的记录数量</returns>
        public int ReadLog(int pageSize,Func<List<MyCommandLogEntity>,bool> func )
        {
            int pageNumber = 1;
            int readCount = 0;
            while (true)
            {
                var list = OQL.From<MyCommandLogEntity>()
                .Select()
                .OrderBy((o, p) => o.Asc(p.CommandID))
                .Limit(pageSize, pageNumber)
                .ToList(this.CurrentDataBase);
                if (list.Count == 0)
                    break;
                readCount += list.Count;

                if (!func(list))
                    break;
                if (list.Count < pageSize)
                    break;
                pageNumber++;
            }
            return readCount;
        }

        /// <summary>
        /// 将制定的数据复制到当前数据访问对象关联的数据库中
        /// </summary>
        /// <param name="log">事务日志实体</param>
        /// <returns>如果数据已经存在或者复制成功，返回真；如果遇到错误，返回假</returns>
        public bool DataReplication(MyCommandLogEntity log)
        {
            this.ErrorMessage = string.Empty;
            this.CurrentStatus = ReplicationStatus.UnKnown;

            var query = this.NewQuery<MyCommandLogEntity>();
            if (query.ExistsEntity(log))
            {
                this.CurrentStatus = ReplicationStatus.LogExists;
                return true;
            }
            else
            {
                string errorMessage;
                MyCommandLogEntity newLog = new MyCommandLogEntity();
                newLog.CommandID = log.CommandID;
                newLog.CommandName = log.CommandName;
                newLog.CommandText = log.CommandText;
                newLog.CommandType = log.CommandType;
                newLog.ExecuteTime = DateTime.Now;
                newLog.LogFlag = 2;//表示已经复制的状态

                bool result= Transaction(ctx => {
                    query.Insert(newLog);
                    
                    //解析得到真正的命令参数信息
                    var paras= log.ParseParameter(this.CurrentDataBase);
                    int count= this.CurrentDataBase.ExecuteNonQuery(log.CommandText, log.CommandType, paras);
                    //可以在此处考虑引发处理后事件
                }, out errorMessage);

                this.ErrorMessage = errorMessage;
                this.CurrentStatus = result ? ReplicationStatus.Succeed : ReplicationStatus.Error;
                return result;
            }
        }
    }
}
