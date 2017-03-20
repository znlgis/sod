using System;
using System.Collections.Generic;
using System.ServiceModel;
using MessagePublishService;
using PWMIS.EnterpriseFramework.Common;
using PWMIS.EnterpriseFramework.Common.Hardware;

namespace MessageSubscriber
{
    /// <summary>
    /// 订阅者；
    /// </summary>
    public class Subscriber : ISubscriber, IDisposable
    {
        private string _serviceUri;
        private MessageListener _listener;//= new MessageListener();
        private IMessagePublishService _serviceProxy;
        private bool _registed = false;
        private int _closedFlag;
        private string _receivedMessage = "";
        private bool _waitCallbacking;
        private int waitTimeOut = 360 * 100;//同步的等待响应的最大时间，为3分钟
        private static object _syncObject = new object();

        //private MyFunc<string, object> _currFunc;
        private Action<string> _currAction;

        private int _messageId = 0;
        private string _currSendMessage = "";
        private Dictionary<int, Action<string>> _dictAction;
        private Dictionary<int, Type> _dictResultType;
        private Dictionary<int, MyFunc<string, string>> _dictFunction;

        private DateTime heartBeatTime = DateTime.Now;//心跳发生的时间

        /// <summary>
        /// 服务器正在出版消息
        /// </summary>
        public event EventHandler<MessageEventArgs> PublishingMessage;

        public event EventHandler<MessageEventArgs> ErrorMessage;

        public event EventHandler<MessageEventArgs> HeartBeatError;

        public bool Closed { get; private set; }
        /// <summary>
        /// (同步模式下面的)请求响应的超时时间
        /// </summary>
        public int TimeOut
        {
            get { return waitTimeOut; }
            set { waitTimeOut = value; }
        }
        /// <summary>
        /// 监听器是否已经注册
        /// </summary>
        public bool Registed
        {
            get
            {
                return _registed;
            }
        }

        ///// <summary>
        ///// 订阅服务方法的时候的结果类型
        ///// </summary>
        //public Type ResultType
        //{
        //    get { return _listener.ResultType; }
        //    set { _listener.ResultType = value; }
        //}

        private Dictionary<int, MyFunc<string, string>> DictFunction
        {
            get
            {
                if (_dictFunction == null)
                    _dictFunction = new Dictionary<int, MyFunc<string, string>>();
                return _dictFunction;
            }
        }
        /// <summary>
        /// 以一个服务订阅地址初始化本类
        /// </summary>
        /// <param name="serviceUri"></param>
        public Subscriber(string serviceUri)
        {
            _serviceUri = serviceUri;
            _listener = new MessageListener(this);
            _dictAction = new Dictionary<int, Action<string>>();
            _dictResultType = new Dictionary<int, Type>();

            //Closed = true;
        }

        /// <summary>
        /// 重新初始化监听器，并发起订阅
        /// </summary>
        public void ReOpen()
        {
            _listener = new MessageListener(this);
            //Closed = true;
            Subscribe();
        }

        /// <summary>
        /// 发起订阅并注册身份
        /// </summary>
        public void Subscribe()
        {
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            binding.MaxBufferSize = int.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = 65536;
            binding.ReaderQuotas.MaxBytesPerRead = 10 * 1024 * 1024;
            binding.ReaderQuotas.MaxStringContentLength = 10 * 1024 * 1024; //10M;
            binding.ReceiveTimeout = TimeSpan.MaxValue;//设置连接自动断开的空闲时长；

            binding.SendTimeout = new TimeSpan(0, 10, 9);
            _serviceProxy = DuplexChannelFactory<IMessagePublishService>.CreateChannel(_listener, binding, new EndpointAddress(_serviceUri));
            //下面通过配置，仅供测试
            //_serviceProxy = DuplexChannelFactory<IMessagePublishService>.CreateChannel(new InstanceContext(_listener), "defaultEndpoint");
            try
            {
                string indentity = "PDF.NET;20111230;" + HardDiskSN.SerialNumber;
                _serviceProxy.QuikRegist(indentity);
                _registed = true;
                Closed = false;
            }
            catch (Exception ex)
            {
                OnErrorMessage(string.Format("Subscribe Error,ErrorMessage:{0}", ex.Message));
            }
        }

        /// <summary>
        /// 向发布服务器发送消息（本地不等待服务器回调）。注意，调用本方法将清除原有的接收消息的自定义处理方法
        /// </summary>
        /// <param name="msg"></param>
        public void SendMessage(string msg)
        {
            _receivedMessage = "";
            _currAction = null;
            try
            {
                _currSendMessage = msg;
                _serviceProxy.OnAccept(this.CreateMessageID(), msg);
            }
            catch (Exception ex)
            {
                OnErrorMessage(string.Format("SendMessage Error,msg:{0},ErrorMessage:{1}", msg.Length > 255 ? msg.Substring(0, 255) : msg, ex.Message));
            }
        }

        /// <summary>
        /// （请求-响应模式）请求返回服务器的消息
        /// </summary>
        /// <param name="msg">发送给服务器的消息</param>
        /// <returns></returns>
        public string RequestMessage(string msg)
        {
            _receivedMessage = "";
            _currAction = null;
            try
            {
                return _serviceProxy.OnRequest(msg);
            }
            catch (Exception ex)
            {
                OnErrorMessage(string.Format("RequestMessage Error,msg:{0},ErrorMessage:{1}", msg, ex.Message));
            }
            return "";
        }

        /// <summary>
        /// 在订阅模式下，向发布端发送文本消息
        /// </summary>
        /// <param name="msg"></param>
        public void SendTextMessage(string msg)
        {
            SendTextMessage(this.CreateMessageID(), msg);
        }

        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="messageId">消息编号</param>
        /// <param name="msg">给服务器的消息</param>
        public void SendTextMessage(int messageId, string msg)
        {
            _receivedMessage = "";
            try
            {
                _serviceProxy.OnAccept(messageId, msg);
            }
            catch (Exception ex)
            {
                OnErrorMessage(string.Format("SendTextMessage Error,msg:{0},ErrorMessage:{1}", msg, ex.Message));
            }
        }

        /// <summary>
        /// 向服务器发送消息，当服务器处理完后，会回调当前提供的回调方法
        /// </summary>
        /// <param name="message">发给服务器的消息</param>
        /// <param name="ResultType">结果的对象类型</param>
        /// <param name="action">客户端的回调方法</param>
        /// <returns>消息标识</returns>
        public int SendMessage(string message, Type ResultType, Action<string> action)
        {
            return SendMessage(message, ResultType, action, null, null);
        }

        /// <summary>
        /// 向服务器发送消息，当服务器处理完后，会回调当前提供的回调方法
        /// </summary>
        /// <param name="message">发给服务器的消息</param>
        /// <param name="ResultType">结果的对象类型</param>
        /// <param name="action">客户端的回调方法</param>
        /// <param name="function">在执行过程中回调的客户端函数</param>
        /// <param name="previewFunction">在执行过程前预先回调的客户端函数</param>
        /// <returns>消息标识</returns>
        public int SendMessage(string message, Type ResultType, Action<string> action, MyFunc<string, string> function, MyFunc<string, string> previewFunction)
        {
            _receivedMessage = "";
            if (this.Closed)
            {
                OnErrorMessage("通信连接已经关闭。");
                return 0;
            }
            try
            {
                int messageId = this.CreateMessageID();
                _dictAction.Add(messageId, action);
                _dictResultType.Add(messageId, ResultType);
                if (function != null)
                    DictFunction.Add(messageId, function);
                if (previewFunction != null)
                    DictFunction.Add(int.MaxValue - messageId, previewFunction);

                _serviceProxy.OnAccept(messageId, message);
                //服务端接受消息后，可能要等段时间才会回调OnReceivingMessage 方法，这里传入要处理的方法委托
                _currAction = action;
                _waitCallbacking = true;
                this._closedFlag = 1;//允许回调完成，监听器取消注册。
                return messageId;
            }
            catch (Exception ex)
            {
                OnErrorMessage(string.Format("SendTextMessage Error,msg:{0},ErrorMessage:{1}", message, ex.Message));
            }
            return 0;
        }

        /// <summary>
        /// 移除消息，不在处理来自服务的消息，主要用于取消订阅
        /// </summary>
        /// <param name="messageId"></param>
        public void RemoveMessage(int messageId)
        {
            _dictAction.Remove(messageId);
            _dictResultType.Remove(messageId);
        }

        /// <summary>
        /// 接收来自发布服务器的响应的消息，并调用在发送消息的时候提供的客户端回调方法。
        /// </summary>
        /// <param name="msg"></param>
        public void OnReceivingMessage(int id, string msg)
        {
            if (checkHeartBeat(id, msg))
                return;

            _receivedMessage = msg;
            _waitCallbacking = false;
            if (_currAction != null)
            {
                Action<string> action = _dictAction[id];
                action(msg);
            }
        }

        /// <summary>
        /// 接收发布服务器发布的消息,如果消息送达,则触发消息发布事件
        /// </summary>
        /// <param name="msg"></param>
        public void OnPublishMessage(int id, string msg)
        {
            if (checkHeartBeat(id, msg))
                return;

            _waitCallbacking = true;
            if (_dictAction.Count > 0 && _dictAction.ContainsKey(id))
            {
                Action<string> action = _dictAction[id];
                action(msg);
            }
            _waitCallbacking = false;
        }

        /// <summary>
        /// 等待服务器器响应消息，然后执行自定义的消息处理方法。本操作用于同步模式
        /// </summary>
        /// <param name="action">自定义的消息处理方法</param>
        public void Wait(Action<string> action)
        {
            int count = 0;
            while (!Closed)
            {
                System.Threading.Thread.Sleep(5);
                if (_receivedMessage != "")
                {
                    action(_receivedMessage);
                    System.Diagnostics.Debug.WriteLine("wait count:" + count);
                    break;
                }
                else
                {
                    count++;
                    if (count > waitTimeOut)
                    {
                        Closed = true;
                        OnErrorMessage("同步请求服务超时，任务退出。Send Message:" + _currSendMessage);
                        break;
                    }
                }
            }
        }

        public string RequestMessageType(int id)
        {
            if (_dictResultType.ContainsKey(id) && _dictResultType[id] != null)
                return _dictResultType[id].FullName;
            else
                return "";
        }

        /// <summary>
        /// 服务回调客户端，获取客户端返回的消息
        /// </summary>
        /// <param name="id">消息编号</param>
        /// <param name="para">消息参数</param>
        /// <returns></returns>
        public string CallBackMessage(int id, string para)
        {
            if (DictFunction.ContainsKey(id) && DictFunction[id] != null)
                return DictFunction[id](para);
            else
                return "";
        }

        /// <summary>
        /// 预先回调客户端，获取客户端返回的消息。可以在 CallBackMessage 执行之前进行预先的调用,以决定下面的操作
        /// </summary>
        /// <param name="id">消息编号</param>
        /// <param name="para">消息参数</param>
        /// <returns></returns>
        public string PreCallBackMessage(int id, string para)
        {
            int preId = int.MaxValue - id;
            if (DictFunction.ContainsKey(preId) && DictFunction[preId] != null)
                return DictFunction[preId](para);
            else
                return "";
        }

        #region IDisposable 成员

        public void Dispose()
        {
            if (_listener != null)
                innerDispose();
        }

        #endregion

        private void innerDispose()
        {
            int count = 0;
            while (_waitCallbacking)
            {
                System.Threading.Thread.Sleep(100);
                count++;
                if (count > waitTimeOut)
                {
                    Closed = true;
                    OnErrorMessage("等待服务回调超时，innerDispose退出。Send Message:" + _currSendMessage);
                    break;
                }
            }

            try
            {
                if (_serviceProxy != null)
                {
                    if (this._closedFlag != 0) //关闭标志==0 表示在注册订阅阶段发生的事件，指示未注册成功
                        _serviceProxy.Unregist();
                    if (_registed)
                        (_serviceProxy as IDisposable).Dispose();
                }
            }
            catch (Exception ex)
            {
                OnErrorMessage(string.Format("innerDispose Error {0}", ex.Message));
            }
            finally
            {

            }

            _listener = null;
            _serviceProxy = null;
        }

        private void OnErrorMessage(string message)
        {
            this.Closed = true;
            if (this.ErrorMessage != null)
                this.ErrorMessage(this, new MessageEventArgs(message));
        }

        /// <summary>
        /// 创建一个当前会话内的新消息编号
        /// </summary>
        /// <returns></returns>
        private int CreateMessageID()
        {
            //lock (_syncObject)
            //{
            //    return ++_messageId;
            //}
            return System.Threading.Interlocked.Increment(ref _messageId);
        }

        /// <summary>
        /// 服务端通知客户端关闭监听器
        /// </summary>
        /// <param name="flag"></param>
        public void Close(int flag)
        {
            this._closedFlag = flag;
            _waitCallbacking = false;
            if (this._registed)
            {
                innerDispose();
            }
            this.Closed = true;
        }

        /// <summary>
        /// 是否是在检查心跳情况，该方法由服务器触发
        /// </summary>
        /// <param name="id">消息编号</param>
        /// <param name="msg">心跳标记</param>
        /// <returns> 是否是在检查心跳情况</returns>
        private bool checkHeartBeat(int id, string msg)
        {
            if (id < 1 && msg == "heart beat")
            {
                heartBeatTime = DateTime.Now;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检查心跳情况，如果超过一定时间还没有收到心跳相应，可能服务器已经发生异常，并抛出异常通知客户端。该方法由客户端主动调用
        /// </summary>
        /// <returns></returns>
        public bool CheckHeartBeat()
        {
            if (DateTime.Now.Subtract(heartBeatTime).TotalSeconds >= 60) //心跳间隔不能超过1分钟
            {
                OnErrorMessage("服务器在指定的时间内容没有响应，可能已经断开跟客户端的连接。");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 检查心跳情况的线程计时器回调对象
        /// </summary>
        /// <returns></returns>
        public System.Threading.TimerCallback CheckHeartBeatCallBack()
        {
            return new System.Threading.TimerCallback(o =>
            {
                if (DateTime.Now.Subtract(heartBeatTime).TotalSeconds >= 60)
                {
                    if (HeartBeatError != null)
                        HeartBeatError(this, new MessageEventArgs("服务器在指定的时间内容没有响应，可能已经断开跟客户端的连接。"));
                }
            });
        }
    }
}
