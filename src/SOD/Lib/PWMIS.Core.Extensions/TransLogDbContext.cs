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
    /// 日志阅读事件参数
    /// </summary>
    public class ReadLogEventArgs : EventArgs
    {
        /// <summary>
        /// 日志记录总数
        /// </summary>
        public int AllCount { get; private set; }
        /// <summary>
        /// 当前已经读取并处理的日志记录数
        /// </summary>
        public int ReadCount { get; private set; }
        /// <summary>
        /// 已经读取的数据
        /// </summary>
        public IEnumerable<MyCommandLogEntity> ReadData { get;private set; }
        /// <summary>
        /// 以日志记录综合和阅读数初始化本类
        /// </summary>
        /// <param name="allCount"></param>
        /// <param name="readCount"></param>
        /// <param name="data">读取的数据</param>
        public ReadLogEventArgs(int allCount, int readCount , IEnumerable<MyCommandLogEntity> data)
        {
            this.ReadCount = readCount;
            this.AllCount = allCount;
        }
    }

    /// <summary>
    /// 复制操作发生后的事件参数
    /// </summary>
    public class ReplicationEventArgs : EventArgs
    {
        /// <summary>
        /// 当前日志对象
        /// </summary>
        public MyCommandLogEntity Log { get; private set; }
        /// <summary>
        /// 受影响的记录数。如果执行某个事务日志的复制并没有引发相应的影响记录数可能是有问题的，
        /// 可以在事件中处理，或者抛出异常停止复制过程
        /// </summary>
        public int AffectedCount { get; private set; }
       /// <summary>
       /// 以一个日志对象初始化本类
       /// </summary>
       /// <param name="log"></param>
       /// <param name="affected"></param>
        public ReplicationEventArgs(MyCommandLogEntity log,int affected)
        {
            this.Log = log;
            this.AffectedCount = affected;
        }
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
        /// 读取日志并且处理前的时候
        /// </summary>
        public event EventHandler<ReadLogEventArgs> OnReadLog;
        /// <summary>
        /// 复制数据，提交事务之前
        /// </summary>
        public event EventHandler<ReplicationEventArgs> AfterReplications;

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
        /// <param name="partLogName">分部的日志消息表名字,可以为空</param>
        /// <returns>返回已经读取的记录数量</returns>
        public int ReadLog(int pageSize,Func<List<MyCommandLogEntity>,bool> func ,string partLogName=null)
        {
            int pageNumber = 1;
            int readCount = 0;
            int allCount = 0;
            //先查询出所有记录数和第一页的数据
            MyCommandLogEntity log = new MyCommandLogEntity();
            if (!string.IsNullOrEmpty(partLogName))
                log.MapNewTableName(log.GetTableName() + "_" + partLogName);

            var oql = OQL.From(log)
                .Select()
                .OrderBy(o => o.Asc(log.CommandID))
                .END
                .Limit(pageSize, pageNumber);

            oql.PageWithAllRecordCount = 0;
            var list = EntityQuery<MyCommandLogEntity>.QueryList(oql, this.CurrentDataBase);
            allCount = oql.PageWithAllRecordCount;

            while (list.Count>0 )
            {
                readCount += list.Count;
                ReadLogEventArgs args = new ReadLogEventArgs(allCount, readCount, list);
                if (OnReadLog != null)
                    OnReadLog(this, args);
                if (!func(list))
                    break;
                if (list.Count < pageSize)
                    break;
               
                pageNumber++;

                /*
                //使用GOQL简化查询
                list = OQL.From<MyCommandLogEntity>()
                    .Select()
                    .OrderBy((o, p) => o.Asc(p.CommandID))
                    .Limit(pageSize, pageNumber,allCount)
                    .ToList(this.CurrentDataBase);
                */
                //因为日志可能分表，需要修改下面的方式:
                var oql1 = OQL.From(log)
                    .Select()
                    .OrderBy(o => o.Asc(log.CommandID))
                    .END
                    .Limit(pageSize, pageNumber);
                oql1.PageWithAllRecordCount = allCount;
                list = EntityQuery<MyCommandLogEntity>.QueryList(oql1, this.CurrentDataBase);
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
                //log 可能映射了新的表名
                newLog.MapNewTableName(log.GetTableName());

                bool result= Transaction(ctx => {
                    query.Insert(newLog);
                    
                    //解析得到真正的命令参数信息
                    var paras= log.ParseParameter(this.CurrentDataBase);
                    int count= this.CurrentDataBase.ExecuteNonQuery(log.CommandText, log.CommandType, paras);
                    //可以在此处考虑引发处理后事件
                    if (AfterReplications != null)
                        AfterReplications(this, new ReplicationEventArgs(log, count));
                }, out errorMessage);

                this.ErrorMessage = errorMessage;
                this.CurrentStatus = result ? ReplicationStatus.Succeed : ReplicationStatus.Error;
                return result;
            }
        }
    }
}
