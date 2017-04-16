using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PWMIS.EnterpriseFramework.Service.Runtime
{
    public class ServiceEventSource
    {
        public object Source { get; private set; }

        public ServiceEventSource(object source)
        {
            this.Source = source;
        }
    }
}
