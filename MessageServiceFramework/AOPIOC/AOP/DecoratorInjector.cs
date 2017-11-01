/* 
 * 原文地址：http://doc.chinaunix.net/sqlserver/200712/157836_7.shtml
 * 
 * 注意：下面的Emit代码在.NET 3.5、4.0 下面有所区别，如果要在.NET 4.0下面使用，请使用WCFMail解决方案下面的Aop项目  
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;


namespace PWMIS.EnterpriseFramework.AOP
{
    public class DecoratorInjector
    {
        public const string AssemblyName = "TEMP_DYNAMIC_ASSEMBLY";
        public const string ClassName = "TEMP_CLASS_NAME";
        private static TypeBuilder typeBuilder;
        private static FieldBuilder target, iface;
        private static Dictionary<string, Type> CacheProxyType = new Dictionary<string, Type>();

        public static object InjectHandlerMethod(object target, MethodBase method, object[] parameters,DecoratorAttribute[] attributes)
        {
            object returnValue = null;
            foreach (DecoratorAttribute attribute in attributes)
                if (attribute is BeforeDecoratorAttribute)
                    attribute.Process(target, method, parameters);
            returnValue = target.GetType().GetMethod(method.Name).Invoke(target, parameters);

            foreach (DecoratorAttribute attribute in attributes)
                if (attribute is AfterDecoratorAttribute)
                    attribute.Process(target, method, parameters);
            return returnValue;
        }

        public static object Create(object target, Type interfaceType)
        {
            Type targetType=target.GetType();
            Type proxyType = null;
            if(CacheProxyType.ContainsKey (targetType.FullName))
            {
                proxyType = CacheProxyType[targetType.FullName];
            }
            else
            {
                proxyType = EmiProxyType(targetType, interfaceType);
                CacheProxyType[targetType.FullName] = proxyType;
            }
            return Activator.CreateInstance(proxyType, new object[] { target, interfaceType });
        }

        ///// <summary>
        ///// 动态代理，可以保存程序集到磁盘并调用，发现没有把targetType 直接包装，保存为独立程序集没有太大用处
        ///// </summary>
        ///// <param name="targetType"></param>
        ///// <param name="interfaceType"></param>
        ///// <returns></returns>
        //private static Type EmiProxyType(Type targetType, Type interfaceType)
        //{
        //    string typeName = AssemblyName + "__Proxy" + interfaceType.Name + targetType.Name;
        //    //string currClassName = ClassName + "__Proxy" + targetType.Name;
        //    string assemFileName = typeName + ".dll";
        //    AppDomain currentDomain = System.Threading.Thread.GetDomain();
        //    AssemblyName assemblyName = new AssemblyName();
        //    assemblyName.Name = typeName;

        //    //Only save the custom-type dll while debugging 
        //    AssemblyBuilder assemblyBuilder =
        //    currentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave );
        //    ModuleBuilder modBuilder = assemblyBuilder.DefineDynamicModule(assemFileName, assemFileName);

            
        //    Type type = modBuilder.GetType(typeName);
        //    if (type == null)
        //    {
        //        typeBuilder = modBuilder.DefineType(typeName, TypeAttributes.Class | TypeAttributes.Public,
        //        targetType.BaseType, new Type[] { interfaceType });
        //        target = typeBuilder.DefineField("target", interfaceType, FieldAttributes.Private);
        //        iface = typeBuilder.DefineField("iface", typeof(Type), FieldAttributes.Private);
        //        EmitConstructor(typeBuilder, target, iface);
        //        MethodInfo[] methods = interfaceType.GetMethods();
        //        foreach (MethodInfo m in methods)
        //            EmitProxyMethod(m, typeBuilder);
        //        type = typeBuilder.CreateType();
        //    }

        //    //将当前程序集信息写入IOC配置文件，以便下次由IOC加载当前程序集
        //    IocProvider provider = new IocProvider();
        //    provider.Key = interfaceType.FullName ;
        //    provider.InterfaceName = Unity.Instance.GetInterfaceName(interfaceType.FullName );
        //    provider.FullClassName = targetType.FullName;
        //    provider.Assembly = typeName;
        //    IOCConfig.AddIocProvider("AOP", provider);

        //    //保存当前的动态程序集
        //    assemblyBuilder.Save(assemFileName);

        //    return type;
        //}


        private static Type EmiProxyType(Type targetType, Type interfaceType)
        {
            AppDomain currentDomain = System.Threading.Thread.GetDomain();
            AssemblyName assemblyName = new AssemblyName();
            assemblyName.Name = AssemblyName;

            //Only save the custom-type dll while debugging 
            AssemblyBuilder assemblyBuilder =
            currentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder modBuilder = assemblyBuilder.DefineDynamicModule(ClassName);

            string typeName = assemblyName + "__Proxy" + interfaceType.Name + targetType.Name;
            Type type = modBuilder.GetType(typeName);
            if (type == null)
            {
                typeBuilder = modBuilder.DefineType(typeName, TypeAttributes.Class | TypeAttributes.Public,
                targetType.BaseType, new Type[] { interfaceType });
                target = typeBuilder.DefineField("target", interfaceType, FieldAttributes.Private);
                iface = typeBuilder.DefineField("iface", typeof(Type), FieldAttributes.Private);
                EmitConstructor(typeBuilder, target, iface);
                MethodInfo[] methods = interfaceType.GetMethods();
                foreach (MethodInfo m in methods)
                    EmitProxyMethod(m, typeBuilder);
                type = typeBuilder.CreateType();
            }

            return type;
        }

        

        private static void EmitProxyMethod(MethodInfo method, TypeBuilder typeBuilder)
        {
            // 1、定义动态IL 生成对象 
            Type[] paramTypes = GetParameterTypes(method);
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(method.Name,
            MethodAttributes.Public | MethodAttributes.Virtual, method.ReturnType, paramTypes);
            ILGenerator il = methodBuilder.GetILGenerator();
            LocalBuilder parameters = il.DeclareLocal(typeof(object[]));
            il.Emit(OpCodes.Ldc_I4, paramTypes.Length);
            il.Emit(OpCodes.Newarr, typeof(object));
            il.Emit(OpCodes.Stloc, parameters);
            for (int i = 0; i < paramTypes.Length; i++)
            {
                il.Emit(OpCodes.Ldloc, parameters);
                il.Emit(OpCodes.Ldc_I4, i);
                il.Emit(OpCodes.Ldarg, i + 1);
                if (paramTypes[i].IsValueType)
                    il.Emit(OpCodes.Box, paramTypes[i]);
                il.Emit(OpCodes.Stelem_Ref);
            }
            il.EmitCall(OpCodes.Callvirt,
            typeof(DecoratorInjector).GetProperty("InjectHandler").GetGetMethod(), null);

            // 2、生成目标对象实例 
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, (FieldInfo)target);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, (FieldInfo)target);
            il.EmitCall(OpCodes.Call, typeof(object).GetMethod("GetType"), null);
            il.EmitCall(OpCodes.Call, typeof(MethodBase).GetMethod("GetCurrentMethod"), null);

            // 3、生成目标对象方法 
            il.EmitCall(OpCodes.Call, typeof(DecoratorInjector).GetMethod("GetMethod"), null);

            // 4、生成参数 
            il.Emit(OpCodes.Ldloc, parameters);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, (FieldInfo)iface);
            il.EmitCall(OpCodes.Call, typeof(MethodBase).GetMethod("GetCurrentMethod"), null);
            il.EmitCall(OpCodes.Call, typeof(DecoratorInjector).GetMethod("GetMethod"), null);
            il.Emit(OpCodes.Ldtoken, typeof(DecoratorAttribute));
            il.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));
            il.Emit(OpCodes.Ldc_I4, 1);
            il.EmitCall(OpCodes.Callvirt,
            typeof(MethodInfo).GetMethod("GetCustomAttributes",
            new Type[] { typeof(Type), typeof(bool) }), null);

            // 5、导入“横切”对象 
            il.EmitCall(OpCodes.Callvirt, typeof(DecoratorInjector).GetMethod("UnionDecorators"), null);
            il.EmitCall(OpCodes.Callvirt, typeof(MethodCall).GetMethod("Invoke"), null);
            if (method.ReturnType == typeof(void))
                il.Emit(OpCodes.Pop);
            else if (method.ReturnType.IsValueType)
            {
                il.Emit(OpCodes.Unbox, method.ReturnType);
                il.Emit(OpCodes.Ldind_Ref);
            }
            il.Emit(OpCodes.Ret);
        }

        /// 通过动态生成的MSIL形成构造方法 
        private static void EmitConstructor(TypeBuilder typeBuilder, FieldBuilder target, FieldBuilder iface)
        {
            Type objType = Type.GetType("System.Object");
            ConstructorInfo objCtor = objType.GetConstructor(new Type[0]);
            ConstructorBuilder pointCtor = typeBuilder.DefineConstructor(MethodAttributes.Public,
            CallingConventions.Standard, new Type[] { typeof(object), typeof(Type) });

            ILGenerator ctorIL = pointCtor.GetILGenerator();
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Call, objCtor);
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Ldarg_1);
            ctorIL.Emit(OpCodes.Stfld, target);
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Ldarg_2);
            ctorIL.Emit(OpCodes.Stfld, iface);
            ctorIL.Emit(OpCodes.Ret);
        }

        public static MethodCall InjectHandler
        {
            get { return new MethodCall(InjectHandlerMethod); }
        }

        public static Type[] GetParameterTypes(MethodInfo method)
        {
            if (method == null) return null;
            Type[] types = new Type[method.GetParameters().Length];
            int i = 0;
            foreach (ParameterInfo parameter in method.GetParameters())
                types[i++] = parameter.ParameterType;
            return types;
        }

        public static MethodInfo GetMethod(Type type, MethodBase method)
        {
            return type.GetMethod(method.Name);
        }

        public static DecoratorAttribute[] UnionDecorators(object[] obj)
        {
            return (DecoratorAttribute[])obj;
        }
    } 


}
