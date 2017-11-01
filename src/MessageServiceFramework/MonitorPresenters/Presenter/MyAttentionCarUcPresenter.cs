using System;
using System.Collections.Generic;
 
using TranstarAuction.Model.AuctionMain;
using TranstarAuction.Presenters.ViewInterface;
using TranstarAuction.Model;
using PWMIS.EnterpriseFramework.Common;
using PWMIS.EnterpriseFramework.Service.Basic;

namespace TranstarAuction.Presenters.Presenter
{
    public class MyAttentionCarUcPresenter : PresenterBase
    {
       IMyAttentionCarUcView myAttentionCarUcView;

        /// <summary>
        /// 得到结束时间实体列表
        /// </summary>
        public void GetEndTimeModelList()
        {
            string url = "Service://myAttentionCarUcViewService/GetEndTimeModelList/";
            base.ServiceProxy.RequestService<List<AuctionEndTimeModel>>(url,  DataType.Json, message =>
            {
                this.myAttentionCarUcView.BindEndTimeData(message);
            });
        }

        /// <summary>
        /// 生成真正的NotifyModel
        /// </summary>
        /// <param name="models">具有NotifyModel基类的集合</param>
        /// <returns></returns>
        private List<IAttentionCarModel> GetRealNotifyModel(List<AttentionCarNotifyModel> models)
        {
            List<IAttentionCarModel> list = new List<IAttentionCarModel>();
            foreach (var item in models)
            {
                item.CurrentAuctionMoneyStr = item.CurrentAuctionMoney.ToString("0.00");
                item.Unit = "万元";
                //item.CarName = item.CarName + "  " + item.CarYear.Substring(2, 2) + "年";
                if (item.BidStatus == BidStatusType.出价不是最高)//"2"
                {
                    if (item.IsSetRebot == true)
                    {
                        item.AuctionState = "机器人出价";
                    }
                    else
                    {
                        double currentprices = Convert.ToDouble(item.CurrentAuctionMoney);
                        double addDegree = 0.02;
                        item.AuctionState = "出价至            " + (item.CurrentAuctionMoney + addDegree).ToString("0.00") + "万元";
                    }
                }
                else if (item.BidStatus == BidStatusType.确认条件 && item.IsSetRebot == true)//"0" by 120131 hanshijie (没出价情况下设置机器人，状态应为：“机器人出价”)
                {
                    item.AuctionState = "机器人出价";
                }
                else
                {
                    item.AuctionState = item.BidStatus.ToString();// ((BidStatusType)Convert.ToInt32(item.BidStatus)).ToString();
                }

                //
                //IAttentionCarModel realNotifyModel = IBMP.AOP.Utility.Create<IAttentionCarModel>(item);
                IAttentionCarModel realNotifyModel = item;
                list.Add(realNotifyModel);
            }
            return list;
        }

        /// <summary>
        /// 得到结束时间内车实体列表
        /// </summary>
        public void GetEndTimeAttentionCarModelList11(int tvaId, string endTime)
        {
            // string url = "Service://myAttentionCarUcViewService/GetAuctionTitleDataList/";
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "myAttentionCarUcViewService";
            request.MethodName = "GetAuctionTitleDataList";
            request.Parameters = new object[] { tvaId, endTime };
            //改为订阅模式，服务器推送
            this.myAttentionCarUcView.MainPresenter.ServiceProxy.Subscribe<List<AttentionCarModel>, List<AttentionCarNotifyModel>>(request,
                 DataType.Json,
                converter =>
                {
                    var models = converter.Result;
                    var list = GetRealNotifyModel(models);
                    if (list != null && list.Count > 0)
                    {
                        this.myAttentionCarUcView.BindAttentionCar11(list);

                       
                    }
                }
                );


        }


        /// <summary>
        /// 得到结束时间内车实体列表
        /// </summary>
        public void GetEndTimeAttentionCarModelList16(int tvaId, string endTime)
        {
            
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "myAttentionCarUcViewService";
            request.MethodName = "GetAuctionTitleDataList";
            request.Parameters = new object[] { tvaId, endTime };
            //改为订阅模式，服务器推送
            this.myAttentionCarUcView.MainPresenter.ServiceProxy.Subscribe<List<AttentionCarModel>, List<AttentionCarNotifyModel>>(request,
                 DataType.Json,
                converter =>
                {
                    var models = converter.Result;
                    var list = GetRealNotifyModel(models);
                    if (list.Count > 0)
                        this.myAttentionCarUcView.BindAttentionCar16(list);
                }
                );

            
        }

        /// <summary>
        /// 得到结束时间内车实体列表
        /// </summary>
        public void GetEndTimeAttentionCarModelListOther(int tvaId, string endTime)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "myAttentionCarUcViewService";
            request.MethodName = "GetAuctionTitleDataList";
            request.Parameters = new object[] { tvaId, endTime };
            //改为订阅模式，服务器推送
            this.myAttentionCarUcView.MainPresenter.ServiceProxy.Subscribe<List<AttentionCarModel>, List<AttentionCarNotifyModel>>(request,
                 DataType.Json,
                converter =>
                {
                    var models = converter.Result;
                    var list = GetRealNotifyModel(models);
                    if (list.Count > 0)
                        this.myAttentionCarUcView.BindAttentionCarOther(list);
                }
                );

            
        }

        /// <summary>
        /// 得到结束时间内车数量统计
        /// </summary>
        public void GetEndTimeCarModelCount(int tvaId)
        {
            
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "myAttentionCarUcViewService";
            request.MethodName = "GetEndTimeCount";
            request.Parameters = new object[] { tvaId };
            base.ServiceProxy.RequestService<List<AuctionEndTimeModel>>(request,  DataType.Json, message =>
            {
                this.myAttentionCarUcView.GetEndTimeCarCount(message);
            });
        }

        /// <summary>
        /// 竞价操作
        /// </summary>
        public void AuctionPrice(IAttentionCarModel model)
        {
            AttentionCarModel model1 = new AttentionCarModel();
            model1.AttentionState = model.AttentionState;
            model1.AuctionState = model.AuctionState;
            model1.BidStatus = model.BidStatus;
            model1.BuyerId = model.BuyerId;
            model1.BuyerVendorId = model.BuyerVendorId;
            model1.CarColor = model.CarColor;
            model1.CarDisplacement = model.CarDisplacement;
            model1.CarName = model.CarName;
            model1.CarSourceId = model.CarSourceId;
            model1.CarYear = model.CarYear;
            model1.CityName = model.CityName;
            model1.CurrentAuctionMoney = model.CurrentAuctionMoney;
            model1.CurrentAuctionMoneyStr = model.CurrentAuctionMoneyStr;
            model1.CurrentAuctionPersonNums = model.CurrentAuctionPersonNums;
            model1.DataStatus = model.DataStatus;
            model1.ExpectMoneyRecExpired = model.ExpectMoneyRecExpired;
            model1.ExpectTransferExpired = model.ExpectTransferExpired;
            model1.IsNew = model.IsNew;
            model1.Isrelocation = model.Isrelocation;
            model1.LogisticsMode = model.LogisticsMode;
            model1.OriginalPrice = model.OriginalPrice;
            model1.Price = model.Price;
            model1.PriceDegree = model.PriceDegree;
            model1.PublishId = model.PublishId;
            model1.PublishStopTime = model.PublishStopTime;
            model1.SellerTvaId = model.SellerTvaId;
            model1.SellerTvuId = model.SellerTvuId;
            model1.StopTime = model.StopTime;
            model1.SysRouteId = model.SysRouteId;
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "myAttentionCarUcViewService";
            request.MethodName = "AuctionBid";
            request.Parameters = new object[] { model1 };

            base.ServiceProxy.RequestService<string>(request,  DataType.Json, message =>
            {
                this.myAttentionCarUcView.AuctionPrice(message);
            });
        }

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="TvaID"></param>
        /// <param name="TvuID"></param>
        /// <param name="carsourceID"></param>
        /// <param name="pID"></param>
        /// <param name="sta"></param>
        /// <param name="sr"></param>
        /// <returns></returns>
        public void CacelAttention(int TvaID, int TvuID, int carsourceID, long pID, int sta, int sr)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionHallService";
            request.MethodName = "AddOrUptAtt";
            request.Parameters = new object[] { TvaID, TvuID, carsourceID, pID, sta, sr };
         

            this.myAttentionCarUcView.MainPresenter.ServiceProxy.RequestService<bool>(
                 this.myAttentionCarUcView.MainPresenter.ServiceProxy.ServiceSubscriber
                 , request,
                  DataType.Text,
                 message =>
                 {
                     myAttentionCarUcView.AddOrUptAtt(message);
                 });
        }
    }
}
