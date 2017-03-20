using System;
using System.Collections.Generic;
 


namespace TranstarAuction.Presenters.ViewInterface
{
    public interface IAuctionMainFormView 
    {
        void ShowMessage(string message);
        void ShowMsgAndLogout(string title, string msg);
    }
}
