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

        DateTime _expire;
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime Expire
        {
            get { return _expire; }
            set { _expire = value; }
        }

        /// <summary>
        /// 标识是否已经验证
        /// </summary>
        public bool IsAuthenticated { get; set; }
        /// <summary>
        /// 标识对应的缓存的键，通过它判断是否是同一个缓存中的对象
        /// </summary>
        public string Key { get; set; }
    }
}
