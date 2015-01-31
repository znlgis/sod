using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Data;

namespace PWMIS.Core
{
    /// <summary>
    /// 通用方法类
    /// </summary>
   public  class CommonUtil
    {
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
       /// 根据指定的类型，从该类型所在的程序集中获取指定的资源内容文本
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
       /// 替换Web路径格式中的相对虚拟路径（~）为当前程序执行的绝对路径
       /// </summary>
       /// <param name="sourcePath"></param>
       public static void ReplaceWebRootPath(ref string sourcePath)
       {
           if (!string.IsNullOrEmpty(sourcePath) && sourcePath.IndexOf('~') >=0)
           {
               string appRootPath = "";
               string EscapedCodeBase = Assembly.GetExecutingAssembly().EscapedCodeBase;
               Uri u = new Uri(EscapedCodeBase);
               string path = Path.GetDirectoryName(u.LocalPath);
               if (path.Length > 4 && path.EndsWith("bin", StringComparison.OrdinalIgnoreCase))
               {
                   appRootPath = path.Substring(0, path.Length - 4);// 去除Web项目的 \bin，获取根目录
                   sourcePath = sourcePath.Replace("~", appRootPath);
               }
               else
               {
                   sourcePath = sourcePath.Replace("~", ".");
               }
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
               if (typeof(T).IsEnum)
                   return (T)Convert.ChangeType(Value, System.TypeCode.Int32);
               else if (!typeof(T).IsGenericType)
               {
                   return (T)Convert.ChangeType(Value, typeof(T));
               }
               else
               {
                   //增加对可空类型的支持，网友 ※DS 提供代码
                   Type genericTypeDefinition = typeof(T).GetGenericTypeDefinition();
                   if (genericTypeDefinition == typeof(Nullable<>))
                   {
                       if (string.IsNullOrEmpty(Value.ToString()))
                       {
                           return default(T);
                       }
                       return (T)Convert.ChangeType(Value, Nullable.GetUnderlyingType(typeof(T)));
                   }
                   return (T)Convert.ChangeType(Value, typeof(T));
               }
           }


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

    }
}
