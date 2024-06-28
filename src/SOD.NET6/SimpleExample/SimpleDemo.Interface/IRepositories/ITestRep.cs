using SimpleDemo.Interface.Infrastructure;

namespace SimpleDemo.Interface.IRepositories
{
    public interface ITestRep : IRepository
    {
        void Add(object obj);
    }
}
