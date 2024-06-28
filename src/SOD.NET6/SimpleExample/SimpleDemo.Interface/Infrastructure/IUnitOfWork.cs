namespace SimpleDemo.Interface.Infrastructure
{
    public interface IUnitOfWork : IUowManager, IDisposable
    {
        T GetRepository<T>() where T : IRepository;
    }
}
