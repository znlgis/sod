namespace SimpleDemo.Interface.Infrastructure
{
    public interface IUowManager
    {
        /// <summary>
        /// 当前工作单元对象实例的标识
        /// </summary>
        Guid ID { get; }
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
