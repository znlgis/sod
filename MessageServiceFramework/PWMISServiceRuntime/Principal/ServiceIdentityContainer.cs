using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PWMIS.EnterpriseFramework.Service.Basic;

namespace PWMIS.EnterpriseFramework.Service.Runtime.Principal
{
    /// <summary>
    /// 服务标识容器，存储所有只跟客户机器相关（不与连接相关）的凭据信息。凭据存储在全局缓存服务器中
    /// </summary>
    public class ServiceIdentityContainer
    {
        private void ResetIdentityKeyList()
        {
            _identityKeyList = null;
        }

        private List<string> _identityKeyList;
        List<string> IdentityKeyList
        {
            get
            {
                //修改成从全局缓存获取
                if (_identityKeyList == null)
                    _identityKeyList = this.Cache.Get<List<string>>(KeyListFlag, () => new List<string>());
                return _identityKeyList;
            }
            set
            {
                _identityKeyList = value;
            }
        }
        /// <summary>
        /// 获取一个新的缓存实例
        /// </summary>
        ICacheProvider Cache
        {
            get
            {
                return CacheProviderFactory.GetGlobalCacheProvider();
            }
        }
        const string KeyFlag = "_ServiceIdentity:";
        const string KeyListFlag = "_ServiceIdentity_KeyList";

        protected internal string GetKeyString(ServiceRequest request)
        {
            return KeyFlag + request.ClientIP + ":" + request.ClientIdentity;
        }

        public void Add(ServiceRequest request, ServiceIdentity identity)
        {
            string key = GetKeyString(request);
            identity.Key = key;
            //插入到全局缓存
            ResetIdentityKeyList();
            IdentityKeyList.Add(key);
            Cache.Insert<List<string>>(KeyListFlag, IdentityKeyList);
            Cache.Insert<ServiceIdentity>(key, identity,
              new System.Runtime.Caching.CacheItemPolicy() { SlidingExpiration = identity.Expire });
        }

        public ServiceIdentity Get(ServiceRequest request)
        {
            string key = GetKeyString(request);
            return this.Cache.Get<ServiceIdentity>(key);
        }

        /// <summary>
        /// 根据已有的标识，寻找容器中对应的标识对象
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public ServiceIdentity Find(ServiceIdentity identity)
        {
            List<string> keyList = this.IdentityKeyList;
            foreach (string key in keyList.ToArray())
            {
                ServiceIdentity item = this.Cache.Get<ServiceIdentity>(key);
                if (item != null)
                {
                    if (item.Id == identity.Id && item.Name == identity.Name)
                    {
                        return item;
                    }
                }
                else
                {
                    //从全局缓存移除
                    ResetIdentityKeyList();
                    IdentityKeyList.Remove(key);
                    Cache.Insert<List<string>>(KeyListFlag, IdentityKeyList);
                }
            }
            return null;
        }

        /// <summary>
        /// 获取系统中的所有服务凭据
        /// </summary>
        /// <returns></returns>
        public List<ServiceIdentity> GetAllIdentitys()
        {
            List<ServiceIdentity> list = new List<ServiceIdentity>();
            List<string> keyList = this.IdentityKeyList;
            foreach (string key in keyList.ToArray())
            {
                ServiceIdentity item = this.Cache.Get<ServiceIdentity>(key);
                if (item != null)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        /// <summary>
        /// 从容器中移除请求对应的标识
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool Remove(ServiceRequest request)
        {
            string key = GetKeyString(request);
            Cache.Remove(key);

            ResetIdentityKeyList();
            IdentityKeyList.Remove(key);//从全局缓存移除
            Cache.Insert<List<string>>(KeyListFlag, IdentityKeyList);
            return true;
        }

        /// <summary>
        /// 移除指定的标识
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public bool Remove(ServiceIdentity identity)
        {
            ServiceIdentity item = this.Cache.Get<ServiceIdentity>(identity.Key);
            if (item != null && item.Key == identity.Key)
            {
                this.Cache.Remove(identity.Key);

                ResetIdentityKeyList();
                IdentityKeyList.Remove(identity.Key);//从全局缓存移除
                Cache.Insert<List<string>>(KeyListFlag, IdentityKeyList);
                return true;
            }
            return false;
        }

        #region ServiceIdentityContainer 的单例实现
        private static readonly object _syncLock = new object();//线程同步锁；
        private static ServiceIdentityContainer _instance;
        /// <summary>
        /// 返回 MessageCenter 的唯一实例；
        /// </summary>
        public static ServiceIdentityContainer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ServiceIdentityContainer();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 保证单例的私有构造函数；
        /// </summary>
        private ServiceIdentityContainer() { }

        #endregion

    }
}
