using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PWMIS.Common;

namespace PWMIS.DataMap.Entity
{
    /// <summary>
    ///     实体元数据
    /// </summary>
    public record EntityMetaData
    {
        private static readonly Dictionary<string, EntityMetaData> metaCache = new();
        private static readonly object lock_obj = new();

        /// <summary>
        ///     表名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        ///     表所从属的架构名，注意有些数据库不支持
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        ///     数据源，例如连接字符串或者数据连接配置名或其它有用的信息
        /// </summary>
        public string DataSource { get; set; }

        public EntityMapType EntityMap { get; set; } = EntityMapType.Table;

        /// <summary>
        ///     表的标识字段名称
        /// </summary>
        public string IdentityName { get; set; }

        /// <summary>
        ///     是否是共享的元数据
        /// </summary>
        public bool Sharing { get; protected internal set; }


        /// <summary>
        ///     创建一个元数据的共享实例。如果缓存中没有当前实体类的元数据对象，则创建一个。
        /// </summary>
        /// <typeparam name="T">当前的实体类类型</typeparam>
        /// <param name="meta"></param>
        /// <returns></returns>
        public static EntityMetaData SharedMeta<T>(Action<EntityMetaData> init_meta) where T : EntityBase
        {
            var entity_key = typeof(T).FullName;
            if (metaCache.TryGetValue(entity_key, out var value))
                return value;
            lock (lock_obj)
            {
                var meta = new EntityMetaData();
                init_meta(meta);
                meta.Sharing = true;
                metaCache.Add(entity_key, meta);
                return meta;
            }
        }

        public static EntityMetaData GetSharedMeta(string entity_key)
        {
            if (metaCache.TryGetValue(entity_key, out var value))
                return value;
            lock (lock_obj)
            {
                var meta = new EntityMetaData();

                meta.Sharing = true;
                metaCache.Add(entity_key, meta);
                return meta;
            }
        }
    }


    /// <summary>
    ///     改变集合元素的操作方法无法修改当前集合对象，但是会返回一个新的集合对象，从而使得当前集合对象看起来是不可变的，只读的。注意线程安全。
    /// </summary>
    public class NotifyingArrayList<T> : IEnumerable<T>
    {
        private T[] _arr;


        /// <summary>
        ///     通知有更改集合元数行为的操作
        /// </summary>
        public Action<NotifyingArrayList<T>> Changed;

        public NotifyingArrayList()
        {
            _arr = new T[0];
        }

        public NotifyingArrayList(T item)
        {
            CreateNewData(item);
        }

        public NotifyingArrayList(IEnumerable<T> data)
        {
            CreateNewData(data);
        }

        public int Count => _arr.Length;

        private void CreateNewData(T item)
        {
            _arr = new[] { item };
        }

        private void CreateNewData(IEnumerable<T> data)
        {
            var count = data.Count();
            var temp = new T[count];
            var i = 0;
            foreach (var item in data)
                temp[i++] = item;
            _arr = temp;
        }

        /// <summary>
        ///     向集合添加一个不重复的元素到末尾并返回新的集合，原来的集合不变
        /// </summary>
        /// <param name="item"></param>
        /// <returns>当集合元素多余一个且操作成功，触发Changed方法</returns>
        public NotifyingArrayList<T> Add(T item)
        {
            if (_arr == null || _arr.Length == 0)
            {
                CreateNewData(item);
            }
            else
            {
                if (!Contains(item))
                {
                    var temp = new T[_arr.Length + 1];
                    Array.Copy(_arr, temp, _arr.Length);
                    temp[temp.Length - 1] = item;

                    var newObj = new NotifyingArrayList<T>(temp);
                    if (Changed != null)
                        Changed(newObj);
                    return newObj;
                }
            }

            return this;
        }

        /// <summary>
        ///     返回新的集合，原来的集合不变
        /// </summary>
        /// <returns>如果操作成功，触发Changed方法</returns>
        public NotifyingArrayList<T> Clear()
        {
            var newObj = new NotifyingArrayList<T>();
            if (Changed != null)
                Changed(newObj);
            return newObj;
        }


        /// <summary>
        ///     从数组删除一个元素，如果元素为空或者元素不存在于数组中，将返回原数组
        /// </summary>
        /// <param name="item"></param>
        /// <returns>如果操作成功，触发Changed方法</returns>
        public NotifyingArrayList<T> Remove(T item)
        {
            if (!Contains(item))
                return this;
            var temp = new T[_arr.Length - 1];
            var j = 0;
            for (var i = 0; i < _arr.Length; i++)
                if (!Equals(_arr[i], item))
                    temp[j++] = _arr[i];
            var newObj = new NotifyingArrayList<T>(temp);
            if (Changed != null)
                Changed(newObj);
            return newObj;
        }

        public bool Contains(T item)
        {
            for (var i = 0; i < _arr.Length; i++)
                if (Equals(_arr[i], item))
                    return true;
            return false;
        }

        #region 接口方法

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < _arr.Length; i++)
                yield return _arr[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }

    /// <summary>
    ///     共享字符串列表类型。向该类型的首个实例对象添加数据将创建共享的数据，当其它实例对象添加不同的数据的时候会添加到自己的实例上。
    ///     不同的使用类型将使用不同的共享数据。
    /// </summary>
    /// <typeparam name="T">使用该类的类型</typeparam>
    public class SharedStringList<T> : IEnumerable<string> where T : class, new()
    {
        private static readonly Dictionary<Type, string[]> SharedData = new();
        private static readonly object SharedDataLock = new();
        private string[] _arr;

        public SharedStringList()
        {
            //_arr = new string[0];
        }

        public SharedStringList(string item)
        {
            CreateNewData(item);
        }

        public SharedStringList(IEnumerable<string> data)
        {
            CreateNewData(data);
        }

        private void CreateNewData(string item)
        {
            _arr = new[] { item };
        }

        private void CreateNewData(IEnumerable<string> data)
        {
            var count = data.Count();
            var temp = new string[count];
            var i = 0;
            foreach (var item in data)
                temp[i++] = item;
            _arr = temp;
        }

        /// <summary>
        ///     向集合添加一个不重复的元素到末尾。
        ///     如果还没有共享的数据，此时添加的数据会被其它实例共享，否则只会添加到当前实例对象上。线程安全。
        /// </summary>
        /// <param name="item"></param>
        /// <returns>返回是否成功添加</returns>
        public bool Add(string item)
        {
            if (_arr == null)
            {
                var type = typeof(T);
                if (SharedData.TryGetValue(type, out _arr))
                {
                    if (_arr.Contains(item)) return false;

                    _arr = new string[1] { item };
                    return true;
                }

                //如果还没有共享数据
                lock (SharedDataLock)
                {
                    //再次判断，确保线程安全
                    if (SharedData.TryGetValue(type, out _arr))
                    {
                        if (_arr.Contains(item)) return false;

                        _arr = new string[1] { item };
                        return true;
                    }

                    _arr = new string[1] { item };
                    SharedData[type] = _arr;
                    return true;
                }
            }

            //此操作将添加到当前对象的实例数据而不会添加到共享数据
            lock (SharedDataLock)
            {
                if (!Contains(item))
                {
                    var temp = new string[_arr.Length + 1];
                    Array.Copy(_arr, temp, _arr.Length);
                    temp[temp.Length - 1] = item;
                    _arr = temp;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     清除当前数据并使用新的实例数据
        /// </summary>
        public void Clear()
        {
            _arr = new string[0];
        }


        /// <summary>
        ///     从集合删除一个元素，如果操作成功，当前对象将使用新的实例数据。线程安全。
        /// </summary>
        /// <param name="item"></param>
        /// <returns>是否操作成功</returns>
        public bool Remove(string item)
        {
            if (!Contains(item))
                return false;
            lock (SharedDataLock)
            {
                var temp = new string[_arr.Length - 1];
                var j = 0;
                for (var i = 0; i < _arr.Length; i++)
                    if (!Equals(_arr[i], item))
                        temp[j++] = _arr[i];
                _arr = temp;
                return true;
            }
        }

        public int Count()
        {
            if (_arr == null)
                return 0;
            return _arr.Length;
        }

        public bool Contains(string item)
        {
            if (_arr == null) return false;
            for (var i = 0; i < _arr.Length; i++)
                if (Equals(_arr[i], item))
                    return true;
            return false;
        }

        #region 接口方法

        public IEnumerator<string> GetEnumerator()
        {
            if (_arr == null)
            {
                var type = typeof(T);
                if (!SharedData.TryGetValue(type, out _arr)) yield return null;
            }

            for (var i = 0; i < _arr.Length; i++)
                yield return _arr[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }

    public class SharedStringList : IEnumerable<string>
    {
        private static readonly Dictionary<Type, string[]> SharedData = new();
        private static readonly object SharedDataLock = new();
        private readonly Type _sharedType;
        private string[] _arr;

        public SharedStringList(Type sharedType)
        {
            _sharedType = sharedType;
            //_arr = new string[0];
        }

        public SharedStringList(Type sharedType, string item)
        {
            _sharedType = sharedType;
            CreateNewData(item);
        }

        public SharedStringList(Type sharedType, IEnumerable<string> data)
        {
            _sharedType = sharedType;
            CreateNewData(data);
        }

        public int Count
        {
            get
            {
                if (_arr == null)
                    return 0;
                return _arr.Length;
            }
        }

        private void CreateNewData(string item)
        {
            _arr = new[] { item };
        }

        private void CreateNewData(IEnumerable<string> data)
        {
            var count = data.Count();
            var temp = new string[count];
            var i = 0;
            foreach (var item in data)
                temp[i++] = item;
            _arr = temp;
        }

        /// <summary>
        ///     向集合添加一个不重复的元素到末尾。
        ///     如果还没有共享的数据，此时添加的数据会被其它实例共享，否则只会添加到当前实例对象上。线程安全。
        /// </summary>
        /// <param name="item"></param>
        /// <returns>返回是否成功添加</returns>
        public bool Add(string item)
        {
            if (_arr == null)
            {
                if (SharedData.TryGetValue(_sharedType, out _arr))
                {
                    if (_arr.Contains(item)) return false;

                    _arr = new string[1] { item };
                    return true;
                }

                //如果还没有共享数据
                lock (SharedDataLock)
                {
                    //再次判断，确保线程安全
                    if (SharedData.TryGetValue(_sharedType, out _arr))
                    {
                        if (_arr.Contains(item)) return false;

                        _arr = new string[1] { item };
                        return true;
                    }

                    _arr = new string[1] { item };
                    SharedData[_sharedType] = _arr;
                    return true;
                }
            }

            //此操作将添加到当前对象的实例数据而不会添加到共享数据
            lock (SharedDataLock)
            {
                if (!Contains(item))
                {
                    var temp = new string[_arr.Length + 1];
                    Array.Copy(_arr, temp, _arr.Length);
                    temp[temp.Length - 1] = item;
                    _arr = temp;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     清除当前数据并使用新的实例数据
        /// </summary>
        public void Clear()
        {
            _arr = new string[0];
        }


        /// <summary>
        ///     从集合删除一个元素，如果操作成功，当前对象将使用新的实例数据。线程安全。
        /// </summary>
        /// <param name="item"></param>
        /// <returns>是否操作成功</returns>
        public bool Remove(string item)
        {
            if (!Contains(item))
                return false;
            lock (SharedDataLock)
            {
                var temp = new string[_arr.Length - 1];
                var j = 0;
                for (var i = 0; i < _arr.Length; i++)
                    if (!Equals(_arr[i], item))
                        temp[j++] = _arr[i];
                _arr = temp;
                return true;
            }
        }

        public bool Contains(string item)
        {
            if (_arr == null) return false;
            for (var i = 0; i < _arr.Length; i++)
                if (Equals(_arr[i], item))
                    return true;
            return false;
        }

        #region 接口方法

        public IEnumerator<string> GetEnumerator()
        {
            if (_arr == null)
                if (!SharedData.TryGetValue(_sharedType, out _arr))
                    yield return null;
            for (var i = 0; i < _arr.Length; i++)
                yield return _arr[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}

//解决多目标框架编译 不支持 init 语法问题
//https://www.5axxw.com/questions/content/hfi1bm

namespace System.Runtime.CompilerServices
{
    internal class IsExternalInit
    {
    }
}