/**
 * AOP 面向方面框架
 * 注：该项目不兼容.NET 4.0，如果需要在.NET 4.0下面使用，请使用 WCFMail解决方案
 */ 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using PWMIS.EnterpriseFramework.IOC;


namespace PWMIS.EnterpriseFramework.AOP
{
    public delegate object MethodCall(object target, MethodBase method,object[] parameters, DecoratorAttribute[] attributes);

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property |  AttributeTargets.Interface, Inherited = true)]
    public abstract class DecoratorAttribute : Attribute
    {
        public abstract void Process(object target, MethodBase method, object[] parameters);
    }

    /// 代表执行前和执行后外部“横切”机制的抽象对象 
    public abstract class BeforeDecoratorAttribute : DecoratorAttribute { }
    public abstract class AfterDecoratorAttribute : DecoratorAttribute { } 

    /// <summary>
    /// AOP工厂类
    /// </summary>
    public class AOPFactory
    {
        /// <summary>
        /// 通过IOC的配置信息，创建AOP对象的实例
        /// </summary>
        /// <typeparam name="T">返回值类型，通常为AOP对象的接口</typeparam>
        /// <returns>AOP对象的实例</returns>
        public  T Create<T>() where T:class 
        {
            
           
            Type interfaceType = typeof(T);
            T obj = null;
            //使用预先生成的程序集没有太大价值，下面代码被注释
            //-------------------------------------------------------------------------------------------
            //先看类型T对应的程序集有没有生成，如果生成，就采用生成好的
            //try
            //{
            //    obj = Unity.Instance.GetInstance<T>("AOP", interfaceType.FullName );
            //}
            //catch(Exception ex)
            //{
            //    obj = (T)DecoratorInjector.Create(CreateInstance(interfaceType.FullName ), interfaceType);
            //}
            obj = (T)DecoratorInjector.Create(CreateInstance(interfaceType.FullName), interfaceType);
            return obj;
        }

        /// <summary>
        /// 包装传入对象的实例，使得它具有“方面”的功能
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public T Create<T>(object target) where T : class
        {
            Type interfaceType = typeof(T);
            T obj = (T)DecoratorInjector.Create(target, interfaceType);
            return obj;
        }

        /// <summary>
        /// 创建类型未包装前的原始实例
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        private object CreateInstance(string typeName)
        {
            string interfaceName = Unity.Instance.GetInterfaceName(typeName);
            return Unity.Instance.GetInterfaceInstance(interfaceName);
        }

       
    }
}
