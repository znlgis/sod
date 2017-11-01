using System;
using System.Collections.Generic;
using System.Text;

namespace TranstarAuction.Model.AuctionMain
{
    public class BidRebotModel
    {
        /// <summary>
        /// 拍品id
        /// </summary>
        public long PublishId
        {
            get;
            set;
        }
        /// <summary>
        /// 出价经销商id
        /// </summary>
        public int BuyTvaId
        {
            get;
            set;
        }
        /// <summary>
        /// 出价用户id
        /// </summary>
        public int BuyTvuId
        {
            get;
            set;
        }
        /// <summary>
        /// 最高价
        /// </summary>
        public decimal MaxPrice
        {
            get;
            set;
        }
        /// <summary>
        /// 加价幅度
        /// </summary>
        public decimal PriceDegree
        {
            get;
            set;
        }
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

        /// <summary>
        /// 起拍价
        /// </summary>
        public decimal FirstPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 出价是否提醒：0,不提醒1,提醒
        /// </summary>
        public int IsBidMsgNotice
        {
            get;
            set;
        }
        /// <summary>
        /// 最高值是否提醒：0,不提醒1,提醒
        /// </summary>
        public int IsMaxPriceMsgNotice
        {
            get;
            set;
        }
        /// <summary>
        /// 机器人状态：0,无效1,有效
        /// </summary>
        public int RobotStatus
        {
            get;
            set;
        }
    }
}
