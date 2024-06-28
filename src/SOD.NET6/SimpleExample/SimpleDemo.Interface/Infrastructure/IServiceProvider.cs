namespace SimpleDemo.Interface.Infrastructure
{
    public interface ISimpleServiceProvider
    {
        T GetService<T>() where T : notnull;
        T GetService<TPara,T>(TPara paras);
    }

    public interface IRepositoryProvider
    {
        T GetRepository<T>(IUowManager? context=null) where T : IRepository;
    }
}
