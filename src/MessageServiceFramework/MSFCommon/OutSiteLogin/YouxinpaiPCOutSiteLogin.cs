using System;
using System.Collections.Generic;
using System.Text;

namespace TranstarAuction.Common.OutSiteLogin
{
    /// <summary>
    /// 优信拍ＰＣ客户端登录到网站的类，需要设置　LoginOKPage　属性
    /// </summary>
    public class YouxinpaiPCOutSiteLogin : OutSiteLoginBase
    {
        private string transtarUserName;
         /// <summary>
        /// 以一个登录的用户标示初始化本类
        /// </summary>
        /// <param name="identityName"></param>
        public YouxinpaiPCOutSiteLogin(string identityName)
            : base(identityName)
        {
            this.transtarUserName = identityName;
        }

        /// <summary>
        /// 获取车商通/优信拍用户名
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        protected override string GetTranstarUser(object para)
        {
            return this.transtarUserName;
        }


        /// <summary>
        /// 检查外部用户是否在本地存在，如果存在，则返回本地对照的登录用户名
        /// </summary>
        /// <param name="transtarUser">外部用户名（如车商通）</param>
        /// <returns>如果不存在，返回空值</returns>
        protected override string GetLocalUserFromDB(string userName)
        {
            return userName;
        }
    }
}
