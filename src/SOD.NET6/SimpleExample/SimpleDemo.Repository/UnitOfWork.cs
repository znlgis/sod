using SimpleDemo.Interface.Infrastructure;

namespace SimpleDemo.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IUowManager _dbContext;
        private readonly IRepositoryProvider _provider;

        public Guid ID { get; private set; }

        public UnitOfWork(IUowManager dbContext, IRepositoryProvider provider)
        {
            _dbContext = dbContext;
            _provider= provider;
            ID = dbContext.ID;
        }

        public void BeginTransaction()
        {
            _dbContext.BeginTransaction();
        }

        public void Commit()
        {
            _dbContext.Commit();
        }

        public void Rollback()
        {
            _dbContext.Rollback();
        }

        public T GetRepository<T>() where T : IRepository
        {
            return _provider.GetRepository<T>(_dbContext);
        }

        public void Dispose()
        {
            //_dbContext.Dispose();
        }
    }
}
