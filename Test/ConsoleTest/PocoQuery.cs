using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

namespace ConsoleTest
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
        object GetValue(object instance);
        void SetValue(object instance, object newValue);
    }

    internal class PropertyAccessor<T, P> : INamedMemberAccessor
    {
        private Func<T, P> GetValueDelegate;
        private Action<T, P> SetValueDelegate;

        public PropertyAccessor(Type type, string propertyName)
        {
            var propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo != null)
            {
                GetValueDelegate = (Func<T, P>)Delegate.CreateDelegate(typeof(Func<T, P>), propertyInfo.GetGetMethod());
                SetValueDelegate = (Action<T, P>)Delegate.CreateDelegate(typeof(Action<T, P>), propertyInfo.GetSetMethod());
            }
        }

        public object GetValue(object instance)
        {
            return GetValueDelegate((T)instance);
        }

        public void SetValue(object instance, object newValue)
        {
            SetValueDelegate((T)instance, (P)newValue);
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
            var key = type.FullName + memberName;
            INamedMemberAccessor accessor;
            accessorCache.TryGetValue(key, out accessor);
            if (accessor == null)
            {
                var propertyInfo = type.GetProperty(memberName);
                if (propertyInfo == null)
                    throw new ArgumentException("实体类中没有属性名为" + memberName + " 的属性！");
                accessor = Activator.CreateInstance(typeof(PropertyAccessor<,>).MakeGenericType(type, propertyInfo.PropertyType), type, memberName) as INamedMemberAccessor;
                accessorCache.Add(key, accessor);
            }

            return accessor;
        }
    }
    
    class PocoQuery
    {
        public List<T> QueryList<T>(IDataReader reader) where T : class,new()
        {
            List<T> list = new List<T>();
            using (reader)
            {

                if (reader.Read())
                {
                    int fcount = reader.FieldCount;
                    INamedMemberAccessor[] accessors = new INamedMemberAccessor[fcount];
                    DelegatedReflectionMemberAccessor drm = new DelegatedReflectionMemberAccessor();
                    for (int i = 0; i < fcount; i++)
                    {
                        accessors[i] = drm.FindAccessor<T>(reader.GetName(i));
                    }
                   
                    do
                    {
                        T t = new T();
                        for (int i = 0; i < fcount; i++)
                        {
                            if(!reader.IsDBNull(i))
                                accessors[i].SetValue(t, reader.GetValue(i));
                        }
                        list.Add(t);
                    } while (reader.Read());
                }
            }
            return list;
        }
    }
}
