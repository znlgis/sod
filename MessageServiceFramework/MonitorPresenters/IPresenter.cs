using PWMIS.EnterpriseFramework.Service.Client;
using System;
using System.Collections.Generic;
 


namespace TranstarAuction.Presenters
{
    /// <summary>
    /// 界面-逻辑 主持人接口
    /// </summary>
    public interface IPresenter
    {
        /// <summary>
        /// 服务代理类
        /// </summary>
        Proxy ServiceProxy { get; }       
    }
}
