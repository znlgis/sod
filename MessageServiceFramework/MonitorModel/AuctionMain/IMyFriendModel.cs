using System;
using System.ComponentModel;
namespace TranstarAuction.Model.AuctionMain
{
    public interface IMyFriendModel : INotifyPropertyChanged
    {
        string City { get; set; }
        string CompanyName { get; set; }
        int SellerAId { get; set; }
        string SellerName { get; set; }
        int SellerUId { get; set; }
    }
}
