using System;
using System.Collections.Generic;
using System.Text;

namespace MessageSubscriber
{
    /// <summary>
    /// 订阅者接口
    /// </summary>
    public interface ISubscriber : IClose
    {
        /// <summary>
        /// 发起订阅并注册身份
        /// </summary>
        void Subscribe();
        /// <summary>
        /// 向发布服务器发送消息
        /// </summary>
        /// <param name="msg"></param>
        void SendMessage(string msg);
        /// <summary>
        /// 接收来自发布服务器的消息
        /// </summary>
        /// <param name="msg"></param>
        void OnReceivingMessage(int id, string msg);
        /// <summary>
        /// 处理服务器发布的消息
        /// </summary>
        /// <param name="msg"></param>
        void OnPublishMessage(int id, string msg);
        /// <summary>
        /// 请求的消息类型（FullClassName）
        /// </summary>
        /// <param name="id">消息编号</param>
        /// <returns></returns>
        string RequestMessageType(int id);
        /// <summary>
        /// 服务回调客户端，获取客户端返回的消息
        /// </summary>
        /// <param name="id">消息编号</param>
        /// <param name="para">消息参数</param>
        /// <returns></returns>
        string CallBackMessage(int id, string para);
    }
}
