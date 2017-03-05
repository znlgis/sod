using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PWMIS.EnterpriseFramework.Service.Runtime;
using PWMIS.EnterpriseFramework.Service.Client.Model;


namespace PWMIS.EnterpriseFramework.Service.Group
{
    public class RegServiceContainer : ServiceBase
    {
        /// <summary>
        /// 注册集群服务节点
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool RegService(ServiceRegModel model)
        {
            ICacheProvider cache = base.GlobalCache;
            if (cache is CacheServerProvider)
            {
                //根据Key获取全局缓存中Key对应的列表
                List<ServiceRegModel> curCacheList = cache.Get<List<ServiceRegModel>>("ServiceRegModels");
                //如果列表为空，表明缓存不存在，当前为首次请求
                if (curCacheList == null)
                {
                    curCacheList = new List<ServiceRegModel>();
                    curCacheList.Add(model);
                    cache.Insert<List<ServiceRegModel>>("ServiceRegModels", curCacheList);
                    return true;
                }
                else//若列表不为空，表明Cache存在，查找Cache中是否存在当前请求
                {
                    //若存在则返回
                    if (curCacheList.Exists(p => p.RegServerDesc == model.RegServerDesc))
                    {
                        return true;
                    }
                    else
                    {
                        //不存在，加入
                        curCacheList.Add(model);
                        // base.GlobalCache.Remove("ServiceRegModels");Insert方法已经实现，无需Remove
                        cache.Insert<List<ServiceRegModel>>("ServiceRegModels", curCacheList);
                        return true;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取当前可用的服务器列表
        /// </summary>
        /// <returns></returns>
        public List<ServiceRegModel> GetActiveServerList()
        {
            ICacheProvider cache = base.GlobalCache;
            List<ServiceRegModel> curCacheList = null;

            if (cache is CacheServerProvider)
            {
                //根据Key获取全局缓存中Key对应的列表
                curCacheList = cache.Get<List<ServiceRegModel>>("ServiceRegModels");
                //如果列表为空，表明缓存不存在，当前为首次请求
                if (curCacheList == null)
                {
                    curCacheList = new List<ServiceRegModel>();
                    curCacheList.Add(this.CurrentContext.Host);
                    return curCacheList;
                }
                else
                {
                    //这里需要检查集群中的节点是否有效
                    bool change = false;
                    List<ServiceRegModel> hostList = new List<ServiceRegModel>();
                    foreach (ServiceRegModel item in curCacheList.ToArray())
                    {
                        string key = item.GetUri() + "_HostInfo";
                        ServiceHostInfo serviceHostInfo = cache.Get<ServiceHostInfo>(key);
                        if (serviceHostInfo == null)
                        {
                            change = true;
                            curCacheList.Remove(item);
                        }
                        else
                        {
                            hostList.Add(serviceHostInfo);
                        }
                    }
                    //经过检查后，可能其它的服务都已经挂机
                    if (curCacheList.Count == 0)
                    {
                        change = true;
                        curCacheList.Add(this.CurrentContext.Host);
                    }
                    if (change)
                    {
                        cache.Insert<List<ServiceRegModel>>("ServiceRegModels", curCacheList);
                    }
                    return hostList;
                }
            }
            else
            {
                //使用本地配置
                curCacheList = new List<ServiceRegModel>();
                curCacheList.Add(this.CurrentContext.Host);
                return curCacheList;
            }
        }

        #region 单列

        private static readonly object _instanceLock = new object();//线程同步锁；
        private static RegServiceContainer _instance;

        /// <summary>
        /// 返回注册实例
        /// </summary>
        public static RegServiceContainer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_instanceLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new RegServiceContainer();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

    }
}
