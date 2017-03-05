using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PWMIS.EnterpriseFramework.Service.Client.Model
{
    /// <summary>
    /// 服务注册信息
    /// </summary>
    [Serializable]
    public class ServiceRegModel
    {
        private string _regServerIP = string.Empty;
        /// <summary>
        /// 服务IP:订阅者
        /// </summary>
        public string RegServerIP
        {
            get { return _regServerIP; }
            set { _regServerIP = value; }
        }

        private int _regServerPort;
        /// <summary>
        /// 服务端口:订阅者
        /// </summary>
        public int RegServerPort
        {
            get { return _regServerPort; }
            set { _regServerPort = value; }
        }

        private string _regServerDesc = string.Empty;
        /// <summary>
        /// 服务描述:订阅者
        /// </summary>
        public string RegServerDesc
        {
            get { return _regServerDesc; }
            set { _regServerDesc = value; }
        }

        private bool _isActive = false;
        /// <summary>
        /// 是否当前使用
        /// </summary>
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        private string _serverMappingIP;
        /// <summary>
        /// 映射的服务服务IP，用于防火墙或路由器映射的公网IP
        /// </summary>
        public string ServerMappingIP
        {
            get
            {
                if (string.IsNullOrEmpty(_serverMappingIP))
                    return this.RegServerIP;
                else
                    return _serverMappingIP;
            }
            set
            {
                _serverMappingIP = value;
            }
        }

        /// <summary>
        /// 获取服务地址的Uri形势
        /// </summary>
        /// <returns></returns>
        public string GetUri()
        {
            return string.Format("net.tcp://{0}:{1}", this.ServerMappingIP, this.RegServerPort);
        }
    }
}
