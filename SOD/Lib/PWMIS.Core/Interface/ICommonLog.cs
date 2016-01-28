using System;
namespace PWMIS.Common
{
    /// <summary>
    /// 通用日志接口
    /// </summary>
    public interface ICommandLog
    {
        void WriteLog(string msg, string who);
    }
}
