using PWMIS.EnterpriseFramework.Service.Runtime.Principal;
using System;
using System.Collections.Generic;
using System.Text;

namespace TranstarAuction.Service.Runtime.Principal
{
    /// <summary>
    /// 服务权限类
    /// </summary>
    public class ServicePrincipal
    {
        public ServiceIdentity Identity { get; set; }

        public bool IsInRole(string role)
        {
            return false;
        }


    }
}
