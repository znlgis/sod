using SimpleDemo.Interface.Infrastructure;
using SimpleDemo.Interface.IRepositories;

namespace SimpleDemo.Repository.Implements
{
    public class TestRep : ITestRep
    {
        public Guid ID { get; }
        public IUowManager UnitOfWorkManager { get; }

        public TestRep(IUowManager manager)
        {
            this.UnitOfWorkManager = manager;
            this.ID = Guid.NewGuid();
        }

        public void Add(object obj)
        {
            Console.WriteLine("TestRep Add a data "+obj);
        }
    }
}
