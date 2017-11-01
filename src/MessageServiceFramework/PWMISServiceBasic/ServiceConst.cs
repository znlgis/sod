using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PWMIS.EnterpriseFramework.Service.Basic
{
    public class ServiceConst
    {
        public const string Service_Execute_Error = "Service_Execute_Error";
        public static string CreateServiceErrorMessage(string message)
        {
            return string.Format("{0}:{1}",Service_Execute_Error,message);
        }

        public static string GetServiceErrorMessage(string testMessage)
        {
            if (testMessage.StartsWith(Service_Execute_Error))
            {
                return testMessage.Substring(Service_Execute_Error.Length + 1);
            }
            return string.Empty;
        }
    }
}
