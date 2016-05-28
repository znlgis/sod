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
    }
}
