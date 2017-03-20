using System;
using System.Collections.Generic;
 
using TranstarAuction.Presenters.ViewInterface;
using TranstarAuction.Model.AuctionMain;
using PWMIS.EnterpriseFramework.Service.Basic;
using PWMIS.EnterpriseFramework.Common;

namespace TranstarAuction.Presenters.Presenter
{
    public class CityFormPresenter : PresenterBase
    {
        public ICityForm CityForm;

        public CityFormPresenter(ICityForm cityForm)
        {
            this.CityForm = cityForm;
        }

        /// <summary>
        /// 得到城市列表
        /// </summary>
        public void GetCity()
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "GetLgtRouteList";


            base.ServiceProxy.RequestService<List<AuctionEndTimeModel>>(request,  DataType.Json, message =>
            {
                this.CityForm.GetCityData(message);
            });
        }
    }
}
