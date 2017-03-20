using PWMIS.EnterpriseFramework.Common;
using PWMIS.EnterpriseFramework.Service.Basic;
using System;
using System.Collections.Generic;
using TranstarAuction.Presenters.ViewInterface;

namespace TranstarAuction.Presenters.Presenter
{
    public class RightConfirmPassPresenter :PresenterBase
    {
         public IRightConfirmPassView RightConfirPassView;

        public RightConfirmPassPresenter(IRightConfirmPassView rightConfirPassView)
        {
            this.RightConfirPassView = rightConfirPassView;
        }

        /// <summary>
        /// 取目的地城市名称
        /// </summary>
        /// <param name="sysRouterId"></param>
        public void GetCityName(string sysRouterId)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "GetCityName";
            request.Parameters = new object[] { sysRouterId};

            base.ServiceProxy.RequestService<string>(request,  DataType.Text, message =>
            {
                this.RightConfirPassView.GetCity(message);
            });
        }
    }
}
