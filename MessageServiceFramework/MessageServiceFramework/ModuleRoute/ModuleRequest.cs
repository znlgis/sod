using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWMIS.EnterpriseFramework.ModuleRoute
{
    public class ModuleRequest
    {
        public string ModuleName { get; protected internal set; }

        public string ProviderName { get; protected internal set; }

        public string ActionName { get; protected internal set; }

        public override string ToString()
        {
            return string.Format("Module Request:{0}/{1}/{2}", this.ModuleName, this.ProviderName, this.ActionName);
        }
    }
}
