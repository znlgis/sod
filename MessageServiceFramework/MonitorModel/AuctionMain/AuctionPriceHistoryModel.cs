using System;
using System.Collections.Generic;
using System.Text;

namespace TranstarAuction.Model.AuctionMain
{
    public  class AuctionPriceHistoryModel
    {
        /// <summary>
        /// 出价人姓名
        /// </summary>
        public string AuctionPersonName
        {
            get;
            set;
        }
        /// <summary>
        /// 买家参拍编号
        /// </summary>
        public string BuyerCode
        {
            get;
            set;
        }
        /// <summary>
        /// 经销商id
        /// </summary>
        public int TvaId
        {
            get;
            set;
        }
        /// <summary>
        /// 用户id
        /// </summary>
        public int TvuId
        {
            get;
            set;
        }
        /// <summary>
        /// 所出价格
        /// </summary>
        public double AuctionPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 出价时间
        /// </summary>
        public DateTime AuctionPriceTime
        {
            get;
            set;
        }
    }
}
