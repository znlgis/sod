namespace SimpleDemo.Interface.Infrastructure
{
    

    public interface IRepository 
    {
        /// <summary>
        /// 当前仓储对象实例的标识
        /// </summary>
        Guid ID { get; }
        IUowManager UnitOfWorkManager { get; }
    }

    public interface IRepository<TParent, T> : IRepository where TParent : IIdentityProperty<T>
    {
        IEnumerable<TParent> GetAll();
        TParent? GetById(T id);
        void Insert(TParent data);
        void Update(TParent data);
        void Delete(TParent data);
    }
}
