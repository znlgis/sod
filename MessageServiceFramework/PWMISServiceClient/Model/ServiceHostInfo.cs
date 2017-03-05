using System;
using System.Collections.Generic;
using System.Text;

namespace PWMIS.EnterpriseFramework.Service.Client.Model
{
    /// <summary>
    /// 服务宿主信息
    /// </summary>
    public class ServiceHostInfo : ServiceRegModel
    {
        /// <summary>
        /// 服务宿主上活动的连接数量（长连接）
        /// </summary>
        public int ActiveConnectCount { get; set; }
        /// <summary>
        /// 当前监听器数量（快照）
        /// </summary>
        public int ListenerCount { get; set; }
        /// <summary>
        /// 监听器最大值
        /// </summary>
        public int ListenerMaxCount { get; set; }
        /// <summary>
        /// 监听器最大值发生时间
        /// </summary>
        public DateTime ListenerMaxDateTime { get; set; }
        /// <summary>
        /// 日志目录
        /// </summary>
        public string LogDirectory { get; set; }
    }
}
