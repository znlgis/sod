using System;
using System.Collections.Generic;
 
using System.Timers;
using TranstarAuction.Model.AuctionMain;
using TranstarAuction.Presenters.ViewInterface;
using PWMIS.EnterpriseFramework.Service.Basic;
using PWMIS.EnterpriseFramework.Common;

namespace TranstarAuction.Presenters.Presenter
{
    public class MainUcPresenter : PresenterBase
    {
        IMainUcView ManiUcView;

        public MainUcPresenter(IMainUcView maniUcView)
        {
            ManiUcView = maniUcView;
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
            GetCustomerInfoList();
        }

        #endregion

        /// <summary>
        /// 得到客户信息列表
        /// </summary>
        public void GetCustomerInfoList()
        {

            
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "GetAuctionCustomerInfoModel";
            request.Parameters = new object[] { CurrentUser.UserID };
            //request.Parameters = new object[] { 41 };
            base.ServiceProxy.RequestService<AuctionCustomerInfoModel>(request,  DataType.Json, message =>
            {
                this.ManiUcView.BindDataCustomerInfo(message);
            });


        }
    }
}
