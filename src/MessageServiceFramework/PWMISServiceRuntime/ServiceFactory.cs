using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PWMIS.EnterpriseFramework.Service.Basic;
using PWMIS.EnterpriseFramework.IOC;

namespace PWMIS.EnterpriseFramework.Service.Runtime
{
    /// <summary>
    /// 服务工厂
    /// </summary>
    public class ServiceFactory
    {
        /// <summary>
        /// 根据服务名称获取服务对象实例
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static IService GetService(string serviceName)
        {
            return Unity.Instance.GetInstance<IService>(serviceName);
        }

        /// <summary>
        /// 根据服务上下文创建服务对象实例。如果是推送模式，则从缓存获取服务对象。
        /// </summary>
        /// <param name="context">当前服务上下文</param>
        /// <returns></returns>
        public static IService GetService(IServiceContext context)
        {
            ServiceRequest request = context.Request;
            if (request.RequestModel == RequestModel.GetService)
            {
                return Unity.Instance.GetInstance<IService>(request.ServiceName);
            }
            else
            {
                return context.Cache.Get<IService>(request.ServiceUrl, () => {
                    return Unity.Instance.GetInstance<IService>(request.ServiceName);
                });
            }
        }

        /// <summary>
        /// 移除缓存的服务对象实例
        /// </summary>
        /// <param name="context">当前服务上下文</param>
        public static void RemoveServiceObject(IServiceContext context)
        {
            context.Cache.Remove(context.Request.ServiceUrl);
        }
    }

    /*
     * 示例：在服务层，需要按照下面的方式定义具体的服务类 
     * 
    public class TestCalculatorService : IService
    {
        public int Add(int a, int b)
        {
            //模拟服务器延时
            System.Threading.Thread.Sleep(5000);
            return a + b;
        }

        public void ProcessRequest(IServiceContext context)
        {

        }
    }
     * */
}
