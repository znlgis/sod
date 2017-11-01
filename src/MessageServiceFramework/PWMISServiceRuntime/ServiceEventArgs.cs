using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PWMIS.EnterpriseFramework.Service.Runtime
{
    public class ServiceEventArgs:EventArgs
    {
        public object EventData { get; private set; }
        public ServiceEventArgs(object eventData)
        {
            this.EventData = eventData;
        }
    }
}
