using System;
using System.Collections.Generic;
 
using TranstarAuction.Presenters.ViewInterface;
using TranstarAuction.Model;
using TranstarAuction.Model.AuctionMain;
using System.ComponentModel;
using PWMIS.EnterpriseFramework.Service.Client;
using PWMIS.EnterpriseFramework.Service.Basic;
using PWMIS.EnterpriseFramework.Common;

namespace TranstarAuction.Presenters.Presenter
{
    /// <summary>
    /// 我关注的车 逻辑主持人
    /// </summary>
    public class MyAttentionCarPresenter : PresenterBase
    {
        IMyAttentionCarView View;
        /// <summary>
        /// 获取全局逻辑主持人
        /// </summary>
        public IGlobalPresenter GlobalPresenter{get;private set;}

        public MyAttentionCarPresenter(IMyAttentionCarView view,IGlobalPresenter pesenter)
        {
            this.View = view;
            this.GlobalPresenter = pesenter;
            this.GlobalPresenter.DataSourceChange += new EventHandler(GlobalPresenter_DataSourceChange);
            if (this.GlobalPresenter is MainFormPresenter)
            {
                ((MainFormPresenter)this.GlobalPresenter).ChangeDataDay += new EventHandler<GlobalMessageArgs>(MyAttentionCarPresenter_ChangeDataDay);
            }
        }

        void MyAttentionCarPresenter_ChangeDataDay(object sender, GlobalMessageArgs e)
        {
            this.View.ChangeDataDay(e.Parameter.ToString());
        }

        void GlobalPresenter_DataSourceChange(object sender, EventArgs e)
        {
            SetDataSource();
        }

        /// <summary>
        /// 从全局数据源中，设置当前视图的数据源
        /// </summary>
        private void SetDataSource()
        {
            List<IAttentionCarModel> temp11 = new List<IAttentionCarModel>();
            List<IAttentionCarModel> temp16 = new List<IAttentionCarModel>();
            List<IAttentionCarModel> temp14 = new List<IAttentionCarModel>();
            List<IAttentionCarModel> tempOtherTime = new List<IAttentionCarModel>();

            foreach (IAuctionHallDataModel model in GlobalPresenter.GlobalDataSource)
            {
                IAttentionCarModel attModel = HallModelToAuctionModel.TransferModel(model);
                switch (model.Type)
                { 
                    case DataSourceTime.End11:
                        temp11.Add(attModel);
                        break;
                    case  DataSourceTime.End16:
                        temp16.Add(attModel);
                        break;
                    case  DataSourceTime.End14:
                        temp14.Add(attModel);
                        break;
                    case DataSourceTime.EndAll:
                        tempOtherTime.Add(attModel);
                        break;
                }
            }

            this.SetRealNotifyModel(temp11);
            this.SetRealNotifyModel(temp16);
            this.SetRealNotifyModel(temp14);
            this.SetRealNotifyModel(tempOtherTime);

            this.View.UpdateDataSource(DataSourceTime.End11, temp11);
            this.View.UpdateDataSource(DataSourceTime.End16, temp16);
            this.View.UpdateDataSource(DataSourceTime.End14, temp14);
            this.View.UpdateDataSource(DataSourceTime.EndAll, tempOtherTime);
        }

        private IAttentionCarModel FindData(BindingList<IAttentionCarModel> dataSource, IAttentionCarModel newModel)
        {
            foreach (IAttentionCarModel item in dataSource)
            {
                if (item.PublishId == newModel.PublishId)
                    return item;
            }
            return null;
        }
        /// <summary>
        /// 将新实体数据赋值给旧实体
        /// </summary>
        /// <param name="newModel"></param>
        /// <param name="oldModel"></param>
        private void NewModelToOld(IAttentionCarModel newModel,IAttentionCarModel oldModel)
        {
            oldModel.AttentionState = newModel.AttentionState;
            oldModel.AuctionState = newModel.AuctionState;
            oldModel.BidStatus = newModel.BidStatus;
            oldModel.BrandId = newModel.BrandId;
            oldModel.BuyerId = newModel.BuyerId;
            oldModel.BuyerVendorId = newModel.BuyerVendorId;
            oldModel.CarColor = newModel.CarColor;
            oldModel.CarDemand = newModel.CarDemand;
            oldModel.CarDisplacement = newModel.CarDisplacement;
            oldModel.CarName = newModel.CarName;
            oldModel.CarSerialName = newModel.CarSerialName;
            oldModel.CarSourceId = newModel.CarSourceId;
            oldModel.CarYear = newModel.CarYear;
            oldModel.CityName = newModel.CityName;
            oldModel.CurrentAuctionMoney = newModel.CurrentAuctionMoney;
            oldModel.CurrentAuctionMoneyStr = newModel.CurrentAuctionMoneyStr;
            oldModel.CurrentAuctionPersonNums = newModel.CurrentAuctionPersonNums;
            oldModel.DataStatus = newModel.DataStatus;
            oldModel.ExpectMoneyRecExpired = newModel.ExpectMoneyRecExpired;
            oldModel.ExpectTransferExpired = newModel.ExpectTransferExpired;
            oldModel.IsAuction = newModel.IsAuction;
            oldModel.IsBid = newModel.IsBid;
            oldModel.IsBlackTraID = newModel.IsBlackTraID;
            oldModel.IsFriend = newModel.IsFriend;
            oldModel.IsNew = newModel.IsNew;
            oldModel.IsOnLinePay = newModel.IsOnLinePay;
            oldModel.Isrelocation = newModel.Isrelocation;
            oldModel.IsSelfCar = newModel.IsSelfCar;
            oldModel.IsSetRebot = newModel.IsSetRebot;
            oldModel.LogisticsMode = newModel.LogisticsMode;
            oldModel.OriginalPrice = newModel.OriginalPrice;
            oldModel.Price = newModel.Price;
            oldModel.PriceDegree = newModel.PriceDegree;
            oldModel.PublishId = newModel.PublishId;
            oldModel.PublishStopTime = newModel.PublishStopTime;
            oldModel.SellerTvaId = newModel.SellerTvaId;
            oldModel.SellerTvuId = newModel.SellerTvuId;
            oldModel.ServerTime = newModel.ServerTime;
            oldModel.StartTime = newModel.StartTime;
            oldModel.StopTime = newModel.StopTime;
            oldModel.SysRouteId = newModel.SysRouteId;
            oldModel.Unit = newModel.Unit;
            
        }
        /// <summary>
        /// 更新数据源（由于线程安全问题，此方法需要在View里面调用）
        /// </summary>
        /// <param name="dataSource">已经绑定的数据源</param>
        /// <param name="newData">新数据</param>
        /// <returns>返回数据修改过的拍品ID列表</returns>
        public List<long> UpdateDataSource(BindingList<IAttentionCarModel> dataSource, List<IAttentionCarModel> newData)
        {
            if (newData == null || newData.Count == 0)
                return null;

            IAttentionCarModel target = null;
            List<long> flashRowId = new List<long>();
 
            for (int i = 0; i < newData.Count; i++)
            {
                IAttentionCarModel model = newData[i];
                switch (model.DataStatus)
                {
                    case 1://edit 来自大厅的数据，可能是修改的，比如修改了关注属性
                        target = FindData(dataSource, model);
                        if (target != null)
                        {
                            int index = dataSource.IndexOf(target);
                            
                            if (model.AttentionState == "0")
                            {
                                dataSource.Remove(target);
                                break;
                            }
                           
                            //如果是取消关注，则进行下一个处理；
                            //dataSource.Insert(index, model);



                            //处理剩余60秒的数据
                            //下面是真实的代码
                            int diffVal = (int)DateTime.Parse(model.StopTime).Subtract(DateTime.Parse(target.StopTime)).TotalSeconds;
                            if (diffVal > 0 && diffVal <= 60)
                            {
                                ShowDelayEntity newEntity = new ShowDelayEntity();
                                newEntity.StopTime = DateTime.Parse(model.StopTime);
                                newEntity.ServerTime = model.ServerTime;

                                ShowDelayEntity oldEntity = new ShowDelayEntity();
                                oldEntity.StopTime = DateTime.Parse(target.StopTime);
                                oldEntity.ServerTime = target.ServerTime;

                                ((AttentionCarNotifyModel)model).DataChangeEntitys = new DataChangeEntitys(newEntity, oldEntity);
                            }
                            NewModelToOld(model, target);
                            //剩余60秒模拟数据，正式环境请注释下面的代码
                            //DataChangeEntitys dce = new DataChangeEntitys(
                            //    new ShowDelayEntity() { StopTime=DateTime.Now.AddSeconds(59), ServerTime=DateTime.Now.AddSeconds(3) },
                            //    new ShowDelayEntity() { StopTime = DateTime.Now.AddSeconds(5), ServerTime = DateTime.Now.AddSeconds(1) }
                            //    );
                            //((AttentionCarNotifyModel)model).DataChangeEntitys = dce;
                            //模拟结束
                           
                            flashRowId.Add(model.PublishId);
                        }
                        else
                        { 
                            //在当前数据源中未找到记录，应该判断是否为增加
                            if (model.AttentionState == "1")
                                dataSource.Add(model);
                        }
                        this.View.DataModifyAction(model);
                        break;
                    case 2://delete
                        target = FindData(dataSource, model);
                        if (target != null)
                        {
                            dataSource.Remove(target);
                        }
                        break;
                    case 0:
                    case 3://add
                        if(model.AttentionState=="1")
                            dataSource.Add(model);
                        break;
                }
            }//end for
           
            return flashRowId;
        }

        /// <summary>
        /// 获取加价幅度
        /// </summary>
        /// <param name="reservationPrice"></param>
        private string GetAddPriceRangeModel(double reservationPrice)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "GetAddPriceRangeModel";
            request.Parameters = new object[] { reservationPrice };
            string result="";

            MessageConverter<AddPriceRangeModel> convert = base.ServiceProxy.GetServiceMessage<AddPriceRangeModel>(request,  DataType.Json);
            result = convert.Result.QuickPriceRange;
            return result;
        }

        /// <summary>
        /// 生成真正的NotifyModel
        /// </summary>
        /// <param name="models">具有NotifyModel基类的集合</param>
        /// <returns></returns>
        private void SetRealNotifyModel(List<IAttentionCarModel> models)
        {
            foreach (var item in models)
            {
                item.CurrentAuctionMoneyStr = item.CurrentAuctionMoney.ToString("0.00");
                item.Unit = "万";
                
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
                        double addDegree=0.02;
                        string quickPrice=GetAddPriceRangeModel(item.KeepPrice);//获取快速加价幅度

                        if (!String.IsNullOrEmpty(quickPrice))
                        {
                            addDegree = double.Parse(quickPrice.Split('|')[0])/10000.00;
                        }
                        
                        item.AuctionState = "出价至               " + (item.CurrentAuctionMoney + addDegree).ToString("0.00") + "万";
                    }
                }
                else if (item.BidStatus == BidStatusType.确认条件 && item.IsSetRebot == true)//by 120131 hanshijie (没出价情况下设置机器人，状态应为：“机器人出价”)
                {
                    item.AuctionState = "机器人出价";
                }
                else
                {
                    //switch (item.BidStatus)
                    //{
                    //    case "0": item.AuctionState = "确认条件"; break;
                    //    case "1": item.AuctionState = "最高价"; break;
                    //    case "2": item.AuctionState = "出价不是最高"; break;
                    //    case "3": item.AuctionState = "竞价结束"; break;
                    //    case "4": item.AuctionState = "竞价成功"; break;
                    //    case "5": item.AuctionState = "竞价失败"; break;
                    //    default: item.AuctionState = item.BidStatus; break;
                    //}
                    item.AuctionState = item.BidStatus.ToString();
                }

               
            }
           
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

            this.GlobalPresenter.ServiceProxy.RequestService<bool>(
                 this.GlobalPresenter.ServiceProxy.ServiceSubscriber, 
                 request,
                  DataType.Text,
                 message =>
                 {
                     this.View.AddOrUptAtt(message);
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

            base.ServiceProxy.RequestService<string>(request,  DataType.Json, message =>
            {
                this.View.AuctionPrice(message);
            });
        }

    }
}
