using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PWMIS.EnterpriseFramework.ModuleRoute
{
    public interface IModuleContext
    {
        string ModuleName { get; }

       // IModuleRuntime ModuleRuntime { get; set; }

        void AddProvider(IModuleProvider provider);

        IModuleProvider GetProvider(string providerName);

        T GetProvider<T>() where T : IModuleProvider;

        //MethodInfo[] GetActionMethods(IModuleProvider provider);

        MethodInfo GetActionMethod(string providerName, string actionName);
    }

    public class MethodHandleInfo
    {
        public System.RuntimeMethodHandle MethodHandle;
        public string MethodName;
    }

    internal class ModuleContext : IModuleContext
    {
        //使用类型句柄，参考：http://blog.csdn.net/jiankunking/article/details/53868359

        protected internal Dictionary<string, System.RuntimeTypeHandle> dictModuleProviders 
            = new Dictionary<string, System.RuntimeTypeHandle>(); //IModuleProvider
        protected internal Dictionary<System.RuntimeTypeHandle, MethodHandleInfo[]> dictProviderMethods
            = new Dictionary<System.RuntimeTypeHandle, MethodHandleInfo[]>(); ////IModuleProvider
        public ModuleContext(string moduleName)
        {
            this.ModuleName = moduleName;
        }
        public string ModuleName { get; private set; }

       // public IModuleRuntime ModuleRuntime { get; set; }

        public void AddProvider(IModuleProvider provider)
        {
            System.RuntimeTypeHandle providerHandle= provider.GetType().TypeHandle;
            dictModuleProviders.Add(provider.PrividerName, providerHandle);
            //反射出所有结果为 IActionResult类型的方法

            List<MethodHandleInfo> actionMethods = new List<MethodHandleInfo>();
            foreach (MethodInfo method in provider.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                if (method.ReturnType.GetInterfaces().Contains(typeof(IActionResult)))
                {
                    MethodHandleInfo info = new MethodHandleInfo();
                    info.MethodHandle = method.MethodHandle;
                    info.MethodName = method.Name;
                    actionMethods.Add(info);
                }
            }

            dictProviderMethods.Add(providerHandle, actionMethods.ToArray());
        }

        public  IModuleProvider GetProvider(string providerName)
        {
            if (dictModuleProviders.ContainsKey(providerName))
            {
                Type providerType = Type.GetTypeFromHandle(dictModuleProviders[providerName]);
                return (IModuleProvider)Activator.CreateInstance(providerType);
            
            }
            else
                throw new Exception("未找到名称为" + providerName + " 的模块提供程序，请检查此类型是否在 ModuleRegistration 注册。");
        }

        public T GetProvider<T>() where T:IModuleProvider
        {
            System.RuntimeTypeHandle handle =typeof(T).TypeHandle;
            System.RuntimeTypeHandle providerHandle =dictModuleProviders.Values.FirstOrDefault(p => p.Equals (handle));
          
            if (providerHandle != null)
            {
                //Type providerType = Type.GetTypeFromHandle(providerHandle);
                //return (IModuleProvider)Activator.CreateInstance(providerType);
                return Activator.CreateInstance<T>();
            }
            else
                throw new Exception("类型未在 ModuleRegistration 注册，类型名称：" + typeof(T).Name);


        }


        public MethodInfo GetActionMethod(string providerName,string actionName)
        {
            if (dictModuleProviders.ContainsKey(providerName))
            {
                System.RuntimeTypeHandle providerHandle = dictModuleProviders[providerName];
                MethodHandleInfo[] actionHandleArr = dictProviderMethods[providerHandle];
                MethodHandleInfo actionInfo = actionHandleArr.FirstOrDefault(p => p.MethodName == actionName);
                if(actionInfo==null)
                    throw new Exception("未找到名称为 " + actionName + " 并且返回值是IActionResult 类型的方法");
                return (MethodInfo)MethodInfo.GetMethodFromHandle(actionInfo.MethodHandle);
            }
            throw new Exception("未找到名称为" + providerName + " 的模块提供程序，请检查此类型是否在 ModuleRegistration 注册。");
        }
    }

    
}
