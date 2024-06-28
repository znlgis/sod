//ActivatorUtilities :https://www.jianshu.com/p/92962ac83478
// https://www.cnblogs.com/yeqifeng2288/p/14445937.html
// https://blog.csdn.net/weixin_37648525/article/details/127942292

using SimpleDemo.Interface.Infrastructure;

namespace SimpleWebApi
{
    /// <summary>
    /// 从IOC容器获取类型实例
    /// </summary>
    public class SimpleServiceProvider: ISimpleServiceProvider
    {
        private readonly IServiceProvider _serviceProvider;
        public SimpleServiceProvider(IServiceProvider provider) {
            _serviceProvider=provider;
        }

        /// <summary>
        /// 从IOC容器中获取类型实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetService<T>() where T : notnull
        {
           return _serviceProvider.GetRequiredService<T>();
        }

        /// <summary>
        /// 从容器中获取类型T的实例，并传递一个参数给实例的构造函数。注意，构造函数的其他参数可能从容器获取。
        /// </summary>
        /// <typeparam name="TPara">构造函数的参数类型</typeparam>
        /// <typeparam name="T">要获取的实例对象类型</typeparam>
        /// <param name="paras">参数值</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public T GetService<TPara, T>(TPara paras)
        {
            if(paras == null) 
                throw new ArgumentNullException(nameof(paras));
            T service = ActivatorUtilities.CreateInstance<T>(_serviceProvider, paras);
            return service;
        }
    }
}
