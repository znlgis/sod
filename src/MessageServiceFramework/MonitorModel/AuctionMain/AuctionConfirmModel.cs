using System;
using System.Collections.Generic;
using System.Text;

namespace TranstarAuction.Model.AuctionMain
{
   public class AuctionConfirmModel
    {
        /// <summary>
        /// 物流模式
        /// </summary>
        public int LogisticsMode
        {
            get;
            set;
        }

        /// <summary>
        /// 路线
        /// </summary>
        public int SysRouteId
        {
            get;
            set;
        }
    }
}
