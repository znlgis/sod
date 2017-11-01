#region << 版 本 注 释 >>
/*
 * ========================================================================
 * Copyright(c) 2011-2012 BitAuto.com, All Rights Reserved.
 * ========================================================================
 *  
 * 【服务注册】
 *  
 * 作者：[周燕龙]   时间：2012-01-16  
 * 文件名：RegService
 * 版本：V1.0.0
 * 
 * 修改者：[周燕龙]   时间：      
 * 修改说明：添加相关注释
 * ========================================================================
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PWMIS.EnterpriseFramework.Service.Runtime;
using PWMIS.EnterpriseFramework.Service.Client.Model;


namespace PWMIS.EnterpriseFramework.Service.Group
{
    /// <summary>
    /// 注册服务
    /// </summary>
    public class RegService : ServiceBase
    {
        /// <summary>
        /// 服务注册
        /// </summary>
        /// <param name="curModel"></param>
        /// <returns></returns>
        public bool Register(ServiceRegModel curModel)
        {
            RegServiceContainer container = new RegServiceContainer();
            container.CurrentContext = this.CurrentContext;
            return container.RegService(curModel);
        }

        /// <summary>
        /// 获取当前可用的服务器列表
        /// </summary>
        /// <returns>结果肯定不为空</returns>
        public List<ServiceRegModel> GetActiveServerList()
        {
            RegServiceContainer container = new RegServiceContainer();
            container.CurrentContext = this.CurrentContext;
            return container.GetActiveServerList();
        }

        /// <summary>
        /// 获取当前的服务器状态信息（建议使用订阅模式）
        /// </summary>
        /// <returns></returns>
        public ServiceHostInfo GetCurrentHostInfo()
        {
            base.CurrentContext.BatchInterval = 10000;
            //string key = base.CurrentContext.Host.GetUri() + "_HostInfo";
            //ServiceHostInfo serviceHostInfo = base.GlobalCache.Get<ServiceHostInfo>(key);
            return base.CurrentContext.Host;
        }

    }
}
