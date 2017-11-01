using System;
using System.Collections.Generic;
using System.Text;

namespace TranstarAuction.Model.AuctionMain
{
   public  class AttentionCarBaseInfoModel
    {
        /// <summary>
        /// 车编号
        /// </summary>
        public int CarSourceId
        {
            get;
            set;
        }

        /// <summary>
        /// 发布编号
        /// </summary>
        public long PublishId
        {
            get;
            set;
        }

       /// <summary>
       /// 车名称
       /// </summary>
        public string CarName
        {
            get;
            set;
        }

        /// <summary>
        /// 当前出价
        /// </summary>
        public decimal CurrentAuctionMoney
        {
            get;
            set;
        }

        

        /// <summary>
        /// 当前竞价人数
        /// </summary>
        public int CurrentAuctionPersonNums
        {
            get;
            set;
        }

       /// <summary>
       /// 城市名称
       /// </summary>
        public string CityName
        {
            get;
            set;
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string StopTime
        {
            get;
            set;
        }

       /// <summary>
       /// 起拍价
       /// </summary>
        public decimal InitAuctionPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 付款条件
        /// </summary>
        public int ExpectMoneyRecExpired
        {
            get;
            set;
        }
        /// <summary>
        /// 过户条件
        /// </summary>
        public int ExpectTransferExpired
        {
            get;
            set;
        }
        /// <summary>
        /// 是否需要外迁 0：默认；1：外迁；2：不外迁
        /// </summary>
        public int Isrelocation
        {
            get;
            set;
        }

        /// <summary>
        /// 卖家经销商编号
        /// </summary>
        public int SellerTvaId
        {
            get;
            set;
        }

        /// <summary>
        /// 卖家编号
        /// </summary>
        public int SellerTvuId
        {
            get;
            set;
        }

        /// <summary>
        /// 买家编号
        /// </summary>
        public int BuyerId
        {
            get;
            set;
        }

        /// <summary>
        /// 买家编号
        /// </summary>
        public int BuyerVendorId
        {
            get;
            set;
        }

        /// <summary>
        /// 车排量
        /// </summary>
        public string CarDisplacement
        {
            get;
            set;
        }

       /// <summary>
       /// 当前竞价字符形式
       /// </summary>
        public string CurrentAuctionMoneyStr
        {
            get;
            set;
        }


       /// <summary>
       /// 竞价状态
       /// </summary>
        public string BidStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 关注
        /// </summary>
        public string AttentionState
        {
            get;
            set;
        }

       
    }
}
