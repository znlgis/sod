using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using IBMP.AOP;
namespace TranstarAuction.Model
{
    /// <summary>
    /// 拍卖大厅数据模型
    /// </summary>
    public class AuctionHallDataModel : ICloneable, IAuctionHallDataModel
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
        /// 关注
        /// </summary>
        public string AttentionState
        {
            get;
            set;
        }
        /// <summary>
        /// 城市
        /// </summary>
        public string City
        {
            get;
            set;
        }
     
        /// <summary>
        /// 车型及颜色
        /// </summary>
        public string CarTypeAndColor
        {
            get;
            set;
        }
        ///// <summary>
        ///// 主品牌名称
        ///// </summary>
        //public string MasterBrandName
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 车系名称
        /// </summary>
        public string CarSerialName
        {
            get;
            set;
        }
        /// <summary>
        /// 车型
        /// </summary>
        public string CarType
        {
            get;
            set;
        }

        
        /// <summary>
        /// 颜色
        /// </summary>
        public string CarColor
        {
            get;
            set;
        }
        /// <summary>
        /// 里程数
        /// </summary>
        public double Mileage
        {
            get;
            set;
        }
        /// <summary>
        /// 里程数单位
        /// </summary>
        public string MileageUnit
        {
            get;
            set;
        }
        /// <summary>
        /// 年限
        /// </summary>
        public string Years
        {
            get;
            set;
        }
        /// <summary>
        /// 当前价(万)
        /// </summary>
        public double CurrentPrices
        {
            get;
            set;
        }
        /// <summary>
        /// 起拍价
        /// </summary>
        public string StartPrices
        {
            get;
            set;
        }
        /// <summary>
        /// 拍卖状态
        /// </summary>
        public string AuctionStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 出价结束时间
        /// </summary>
        public string StopTime
        {
            get;
            set;
        }
        /// <summary>
        /// 竞拍人数
        /// </summary>
        public int BidCount
        {
            get;
            set;
        }
        /// <summary>
        /// 起拍价
        /// </summary>
        public double InitAuctionPrice
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
        /// 车排量
        /// </summary>
        public string CarDisplacement
        {
            get;
            set;
        }
        /// <summary>
        /// 0:确认条件1：最高价2：出价不是最高3：竞价结束4：竞价成功5：竞价失败
        /// </summary>
        public BidStatusType BidStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            get;
            set;
        }
        /// <summary>
        /// 拍卖结束时间
        /// </summary>
        public string PublishStopTime
        {
            get;
            set;
        }
        /// <summary>
        /// 0:默认，1：更新，2：删除，3：增加，4：已执行删除； 5:在其它地方更改的
        /// </summary>
        public int DataStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 是否设置机器人
        /// </summary>

        public bool IsSetRebot
        {
            get;
            set;
        }
        /// <summary>
        /// 是否出价 
        /// </summary>
        public bool IsBid
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
        /// 品牌id
        /// </summary>
        public int BrandId
        {
            set;
            get;
        }
               
        /// <summary>
        /// 出价开始时间
        /// </summary>
        public string StartTime
        {
            set;
            get;
        }
        /// <summary>
        /// 标注数据源时段
        /// </summary>
        public DataSourceTime Type
        {
            set;
            get;
        }
        /// <summary>
        /// 提车要求
        /// </summary>
        public string CarDemand
        {
            set;
            get;
        }
        /// <summary>
        /// 出价结束时间
        /// </summary>
        public DateTime PriceEndTime
        {
            set;
            get;
        }
        /// <summary>
        /// 是否自有车辆
        /// </summary>
        public bool IsSelfCar
        {
            set;
            get;
        }
        /// <summary>
        /// 是否伙伴
        /// </summary>
        public bool IsFriend
        {
            set;
            get;
        }
        public bool IsOnLinePay
        {
            set;
            get;
        }
        /// <summary>
        /// 是否可参拍
        /// </summary>
        public bool IsAuction
        {
            set;
            get;
        }
        

        /// <summary>
        /// 克隆
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public DateTime ServerTime { get; set; }

        /// <summary>
        /// 当前拍品的经销商是否是当前操作人所在单位的黑名单
        /// </summary>
        public bool IsBlackTraID { get; set; }

        /// <summary>
        /// 是否可见
        /// </summary>
        public bool IsShow { get; set; }

        /// <summary>
        /// 竞价是否结束
        /// </summary>
        public bool IsAuctionEnd { get; set; }

        /// <summary>
        /// 保留价
        /// </summary>
        public double KeepPrice { get; set; }

        /// <summary>
        /// 在线或线下
        /// </summary>
        public string OnlineOrOffline
        {
            get;
            set;
        }

        /// <summary>
        /// 是否支持代理支付
        /// </summary>
        public bool IsAgentPay
        {
            get;
            set;
        }

        /// <summary>
        /// 是否优信拍代理过户
        /// </summary>
        public bool IsAgentTransfer
        {
            get;
            set;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    /*
     * 具有通知功能的Model使用示例：
     * IAuctionHallDataModel model=IBMP.AOP.Utility.Create<IAuctionHallDataModel>(new AuctionHallDataNotifyModel());
     * 
     */

    /// <summary>
    /// 具有更改通知功能的拍卖大厅数据模型
    /// </summary>
    public class AuctionHallDataNotifyModel : NotifyBase, IAuctionHallDataModel
    {

        int _CarSourceId;

        /// <summary>
        /// 车编号
        /// </summary>
        public int CarSourceId
        {
            get { return _CarSourceId; }
            set { _CarSourceId = value; OnPropertyChanged("CarSourceId"); }
        }

        long _PublishId;

        /// <summary>
        /// 发布编号
        /// </summary>
        public long PublishId
        {
            get { return _PublishId; }
            set { _PublishId = value; OnPropertyChanged("PublishId"); }
        }

        string _AttentionState;
        /// <summary>
        /// 关注
        /// </summary>
        public string AttentionState
        {
            get { return _AttentionState; }
            set { _AttentionState = value; OnPropertyChanged("AttentionState"); }
        }
        bool _IsBid;
        /// <summary>
        /// 是否出价
        /// </summary>
        public bool IsBid
        {
            get { return _IsBid; }
            set { _IsBid = value; OnPropertyChanged("IsBid"); }
        }
        int _BrandId;
        /// <summary>
        /// 品牌ID
        /// </summary>
        public int BrandId
        {
            get { return _BrandId; }
            set { _BrandId = value; OnPropertyChanged("BrandId"); }
        }
        string _City;
        /// <summary>
        /// 城市
        /// </summary>
        public string City
        {
            get { return _City; }
            set { _City = value; OnPropertyChanged("City"); }
        }
        string _StartTime;
        /// <summary>
        /// 拍卖开始时间
        /// </summary>
        public string StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; OnPropertyChanged("StartTime"); }
        }
        string _CarTypeAndColor;
        /// <summary>
        /// 车型及颜色
        /// </summary>
        public string CarTypeAndColor
        {
            get { return _CarTypeAndColor; }
            set { _CarTypeAndColor = value; OnPropertyChanged("CarTypeAndColor"); }
        }

        string _CarType;
        /// <summary>
        /// 车型
        /// </summary>
        public string CarType
        {
            get { return _CarType; }
            set { _CarType = value; OnPropertyChanged("CarType"); }
        }
        string _carSerialName;
        /// <summary>
        /// 车系名称
        /// </summary>
        public string CarSerialName
        {
            get { return _carSerialName; }
            set { _carSerialName = value; OnPropertyChanged("CarSerialName"); }
        }

        string _CarColor;
        /// <summary>
        /// 颜色
        /// </summary>
        public string CarColor
        {
            get { return _CarColor; }
            set { _CarColor = value; OnPropertyChanged("CarColor"); }
        }

        double _Mileage;
        /// <summary>
        /// 里程数
        /// </summary>
        public double Mileage
        {
            get { return _Mileage; }
            set { _Mileage = value; OnPropertyChanged("Mileage"); }
        }
        //string _MileageUnit;
        ///// <summary>
        ///// 里程数单位
        ///// </summary>
        //public string MileageUnit
        //{
        //    get { return _MileageUnit; }
        //    set { _MileageUnit = value; OnPropertyChanged("MileageUnit"); }
        //}
        string _Years;
        /// <summary>
        /// 年限
        /// </summary>
        public string Years
        {
            get { return _Years; }
            set { _Years = value; OnPropertyChanged("Years"); }
        }
        DateTime _LicenseDate;
        /// <summary>
        /// LicenseDate
        /// </summary>
        public DateTime LicenseDate
        {
            get { return _LicenseDate; }
            set { _LicenseDate = value; OnPropertyChanged("LicenseDate"); }
        }

        double _CurrentPrices;
        /// <summary>
        /// 当前价(万)
        /// </summary>
        public double CurrentPrices
        {
            get { return _CurrentPrices; }
            set { _CurrentPrices = value; OnPropertyChanged("CurrentPrices"); }
        }

        string _StartPrices;
        /// <summary>
        /// 起拍价
        /// </summary>
        public string StartPrices
        {
            get { return _StartPrices; }
            set { _StartPrices = value; OnPropertyChanged("StartPrices"); }
        }

        string _AuctionStatus;
        /// <summary>
        /// 拍卖状态
        /// </summary>
        public string AuctionStatus
        {
            get { return _AuctionStatus; }
            set { _AuctionStatus = value; OnPropertyChanged("AuctionStatus"); }
        }

        string _StopTime;
        /// <summary>
        /// 结束时间
        /// </summary>
        public string StopTime
        {
            get { return _StopTime; }
            set { _StopTime = value; OnPropertyChanged("StopTime"); }
        }

        int _BidCount;
        /// <summary>
        /// 竞拍人数
        /// </summary>
        public int BidCount
        {

            get { return _BidCount; }
            set { _BidCount = value; OnPropertyChanged("BidCount"); }
        }

        double _InitAuctionPrice;
        /// <summary>
        /// 起拍价
        /// </summary>
        public double InitAuctionPrice
        {
            get { return _InitAuctionPrice; }
            set { _InitAuctionPrice = value; OnPropertyChanged("InitAuctionPrice"); }
        }
        int _ExpectMoneyRecExpired;
        /// <summary>
        /// 付款条件
        /// </summary>
        public int ExpectMoneyRecExpired
        {
            get { return _ExpectMoneyRecExpired; }
            set { _ExpectMoneyRecExpired = value; OnPropertyChanged("ExpectMoneyRecExpired"); }
        }
        int _ExpectTransferExpired;
        /// <summary>
        /// 过户条件
        /// </summary>
        public int ExpectTransferExpired
        {
            get { return _ExpectTransferExpired; }
            set { _ExpectTransferExpired = value; OnPropertyChanged("ExpectTransferExpired"); }
        }
        int _Isrelocation;
        /// <summary>
        /// 是否需要外迁 0：默认；1：外迁；2：不外迁
        /// </summary>
        public int Isrelocation
        {
            get { return _Isrelocation; }
            set { _Isrelocation = value; OnPropertyChanged("Isrelocation"); }
        }
        int _SellerTvaId;
        /// <summary>
        /// 卖家经销商编号
        /// </summary>
        public int SellerTvaId
        {
            get { return _SellerTvaId; }
            set { _SellerTvaId = value; OnPropertyChanged("SellerTvaId"); }
        }

        int _SellerTvuId;
        /// <summary>
        /// 卖家编号
        /// </summary>
        public int SellerTvuId
        {
            get { return _SellerTvuId; }
            set { _SellerTvuId = value; OnPropertyChanged("SellerTvuId"); }
        }

        string _CarDisplacement;
        /// <summary>
        /// 车排量
        /// </summary>
        public string CarDisplacement
        {
            get { return _CarDisplacement; }
            set { _CarDisplacement = value; OnPropertyChanged("CarDisplacement"); }
        }

        BidStatusType _BidStatus;
        /// <summary>
        /// 0:确认条件1：最高价2：出价不是最高3：竞价结束4：竞价成功5：竞价失败
        /// </summary>
        public BidStatusType BidStatus
        {
            get { return _BidStatus; }
            set { _BidStatus = value; OnPropertyChanged("BidStatus"); }
        }

        string _Unit;
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            get { return _Unit; }
            set { _Unit = value; OnPropertyChanged("Unit"); }
        }

        string _PublishStopTime;
        /// <summary>
        /// 拍卖结束时间
        /// </summary>
        public string PublishStopTime
        {
            get { return _PublishStopTime; }
            set { _PublishStopTime = value; OnPropertyChanged("PublishStopTime"); }
        }

        int _DataStatus;
        /// <summary>
        /// 0:新增1：更新2：删除
        /// </summary>
        public int DataStatus
        {
            get { return _DataStatus; }
            set { _DataStatus = value; OnPropertyChanged("DataStatus"); }
        }
        bool _IsSetRebot;
        /// <summary>
        /// 是否设置机器人
        /// </summary>
        public bool IsSetRebot
        {
            get { return _IsSetRebot; }
            set { _IsSetRebot = value; OnPropertyChanged("IsSetRebot"); }
        }
        string _CarDemand;
        public string CarDemand
        {
            get { return _CarDemand; }
            set { _CarDemand = value; OnPropertyChanged("_CarDemand"); }
        }
        DateTime _PriceEndTime;
        /// <summary>
        /// 出价结束时间
        /// </summary>
        public DateTime PriceEndTime
        {
            get { return _PriceEndTime; }
            set { _PriceEndTime = value; OnPropertyChanged("PriceEndTime"); }
        }
        bool _IsSelfCar;
        /// <summary>
        /// 是否自有车辆
        /// </summary>
        public bool IsSelfCar
        {
            get { return _IsSelfCar; }
            set { _IsSelfCar = value; OnPropertyChanged("IsSelfCar"); }
        }
        bool _IsFriend;
        /// <summary>
        /// 是否伙伴
        /// </summary>
        public bool IsFriend
        {
            get { return _IsFriend; }
            set { _IsFriend = value; OnPropertyChanged("IsFriend"); }
        }
        private int _aState;
        /// <summary>
        /// 关注选择状态
        /// </summary>
        public int AState
        {
            get { return _aState; }
            set { _aState = value; OnPropertyChanged("AState"); }
        }

        private int _buyerId;
        public int BuyerId
        {
            get
            {
                return _buyerId;
            }
            set
            {
                _buyerId = value; OnPropertyChanged("BuyerId");
            }
        }

        private int _buyerVendorId;
        public int BuyerVendorId
        {
            get
            {
                return _buyerVendorId;
            }
            set
            {
                _buyerVendorId = value; OnPropertyChanged("BuyerVendorId");
            }
        }

        private DataSourceTime type;

        public DataSourceTime Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value; OnPropertyChanged("Type");
            }
        }

        private bool isOnLinePay;
        public bool IsOnLinePay
        {
            get
            {
                return isOnLinePay;
            }
            set
            {
                isOnLinePay = value; OnPropertyChanged("IsOnLinePay");
            }
        }
        public DateTime ServerTime { get; set; }
        private bool _isBlackTraID;
        /// <summary>
        /// 当前拍品的经销商是否是当前操作人所在单位的黑名单
        /// </summary>
        public bool IsBlackTraID
        {
            get { return _isBlackTraID; }
            set { _isBlackTraID = value; OnPropertyChanged("IsBlackTraID"); }
        }
        private bool _IsAuction;
        /// <summary>
        /// 是否可参拍
        /// </summary>
        public bool IsAuction
        {
            get { return _IsAuction; }
            set { _IsAuction = value; OnPropertyChanged("IsAuction"); }
        }

        private bool _IsShow;
        /// <summary>
        /// 是否可见
        /// </summary>
        public bool IsShow 
        {
            get { return _IsShow; }
            set { _IsShow = value; OnPropertyChanged("IsShow"); }
        }
        private bool _IsAuctionEnd;
        /// <summary>
        /// 竞价是否结束
        /// </summary>
        public bool IsAuctionEnd 
        {
            get { return _IsAuctionEnd; }
            set { _IsAuctionEnd = value; OnPropertyChanged("IsAuctionEnd"); }
        }

        private double _KeepPrice;
        /// <summary>
        /// 保留价
        /// </summary>
        public double KeepPrice 
        {
            get { return _KeepPrice; }
            set { _KeepPrice = value; OnPropertyChanged("KeepPrice"); }
        }
        private string _OnlineOrOffline;
        /// <summary>
        /// 在线或线下
        /// </summary>
        public string OnlineOrOffline
        {
            get { return _OnlineOrOffline; }
            set { _OnlineOrOffline = value; OnPropertyChanged("OnlineOrOffline"); }
        }
        private bool _IsAgentPay;

        /// <summary>
        /// 是否支持代理支付
        /// </summary>
        public bool IsAgentPay
        {
            get { return _IsAgentPay; }
            set { _IsAgentPay = value; OnPropertyChanged("IsAgentPay"); }
        }

        private bool _IsAgentTransfer;
        /// <summary>
        /// 是否优信拍代理过户
        /// </summary>
        public bool IsAgentTransfer
        {
            get { return _IsAgentTransfer; }
            set { _IsAgentTransfer = value; OnPropertyChanged("IsAgentTransfer"); }
        }
    }
}
