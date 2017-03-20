using System;
using IBMP.AOP;
using System.ComponentModel;

namespace TranstarAuction.Model.AuctionMain
{
    public interface IAttentionCarModel: INotifyPropertyChanged
    {
        //IDataChange DataChange { set; get; }
        //int LeftSec { get; set; }

       string AttentionState { get; set; }
       /// <summary>
       /// 0:确认条件1：最高价2：出价不是最高3：竞价结束4：竞价成功5：竞价失败
       /// </summary>
       BidStatusType BidStatus { get; set; }
       int BuyerId { get;  set; }
       int BuyerVendorId { get;  set; }
       string CarColor { get;  set; }
        /// <summary>
        /// 排量
        /// </summary>
       string CarDisplacement { get;  set; }
       string CarName { get;  set; }
       int CarSourceId { get;  set; }
       string CarYear { get;  set; }
       string CityName { get;  set; }
       double CurrentAuctionMoney { get;  set; }
       int CurrentAuctionPersonNums { get;   set; }
       int DataStatus { get;  set; }
       int ExpectMoneyRecExpired { get;  set; }
       int ExpectTransferExpired { get;  set; }
       bool IsNew { get;  set; }
       int Isrelocation { get;  set; }
       int LogisticsMode { get;  set; }
       double OriginalPrice { get;   set; }
        /// <summary>
        /// 起拍价
        /// </summary>
       double Price { get;  set; }
       double PriceDegree { get;  set; }
       long PublishId { get;  set; }
       string PublishStopTime { get;  set; }
       int SellerTvaId { get;  set; }
       int SellerTvuId { get;  set; }
       string StopTime { get;  set; }
       int SysRouteId { get;  set; }
       string CurrentAuctionMoneyStr { get;  set; }
       string AuctionState { get;  set; }
       bool IsSetRebot { get; set; }
       bool IsBid { get; set; }
       int BrandId { get; set; }
       DateTime ServerTime { get; set; }
       string Unit { get; set; }
       string StartTime { get; set; }
       string CarDemand { get; set; }
       bool IsSelfCar { get; set; }
       bool IsFriend { get; set; }
       bool IsOnLinePay { get; set; }
       bool IsBlackTraID { get; set; }
       /// <summary>
       /// 车系
       /// </summary>
       string CarSerialName
       {
           get;
           set;
       }
        /// <summary>
        /// 是否可参拍
        /// </summary>
       bool IsAuction { get; set; }

        /// <summary>
        /// 竞价是否结束
        /// </summary>
       bool IsAuctionEnd { get; set; }

       /// <summary>
       /// 保留价
       /// </summary>
       double KeepPrice { get; set; }

       /// <summary>
       /// 在线或线下
       /// </summary>
       string OnlineOrOffline { get; set; }

       /// <summary>
       /// 是否支持代理支付
       /// </summary>
       bool IsAgentPay
       {
           get;
           set;
       }

       /// <summary>
       /// 是否优信拍代理过户
       /// </summary>
       bool IsAgentTransfer
       {
           get;
           set;
       }
    }



    /*
     * ==DataGridView 自动生成列的问题：
     * 
     *  public interface IAttentionCarModelBase
    {
       string AttentionState { get; set; }
       string BidStatus { get; set; }
      //.......
       
    }

    /// <summary>
    /// 具有消息通知功能的Model
    /// </summary>
   public interface IAttentionCarModel : IAttentionCarModelBase, INotifyPropertyChanged
   { }


//这样使用 IAttentionCarModel ,DataGridView 没法自动生成列
     * 
     *
     */
}
