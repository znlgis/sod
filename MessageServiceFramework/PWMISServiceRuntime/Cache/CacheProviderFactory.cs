using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PWMIS.EnterpriseFramework.Service.Runtime
{
    /// <summary>
    /// 缓存工厂类
    /// </summary>
    public class CacheProviderFactory
    {
        //注意：不同Cache实例，相同Key值是否有影响的问题待验证！

        private static ICacheProvider _cache;
        /// <summary>
        /// 获取缓存提供者
        /// </summary>
        /// <returns></returns>
        public static ICacheProvider GetCacheProvider()
        {
            if (_cache == null)
                _cache = new MemoryCacheProvider();
            return _cache;
        }

        public static ICacheProvider GetNewCacheProvider()
        {
            return new MemoryCacheProvider();
        }

        /// <summary>
        /// （根据配置）获取全局的缓存服务提供者，比如单独的缓存服务器
        /// </summary>
        /// <returns></returns>
        public static ICacheProvider GetGlobalCacheProvider()
        {
            string prividerName = System.Configuration.ConfigurationManager.AppSettings["GlobalCacheProvider"];
            if (prividerName == "CacheServer")
                return new CacheServerProvider();
            else
                return new MemoryCacheProvider();
        }
    }
}
