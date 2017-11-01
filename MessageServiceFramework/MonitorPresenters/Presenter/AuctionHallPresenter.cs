using System;
using System.Collections.Generic;
 
using TranstarAuction.Model;
using TranstarAuction.Presenters.ViewInterface;
using PWMIS.EnterpriseFramework.Service.Basic;
using TranstarAuction.Model.AuctionMain;
using PWMIS.EnterpriseFramework.Common;

namespace TranstarAuction.Presenters.Presenter
{
    public class AuctionHallPresenter : PresenterBase
    {
        IAuctionHallView view;
        IGlobalPresenter GlobalPresenter;

        public AuctionHallPresenter(IAuctionHallView view)
        {
            this.view = view;
        }
        public AuctionHallPresenter(IAuctionHallView view,IGlobalPresenter pesenter)
        {
            this.view = view;
            this.GlobalPresenter = pesenter;
            this.GlobalPresenter.DataSourceChange += new EventHandler(GlobalPresenter_DataSourceChange);
        }

        void GlobalPresenter_DataSourceChange(object sender, EventArgs e)
        {
            List<AuctionHallDataNotifyModel> auctionHallDataModel = GlobalPresenter.GlobalDataSource.ConvertAll<AuctionHallDataNotifyModel>(i => (AuctionHallDataNotifyModel)i);
            this.view.ShowAuctionHallData(auctionHallDataModel, "");
        }
        /// <summary>
        /// 获取搜索结果
        /// </summary>
        /// <param name="TvaID">用户ID</param>
        /// <returns>搜索结果</returns>
        public void ShowCarSearchResult(string CarName)
        {
            string url = "Service://AuctionHallService/GetCarList/System.String=" + CarName;
            base.ServiceProxy.RequestService<List<CarSearchResultModel>>(url,  DataType.Json, message =>
            {
                this.view.ShowCarSearchData(message);
            });
        }
        /// <summary>
        /// 增加关注车辆
        /// </summary>
        /// <param name="TvaID">经销商ID</param>
        /// <param name="TvuID">用户ID</param>
        /// <param name="carsourceID">车源ID</param>
        /// <param name="pID">拍品ID</param>
        /// <param name="sta">0</param>
        /// <param name="sr">0</param>
        /// <param name="rowIndex">当前操作的行索引</param>
        public void AddOrUptAtt(int TvaID, int TvuID, int carsourceID, long pID, int sta, int sr)
        {
            ServiceRequest request = new ServiceRequest();
            request.ServiceName = "AuctionHallService";
            request.MethodName = "AddOrUptAtt";
            request.Parameters = new object[] { TvaID, TvuID, carsourceID, pID, sta, sr };

            this.view.MainPresenter.ServiceProxy.RequestService<bool>(
                this.view.MainPresenter.ServiceProxy.ServiceSubscriber,
                request,
                 DataType.Text,
                message =>
                {
                    this.view.AddOrUptAtt(message, pID);
                });
        }
        /// <summary>
        /// 出价
        /// </summary>
        /// <param name="model"></param>
        public string AuctionBid(AttentionCarModel model, int TvalId, int UserID, int CarSId, long PID, string IsGZ)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "AuctionBid";
            request.Parameters = new object[] { model };
            string result = string.Empty;
            base.ServiceProxy.RequestService<string>(request,  DataType.Text, message =>
            {
                result = message;
                //if (result == "\"00\"")
                //{
                //    if (IsGZ != "1")
                //    {
                //        this.AddOrUptAtt(TvalId, UserID, CarSId, PID, 1, 0);
                //    }
                //}
                this.view.AuctionBid(message);

            });
            return result;
        }
        /// <summary>
        /// 获取汇总数据
        /// </summary>
        /// <param name="TvaID"></param>
        /// <param name="carname"></param>
        public void GetAllEndTime(int TvaID, string carname)
        {
            if (carname == "请输入品牌或车系")
            {
                carname = "";
            }
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "GetCarCount";
            request.Parameters = new object[] { TvaID, carname };

            base.ServiceProxy.RequestService<int>(request,  DataType.Text, message =>
            {
                this.view.GetAllEndTime(message);
            });
        }
        /// <summary>
        /// 拍卖大厅数据(排序时)
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="dateTime">统计时间【11点:11   16点:16    其他整点:11,16   今日全部:当天时间】</param>
        /// <param name="TypeOrderbyStr">如：里程数（order by Mileage desc）</param>
        public void ShowAuctionHallResult(int TvaID, string dateTime, string carName, string TypeOrderbyStr)
        {
            //GetHallData(TvaID, dateTime, carName, TypeOrderbyStr);
        }
        private void GetHallData(int TvaID, string dateTime, string carName, string TypeOrderbyStr)
        {
            //if (carName == "请输入品牌或车系")
            //{
            //    carName = "";
            //}
            //if (TypeOrderbyStr == null)
            //{
            //    TypeOrderbyStr = "";
            //}
            // ServiceRequest request = new  ServiceRequest();
            //request.ServiceName = "AuctionHallService";
            //request.MethodName = "GetAuctionHallDataList";
            //request.Parameters = new object[] { TvaID, dateTime, carName, TypeOrderbyStr };
            ////订阅模式，推送数据
            //this.view.MainPresenter.ServiceProxy.Subscribe<List<AuctionHallDataModel>, List<AuctionHallDataNotifyModel>>(
            //    request,
            //     DataType.Json,
            //    converter =>
            //    {
            //        var message = converter.Result;
            //        #region
            //        foreach (var item in message)
            //        {
            //            #region 里程数
            //            double mileage = Math.Round(item.Mileage / 10000.00, 4);
            //            if (mileage < 1)
            //            {
            //                item.Mileage = 1;
            //                item.MileageUnit = "万公里内";
            //            }
            //            else
            //            {
            //                item.Mileage =Convert.ToDouble(mileage.ToString().Split('.')[0]);
            //                item.MileageUnit = "万公里";
            //            }
            //            #endregion
            //            #region 拍卖状态
            //            if (item.BidStatus == "2")
            //            {
            //                if (item.IsSetRebot == true)
            //                {
            //                    item.BidStatus = "机器人出价";
            //                }
            //                else
            //                {
            //                    item.BidStatus = "出价至" + (item.CurrentPrices + 0.02).ToString("0.00") + "万元";
            //                }
            //            }
            //            else if (item.BidStatus == "0" && item.IsSetRebot == true)
            //            {
            //                item.BidStatus = "机器人出价";
            //            }
            //            else
            //            {
            //                item.BidStatus = ((BidStatusType)Convert.ToInt32(item.BidStatus)).ToString();
            //            }
            //            #endregion
            //            //if(item.DataStatus==3)
            //            //{
            //            //    GetAllEndTime(TvaID, carName);
            //            //}
            //            item.CurrentPrices = Math.Round(item.CurrentPrices, 2);
            //            item.CarTypeAndColor = item.CarType + " (" + item.CarColor + ")";
            //            item.Unit = "万元";
            //            item.Years = item.Years.Substring(2, 2) + "年";
            //            int astate;
            //            if (int.TryParse(item.AttentionState, out astate))
            //                item.AState = astate;
            //        }
            //        this.view.ShowAuctionHallData(message, dateTime);
            //        #endregion
            //    });
            #region 请求模式，被注释
            //base.ServiceProxy.RequestService<List<AuctionHallDataModel>, List<AuctionHallDataNotifyModel>>(request,  DataType.Json, message =>
            //{
            //    #region
            //    foreach (var item in message)
            //    {
            //        #region 里程数
            //        if (item.Mileage != "0")
            //        {
            //            item.Mileage = Convert.ToString(Math.Round(Convert.ToDouble(Convert.ToInt32(item.Mileage) / 10000.00), 4)) + "万公里";
            //        }
            //        #endregion

            //        #region 拍卖状态
            //        if (item.BidStatus == "2")
            //        {
            //            double currentprices = Convert.ToDouble(item.CurrentPrices);
            //            item.BidStatus = "出价至" + Convert.ToString(Math.Round(((currentprices + 200) / 10000.00), 2)) + "万元";
            //        }
            //        else
            //        {
            //            item.BidStatus = ((BidStatusType)Convert.ToInt32(item.BidStatus)).ToString();
            //        }
            //        if (item.CurrentPrices != "0.00")
            //        {
            //            double currentprices = Convert.ToDouble(item.CurrentPrices);
            //            item.CurrentPrices = Convert.ToString(Math.Round((currentprices / 10000.00), 2));
            //        }
            //        #endregion
            //        item.CarTypeAndColor = item.CarType + " (" + item.CarColor + ")";
            //        item.Unit = "万元";
            //        item.Years = item.Years.Substring(0, 4) + "年";
            //    }
            //    this.view.ShowAuctionHallData(message);
            //    #endregion

            //    //base.ServiceProxy.Subscribe<List<AuctionHallDataModel>>(request,  DataType.Json, Conveter =>
            //    //{

            //    //});
            //});
            #endregion
        }
    }
}
