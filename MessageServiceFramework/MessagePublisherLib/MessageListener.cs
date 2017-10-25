using System;
using System.Collections.Generic;
using System.Text;
using MessagePublishService;

namespace MessagePublisher
{
    /// <summary>
    /// 服务端消息监听器
    /// </summary>
    public class MessageListener
    {
        public string FromIP { get; private set; }

        public int FromPort { get; private set; }
        /// <summary>
        /// 当前消息，注意此消息可能会变
        /// </summary>
        public string FromMessage { get; set; }
        /// <summary>
        /// 当前消息编号，注意此ID可能会改变
        /// </summary>
        public int MessageID { get; set; }

        public MessageUser User { get; set; }
        /// <summary>
        /// 消息订阅者的会话标识，由客户端的IP，端口号，客户端的标识和时间格式字符串（HHmmssfff）四部分组成
        /// </summary>
        public string SessionID { get; set; }
        /// <summary>
        /// 客户端硬件标识
        /// </summary>
        public string Identity;
        /// <summary>
        /// 接收到消息的时间
        /// </summary>
        public DateTime AtTime { get; private set; }

        /// <summary>
        /// 是否移除不再使用
        /// </summary>
        public bool Removed { get; set; }

        private IMessageListenerCallBack _innerListener;
        //public IMessageListener InnerListener
        //{
        //    get { return _innerListener; }
        //}

        public IMessageListenerCallBack GetListener()
        {
            return _innerListener;
        }

        public MessageListener(string fromIP, int fromPort, IMessageListenerCallBack innerListener)
        {
            this.FromIP = fromIP;
            this.FromPort = fromPort;
            this.AtTime = DateTime.Now;
            _innerListener = innerListener;
        }

        public MessageListener(string fromIP, int fromPort, IMessageListenerCallBack innerListener, int id, string message)
        {
            this.FromIP = fromIP;
            this.FromPort = fromPort;
            this.FromMessage = message;
            this.MessageID = id;
            this.AtTime = DateTime.Now;
            _innerListener = innerListener;
        }

        /// <summary>
        /// 通知消息；
        /// </summary>
        /// <param name="message"></param>
        public void Notify(int id, string message)
        {
            if (!this.Removed)//没有起到真正的作用，在移出方法那里移出的不是同一个对象
                _innerListener.OnPublish(id, message);
        }

        /// <summary>
        /// 响应请求，回复消息
        /// </summary>
        /// <param name="message"></param>
        public void Response(int id, string message)
        {
            _innerListener.OnReceive(id, message);
        }

        /// <summary>
        /// 获取客户端的标识
        /// </summary>
        /// <returns></returns>
        public string GetIdentity()
        {
            this.Identity = _innerListener.GetIdentity();
            return this.Identity;
        }

        public string RequestMessageType()
        {
            return _innerListener.RequestMessageType(this.MessageID);
        }

        public string CallBackFunction(int msgId, string para)
        {
            return _innerListener.CallBackMessage(msgId, para);
        }

        public string PreCallBackFunction(int msgId, string para)
        {
            return _innerListener.PreCallBackMessage(msgId, para);
        }

        /// <summary>
        /// 通知客户端关闭连接
        /// </summary>
        public void Close(int flag)
        {
            _innerListener.OnClose(flag);
        }

        public override bool Equals(object obj)
        {
            bool eq = base.Equals(obj);
            if (!eq)
            {
                MessageListener lstn = obj as MessageListener;
#if(MONO)
                //下面一行用于mono,FromIP 是sessionid
                if (this.FromIP == lstn.FromIP && this.FromPort == lstn.FromPort)
                    eq = true;
#else
                if (lstn._innerListener.Equals(this._innerListener))
                {
                    eq = true;
                }
#endif
            }
            return eq;
        }
    }
}
