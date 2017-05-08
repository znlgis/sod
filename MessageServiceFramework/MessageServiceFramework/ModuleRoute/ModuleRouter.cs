using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWMIS.EnterpriseFramework.ModuleRoute
{
    public class ModuleRouter
    {
        private Dictionary<string, IModuleProvider> moduleProviders = new Dictionary<string, IModuleProvider>();
        public void Register(IModuleProvider provider)
        {
            moduleProviders.Add(provider.PrividerName, provider);
        }
    }
}
