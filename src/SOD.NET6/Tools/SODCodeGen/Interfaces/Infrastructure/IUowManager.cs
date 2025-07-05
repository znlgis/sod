using System;

namespace Sod.Pwmis.com.Interface.Infrastructure
{
    public interface IUowManager
    {
        /// <summary>
        /// 当前工作单元对象实例的标识
        /// </summary>
        Guid ID { get; }
        /// <summary>
        /// 开始事务。
        /// </summary>
        /// <param name="transName">事务名字，可选。如设置事务名字可方便跟踪事务的执行，比如在查询日志记录里面进行记录。</param>
        void BeginTransaction(string? transName="");
        /// <summary>
        /// 提交事务，如果提交不成功，自动回滚。
        /// </summary>
        /// <returns>是否提交成功</returns>
        bool Commit();
        /// <summary>
        /// 手动回滚事务
        /// </summary>
        void Rollback();
        /// <summary>
        /// 获取或设置当前是否为分布式事务
        /// </summary>
        bool IsGlobalTrans { get; set; }
    }
}
