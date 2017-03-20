using System;
using IBMP.AOP;
using System.ComponentModel;
namespace TranstarAuction.Model
{
    public  interface IAuctionHallDataModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 车编号
        /// </summary>
          int CarSourceId
        {
            get;
            set;
        }
        /// <summary>
        /// 发布编号
        /// </summary>
          long PublishId
        {
            get;
            set;
        }
        /// <summary>
        /// 关注
        /// </summary>
          string AttentionState
        {
            get;
            set;
        }
        /// <summary>
        /// 城市
        /// </summary>
          string City
        {
            get;
            set;
        }
        /// <summary>
        /// 车型及颜色
        /// </summary>
          string CarTypeAndColor
        {
            get;
            set;
        }
        /// <summary>
        /// 车型
        /// </summary>
          string CarType
        {
            get;
            set;
        }
        /// <summary>
        /// 车系
        /// </summary>
          string CarSerialName
          {
              get;
              set;
          }
        /// <summary>
        /// 颜色
        /// </summary>
          string CarColor
        {
            get;
            set;
        }
        /// <summary>
        /// 里程数
        /// </summary>
          double Mileage
        {
            get;
            set;
        }
        ///// <summary>
        ///// 里程数单位
        ///// </summary>
        //  string MileageUnit
        //{
        //    get;
        //    set;
        //}
        /// <summary>
        /// 年限
        /// </summary>
          string Years
        {
            get;
            set;
        }
        /// <summary>
        /// 当前价(万)
        /// </summary>
          double CurrentPrices
        {
            get;
            set;
        }
        /// <summary>
        /// 起拍价
        /// </summary>
          string StartPrices
        {
            get;
            set;
        }
        /// <summary>
        /// 拍卖状态
        /// </summary>
          string AuctionStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 结束时间
        /// </summary>
          string StopTime
        {
            get;
            set;
        }
        /// <summary>
        /// 竞拍人数
        /// </summary>
          int BidCount
        {
            get;
            set;
        }
        /// <summary>
        /// 起拍价
        /// </summary>
          double InitAuctionPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 付款条件
        /// </summary>
          int ExpectMoneyRecExpired
        {
            get;
            set;
        }
        /// <summary>
        /// 过户条件
        /// </summary>
          int ExpectTransferExpired
        {
            get;
            set;
        }
        /// <summary>
        /// 是否需要外迁 0：默认；1：外迁；2：不外迁
        /// </summary>
          int Isrelocation
        {
            get;
            set;
        }
        /// <summary>
        /// 卖家经销商编号
        /// </summary>
          int SellerTvaId
        {
            get;
            set;
        }

        /// <summary>
        /// 卖家编号
        /// </summary>
          int SellerTvuId
        {
            get;
            set;
        }
        /// <summary>
        /// 车排量
        /// </summary>
          string CarDisplacement
        {
            get;
            set;
        }
        /// <summary>
        /// 0:确认条件1：最高价2：出价不是最高3：竞价结束4：竞价成功5：竞价失败
        /// </summary>
          BidStatusType BidStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 单位
        /// </summary>
          string Unit
        {
            get;
            set;
        }
        /// <summary>
        /// 拍卖结束时间
        /// </summary>
          string PublishStopTime
        {
            get;
            set;
        }
        /// <summary>
        /// 0:默认1：更新2：删除3：增加
        /// </summary>
          int DataStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 是否设置机器人
        /// </summary>

          bool IsSetRebot
        {
            get;
            set;
        }
        /// <summary>
        /// 是否出价 
        /// </summary>
          bool IsBid
        {
            get;
            set;
        }
        /// <summary>
        /// 买家编号
        /// </summary>
          int BuyerId
        {
            get;
            set;
        }

        /// <summary>
        /// 买家编号
        /// </summary>
          int BuyerVendorId
        {
            get;
            set;
        }
        /// <summary>
        /// 品牌id
        /// </summary>
          int BrandId
        {
            set;
            get;
        }
        /// <summary>
        /// 出价开始时间
        /// </summary>
          string StartTime
        {
            set;
            get;
        }
        /// <summary>
        /// 标注数据源时段
        /// </summary>
          DataSourceTime Type
        {
            set;
            get;
        }
        /// <summary>
        /// 提车要求
        /// </summary>
          string CarDemand
        {
            set;
            get;
        }
        /// <summary>
        /// 出价结束时间
        /// </summary>
          DateTime PriceEndTime
          {
              set;
              get;
          }
          bool IsSelfCar { get; set; }
          bool IsFriend { get; set; }
        /// <summary>
        /// 是否支持在线付款
        /// </summary>
          bool IsOnLinePay { get; set; }
        /// <summary>
        /// 当前记录的服务器当前时间
        /// </summary>
          DateTime ServerTime { get; set; }
        /// <summary>
        /// 当前拍品的经销商是否是当前操作人所在单位的黑名单
        /// </summary>
          bool IsBlackTraID { get; set; }
        /// <summary>
        /// 是否可参拍
        /// </summary>
          bool IsAuction { get; set; }

        /// <summary>
        /// 是否可见
        /// </summary>
          bool IsShow { get; set; }

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
}
