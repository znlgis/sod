using System;
using System.Collections.Generic;
using System.Text;

namespace TranstarAuction.Model
{
    public class UserLoginInfoModel
    {
        /// <summary>
        /// 登录的用户
        /// </summary>
        public UserModel User { get; set; }
        /// <summary>
        /// 登录是否成功
        /// </summary>
        public bool LoginResult { get; set; }
        /// <summary>
        /// 登录结果信息
        /// </summary>
        public string LoginResultMessage { get; set; }
        /// <summary>
        /// 服务的地址，登录成功后，客户端访问服务端的时候，全部使用该地址。
        /// </summary>
        public string ServiceUri { get; set; }
    }
}
