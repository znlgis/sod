using System;
using System.Collections.Generic;
 
using TranstarAuction.Presenters.ViewInterface;
using TranstarAuction.Model.AuctionMain;
using PWMIS.EnterpriseFramework.Service.Basic;
using PWMIS.EnterpriseFramework.Common;

namespace TranstarAuction.Presenters.Presenter
{
    public class RightConfirPresenter : PresenterBase
    {
        public IRightConfirView RightConfirView;

        public RightConfirPresenter(IRightConfirView rightConfirView)
        {
            this.RightConfirView = rightConfirView;
        }

        /// <summary>
        /// 获取物流路线
        /// </summary>
        /// <param name="TvaID"></param>
        /// <param name="PId"></param>
        //public void GetIsSysRounte(int BrandId, int TvaId, string city)
        //{
        //     ServiceRequest request = new  ServiceRequest();
        //    request.ServiceName = "AuctionMainFormService";
        //    request.MethodName = "IsSysRounte";
        //    request.Parameters = new object[] { BrandId, TvaId, city };

        //    base.ServiceProxy.RequestService<bool>(request,  DataType.Json, message =>
        //    {
        //        this.RightConfirView.IsSysRounte(message);
        //    });
        //}

        /// <summary>
        /// 获取物流路线 //老客户端使用的是老方法，不升级将带来问题
        /// </summary>
        /// <param name="TvaID"></param>
        /// <param name="PId"></param>
        public void GetIsSysRounte(string cityName, int carsourceId)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "IsSysRounte";
            request.Parameters = new object[] { cityName, carsourceId };

            base.ServiceProxy.RequestService<bool>(request,  DataType.Text, message =>
            {
                this.RightConfirView.IsSysRounte(message);
            });
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
                this.RightConfirView.BindCity(message);
            });
        }

        /// <summary>
        /// 获取物流费
        /// </summary>
        public void GetLgsPrice(int SysRouteID, int CarSourceID, decimal CarPrice)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "GetLgsPrice";
            request.Parameters = new object[]{ SysRouteID, CarSourceID, CarPrice };


            base.ServiceProxy.RequestService<decimal>(request,  DataType.Json, message =>
            {
                this.RightConfirView.BindLgsPrice(message);
            });
        }
    }
}
