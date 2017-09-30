/*
 * ========================================================================
 * Copyright(c) 2008-2010北京高阳金信信息技术有限公司, All Rights Reserved.
 * ========================================================================
 *  依赖注入容器管理类
 * 
 * 作者：邓太华     时间：2010-06-18至21
 * 版本：V1.0
 * 
 * 修改者：         时间：                
 * 修改说明：
 * ========================================================================
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace PWMIS.EnterpriseFramework.IOC
{
    /// <summary>
    /// 依赖注入容器管理类
    /// </summary>
    public class Unity
    {
        private static Unity m_Unity;
        private static object lock_obj = new object();

        private Dictionary<string, Type> dictTypeCache = new Dictionary<string, Type>();

        /// <summary>
        /// 获取容器管理类的实例
        /// </summary>
        public static Unity Instance
        {
            get
            {
                if (m_Unity == null)
                {
                    lock (lock_obj)
                    {
                        if (m_Unity == null)
                        {
                            m_Unity = new Unity();
                        }
                    }
                }
                return m_Unity;
            }
        }

        /// <summary>
        /// 获取缓存中的类型
        /// </summary>
        /// <param name="typeName">类型的全名称</param>
        /// <param name="assemblyName">当前类型加载的程序集名称</param>
        /// <returns>类型</returns>
        public Type GetCacheType(string typeName, string assemblyName)
        {
            Type t = null;
            if (dictTypeCache.ContainsKey(typeName))
            {
                t = dictTypeCache[typeName];
            }
            else
            {
                Assembly assembly = Assembly.Load(assemblyName);
                t = assembly.GetType(typeName, true);
                dictTypeCache.Add(typeName, t);
            }
            return t;
        }

        /// <summary>
        /// 获取指定的程序集中符合某个基类的所有派生类类型
        /// </summary>
        /// <param name="baseType">基类性，如接口，抽象类</param>
        /// <param name="assemblyName">指定的程序集名称</param>
        /// <returns>所有派生类类型</returns>
        public Type[] GetFactTypes(Type baseType, string assemblyName)
        {
            Assembly assembly = Assembly.Load(assemblyName);
            List<Type> types = new List<Type>();
            foreach (Type t in assembly.GetTypes())
            {
                if (t.BaseType != null && t.BaseType == baseType)
                {
                    types.Add(t);
                }
            }
            types.Sort((x, y) => string.Compare(x.FullName, y.FullName));
            return types.ToArray();
        }

        /// <summary>
        /// 根据容器名称，获取容器中所有的提供程序信息
        /// </summary>
        /// <param name="iocName">容器名称</param>
        /// <returns>提供程序列表</returns>
        public List<IocProvider> GetProviders(string iocName)
        {
            foreach (IOCConfigEntity.IOC ioc in IOCConfig.IOCConfigEntity.GroupSet)
            {
                if (ioc.Name == iocName)
                    return ioc.IocProviderList;
            }
            return null;
        }

        /// <summary>
        /// 根据提供程序获取它的实例
        /// </summary>
        /// <param name="provider">提供程序</param>
        /// <returns>对象实例</returns>
        public object GetProviderInstance(IocProvider provider)
        {
            Type t = GetProviderType(provider);
            //下面的方式稍快
            object o = Activator.CreateInstance(t);
            return o;

            //下面的方式稍慢
            //object obj = t.InvokeMember(null, BindingFlags.CreateInstance, null, null, null);
            //return (T)obj;
        }

        /// <summary>
        /// 获取提供程序的运行时类型（将通过缓存提升效率）
        /// </summary>
        /// <param name="provider">提供程序</param>
        /// <returns>对象类型</returns>
        public Type GetProviderType(IocProvider provider)
        {
            Type t = null;
            if (dictTypeCache.ContainsKey(provider.FullClassName))
            {
                t = dictTypeCache[provider.FullClassName];
            }
            else
            {
                //缓存可以大大提高效率
                lock (lock_obj)
                {
                    if (dictTypeCache.ContainsKey(provider.FullClassName))
                    {
                        t = dictTypeCache[provider.FullClassName];
                    }
                    else
                    {
                        Assembly assembly = Assembly.Load(provider.Assembly);
                        t = assembly.GetType(provider.FullClassName, true);
                        dictTypeCache.Add(provider.FullClassName, t);
                    }
                }
            }
            return t;
        }

        /// <summary>
        /// 创建指定类型（接口，抽象类）的实例
        /// </summary>
        /// <typeparam name="T">指定类型（接口，抽象类）</typeparam>
        /// <param name="providerAssembly">提供程序的程序集名称</param>
        /// <param name="providerType">类型的完整名称</param>
        /// <returns></returns>
        public T CreateInstance<T>(string providerAssembly, string providerType)
        {
            Assembly assembly = Assembly.Load(providerAssembly);
            object provider = assembly.CreateInstance(providerType);

            if (provider is T)
            {
                return (T)provider;
            }
            else
            {
                throw new InvalidOperationException("当前指定的的提供程序不是当前类型具体实现类");
            }
        }
        /// <summary>
        /// 提供程序比较委托
        /// </summary>
        /// <param name="provider">提供程序</param>
        /// <returns>是否符合比较条件</returns>
        private delegate bool ProviderCompare(IocProvider provider);

        /// <summary>
        /// 从容器中寻找指定的提供程序配置信息
        /// </summary>
        /// <param name="compare"></param>
        /// <returns></returns>
        private IocProvider findProvider(ProviderCompare compare)
        {
            foreach (IOCConfigEntity.IOC ioc in IOCConfig.IOCConfigEntity.GroupSet)
            {
                foreach (IocProvider provider in ioc.IocProviderList)
                {
                    if (compare(provider))
                        return provider;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据条件寻找提供程序列表
        /// </summary>
        /// <param name="compare"></param>
        /// <returns></returns>
        private IEnumerable<IocProvider> findProviderList(ProviderCompare compare)
        {
            foreach (IOCConfigEntity.IOC ioc in IOCConfig.IOCConfigEntity.GroupSet)
            {
                foreach (IocProvider provider in ioc.IocProviderList)
                {
                    if (compare(provider))
                        yield return provider;
                }
            }

        }


        /// <summary>
        /// 从所有容器中寻找符合当前指定类型（接口）的第一个提供程序
        /// </summary>
        /// <typeparam name="T">指定类型（接口）</typeparam>
        /// <returns>提供程序</returns>
        public T GetInstance<T>()
        {
            //foreach (IOCConfigEntity.IOC ioc in IOCConfig.IOCConfigEntity.GroupSet)
            //{
            //    foreach (IocProvider provider in ioc.IocProviderList )
            //    {
            //        return (T)GetProviderInstance(provider);
            //    }
            //}
            // ProviderCompare cmp = delegate() {return  true; };

            //采用委托简化代码
            string iName = this.GetInterfaceName(typeof(T).FullName);
            IocProvider provider = findProvider(p => p.InterfaceName == iName);
            if (provider != null)
                return (T)GetProviderInstance(provider);
            else
                throw new InvalidOperationException("从注册的所有容器中没有找到符合当前类型的提供程序。");
        }


        /// <summary>
        /// 从指定的容器名称中寻找指定的提供程序（容器的项名，XPath="/GroupSet/IOC[@Name='iocName']/Add[@Key='iocItemKey']"）
        /// </summary>
        /// <typeparam name="T">指定类型（接口）</typeparam>
        /// <param name="iocName">容器名称</param>
        /// <param name="iocItemKey">容器中的项名称</param>
        /// <returns>提供程序</returns>
        public T GetInstance<T>(string iocName, string iocItemKey) //where T:class 
        {
            foreach (IOCConfigEntity.IOC ioc in IOCConfig.IOCConfigEntity.GroupSet)
            {
                if (ioc.Name == iocName)
                {
                    foreach (IocProvider provider in ioc.IocProviderList)
                    {
                        if (provider.Key == iocItemKey)
                        {
                            return (T)GetProviderInstance(provider);
                        }
                    }
                }

            }

            throw new InvalidOperationException("从注册的所有容器中没有找到符合当前类型的提供程序。");
        }

        /// <summary>
        /// 从所有容器名称中寻找指定的提供程序（容器的项名，XPath="/GroupSet/IOC/Add[@Key='iocItemKey']"），如果有多个，返回第一个。
        /// </summary>
        /// <typeparam name="T">指定类型（接口）</typeparam>
        /// <param name="iocName">容器名称</param>
        /// <param name="iocItemKey">容器中的项名称</param>
        /// <returns>提供程序</returns>
        public T GetInstance<T>(string iocItemKey)
        {
            //foreach (IOCConfigEntity.IOC ioc in IOCConfig.IOCConfigEntity.GroupSet)
            //{
            //    foreach (IocProvider provider in ioc.IocProviderList)
            //    {
            //        if (provider.Key == iocItemKey)
            //        {
            //            return (T)GetProviderInstance(provider);
            //        }
            //    }
            //}

            //采用委托简化代码
            IocProvider provider = findProvider(p => p.Key == iocItemKey);
            if (provider != null)
                return (T)GetProviderInstance(provider);
            else
                throw new InvalidOperationException("从注册的所有容器中没有找到符合当前类型的提供程序。");
        }



        /// <summary>
        /// 从所有容器中寻找符合当前指定类型（接口）的 提供程序列表
        /// </summary>
        /// <typeparam name="T">指定类型（接口）</typeparam>
        /// <returns>提供程序列表</returns>
        public List<T> GetInstanceList<T>()
        {
            List<T> list = new List<T>();
            //foreach (IOCConfigEntity.IOC ioc in IOCConfig.IOCConfigEntity.GroupSet)
            //{
            //    foreach (IocProvider provider in ioc.IocProviderList)
            //    {
            //        list.Add ( (T)GetProviderInstance(provider));
            //    }
            //}

            //采用迭代器结合委托简化代码，此代码可能有误
            foreach (IocProvider provider in findProviderList(p => p.Key == p.Key))
            {
                list.Add((T)GetProviderInstance(provider));
            }
            return list;
        }

        /// <summary>
        /// 从所有容器中寻找符合当前指定类型（接口）的 提供程序列表
        /// </summary>
        /// <param name="interfaceName">容器中定义的接口别名</param>
        /// <returns>提供程序列表</returns>
        public List<object> GetInstanceList(string interfaceName)
        {
            List<object> list = new List<object>();
            //foreach (IOCConfigEntity.IOC ioc in IOCConfig.IOCConfigEntity.GroupSet)
            //{
            //    foreach (IocProvider provider in ioc.IocProviderList)
            //    {
            //        if (provider.InterfaceName == interfaceName)
            //        {
            //            object obj= GetProviderInstance(provider);
            //            list.Add(obj);
            //        }

            //    }
            //}

            //采用迭代器结合委托简化代码
            foreach (IocProvider provider in findProviderList(p => p.InterfaceName == interfaceName))
            {
                object obj = GetProviderInstance(provider);
                list.Add(obj);
            }
            return list;
        }

        /// <summary>
        ///  从所有容器中寻找符合当前指定类型（接口）的第一个提供程序，提供程序信息不能定义在AOP组中
        /// </summary>
        /// <param name="interfaceName"></param>
        /// <returns></returns>
        public object GetInterfaceInstance(string interfaceName)
        {
            IocProvider provider = findProvider(p => p.InterfaceName == interfaceName && p.CurrentIOC.Name != "AOP");
            if (provider != null)
                return GetProviderInstance(provider);
            else
                throw new InvalidOperationException("在IOC配置文件中没有找到 InterfaceName='" + interfaceName + "' 的具体对象实例配置信息。");
        }

        /// <summary>
        /// 根据在容器中注册的项名称，执行指定的方法。
        /// </summary>
        /// <param name="iocItemKey">注册的项名称</param>
        /// <param name="methodName">要执行的方法名称</param>
        /// <param name="parasType">方法的参数类型数组，例如 new Type[] { typeof(string),typeof(int) }</param>
        /// <param name="parasValue">跟方法参数对应的参数值数组，例如 new object[]{"aaa",999}</param>
        /// <returns>执行方法返回的值</returns>
        public object InvokeMethod(string iocItemKey, string methodName, Type[] parasType, object[] parasValue)
        {
            //foreach (IOCConfigEntity.IOC ioc in IOCConfig.IOCConfigEntity.GroupSet)
            //{
            //    foreach (IocProvider provider in ioc.IocProviderList)
            //    {
            //        if (provider.Key == iocItemKey)
            //        {
            //            return InvokeMethod(provider.Assembly, provider.FullClassName,methodName, parasType, parasValue);
            //        }
            //    }
            //}

            //采用委托简化代码
            IocProvider provider = findProvider(p => p.Key == iocItemKey);
            if (provider != null)
                return InvokeMethod(provider.Assembly, provider.FullClassName, methodName, parasType, parasValue);
            else
                throw new InvalidOperationException("从注册的所有容器中没有找到指定项的提供程序。");
        }

        /// <summary>
        /// 根据指定的程序集信息，执行指定的方法
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="fullClassName">完整的类名称（带名称空间）</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="parasType">执行方法需要的参数类型定义</param>
        /// <param name="parasValue">执行方法需要的参数值</param>
        /// <returns></returns>
        public object InvokeMethod(string assemblyName, string fullClassName, string methodName, Type[] parasType, object[] parasValue)
        {
            Assembly assembly = Assembly.Load(assemblyName);
            Type T = assembly.GetType(fullClassName);

            MethodInfo mi = T.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance, null, parasType, null);
            object o = Activator.CreateInstance(T);
            object result = mi.Invoke(o, parasValue);
            return result;
        }

        /// <summary>
        /// 从IOC配置文件中根据具体的接口类型名称寻找接口名称
        /// </summary>
        /// <param name="interfaceTypeName">接口类型名称</param>
        /// <returns>接口名称</returns>
        public string GetInterfaceName(string interfaceTypeName)
        {
            string interfaceName = "";
            foreach (IOCConfigEntity.InterfaceRecord record in IOCConfig.IOCConfigEntity.SystemInterface)
            {
                if (record.InterfaceFullName == interfaceTypeName)
                {
                    interfaceName = record.Name;
                    break;
                }
            }
            if (interfaceName == "")
                throw new Exception("在IOC配置文件中未找到接口，Interface=" + interfaceTypeName);
            return interfaceName;
        }
    }
}
