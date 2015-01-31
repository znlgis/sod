using System;
using System.Collections.Generic;
using System.Text;

namespace PWMIS.Core
{
    /// <summary>
    /// 内存缓存类
    /// </summary>
    public class MemoryCache<T>
    {
        /*
         * 在一定的时间间隔外，如果写入缓存，则执行缓存检查
         */ 
        private IDictionary<string, CacheValueItem<T>> cache;
        private object sync_lock = new object();
        private static object instance_lock = new object();
        private DateTime lastCheckTime;

        public MemoryCache()
        {
            cache = new HotNameValue<CacheValueItem<T>>();
            //cache = new Dictionary<string,CacheItem<T>>();
            lastCheckTime = DateTime.Now;
        }

        static MemoryCache<T> instance = null;
        /// <summary>
        /// 获取当前类型的默认实例
        /// </summary>
        public static MemoryCache<T> Default
        {
            get {
                if (instance == null)
                {
                    lock (instance_lock)
                    {
                        if (instance == null)
                        {
                            MemoryCache<T> temp = new MemoryCache<T>();
                            instance = temp;
                        }
                    }
                }
                return instance;
            }
        }

        public void Add(string name,T Value)
        {
            CheckCache();
            cache.Add(name, new CacheValueItem<T>( Value));
        }

        public void Add(CacheItem<T> item)
        {
            cache.Add(item.Name, item);
        }

        public T Get(string name)
        {
            CacheValueItem<T> data= cache[name];
            if (data != null)
                return data.Value;
            else
                return default(T);
        }

        public T Get<TPara>(string name, PWMIS.Common.MyFunc<TPara, T> initData,TPara para)
        {
            CacheValueItem<T> data ;
            cache.TryGetValue(name, out data);

            if (data != null)
            {
                return data.Value;
            }
            else
            {
                T Value = initData(para);
                Add(name, Value);
                return Value;
            }
        }

        public bool Remove(string name)
        {
           return cache.Remove(name);
        }

        /// <summary>
        /// 清理缓存
        /// </summary>
        private void CleanUp()
        {
            List<string> expireKey = new List<string>();
            var hotCache = cache as HotNameValue<CacheValueItem<T>>;
            foreach (var item in hotCache)
            {
                if (item.Value != null)
                {
                    if (item.Value.IsAbsoluteExpire && item.Value.ExpireTime < DateTime.Now)
                        expireKey.Add(item.Key);
                    else if (item.Value.HitCount < 0)
                        expireKey.Add(item.Key);
                    else
                        item.Value.SubHit();
                }
            }
            foreach (string key in expireKey)
                hotCache.Remove(key);
        }
        /// <summary>
        /// 检查缓存是否符合情理条件，如果符合则出发清理操作
        /// </summary>
        private void CheckCache()
        {
            if (DateTime.Now.Subtract(lastCheckTime).TotalMinutes > 5)
            {
                lock (sync_lock)
                {
                    if (DateTime.Now.Subtract(lastCheckTime).TotalMinutes > 5)
                    {
                        CleanUp();
                        lastCheckTime = DateTime.Now;
                    }
                }
            }
        }
    }

   public class CacheValueItem<T>
    {
       
        /// <summary>
        /// 缓存的值 
        /// </summary>
        public T Value { get; set; }
        /// <summary>
        /// 缓存创建时间
        /// </summary>
        public DateTime CreateTime { get; private set; }
        /// <summary>
        /// 缓存过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }
        /// <summary>
        /// 是否绝对过期。如果设置，则在设定的过期时间后缓存项被清除
        /// </summary>
        public bool IsAbsoluteExpire { get; private set; }
        /// <summary>
        /// 缓存被命中的次数
        /// </summary>
        public int HitCount { get; private set; }

        public void SubHit()
        {
            HitCount--;
        }
        /// <summary>
        /// 新建一个缓存，制定名字和值
        /// </summary>
        /// <param name="cacheValue"></param>
        public CacheValueItem(T cacheValue)
        {
            //this.Name = cacheName;
            this.Value = cacheValue;
            this.CreateTime = DateTime.Now;
        }

        internal CacheValueItem()
        { 
         
        }
    }

    public class CacheItem<T> : CacheValueItem<T>
    {
        /// <summary>
        /// 缓存的名字
        /// </summary>
        public string Name { get; private set; }

        public CacheItem(string cacheName, T cacheValue):base(cacheValue)
        {
            this.Name = cacheName;
        }

    }
}
