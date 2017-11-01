using PWMIS.EnterpriseFramework.ModuleRoute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PWMIS.EnterpriseFramework.Service.Host
{
    public class ServiceModuleApplication : ModuleApplication
    {
        public void Application_Start()
        {
            ModuleRegistration.RegisterAllModules();

        }
    }
}
