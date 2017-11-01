using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace MessagePublishService
{
/// <summary>
/// 消息监听器；
/// 作为消息发布服务的回调契约；
/// </summary>
    [ServiceContract]
    public interface IMessageListenerCallBack
    {
        /// <summary>
        /// 在监听器上推送消息（“出版-订阅”模式）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        [OperationContract(IsOneWay = true)]
        void OnPublish(int id, string message);

        /// <summary>
        /// 接收来自服务器的消息（“请求-响应”模式）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        [OperationContract(IsOneWay = true)]
        void OnReceive(int id, string message);

        /// <summary>
        /// 获取客户端标识
        /// </summary>
        /// <returns></returns>
        [OperationContract()]
        string GetIdentity();

        /// <summary>
        /// 获取客户端请求的消息类型，例如System.String,String.Int32等
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract()]
        string RequestMessageType(int id);

        /// <summary>
        /// 通知客户端，关闭监听器
        /// </summary>
        /// <param name="falg"></param>
        [OperationContract(IsOneWay = true)]
        void OnClose(int falg);

        /// <summary>
        /// 服务回调客户端，获取客户端返回的消息
        /// </summary>
        /// <param name="id">消息处理器编号</param>
        /// <param name="para">消息参数</param>
        /// <returns></returns>
        [OperationContract()]
        string CallBackMessage(int id, string para);

        /// <summary>
        /// 服务回调客户端，预先获取客户端返回的消息
        /// </summary>
        /// <param name="id">消息处理器编号</param>
        /// <param name="para">消息参数</param>
        /// <returns></returns>
        [OperationContract()]
        string PreCallBackMessage(int id, string para);
    }
}
