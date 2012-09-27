using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using PWMIS.Common;

namespace PWMIS.DataMap.Entity
{
    /// <summary>
    /// PDF.NET 4.0 实体类基础类 edit 2011.3.13
    /// </summary>
    //[System.SerializableAttribute()]
    //[System.Runtime.Serialization.DataContract(Namespace = "http://schemas.datacontract.org/2004/07/")]
    public abstract class EntityBase : INotifyPropertyChanged, ICloneable, PWMIS.Common.IEntity
    {
        /*
         * 为了更加减少内存垃圾回收,使用下面的方式在实体类进行初始化,需要修改代码生成器
        
         */

        private PWMIS.Common.EntityMapType _entityMap=PWMIS .Common .EntityMapType .Table ;
        /// <summary>
        /// 实体类的映射类型
        /// </summary>
        public PWMIS.Common.EntityMapType EntityMap
        {
            get { return _entityMap; }
            protected internal  set { _entityMap = value; }
        }

        //[NonSerialized()] 
        private string[] names;
        /// <summary>
        /// 属性字段名列表
        /// </summary>
        public virtual string[] PropertyNames
        {
            get {
                if (names == null)
                {
                    this.SetFieldNames();
                    changedlist = new bool[names.Length];
                }
                return names; 
            }
            protected internal set
            {
                names = value;
                changedlist = new bool[names.Length];
            }
        }

        //新版本必须实现这个细节,为了兼容性,这里不作为抽象方法
        protected internal virtual void SetFieldNames()
        {
            //this.names = names;
        }
    
        //[NonSerialized()] 
        private object[] values;
        /// <summary>
        /// 属性值列表
        /// </summary>
        public virtual object[] PropertyValues
        {
            get {
                if (values == null)
                {
                    values = new object[PropertyNames.Length];
                }
                return values; 
            }
            protected internal set { values = value; }
        }
       

        /// <summary>
        /// 设置所有属性的值
        /// </summary>
        /// <param name="values"></param>
        public void SetPropertyValues(object[] values)
        {
            if (values.Length != PropertyNames.Length)
                throw new Exception("要设置的值数组大小跟属性名数量不一致。");
            PropertyValues = values;
        }

        

        //public void FillEntity(string[] propertyNames,object[] propertyValues)
        //{ 
        
        //}
        //[NonSerialized()] 
        private bool[] changedlist;
        /// <summary>
        /// 属性值被改变的属性名列表
        /// </summary>
        protected internal List<string> PropertyChangedList
        {
            get {
                List<string> list = new List<string>();
                if (names.Length > 0)
                {
                    for (int i = 0; i < changedlist.Length; i++)
                    {
                        if (changedlist[i])
                            list.Add(names[i]);
                    }
                }
                return list;
            }
        }

        //[NonSerialized()] 
        private List<string> _pks = new List<string>();
        //[NonSerialized()] 
        private string _identity = "";
       

        #region INotifyPropertyChanged 成员

        /// <summary>
        /// 属性改变事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 触发属性改变事件
        /// </summary>
        /// <param name="e">属性改变事件对象</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }
        }

        #endregion

        #region ICloneable 成员
        /// <summary>
        /// 获取当前对象的浅表副本
        /// </summary>
        /// <returns>当前对象的浅表副本</returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

        #region IEntity 成员

        /// <summary>
        /// 标识字段名称（有些数据库可能内置不支持），该字段不可更新，但是插入数据的时候可以获取该字段
        /// </summary>
        public string IdentityName
        {
            get { return _identity; }
            protected set { _identity = value; }
        }

        /// <summary>
        /// 主键字段名称列表
        /// </summary>
        public List<string> PrimaryKeys
        {
            protected set { _pks = value; }
            get { return _pks; }
        }

       /// <summary>
        /// 获取属性列的值
       /// </summary>
       /// <param name="propertyName">属性字段名称</param>
       /// <returns>属性值</returns>
        public object PropertyList(string propertyName)
        {
            if (names != null)
            {
                for(int i=0;i<names.Length ;i++)
                {
                    if (string.Compare(names[i], propertyName, true) == 0)
                    {
                        return values[i];
                    }
                }
            }
            
            return null;
        }
        //[NonSerialized()] 
        private string _tableName;
        /// <summary>
        /// 实体类对应的数据库表名称
        /// </summary>
        public string TableName
        {
            protected set { _tableName = value; }
            get {
                if (EntityMap == EntityMapType.SqlMap)
                {
                    int at = _tableName.LastIndexOf('.');
                    if (at > 0)
                        return _tableName.Substring(at+1);
                    else
                        return _tableName;

                }
                return _tableName; 
            }
        }


        #endregion



        #region IEntity 成员


        //public Dictionary<string, object> PropertyList
        //{
        //    get { throw new NotImplementedException(); }
        //}

        #endregion

        /// <summary>
        /// 属性获取事件
        /// </summary>
        public event EventHandler<PropertyGettingEventArgs> PropertyGetting;
        /// <summary>
        /// 获取属性的时候
        /// </summary>
        /// <param name="name"></param>
        protected virtual void OnPropertyGeting(string name)
        {
            if (this.PropertyGetting != null)
            {
                this.PropertyGetting(this, new PropertyGettingEventArgs(name));
            }
        }
        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="propertyName">属性字段名称</param>
        /// <param name="t">属性类型代码</param>
        /// <returns>属性值</returns>
        protected object getProperty(string propertyName, TypeCode t)
        {
            this.OnPropertyGeting(propertyName);

            object Value= PropertyList(propertyName);

            if (Value == DBNull.Value || Value ==null)
            {
                switch (t)
                {
                    case TypeCode.String: return null;
                    case TypeCode.DateTime: return new DateTime(1900, 1, 1);
                    case TypeCode.Boolean: return false;
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode .UInt16 :
                    case TypeCode .UInt32 :
                    case TypeCode .UInt64 :return 0;
                    case TypeCode .Char :return null;
                    default: return 0.0;
                }
               
            }
            return Value;
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="propertyName">属性名称</param>
        /// <returns>属性值</returns>
        protected T getProperty<T>(string propertyName)
        {
            this.OnPropertyGeting(propertyName);

            object Value = PropertyList(propertyName);

            if (Value == DBNull.Value || Value == null)
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
            return (T)Value;
        }
        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="propertyName">属性字段名称</param>
        /// <param name="Value">属性值</param>
        protected internal void setProperty(string propertyName, object Value)
        {
            if (names != null && values !=null)
            {
                for (int i = 0; i < names.Length; i++)
                {
                    if (string.Compare(names[i], propertyName, true) == 0)
                    {
                        values[i]=Value ;

                        this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
                        changedlist[i] = true;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 设置属性，如果值是字符类型且设置了最大长度大于0，那么不允许设置大于此长度的字符串
        /// </summary>
        /// <param name="name">字段名称</param>
        /// <param name="Value">值</param>
        /// <param name="maxLength">最大长度</param>
        protected internal void setProperty(string name, string Value, int maxLength)
        {
            if (Value != null && maxLength > 0 && Value.Length > maxLength)
                throw new Exception("字段" + name + "的实际长度超出了最大长度" + maxLength);
            else
                setProperty(name, Value);
        }
    }
}
