/*
 * 实现消息发布服务
 * 注意：如果需要使用 MONO框架，请指定解决方案的条件编译常量 MONO
 */
using System;
using System.Collections.Generic;
using System.Text;
using MessagePublishService;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;

namespace MessagePublisher
{
/// <summary>
/// 实现消息发布服务；
/// </summary>
[ServiceBehavior(
    InstanceContextMode = InstanceContextMode.Single,
    ConcurrencyMode = ConcurrencyMode.Multiple,
    IncludeExceptionDetailInFaults = true,
    UseSynchronizationContext = false
    )]
    public class MessagePublishServiceImpl : IMessagePublishService
    {
        #region IMessagePublishService 成员
        /// <summary>
        /// 注册；
        /// </summary>
        public void Regist()
        {
            RemoteEndpointMessageProperty remoteEndpointProp = GetRemoteEndpointProp();
            IMessageListenerCallBack callback = OperationContext.Current.GetCallbackChannel<IMessageListenerCallBack>();
            MessageCenter.Instance.AddListener(new MessageListener(remoteEndpointProp.Address, remoteEndpointProp.Port, callback));
            //下面的事件在mono 无效
            OperationContext.Current.Channel.Closing += new EventHandler(MessageCenter.Instance.Channel_Closing);
            //Channel.Closing 参考 http://bbs.csdn.net/topics/390272596
            //Console.WriteLine("Regist ok.");
        }

        /// <summary>
        /// 注销；
        /// </summary>
        public void Unregist()
        {
            RemoteEndpointMessageProperty remoteEndpointProp = GetRemoteEndpointProp();
            IMessageListenerCallBack callback = OperationContext.Current.GetCallbackChannel<IMessageListenerCallBack>();
            MessageCenter.Instance.RemoveListener(new MessageListener(remoteEndpointProp.Address, remoteEndpointProp.Port, callback));
        }

        /// <summary>
        /// 接受消息
        /// </summary>
        /// <param name="id">消息编号</param>
        /// <param name="message">消息内容</param>
        public void OnAccept(int id, string message)
        {
            //Console.WriteLine("OnAccept:id:{0},message:{1}",id,message);
            RemoteEndpointMessageProperty remoteEndpointProp = GetRemoteEndpointProp();
            IMessageListenerCallBack callback = OperationContext.Current.GetCallbackChannel<IMessageListenerCallBack>();
            MessageCenter.Instance.AcceptMessage(new MessageListener(remoteEndpointProp.Address, remoteEndpointProp.Port, callback, id, message));
        }


        /// <summary>
        /// 快速注册
        /// </summary>
        /// <param name="indentity">客户端标识</param>
        public void QuikRegist(string indentity)
        {
            RemoteEndpointMessageProperty remoteEndpointProp = GetRemoteEndpointProp();
            IMessageListenerCallBack callback = OperationContext.Current.GetCallbackChannel<IMessageListenerCallBack>();
            MessageCenter.Instance.AddListener(new MessageListener(remoteEndpointProp.Address, remoteEndpointProp.Port, callback), indentity);
            //Console.WriteLine("QuikRegist ok..");
            OperationContext.Current.Channel.Closing += new EventHandler(MessageCenter.Instance.Channel_Closing);
        }

        public string OnRequest(string message)
        {
            //Console.WriteLine("OnRequest begin..");
            RemoteEndpointMessageProperty remoteEndpointProp = GetRemoteEndpointProp();
            IMessageListenerCallBack callback = OperationContext.Current.GetCallbackChannel<IMessageListenerCallBack>();
            return MessageCenter.Instance.RequestMessage(new MessageListener(remoteEndpointProp.Address, remoteEndpointProp.Port, callback, 999, message));
        }
        #endregion

        private RemoteEndpointMessageProperty GetRemoteEndpointProp()
        {
            RemoteEndpointMessageProperty remoteEndpointProp = OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
#if(MONO)
            if (remoteEndpointProp == null)
            {
                //mono RemoteEndpointMessageProperty 无法获取到，用下面的方式获取：
                var uri = OperationContext.Current.Channel.RemoteAddress.Uri;
                string clientHostName = uri.Host;//获取当前机器名称-----多个客户端不在同一台机器上，就使用此信息。
                int clientHostPort = uri.Port;
                remoteEndpointProp = new RemoteEndpointMessageProperty(clientHostName, clientHostPort);
            }
#endif
            return remoteEndpointProp;
        }


    }
}
