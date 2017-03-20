using System;
using System.Collections.Generic;
 
using TranstarAuction.Model;
using System.Timers;
using TranstarAuction.Presenters.ViewInterface;
using PWMIS.EnterpriseFramework.Common;
using PWMIS.EnterpriseFramework.Service.Basic;

namespace TranstarAuction.Presenters.Presenter
{
    public class NotifyMessageArgs : EventArgs
    {
        public List<NotifyMessageModel> MessageList { get; set; }
    }

    /// <summary>
    /// 全局消息事件
    /// </summary>
    public class GlobalMessageArgs : EventArgs
    {
        public string MessageText { get; private set; }
        public object Parameter { get; private set; }

        public GlobalMessageArgs(string text, object para)
        {
            MessageText = text;
            Parameter = para;
        }
    }

    /// <summary>
    /// 主窗体 逻辑主持人
    /// </summary>
    public class MainFormPresenter : PresenterBase, IGlobalPresenter
    {
        private SysConfigPresenter _configPresenter;
        /// <summary>
        /// 用户配置处理
        /// </summary>
        public SysConfigPresenter ConfigPresenter
        {
            get { return _configPresenter; }
            set { _configPresenter = value; }
        }


        /// <summary>
        /// 提醒消息处理
        /// </summary>
        public NotifyPresenter Notifypresenter { get; set; }
        private Timer _timer = new Timer();
        public event EventHandler<NotifyMessageArgs> NotifyMessage;

        #region add by 周燕龙

        /// <summary>
        /// 主窗体View
        /// </summary>
        public IAuctionMainFormView View;

        /// <summary>
        /// 文件更新
        /// </summary>
        private UpdateFilePresenter uppresenter = new UpdateFilePresenter();

        public MainFormPresenter(IAuctionMainFormView view)
        {
            Notifypresenter = new NotifyPresenter(this);
            this.View = view;
            InitTimer();
            //Notifypresenter.ServiceProxy.ErrorMessage += new EventHandler<MessageSubscriber.MessageEventArgs>(ServiceProxy_ErrorMessage);
            base.ServiceProxy.ErrorMessage += new EventHandler<MessageSubscriber.MessageEventArgs>(ServiceProxy_ErrorMessage);

        }

        void ServiceProxy_ErrorMessage(object sender, MessageSubscriber.MessageEventArgs e)
        {
            base.OnServiceProxyError(string.Format("source:{0}\r\nError Message:\r\n{1}", sender == null ? "null" : sender.GetType().FullName, e.MessageText));
            View.ShowMsgAndLogout("网络错误", "请重新登录！");
        }

        public void CancelErrorMessageEventHandle()
        {
            this.ServiceProxy.ErrorMessage -= ServiceProxy_ErrorMessage;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        public void SubscibeUpdateFile(bool isNotifyNoFile)
        {
            uppresenter.GetFiles(this.View, isNotifyNoFile);
            // SubscibeUpdateFile2(isNotifyNoFile);
        }

        public void SubscibeUpdateFile(bool isNotifyNoFile, string path)
        {
            uppresenter.GetFiles(this.View, isNotifyNoFile, path);
            // SubscibeUpdateFile2(isNotifyNoFile);
        }

        /// <summary>
        /// 偷偷拷贝文件
        /// </summary>
        public void CopyUpdateFile()
        {
            uppresenter.CopyFile();
        }


        /// <summary>
        /// 是否特殊目录接受UAC控制
        /// </summary>
        /// <returns></returns>
        public bool IsUAC()
        {
            return uppresenter.IsUAC();
        }

        //public void SubscibeUpdateFile2(bool isNotifyNoFile)
        //{
        //    System.Threading.Thread t = new System.Threading.Thread(delegate() { uppresenter.GetFiles(this.View, isNotifyNoFile); });
        //    t.Start();

        //}

        #endregion

        private void InitTimer()
        {
            _timer.Interval = 1000 * 60;
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Notifypresenter.RequestNotifyMessageList(message =>
            {
                OnNotifyMessage(message);
            });
        }

        private void OnNotifyMessage(List<NotifyMessageModel> messagelist)
        {
            if (NotifyMessage != null)
            {
                NotifyMessage(this, new NotifyMessageArgs() { MessageList = messagelist });
            }
        }

        /// <summary>
        /// 订阅通知消息
        /// </summary>
        public void SubscibeNotify()
        {
            //notifypresenter.RequestNotifyMessageList(message =>
            //{
            //    OnNotifyMessage(message);
            //});
            _timer.Start();

            // ServiceRequest request = new  ServiceRequest();
            //request.ServiceName = "AuctionMainFormService";
            //request.MethodName = "PublishSubscibeNotify";
            //base.ServiceProxy.Subscribe<NotifyMessageModel, SubNotifyMessageModel>(request,  DataType.Json, o =>
            //    {
            //        if (o.Scceed)
            //        {
            //            o.Result.onsubmit();
            //        }
            //    });

            //base.ServiceProxy.Subscribe<NotifyMessageModel>(request,  DataType.Json, o =>
            //{
            //    if (o.Scceed)
            //    {
            //        OnNotifyMessage(o.Result);                    
            //    }
            //});
        }

        /// <summary>
        /// 取消消息订阅
        /// </summary>
        public void CancelSubscibeNotify()
        {
            _timer.Stop();
        }

        /// <summary>
        /// 关闭通信连接并清除会话
        /// </summary>
        public void LogoutServiceIdentity()
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "User";
            request.MethodName = "Logout";
            request.Parameters = new object[] { };
            base.ServiceProxy.RequestService<bool>(request,  DataType.Text, null);
        }

        /// <summary>
        /// 关闭通信连接并清除会话
        /// </summary>
        public void CloseConnect()
        {
            LogoutServiceIdentity();
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionHallService";
            request.MethodName = "UnSubscribeService";
            request.Parameters = new object[] { };
            base.ServiceProxy.Connect();
            var result = base.ServiceProxy.GetServiceMessage<bool>(request,  DataType.Text);
            base.ServiceProxy.Close();
        }

        /// <summary>
        /// 获取黑名单列表
        /// </summary>
        public List<int> GetAllTvas()
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "GetAllBlackTvas";
            request.Parameters = new object[] { PresenterBase.CurrentUser.TvalID };

            var result = base.ServiceProxy.GetServiceMessage<List<int>>(request,  DataType.Text);
            if (result.Succeed)
                return result.Result;
            return null;
        }

        public void SubscibeCheckUserIdentity(Action<bool> CheckComplete)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "User";
            request.MethodName = "CheckUserIdentity";
            request.Parameters = new object[] { };

            base.ServiceProxy.Subscribe<bool>(request,  DataType.Text, o =>
                {
                    if (o.Succeed)
                    {
                        if (CheckComplete != null)
                        {
                            CheckComplete(o.Result);
                        }
                    }
                });
            //已经使用长连接的方式，每分钟去请求消息服务，这样也可以监测到网络异常，故不用开启心跳检测
            //base.ServiceProxy.StartCheckHeartBeat();
        }

        #region 接口方法
        /// <summary>
        /// 发送消息给全局主持人，由主持人决定如何进一步处理消息
        /// </summary>
        /// <param name="sender">发送人</param>
        /// <param name="message">消息内容</param>
        /// <param name="paraData">消息附带的数据</param>
        public void SendMessage(object sender, string message, object paraData)
        {
            if (message == "ChangeDataDay") //切换标签为今日或者明日
            {
                if (ChangeDataDay != null)
                    ChangeDataDay(this, new GlobalMessageArgs(message, paraData));
            }
        }

        /// <summary>
        /// 订阅大厅全部数据
        /// </summary>
        /// <param name="trvid"></param>
        public void SubscribeAutionHallData(int trvid)
        {
            //MockGetAllHallData();
            GetAllHallData(PresenterBase.CurrentUser.TvalID);
        }

        /// <summary>
        /// 全局数据源
        /// </summary>
        public List<IAuctionHallDataModel> GlobalDataSource { get; set; }

        public event EventHandler DataSourceChange;

        /// <summary>
        /// 切换数据面板的日期
        /// </summary>
        public event EventHandler<GlobalMessageArgs> ChangeDataDay;

        private void MockGetAllHallData()
        {
            List<IAuctionHallDataModel> list = new List<IAuctionHallDataModel>();
            list.Add(new AuctionHallDataNotifyModel() { CarType = "奔驰[黑色]", Years = "2011", AuctionStatus = "拍卖中", AttentionState = "1", BuyerId = 1, Unit = "12.0", Type = DataSourceTime.End11 });
            list.Add(new AuctionHallDataNotifyModel() { CarType = "大众[灰色]", Years = "2010", AuctionStatus = "拍卖中", AttentionState = "1", BuyerId = 2, Unit = "12.0", Type = DataSourceTime.End16 });
            list.Add(new AuctionHallDataNotifyModel() { CarType = "菱木[蓝色]", Years = "2012", AuctionStatus = "拍卖中", AttentionState = "1", BuyerId = 3, Unit = "12.0", Type = DataSourceTime.End14 });

            this.GlobalDataSource = list;
            if (DataSourceChange != null)
                DataSourceChange(this, new EventArgs());
        }

        private void GetAllHallData(int TvaID)
        {
            string dateTime = DateTime.Now.ToString("yyyy-MM-dd");
            string carName = "";
            string TypeOrderbyStr = "";

             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionHallService";
            request.MethodName = "GetAuctionHallDataList";
            request.Parameters = new object[] { TvaID, dateTime, carName, TypeOrderbyStr };
            //订阅模式，推送数据
            base.ServiceProxy.Subscribe<List<AuctionHallDataModel>, List<AuctionHallDataNotifyModel>>(
                request,
                 DataType.Json,
                converter =>
                {
                    var message = converter.Result;
                    //List<long> pids = new List<long>();
                    //foreach (AuctionHallDataNotifyModel ahm in converter.Result)
                    //{

                    //    long pid = ahm.PublishId;
                    //    if ((int)ahm.BidStatus < 3)
                    //        pids.Add(pid);
                    //}
                    #region
                    foreach (var item in message)
                    {
                        if (item.PublishId == 0)
                        {
                            continue;
                        }

                        #region 里程数
                        //double mileage = Math.Round(item.Mileage / 10000.00, 4);
                        //if (mileage < 1)
                        //{
                        //    item.Mileage = 1;
                        //    item.MileageUnit = "万公里内";
                        //}
                        //else
                        //{
                        //    item.Mileage = Convert.ToDouble(mileage.ToString().Split('.')[0]);
                        //    item.MileageUnit = "万公里";
                        //}
                        #endregion

                        #region 拍卖状态
                        //if (item.BidStatus == "2")
                        //{
                        //    if (item.IsSetRebot == true)
                        //    {
                        //        item.BidStatus = "机器人出价";
                        //    }
                        //    else
                        //    {
                        //        item.BidStatus = "出价至" + (item.CurrentPrices + 0.02).ToString("0.00") + "万元";
                        //    }
                        //}
                        //else if (item.BidStatus == "0" && item.IsSetRebot == true)
                        //{
                        //    item.BidStatus = "机器人出价";
                        //}
                        //else
                        //{
                        //    item.BidStatus = ((BidStatusType)Convert.ToInt32(item.BidStatus)).ToString();
                        //}
                        #endregion

                        item.CurrentPrices = Math.Round(item.CurrentPrices, 2);
                        item.CarTypeAndColor = item.CarType + " " + item.CarDisplacement + " (" + item.CarColor + ")";
                        item.Unit = "万元";
                        DateTime ldate;
                        DateTime.TryParse(item.Years, out ldate);
                        item.LicenseDate = ldate;
                        item.Years = item.Years.Substring(2, 2) + "年";
                        int astate;
                        if (int.TryParse(item.AttentionState, out astate))
                            item.AState = astate;
                    }//end for

                    this.GlobalDataSource = message.ConvertAll<IAuctionHallDataModel>(i => i);
                    if (DataSourceChange != null)
                        DataSourceChange(this, new EventArgs());

                    #endregion
                });

        }

        #endregion

    }
}
