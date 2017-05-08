using PWMIS.EnterpriseFramework.Service.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PWMIS.EnterpriseFramework.ModuleRoute
{
    public class ModuleRuntime : IModuleRuntime
    {
        ModuleRegistrationContext RegContext;
        
        static Type CurrentAppType;

        internal ModuleRuntime(ModuleRegistrationContext context)
        {
            this.RegContext = context;
        }

        //第一个请求到来，会执行此方法 XXModuleApplication.Application_Start

        public ActionResult ExecuteAction(string moduleName, string providerName, string actionName, object[] paramters)
        {
            //注意：当前方法被多线程调用，不可以使用其它全局变量
            ModuleApplication application = (ModuleApplication)Activator.CreateInstance(CurrentAppType);

            MethodInfo action = GetActionMethod(moduleName, providerName, actionName,application);
            try
            {
                application.OnBeginRequest();
                DynamicMethodExecutor executor = new DynamicMethodExecutor(action);
                object result = executor.Execute(application.ModuleProvider, paramters);
                return new ActionResult() { ObjectResult = result };
            }
            catch (Exception ex)
            {
                return new ErrorAction("调用模块方法错误", ex);
            }
            finally
            {
                application.OnEndRequest();
                application = null;
            }

            
        }

        public ActionResult<TResult> ExecuteAction<T, TResult>(string moduleName, string providerName, string actionName, T parameter)
        {
            //注意：当前方法被多线程调用，不可以使用其它全局变量
            ModuleApplication application = (ModuleApplication)Activator.CreateInstance(CurrentAppType);

            MethodInfo action = GetActionMethod(moduleName, providerName, actionName, application);
            try
            {
                application.OnBeginRequest();

                Func<T, ActionResult<TResult>> fun
                   = (Func<T, ActionResult<TResult>>)System.Delegate.CreateDelegate(typeof(Func<T, ActionResult<TResult>>),
                   application.ModuleProvider, action);

                ActionResult<TResult> ret = fun(parameter);
                ret.Error = null;
                return ret;
            }
            catch (Exception ex)
            {
                return new ActionResult<TResult>() {
                    Error = new ErrorAction("调用模块方法错误", ex)
                };
            }
            finally
            {
                application.OnEndRequest();
                application = null;
            }
        }

        private MethodInfo GetActionMethod(string moduleName, string providerName, string actionName,ModuleApplication currentApp)
        {
            IModuleContext module = this.RegContext.GetModuleContext(moduleName);
            currentApp.ModuleContext = module;
            currentApp.Request = new ModuleRequest()
            { 
                ModuleName= moduleName ,
                ProviderName= providerName,
                ActionName= actionName
            };
            IModuleProvider provider =module.GetProvider(providerName);
            provider.Init(currentApp);
            currentApp.ModuleProvider = provider;

            return module.GetActionMethod(providerName, actionName);
        }

        /// <summary>
        /// 在所有的模块中查找并实例化指定的模块提供程序对象
        /// </summary>
        /// <typeparam name="T">模块提供程序对象类型/typeparam>
        /// <returns>模块提供程序对象</returns>
        public T ResolveProvider<T>() where T:IModuleProvider
        {
            System.RuntimeTypeHandle handle = typeof(T).TypeHandle;
            foreach (IModuleContext module in this.RegContext.GetAllModules())
            {
                try
                {
                    return module.GetProvider<T>();
                }
                catch
                { 
                
                }
            }
            throw new Exception("类型未在相关的 ModuleRegistration实现类中注册，类型名称：" +  typeof(T).Name);
        }

        /// <summary>
        /// 获取当前的模块运行时对象
        /// </summary>
        public static IModuleRuntime Current {
            get
            {
                //在入口应用程序，查找 ModuleApplication 的实现类类型
                if (CurrentAppType == null)
                {
                    Assembly entry = Assembly.GetEntryAssembly();
                    CurrentAppType = entry.GetTypes().FirstOrDefault(p => p.BaseType == typeof(ModuleApplication));
                    if (CurrentAppType != null)
                    {
                        //执行 Application_Start 方法
                        MethodInfo method = CurrentAppType.GetMethod("Application_Start", BindingFlags.Instance | BindingFlags.Public);
                        if (method == null)
                            throw new Exception("类型" + CurrentAppType.Name + " 需要实现一个名字为 Application_Start 无参数方法。");
                        object appInstance = Activator.CreateInstance(CurrentAppType);
                        Action start = (Action)System.Delegate.CreateDelegate(typeof(Action), appInstance, method);
                        start();
                    }
                    else
                        throw new Exception("当前应用程序没有实现 ModuleApplication 类，必须要求实现一个。");
                }

                return ModuleRegistration.Context.ModuleRuntime;
            }
        } 
    }
}
