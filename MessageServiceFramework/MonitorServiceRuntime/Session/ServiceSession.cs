using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TranstarAuction.Service.Runtime
{
    /// <summary>
    /// 服务会话
    /// </summary>
    public class ServiceSession : IServiceSession
    {
        private System.Runtime.Caching.CacheItemPolicy cacheItemPolicy;
        private ICacheProvider cache;
        private List<string> keyList;

        private string GetCurrentKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new InvalidOperationException("键不能为空！");
            string newKey= string.Format("{0}_{1}", this.SessionID, key);
            if (!keyList.Contains(newKey))
                keyList.Add(newKey);
            return newKey;
        }
        /// <summary>
        /// 会话标示
        /// </summary>
        public string SessionID { get; private set; }

        /// <summary>
        /// 以一个会话标示，初始化本类
        /// </summary>
        /// <param name="sessionId"></param>
        public ServiceSession(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
                throw new InvalidOperationException("会话标识不能为空！");
            this.SessionID = sessionId;
            keyList = new List<string>();
            cache = CacheProviderFactory.GetCacheProvider();
            cacheItemPolicy = new System.Runtime.Caching.CacheItemPolicy()
               {
                   SlidingExpiration = new TimeSpan(0, 10, 0), //距离上次调用10分钟后过期
                   RemovedCallback = args =>
                   {
                       object obj = args.CacheItem.Value;
                       //可记录会话日志
                   }
               };
        }

        /// <summary>
        /// 获取指定的会话对象
        /// </summary>
        /// <typeparam name="T">结果类型</typeparam>
        /// <param name="key">存储会话对象的键</param>
        /// <returns>会话对象</returns>
        public T Get<T>(string key)
        {
            key = GetCurrentKey(key);
            T result = cache.Get<T>(key);
            return result;
        }

        /// <summary>
        /// 获取指定的会话对象，如果没有，则使用当前提供的对象作为会话对象的值。会话对象将在距上次调用10分钟后过期。
        /// </summary>
        /// <typeparam name="T">结果类型</typeparam>
        /// <param name="key">存储会话对象的键</param>
        /// <param name="Value">要存储的会话对象</param>
        /// <returns>返回原有的会话对象值</returns>
        public T Get<T>(string key ,T Value) 
        {
            key = GetCurrentKey(key);
            T result = cache.Get<T>(key, () =>
                {
                    return Value;
                },
                cacheItemPolicy
                );

            return result;
        }

        /// <summary>
        /// 增加会话对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="Value"></param>
        public void Set<T>(string key, T Value)
        {
            key = GetCurrentKey(key);
            cache.Remove(key);
            cache.Insert<T>(key, Value, cacheItemPolicy);
        }

        /// <summary>
        /// 清除当前的会话数据
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            key = GetCurrentKey(key);
            cache.Remove(key);
        }

        /// <summary>
        /// 清除所有的会话对象
        /// </summary>
        public void Clear()
        {
            foreach (string key in keyList)
            {
                cache.Remove(key);
            }
        }
    }

  
}
