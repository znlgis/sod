using System;
namespace PWMIS.Common
{
    /// <summary>
    /// 通用日志接口
    /// </summary>
    public interface ICommonLog:IDisposable 
    {
       /// <summary>
       /// 写日志文件
       /// </summary>
       /// <param name="msg">日志消息</param>
       /// <param name="who">写日志人</param>
        void WriteLog(string msg, string who);
        /// <summary>
        /// 写日志文件
        /// </summary>
        /// <param name="log">日志内容</param>
        void WriteLog(string log);
        /// <summary>
        /// 将缓存的日志内容刷新到日志文件
        /// </summary>
        void  Flush();
    }
}
