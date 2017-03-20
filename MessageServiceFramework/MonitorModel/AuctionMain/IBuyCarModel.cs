using System;
using System.ComponentModel;
namespace TranstarAuction.Model.AuctionMain
{
    public interface IBuyCarModel : INotifyPropertyChanged
    {
        string CarColor { get; set; }
        string CarName { get; set; }
        string CarYear { get; set; }
        int OrderId { get; set; }
        string OrderNumer{get;set;}
        OrderState OrderState { get; set; }
        ArbState ArbState { get; set; }
        PaymentType PaymentType { get; set; }
        ReceiveType ReceiveType { get; set; }
        TstShow TstShow { get; set; }
        TstResult TstResult { get; set; }
        int BuyTvuId { get; set; }
        int BuyTvaId { get; set; }
        int SellerTvuId { get; set; }
        int SellerTvaId { get; set; }
        int DataStatus { get; set; }
        /// <summary>
        /// 成交日期
        /// </summary>
        DateTime TradeDate { get; set; }
    }
}
