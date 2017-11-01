using System;
namespace PWMIS.EnterpriseFramework.Service.Runtime
{
    /// <summary>
    /// 服务会话接口
    /// </summary>
    public interface IServiceSession
    {
        /// <summary>
        /// 获取会话标示
        /// </summary>
        string SessionID { get; }
        /// <summary>
        /// 获取指定的会话对象
        /// </summary>
        /// <typeparam name="T">结果类型</typeparam>
        /// <param name="key">存储会话对象的键</param>
        /// <returns>会话对象</returns>
        T Get<T>(string key);
        /// <summary>
        /// 获取指定的会话对象，如果没有，则使用当前提供的对象作为会话对象的值。会话对象将在距上次调用10分钟后过期。
        /// </summary>
        /// <typeparam name="T">结果类型</typeparam>
        /// <param name="key">存储会话对象的键</param>
        /// <param name="Value">要存储的会话对象</param>
        /// <returns>返回原有的会话对象值</returns>
        T Get<T>(string key, T Value);

        /// <summary>
        /// 增加或者修改一个会话对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="Value"></param>
        void Set<T>(string key, T Value);
        /// <summary>
        /// 清除指定键的会话对象
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
        /// <summary>
        /// 清除当前的会话数据
        /// </summary>
        void Clear();

    }

    /// <summary>
    /// 会话模式
    /// </summary>
    public enum SessionModel
    { 
        /// <summary>
        /// 默认，使用每请求会话模式
        /// </summary>
        Default,
        /// <summary>
        /// 每请求一个会话，请求的会话标识信息连接信息、客户端硬件信息和连接时间综合构成
        /// </summary>
        PerRequest,
        /// <summary>
        /// 每连接一个会话，包括客户端的IP和端口号。注意多次请求可能会使用一个连接。
        /// </summary>
        PerConnection,
        /// <summary>
        /// 每用户一个会话
        /// </summary>
        UserName,
        /// <summary>
        /// 以客户端硬件标识一个会话
        /// </summary>
        HardwareIdentity,
        /// <summary>
        /// 以注册连接时候的数据标识一个会话
        /// </summary>
        RegisterData
    }
}
