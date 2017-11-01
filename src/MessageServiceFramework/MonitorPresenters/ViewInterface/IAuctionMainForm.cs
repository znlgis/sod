using System;
using System.Collections.Generic;
 
using TranstarAuction.Model.AuctionMain;

using TranstarAuction.Model;

namespace TranstarAuction.Presenters.ViewInterface
{
    public interface IAuctionMainForm : IBaseView
    {

      void BindDataCustomerInfo( AuctionCustomerInfoModel CustomerInfoModel);//客户基本信息

      string CurrentPanelName { get; }
    }
}
