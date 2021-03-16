using PWMIS.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PWMIS.DataMap.Entity
{
    /// <summary>
    /// 实体元数据
    /// </summary>
    public record EntityMetaData
    {
        private static Dictionary<string, EntityMetaData> metaCache = new Dictionary<string, EntityMetaData>();
        private static object lock_obj = new object();

        public EntityMetaData()
        { }
        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 表所从属的架构名，注意有些数据库不支持
        /// </summary>
        public string Schema { get; set; } 
        /// <summary>
        /// 数据源，例如连接字符串或者数据连接配置名或其它有用的信息
        /// </summary>
        public string DataSource { get; set; }

        public EntityMapType EntityMap { get; set; } = EntityMapType.Table;
        /// <summary>
        /// 表的标识字段名称
        /// </summary>
        public string IdentityName { get; set; }
        /// <summary>
        /// 是否是共享的元数据
        /// </summary>
        public bool Sharing { get; protected  internal set; }

        /* 对PrimaryKeys的任何操作都会产生一个新的PrimaryKeys */

        /// <summary>
        /// 实体类的主键名称集合
        /// </summary>
        public NotifyingArrayList<string> PrimaryKeys { get; protected internal set; } 
        /// <summary>
        /// 添加一个主键名字。本方法线程安全。
        /// </summary>
        /// <param name="keyName"></param>
        public void AddPrimaryKey(string keyName)
        {
            lock (lock_obj)
            {
                if (this.PrimaryKeys == null)
                {
                    PrimaryKeys = new NotifyingArrayList<string>(keyName);
                }
                else
                {
                    var keys = PrimaryKeys.Add(keyName);
                    PrimaryKeys = new NotifyingArrayList<string>(keys);
                }
            }
        }
        /// <summary>
        /// 创建一个元数据的共享实例。如果缓存中没有当前实体类的元数据对象，则创建一个。
        /// </summary>
        /// <typeparam name="T">当前的实体类类型</typeparam>
        /// <param name="meta"></param>
        /// <returns></returns>
        public static EntityMetaData SharedMeta<T>(Action<EntityMetaData> init_meta) where T : EntityBase
        {
            string entity_key = typeof(T).FullName;
            if (metaCache.TryGetValue(entity_key, out EntityMetaData value))
            {
                return value;
            }
            else
            {
                lock (lock_obj)
                {
                    EntityMetaData meta = new EntityMetaData() { PrimaryKeys = new NotifyingArrayList<string>() };
                    init_meta(meta);
                    meta.Sharing = true;
                    metaCache.Add(entity_key, meta);
                    return meta;
                }
            }
        }
    }

   



    /// <summary>
    /// 改变集合元素的操作方法无法修改当前集合对象，但是会返回一个新的集合对象，从而使得当前集合对象看起来是不可变的，只读的。注意线程安全。
    /// </summary>
    public class NotifyingArrayList<T> : IEnumerable<T>, IEnumerator<T> 
    {
        private T[] _arr;
        int position = -1;

        public NotifyingArrayList()
        {
            _arr = new T[0];
        }

        public NotifyingArrayList(T item)
        {
            _arr = new T[] { item};
        }

        public NotifyingArrayList(IEnumerable<T> data)
        {
            int count = data.Count();
            T[] temp = new T[count];
            int i = 0;
            foreach (T item in data)
                temp[i++] = item;
            this._arr = temp;
        }

        /// <summary>
        /// 通知有更改集合元数行为的操作
        /// </summary>
        public Action<NotifyingArrayList<T>> Changed;
        /// <summary>
        /// 向数组添加一个元素到数组末尾
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public NotifyingArrayList<T> Add(T item)
        {
            if (_arr == null)
            {
                return new NotifyingArrayList<T>(item);
            }
            T[] temp = new T[_arr.Length+1];
            Array.Copy(_arr, temp,_arr.Length);
            temp[temp.Length - 1] = item;
            return new NotifyingArrayList<T>(temp);
        }

        /// <summary>
        /// 创建一个没有任何成员的新数组对象
        /// </summary>
        /// <returns></returns>
        public NotifyingArrayList<T> Clear()
        {
            return new NotifyingArrayList<T>();
        }


        /// <summary>
        /// 从数组删除一个元素，如果元素为空或者元素不存在于数组中，将返回原数组
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public NotifyingArrayList<T> Remove(T item)
        {
            if (item == null) return this;
            if (!this.Contains(item)) return this;
            T[] temp = new T[_arr.Length - 1];
            int j = 0;
            for (int i = 0; i < _arr.Length; i++)
            {
                if (!object.Equals(_arr[i], item))
                    temp[j++] = _arr[i];
            }
            return new NotifyingArrayList<T>(temp);
        }

        public int Count => _arr.Length;

        public bool Contains(T item)
        {
            for (int i = 0; i < _arr.Length; i++)
            {
                if (object.Equals(_arr[i], item))
                    return true;
            }
            return false;
        }

        #region 接口方法
        public T Current
        {
            get
            {
                try
                {
                    return _arr[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        object IEnumerator.Current
        {
            get
            {
                try
                {
                    return _arr[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

       

        public IEnumerator<T> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            position++;
            return (position < _arr.Length);
        }

        public void Reset()
        {
            position = -1;
        }

        public void Dispose()
        {
           
        }
        #endregion
    }
}

//解决多目标框架编译 不支持 init 语法问题
//https://www.5axxw.com/questions/content/hfi1bm

namespace System.Runtime.CompilerServices
{
    internal class IsExternalInit { }
}

