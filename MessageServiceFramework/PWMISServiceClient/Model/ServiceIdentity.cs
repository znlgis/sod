using System;
using System.Collections.Generic;
using System.Text;

namespace PWMIS.EnterpriseFramework.Service.Runtime.Principal
{
    /// <summary>
    /// 服务相关的用户标识
    /// </summary>
    public class ServiceIdentity
    {
        public ServiceIdentity()
        {
            this.Expire = new TimeSpan(0, 3, 0);
            this.CreateTime = DateTime.Now;
        }

        /// <summary>
        /// 获取凭据的创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get;
            private set;
        }

        int _userId;
        /// <summary>
        /// 标识编号
        /// </summary>
        public int Id
        {
            get { return _userId; }
            set { _userId = value; }
        }

        string _userName;
        /// <summary>
        /// 标识名
        /// </summary>
        public string Name
        {
            get { return _userName; }
            set { _userName = value; }
        }


        /// <summary>
        /// 过期时间，默认时间 3分钟
        /// </summary>
        public TimeSpan Expire { get; set; }

        /// <summary>
        /// 标识是否已经验证
        /// </summary>
        public bool IsAuthenticated { get; set; }
        /// <summary>
        /// 标识对应的缓存的键，通过它判断是否是同一个缓存中的对象
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 标识所在的服务基地址（可用于检查集群负载情况）
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// 客户的硬件标识
        /// </summary>
        public string HardwareIdentity { get; set; }

        /// <summary>
        /// 注册连接的时候的自定义数据
        /// </summary>
        public string UserData { get; set; }

    }
}
