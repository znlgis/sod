using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PWMIS.EnterpriseFramework.ModuleRoute
{
    /// <summary>
    /// 模块注册基类，需要在各个子模块重写该类
    /// </summary>
    public abstract class ModuleRegistration
    {
        internal static ModuleRegistrationContext Context = new ModuleRegistrationContext();

        /// <summary>
        /// 需要重写的模块名称
        /// </summary>
        public abstract string ModuleName { get; }

        /// <summary>
        /// 需要重写的模块注册方法
        /// </summary>
        /// <param name="context">参数 context 将注入当前类的静态Context </param>
        protected internal abstract void RegisterModule(ModuleRegistrationContext context);
       

        /// <summary>
        /// 注册所有的模块和它的提供者程序，该方法必须被首先调用
        /// </summary>
        public static void RegisterAllModules()
        {
            //反射查找当前引用的所有模块提供者程序集 ModuleRegistration
            string path = Assembly.GetCallingAssembly().Location;
            string folder = System.IO.Path.GetDirectoryName(path);
            System.Environment.CurrentDirectory = folder;
            foreach (string file in System.IO.Directory.GetFiles(folder,"*.dll"))
            {
                string shortFile = System.IO.Path.GetFileNameWithoutExtension(file);
                Assembly ass = Assembly.Load(shortFile);
                Type registerType = ass.GetTypes().FirstOrDefault(t => t.BaseType == typeof(ModuleRegistration));
                if (registerType != null)
                {
                    ModuleRegistration register = (ModuleRegistration)Activator.CreateInstance(registerType);
                    IModuleContext moduleContext = new ModuleContext(register.ModuleName);
                    Context.ModuleRegister(moduleContext);
                    register.RegisterModule(Context);
                    Console.WriteLine("已加载[{0}]模块！", register.ModuleName);
                }
            }

          
        }

    }
}
