using Sod.Pwmis.com.Common;
using Sod.Pwmis.com.Interface;
using Sod.Pwmis.com.Interface.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;

namespace Sod.Pwmis.com.Service
{
    public class ServiceBase<TParent,TObj, Tid> : IServiceBase<TParent,TObj, Tid> 
        where TParent : class, IIdentityProperty<Tid>
        where TObj : class, TParent, new()
    {
        private IRepository<TParent, Tid> _repository;
        private ISimpleServiceProvider _serviceProvider;

        public ServiceBase(IRepository<TParent, Tid> repository,ISimpleServiceProvider serviceProvider)
        {
            Type type = typeof(Tid);
            if(type != typeof(int) && type != typeof(long) && type != typeof(string) && type != typeof(Guid)) 
            {
                throw new TypeAccessException("Tid类型只能是int,long,string,Guid中的一种。");
            }
            _repository = repository;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 服务提供程序，从DI容器中提供指定的服务。
        /// </summary>
        public ISimpleServiceProvider ServiceProvider { get { return _serviceProvider; } }
        protected IRepository<TParent, Tid> CurrentRepository { get { return _repository; } }

        public bool Delete(Tid id)
        {
            return _repository.Delete(id) > 0;
        }

        public IEnumerable<TObj> GetAll()
        {
            var list = _repository.GetAll();
            var result= list.Select<TParent, TObj>(_repository.ConvertTo<TObj>);
            return result;
        }

        public TObj? GetById(Tid id)
        {
            TParent? parent= _repository.GetById(id);
            if (parent != null)
                return _repository.ConvertTo<TObj>(parent);
            else return null;
        }

        public TObj? GetByPropertyValue(TObj obj, Expression<Func<TObj, object>> expression)
        {
            if (obj == null)
                throw new ArgumentNullException("para obj is not null.");
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
                throw new ArgumentException("Expression is not a member access expression.", nameof(expression));

            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
                throw new ArgumentException("Expression is not a property.", nameof(expression));
            string propName= property.Name;
            object? value= property.GetValue(obj);
            TParent? parent = _repository.GetByPropertyValue(propName, value);
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

        /// <summary>
        /// 处理分页查询条件的可选方法。父类此方法无逻辑，需要在子类中重写实现自定义的处理逻辑。
        /// </summary>
        /// <param name="condition">已经初步处理后查询条件键值对列表</param>
        protected virtual void ProcessPageQeuryCondition(List<KeyValuePair<string, string>> condition) { }

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
            var kvs= CommonUtil.GetFilterKeyValues(filter);
            ProcessPageQeuryCondition(kvs);
            var result0= _repository.GetPageList(pageSize, pageNum, total, kvs);
            var list = result0.Items.Select<TParent, TObj>(_repository.ConvertTo<TObj>);
            return new PageResult<TObj>()
            {
                Total = result0.Total,
                Items = list
            };
        }

        public PageResult<TObj> GetPageList(int pageSize, int pageNum, int total, List<KeyValuePair<string, string>> keyValuePairs)
        {
            ProcessPageQeuryCondition(keyValuePairs);
            var result0 = _repository.GetPageList(pageSize, pageNum, total, keyValuePairs);
            var list = result0.Items.Select<TParent, TObj>(_repository.ConvertTo<TObj>);
            return new PageResult<TObj>()
            {
                Total = result0.Total,
                Items = list
            };
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="tids">要删除数据的ID数组</param>
        /// <returns>删除数据的条数</returns>
        public int BatchDelete(Tid[] tids)
        {
            return _repository.BatchDelete(tids);
        }
    }
}
