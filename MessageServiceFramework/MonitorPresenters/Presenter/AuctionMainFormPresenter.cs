using System;
using System.Collections.Generic;
 
using TranstarAuction.Presenters.ViewInterface;
using TranstarAuction.Model.AuctionMain;
using TranstarAuction.Presenters;
using TranstarAuction.Model;
using System.Timers;
using PWMIS.EnterpriseFramework.Common;
using PWMIS.EnterpriseFramework.Service.Basic;
namespace TranstarAuction.Presenters.Presenter
{
    public class AuctionMainFormPresenter : PresenterBase
    {
        public IAuctionMainForm AuctionMainForm;
        private static object lock_obj = new object();

        public AuctionMainFormPresenter(IAuctionMainForm auctionMainForm)
        {
            this.AuctionMainForm = auctionMainForm;
            InitTimer();
        }

        #region add by zyl 添加timer每5分钟刷新一次保证金


        private Timer _timer = new Timer();

        private void InitTimer()
        {
#if DEBUG
            _timer.Interval = 1000 * 5;//方便测试设置为5秒
#else
            _timer.Interval = 1000 * 60 * 5;//方便测试设置为5秒
#endif

            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            _timer.Start();
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //主窗体整合，注释，完成后，需要取消
            GetCustomerInfoList();
        }

        public void StopTimer()
        {
            _timer.Stop();
            _timer.Dispose();
        }
        #endregion

        /// <summary>
        /// 得到客户信息列表
        /// </summary>
        public void GetCustomerInfoList()
        {

            // string url = "Service://AuctionMainFormService/GetAuctionCustomerInfoModel/System.Int32=41";
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "GetAuctionCustomerInfoModel";
            request.Parameters = new object[] { CurrentUser.UserID };
            //request.Parameters = new object[] { 41 };
            base.ServiceProxy.RequestService<AuctionCustomerInfoModel>(request,  DataType.Json, message =>
            {
                this.AuctionMainForm.BindDataCustomerInfo(message);
            });
        }

        #region 重构，无用，注释
        /// <summary>
        /// 得到结束时间内车实体列表
        /// </summary>
        //public void GetEndTimeAttentionCarModelList()
        //{
        //   // string url = "Service://AuctionMainFormService/GetAuctionTitleDataList/";
        //     ServiceRequest request = new  ServiceRequest();
        //    request.ServiceName = "AuctionMainFormService";
        //    request.MethodName = "GetAuctionTitleDataList";
        //    base.ServiceProxy.RequestService<List<AttentionCarModel>>(request,  DataType.Json, message =>
        //    {
        //        this.AuctionMainForm.BindAttentionCar(message);
        //    });
        //}


        ///// <summary>
        ///// 生成真正的NotifyModel
        ///// </summary>
        ///// <param name="models">具有NotifyModel基类的集合</param>
        ///// <returns></returns>
        //private List<IAttentionCarModel> GetRealNotifyModel(List<AttentionCarNotifyModel> models)
        //{
        //    List<IAttentionCarModel> list = new List<IAttentionCarModel>();
        //    foreach (var item in models)
        //    {
        //        item.CurrentAuctionMoneyStr = item.CurrentAuctionMoney.ToString("0.00");
        //        item.Unit = "万元";
        //        //item.CarName = item.CarName + "  " + item.CarYear.Substring(2, 2) + "年";
        //        if (item.BidStatus == BidStatusType.出价不是最高)//"2"
        //        {
        //            if (item.IsSetRebot == true)
        //            {
        //                item.AuctionState = "机器人出价";
        //            }
        //            else
        //            {
        //                double currentprices = Convert.ToDouble(item.CurrentAuctionMoney);
        //                double addDegree = 0.02;
        //                item.AuctionState = "出价至            " + (item.CurrentAuctionMoney + addDegree).ToString("0.00") + "万元";
        //            }
        //        }
        //        else if (item.BidStatus == BidStatusType.确认条件 && item.IsSetRebot == true)//"0" by 120131 hanshijie (没出价情况下设置机器人，状态应为：“机器人出价”)
        //        {
        //            item.AuctionState = "机器人出价";
        //        }
        //        else
        //        {
        //            item.AuctionState = item.BidStatus.ToString();// ((BidStatusType)Convert.ToInt32(item.BidStatus)).ToString();
        //        }

        //        //
        //        //IAttentionCarModel realNotifyModel = IBMP.AOP.Utility.Create<IAttentionCarModel>(item);
        //        IAttentionCarModel realNotifyModel = item;
        //        list.Add(realNotifyModel);
        //    }
        //    return list;
        //}

        ///// <summary>
        ///// 得到结束时间内车实体列表
        ///// </summary>
        //public void GetEndTimeAttentionCarModelList11(int tvaId, string endTime)
        //{
        //    // string url = "Service://AuctionMainFormService/GetAuctionTitleDataList/";
        //     ServiceRequest request = new  ServiceRequest();
        //    request.ServiceName = "AuctionMainFormService";
        //    request.MethodName = "GetAuctionTitleDataList";
        //    request.Parameters = new object[] { tvaId, endTime };
        //    //改为订阅模式，服务器推送
        //    this.AuctionMainForm.MainPresenter.ServiceProxy.Subscribe<List<AttentionCarModel>, List<AttentionCarNotifyModel>>(request,
        //         DataType.Json,
        //        converter =>
        //        {
        //            var models = converter.Result;
        //            var list = GetRealNotifyModel(models);
        //            if (list!=null&&list.Count > 0)
        //            {
        //                //this.AuctionMainForm.BindAttentionCar11(list);

        //                //foreach (var l in list)
        //                //{
        //                //    AttentionCarNotifyModel l1 = (AttentionCarNotifyModel)l;
        //                //    l1.dataChange = AuctionMainForm;
        //                //    l1.RaiseChange();
        //                //}
        //            }
        //        }
        //        );


        //}


        ///// <summary>
        ///// 得到结束时间内车实体列表
        ///// </summary>
        //public void GetEndTimeAttentionCarModelList16(int tvaId, string endTime)
        //{
        //    // string url = "Service://AuctionMainFormService/GetAuctionTitleDataList/";
        //     ServiceRequest request = new  ServiceRequest();
        //    request.ServiceName = "AuctionMainFormService";
        //    request.MethodName = "GetAuctionTitleDataList";
        //    request.Parameters = new object[] { tvaId, endTime };
        //    //改为订阅模式，服务器推送
        //    this.AuctionMainForm.MainPresenter.ServiceProxy.Subscribe<List<AttentionCarModel>, List<AttentionCarNotifyModel>>(request,
        //         DataType.Json,
        //        converter =>
        //        {
        //            var models = converter.Result;
        //            var list = GetRealNotifyModel(models);
        //            //if (list.Count > 0)
        //            //    this.AuctionMainForm.BindAttentionCar16(list);
        //        }
        //        );

        //    //base.ServiceProxy.RequestService<List<AttentionCarModel>>(request,  DataType.Json, message =>
        //    //{
        //    //    this.AuctionMainForm.BindAttentionCar(message);
        //    //});
        //}

        ///// <summary>
        ///// 得到结束时间内车实体列表
        ///// </summary>
        //public void GetEndTimeAttentionCarModelListOther(int tvaId, string endTime)
        //{
        //     ServiceRequest request = new  ServiceRequest();
        //    request.ServiceName = "AuctionMainFormService";
        //    request.MethodName = "GetAuctionTitleDataList";
        //    request.Parameters = new object[] { tvaId, endTime };
        //    //改为订阅模式，服务器推送
        //    this.AuctionMainForm.MainPresenter.ServiceProxy.Subscribe<List<AttentionCarModel>, List<AttentionCarNotifyModel>>(request,
        //         DataType.Json,
        //        converter =>
        //        {
        //            var models = converter.Result;
        //            var list = GetRealNotifyModel(models);
        //            //if (list.Count > 0)
        //            //    this.AuctionMainForm.BindAttentionCarOther(list);
        //        }
        //        );

        //    //base.ServiceProxy.RequestService<List<AttentionCarModel>>(request,  DataType.Json, message =>
        //    //{
        //    //    this.AuctionMainForm.BindAttentionCar(message);
        //    //});
        //}

        #endregion
    }
}
