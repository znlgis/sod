using System;
using System.Collections.Generic;
 
using TranstarAuction.Model.AuctionMain;

namespace TranstarAuction.Presenters.ViewInterface
{
    public interface IMainUcView : IBaseView
    {
        void BindDataCustomerInfo(AuctionCustomerInfoModel CustomerInfoModel);//客户基本信息
    }
}
