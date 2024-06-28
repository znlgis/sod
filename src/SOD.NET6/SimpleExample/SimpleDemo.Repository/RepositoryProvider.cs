using SimpleDemo.Interface.Infrastructure;

namespace SimpleDemo.Repository
{
    public class RepositoryProvider : IRepositoryProvider
    {
        private readonly ISimpleServiceProvider _provider;
        public RepositoryProvider(ISimpleServiceProvider provider)
        {
            _provider= provider;
        }

        public T GetRepository<T>(IUowManager? context=null) where T : IRepository
        {
            T rep= _provider.GetService<T>();
            if (context != null && rep?.UnitOfWorkManager != context)
                throw new InvalidOperationException("IUowManager 对象不是同一个实例。");
            return rep;
            /*
            //使用注入方式传递context
            if (context == null)
                return _provider.GetService<T>();
            else
                return _provider.GetService<IUowManager, T>(context);
            */
            /*
            //使用工厂方法模式
            Type type = typeof(T);
            if (type is IEquipmentRep)
            {
                IEquipmentRep rep = new EquipmentRep(context);
                return (T)rep;

            }
            throw new NotImplementedException();
            */
        }
    }
}
