using System;
using System.Collections.Generic;
 
using TranstarAuction.Presenters.ViewInterface;
using TranstarAuction.Model.AuctionMain;
using PWMIS.EnterpriseFramework.Service.Basic;
using PWMIS.EnterpriseFramework.Common;

namespace TranstarAuction.Presenters.Presenter
{
    public class MyFriendPresenter : PresenterBase
    {
        IMyFriendView MyFriendView=null;
        public MyFriendPresenter(IMyFriendView myFriendView)
        {
            MyFriendView = myFriendView;
        }

        public void RequestMyFriendData()
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "GetFriendList";
            request.Parameters = new object[] { CurrentUser.TvalID };

            base.ServiceProxy.RequestService<List<MyFriendModel>>(request,  DataType.Json, message =>
            {
                this.MyFriendView.BindMyFriendData(message);
            });
        }
    }
}
