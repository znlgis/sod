/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap 
 * ========================================================================
 * 该类的作用
 * 
 * 作者：邓太华     时间：2008-10-12
 * 版本：V3.0
 * 
 * 修改者：         时间：                
 * 修改说明：
 * ========================================================================
*/

/*
 * ==================PWMIS 实体类===================================
 * 邓太华 2009.12.29 ver 1.0
 * -----------------------------------------------------------------
 * 以下例子说明如何使用 Entity 类
 * 自定义类User 映射数据库的表 TB_User，下面是创建表的脚本
 * 
--用户类 表脚本创建 for SQL SERVER
Create Table TB_User
(
  ID int identity(1,1) primary key,
  Name varchar(50) not null,
  Birthday datetime
);
 * 
    /// <summary>
    /// 用户类
    /// </summary>
    public class User : Entity
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public User()
        {
            TableName = "Tb_User";
            IdentityName = "id";
            PrimaryKeys.Add("id");

            AddProperty("id", default(int));
            AddProperty("Name", default(string));
            AddProperty("Birthday", default(DateTime));
        }

       
        /// <summary>
        /// 用户标记
        /// </summary>
        public int Uid
        {
            get
            {
                return (int)getProperty("id");
            }
            set
            {
                setProperty("id", value);
            }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name
        {
            get
            {
                return (string)getProperty("Name");
            }
            set
            {
                setProperty("Name", value);
            }
        }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday
        {
            get
            {
                return (DateTime)getProperty("Birthday");
            }
            set
            {
                setProperty("Birthday", value);
            }
        }
    }
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace PWMIS.DataMap.Entity
{
    /// <summary>
    /// PWMIS 实体类 基类,该类在 PDF.NET 4.0中已经过时。
    /// </summary>
    [Serializable()]
    abstract class Entity : INotifyPropertyChanged,ICloneable
    {
        /// <summary>
        /// 属性-值列表，键 不区分大小写
        /// </summary>
        private  Dictionary<string, object> _propertyList = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        private List<string> _pks = new List<string>();
        private string _identity = "";
        private Dictionary<string, bool> _propertyChangedList ;//= new Dictionary<string, bool>();
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Entity()
        {
            _propertyChangedList = new Dictionary<string, bool>();

        }
       
        /// <summary>
        /// 标识字段名称（有些数据库可能内置不支持），该字段不可更新，但是插入数据的时候可以获取该字段
        /// </summary>
        public string IdentityName
        {
            get { return _identity; }
            protected  set { _identity = value; }
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
        /// 获取属性列表
        /// </summary>
        public  Dictionary<string, object> PropertyList
        {
            get { return _propertyList; }
        }

        /// <summary>
        /// 获取值改变的属性
        /// </summary>
        public Dictionary<string, bool > PropertyChangedList
        {
            get { return _propertyChangedList; }
        }

        private string _tableName;

        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName
        {
            protected set { _tableName = value; }
            get { return _tableName; }
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected object getProperty(string name)
        {
            this.OnPropertyGeting(name);
           
            return _propertyList[name];
        }

        /// <summary>
        /// 增加属性及其默认值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Value"></param>
        protected void AddProperty(string name, object Value)
        {
            _propertyList[name] = Value;
            _propertyChangedList[name] = false;
        }

       /// <summary>
        /// 设置属性
       /// </summary>
       /// <param name="name"></param>
       /// <param name="Value"></param>
        protected internal void setProperty(string name, object Value)
        {
           
            //根据属性的默认值，处理来自数据库的空值查询
            if (Value == null || Value == DBNull.Value)
            {
                //if (_propertyList[name] == null)
                //    _propertyList[name]=null;
                //else if (_propertyList[name] != null && _propertyList[name].GetType() == typeof(string))
                //    _propertyList[name] = null;
                //else
                //    _propertyList[name] = 0;//其他值类型
                // Null 字符串 GetType() 将报错。
                if (_propertyList[name] is ValueType)
                    _propertyList[name] = 0;
                else
                    _propertyList[name] = null;
            }
            else
                _propertyList[name] = Value;
            //
            this.OnPropertyChanged(new PropertyChangedEventArgs(name));
            _propertyChangedList[name] = true;
        }

        /// <summary>
        /// 从数据库设置属性值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Value"></param>
        protected internal void dbSetProperty(string name, object Value)
        {
            //根据属性的默认值，处理来自数据库的空值查询
            if ( Value == DBNull.Value || Value == null )
            {
                if (_propertyList[name] is ValueType)
                    _propertyList[name] = 0;
                else
                    _propertyList[name] = null;
            }
            else
                _propertyList[name] = Value;

            //_propertyChangedList[name] = false;
            
            
        }

        /// <summary>
        /// 设置属性，如果值是字符类型且设置了最大长度大于0，那么不允许设置大于此长度的字符串
        /// </summary>
        /// <param name="name">字段名称</param>
        /// <param name="Value">值</param>
        /// <param name="maxLength">最大长度</param>
        protected internal void setProperty(string name, string  Value, int maxLength)
        {
            if (Value != null && maxLength > 0 && Value.Length > maxLength)
                throw new Exception("字段" + name + "的实际长度超出了最大长度" + maxLength);
            else
                setProperty(name, Value);
        }


        #region INotifyPropertyChanged 成员
        /// <summary>
        /// 属性改变事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }
        }

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


        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone () ;
        }

        #endregion
    }

    /// <summary>
    /// 属性获取事件
    /// </summary>
    public class PropertyGettingEventArgs : EventArgs
    {
        private string _name;

        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 以属性名称初始化本类
        /// </summary>
        /// <param name="name"></param>
        public PropertyGettingEventArgs(string name)
        {
            this.PropertyName = name;
        }
    }

    /// <summary>
    /// 属性改变前事件
    /// </summary>
    public class PropertyChangingEventArgs : EventArgs
    {
        /// <summary>
        /// 初始化属性改变前事件
        /// </summary>
        /// <param name="propertyFieldName">属性字段名</param>
        /// <param name="value">当前属性要设置的值</param>
        /// <param name="length">当前属性字段的长度</param>
        public PropertyChangingEventArgs(string propertyFieldName,object value,int length)
        {
            this.PropertyName = propertyFieldName;
            this.NewValue = value;
            this.MaxValueLength = length;
        }

        /// <summary>
        /// 属性名，如果属性对应的字段名与属性名不同，那么这里是属性字段名
        /// </summary>
        public virtual string PropertyName { get; private set; }
        /// <summary>
        /// 新设置的属性值
        /// </summary>
        public object NewValue { get; private set; }
        /// <summary>
        /// 新设置的属性值的最大长度，仅仅对string类型有效，其它类型，都是-1
        /// </summary>
        public int MaxValueLength { get; private set; }
        /// <summary>
        /// 是否取消改变属性的值
        /// </summary>
        public bool IsCancel { get; set; }


    }

}
