﻿#define LockFree
//#undef LockFree

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace PWMIS.Core
{
    /// <summary>
    ///     最热访问的名-值 对管理对象，节点被访问的次数越多，越容易被访问到的数据结构
    /// </summary>
    public class HotNameValue<T> : IDictionary<string, T>
    {
        private const int maxCount = 0xffff;
        private readonly List<DataItem> extList = new List<DataItem>();
        //SpinLock sl = new SpinLock();
        private readonly Queue<int> freeQueue = new Queue<int>(); //空闲位置队列
        private readonly int hotCount = 3;
        private readonly object sync_obj = new object();
        private readonly MakeWordKey wk = new MakeWordKey();
        private int _count; //元素的有效数量
        //
        private DataItem[] dataArr = new DataItem[maxCount];
        private int[] hotData;

        /// <summary>
        ///     默认构造函数
        /// </summary>
        public HotNameValue()
        {
            At = 0;
            Init();
        }

        /// <summary>
        ///     初始化热点数据大小，不能大于64，建议在10以内，默认是3
        /// </summary>
        /// <param name="hot"></param>
        public HotNameValue(int hot)
        {
            At = 0;
            if (hot < 64 && hot > 0)
                hotCount = hot;
            Init();
        }

        /// <summary>
        ///     发现目标的位置
        /// </summary>
        public int At { get; private set; }

        /// <summary>
        ///     (键)碰撞的数量
        /// </summary>
        public int CollidedCount
        {
            get { return extList.Count; }
        }

        /// <summary>
        ///     容量大小，默认是64K，不可设置比这个数更大的值。容器超过这个容量，则容量外的很少使用的元素将被移除。
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        ///     容器已经存储的元素总数量
        /// </summary>
        public int Count
        {
            get { return _count; }
        }

        #region IEnumerable<KeyValuePair<string,T>> 成员

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            for (var i = 0; i < maxCount; i++)
            {
                if (dataArr[i].Key != null)
                    yield return new KeyValuePair<string, T>(dataArr[i].Key, dataArr[i].Value);
            }
            for (var i = 0; i < extList.Count; i++)
            {
                if (extList[i].Key != null)
                    yield return new KeyValuePair<string, T>(extList[i].Key, extList[i].Value);
            }
        }

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (var i = 0; i < maxCount; i++)
            {
                if (dataArr[i].Key != null)
                    yield return dataArr[i].Value;
            }
            for (var i = 0; i < extList.Count; i++)
            {
                if (extList[i].Key != null)
                    yield return extList[i].Value;
            }
        }

        #endregion

        private void Init()
        {
            Capacity = maxCount;
            extList.Add(new DataItem());
            hotData = new int[hotCount];
            for (var i = 0; i < hotData.Length; i++)
                hotData[i] = -1;
        }

        /// <summary>
        ///     获取扩展列表中的下一个空闲位置写入新的数据
        /// </summary>
        /// <returns></returns>
        private int SetDataOnFreeIndex(ref DataItem data)
        {
            lock (sync_obj)
            {
                var index = 0;
                if (freeQueue.Count > 0)
                {
                    index = freeQueue.Dequeue();
                    extList[index] = data;
                }
                else
                {
                    index = extList.Count;
                    extList.Add(data);
                }
                _count++;
                return index;
            }
        }

        private bool GetFromHot(string key, out T result)
        {
            for (var i = 0; i < hotCount; i++)
            {
                var p = hotData[i];
                if (p == -1)
                    continue;
                if (dataArr[p].Key == key) //未碰撞
                {
                    At = p;
                    dataArr[p].AddReadCount();
                    result = dataArr[p].Value;
                    return true;
                }
            }
            result = default(T);
            return false;
        }

        private void UpdateHot(int objP)
        {
            var readCount = dataArr[objP].ReadCount;

            for (var i = 0; i < hotCount; i++)
            {
                var p = hotData[i];
                if (p == -1)
                {
                    hotData[i] = objP;
                    break;
                }
                if (dataArr[p].ReadCount < readCount)
                {
                    hotData[i] = objP;
                    break;
                }
            }
        }

        /// <summary>
        ///     查找指定的键的值，如果找不到将抛出异常
        /// </summary>
        /// <param name="key">指定的键</param>
        /// <returns>指定的键的值</returns>
        public T Get(string key)
        {
            At = -1;
            T result;
            if (GetFromHot(key, out result))
                return result;

            var p = wk.String2Int(key);
            if (dataArr[p].Key == key) //未碰撞
            {
                At = p;
                dataArr[p].AddReadCount();
                //更新热点
                UpdateHot(p);
                return dataArr[p].Value;
            }
            if (dataArr[p].Key == null)
                throw new ArgumentException("指定的键[" + key + "]未找到！（内部错误位置：1）");

            var index = dataArr[p].ExtNext;
            //碰撞，根据完整的名字在链表中找到目标
            while (index > 0)
            {
                if (extList[index].Key == key)
                {
                    At = p;
                    var objData = extList[index];
                    objData.AddReadCount();

                    extList[index] = objData;
                    return objData.Value;
                }
                index = extList[index].ExtNext;
            }
            if (index == 0)
                throw new ArgumentException("指定的键[" + key + "]未找到！（内部错误位置：2）");
            throw new ArgumentException("指定的键[" + key + "]未找到！（内部错误位置：3）");
        }

        /// <summary>
        ///     查找指定的键的值，但是找不到不抛出异常
        /// </summary>
        /// <param name="key">指定的键</param>
        /// <param name="result">指定的键的值</param>
        /// <returns>查找是否成功</returns>
        public bool TryGet(string key, out T result)
        {
            At = -1;
            result = default(T);

            if (GetFromHot(key, out result))
                return true;

            var p = wk.String2Int(key);
            if (dataArr[p].Key == null)
                return false;
            if (dataArr[p].Key == key) //未碰撞
            {
                At = p;
                dataArr[p].AddReadCount();
                result = dataArr[p].Value;
                //更新热点
                UpdateHot(p);
                return true;
            }
            var index = dataArr[p].ExtNext;
            //碰撞，根据完整的名字在链表中找到目标
            //extList 不会收缩，可以确保 extList[index] 不会为空，线程安全
            while (index > 0)
            {
                if (extList[index].Key == key)
                {
                    At = p;
                    var objData = extList[index];
                    objData.AddReadCount();
                    result = objData.Value;

                    extList[index] = objData;
                    return true;
                }
                index = extList[index].ExtNext;
            }
            return false;
        }

        /// <summary>
        ///     设置指定键的值，如果存在，则替换
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, T value)
        {
            var p = wk.String2Int(key);
            //检查是否有碰撞
            //下面的判断类似于 下面的功能，但是线程安全
            //if (dataArr[p].Key == null)
            //{
            //    dataArr[p].Key = key;
            //    dataArr[p].Value = value;
            //}
            if (Interlocked.CompareExchange(ref dataArr[p].Key, key, null) == null)
            {
                dataArr[p].Value = value;
                Interlocked.Increment(ref _count);
            }
            else
            {
                if (dataArr[p].Key == key) //未碰撞，此时应该更新值
                {
                    dataArr[p].Value = value;
                }
                else
                {
#if(LockFree)
                    //发生碰撞，更新节点指针 ExtNext
                    var data = new DataItem();
                    data.Key = key;
                    data.Value = value;
                    var index = SetDataOnFreeIndex(ref data);

                    if (Interlocked.CompareExchange(ref dataArr[p].ExtNext, index, 0) > 0)
                    {
                        //碰撞的数大于2个
                        lock (sync_obj)
                        {
                            var extIndex = dataArr[p].ExtNext;
                            while (extList[extIndex].ExtNext > 0)
                            {
                                extIndex = extList[extIndex].ExtNext;
                            }
                            //注意：
                            //List<T> 中的结构体，直接修改或者调用方法，无效，但是数组可以
                            //所以需要重新整体赋值
                            var objData = extList[extIndex];
                            objData.ExtNext = index;

                            extList[extIndex] = objData;
                        }
                    }

#else
                    lock (sync_obj)
                    {

                        //发生碰撞，更新节点指针 ExtNext
                        DataItem data = new DataItem();
                        data.Key = key;
                        data.Value = value;
                        extList.Add(data);
                        int index = extList.Count - 1;

                        if (dataArr[p].ExtNext > 0)
                        {
                            int extIndex = dataArr[p].ExtNext;
                            while (extList[extIndex].ExtNext > 0)
                            {
                                extIndex = extList[extIndex].ExtNext;
                            }
                            DataItem objData = extList[extIndex];
                            objData.ExtNext = index;
                            extList[extIndex] = objData;
                        }
                        else
                        {
                            dataArr[p].ExtNext = index;
                        }

                        _count++;                   
                    }
#endif
                }
            }
        }

        //see http://www.cnblogs.com/yangecnu/p/Something-about-Concurrent-and-Parallel-Programming-PartII.html

        //public static void DoWithCAS<T>(ref T location, MyFunc<T, T> generator) where T : class
        //{
        //    T temp, replace;
        //    do
        //    {
        //        temp = location;
        //        replace = generator(temp);
        //    } while (Interlocked.CompareExchange(ref location, replace, temp) != temp);
        //}


        private struct DataItem
        {
            private T _value;

            /// <summary>
            ///     如果Key发生碰撞，那么新的节点的位置
            /// </summary>
            public int ExtNext;

            /// <summary>
            ///     原始的Key,可能跟散列的Key不一致，发生Key碰撞
            /// </summary>
            public string Key;

            public int ReadCount;

            /// <summary>
            ///     获取或者设置值，线程安全
            /// </summary>
            public T Value
            {
                get { return _value; }
                set
                {
                    _value = value;
                    Thread.MemoryBarrier();
                    //调用内存栅栏，使得后面的读线程能够立即读取到最新的值
                }
            }

            public void AddReadCount()
            {
                //ReadCount++;
                Interlocked.Increment(ref ReadCount);
            }
        }

        #region IDictionary<string,T> 成员

        public void Add(string key, T value)
        {
            Set(key, value);
        }

        public bool ContainsKey(string key)
        {
            T data;
            return TryGet(key, out data);
        }

        public ICollection<string> Keys
        {
            get
            {
                var allCount = _count + extList.Count;
                var result = new string[allCount];
                var index = allCount - 1;
                for (var i = 0; i < maxCount; i++)
                {
                    if (dataArr[i].Key != null)
                        result[index--] = dataArr[i].Key;
                }
                for (var i = 0; i < extList.Count; i++)
                {
                    if (extList[i].Key != null)
                        result[index--] = extList[i].Key;
                }
                var temp = new string[allCount - index];
                if (index == 0)
                    return temp;
                result.CopyTo(temp, index);
                return temp;
            }
        }

        public bool Remove(string key)
        {
            var p = wk.String2Int(key);
            //检查是否有碰撞
            if (dataArr[p].Key == null) //没有对应的数据
            {
                return false;
            }
            if (dataArr[p].Key == key) //未碰撞，此时应该更新值
            {
                dataArr[p].Value = default(T);
                dataArr[p].ReadCount = 0;
                //ExtNext 不可更新
                return true;
            }
            //发生碰撞，更新节点指针 ExtNext
            lock (sync_obj)
            {
                var index = dataArr[p].ExtNext;
                if (index > 0)
                {
                    var keyIndexList = new List<int>();
                    var objIndex = 0;
                    while (index > 0)
                    {
                        keyIndexList.Add(index);
                        if (extList[index].Key == key)
                        {
                            objIndex = index;
                            break;
                        }
                        index = extList[index].ExtNext;
                    }

                    if (objIndex > 0)
                    {
                        if (keyIndexList.Count >= 1)
                        {
                            var nextIndex = extList[objIndex].ExtNext;
                            if (nextIndex > 0 || keyIndexList.Count > 1)
                            {
                                if (keyIndexList.Count > 1)
                                {
                                    var preIndex = keyIndexList[keyIndexList.Count - 2];
                                    var preData = extList[preIndex];
                                    preData.ExtNext = nextIndex;
                                    extList[preIndex] = preData;
                                }
                                else
                                {
                                    //修正入口位置
                                    dataArr[p].ExtNext = nextIndex;
                                }
                            }
                            else
                            {
                                //当前节点没有下一个节点，将入口位置置为0
                                dataArr[p].ExtNext = 0;
                            }
                            //如果没有下一节点，直接将当前节点移除
                            extList[objIndex] = new DataItem();
                            freeQueue.Enqueue(objIndex);
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        public bool TryGetValue(string key, out T value)
        {
            return TryGet(key, out value);
        }

        public ICollection<T> Values
        {
            get
            {
                var result = new List<T>();
                for (var i = 0; i < maxCount; i++)
                {
                    if (dataArr[i].Key != null)
                        result.Add(dataArr[i].Value);
                }
                for (var i = 0; i < extList.Count; i++)
                {
                    if (extList[i].Key != null)
                        result.Add(extList[i].Value);
                }
                return result;
            }
        }

        public T this[string key]
        {
            get { return Get(key); }
            set { Set(key, value); }
        }

        #endregion

        #region ICollection<KeyValuePair<string,T>> 成员

        public void Add(KeyValuePair<string, T> item)
        {
            Set(item.Key, item.Value);
        }

        public void Clear()
        {
            dataArr = new DataItem[maxCount];
            for (var i = 0; i < extList.Count; i++)
            {
                var data = extList[i];
                data.Value = default(T);
                extList[i] = data;
            }
        }

        public bool Contains(KeyValuePair<string, T> item)
        {
            var data = Get(item.Key);
            return data.Equals(item.Value);
        }

        public void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
        {
            var index = 0;
            if (arrayIndex < 0) arrayIndex = 0;
            for (var i = 0; i < maxCount; i++)
            {
                if (dataArr[i].Key != null && index >= arrayIndex)
                {
                    var item = new KeyValuePair<string, T>(dataArr[i].Key, dataArr[i].Value);
                    array[index++] = item;
                }
            }
            for (var i = 0; i < extList.Count; i++)
            {
                if (extList[i].Key != null && index >= arrayIndex)
                {
                    var item = new KeyValuePair<string, T>(extList[i].Key, extList[i].Value);
                    array[index++] = item;
                }
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<string, T> item)
        {
            return Remove(item.Key);
        }

        #endregion
    }

    /// <summary>
    ///     以一个输入的字符串，从右向左扫描，得到一个16进制数字的字符串。不区分字符的大小写。
    /// </summary>
    public class MakeWordKey
    {
        private const int Zero = '0';
        private const int AL = 'a';
        private const int AU = 'A';
        private const int KL = 'k';
        private const int KU = 'K';
        private const int QL = 'q';
        private const int QU = 'Q';
        private string _hexString;
        private string inputWord = string.Empty;

        public MakeWordKey()
        {
        }

        public MakeWordKey(string input)
        {
            inputWord = input;
        }

        /// <summary>
        ///     获取当前的16进制字符串
        /// </summary>
        public string HexString
        {
            get
            {
                if (_hexString == null)
                {
                    _hexString = MakeHexString();
                }

                return _hexString;
            }
        }

        private char GetMyHexChar(char oldChar, char newChar)
        {
            var oi = Hex2Int(oldChar.ToString());
            var ni = Hex2Int(newChar.ToString());
            var avg = (oi + ni)/2;
            var str = avg.ToString("x");
            return str[0];
        }

        private void SetMyHexChar(char[] charArr, ref int charIndex, char newChar)
        {
            var temp = '0';
            if (charArr[charIndex] == '0')
                temp = newChar;
            else
                temp = GetMyHexChar(charArr[charIndex], newChar);

            charArr[charIndex--] = temp;
        }

        private string MakeHexString()
        {
            char[] charArr = {'0', '0', '0', '0'};
            var charIndex = 3;

            for (var i = inputWord.Length - 1; i >= 0; i--)
            {
                var c = inputWord[i];
                if (char.IsDigit(c))
                {
                    SetMyHexChar(charArr, ref charIndex, c);
                }
                else if (char.IsLower(c))
                {
                    if (c < 'k')
                        SetMyHexChar(charArr, ref charIndex, (char) (c - AL + Zero));
                    else if (c < 'q')
                        SetMyHexChar(charArr, ref charIndex, (char) (c - 10));
                }
                else if (char.IsUpper(c))
                {
                    if (c < 'K')
                        SetMyHexChar(charArr, ref charIndex, (char) (c - AU + Zero));
                    else if (c < 'Q')
                        SetMyHexChar(charArr, ref charIndex, (char) (c - 10));
                }

                if (charIndex < 0)
                    charIndex = 3;
            }

            return new string(charArr);
        }

        /// <summary>
        ///     构造一个小于0xFFFF 的16进制字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string MakeHexString(string input)
        {
            inputWord = input;
            _hexString = null;
            return HexString;
        }

        /// <summary>
        ///     将1个16进制字符串转换成一个整数
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public int Hex2Int(string hexString)
        {
            return int.Parse(hexString, NumberStyles.HexNumber);
        }

        /// <summary>
        ///     将任意一个字符串转换成65536 以内的整数,方法自身线程安全
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public int String2Int(string input)
        {
            inputWord = input;
            _hexString = null;

            var iArr = new int[4];
            var index = 3;

            for (var i = input.Length - 1; i >= 0; i--)
            {
                var c = input[i];
                int ic = c;
                int temp;
                //如果当前位置不是0，则计算平均数
                if (char.IsDigit(c)) //得到字符型数字对应的数字
                {
                    temp = iArr[index] == 0 ? (ic - 48) : (iArr[index] + (ic - 48)) >> 2;
                    iArr[index--] = temp;
                }
                else if (char.IsLower(c) && ic < QL) //计算小写字母，得到0xF 以内的数
                {
                    temp = iArr[index] == 0 ? (ic - AL) : (iArr[index] + (ic - AL)) >> 2;
                    iArr[index--] = temp;
                }
                else if (char.IsUpper(c) && ic < QU) //计算大写写字母，得到0xF 以内的数
                {
                    temp = iArr[index] == 0 ? (ic - AU) : (iArr[index] + (ic - AU)) >> 2;
                    ;
                    iArr[index--] = temp;
                }

                if (index < 0)
                    index = 3;
            }
            //位移操作，比如 0xf<<4 == 0xf0
            if (iArr[0] > 0) iArr[0] <<= 12;
            if (iArr[1] > 0) iArr[1] <<= 8;
            if (iArr[2] > 0) iArr[2] <<= 4;
            return iArr[0] + iArr[1] + iArr[2] + iArr[3];
        }
    }

    /// <summary>
    /// </summary>
    public class SpinLock
    {
        private volatile int locked;

        /// <summary>
        ///     获得自旋锁
        /// </summary>
        public void Acquire()
        {
            while (Interlocked.CompareExchange(ref locked, 1, 0) != 0) ;
            //第一次执行是locked默认为0，为false，推出循环，locked赋值为1
            //第二个线程获取时lock为1，循环一直进行。
            //知道第一个线程release之后，locked为0.
        }

        /// <summary>
        ///     释放自旋锁
        /// </summary>
        public void Release()
        {
            locked = 0;
        }
    }
}