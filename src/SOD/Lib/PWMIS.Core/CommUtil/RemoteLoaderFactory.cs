﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using PWMIS.DataMap.Entity;

//http://www.cnblogs.com/daxnet/archive/2009/01/12/1686999.html
/*
 * private void button1_Click(object sender, EventArgs e)
2.{
3.    LocalLoader ll = new LocalLoader();
4.    ll.LoadAssembly(@"C:\testlib.dll");
5.    label1.Text = ll.FullName;
6.    ll.Unload();
7.}

 *
 */

namespace PWMIS.Core
{
    public class RemoteLoader : MarshalByRefObject
    {
        private Assembly assembly;

        public string FullName => assembly.FullName;

        public string ErrorMessage { get; private set; }

        public bool IsEntityClass { get; private set; }

        public void LoadAssembly(string fullName)
        {
            assembly = Assembly.LoadFrom(fullName);
        }

        public string[] GetAllTypeNames()
        {
            var list = new List<string>();
            foreach (var t in assembly.GetTypes()) list.Add(t.FullName + "," + t.Name);
            return list.ToArray();
        }

        public List<KeyValuePair<string, string>> GetAllPropertyNames(string className)
        {
            Type objType = null;
            ErrorMessage = "";
            foreach (var t in assembly.GetTypes())
                if (t.FullName == className || t.Name == className)
                {
                    objType = t;
                    break;
                }

            if (objType != null)
            {
                IsEntityClass = objType.BaseType.FullName == "PWMIS.DataMap.Entity.EntityBase";

                var dataList = new List<KeyValuePair<string, string>>();
                SearchPropertys(objType, "", dataList);
                return dataList;
            }

            IsEntityClass = false;
            ErrorMessage = "未找到类型 " + className;
            return null;
        }

        private void SearchPropertys(Type objType, string parentPropName, List<KeyValuePair<string, string>> dataList)
        {
            foreach (var prop in objType.GetProperties(BindingFlags.Instance | BindingFlags.Public |
                                                       BindingFlags.DeclaredOnly | BindingFlags.NonPublic))
                if (prop.CanRead && prop.CanWrite)
                {
                    var propType = prop.PropertyType;
                    if (propType.IsValueType || propType == typeof(string))
                    {
                        var info = parentPropName + prop.Name.PadRight(20) + "|" + Type.GetTypeCode(propType);
                        dataList.Add(new KeyValuePair<string, string>(info, parentPropName + prop.Name));
                    }
                    else
                    {
                        //对于复杂类型，递归解析出所有的类型和属性
                        var info = parentPropName + prop.Name.PadRight(20) + "|" + propType;
                        dataList.Add(new KeyValuePair<string, string>(info, parentPropName + prop.Name));
                        SearchPropertys(propType, parentPropName + prop.Name + ".", dataList);
                    }
                }
        }

        public object Invoke(string fullClassName, string methodName, object[] paraValues)
        {
            ErrorMessage = "";
            Type objType = null;
            foreach (var t in assembly.GetTypes())
                if (t.FullName == fullClassName || t.Name == fullClassName)
                {
                    objType = t;
                    break;
                }

            if (objType != null)
            {
                var objMethod = objType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
                if (objMethod != null)
                    try
                    {
                        var instance = Activator.CreateInstance(objType);
                        return objMethod.Invoke(instance, paraValues);
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = ex.Message;
                    }
                else
                    ErrorMessage = "未找到方法 " + methodName;
            }
            else
            {
                ErrorMessage = "未找到类型 " + fullClassName;
            }

            return null;
        }

        /// <summary>
        ///     根据指定的实体类中的属性名，返回实体类的表名称和属性对应的字段名称
        /// </summary>
        /// <param name="fullClassName">实体类名称</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>返回实体类的表名称和属性对应的字段名称</returns>
        public string[] TableFieldName(string fullClassName, string propertyName)
        {
            ErrorMessage = "";
            Type objType = null;
            foreach (var t in assembly.GetTypes())
                if (t.FullName == fullClassName || t.Name == fullClassName)
                {
                    objType = t;
                    break;
                }

            if (objType != null)
            {
                var ef = new EntityFields();
                if (ef.InitEntity(objType))
                {
                    var fieldName = ef.GetPropertyField(propertyName);
                    if (fieldName == null)
                    {
                        ErrorMessage = "属性 " + propertyName + " 不是PDF.NET的实体类属性，无法找到对应的属性字段。";
                        return null;
                    }

                    string[] arr = { ef.TableName, fieldName };
                    return arr;
                }

                ErrorMessage = "类型 " + fullClassName + " 不是PDF.NET实体类。";
                return null;
            }

            ErrorMessage = "未找到类型 " + fullClassName;
            return null;
        }
    }

    public class LocalLoader
    {
        private readonly RemoteLoader remoteLoader;
        private AppDomain appDomain;

        /// <summary>
        ///     根据Pwmis.core.dll文件所在目录初始化加载器
        /// </summary>
        /// <param name="basePath"></param>
        public LocalLoader(string basePath)
        {
            var setup = new AppDomainSetup();
            setup.ApplicationName = "PdfNetApplication";
            setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            setup.PrivateBinPath = Path.Combine(setup.ApplicationBase, "private");
            setup.CachePath = setup.ApplicationBase;
            setup.ShadowCopyFiles = "true";
            setup.ShadowCopyDirectories = setup.ApplicationBase;

            appDomain = AppDomain.CreateDomain("PdfNetDomain", null, setup);
            var fileName = Path.Combine(basePath, "PWMIS.Core.dll");
            //var executeAssembly = Assembly.GetExecutingAssembly();
            //string name = executeAssembly.GetName().FullName;
            //remoteLoader = (RemoteLoader)appDomain.CreateInstanceAndUnwrap(
            //    name,
            //    typeof(RemoteLoader).FullName);

            remoteLoader = (RemoteLoader)appDomain.CreateInstanceFromAndUnwrap(
                fileName, //例如： @"E:\SimpleAccessWinForm\bin\Debug\PWMIS.Core.dll",
                typeof(RemoteLoader).FullName);
        }

        public string FullName => remoteLoader.FullName;

        public string[] GetAllTypeNames => remoteLoader.GetAllTypeNames();

        public string ErrorMessage => remoteLoader.ErrorMessage;

        public bool IsEntityClass => remoteLoader.IsEntityClass;

        public void LoadAssembly(string fullName)
        {
            remoteLoader.LoadAssembly(fullName);
        }

        public void Unload()
        {
            AppDomain.Unload(appDomain);
            appDomain = null;
        }

        public object Invoke(string fullClassName, string methodName, object[] paraValues)
        {
            return remoteLoader.Invoke(fullClassName, methodName, paraValues);
        }

        public List<KeyValuePair<string, string>> GetAllPropertyNames(string className)
        {
            return remoteLoader.GetAllPropertyNames(className);
        }

        public string[] TableFieldName(string fullClassName, string propertyName)
        {
            return remoteLoader.TableFieldName(fullClassName, propertyName);
        }
    }
}