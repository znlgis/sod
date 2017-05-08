using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWMIS.EnterpriseFramework.ModuleRoute
{
    /// <summary>
    /// 模块应用程序
    /// <remarks>子类必须实现Application_Start 方法</remarks>
    /// </summary>
    public class ModuleApplication
    {
        //要求在子类实现
        //public void Application_Start()
        //{
        //    ModuleRegistration.RegisterAllModules();
            
        //}



        public event EventHandler BeginRequest;
        public event EventHandler EndRequest;

        protected internal void OnBeginRequest()
        {
            if (BeginRequest != null)
                BeginRequest(this, new EventArgs());
        }

        protected internal void OnEndRequest()
        {
            if (EndRequest != null)
                EndRequest(this, new EventArgs());
        }

        public IModuleContext ModuleContext { get; internal set; }
        public IModuleProvider ModuleProvider { get; internal set; }

        public ModuleRequest Request { get; internal set; }

       
    }
}
