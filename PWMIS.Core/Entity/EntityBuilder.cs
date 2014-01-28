using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using PWMIS.DataMap.Entity;
using PWMIS.Common;

namespace PWMIS.DataMap.Entity
{
    /// <summary>
    /// PDF.NET实体类构造器，程序内部使用
    /// </summary>
    public class EntityBuilder
    {
        private Type targetType;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="targetType">被实现或者继承的类型</param>
        public EntityBuilder(Type targetType)
        {
            this.targetType = targetType;
        }

        private static Dictionary<Type, Type> dictEntityType = new Dictionary<Type, Type>();
        private static object sync_lock = new object();

        /// <summary>
        /// 根据接口类型，创建实体类的实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateEntity<T>() where T:class
        {
            Type targetType = null;
            Type sourceType = typeof(T);
           
            if (sourceType.IsSubclassOf(typeof(EntityBase)) || sourceType.IsSubclassOf(typeof(IReadData)) 
                || !sourceType.IsInterface)
            {
               return Activator.CreateInstance<T>();
            }
            else
            {
                if (!dictEntityType.TryGetValue(sourceType, out targetType))
                {
                    lock (sync_lock)
                    {
                        if (!dictEntityType.TryGetValue(sourceType, out targetType))
                        {
                            EntityBuilder builder = new EntityBuilder(sourceType);
                            targetType = builder.Build();
                            dictEntityType[sourceType] = targetType;
                        }
                    }
                }
            }
            
            T entity = (T)Activator.CreateInstance(targetType);
            return entity;
        }

        /// <summary>
        /// 构造实体类
        /// </summary>
        /// <returns></returns>
        public Type Build()
        {
            AppDomain currentAppDomain = AppDomain.CurrentDomain;
            AssemblyName assyName = new AssemblyName();

            //为要创建的Assembly定义一个名称（这里忽略版本号，Culture等信息）
            assyName.Name = "PDFNetAssyFor_" + targetType.Name;

            //AssemblyBuilderAccess有Run，Save，RunAndSave三个取值
            AssemblyBuilder assyBuilder = currentAppDomain.DefineDynamicAssembly(assyName, AssemblyBuilderAccess.Run);

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

            ModuleBuilder modBuilder = assyBuilder.DefineDynamicModule("PDFNetModFor_" + targetType.Name);//, assyName.Name + ".dll"
            //ISymbolDocumentWriter DOC = modBuilder.DefineDocument(@"e:\ilSource.txt", Guid.Empty, Guid.Empty, Guid.Empty);//要定义源代码位置，这个文档不需要全部源代码，只需要你想调试的il源代码翻译就可以了

            String newTypeName = "PDFNetDynamicEntity_" + targetType.Name;

            //新类型的属性：要创建的是Class，而非Interface，Abstract Class等，而且是Public的
            TypeAttributes newTypeAttribute = TypeAttributes.Class | TypeAttributes.Public;

            //声明要创建的新类型的父类型
            Type newTypeParent;

            //声明要创建的新类型要实现的接口
            Type[] newTypeInterfaces;

            //对于基类型是否为接口，作不同处理
            if (targetType.IsInterface)
            {
                newTypeParent = typeof(EntityBase);
                newTypeInterfaces = new Type[] { targetType };
            }
            else
            {
                newTypeParent = targetType;
                newTypeInterfaces = new Type[0];
            }

            //得到类型生成器            
            TypeBuilder typeBuilder = modBuilder.DefineType(newTypeName, newTypeAttribute, newTypeParent, newTypeInterfaces);
            typeBuilder.AddInterfaceImplementation(targetType);

            //定义构造函数
            BuildConstructor(typeBuilder, newTypeParent, targetType.Name);

            //以下将为新类型声明方法：新类型应该override基类型的所以virtual方法
            PropertyInfo[] pis = targetType.GetProperties();
            List<string> propertyNames = new List<string>();

            foreach (PropertyInfo pi in pis)
            {
                propertyNames.Add(pi.Name);
                //属性构造器
                PropertyBuilder propBuilder = typeBuilder.DefineProperty(pi.Name,
                    System.Reflection.PropertyAttributes.HasDefault,
                    pi.PropertyType,
                    null);

                MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.Final;
                //构造Get访问器
                MethodBuilder getPropMethodBuilder = typeBuilder.DefineMethod("get_" + pi.Name,
                    getSetAttr, 
                    pi.PropertyType, 
                    Type.EmptyTypes);
                GeterIL(pi.Name, newTypeParent, pi.PropertyType, getPropMethodBuilder);
                //构造Set访问器
                MethodBuilder setPropMethodBuilder = typeBuilder.DefineMethod("set_" + pi.Name, 
                    getSetAttr,
                    null,
                    new Type[] { pi.PropertyType });
                SeterIL(pi.Name, newTypeParent, pi.PropertyType, setPropMethodBuilder);
                //添加到属性构造器
                propBuilder.SetGetMethod(getPropMethodBuilder);
                propBuilder.SetSetMethod(setPropMethodBuilder);
            }

            MethodBuilder SetFieldNamesBuilder = typeBuilder.DefineMethod("SetFieldNames", MethodAttributes.Family  | MethodAttributes.Virtual | MethodAttributes.HideBySig);
            SetFieldNamesIL(newTypeParent, SetFieldNamesBuilder, propertyNames.ToArray());
            
            //真正创建，并返回
            Type resuleType=typeBuilder.CreateType();
            //assyBuilder.Save(assyName.Name+".dll");
            return resuleType;
        }
        /// <summary>
        /// 构造构造函数
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="newTypeParent"></param>
        /// <param name="newTypeName"></param>
        void BuildConstructor(TypeBuilder typeBuilder, Type newTypeParent, string newTypeName)
        {
            //去除接口名称的约定 I 字母打头，作为表名称
            string tableName = newTypeName.Length > 1 ? newTypeName.Substring(1) : newTypeName;

            ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.HideBySig | 
                MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.HasThis,
                null);
            ILGenerator ctorIL = constructorBuilder.GetILGenerator();
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Call, newTypeParent.GetConstructors(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)[0]);

            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Ldstr, tableName);
            ctorIL.Emit(OpCodes.Call, newTypeParent.GetMethod("set_TableName", BindingFlags.NonPublic | BindingFlags.Instance));
            ctorIL.Emit(OpCodes.Ret);
        }
        /// <summary>
        /// 重载 SetFieldNames 方法
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="methodBuilder"></param>
        /// <param name="names"></param>
        void SetFieldNamesIL(Type baseType, MethodBuilder methodBuilder, string[] names)
        {
            string str = string.Join(",", names);
            MethodInfo splitMI=typeof(string).GetMethod("Split", new Type[] { typeof(char[]) });
            MethodInfo set_PropertyNamesMI = baseType.GetMethod("set_PropertyNames", BindingFlags.Instance | BindingFlags.NonPublic);

            var ilGenerator = methodBuilder.GetILGenerator();
            var charArray = ilGenerator.DeclareLocal(typeof(char[]));
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldstr, str);
            ilGenerator.Emit(OpCodes.Ldc_I4_1);
            ilGenerator.Emit(OpCodes.Newarr,typeof(char));
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
        /// 构造Get访问器
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="baseType"></param>
        /// <param name="propertyType"></param>
        /// <param name="methodBuilder"></param>
        void GeterIL(string propertyName, Type baseType, Type propertyType, MethodBuilder methodBuilder)
        {
            MethodInfo getProperty = null;

            MethodInfo[] ms = typeof(EntityBase).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (MethodInfo info in ms)
            {
                if (info.Name == "getProperty" && info.IsGenericMethod)
                {
                    getProperty = info;
                    break;
                }
            }
            getProperty = getProperty.MakeGenericMethod(propertyType);

            var ilGenerator = methodBuilder.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldstr, propertyName);
            ilGenerator.Emit(OpCodes.Call, getProperty);
            ilGenerator.Emit(OpCodes.Ret);
        }
        /// <summary>
        /// 构造Set访问器
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="baseType"></param>
        /// <param name="propertyType"></param>
        /// <param name="methodBuilder"></param>
        void SeterIL(string propertyName, Type baseType, Type propertyType, MethodBuilder methodBuilder)
        {
            MethodInfo setProperty =null;//= baseType.GetMethod("setProperty", BindingFlags.Instance | BindingFlags.NonPublic);
            MethodInfo[] ms = typeof(EntityBase).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (MethodInfo info in ms)
            {
                if (info.Name == "setProperty" )
                {
                    if (info.GetParameters().Length == 2)
                    {
                        setProperty = info;
                        break;
                    }
                }
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
