using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sod.Pwmis.com.Interface.Infrastructure
{
    //分布式事务管理，参考 https://www.modb.pro/db/394546

    public enum GMT_COMMIT_RESULT
    {
        /// <summary>
        /// 事务中断，不能提交
        /// </summary>
        ABORTED=0,
        /// <summary>
        /// 事务提交指令
        /// </summary>
        COMMIT =1,
        /// <summary>
        /// 事务回滚指令
        /// </summary>
        ROLLBACK=2
    }

    /// <summary>
    /// 分布式事务管理器接口
    /// </summary>
    public interface IGMT
    {
        /// <summary>
        /// 通知分布式事务管理器是否可以开启一个分布式事务，告知当前事务节点将开启一个指定名字的分布式事务，双方开始保持连接
        /// </summary>
        /// <param name="globalTransName"></param>
        /// <returns>返回是否为分布式事务</returns>
        bool BeginGlobalTran(string globalTransName);
        /// <summary>
        /// 通知分布式事务管理器，告知当前事务节点即将提交本地事务。
        /// 执行该方法等待GMT直到返回GMT_COMMIT_RESULT 结果。
        /// </summary>
        GMT_COMMIT_RESULT PrepareCommit(int timeout);
        /// <summary>
        /// 通知分布式事务管理器，告知当前事务节点已经回滚本地事务。
        /// </summary>
        void Rallback();
    }

    /// <summary>
    /// 分布式事务存储服务
    /// </summary>
    public interface IGTSS
    {
        /// <summary>
        /// 增加日志记录
        /// </summary>
        /// <param name="transName">事务名称</param>
        /// <param name="instance">实例名称</param>
        /// <returns></returns>
        bool AddLogRecord(string transName,string instance);
        /// <summary>
        /// 查询日志记录
        /// </summary>
        /// <param name="transName">事务名称</param>
        /// <returns>返回该事务当前的实例名称集合</returns>
        IEnumerable<string> QueryLogRecord(string transName);
    }
}
