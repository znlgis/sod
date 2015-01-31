/*
 * 属性转换类，将一个类的属性值转换给另外一个类的同名属性，注意该类使用的是浅表复制
 * 作者：bluedoctor 时间：2012-12-19
 * -----------------------------------------------------
 * 欢迎使用PDF.NET(PWMIS开发框架) 
 * http://www.pwmis.com/sqlmap
 * http://pwmis.codeplex.com
 * -----------------------------------------------------
 * 
 * 使用说明：请看类的注释。
 * 如果需要在.net 4.0 下面使用，可以取消FastPropertyAccessor 类的注释，有关
 * FastPropertyAccessor 类的原理，参见 
 * http://www.cnblogs.com/bluedoctor/archive/2012/12/18/2823325.html
 * 
 * 本文原理，参见
 * http://www.cnblogs.com/bluedoctor/archive/2012/12/20/2826392.html
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using PWMIS.DataMap.Entity;
//using System.Collections.Concurrent;

namespace PWMIS.Core.Extensions
{
    /// <summary>
    /// 属性转换类，将一个类的属性值转换给另外一个类的同名属性，注意该类使用的是浅表复制。
    /// <example>
    /// <code>
    /// <![CDATA[
    ///        //下面几种用法一样:
    ///        ModelCast.GetCast(typeof(CarInfo), typeof(ImplCarInfo)).Cast(info, ic);
    ///        ModelCast.CastObject<CarInfo, ImplCarInfo>(info, ic);
    ///        ModelCast.CastObject(info, ic);
    ///
    ///        ImplCarInfo icResult= info.CopyTo<ImplCarInfo>(null);
    ///
    ///        ImplCarInfo icResult2 = new ImplCarInfo();
    ///        info.CopyTo《ImplCarInfo》(icResult2);
    /// ]]>
    /// </code>
    /// </example>
    /// </summary>
    public class ModelCast
    {
        private List<CastProperty> mProperties = new List<CastProperty>();

        static Dictionary<Type, Dictionary<Type, ModelCast>> mCasters = new Dictionary<Type, Dictionary<Type, ModelCast>>(256);

        private static Dictionary<Type, ModelCast> GetModuleCast(Type sourceType)
        {
            Dictionary<Type, ModelCast> result;
            lock (mCasters)
            {
                if (!mCasters.TryGetValue(sourceType, out result))
                {
                    result = new Dictionary<Type, ModelCast>(8);
                    mCasters.Add(sourceType, result);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取要转换的当前转换类实例
        /// </summary>
        /// <param name="sourceType">要转换的源类型</param>
        /// <param name="targetType">目标类型</param>
        /// <returns></returns>
        public static ModelCast GetCast(Type sourceType, Type targetType)
        {
            Dictionary<Type, ModelCast> casts = GetModuleCast(sourceType);
            ModelCast result;
            lock (casts)
            {
                if (!casts.TryGetValue(targetType, out result))
                {
                    result = new ModelCast(sourceType, targetType);
                    casts.Add(targetType, result);
                }
            }
            return result;
        }

        /// <summary>
        /// 以两个要转换的类型作为构造函数，构造一个对应的转换类
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="targetType"></param>
        public ModelCast(Type sourceType, Type targetType)
        {
            PropertyInfo[] targetProperties = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo sp in sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                foreach (PropertyInfo tp in targetProperties)
                {
                    if (sp.Name == tp.Name && sp.PropertyType == tp.PropertyType)
                    {
                        CastProperty cp = new CastProperty();
                        cp.SourceProperty = new PropertyAccessorHandler(sp);
                        cp.TargetProperty = new PropertyAccessorHandler(tp);
                        mProperties.Add(cp);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 将源类型的属性值转换给目标类型同名的属性
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public void Cast(object source, object target)
        {
            Cast(source, target, null);
        }

        /// <summary>
        /// 将源类型的属性值转换给目标类型同名的属性，排除要过滤的属性名称
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="filter">要过滤的属性名称</param>
        public void Cast(object source, object target,string[] filter)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (target == null)
                throw new ArgumentNullException("target");
            if (source is EntityBase)
            {
                if (filter == null)
                    filter = new string[] { };
          
                var list = filter.ToList();
                list.Add("IdentityName");
                list.Add("PrimaryKeys");
                list.Add("PropertyNames");
                list.Add("PropertyValues");
                list.Add("TableName");
                list.Add("Item");
                filter = list.ToArray();
            }
            PropertyAccessorHandler ctp = null;
            for (int i = 0; i < mProperties.Count; i++)
            {
                CastProperty cp = mProperties[i];
                
                if (cp.SourceProperty.Getter != null)
                {
                    ctp=cp.TargetProperty;
                    if (ctp.Setter != null)
                    {
                        if (filter == null)
                        {
                            object Value = cp.SourceProperty.Getter(source, null); 
                            ctp.Setter(target, Value, null);
                        }
                        else
                        {
                            if (!filter.Contains(ctp.PropertyName))
                            {
                                object Value = cp.SourceProperty.Getter(source, null);
                                ctp.Setter(target, Value, null);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 转换对象
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="source">源对象</param>
        /// <param name="target">目标对象</param>
        public static void CastObject<TSource, TTarget>(TSource source, TTarget target)
            where TSource : class
            where TTarget : class
        {
            ModelCast.GetCast(typeof(TSource), typeof(TTarget)).Cast(source, target);
        }


        /// <summary>
        /// 转换属性对象
        /// </summary>
        public class CastProperty
        {
            /// <summary>
            /// 源属性
            /// </summary>
            public PropertyAccessorHandler SourceProperty
            {
                get;
                set;
            }
            /// <summary>
            /// 目标属性
            /// </summary>
            public PropertyAccessorHandler TargetProperty
            {
                get;
                set;
            }
        }

        /// <summary>
        /// 属性访问器
        /// </summary>
        public class PropertyAccessorHandler
        {
            /// <summary>
            /// 以一个属性对象初始化类
            /// </summary>
            /// <param name="propInfo">性对象</param>
            public PropertyAccessorHandler(PropertyInfo propInfo)
            {
                this.PropertyName = propInfo.Name;
                //var obj = Activator.CreateInstance(classType);
                //var getterType = typeof(FastPropertyAccessor.GetPropertyValue<>).MakeGenericType(propInfo.PropertyType);
                //var setterType = typeof(FastPropertyAccessor.SetPropertyValue<>).MakeGenericType(propInfo.PropertyType);

                //this.Getter = Delegate.CreateDelegate(getterType, null, propInfo.GetGetMethod());
                //this.Setter = Delegate.CreateDelegate(setterType, null, propInfo.GetSetMethod());
                if(propInfo.CanRead)
                    this.Getter = propInfo.GetValue;

                if(propInfo.CanWrite)
                    this.Setter = propInfo.SetValue;
            }
            /// <summary>
            /// 属性名称
            /// </summary>
            public string PropertyName { get; set; }
            /// <summary>
            /// Get访问器
            /// </summary>
            public Func<object, object[], object> Getter { get; private set; }
            /// <summary>
            /// Set访问器
            /// </summary>
            public Action<object, object, object[]> Setter { get; private set; }
        }
    }

    ///// <summary>
    ///// 快速属性访问器，注意，这将于具体的对象相绑定，如果某对象用得不多，请使用 NoCache 相关的方法
    ///// </summary>
    //public class FastPropertyAccessor
    //{
    //    public delegate T GetPropertyValue<T>();
    //    public delegate void SetPropertyValue<T>(T Value);

    //    private static ConcurrentDictionary<string, Delegate> myDelegateCache = new ConcurrentDictionary<string, Delegate>();

    //    public static GetPropertyValue<T> CreateGetPropertyValueDelegate<TSource, T>(TSource obj, string propertyName)
    //    {
    //        string key = string.Format("DGP-{0}-{1}", typeof(TSource).FullName, propertyName);//Delegate-GetProperty-{0}-{1}
    //        GetPropertyValue<T> result = (GetPropertyValue<T>)myDelegateCache.GetOrAdd(
    //            key,
    //            newkey =>
    //            {
    //                return Delegate.CreateDelegate(typeof(GetPropertyValue<T>), obj, typeof(TSource).GetProperty(propertyName).GetGetMethod());
    //            }
    //            );

    //        return result;
    //    }
    //    public static SetPropertyValue<T> CreateSetPropertyValueDelegate<TSource, T>(TSource obj, string propertyName)
    //    {
    //        string key = string.Format("DSP-{0}-{1}", typeof(TSource).FullName, propertyName);//Delegate-SetProperty-{0}-{1}
    //        SetPropertyValue<T> result = (SetPropertyValue<T>)myDelegateCache.GetOrAdd(
    //           key,
    //           newkey =>
    //           {
    //               return Delegate.CreateDelegate(typeof(SetPropertyValue<T>), obj, typeof(TSource).GetProperty(propertyName).GetSetMethod());
    //           }
    //           );

    //        return result;
    //    }

    //    public static GetPropertyValue<T> CreateGetPropertyValueDelegateNoCache<TSource, T>(TSource obj, string propertyName)
    //    {
    //        return (GetPropertyValue<T>)Delegate.CreateDelegate(typeof(GetPropertyValue<T>), obj, typeof(TSource).GetProperty(propertyName).GetGetMethod()); ;
    //    }
    //    public static SetPropertyValue<T> CreateSetPropertyValueDelegateNoCache<TSource, T>(TSource obj, string propertyName)
    //    {
    //        return (SetPropertyValue<T>)Delegate.CreateDelegate(typeof(SetPropertyValue<T>), obj, typeof(TSource).GetProperty(propertyName).GetSetMethod()); ;
    //    }
    //}

    /// <summary>
    /// 对象转换扩展（可安全的处理PDF.NET实体类之间的转换）
    /// <example>
    /// <code>
    /// <![CDATA[
    ///   A a = new A() {  Name="aaa", NoCopyName="no.no.no."};
    ///   var b = a.CopyTo<B>(filter: new string[] { "NoCopyName" });
    /// ]]>
    /// </code>
    /// </example>
    /// </summary>
    public static class ModelCastExtension
    {
        /// <summary>
        /// 将当前对象的属性值复制到目标对象，使用浅表复制
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="source">源对象</param>
        /// <param name="target">目标对象，如果为空，将生成一个</param>
        /// <param name="filter">要过滤的属性名称数组</param>
        /// <returns>复制过后的目标对象</returns>
        public static T CopyTo<T>(this object source, T target,string[] filter) where T : class,new()
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (target == null)
                target = new T();
            ModelCast.GetCast(source.GetType(), typeof(T)).Cast(source, target, filter);
            return target;
        }

        /// <summary>
        /// 拷贝当前对象的属性值到目标对象上
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="source">当前对象</param>
        /// <param name="target">目标对象</param>
        /// <returns>返回赋值过后的目标对象</returns>
         public static T CopyTo<T>(this object source, T target) where T : class,new()
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (target == null)
                target = new T();
            ModelCast.GetCast(source.GetType(), typeof(T)).Cast(source, target);
            return target;
        }

        /// <summary>
         /// 拷贝当前对象的属性值到目标对象上
        /// </summary>
         /// <typeparam name="T">目标对象类型</typeparam>
         /// <param name="source">当前对象</param>
         /// <returns>返回赋值过后的目标对象</returns>
         public static T CopyTo<T>(this object source) where T : class, new()
         {
             if (source == null)
                 throw new ArgumentNullException("source");

             T target = new T();
             ModelCast.GetCast(source.GetType(), typeof(T)).Cast(source, target);
             return target;
         }
    }
}
