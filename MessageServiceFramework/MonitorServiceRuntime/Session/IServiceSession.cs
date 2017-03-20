using System;
namespace TranstarAuction.Service.Runtime
{
    /// <summary>
    /// 服务会话接口
    /// </summary>
   public  interface IServiceSession
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
}
