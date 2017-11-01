using System;
using System.Collections.Generic;
 
using TranstarAuction.Presenters.ViewInterface;
using TranstarAuction.Model.AuctionMain;
using PWMIS.EnterpriseFramework.Service.Basic;
using PWMIS.EnterpriseFramework.Common;

namespace TranstarAuction.Presenters.Presenter
{
    public class MyBuyCarPresenter:PresenterBase
    {
        IMyBuyCarView View = null;
        /// <summary>
        /// 获取全局逻辑主持人
        /// </summary>
        public IGlobalPresenter GlobalPresenter { get; private set; }

        public MyBuyCarPresenter(IMyBuyCarView view,IGlobalPresenter presenter)
        {
            View = view;
            GlobalPresenter = presenter;
        }

        public void RequestMyReviveCarData(int orderId,int tvuid)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "ConfirmTstOrder";
            request.Parameters = new object[] {orderId,tvuid };

            base.ServiceProxy.RequestService<bool>(request,  DataType.Text, message =>
            {
                this.View.BindReviveCar(message);
            });
        }

        public void RequestAllBuyCarData()
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "GetBuyCarModelList";
            request.Parameters = new object[] { CurrentUser.TvalID };

            this.GlobalPresenter.ServiceProxy.Subscribe<List<BuyCarModel>, List<BuyCarNotifyModel>>(
                request,
                 DataType.Json,
                converter =>
                { 
                    var models = converter.Result;
                    var list = GetRealNotifyModel(models);
                    if (list != null && list.Count > 0)
                    {
                        this.View.PushDataList(list);

                    }
                });
                
        }

        /// <summary>
        /// 生成真正的NotifyModel
        /// </summary>
        /// <param name="models">具有NotifyModel基类的集合</param>
        /// <returns></returns>
        private List<IBuyCarModel> GetRealNotifyModel(List<BuyCarNotifyModel> models)
        {
            List<IBuyCarModel> list = new List<IBuyCarModel>();
            foreach(var item in models)
            {
                item.CarName = item.CarName + "(" + item.CarColor + ")";
                item.CarYear = item.CarYear.Substring(2, 2)+"年";
                IBuyCarModel realNotifyModel = item;
                list.Add(realNotifyModel);
            }
            return list;
        }

    }
}
