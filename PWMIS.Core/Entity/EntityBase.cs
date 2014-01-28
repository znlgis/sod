﻿/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap 
 * ========================================================================
 * 该类的作用
 * 
 * 作者：邓太华     时间：2008-10-12
 * 版本：V5.0
 * 
 * 修改者：         时间：2013-3-26                
 * 修改说明：增加SetDefaultChanges 方法，用于跟属性默认值比较，从而设置属性是否更改过值
 * 
 *  * 修改者：         时间：2013-4-5\6                
 * 修改说明：TableName 和 IdentityName 属性修改成受保护的属性，方便直接在GridView 控件中使用
 *           增加MapNewTableName 新方法来设置要更改的表名称
 *           
 *  * 修改者：         时间：2013-8-16                
 * 修改说明：增加索引器，方便属性访问
 *  
  *  * 修改者：         时间：2013-10-7                
 * 修改说明：解决如果从部分选择查询出来的实体类，无法在应用中保存选择查询列之外的事实体属性值无法设置的问题。
 * 
 *   * 修改者：         时间：2013-11-23                
 * 修改说明：放开实体类对索引属性访问的写入功能，例如下面的代码：
 *          UserModels um = new UserModels();
            um.AddPropertyName("TestName");
            um["TestName"] = 123;
            int testi =(int) um["TestName"];
 * ========================================================================
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using PWMIS.Common;
using PWMIS.Core;

namespace PWMIS.DataMap.Entity
{
    /// <summary>
    /// PDF.NET 4.6.2 实体类基础类
    /// </summary>
    //[System.SerializableAttribute()]
    //[System.Runtime.Serialization.DataContract(Namespace = "http://schemas.datacontract.org/2004/07/")]
    public abstract class EntityBase : INotifyPropertyChanged, ICloneable, PWMIS.Common.IEntity
    {
        #region 处理字符串属性与对应列的长度映射
        //为字符串字段指定长度，将有利于查询提高效率 edit at 2012.4.23
        protected internal static Dictionary<string, int> StringFieldSize = new Dictionary<string, int>();
        protected internal static int GetStringFieldSize(string tableName, string fieldName)
        {
            string key = string.Format("{0}_{1}", tableName, fieldName);
            if (StringFieldSize.ContainsKey(key))
                return StringFieldSize[key];
            else
                return 255;
        }
        protected internal int GetStringFieldSize(string fieldName)
        {
            return GetStringFieldSize(TableName, fieldName);
        }
        #endregion

        #region 实体类基本映射信息 相关成员
        private PWMIS.Common.EntityMapType _entityMap = PWMIS.Common.EntityMapType.Table;
        /// <summary>
        /// 实体类的映射类型
        /// </summary>
        protected internal PWMIS.Common.EntityMapType EntityMap //
        {
            get { return _entityMap; }
            set { _entityMap = value; }
        }


        /// <summary>
        /// 设置实体类的对应的字段名称数组
        /// 新版本必须实现这个细节,为了兼容性,这里不作为抽象方法
        /// </summary>
        protected internal virtual void SetFieldNames()
        {
            //this.names = names;
        }

        //[NonSerialized()] 
        private string _identity = "";

        /// <summary>
        /// 标识字段名称（有些数据库可能内置不支持），该字段不可更新，但是插入数据的时候可以获取该字段
        /// </summary>
        protected internal string IdentityName
        {
            get { return _identity; }
            set { _identity = value; }
        }

        /// <summary>
        /// 外键
        /// </summary>
        protected internal string ForeignKey { get; set; }

        //[NonSerialized()] 
        private string _tableName;
        /// <summary>
        /// 实体类对应的数据库表名称
        /// </summary>
        protected internal string TableName
        {
            set { _tableName = value; }
            get
            {
                if (EntityMap == EntityMapType.SqlMap)
                {
                    int at = _tableName.LastIndexOf('.');
                    if (at > 0)
                        return _tableName.Substring(at + 1);
                    else
                        return _tableName;

                }
                return this.GetTableName();
            }
        }
        /// <summary>
        /// 将实体类的表名称映射到一个新的表名称
        /// </summary>
        /// <param name="newTableName">新的表名称</param>
        /// <returns>是否成功</returns>
        public bool MapNewTableName(string newTableName)
        {
            if (EntityMap == EntityMapType.Table)
            {
                this.TableName = newTableName;
                return true;
            }
            return false;
        }
        #endregion

        #region 属性状态改变状态成员

        //[NonSerialized()] 
        private bool[] changedlist;

        /// <summary>
        /// 重置属性值的修改状态
        /// </summary>
        protected internal void ResetChanges()
        {
            ResetChanges(false);
        }


        /// <summary>
        /// 重置实体类全部属性的修改标记。注意，EntityQuery将根据该标记决定更新哪些字段到数据库，
        /// 它只更新标记为已经修改的实体类属性
        /// </summary>
        /// <param name="flag">是否已经修改</param>
        public void ResetChanges(bool flag)
        {
            if (changedlist != null)
            {
                for (int i = 0; i < changedlist.Length; i++)
                    changedlist[i] = flag;
            }
        }

        /// <summary>
        /// 设置属性的值是否跟默认值一样，如果一样则表示该属性未更改过，例如在分布式系统中DTO转换到实体类对象后的处理。
        /// <remarks>2013.3.26 增加，用在WebService或者WCF的系统中</remarks>
        /// </summary>
        public void SetDefaultChanges()
        {
            for (int i = 0; i < PropertyValues.Length; i++)
            {
                object value = PropertyValues[i];
                if (value != null && value != DBNull.Value)
                {
                    Type type = value.GetType();
                    if (type.IsValueType)
                    {
                        object newValue = Activator.CreateInstance(type);
                        changedlist[i] = !newValue.Equals(value);//等于默认值，未改变
                    }
                }
                else
                {
                    //该属性未设置任何值，或者是字符串属性
                    changedlist[i] = false;
                }
            }
        }

        /// <summary>
        /// 属性值被改变的属性名列表
        /// </summary>
        protected internal List<string> PropertyChangedList
        {
            get
            {
                List<string> list = new List<string>();
                if (PropertyNames.Length > 0)
                {
                    for (int i = 0; i < changedlist.Length; i++)
                    {
                        if (changedlist[i])
                            list.Add(PropertyNames[i]);
                    }
                }
                return list;
            }
        }

        #endregion

        #region 属性获取事件相关

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
        #endregion

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

        #region IEntity 成员

        //[NonSerialized()] 
        private List<string> _pks = new List<string>();
        /// <summary>
        /// 主键字段名称列表
        /// </summary>
        public List<string> PrimaryKeys
        {
            protected set { _pks = value; }
            get { return _pks; }
        }

        public string GetIdentityName()
        {
            return this.IdentityName;
        }

        /// <summary>
        /// 获取表名称。如果实体类有分表策略，那么请重写该方法
        /// </summary>
        /// <returns></returns>
        public virtual string GetTableName()
        {
            return _tableName; ;
        }

        /// <summary>
        /// 新增加实体虚拟字段属性，用来传递内容
        /// </summary>
        /// <param name="name"></param>
        public void AddPropertyName(string name)
        {
            if (name == null || name.Length == 0) return;
            var strA = string.Empty;

            int count=this.PropertyNames.Length+1;
            string[] temp = new string[count];
            this.PropertyNames.CopyTo(temp, 0);
            temp[count - 1] = name;

            this.PropertyNames = temp;
        }

        /// <summary>
        /// 获取属性列的值
        /// </summary>
        /// <param name="propertyName">属性字段名称</param>
        /// <returns>属性值</returns>
        public object PropertyList(string propertyName)
        {
            for (int i = 0; i < PropertyNames.Length; i++)
            {
                if (string.Compare(PropertyNames[i], propertyName, true) == 0)
                {
                    return PropertyValues[i];
                }
            }
            return null;
        }

        //[NonSerialized()] 
        private string[] names;
        /// <summary>
        /// 属性字段名列表
        /// </summary>
        public virtual string[] PropertyNames
        {
            get
            {
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

        #endregion

        #region 获取或者设置属性值
        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="propertyName">属性字段名称</param>
        /// <param name="t">属性类型代码</param>
        /// <returns>属性值</returns>
        protected object getProperty(string propertyName, TypeCode t)
        {
            this.OnPropertyGeting(propertyName);

            object Value = PropertyList(propertyName);

            if (Value == DBNull.Value || Value == null)
            {
                switch (t)
                {
                    case TypeCode.String: return null;
                    case TypeCode.DateTime: return new DateTime(1900, 1, 1);
                    case TypeCode.Boolean: return false;
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64: return 0;
                    case TypeCode.Char: return null;
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
            return CommonUtil.ChangeType<T>(PropertyList(propertyName));
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="propertyName">属性字段名称</param>
        /// <param name="Value">属性值</param>
        protected internal void setProperty(string propertyName, object Value)
        {
            for (int i = 0; i < PropertyNames.Length; i++)
            {
                if (string.Compare(PropertyNames[i], propertyName, true) == 0)
                {
                    PropertyValues[i] = Value;

                    this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
                    changedlist[i] = true;
                    return;
                }
            }
            //可能实体类来自Select 部分字段
            //备份原来的名值组
            Dictionary<string, object> dictTemp = new Dictionary<string, object>();
            Dictionary<string, bool> dictChengs = new Dictionary<string, bool>();
            for (int i = 0; i < PropertyNames.Length; i++)
            {
                dictTemp.Add(PropertyNames[i], PropertyValues[i]);
                dictChengs.Add(PropertyNames[i], changedlist[i]);
            }
            //重置字段名数组
            names = null;
            values = null;
            //复制值
            for (int i = 0; i < PropertyNames.Length; i++)
            {
                PropertyValues[i] = dictTemp[PropertyNames[i]];
                changedlist[i] = dictChengs[PropertyNames[i]];
            }
            // 如果propertyName 仍然不在实体类本身类型定义的字段名中，说明是非法的设置，无效；
            //否则，重新设置当前要设置的值。
            for (int i = 0; i < PropertyNames.Length; i++)
            {
                if (string.Compare(PropertyNames[i], propertyName, true) == 0)
                {
                    PropertyValues[i] = Value;

                    this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
                    changedlist[i] = true;
                    return;
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
            string key = string.Format("{0}_{1}", TableName, name);
            StringFieldSize[key] = maxLength;

            if (Value != null && maxLength > 0 && Value.Length > maxLength)
                throw new Exception("字段" + name + "的实际长度超出了最大长度" + maxLength);
            else
                setProperty(name, Value);
        }

        /// <summary>
        /// 获取实体类的属性名值对对象
        /// </summary>
        /// <returns></returns>
        public PropertyNameValues GetNameValues()
        {
            PropertyNameValues result = new PropertyNameValues();
            result.PropertyNames = this.PropertyNames;
            result.PropertyValues = this.PropertyValues;
            return result;
        }

        //[NonSerialized()] 
        private object[] values;
        /// <summary>
        /// 属性值列表
        /// </summary>
        public virtual object[] PropertyValues
        {
            get
            {
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

        #region 索引器
        /// <summary>
        /// 获取指定属性名称的值
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        public object this[string propertyName]
        {
            get
            {
                EntityFields ef = EntityFieldsCache.Item(this.GetType());
                string fieldName = ef.GetPropertyField(propertyName);
                if (fieldName != null)
                {
                    this.OnPropertyGeting(fieldName);
                    return PropertyList(fieldName);
                }
                //获取虚拟的字段值
                return PropertyList(propertyName); ;
            }
            set
            {
                EntityFields ef = EntityFieldsCache.Item(this.GetType());
                string fieldName = ef.GetPropertyField(propertyName);

                if (fieldName != null)
                {
                    //如果是实体类基础定义的字段，必须检查设置的值得类型
                    if (value != null)
                    {
                        Type fieldType = ef.GetPropertyType(propertyName);
                        if (value.GetType() != fieldType)
                            throw new ArgumentException("实体类的属性字段" + propertyName + " 需要"
                                + fieldType.Name + " 类型的值，但准备赋予的值不是该类型！");
                    }
                    this.setProperty(fieldName, value);
                }
                else
                {
                    this.setProperty(propertyName, value);
                }
            }
        }

        /// <summary>
        /// 获取指定索引位置的属性的值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                if (index < 0 || index > this.PropertyNames.Length)
                    return null;
                string fieldName = this.PropertyNames[index];
                this.OnPropertyGeting(fieldName);
                return PropertyList(fieldName);
            }
        }
        #endregion
    }
}
