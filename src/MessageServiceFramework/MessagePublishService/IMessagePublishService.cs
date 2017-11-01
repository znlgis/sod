using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace MessagePublishService
{
/// <summary>
/// 消息发布服务；
/// </summary>
[ServiceContract(CallbackContract=typeof(IMessageListenerCallBack))]
    public interface IMessagePublishService
    {
        /// <summary>
        /// 注册消息监听器(已过时)；
        /// </summary>
        [OperationContract]
        void Regist();

        /// <summary>
        /// 快速注册
        /// </summary>
        /// <param name="indentity">客户端标识，可能附带自定义的调用端应用程序标识</param>
        [OperationContract]
        void QuikRegist(string indentity);

        /// <summary>
        /// 注销消息监听器；
        /// </summary>
        [OperationContract]
        void Unregist();

        /// <summary>
        /// 接受监听器的请求
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        [OperationContract(IsOneWay = true)]
        void OnAccept(int id, string message);

        /// <summary>
        /// 声明一个有返回值的方法，作为客户端的请求-响应 模式
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [OperationContract]
        string OnRequest(string message);
    }
}
