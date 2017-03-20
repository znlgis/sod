using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Caching;

namespace TranstarAuction.Service.Runtime
{
    /// <summary>
    /// .Net 4.0 缓存提供者接口，采用ObjectCache，不依赖于System.Web.Cache
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// Saves the specified key.，注意，如果缓存的值存在，则插入不成功，请先移除。
        /// </summary>
        /// <typeparam name="T">缓存对象的类型</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        void Insert<T>(string key, T data);
        /// <summary>
        /// Saves the specified key.，注意，如果缓存的值存在，则插入不成功，请先移除。
        /// </summary>
        /// <typeparam name="T">缓存对象的类型</typeparam>
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
        /// <typeparam name="T">缓存对象的类型</typeparam>
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
        /// <typeparam name="T">缓存对象的类型</typeparam>
        /// <param name="cacheKey">键名称</param>
        /// <param name="setInitData">如果没有缓存对象，设置一个得到缓存对象的委托方法</param>
        /// <param name="settings">缓存依赖</param>
        /// <returns>缓存对象</returns>
        /// <example>
        /// 例如获取缓存两小时的基金主表对象：
        /// <code>
        /// ICacheProvider cache = CacheProviderFactory.GetCacheProvider();
        /// List《JJZB》 allJJZB = cache.Get《List《JJZB》》("allJJZB", () =>
        /// {
        ///    JJZB jjzb = new JJZB();
        ///    OQL q = new OQL(jjzb);
        ///    q.Select();
        ///    return EntityQuery《JJZB》.QueryList(q);
        /// }
        /// , new CacheItemPolicy() { AbsoluteExpiration = DateTimeOffset.Now.AddHours(2) }
        /// );
        /// </code>
        /// </example>
        T Get<T>(string cacheKey, Func<T> setInitData, CacheItemPolicy settings);
    }
}
