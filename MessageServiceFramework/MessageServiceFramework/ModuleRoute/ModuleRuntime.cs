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
                ActionResult result = (ActionResult)executor.Execute(application.ModuleProvider, paramters);
                result.Succeed = true;
                return result;
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

        /// <summary>
        /// 执行模块方法调用，并将结果类型转换成指定的类型。要求目标方法的参数数量不能多于一个。
        /// </summary>
        /// <typeparam name="TResult">结果类型转换指定的类型</typeparam>
        /// <param name="moduleName">模块名称</param>
        /// <param name="providerName">模块提供程序名称</param>
        /// <param name="actionName">模块提供程序的方法</param>
        /// <param name="parameter">模块的方法的参数类，如果与目标方法的参数类型不一致，框架会尝试将此参数对象的值拷贝到目标参数对象上</param>
        /// <param name="objectMapper">两个对象之间进行属性值映射拷贝的方法，第一个参数为源对象，第二个参数为目标对象</param>
        /// <returns></returns>
        public ActionResult<TResult> ExecuteAction<TResult>(string moduleName, string providerName, string actionName, object parameter,Action<object, object> objectMapper) 
            where TResult:new()
        {
            //注意：当前方法被多线程调用，不可以使用其它全局变量
            ModuleApplication application = (ModuleApplication)Activator.CreateInstance(CurrentAppType);

            MethodInfo action = GetActionMethod(moduleName, providerName, actionName, application);
            try
            {
                application.OnBeginRequest();
                //处理参数
                ParameterInfo[] paras = action.GetParameters();
                object[] executorParas;
                if (paras.Length > 0)
                {
                    ParameterInfo pi = paras[0];
                    object objPara = Activator.CreateInstance(pi.ParameterType);
                    objectMapper(parameter, objPara);
                    executorParas = new object[] { objPara };
                }
                else
                {
                    executorParas = new object[] { };
                }

                DynamicMethodExecutor executor = new DynamicMethodExecutor(action);
                object result = executor.Execute(application.ModuleProvider, executorParas);

                ActionResult resultTemp = result as ActionResult;
                if (resultTemp != null)
                {
                    if (result is EmptyAction)
                    {
                        return new ActionResult<TResult>()
                        {
                            Succeed = false,
                            Error = new ErrorAction("调用模块方法错误，目标模块方法的返回值是 EmptyAction，不符合当前方法返回值类型", null)
                        };
                    }
                    else if (result is ErrorAction)
                    {
                        return new ActionResult<TResult>()
                        {
                            Succeed = false,
                            Error = (ErrorAction)result
                        };
                    }
                    else
                    {
                        if (typeof(TResult).IsClass)
                        {
                            TResult targetResult = new TResult();
                            objectMapper(resultTemp.ObjectResult, targetResult);
                            return new ActionResult<TResult>()
                            {
                                Succeed = true,
                                Result = targetResult
                            };
                        }
                        else
                        {
                            return new ActionResult<TResult>()
                            {
                                Succeed = true,
                                Result = (TResult)resultTemp.ObjectResult
                            };
                        }
                    }
                }
                else
                {
                    //可能为泛型结果类型
                    object objResult = ((IActionResult)result).ObjectResult;
                    if (typeof(TResult).IsClass)
                    {
                        TResult targetResult = new TResult();
                        objectMapper(objResult, targetResult);
                        return new ActionResult<TResult>()
                        {
                            Succeed = true,
                            Result = targetResult
                        };
                    }
                    else
                    {
                        return new ActionResult<TResult>()
                        {
                            Succeed = true,
                            Result = (TResult)objResult
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ActionResult<TResult>()
                {
                    Succeed = false,
                    Error = new ErrorAction("调用模块方法错误，详细内容请看内存错误信息", ex)
                };
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
                ret.Succeed = true;
                return ret;
            }
            catch (Exception ex)
            {
                return new ActionResult<TResult>() {
                    Succeed=false ,
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
