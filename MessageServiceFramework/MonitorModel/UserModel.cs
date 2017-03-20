using System;
using System.Collections.Generic;
using System.Text;

namespace TranstarAuction.Model
{
    public class UserModel
    {
        public int UserID { get; set; }
        /// <summary>
        /// 经销商ID
        /// </summary>
        public int TvalID { get; set; }
        public string LoginName { get; set; }
        public string LoginPwd { get; set; }
        public string UserFullName { get; set; }
        /// <summary>
        /// 商户名称
        /// </summary>
        public string VendorName { get; set; }
    }
}
