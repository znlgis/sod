
#region << 版 本 注 释 >>
/*
 * ========================================================================
 * Copyright(c) 2008-2017 拼威&敏思--PWMIS, All Rights Reserved.
 * ========================================================================
 *  
 * 内存数据库，参见 http://www.cnblogs.com/bluedoctor/archive/2011/09/20/2182722.html
 *  
 *  
 * 作者：转自网上     时间：2011/9/2 15:19:30
 * 版本：V2.0.0
 * 
 * 修改者：         时间： 2013.5.15              
 * 修改说明：使用.NET 4.0 线程安全的集合
 * ========================================================================
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;

namespace PWMIS.MemoryStorage
{
    /// <summary>
    /// .Net 4.0 缓存提供者接口，采用ObjectCache，不依赖于System.Web.Cache
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// Saves the specified key.
        /// </summary>
        /// <typeparam name="T">缓存对象的类型，如果是值类型必须使用对应的“可空类型”</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        void Insert<T>(string key, T data);
        /// <summary>
        /// Saves the specified key.
        /// </summary>
        /// <typeparam name="T">缓存对象的类型，如果是值类型必须使用对应的“可空类型”</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="date">The date.</param>
        /// <param name="settings">The settings.</param>
        void Insert<T>(string key, T data, CacheItemPolicy settings);
        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        T Get<T>(string key);
        /// <summary>
        /// Gets the specified cache key.
        /// </summary>
        /// <typeparam name="T">缓存对象的类型，如果是值类型必须使用对应的“可空类型”</typeparam>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="getData">The get data.</param>
        /// <returns></returns>
        T Get<T>(string cacheKey, Func<T> getData);
        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        void Remove(string key);
        /// <summary>
        /// Clears this instance.
        /// </summary>
        void Clear();


        /// <summary>
        /// 获取指定的缓存对象
        /// </summary>
        /// <typeparam name="T">缓存对象的类型，如果是值类型必须使用对应的“可空类型”</typeparam>
        /// <param name="cacheKey">键名称</param>
        /// <param name="getData">获取缓存对象的委托方法</param>
        /// <param name="settings">缓存依赖</param>
        /// <returns>缓存对象</returns>
        /// <example>
        /// 例如获取缓存两小时的基金主表对象：
        /// <code>
        /// ICacheProvider cache = CacheProviderFactory.GetCacheProvider();
        /// List《JJZB》 allJJZB = cache.Get《List《JJZB》》("allJJZB", () =>
        /// {
        ///    JJZB jjzb = new JJZB();
        ///    OQL q = new OQL(jjzb);
        ///    q.Select();
        ///    return EntityQuery《JJZB》.QueryList(q);
        /// }
        /// , new CacheItemPolicy() { AbsoluteExpiration = DateTimeOffset.Now.AddHours(2) }
        /// );
        /// </code>
        /// </example>
        T Get<T>(string cacheKey, Func<T> getData, CacheItemPolicy settings);
    }

    public class MemoryCacheProvider : ICacheProvider
    {
        private ObjectCache _mcache;
        private ObjectCache MCache
        {
            get
            {
                if (_mcache == null)
                    _mcache = MemoryCache.Default;
                return _mcache;
            }
        }
        public MemoryCacheProvider()
        {
        }

        #region ICacheProvider 成员

        public void Insert<T>(string key, T data)
        {
            MCache.Add(key, data, null);
        }

        public void Insert<T>(string key, T data, CacheItemPolicy settings)
        {
            MCache.Add(key, data, settings);
        }

        public T Get<T>(string key)
        {
            return (T)MCache[key];
        }

        public T Get<T>(string cacheKey, Func<T> getData)
        {
            T tdata = Get<T>(cacheKey);
            if (tdata == null)
            {
                tdata = getData();
                Insert(cacheKey, tdata);
            }
            return tdata;
        }

        public void Remove(string key)
        {
            MCache.Remove(key);
        }

        public void Clear()
        {
            _mcache = MemoryCache.Default;
        }

        public T Get<T>(string cacheKey, Func<T> getData, CacheItemPolicy settings)
        {
            T tdata = Get<T>(cacheKey);
            if (tdata == null)
            {
                tdata = getData();
                Insert(cacheKey, tdata, settings);
            }
            return tdata;
        }

        #endregion
    }

    /// <summary>
    /// 缓存工厂类
    /// </summary>
    public class CacheProviderFactory
    {
        /// <summary>
        /// 获取缓存提供者
        /// </summary>
        /// <returns></returns>
        public static ICacheProvider GetCacheProvider()
        {
            return new MemoryCacheProvider();
        }
    }
}