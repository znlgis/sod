using SimpleDemo.Interface.Infrastructure;
using SimpleDemo.Interface;
using SimpleDemo.Model;

namespace SimpleDemo.Service
{
    public class ServiceBase<TParent, TObj, Tid> : IServiceBase<TParent, TObj, Tid>
        where TParent : class, IIdentityProperty<Tid>
        where TObj : class, TParent, new()
    {
        private IRepository<TParent, Tid> _repository;

        public ServiceBase(IRepository<TParent, Tid> repository)
        {
            Type type = typeof(Tid);
            if (type != typeof(int) && type != typeof(long) && type != typeof(string) && type != typeof(Guid))
            {
                throw new TypeAccessException("Tid类型只能是int,long,string,Guid中的一种。");
            }
            _repository = repository;
        }

        public bool Delete(Tid id)
        {
            return _repository.Delete(id) > 0;
        }

        public IEnumerable<TObj> GetAll()
        {
            var list = _repository.GetAll();
            var result = list.Select<TParent, TObj>(_repository.ConvertTo<TObj>);
            return result;
        }

        public TObj? GetById(Tid id)
        {
            TParent? parent = _repository.GetById(id);
            if (parent != null)
                return _repository.ConvertTo<TObj>(parent);
            else return null;
        }

        public bool Insert(TParent data)
        {
            return _repository.Insert(data) > 0;
        }

        public bool Update(TParent data)
        {
            return _repository.Update(data) > 0;
        }

        public PageResult<TObj> GetPageList(int pageSize, int pageNum, int total, string filter)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                int index = filter.IndexOf('?');
                if (index > -1) //过滤某些客户端请求时候附加的时间戳
                {
                    string temp = filter.Substring(0, index);
                    filter = temp;
                }
            }
            var kvs = CommonUtil.GetFilterKeyValues(filter);
            var result0 = _repository.GetPageList(pageSize, pageNum, total, kvs);
            var list = result0.Items.Select<TParent, TObj>(_repository.ConvertTo<TObj>);
            return new PageResult<TObj>()
            {
                Total = result0.Total,
                Items = list
            };
        }
    }
}
