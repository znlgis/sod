using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PWMIS.EnterpriseFramework.Service.Runtime.Principal
{
    /// <summary>
    /// 服务身份验证类
    /// </summary>
    public class ServiceAuthentication
    {
        IServiceContext currentContext;

        /// <summary>
        /// 以一个服务上下文初始化本类
        /// </summary>
        /// <param name="context"></param>
        public ServiceAuthentication(IServiceContext context)
        {
            this.currentContext = context;
        }

        /// <summary>
        /// （在当前会后中）设置用户凭据。（凭据保存到系统缓存中，供相同的客户机各个连接使用）
        /// </summary>
        /// <param name="userId">用户标识</param>
        /// <param name="userName">用户名</param>
        /// <param name="expire">凭据要过期的绝对时间</param>
        public void Authenticate(int userId, string userName, TimeSpan expire)
        {
            ServiceIdentity user = new ServiceIdentity();
            user.Expire = expire;
            user.Id = userId;
            user.Name = userName;

            ServiceIdentityContainer.Instance.Add(this.currentContext.Request, user);
        }

        /// <summary>
        /// （在当前会后中）设置用户凭据。（凭据保存到系统缓存中，供相同的客户机各个连接使用）
        /// </summary>
        /// <param name="user"></param>
        public void Authenticate(ServiceIdentity user)
        {
            ServiceIdentityContainer.Instance.Add(this.currentContext.Request, user);
        }

        /// <summary>
        /// 获取标识
        /// </summary>
        /// <returns></returns>
        public ServiceIdentity GetIdentity()
        {
            return ServiceIdentityContainer.Instance.Get(this.currentContext.Request);
        }

        /// <summary>
        /// 根据指定的标识，查找服务标识容器是否存在对应的标识对象，如果存在，则返回容器中的标识对象
        /// </summary>
        /// <param name="identity"></param>
        /// <returns>如果未找到，返回空</returns>
        public ServiceIdentity FindIdentity(ServiceIdentity identity)
        {
            identity.Key = ServiceIdentityContainer.Instance.GetKeyString(this.currentContext.Request);
            return ServiceIdentityContainer.Instance.Find(identity);
        }

        /// <summary>
        /// 取消当前凭据
        /// </summary>
        public bool SignOut()
        {
            return ServiceIdentityContainer.Instance.Remove(this.currentContext.Request);
        }

        /// <summary>
        /// 取消指定的凭据
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public bool SignOut(ServiceIdentity identity)
        {
            return ServiceIdentityContainer.Instance.Remove(identity);
        }
    }
}
