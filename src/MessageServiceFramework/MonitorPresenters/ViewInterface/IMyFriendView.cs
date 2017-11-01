using System;
using System.Collections.Generic;
 
using TranstarAuction.Model.AuctionMain;


namespace TranstarAuction.Presenters.ViewInterface
{
   public interface IMyFriendView
    {
       void BindMyFriendData(List<MyFriendModel> model);
    }
}
