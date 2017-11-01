using System;
using System.Collections.Generic;
 
using PWMIS.EnterpriseFramework.Service.Basic;
using PWMIS.EnterpriseFramework.Common;

namespace TranstarAuction.Presenters.Presenter
{
    public class CommonPresenter : PresenterBase
    {
        public void RequestServerDateTime(Action<DateTime> action)
        {
            ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "GetServerDateTime";
            request.Parameters = new object[] { };

            base.ServiceProxy.RequestService<DateTime>(request, DataType.Text, action);
        }
        public void RequestServerDateTime2(Action<DateTime> action)
        {
            ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "GetServerDateTime2";
            request.Parameters = new object[] { };

            base.ServiceProxy.RequestService<DateTime>(request,  DataType.Json, action);
        }

        public void GetLoginUserName(Action<List<string>> action)
        {
            ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionHallService";
            request.MethodName = "GetUserList";
            request.Parameters = new object[] { };

            base.ServiceProxy.RequestService<List<string>>(request,  DataType.Json, action);
        }
    }
}
