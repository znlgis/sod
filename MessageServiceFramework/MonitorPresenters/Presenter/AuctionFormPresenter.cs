using System;
using System.Collections.Generic;
 
using TranstarAuction.Presenters.ViewInterface;
using TranstarAuction.Model.AuctionMain;
using TranstarAuction.Presenters;
using PWMIS.EnterpriseFramework.Service.Basic;
using PWMIS.EnterpriseFramework.Common;
namespace TranstarAuction.Presenters.Presenter
{
    public class AuctionFormPresenter : PresenterBase
    {
        private int historyMessageId;//订出价历史记录服务的消息标识阅，用于取消订阅使用

        public IAuctionForm AuctionForm;

        public AuctionFormPresenter(IAuctionForm auctionForm)
        {
            this.AuctionForm = auctionForm;
        }



        /// <summary>
        /// 取消或者设置关注
        /// </summary>
        /// <param name="TvaID"></param>
        /// <param name="TvuID"></param>
        /// <param name="carsourceID"></param>
        /// <param name="pID"></param>
        /// <param name="sta"></param>
        /// <param name="sr"></param>
        public void CancleAttention(int TvaID, int TvuID, int carsourceID, long pID, int sta, int sr)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionHallService";
            request.MethodName = "AddOrUptAtt";
            request.Parameters = new object[] { TvaID, TvuID, carsourceID, pID, sta, sr };

            IGlobalPresenter presenter = ((IBaseView)this.AuctionForm).MainPresenter;

            //presenter.ServiceProxy.RequestService<string>(
            //    presenter.ServiceProxy.ServiceSubscriber
            //    , request,
            //     DataType.Text,
            //    message =>
            //    {

            //    });

            presenter.ServiceProxy.RequestService<bool>(
                presenter.ServiceProxy.ServiceSubscriber, 
                request,
                 DataType.Text,
                message =>
                {
                    this.AuctionForm.AddOrUptAtt(message);
                });
        }

        /// <summary>
        /// 绑定竞价人和当前价格，窗体加载的时候先请求一次，然后再订阅服务。
        /// </summary>
        public void GetAuctionRecord(int carId, long pubId)
        {
            //string url = "Service://AuctionMainFormService/GetBidRecord/System.Int32=" + carId + "&System.Long=" + pubId;
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "GetHighestRecord";//首次的方法
            request.Parameters = new object[] { carId, pubId };

            //采用订阅模式的示例，需要服务器端主动发送数据，而不是客户端发送数据
            //注意：离开页面后，应该调用当前类的 Disconnection 方法
            base.ServiceProxy.RequestService<AuctionPriceHistoryModel>(request,  DataType.Json, message =>
            {
                this.AuctionForm.BindAuctionRecord(message);
                //再以订阅模式执行，推送新数据
                //request.ServiceName = "CurrentBidder";
                //request.MethodName = "GetBidRecord";//推送新数据的方法
                //request.Parameters = new object[] { carId, pubId };
                //request.ResetServiceUrl();

                //bool flag = 0 < base.ServiceProxy.Subscribe<AuctionPriceHistoryModel>(request,  DataType.Json, converter =>
                //{
                //    //下面的代码服务器会适时调用
                //    if (converter.Scceed)
                //    {
                //        this.AuctionForm.BindAuctionRecord(converter.Result);
                //    }
                //    else
                //    {
                //        //这里处理错误信息 message.ErrorMessage
                //    }
                //});
                //if (!flag)
                //{
                //    //这里处理错误信息 "订阅失败"
                //}
            });
        }



        /// <summary>
        /// 断开连接，关闭服务
        /// </summary>
        public void Disconnection()
        {
            this.DeSubscribeRecordHistory();
            base.ServiceProxy.Close();
        }

        /// <summary>
        /// 订阅历史竞价记录
        /// </summary>
        /// <param name="pubId"></param>
        /// <param name="action"></param>
        public void GetAuctionRecordHistory(long pubId, Action<List<AuctionPriceHistoryModel>> action)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "CurrentBidder";
            request.MethodName = "GetBidRecordList";
            request.Parameters = new object[] { pubId };
            //使用全局连接
            this.historyMessageId= this.AuctionForm.MainPresenter.ServiceProxy.Subscribe<List<AuctionPriceHistoryModel>>(
                request,
                 DataType.Json, 
                converter =>
                {
                    if (converter.Succeed && action != null)
                    {
                        action(converter.Result);
                    }
                });
        }

        /// <summary>
        /// 取消订阅历史竞价记录的服务
        /// </summary>
        public void DeSubscribeRecordHistory()
        {
            this.AuctionForm.MainPresenter.ServiceProxy.DeSubscribe(this.historyMessageId);
        }

        /// <summary>
        /// 得到历史竞价记录
        /// </summary>
        /// <param name="carId"></param>
        /// <param name="pubId"></param>
        public void GetAuctionRecordHistory(int carId, long pubId)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "GetBidRecordList";
            request.Parameters = new object[] { carId, pubId };

            base.ServiceProxy.RequestService<List<AuctionPriceHistoryModel>>(request,  DataType.Json, message =>
                {
                    var result = message;
                    foreach (AuctionPriceHistoryModel m in result)
                    {
                        if (m.TvaId == CurrentUser.TvalID)
                        {
                            m.AuctionPersonName = "您";
                        }
                        else
                        {
                            m.AuctionPersonName = "其他人";
                        }
                    }
                    this.AuctionForm.BindAuctionRecordHistory(result);
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
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "AuctionBid";
            request.Parameters = new object[] { model1 };

            base.ServiceProxy.RequestService<string>(request,  DataType.Text, message =>
            {
                this.AuctionForm.AuctionPrice(message);
            });
        }


        /// <summary>
        /// 设置机器人
        /// </summary>
        public void AddBidRobot(BidRebotModel model)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "AddBidRobot";
            request.Parameters = new object[] { model };

            base.ServiceProxy.RequestService<string>(request,  DataType.Text, message =>
            {
                this.AuctionForm.ReturnAddBidRobot(message);
            });
        }

        /// <summary>
        /// 取消机器人设置
        /// </summary>
        /// <param name="TvaID"></param>
        /// <param name="PId"></param>
        public void CancelBidRobot(int TvaID, long PId)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "CancelBidRobot";
            request.Parameters = new object[] { TvaID, PId };

            base.ServiceProxy.RequestService<bool>(request,  DataType.Text, message =>
            {
                this.AuctionForm.CancelBidRobot(message);
            });
        }

        /// <summary>
        /// 获取设置状态
        /// </summary>
        /// <param name="TvaID"></param>
        /// <param name="PId"></param>
        public void IsSetRobot(int TvaID, long PId)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "isSetRobot";
            request.Parameters = new object[] { TvaID, PId };

            base.ServiceProxy.RequestService<bool>(request,  DataType.Text, message =>
            {
                this.AuctionForm.IsSetRobot(message);
            });
        }

        /// <summary>
        /// 获取设置状态
        /// </summary>
        /// <param name="TvaID"></param>
        /// <param name="PId"></param>
        public void IsSetRobotNotLoad(int TvaID, long PId)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "isSetRobot";
            request.Parameters = new object[] { TvaID, PId };

            base.ServiceProxy.RequestService<bool>(request,  DataType.Text, message =>
            {
                this.AuctionForm.IsSetRobotNotLoad(message);
            });

        }

        /// <summary>
        /// 获取车基本信息
        /// </summary>
        /// <param name="TvaID"></param>
        /// <param name="PId"></param>
        public void GetCarBaseInfo(int carsourceId)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "GetCarSourceInfo";
            request.Parameters = new object[] { carsourceId };

            base.ServiceProxy.RequestService<AuctionCarModel>(request,  DataType.Json, message =>
            {
                this.AuctionForm.BindCarBaseInfo(message);
            });

        }


        /// <summary>
        /// 得到买卖家信息
        /// </summary>
        /// <param name="carsourceId"></param>
        public void GetBuySellerInfo(int carsourceId, int TvaId)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "GetVendorInfo";
            request.Parameters = new object[] { carsourceId, TvaId };

            base.ServiceProxy.RequestService<VendorModel>(request,  DataType.Json, message =>
            {
                this.AuctionForm.BindVendorInfo(message);
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

            base.ServiceProxy.RequestService<string>(request,  DataType.Json, message =>
            {

            });
        }

        /// <summary>
        /// 获取火眼Id
        /// </summary>
        /// <param name="carID"></param>
        /// <returns></returns>
        public void GetHYCarID(int carID)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "GetHYCarID";
            request.Parameters = new object[] { carID };

            base.ServiceProxy.RequestService<int>(request,  DataType.Json, message =>
            {
                this.AuctionForm.GetFireEyeCarId(message);
            });
        }

        /// <summary>
        /// 获取机器人设置信息
        /// </summary>
        /// <param name="TvaID"></param>
        /// <param name="PId"></param>
        public void GetBidRobot(int TvaID, long PId)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "GetBidRobot";
            request.Parameters = new object[] { TvaID, PId };

            base.ServiceProxy.RequestService<BidRebotModel>(request,  DataType.Json, message =>
            {
                this.AuctionForm.BindBidRobotInfo(message);
            });
        }

        /// <summary>
        /// 添加确认条件
        /// </summary>
        /// <param name="TvaID"></param>
        /// <param name="PId"></param>
        public void AddPublishCondtition(AuctionTstConditionModel model)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "AddPublishCondtition";
            request.Parameters = new object[] { model };

            IGlobalPresenter presenter = ((IBaseView)this.AuctionForm).MainPresenter;

            presenter.ServiceProxy.RequestService<bool>(presenter.ServiceProxy.ServiceSubscriber, request,  DataType.Text, message =>
            {
                this.AuctionForm.BindPublishCondtition(message);
            });
        }

        /// <summary>
        /// 获取确认信息
        /// </summary>
        /// <param name="TvaID"></param>
        /// <param name="PId"></param>
        public void GetConInfo(long pid)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "GetPublishCondtition";
            request.Parameters = new object[] { pid, CurrentUser.TvalID};

            base.ServiceProxy.RequestService<AuctionTstConditionModel>(request,  DataType.Json, message =>
            {
                this.AuctionForm.BindGetPublishCondtition(message);
            });
        }

        /// <summary>
        /// 获取加价幅度
        /// </summary>
        /// <param name="reservationPrice"></param>
        public void GetAddPriceRangeModel(double reservationPrice)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "GetAddPriceRangeModel";
            request.Parameters = new object[] { reservationPrice };

            base.ServiceProxy.RequestService<AddPriceRangeModel>(request,  DataType.Json, message =>
            {
                this.AuctionForm.BindAddPriceRange(message);
            });
        }
    }
}
