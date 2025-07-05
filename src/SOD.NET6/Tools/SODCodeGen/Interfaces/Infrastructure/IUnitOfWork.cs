using System;

namespace Sod.Pwmis.com.Interface.Infrastructure
{
    public interface IUnitOfWork : IUowManager, IDisposable
    {
        T GetRepository<T>() where T : IRepository;
    }

    public interface IUnitOfWorkService
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
