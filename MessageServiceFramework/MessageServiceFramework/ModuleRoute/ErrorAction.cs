using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWMIS.EnterpriseFramework.ModuleRoute
{
    public class ErrorAction:ActionResult
    {
        public ErrorAction( string errMsg,Exception exception)
        {
            this.ErrorMessage = errMsg;
            this.InnerException = exception;
        }
        public string ErrorMessage { get; private set; }

        public Exception InnerException { get; private set; }
    }

    
}
