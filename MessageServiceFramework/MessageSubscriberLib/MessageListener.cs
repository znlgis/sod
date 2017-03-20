using System;
using System.Collections.Generic;
using System.Text;
using MessagePublishService;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using PWMIS.EnterpriseFramework.Common.Hardware;


namespace MessageSubscriber
{
/// <summary>
/// 消息监听器；
/// <remarks>
/// CallbackBehavior 特性值为 False 可解决WinForm回调客户端线程死锁的问题,详情请参考
/// http://www.cnblogs.com/artech/archive/2007/03/29/692032.html
/// </remarks>
/// </summary>
    [CallbackBehavior(UseSynchronizationContext = false, ConcurrencyMode = ConcurrencyMode.Multiple,IncludeExceptionDetailInFaults=true)]
    public class MessageListener : IMessageListenerCallBack
    {
        private ISubscriber _source;

        public MessageListener() { }

        public MessageListener(ISubscriber source)
        {
            this._source = source;
        }

        //public Type ResultType { get; set; }

        #region IMessageListener 成员

        public void OnPublish(int id, string message)
        {
            this._source.OnPublishMessage(id, message);
        }

        public void OnReceive(int id, string message)
        {
            this._source.OnReceivingMessage(id, message);
        }

        public string GetIdentity()
        {
            //待改进
            return "YXP;20111230;" + HardDiskSN.SerialNumber;
        }

        public string RequestMessageType(int id)
        {
            return this._source.RequestMessageType(id);
        }

        /// <summary>
        /// 服务回调客户端，获取客户端返回的消息
        /// </summary>
        /// <param name="id">消息处理器编号</param>
        /// <param name="para">消息参数</param>
        /// <returns></returns>
        public string CallBackMessage(int id, string para)
        {
            return this._source.CallBackMessage(id, para);
        }

        /// <summary>
        /// 服务回调客户端，预先获取客户端返回的消息
        /// </summary>
        /// <param name="id">消息处理器编号</param>
        /// <param name="para">消息参数</param>
        /// <returns></returns>
        public string PreCallBackMessage(int id, string para)
        {
            return this._source.PreCallBackMessage(id, para);
        }

        /// <summary>
        /// 服务器通知客户端关闭连接
        /// </summary>
        public void OnClose(int flag)
        {
            this._source.Close(flag);
        }
        #endregion
    }
}
