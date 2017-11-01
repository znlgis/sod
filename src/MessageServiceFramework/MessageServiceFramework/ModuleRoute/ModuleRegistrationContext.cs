using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWMIS.EnterpriseFramework.ModuleRoute
{
    public class ModuleRegistrationContext
    {
        private Dictionary<string, IModuleContext> dictModuleContexts = new Dictionary<string, IModuleContext>();
        public IModuleRuntime ModuleRuntime { get; private set; }
        private IModuleContext CurrentModuleContext { get; set; }

        public ModuleRegistrationContext()
        {
            ModuleRuntime = new ModuleRuntime(this);
        }

        /// <summary>
        /// 将模块上下文注册到模块注册器
        /// </summary>
        /// <param name="moduleContext"></param>
        public void ModuleRegister(IModuleContext moduleContext)
        {
            //moduleContext.ModuleRuntime = ModuleRuntime;
            dictModuleContexts.Add(moduleContext.ModuleName, moduleContext);
            this.CurrentModuleContext = moduleContext;
        }

        /// <summary>
        /// 获取所有的模块上下文对象
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IModuleContext> GetAllModules()
        {
            foreach (string key in dictModuleContexts.Keys)
            {
                yield return dictModuleContexts[key];
            }
        }
      
       

        public IModuleContext GetModuleContext(string moduleName)
        {
            if (dictModuleContexts.ContainsKey(moduleName))
                return dictModuleContexts[moduleName];
            else
                throw new Exception("未找到名称为" + moduleName + " 的模块，请检查此名称所在的程序集是否已被AppModuleContainer 引用。");
        }

        /// <summary>
        /// 将模块提供程序注册到模块上下文
        /// </summary>
        /// <param name="provider"></param>
        public void ProviderRegister(IModuleProvider provider)
        {
            this.CurrentModuleContext.AddProvider(provider);
            //provider.CurrentContext = this.ModuleContext;
        }
    }
}
