/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap
 * ========================================================================
 * 该类的作用 实体类构造程序，提供一个接口，自动创建实体类
 *
 * 作者：bluedoctor     时间：2008-10-12
 * 版本：V5.1.2
 *
 *  修改者：         时间：2015-2-11
 *  新增 RegisterType 方法，实现接口和具体实体类类型的注册
 *
 * ========================================================================
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using PWMIS.Common;

namespace PWMIS.DataMap.Entity
{
    /// <summary>
    ///     PDF.NET实体类构造器，程序内部使用
    /// </summary>
    public class EntityBuilder
    {
        private static readonly Dictionary<Type, Type> dictEntityType = new();
        private static readonly object sync_lock = new();
        private readonly Type targetType;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="targetType">被实现或者继承的类型</param>
        public EntityBuilder(Type targetType)
        {
            this.targetType = targetType;
        }

        /// <summary>
        ///     注册实体类的具体实现类
        /// </summary>
        /// <param name="interfaceType">接口类型</param>
        /// <param name="instanceType">实例类型</param>
        public static void RegisterType(Type interfaceType, Type instanceType)
        {
            if (!instanceType.IsSubclassOf(typeof(EntityBase)))
                throw new ArithmeticException(instanceType.Name + " 必须是 EntityBase 的实现类！");
            if (instanceType.GetInterface(interfaceType.Name) != interfaceType)
                throw new ArithmeticException(instanceType.Name + " 必须是 " + interfaceType.Name + " 的实现类！");
            dictEntityType.Add(interfaceType, instanceType);
        }

        /// <summary>
        ///     根据接口类型，创建实体类的实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateEntity<T>() where T : class
        {
            Type targetType = null;
            var sourceType = typeof(T);

            if (sourceType.IsSubclassOf(typeof(EntityBase)) || sourceType.IsSubclassOf(typeof(IReadData))
                                                            || !sourceType.IsInterface)
                return Activator.CreateInstance<T>();

            //如果在类型字典里面没有找到接口的类型实现，则自动创建一个实体类
            if (!dictEntityType.TryGetValue(sourceType, out targetType))
                lock (sync_lock)
                {
                    if (!dictEntityType.TryGetValue(sourceType, out targetType))
                    {
                        var builder = new EntityBuilder(sourceType);
                        targetType = builder.Build();
                        dictEntityType[sourceType] = targetType;
                    }
                }

            var entity = (T)Activator.CreateInstance(targetType);
            return entity;
        }

        /// <summary>
        ///     构造实体类
        /// </summary>
        /// <returns></returns>
        public Type Build()
        {
            var currentAppDomain = AppDomain.CurrentDomain;
            var assyName = new AssemblyName();

            //为要创建的Assembly定义一个名称（这里忽略版本号，Culture等信息）
            assyName.Name = "PDFNetAssyFor_" + targetType.Name;

            //AssemblyBuilderAccess有Run，Save，RunAndSave三个取值
            var assyBuilder = AssemblyBuilder.DefineDynamicAssembly(assyName, AssemblyBuilderAccess.Run);

            /*
            //定义调试信息
            CustomAttributeBuilder debugAttributeBuilder = new CustomAttributeBuilder(
            typeof(DebuggableAttribute).GetConstructor(new Type[] { typeof(DebuggableAttribute.DebuggingModes) }),
            new object[] {
DebuggableAttribute.DebuggingModes.DisableOptimizations | DebuggableAttribute.DebuggingModes.Default
}
            );
            assyBuilder.SetCustomAttribute(debugAttributeBuilder);
            */

            var modBuilder =
                assyBuilder.DefineDynamicModule("PDFNetModFor_" + targetType.Name); //, assyName.Name + ".dll"
            //ISymbolDocumentWriter DOC = modBuilder.DefineDocument(@"e:\ilSource.txt", Guid.Empty, Guid.Empty, Guid.Empty);//要定义源代码位置，这个文档不需要全部源代码，只需要你想调试的il源代码翻译就可以了

            var newTypeName = "PDFNetDynamicEntity_" + targetType.Name;

            //新类型的属性：要创建的是Class，而非Interface，Abstract Class等，而且是Public的
            var newTypeAttribute = TypeAttributes.Class | TypeAttributes.Public;

            //声明要创建的新类型的父类型
            Type newTypeParent;

            //声明要创建的新类型要实现的接口
            Type[] newTypeInterfaces;

            //对于基类型是否为接口，作不同处理
            if (targetType.IsInterface)
            {
                newTypeParent = typeof(EntityBase);
                newTypeInterfaces = new[] { targetType };
            }
            else
            {
                newTypeParent = targetType;
                newTypeInterfaces = new Type[0];
            }

            //得到类型生成器            
            var typeBuilder = modBuilder.DefineType(newTypeName, newTypeAttribute, newTypeParent, newTypeInterfaces);
            typeBuilder.AddInterfaceImplementation(targetType);

            //定义构造函数
            BuildConstructor(typeBuilder, newTypeParent, targetType.Name);

            //以下将为新类型声明方法：新类型应该override基类型的所以virtual方法
            var pis = targetType.GetProperties();
            var propertyNames = new List<string>();

            foreach (var pi in pis)
            {
                propertyNames.Add(pi.Name);
                //属性构造器
                var propBuilder = typeBuilder.DefineProperty(pi.Name,
                    PropertyAttributes.HasDefault,
                    pi.PropertyType,
                    null);

                var getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig |
                                 MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.Final;
                //构造Get访问器
                var getPropMethodBuilder = typeBuilder.DefineMethod("get_" + pi.Name,
                    getSetAttr,
                    pi.PropertyType,
                    Type.EmptyTypes);
                GeterIL(pi.Name, newTypeParent, pi.PropertyType, getPropMethodBuilder);
                //构造Set访问器
                var setPropMethodBuilder = typeBuilder.DefineMethod("set_" + pi.Name,
                    getSetAttr,
                    null,
                    new[] { pi.PropertyType });
                SeterIL(pi.Name, newTypeParent, pi.PropertyType, setPropMethodBuilder);
                //添加到属性构造器
                propBuilder.SetGetMethod(getPropMethodBuilder);
                propBuilder.SetSetMethod(setPropMethodBuilder);
            }

            var SetFieldNamesBuilder = typeBuilder.DefineMethod("SetFieldNames",
                MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.HideBySig);
            SetFieldNamesIL(newTypeParent, SetFieldNamesBuilder, propertyNames.ToArray());

            //真正创建，并返回
            Type resuleType = typeBuilder.CreateTypeInfo();
            //assyBuilder.Save(assyName.Name+".dll");
            return resuleType;
        }

        /// <summary>
        ///     构造构造函数
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="newTypeParent"></param>
        /// <param name="newTypeName"></param>
        private void BuildConstructor(TypeBuilder typeBuilder, Type newTypeParent, string newTypeName)
        {
            //去除接口名称的约定 I 字母打头，作为表名称
            var tableName = newTypeName.Length > 1 ? newTypeName.Substring(1) : newTypeName;

            var constructorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.HideBySig |
                MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.HasThis,
                null);
            var ctorIL = constructorBuilder.GetILGenerator();
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Call, newTypeParent.GetConstructors(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)[0]);

            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Ldstr, tableName);
            ctorIL.Emit(OpCodes.Call,
                newTypeParent.GetMethod("set_TableName", BindingFlags.NonPublic | BindingFlags.Instance));
            ctorIL.Emit(OpCodes.Ret);
        }

        /// <summary>
        ///     重载 SetFieldNames 方法
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="methodBuilder"></param>
        /// <param name="names"></param>
        private void SetFieldNamesIL(Type baseType, MethodBuilder methodBuilder, string[] names)
        {
            var str = string.Join(",", names);
            var splitMI = typeof(string).GetMethod("Split", new[] { typeof(char[]) });
            var set_PropertyNamesMI =
                baseType.GetMethod("set_PropertyNames", BindingFlags.Instance | BindingFlags.NonPublic);

            var ilGenerator = methodBuilder.GetILGenerator();
            var charArray = ilGenerator.DeclareLocal(typeof(char[]));
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldstr, str);
            ilGenerator.Emit(OpCodes.Ldc_I4_1);
            ilGenerator.Emit(OpCodes.Newarr, typeof(char));
            ilGenerator.Emit(OpCodes.Stloc_0);
            ilGenerator.Emit(OpCodes.Ldloc_0);
            ilGenerator.Emit(OpCodes.Ldc_I4_0);
            ilGenerator.Emit(OpCodes.Ldc_I4_S, 0x2c);
            ilGenerator.Emit(OpCodes.Stelem_I2);
            ilGenerator.Emit(OpCodes.Ldloc_0);
            ilGenerator.Emit(OpCodes.Callvirt, splitMI);
            ilGenerator.Emit(OpCodes.Callvirt, set_PropertyNamesMI);
            ilGenerator.Emit(OpCodes.Ret);
        }

        /// <summary>
        ///     构造Get访问器
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="baseType"></param>
        /// <param name="propertyType"></param>
        /// <param name="methodBuilder"></param>
        private void GeterIL(string propertyName, Type baseType, Type propertyType, MethodBuilder methodBuilder)
        {
            MethodInfo getProperty = null;

            var ms = typeof(EntityBase).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var info in ms)
                if (info.Name == "getProperty" && info.IsGenericMethod)
                {
                    getProperty = info;
                    break;
                }

            getProperty = getProperty.MakeGenericMethod(propertyType);

            var ilGenerator = methodBuilder.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldstr, propertyName);
            ilGenerator.Emit(OpCodes.Call, getProperty);
            ilGenerator.Emit(OpCodes.Ret);
        }

        /// <summary>
        ///     构造Set访问器
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="baseType"></param>
        /// <param name="propertyType"></param>
        /// <param name="methodBuilder"></param>
        private void SeterIL(string propertyName, Type baseType, Type propertyType, MethodBuilder methodBuilder)
        {
            MethodInfo
                setProperty =
                    null; //= baseType.GetMethod("setProperty", BindingFlags.Instance | BindingFlags.NonPublic);
            var ms = typeof(EntityBase).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var info in ms)
                if (info.Name == "setProperty")
                    if (info.GetParameters().Length == 2)
                    {
                        setProperty = info;
                        break;
                    }

            var ilGenerator = methodBuilder.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldstr, propertyName);
            ilGenerator.Emit(OpCodes.Ldarg_1);
            //是否是值类型
            if (propertyType.IsValueType)
                ilGenerator.Emit(OpCodes.Box, propertyType);
            ilGenerator.Emit(OpCodes.Call, setProperty);
            ilGenerator.Emit(OpCodes.Ret);
        }
    }
}