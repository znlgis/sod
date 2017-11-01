using System;
using System.Collections.Generic;
using System.Text;

using IBMP.AOP;
using PWMIS.EnterpriseFramework.Common;

namespace TranstarAuction.Model.AuctionMain
{
    public class BuyCarModel : BaseObject
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public int OrderId
        {
            get;
            set;
        }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNumer
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
        /// 车颜色
        /// </summary>
        public string CarColor
        {
            get;
            set;
        }
        /// <summary>
        /// 车年代
        /// </summary>
        public string CarYear
        {
            get;
            set;
        }
        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderState OrderState
        {
            get;
            set;
        }
        /// <summary>
        /// 仲裁状态
        /// </summary>
        public ArbState ArbState
        {
            get;
            set;
        }
        /// <summary>
        /// 交易方式
        /// </summary>
        public PaymentType PaymentType
        {
            get;
            set;
        }
        /// <summary>
        /// 提车方式
        /// </summary>
        public ReceiveType ReceiveType
        {
            get;
            set;
        }
        /// <summary>
        /// 订单显示状态
        /// </summary>
        public TstShow TstShow
        {
            get;
            set;
        }
        /// <summary>
        /// 订单结果
        /// </summary>
        public TstResult TstResult
        {
            get;
            set;
        }
        /// <summary>
        /// 买家ID
        /// </summary>
        public int BuyTvuId
        {
            get;
            set;
        }
        /// <summary>
        /// 买家经销商ID
        /// </summary>
        public int BuyTvaId
        {
            get;
            set;
        }
        /// <summary>
        /// 卖家用户Id
        /// </summary>
        public int SellerTvuId
        {
            get;
            set;
        }
        /// <summary>
        /// 卖家经销商ID
        /// </summary>
        public int SellerTvaId
        {
            get;
            set;
        }
        /// <summary>
        /// 0：初始 ；1：修改；2：增加；3：删除
        /// </summary>
        public int DataStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 交易日期
        /// </summary>
        public DateTime TradeDate
        {
            get;
            set;
        }
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }

    public class BuyCarNotifyModel : NotifyBase, IBuyCarModel
    {
        int _OrderId;
        public int OrderId
        {
            get { return _OrderId; }
            set { _OrderId = value; OnPropertyChanged("OrderId"); }
        }
        string _OrderNumer;
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNumer
        {
            get { return _OrderNumer; }
            set { _OrderNumer = value; OnPropertyChanged("OrderNumer"); }
        }
        string _CarName;
        public string CarName
        {
            get { return _CarName; }
            set { _CarName = value; OnPropertyChanged("CarName"); }
        }
        string _CarColor;
        public string CarColor
        {
            get { return _CarColor; }
            set { _CarColor = value; OnPropertyChanged("CarColor"); }
        }
        string _CarYear;
        public string CarYear
        {
            get { return _CarYear; }
            set { _CarYear = value; OnPropertyChanged("CarYear"); }
        }
        OrderState _OrderState;
        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderState OrderState
        {
            get { return _OrderState; }
            set { _OrderState = value; OnPropertyChanged("OrderState"); }
        }
        ArbState _ArbState;
        /// <summary>
        /// 仲裁状态
        /// </summary>
        public ArbState ArbState
        {
            get { return _ArbState; }
            set { _ArbState = value; OnPropertyChanged("ArbState"); }
        }
        PaymentType _PaymentType;
        /// <summary>
        /// 交易方式
        /// </summary>
        public PaymentType PaymentType
        {
            get { return _PaymentType; }
            set { _PaymentType = value; OnPropertyChanged("PaymentType"); }
        }
        ReceiveType _ReceiveType;
        /// <summary>
        /// 提车方式
        /// </summary>
        public ReceiveType ReceiveType
        {
            get { return _ReceiveType; }
            set { _ReceiveType = value; OnPropertyChanged("ReceiveType"); }
        }
        TstShow _TstShow;
        /// <summary>
        /// 订单显示状态
        /// </summary>
        public TstShow TstShow
        {
            get { return _TstShow; }
            set { _TstShow = value; OnPropertyChanged("TstShow"); }
        }
        TstResult _TstResult;
        /// <summary>
        /// 订单结果
        /// </summary>
        public TstResult TstResult
        {
            get { return _TstResult; }
            set { _TstResult = value; OnPropertyChanged("TstResult"); }
        }
        int _BuyTvuId;
        /// <summary>
        /// 买家ID
        /// </summary>
        public int BuyTvuId
        {
            get { return _BuyTvuId; }
            set { _BuyTvuId = value; OnPropertyChanged("BuyTvuId"); }
        }
        int _BuyTvaId;
        /// <summary>
        /// 买家经销商ID
        /// </summary>
        public int BuyTvaId
        {
            get { return _BuyTvaId; }
            set { _BuyTvaId = value; OnPropertyChanged("BuyTvaId"); }
        }
        int _SellerTvuId;
        /// <summary>
        /// 卖家用户Id
        /// </summary>
        public int SellerTvuId
        {
            get { return _SellerTvuId; }
            set { _SellerTvuId = value; OnPropertyChanged("SellerTvuId"); }
        }
        int _SellerTvaId;
        /// <summary>
        /// 卖家经销商ID
        /// </summary>
        public int SellerTvaId
        {
            get { return _SellerTvaId; }
            set { _SellerTvaId = value; OnPropertyChanged("SellerTvaId"); }
        }
        int _DataStatus;
        /// <summary>
        /// 0：初始 ；1：修改；2：增加；3：删除
        /// </summary>
        public int DataStatus
        {
            get { return _DataStatus; }
            set { _DataStatus = value; OnPropertyChanged("DataStatus"); }
        }
        DateTime _TradeDate;
        /// <summary>
        /// 成交日期
        /// </summary>
        public DateTime TradeDate
        {
            get { return _TradeDate; }
            set { _TradeDate = value; OnPropertyChanged("TradeDate"); }
        }
    }
}
