using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PWMIS.EnterpriseFramework.Service.Runtime;
using Model;

namespace ServiceSample
{
    public class TestTimeService:IService
    {
        TimeCount tc;
        public TestTimeService()
        {
            tc = new TimeCount();
        }

        public TimeCount ServerTime()
        {
           
            tc.Execute();
            return tc;
        }

        public void CompleteRequest(IServiceContext context)
        {
            
        }

        public bool ProcessRequest(IServiceContext context)
        {
            return true;
        }


        public bool IsUnSubscribe
        {
            get { return false; }
        }
    }
}
