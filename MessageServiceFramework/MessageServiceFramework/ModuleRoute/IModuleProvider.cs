using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWMIS.EnterpriseFramework.ModuleRoute
{
    public interface IModuleProvider
    {
        string PrividerName { get; }

        //IModuleContext CurrentContext { get; set; }
        //ModuleApplication Application { get; }
        void Init(ModuleApplication application);
        
    }

    /// <summary>
    /// 模块提供程序抽象类
    /// </summary>
    public abstract class ModuleProviderBase : IModuleProvider
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        public abstract string PrividerName { get; }
        /// <summary>
        /// 获取模块应用程序
        /// </summary>
        public ModuleApplication Application { get; protected set; }
        /// <summary>
        /// 初始化模块应用程序
        /// </summary>
        /// <param name="application"></param>
        public virtual void Init(ModuleApplication application) {
            this.Application = application;
        }
    }
}
