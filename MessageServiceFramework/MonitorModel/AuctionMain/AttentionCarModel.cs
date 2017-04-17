using System;
using System.Collections.Generic;
using System.Text;
using PWMIS.EnterpriseFramework.Common;
using IBMP.AOP;

namespace TranstarAuction.Model.AuctionMain
{
    public class AttentionCarModel : BaseObject
    {
        //public IDataChange dataChange { set; get; }

        //public int LeftSec { get; set; }
        

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
        /// 起拍价
        /// </summary>
        public double Price
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
        /// 加价前的价格
        /// </summary>
        public double OriginalPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 加价幅度
        /// </summary>
        public double PriceDegree
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
        /// 车系
        /// </summary>
        public string CarSerialName
        {
            get;
            set;
        }
        /// <summary>
        /// 车颜色
        /// </summary>
        public string CarColor
        {
            get;
            set;
        }

        /// <summary>
        /// 车年份
        /// </summary>
        public string CarYear
        {
            get;
            set;
        }

        /// <summary>
        /// 当前出价
        /// </summary>
        public double CurrentAuctionMoney
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
        /// 车排量
        /// </summary>
        public string CarDisplacement
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

        BidStatusType _bidStatus;
        /// <summary>
        /// 0:确认条件1：最高价2：出价不是最高3：竞价结束4：竞价成功5：竞价失败
        /// </summary>
        public BidStatusType BidStatus
        {
            get {
                return _bidStatus;
            }
            set {
                //switch (value)
                //{
                //    case "确认条件": _bidStatus = "0"; break;
                //    case "最高价": _bidStatus = "1"; break;
                //    case "出价不是最高": _bidStatus = "2"; break;
                //    case "竞价结束": _bidStatus = "3"; break;
                //    case "竞价成功": _bidStatus = "4"; break;
                //    case "竞价失败": _bidStatus = "5"; break;
                //    default:
                //        if (value.StartsWith("出价至"))
                //            _bidStatus="2";
                //        else
                //            _bidStatus = value; 
                //        break;
                //}
                _bidStatus = value;
            }
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
        /// true：取新的路线和物流方式,false：取上一次竞价的路线和物流方式
        /// </summary>
        public bool IsNew
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
        /// 0:新增1：更新2：删除3:增加
        /// </summary>
        public int DataStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 当前竞价(字符型)
        /// </summary>
        public string CurrentAuctionMoneyStr
        {
            get;
            set;
        }
        /// <summary>
        /// 竞价状态
        /// </summary>
        public string AuctionState
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
        /// 品牌id
        /// </summary>
        public int BrandId
        {
            set;
            get;
        }

        /// <summary>
        /// 当前服务器时间
        /// </summary>
        public DateTime ServerTime
        {
            get;
            set;
        }
        /// <summary>
        /// BY Hanshijie
        /// </summary>
        public string Unit
        {
            get;
            set;
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
        /// 提车要求
        /// </summary>
        public string CarDemand
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

        /// <summary>
        /// 是否支持在线支付
        /// </summary>
        public bool IsOnLinePay
        {
            get;
            set;
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

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }


    public class AttentionCarNotifyModel : NotifyBase, IAttentionCarModel
    {
        public DataChangeEntitys DataChangeEntitys { get; set; }

        //public ICurrentDataChange currentDataChange { set; get; }
        

        //public int LeftSec { get; set; }

        //public void RaiseCurrentChange(object sender)
        //{
        //    if (currentDataChange!=null)
        //    {
        //        currentDataChange.OnCurrentDataChange(sender);
        //    }
        //}
       
        /// <summary>
        /// BY Hanshijie
        /// </summary>
        string _Unit;
        public string Unit
        {
            get { return _Unit; }
            set { _Unit = value; OnPropertyChanged("Unit"); }
        }
        bool _IsBid;
        public bool IsBid
        {
            get { return _IsBid; }
            set { _IsBid = value; OnPropertyChanged("IsBid"); }
        }

        bool _IsSetRebot;
        public bool IsSetRebot
        {
            get { return _IsSetRebot; }
            set { _IsSetRebot = value; OnPropertyChanged("IsSetRebot"); }
        }

        BidStatusType _bidStatus;

        public BidStatusType BidStatus
        {
            get { return _bidStatus; }
            set {
                //switch (value)
                //{
                //    case "确认条件": _bidStatus = "0"; break;
                //    case "最高价": _bidStatus = "1"; break;
                //    case "出价不是最高": _bidStatus = "2"; break;
                //    case "竞价结束": _bidStatus = "3"; break;
                //    case "竞价成功": _bidStatus = "4"; break;
                //    case "竞价失败": _bidStatus = "5"; break;
                //    default:
                //        if (value.StartsWith("出价至"))
                //            _bidStatus = "2";
                //        else
                //            _bidStatus = value;
                //        break;
                //}
                _bidStatus = value;
                OnPropertyChanged("BidStatus"); 
            }
        }
        int _BuyerId;

        public int BuyerId
        {
            get { return _BuyerId; }
            set { _BuyerId = value; OnPropertyChanged("BuyerId"); }
        }
        int _BuyerVendorId;

        public int BuyerVendorId
        {
            get { return _BuyerVendorId; }
            set { _BuyerVendorId = value; OnPropertyChanged("BuyerVendorId"); }
        }
        string _CarColor;

        public string CarColor
        {
            get { return _CarColor; }
            set { _CarColor = value; OnPropertyChanged("CarColor"); }
        }
        string _CarDisplacement;

        public string CarDisplacement
        {
            get { return _CarDisplacement; }
            set { _CarDisplacement = value; OnPropertyChanged("CarDisplacement"); }
        }
        string _CarName;

        public string CarName
        {
            get { return _CarName; }
            set { _CarName = value; OnPropertyChanged("CarName"); }
        }
        string _CarSerialName;
        /// <summary>
        /// 车系
        /// </summary>
        public string CarSerialName
        {
            get { return _CarSerialName; }
            set { _CarSerialName = value; OnPropertyChanged("CarSerialName"); }
        }

        int __CarSourceId;

        public int CarSourceId
        {
            get { return __CarSourceId; }
            set { __CarSourceId = value; OnPropertyChanged("CarSourceId"); }
        }
        string _CarYear;

        public string CarYear
        {
            get { return _CarYear; }
            set { _CarYear = value; OnPropertyChanged("CarYear"); }
        }
        string _CityName;

        public string CityName
        {
            get { return _CityName; }
            set { _CityName = value; OnPropertyChanged("CityName"); }
        }
        double _CurrentAuctionMoney;

        public double CurrentAuctionMoney
        {
            get { return _CurrentAuctionMoney; }
            set { _CurrentAuctionMoney = value; OnPropertyChanged("CurrentAuctionMoney"); }
        }
        int _CurrentAuctionPersonNums;

        public int CurrentAuctionPersonNums
        {
            get { return _CurrentAuctionPersonNums; }
            set { _CurrentAuctionPersonNums = value; OnPropertyChanged("CurrentAuctionPersonNums"); }
        }
        int _DataStatus;

        public int DataStatus
        {
            get { return _DataStatus; }
            set { _DataStatus = value; OnPropertyChanged("DataStatus"); }
        }
        int _ExpectMoneyRecExpired;

        public int ExpectMoneyRecExpired
        {
            get { return _ExpectMoneyRecExpired; }
            set { _ExpectMoneyRecExpired = value; OnPropertyChanged("ExpectMoneyRecExpired"); }
        }
        int _ExpectTransferExpired;

        public int ExpectTransferExpired
        {
            get { return _ExpectTransferExpired; }
            set { _ExpectTransferExpired = value; OnPropertyChanged("ExpectTransferExpired"); }
        }
        bool _IsNew;

        public bool IsNew
        {
            get { return _IsNew; }
            set { _IsNew = value; OnPropertyChanged("IsNew"); }
        }
        int _Isrelocation;

        public int Isrelocation
        {
            get { return _Isrelocation; }
            set { _Isrelocation = value; OnPropertyChanged("Isrelocation"); }
        }
        int _LogisticsMode;

        public int LogisticsMode
        {
            get { return _LogisticsMode; }
            set { _LogisticsMode = value; OnPropertyChanged("LogisticsMode"); }
        }
        double _OriginalPrice;

        public double OriginalPrice
        {
            get { return _OriginalPrice; }
            set { _OriginalPrice = value; OnPropertyChanged("OriginalPrice"); }
        }
        double _Price;

        public double Price
        {
            get { return _Price; }
            set { _Price = value; OnPropertyChanged("Price"); }
        }
        double _PriceDegree;

        public double PriceDegree
        {
            get { return _PriceDegree; }
            set { _PriceDegree = value; OnPropertyChanged("CarSourceId"); }
        }
        long _PublishId;

        public long PublishId
        {
            get { return _PublishId; }
            set { _PublishId = value; OnPropertyChanged("PublishId"); }
        }
        string _PublishStopTime;

        public string PublishStopTime
        {
            get { return _PublishStopTime; }
            set { _PublishStopTime = value; OnPropertyChanged("PublishStopTime"); }
        }
        int _SellerTvaId;

        public int SellerTvaId
        {
            get { return _SellerTvaId; }
            set { _SellerTvaId = value; OnPropertyChanged("SellerTvaId"); }
        }
        int _SellerTvuId;

        public int SellerTvuId
        {
            get { return _SellerTvuId; }
            set { _SellerTvuId = value; OnPropertyChanged("SellerTvuId"); }
        }
        string _StopTime;

        public string StopTime
        {
            get { return _StopTime; }
            set 
            { 
                _StopTime = value; OnPropertyChanged("StopTime");
                //DateTime result;
                //if (DateTime.TryParse(_StopTime, out result))
                //{
                //    LeftSec = result.Subtract(DateTime.Now).Seconds; 
                //}
            }
        }

        

        int _SysRouteId;

        public int SysRouteId
        {
            get { return _SysRouteId; }
            set { _SysRouteId = value; OnPropertyChanged("SysRouteId"); }
        }
        string _CurrentAuctionMoneyStr;

        public string CurrentAuctionMoneyStr
        {
            get { return _CurrentAuctionMoneyStr; }
            set { _CurrentAuctionMoneyStr = value; OnPropertyChanged("CurrentAuctionMoneyStr"); }
        }
        string _AuctionState;

        public string AuctionState
        {
            get { return _AuctionState; }
            set { _AuctionState = value; OnPropertyChanged("AuctionState"); }
        }

        string _AttentionState;
        public string AttentionState
        {
            get { return _AttentionState; }
            set { _AttentionState = value; OnPropertyChanged("AttentionState"); }
        }

        int _BrandId;
        public int BrandId
        {
            get { return _BrandId; }
            set { _BrandId = value; OnPropertyChanged("BrandId"); }
        }

        DateTime _ServerTime;
            /// <summary>
        /// 当前服务器时间
        /// </summary>
        public DateTime ServerTime
        {
            get { return _ServerTime; }
            set { _ServerTime = value; OnPropertyChanged("_ServerTime"); }
        }
        string _StartTime;
        /// <summary>
        /// 出价开始时间
        /// </summary>
        public string StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; OnPropertyChanged("_StartTime"); }
        }
        string _CarDemand;
        public string CarDemand
        {
            get { return _CarDemand; }
            set { _CarDemand = value; OnPropertyChanged("_CarDemand"); }
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
        bool _IsOnLinePay;
        public bool IsOnLinePay
        {
            get { return _IsOnLinePay; }
            set { _IsOnLinePay = value; OnPropertyChanged("IsOnLinePay"); }
        }
        /// <summary>
        /// 是否是黑名单经销商发布的拍品
        /// </summary>
        public bool IsBlackTraID { get; set; }

        private bool _IsAuction;
        /// <summary>
        /// 是否可参拍
        /// </summary>
        public bool IsAuction
        {
            get { return _IsAuction; }
            set { _IsAuction = value; OnPropertyChanged("IsAuction"); }
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
