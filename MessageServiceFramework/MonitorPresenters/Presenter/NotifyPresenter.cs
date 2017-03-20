using System;
using System.Collections.Generic;
 
using TranstarAuction.Model;
using MessageSubscriber;
using PWMIS.EnterpriseFramework.Common;
using PWMIS.EnterpriseFramework.Service.Basic;

namespace TranstarAuction.Presenters.Presenter
{
    public class NotifyPresenter : PresenterBase
    {
        public object lockobj = new object();//消息列表锁对象
        public long LastMaxResponseMsgID = 0;
        private UpdateHisMsgStrategy updatehismsg;
        private List<NotifyMessageModel> _msglist = new List<NotifyMessageModel>();
        PresenterBase mainPresenter { get; set; }
        /// <summary>
        /// 消息列表
        /// </summary>
        public List<NotifyMessageModel> Msglist
        {
            get { return _msglist; }
            set { _msglist = value; }
        }

        public NotifyPresenter(PresenterBase mainpresenter)
        {
            mainPresenter = mainpresenter;
            updatehismsg = new IdleUpdateHisMsgStrategy(this);
        }

        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <param name="action"></param>
        /// <param name="direction">0:向上取1：向下取</param>
        public void RequestNotifyMessageList(Action<List<NotifyMessageModel>> action, long basemsgid, int direction)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "GetMessageList";

            request.Parameters = new object[] { PresenterBase.CurrentUser.UserID, basemsgid, direction };
            //base.ServiceProxy.RequestService<List<NotifyMessageModel>>(request,  DataType.Json, action);
            mainPresenter.ServiceProxy.RequestServiceCurrentSubscribe<List<NotifyMessageModel>>(request,  DataType.Json, action);
        }

        /// <summary>
        /// 获取消息列表（最新）
        /// </summary>
        /// <param name="action">新增消息</param>
        public void RequestNotifyMessageList(Action<List<NotifyMessageModel>> action)
        {
            RequestNotifyMessageList(o =>
                {
                    if (o == null) return;
                    if (o.Count > 0)
                    {
                        o.Sort(new Comparison<NotifyMessageModel>((n1, n2) =>
                            {
                                if (n1.MsgID < n2.MsgID) return 1;
                                else if (n1.MsgID > n2.MsgID) return -1;
                                else return 0;
                            }));

                        lock (lockobj)
                        {
                            if (Msglist.Find(item => item.MsgID == o[0].MsgID) == null)
                            {
                                LastMaxResponseMsgID = o[0].MsgID;
                                if (updatehismsg.LastMinResponseMsgID == 0)
                                    updatehismsg.LastMinResponseMsgID = o[o.Count - 1].MsgID;
                                Msglist.AddRange(o);
                            }
                        }
                    }
                    else
                    {
                        if(!updatehismsg.UpdateComplete)
                            updatehismsg.StartUpdate();
                    }
                    if (action != null)
                        action(o);
                }, LastMaxResponseMsgID, 0);
        }

        /// <summary>
        /// 更新消息状态（已读/未读）
        /// </summary>
        /// <param name="msgid">消息id，以逗号分隔</param>
        /// <param name="action"></param>
        public void RequestUpdateMessage(string msgid, Action<object> action)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "UptMessage";
            request.Parameters = new object[] { PresenterBase.CurrentUser.TvalID, msgid };

            base.ServiceProxy.RequestService<object>(request,  DataType.Text, action); 
        }

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="msgid">消息id，以逗号分隔</param>
        /// <param name="action"></param>
        public void RequestDeleteMessage(string msgid, Action<object> action)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "DelMessage";
            request.Parameters = new object[] { msgid };

            base.ServiceProxy.RequestService<object>(request,  DataType.Text, action);
        }
    }

    public abstract class UpdateHisMsgStrategy
    {
        protected NotifyPresenter notifypresenter;
        public bool UpdateComplete = false;
        public long LastMinResponseMsgID = 0;

        public UpdateHisMsgStrategy(NotifyPresenter p)
        {
            notifypresenter = p;
        }
        public abstract void StartUpdate();
        protected void UpdateMsgList(List<NotifyMessageModel> o)
        {
            o.Sort(new Comparison<NotifyMessageModel>((n1, n2) =>
            {//升序,找出最小消息id
                if (n1.MsgID < n2.MsgID) return -1;
                else if (n1.MsgID > n2.MsgID) return 1;
                else return 0;
            }));

            lock (notifypresenter.lockobj)
            {
                if (notifypresenter.Msglist.Find(item => item.MsgID == o[0].MsgID) == null)
                {
                    LastMinResponseMsgID = o[0].MsgID;
                    notifypresenter.Msglist.AddRange(o);
                }
            }
        }
    }

    public class IdleUpdateHisMsgStrategy : UpdateHisMsgStrategy
    {
        public IdleUpdateHisMsgStrategy(NotifyPresenter p) : base(p) { }
        public override void StartUpdate()
        {
            notifypresenter.RequestNotifyMessageList(o =>
                {
                    if (o.Count > 0)
                    {
                        UpdateMsgList(o);
                    }
                    else
                    {
                        UpdateComplete = true;
                    }
                }, LastMinResponseMsgID, 1);
        }
    }

    public class LoopUpdateHisMsgStrategy : UpdateHisMsgStrategy
    {
        public LoopUpdateHisMsgStrategy(NotifyPresenter p) : base(p) { }
        public override void StartUpdate()
        {
            notifypresenter.RequestNotifyMessageList(o =>
            {
                if (o.Count > 0)
                {
                    UpdateMsgList(o);
                    System.Threading.Thread.Sleep(1000);
                    StartUpdate();
                }
                else
                {
                    UpdateComplete = true;
                }
            }, LastMinResponseMsgID, 1);
        }
    }
}
