﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;

namespace PWMIS.Core
{
    /// <summary>
    /// 通用方法类
    /// </summary>
   public  class CommonUtil
    {
       static readonly UniqueSequenceGUID UniqueId = new UniqueSequenceGUID();

       /// <summary>
       /// 缓存的SQLMAP实体类中映射的SQL，键为实体类的SQL查询名称
       /// </summary>
       public static Dictionary<string, string> CacheEntityMapSql
       {
           get;
           set;
       }

      
       /// <summary>
       /// 从指定的程序集中获取一个嵌入式资源的文本内容
       /// </summary>
       /// <param name="assemblyName">程序集名称</param>
       /// <param name="resourceName">资源名称</param>
       /// <returns>资源内容</returns>
       public static string GetAssemblyResource(string assemblyName, string resourceName)
       {
           Assembly ass = Assembly.Load(assemblyName);
           return GetAssemblyResource(ass, resourceName);
       }

       /// <summary>
       /// 根据指定的类型，从该类型所在的程序集中获取指定的资源内容文本。
       /// 注意：resourceType的命名空间应该使用程序集默认命名空间，如果源文件在项目子目录下，还应该加所在目录名字为该类型的完整命名空间。
       /// </summary>
       /// <param name="resourceType">与指定的类型</param>
       /// <param name="resourceName"></param>
       /// <returns></returns>
       public static string GetAssemblyResource(Type resourceType, string resourceName)
       {
           Assembly ass = resourceType.Assembly;
           resourceName = resourceType.Namespace + "." + resourceName;
           return GetAssemblyResource(ass, resourceName);
       }

       /// <summary>
       /// 在指定的程序集中寻找指定嵌入式资源文本内容
       /// </summary>
       /// <param name="assembly">资源所在的程序集</param>
       /// <param name="resourceName">资源的名称</param>
       /// <returns>嵌入式资源文本内容</returns>
       public static string GetAssemblyResource(Assembly assembly, string resourceName)
       {
           Stream so = assembly.GetManifestResourceStream(resourceName);
           if (so == null)
               throw new Exception("未找到指定的嵌入式资源: " + resourceName);
           StreamReader sr = new StreamReader(so);
           string result = sr.ReadToEnd();
           sr.Close();
           return result;
       }

       /// <summary>
       /// 为字段名加上中括号，避免字段名中有空格的问题
       /// </summary>
       /// <param name="fields">字段名称数组</param>
       /// <returns>新的字段名数组</returns>
       public static string[] PrepareSqlFields(string[] fields)
       {
           string[] result = new string[fields.Length];
           for (int i = 0; i < fields.Length; i++)
               result[i] = "[" + fields[i] + "]";
           return result;
       }

        /// <summary>
        /// 替换Web路径格式中的相对虚拟路径（~\ 或者 ~/）为当前程序执行的绝对路径
        /// </summary>
        /// <param name="sourcePath"></param>
        public static void ReplaceWebRootPath(ref string sourcePath)
        {
            //处理 相对路径，假定 ~ 路径格式就是Web程序的相对路径 edit at 2015-11-29
            if (!string.IsNullOrEmpty(sourcePath) && sourcePath.IndexOf('~') >= 0)
            {
                string appRootPath;
                var escapedCodeBase = Assembly.GetExecutingAssembly().EscapedCodeBase;
                var u = new Uri(escapedCodeBase);
                var path = Path.GetDirectoryName(u.LocalPath);
                if (path != null && (path.Length > 4 && path.EndsWith("bin", StringComparison.OrdinalIgnoreCase)))
                {
                    appRootPath = path.Substring(0, path.Length - 3);// 去除Web项目的 \bin，获取根目录
                }
                else
                {
                    //解决类似SqlServer文件型数据库要求的绝对路径问题，
                    //例如 Data Source=(LocalDB)\v11.0;AttachDbFilename=~/Database1.mdf;Integrated Security=True
                    appRootPath = path+ System.IO.Path.DirectorySeparatorChar;
                }

                //判断 Data Source或者 AttachDbFilename 之后的 ~/ 符号并替换，这里先简单替换：
                //sourcePath = Regex.Replace(sourcePath, @"^\s*~[\\/]", appRootPath);
                //sourcePath = Regex.Replace(sourcePath, @"data source\s*=\s*~[\\/]", "Data Source=" + appRootPath, RegexOptions.IgnoreCase);
                sourcePath = sourcePath.Replace("~/", appRootPath);
                sourcePath = sourcePath.Replace("~\\", appRootPath);
            }
        }

        /// <summary>
        /// 泛型类型转换方法
        /// </summary>
        /// <example>
        ///  object o1 = 111;
        /// int i = getProperty《int》(o1);
        /// o1 = DBNull.Value;
        /// i = getProperty《int》(o1);
        /// DateTime d = getProperty《DateTime》(o1);
        /// o1 = 123.33m;
        /// double db = getProperty《double》(o1);
        /// o1 = "123.4";
        /// float f = getProperty《float》(o1);
        /// o1 = null;
        /// f = getProperty《float》(o1);
        /// </example>
        /// <typeparam name="T">要转换的目标类型</typeparam>
        /// <param name="Value">Object类型的待转换对象</param>
        /// <returns>目标类型</returns>
        public static T ChangeType<T>(object Value)
       {
           if (Value is T)
               return (T)Value;
           else if (Value == DBNull.Value || Value == null)
           {
               if (typeof(T) == typeof(DateTime))
               {
                   //如果取日期类型的默认值 0001/01/01 ,在WCF JSON序列化的时候，会失败。
                   object o = new DateTime(1900, 1, 1);
                   return (T)o;
               }
               else
                   return default(T);
           }
           else
           {
                //如果 Value为 decimal类型，Ｔ　为double 类型， (T)Value 将发生错误
                //支持枚举类型
                Type currType = typeof(T);

               if (currType.IsEnum)
                   return (T)Convert.ChangeType(Value, System.TypeCode.Int32);
               else if (!currType.IsGenericType)
               {
                    //2018.2.14 解决Value是Guid转换成string的问题
                    if (currType == typeof(string))
                        return (T)Convert.ChangeType(Value.ToString(), currType);
                    else
                        return (T)Convert.ChangeType(Value, currType);
               }
               else
               {
                   //增加对可空类型的支持，网友 ※DS 提供代码
                   Type genericTypeDefinition = currType.GetGenericTypeDefinition();
                   if (genericTypeDefinition == typeof(Nullable<>))
                   {
                       if (string.IsNullOrEmpty(Value.ToString()))
                       {
                           return default(T);
                       }
                       return (T)Convert.ChangeType(Value, Nullable.GetUnderlyingType(currType));
                   }
                   return (T)Convert.ChangeType(Value, currType);
               }
           }


       }

        public static object ChangeType(object Value, Type targetType)
        {
            if (Value == null || Value == DBNull.Value)
            {
                if (targetType == typeof(DateTime))
                    return new DateTime(1900, 1, 1);
                else
                    return Value;
            }
            if (Value.ToString() == string.Empty )
            {
                if( targetType == typeof(string))

                    return string.Empty;
                else
                    return null;
            }
            if (Value.GetType() == targetType)
                return Value;
            return Convert.ChangeType(Value, targetType);
        }


       /// <summary>
       /// Type 转换成 DbType
       /// </summary>
       /// <param name="t"></param>
       /// <returns></returns>
       /// <remarks>参考http://www.cnblogs.com/davinci/archive/2010/01/22/1654139.html</remarks>
       public static DbType TypeToDbType(Type t)
       {
           DbType dbt;
           try
           {
               dbt = (DbType)Enum.Parse(typeof(DbType), t.Name);
           }
           catch
           {
               dbt = DbType.Object;
           }
           return dbt;
       }

       /// <summary>
       /// DbType 转换成 Type
       /// </summary>
       /// <param name="dbType"></param>
       /// <returns></returns>
       /// <remarks>参考 http://www.cnblogs.com/davinci/archive/2010/01/22/1654139.html</remarks>
       static Type ConvertType(DbType dbType)
       {
           Type toReturn = typeof(DBNull);

           switch (dbType)
           {
               case DbType.UInt64:
                   toReturn = typeof(UInt64);
                   break;

               case DbType.Int64:
                   toReturn = typeof(Int64);
                   break;

               case DbType.Int32:
                   toReturn = typeof(Int32);
                   break;

               case DbType.UInt32:
                   toReturn = typeof(UInt32);
                   break;

               case DbType.Single:
                   toReturn = typeof(float);
                   break;

               case DbType.Date:
               case DbType.DateTime:
               case DbType.Time:
                   toReturn = typeof(DateTime);
                   break;

               case DbType.String:
               case DbType.StringFixedLength:
               case DbType.AnsiString:
               case DbType.AnsiStringFixedLength:
                   toReturn = typeof(string);
                   break;

               case DbType.UInt16:
                   toReturn = typeof(UInt16);
                   break;

               case DbType.Int16:
                   toReturn = typeof(Int16);
                   break;

               case DbType.SByte:
                   toReturn = typeof(byte);
                   break;

               case DbType.Object:
                   toReturn = typeof(object);
                   break;

               case DbType.VarNumeric:
               case DbType.Decimal:
                   toReturn = typeof(decimal);
                   break;

               case DbType.Currency:
                   toReturn = typeof(double);
                   break;

               case DbType.Binary:
                   toReturn = typeof(byte[]);
                   break;

               case DbType.Double:
                   toReturn = typeof(Double);
                   break;

               case DbType.Guid:
                   toReturn = typeof(Guid);
                   break;

               case DbType.Boolean:
                   toReturn = typeof(bool);
                   break;
           }

           return toReturn;
       }

       /// <summary>
       /// 生成一个新的在秒级别有序的长整形“GUID”，在一秒内，数据比较随机，线程安全，
       /// 但不如NewUniqueSequenceGUID 方法结果更有序（不包含毫秒部分）
       /// </summary>
       /// <returns></returns>
       public static long NewSequenceGUID()
       {
           return UniqueSequenceGUID.InnerNewSequenceGUID(DateTime.Now,false);
       }

        /// <summary>
        /// 生成一个唯一的更加有序的GUID形式的长整数,在一秒内，重复概率低于 千万分之一，线程安全。可用于严格有序增长的ID
        /// </summary>
        /// <returns></returns>
        public static long NewUniqueSequenceGUID()
       {
           return UniqueId.NewID();
       }

        /// <summary>
        /// 当前机器ID，可以作为分布式ID，如果需要指定此ID，请在应用程序配置文件配置 SOD_MachineID 的值，范围大于100，小于1000.
        /// </summary>
        /// <returns></returns>
        public static int CurrentMachineID()
        {
            return UniqueSequenceGUID.GetCurrentMachineID();
        }

    }
}
