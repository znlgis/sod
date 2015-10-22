using System;
using System.Collections.Generic;
using System.Text;
using PWMIS.Common;

namespace PWMIS.Core
{
    /*
      * http://www.cnblogs.com/nankezhishi/archive/2012/02/11/dynamicaccess.html
      */

    /// <summary>
    /// Abstraction of the function of accessing member of a object at runtime.
    /// </summary>
    internal interface IMemberAccessor
    {
        /// <summary>
        /// Get the member value of an object.
        /// </summary>
        /// <param name="instance">The object to get the member value from.</param>
        /// <param name="memberName">The member name, could be the name of a property of field. Must be public member.</param>
        /// <returns>The member value</returns>
        object GetValue(object instance, string memberName);

        /// <summary>
        /// Set the member value of an object.
        /// </summary>
        /// <param name="instance">The object to get the member value from.</param>
        /// <param name="memberName">The member name, could be the name of a property of field. Must be public member.</param>
        /// <param name="newValue">The new value of the property for the object instance.</param>
        void SetValue(object instance, string memberName, object newValue);
    }

    public interface INamedMemberAccessor
    {
        Type MemberType { get; }
        object GetValue(object instance);
        void SetValue(object instance, object newValue);
    }

    internal class PropertyAccessor<T, P> : INamedMemberAccessor
    {
        private MyFunc<T, P> GetValueDelegate;
        private MyAction<T, P> SetValueDelegate;
        private Type memberType;

        public PropertyAccessor(Type type, string propertyName)
        {
            var propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo != null)
            {
                if(propertyInfo.CanRead)
                    GetValueDelegate = (MyFunc<T, P>)Delegate.CreateDelegate(typeof(MyFunc<T, P>), propertyInfo.GetGetMethod());
                if(propertyInfo.CanWrite)
                    SetValueDelegate = (MyAction<T, P>)Delegate.CreateDelegate(typeof(MyAction<T, P>), propertyInfo.GetSetMethod());
            }
            this.memberType = propertyInfo.PropertyType;
        }

        public object GetValue(object instance)
        {
            if (GetValueDelegate != null)
                return GetValueDelegate((T)instance);
            else
                return null;
        }

        public void SetValue(object instance, object newValue)
        {
            if (SetValueDelegate!=null)
                SetValueDelegate((T)instance, (P)newValue);
        }



        public Type MemberType
        {
            get { return this.memberType; }
        }
    }

    public class DelegatedReflectionMemberAccessor : IMemberAccessor
    {
        private static Dictionary<string, INamedMemberAccessor> accessorCache = new Dictionary<string, INamedMemberAccessor>();

        public object GetValue(object instance, string memberName)
        {
            return FindAccessor(instance, memberName).GetValue(instance);
        }

        public void SetValue(object instance, string memberName, object newValue)
        {
            FindAccessor(instance, memberName).SetValue(instance, newValue);
        }

        public INamedMemberAccessor FindAccessor<T>(string memberName) where T : class
        {
            var type = typeof(T);
            return FindAccessor(type, memberName);
        }

        private INamedMemberAccessor FindAccessor(object instance, string memberName)
        {
            var type = instance.GetType();
            return FindAccessor(type, memberName);
        }

        private INamedMemberAccessor FindAccessor(Type type, string memberName)
        {
            return FindAccessor(type, memberName, true);
        }

        /// <summary>
        /// 在指定的类型中寻找指定属性名称的属性访问器，如果找不到返回空。
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="memberName">属性名称</param>
        /// <returns>属性访问起 </returns>
        public INamedMemberAccessor TryFindAccessor(Type type, string memberName)
        {
            return FindAccessor(type, memberName, false);
        }

        private INamedMemberAccessor FindAccessor(Type type, string memberName,bool throwError)
        {
            var key = type.FullName + memberName;
            INamedMemberAccessor accessor;
            accessorCache.TryGetValue(key, out accessor);
            if (accessor == null)
            {
                var propertyInfo = type.GetProperty(memberName);
                if (propertyInfo == null)
                {
                    if (throwError)
                        throw new ArgumentException("实体类中没有属性名为" + memberName + " 的属性！");
                    else
                        return null;
                }
                accessor = Activator.CreateInstance(typeof(PropertyAccessor<,>).MakeGenericType(type, propertyInfo.PropertyType), type, memberName) as INamedMemberAccessor;
                accessorCache.Add(key, accessor);
            }

            return accessor;
        }
    }

}
