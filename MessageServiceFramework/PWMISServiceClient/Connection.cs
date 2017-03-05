using System;
using System.Collections.Generic;
using System.Text;
using MessageSubscriber;
using PWMIS.EnterpriseFramework.Common;


namespace PWMIS.EnterpriseFramework.Service.Client
{
    /// <summary>
    /// 服务连接对象
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// 处理服务的异常信息
        /// </summary>
        public event EventHandler<MessageEventArgs> ErrorMessage;
        /// <summary>
        /// 服务的基地址
        /// </summary>
        public string ServiceUri { get; set; }
        /// <summary>
        /// 获取服务订阅者
        /// </summary>
        public Subscriber ServiceSubscriber { get; private set; }

        /// <summary>
        /// 是否使用连接池，在订阅模式下，不必设置该属性
        /// </summary>
        public bool UseConnectionPool { get; set; }

        private void ServiceSubscriber_ErrorMessage(object sender, MessageEventArgs e)
        {
            e.MessageText = "Connection Error:" + e.MessageText;
            if (this.ErrorMessage != null)
                this.ErrorMessage(sender, e);
        }

        /// <summary>
        /// 以一个服务基地址初始化本类
        /// </summary>
        /// <param name="serviceUri"></param>
        public Connection(string serviceUri)
        {
            this.ServiceUri = serviceUri;
        }

        /// <summary>
        /// 指定服务的基地址和是否使用连接池
        /// </summary>
        /// <param name="serviceUri"></param>
        /// <param name="pool"></param>
        public Connection(string serviceUri, bool pool)
        {
            this.ServiceUri = serviceUri;
            this.UseConnectionPool = pool;
        }

        /// <summary>
        /// 打开服务连接。在执行请求服务的方法之前，必须先调用本方法。
        /// </summary>
        /// <returns></returns>
        public bool Open()
        {
            if (ServiceSubscriber == null || ServiceSubscriber.Closed)
            {
                if (UseConnectionPool)
                {
                    ServiceSubscriber = PublishServicePool.Instance.GetServiceChannel(this.ServiceUri);
                    if (!ServiceSubscriber.Registed)
                    {
                        ServiceSubscriber.ErrorMessage += new EventHandler<MessageEventArgs>(ServiceSubscriber_ErrorMessage);
                        ServiceSubscriber.Subscribe();
                    }
                    else
                    {
                        if (ServiceSubscriber.Closed)
                            ServiceSubscriber.ReOpen();//尝试打开
                    }
                }
                else
                {
                    ServiceSubscriber = new Subscriber(this.ServiceUri);
                    ServiceSubscriber.ErrorMessage += new EventHandler<MessageEventArgs>(ServiceSubscriber_ErrorMessage);
                    ServiceSubscriber.Subscribe();
                }
            }
            return !ServiceSubscriber.Closed;
        }

        /// <summary>
        /// 向服务器发送消息，当服务器处理完后，会回调当前提供的回调方法
        /// </summary>
        /// <param name="message">发给服务器的消息</param>
        /// <param name="ResultType">结果的对象类型</param>
        /// <param name="action">客户端的回调方法</param>
        /// <returns>消息标识</returns>
        public int RequestService(string message, Type ResultType, Action<string> action)
        {
            return ServiceSubscriber.SendMessage(message, ResultType, action);
        }

        /// <summary>
        /// 向服务器发送消息，当服务器处理完后，会回调当前提供的回调方法
        /// </summary>
        /// <param name="message">发给服务器的消息</param>
        /// <param name="ResultType">结果的对象类型</param>
        /// <param name="action">客户端的回调方法</param>
        /// <param name="function">在执行过程中的回调函数</param>
        /// <returns>消息标识</returns>
        public int RequestService(string message, Type ResultType, Action<string> action, MyFunc<string, string> function)
        {
            return ServiceSubscriber.SendMessage(message, ResultType, action, function, null);
        }

        /// <summary>
        /// 向服务器发送消息，当服务器处理完后，会回调当前提供的回调方法
        /// </summary>
        /// <param name="message">发给服务器的消息</param>
        /// <param name="ResultType">结果的对象类型</param>
        /// <param name="action">客户端的回调方法</param>
        /// <param name="function">在执行过程中的回调函数</param>
        /// <param name="preFunction">在执行过程前的回调函数</param>
        /// <returns>消息标识</returns>
        public int RequestService(string message, Type ResultType, Action<string> action, MyFunc<string, string> function, MyFunc<string, string> preFunction)
        {
            return ServiceSubscriber.SendMessage(message, ResultType, action, function, preFunction);
        }

        public string RequestMessage(string sendMessage)
        {
            string remoteMsg = ServiceSubscriber.RequestMessage(sendMessage);
            return remoteMsg;
        }

        /// <summary>
        /// 取消订阅，关闭连接
        /// </summary>
        public void Close()
        {
            if (ServiceSubscriber != null)
            {
                if (UseConnectionPool)
                {
                    //将连接放回连接池
                    PublishServicePool.Instance.BackPool(ServiceSubscriber);
                }
                else
                {
                    if (!ServiceSubscriber.Closed)
                        ServiceSubscriber.Close(1);
                }
            }
        }
    }
}
