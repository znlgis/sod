using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PWMIS.EnterpriseFramework.Service.Client.Model;
using PWMIS.EnterpriseFramework.Service.Runtime;

namespace PWMIS.EnterpriseFramework.Service.Group
{
    /// <summary>
    /// （集群）调度服务
    /// </summary>
    public class DispatchService : ServiceBase
    {
        private RegService regservic = new RegService();


        public DispatchService() { }

        public DispatchService(IServiceContext context)
        {
            base.CurrentContext = context;
        }

        /// <summary>
        /// 执行调度
        /// </summary>
        /// <returns></returns>
        public ServiceRegModel DoDispatch()
        {
            regservic.CurrentContext = this.CurrentContext;
            //从注册的服务列表里面，根据服务中的长连接数量决定选取那个服务
            ServiceRegModel returnValue = null;
            ICacheProvider cache = CacheProviderFactory.GetGlobalCacheProvider();
            var list = regservic.GetActiveServerList();//已经确保GetActiveServerList 不为空
            //if (list != null)
            //{
            //    var regModel = list.Select(o => new { Count = cache.Get<int>(o.GetUri() + "_ListenerCount"), RegModel = o })
            //        .OrderBy(o => o.Count).FirstOrDefault();
            //    if (regModel != null)
            //        returnValue = regModel.RegModel;

            //}
            int minCount = int.MaxValue;
            foreach (ServiceRegModel item in list)
            {
                ServiceHostInfo host = item as ServiceHostInfo;
                if (host != null && minCount > host.ListenerCount)
                {
                    minCount = host.ListenerCount;
                    returnValue = item;
                }
            }
            if (returnValue == null)
                returnValue = this.CurrentContext.Host;
            return returnValue;
        }

        /// <summary>
        /// 获取服务集群中所有的服务宿主信息（供诊断使用）
        /// </summary>
        /// <returns></returns>
        public List<ServiceHostInfo> ActiveConnected()
        {
            List<ServiceHostInfo> result = new List<ServiceHostInfo>();
            regservic.CurrentContext = this.CurrentContext;
            var list = regservic.GetActiveServerList();
            foreach (var item in list)
            {
                ServiceHostInfo host = item as ServiceHostInfo;
                if (host != null)
                {
                    result.Add(host);
                }
            }
            return result;
        }

    }
}
