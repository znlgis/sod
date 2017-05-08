using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWMIS.EnterpriseFramework.ModuleRoute
{
    /// <summary>
    /// 方法调用结果错误的对象
    /// </summary>
    public class ErrorAction:ActionResult
    {
        public ErrorAction( string errMsg,Exception exception)
        {
            this.ErrorMessage = errMsg;
            this.InnerException = exception;
            this.Succeed = false;
        }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; private set; }
        /// <summary>
        /// 详细的内部错误对象
        /// </summary>
        public Exception InnerException { get; private set; }
    }

    
}
