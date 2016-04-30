/*
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
 * 
 *   *修改者：         时间：2014-3-4   
 *   * 修改说明：增加设置和访问实体类外键字段名称的功能，以支持实体类的外键关系。可以设置多个外键字段。
 *   
 *   *修改者：         时间：2014-10-14 
 *   重新设置实体类的值得时候，重置修改状态 感谢网友 Panke 发现此Bug。
 *   该Bug会导致从数据库查询出来的实体类集合在修改数据以后，会重置其它元素的修改状态，从而导致只会修改一条记录
 *   
 * *修改者：         时间：2014-10-21  
 *  新增 SetStringFieldSize 方法，用于使用API的方式设置字段长度
 *  
 *  * 修改者：         时间：2014-11-9  
 *  新增 构造函数调用元数据初始化方法的功能，方便在使用代码生成器的时候不覆盖手写实体类代码
 *  
 *  * 修改者：         时间：2015-2-11  
 *  修复 MapFrom 方法中，实体类的属性字段可能跟属性名称不一样 造成数据无法复制的问题
 *  
 *  * 修改者：         时间：2015-3-3  
 *  增加 GetPropertyFieldNameIndex 内部方法，优化字段名查找效率
 *  
*  * 修改者：         时间：2015-4-22  
 *  感谢网友 照嘉隆 发现重置属性修改状态的方法中,没有判断字符串属性的问题
 *  修正Select 的字段不在实体类中查询的问题
 *  
 * * 修改者：         时间：2015-5-27  
 *  实体类增加 GetChangedValues 方法，获取修改过的属性字段名称和值
 * 
 *  * 修改者：         时间：2015-7-5  
 *  修改实体类字符串长度获取的Bug，当有同名字段的时候会出现问题，感谢网友 阳光尚好 发现此问题
 *  
 *  * 修改者：         时间：2015-10-22  
 *  修改MapFrom 方法，增加设置修改状态的功能，感谢网友 石家庄-零点 提供此建议
 *  
 *  * 修改者：         时间：2015-10-24  
 *  修改MapToPOCO 方法，可以避免覆盖POCO对象不想赋值的属性，感谢网友 石家庄-零点 发现此问题 
 *  
 * 
 * 
 *  * 修改者：         时间：2015-12-4  
 *  新增 GetGolbalEntityID 方法，解决在多线程环境下，获取字符串类型属性字段长度可能引起的问题，感谢网友 Rookie 发现该问题。
 *  
 *  *修改者：         时间：2016-3-23  
 *  新增 属性索引功能，加快实体类在多次使用的情况下的属性访问效率
 *  
 *  * 修改者：         时间：2016-4-30  
 *  新增 属性字段描述功能，代码生成器可以将数据库表设计的字段说明信息生成到实体类上，方便运行时获取该信息。
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
    /// PDF.NET 5.5 实体类基础类
    /// </summary>
    public abstract class EntityBase : INotifyPropertyChanged, ICloneable, PWMIS.Common.IEntity
    {
        #region 构造和初始化
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public EntityBase()
        {
            InitMetaDataExt();
        }

        /// <summary>
        /// 初始化元数据扩展，比如在此中手工设置子实体类与父实体类的外键关系，
        /// 如果在用户的分部类文件中重写该方法，可以防止代码生成器覆盖该方法
        /// </summary>
        protected virtual void InitMetaDataExt()
        {

        }

        /// <summary>
        /// 获取实体类的全局标识，建议实体类具体实现类重写该方法，默认取类型全名称
        /// </summary>
        /// <returns></returns>
        public virtual string GetGolbalEntityID()
        {
            return GetType().FullName;
        }
        #endregion

        #region 处理字符串属性与对应列的长度映射
        //为字符串字段指定长度，将有利于查询提高效率 edit at 2012.4.23
        protected internal static Dictionary<string, int> StringFieldSize = new Dictionary<string, int>();
        protected internal static Dictionary<string, string> PropertyDesciption = new Dictionary<string, string>();
        /// <summary>
        /// 寻找字符串字段的长度，如果找不到，返回0，表示未指定
        /// </summary>
        /// <param name="ownerName">字段的所有者，是一个标识</param>
        /// <param name="fieldName">字段名</param>
        /// <returns>字段的长度</returns>
        protected  static int GetStringFieldSize(string ownerName, string fieldName)
        {
            string key = string.Format("{0}_{1}", ownerName,fieldName);
            if (StringFieldSize.ContainsKey(key))
                return StringFieldSize[key];
            else
                return 0;
        }
        protected internal int GetStringFieldSize(string fieldName)
        {
            //return GetStringFieldSize(TableName, fieldName);
            //TableName 在调用了 MapNewTableName 方法后，可能找不到属性字段的长度，故这里取消原来代码的使用方法
            //感谢网友  广州-玄离 发现该问题 
            //这里必须使用实体类的类型名称来限定，感谢网友  发现问题
            return GetStringFieldSize(GetGolbalEntityID(), fieldName);
        }

        /// <summary>
        /// 设置字符串类型的属性字段在数据库读写的时候使用的字段长度。
        /// 注意这这个设置将一直有效且只对字符串类型的字段有效，除非再次重新设置。
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="length">字段长度</param>
        protected internal void SetStringFieldSize(string fieldName, int length)
        {
            if (length > 0)
            {
                string key = string.Format("{0}_{1}", GetGolbalEntityID(), fieldName);
                StringFieldSize[key] = length;
            }
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
        /// 新版本子类可以实现这个细节,否则框架将反射获得该信息(该特性有利于简化手写的代码)。
        /// <remarks>为了兼容性,这里不作为抽象方法</remarks>
        /// </summary>
        protected internal virtual void SetFieldNames()
        {
            //this.names = names;
            //如果子类未重写该方法，调用框架的数据库元数据获取方法
            EntityFields ef=EntityFieldsCache.Item(this.GetType());
            this.names = ef.Fields;
        }

        /// <summary>
        /// 获取字段描述信息，默认无，如果需要请在具体的实现类里面重写该方法。
        /// 代码生成器可能会重写该方法。
        /// </summary>
        /// <returns></returns>
        protected internal virtual string[] SetFieldDescriptions()
        {
            return null;
        }

        /// <summary>
        /// 获取所有字段的描述，跟字段名一一对应
        /// </summary>
        /// <returns></returns>
        public string[] GetFieldDescriptions()
        {
            string[] desc = this.SetFieldDescriptions();
            if (desc == null)
            {
                EntityFields ef = EntityFieldsCache.Item(this.GetType());
                desc = ef.FieldDescriptions;
                if (desc == null)
                    throw new NotImplementedException("实体类没有重写实现 SetFieldDescriptions 方法。");
            }
            return desc;
        }

        /// <summary>
        /// 获取指定字段名对应的描述
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <returns>字段描述</returns>
        public string GetFieldDescription(string fieldName)
        {
            int index = -1;
            EntityFields ef = EntityFieldsCache.Item(this.GetType());
            for (int i=0;i<ef.Fields.Length;i++)
            {
                string s = ef.Fields[i];
                if (s == fieldName)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
                throw new ArgumentOutOfRangeException(fieldName+" 不在实体类定义的字段范围内。");
            return GetFieldDescriptions()[index];
        }


        //[NonSerialized()] 
        private string _identity = string.Empty;

        /// <summary>
        /// 标识字段名称（有些数据库可能内置不支持），该字段不可更新，但是插入数据的时候可以获取该字段
        /// </summary>
        protected internal string IdentityName
        {
            get { return _identity; }
            set { _identity = value; }
        }

        /// <summary>
        /// 实体类映射的表的架构名字
        /// </summary>
        protected internal string Schema { get; set; }

        private string setingFieldName = string.Empty;

        /// <summary>
        /// 外键
        /// </summary>
        private string foreignKeys = string.Empty;

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

        /// <summary>
        /// 获取带架构的表名称，带中括号
        /// </summary>
        /// <returns></returns>
        protected internal  string GetSchemeTableName()
        {
            if (!string.IsNullOrEmpty(Schema))
                return string.Format("[{0}].[{1}]", Schema, TableName);
            else
                return string.Format("[{1}]", Schema, TableName); 
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
                    else if (type == typeof(string))
                    {
                        //感谢网友 照嘉隆 发现没有判断字符串属性的问题
                        changedlist[i] = value.ToString().Length > 0;
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
        /// 属性改变前事件
        /// </summary>
        public event EventHandler<PropertyChangingEventArgs> PropertyChanging;

        /// <summary>
        /// 属性改变事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 触发属性改变事件
        /// </summary>
        /// <param name="e">属性改变事件对象</param>
        protected virtual void OnPropertyChanged(string propertyFieldName)
        {
            if (this.PropertyChanged != null)
            {
                string currPropName = EntityFieldsCache.Item(this.GetType()).GetPropertyName(propertyFieldName);
                this.PropertyChanged(this, new PropertyChangedEventArgs(currPropName));
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

        ///// <summary>
        ///// 获取设置过的属性字段名称，用于映射到表字段的属性进行赋值操作之后
        ///// </summary>
        ///// <returns></returns>
        //public string GetSetPropertyFieldName()
        //{
        //    return setingFieldName;
        //}

        //private bool _IsTestWriteProperty;
        ///// <summary>
        ///// 测试写入属性（仅程序集内部使用）
        ///// </summary>
        //internal void TestWriteProperty()
        //{
        //    _IsTestWriteProperty = true;
        //}

        /// <summary>
        /// 新增加实体虚拟字段属性，用来传递内容。注意不检查是否重复
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

            object[] tempValue = new object[count];
            this.PropertyValues.CopyTo(tempValue, 0);
            this.PropertyValues = tempValue;

            bool[] tempChanges = new bool[count];
            this.changedlist.CopyTo(tempChanges, 0);
            this.changedlist = tempChanges;
        }

        /// <summary>
        /// 获取属性字段的位置索引，如果找不到，返回-1
        /// </summary>
        /// <param name="propertyFieldName">属性字段名</param>
        /// <returns>属性字段的位置索引，如果找不到，返回-1</returns>
        protected internal int GetPropertyFieldNameIndex(string propertyFieldName)
        {
            if (string.IsNullOrEmpty(propertyFieldName))
                return -1;
            string temp = null;
            int length = propertyFieldName.Length;
            for (int i = 0; i < PropertyNames.Length; i++)
            {
                //原有代码的比较方式不太高效，详细测试代码及原理请参考文章 http://www.cnblogs.com/bluedoctor/p/3899892.html
                //if (string.Compare(PropertyNames[i], propertyFieldName, true) == 0)
                temp = PropertyNames[i];
                if (temp != null && temp.Length == length
                    && string.Equals(temp, propertyFieldName, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 获取属性列的值，但不会产生属性获取事件
        /// </summary>
        /// <param name="propertyName">属性字段名称</param>
        /// <returns>属性值</returns>
        public object PropertyList(string propertyFieldName)
        {
            int index = GetPropertyFieldNameIndex(propertyFieldName);
            if (index == -1)
                return null;
            else
                return PropertyValues[index];
        }

        /// <summary>
        /// 获取属性的值，如果没有，将返回属性类型的默认值；注意该方法调用将发起属性获取事件 。
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public object GetPropertyValueWithEvent(string propertyName)
        {
            EntityFields ef = EntityFieldsCache.Item(this.GetType());
            string fieldName = ef.GetPropertyField(propertyName);
            if (fieldName != null)
            {
                this.OnPropertyGeting(fieldName);
                object result= PropertyList(fieldName);
                if (result == null || result == DBNull.Value)
                {
                    Type propType = ef.GetPropertyType(fieldName);
                    result = Activator.CreateInstance(propType);//活动类型的默认值
                }
                return result;
            }
            return new ArgumentOutOfRangeException("不存在指定的属性名："+propertyName);
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

        #region ICloneable 成员
        /// <summary>
        /// 获取当前对象的浅表副本
        /// </summary>
        /// <returns>当前对象的浅表副本</returns>
        public object Clone(bool isDeep)
        {
            object result= this.MemberwiseClone();
            if (isDeep)
            {
                EntityBase entity = (EntityBase)result;
                object[] Values = new object[this.PropertyNames.Length];
                for (int i = 0; i < Values.Length; i++)
                    Values[i] = this.PropertyValues[i];
                entity.SetPropertyValues(Values);
            }
            return result;
        }

        /// <summary>
        /// 返回当前对象的副本
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return Clone(true );
        }

        #endregion

        #region 获取或者设置属性值
        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="propertyName">属性字段名称</param>
        /// <param name="t">属性类型代码</param>
        /// <returns>属性值</returns>
        protected object getProperty(string propertyFieldName, TypeCode t)
        {
            this.OnPropertyGeting(propertyFieldName);

            object Value = PropertyList(propertyFieldName);

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
        /// <param name="propertyFieldName">属性名称</param>
        /// <returns>属性值</returns>
        protected T getProperty<T>(string propertyFieldName)
        {
            this.OnPropertyGeting(propertyFieldName);
            return CommonUtil.ChangeType<T>(PropertyList(propertyFieldName));
        }

        #region 实体类属性索引 2016.3.23
        //属性的索引号对应属性值的关系数组
        private int[] propertyValueIndex = null;


        /// <summary>
        /// 根据属性字段名称和属性索引号，获取属性的值；采用此方法将加快属性值的获取效率。
        /// 建议实体类属性数量多余10个以上的时候使用本方法，效率将接近 Log(N)=1
        /// <example>
        /// <![CDATA[
        ///   public string Name
        ///   {
        ///       get{ return getProperty<string>("User Name",1);}
        ///       set{ setProperty("User Name",value,50);}
        ///   }
        /// 
        /// ]]>
        /// </example>
        /// </summary>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="propertyFieldName">属性字段名</param>
        /// <param name="propertyIndex">属性的索引号</param>
        /// <returns>属性值</returns>
        protected T getProperty<T>(string propertyFieldName,int propertyIndex)
        {
            this.OnPropertyGeting(propertyFieldName);
            if (propertyIndex >= 0)
            {
                int index = getPropertyValueIndex(propertyIndex);
                if (index == -1)
                {
                    index = GetPropertyFieldNameIndex(propertyFieldName);
                    if (index == -1)
                        return default(T);
                    else
                        propertyValueIndex[propertyIndex] = index;
                }
                return CommonUtil.ChangeType<T>(PropertyValues[index]);
            }
            else
            {
                return CommonUtil.ChangeType<T>(PropertyList(propertyFieldName));
            }
        }

        /// <summary>
        /// 根据实体类属性的索引号，获取实体类属性值的索引
        /// </summary>
        /// <param name="propertyIndex">实体类属性的索引号</param>
        /// <returns></returns>
        private int getPropertyValueIndex(int propertyIndex)
        {
            if (propertyIndex <0 )
                throw new Exception("当前实体类属性读取方法指定的 属性索引 值不能小于0 ");
            if (propertyValueIndex == null)
            {
                //下面一行代码会阻塞多余的线程
                EntityFields ef = EntityFieldsCache.Item(this.GetType());
                int[] tempArr = new int[ef.Fields.Length];
                for (int i = 0; i < tempArr.Length; i++)
                    tempArr[i] = -1; 
                if (propertyValueIndex == null)
                    propertyValueIndex = tempArr; 
            }
            if (propertyIndex > propertyValueIndex.Length)
                throw new Exception("当前实体类属性读取方法指定的 属性索引 值（" + propertyIndex + "）大于可用的实体类属性数量");
          
            return propertyValueIndex[propertyIndex];
        }

        #endregion

        #region 设置属性值的多个重载相关
        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="propertyFieldName">属性字段名</param>
        /// <param name="Value">要设置的值</param>
        protected internal void setProperty(string propertyFieldName, object Value)
        {
            setPropertyValueAndLength(propertyFieldName,-1, Value, 0);
        }

        /// <summary>
        /// 根据属性字段名和属性的索引号设置属性的值，用于快速设置属性值的情况
        /// </summary>
        /// <param name="propertyFieldName">属性字段名</param>
        /// <param name="propertyIndex">属性的索引号</param>
        /// <param name="Value">要设置的值</param>
        protected internal void setProperty(string propertyFieldName,int propertyIndex, object Value)
        {
            setPropertyValueAndLength(propertyFieldName, propertyIndex, Value, 0);
        }

        /// <summary>
        /// 设置字符串属性的值，如果值是字符类型且设置了最大长度大于0，那么不允许设置大于此长度的字符串
        /// </summary>
        /// <param name="propertyFieldName">属性字段名</param>
        /// <param name="Value">要设置的值</param>
        /// <param name="maxLength">字段最大长度，如果为负数，将生成varchar类型的参数</param>
        protected internal void setProperty(string propertyFieldName, string Value, int maxLength)
        {
            setProperty(propertyFieldName, -1, Value, maxLength);
        }

        protected internal void setProperty(string propertyFieldName, int propertyIndex, string Value, int maxLength)
        {
            if (Value != null && maxLength > 0 && Value.Length > maxLength)
                throw new Exception("字段" + propertyFieldName + "的实际长度超出了最大长度" + maxLength);
            else
                setPropertyValueAndLength(propertyFieldName, propertyIndex, Value, maxLength);
        }

        /// <summary>
        /// 设置属性值和长度信息
        /// </summary>
        /// <param name="propertyFieldName">属性字段名称</param>
        /// <param name="Value">属性值</param>
        /// <param name="length"></param>
        private void setPropertyValueAndLength(string propertyFieldName,int propertyIndex, object Value,int length)
        {
            //
            //if (_IsTestWriteProperty)
            //    return;

            if (this.PropertyChanging != null)
            {
                setingFieldName = propertyFieldName;
                PropertyChangingEventArgs changingArg = new PropertyChangingEventArgs(propertyFieldName, Value, length);
                this.PropertyChanging(this, changingArg);
                if (changingArg.IsCancel)
                    return;
            }
            //
            //for (int i = 0; i < PropertyNames.Length; i++)
            //{
            //    if (string.Compare(PropertyNames[i], propertyFieldName, true) == 0)
            //    {
            //        PropertyValues[i] = Value;

            //        this.OnPropertyChanged(new PropertyChangedEventArgs(propertyFieldName));
            //        changedlist[i] = true;
            //        return;
            //    }
            //}
            //用下面的代码替代 GetPropertyFieldNameIndex

            int index = -1;
            if (propertyIndex >= 0 && propertyValueIndex != null)
                index = propertyValueIndex[propertyIndex];
            if(index==-1)
                index = GetPropertyFieldNameIndex(propertyFieldName);
            if (index >= 0)
            {
                PropertyValues[index] = Value;

                this.OnPropertyChanged(propertyFieldName);
                changedlist[index] = true;
                return;
            }
            //可能实体类来自Select 部分字段
            //备份原来的名值组
            string[] namesTemp = PropertyNames;
            object[] valuesTemp = PropertyValues;
            bool[] changesTemp = changedlist;

            //重置字段名数组，names 为空,PropertyNames调用将会触发调用子类重载的 SetFieldNames 方法。
            names = null;
            values = null;
             //复制值
            bool setValueFlag = false;
            for (int i = 0; i < PropertyNames.Length; i++)
            {
                string name = PropertyNames[i];
                if (propertyFieldName == name)
                {
                    setValueFlag = true;
                    PropertyValues[i] = Value;
                    changedlist[i] = true;
                }
                else
                {
                    //如果未找到，说明原来实例对象的属性字段不在实体类的定义的属性字段中,否则，复制值
                    for (int k = 0; k < namesTemp.Length; k++)
                    {
                        if (namesTemp[k] == name)
                        {
                            PropertyValues[i] = valuesTemp[k];
                            changedlist[i] = changesTemp[k];
                            break;
                        }
                    }
                }
            }
            if (!setValueFlag)
            { 
                //要赋值的字段不在实体类定义的字段名中，抛出异常
                throw new ArgumentException("属性字段名称 [" + propertyFieldName + "] 无效，请检查实体类的当前属性定义和重载的SetFieldNames 方法中对PropertyNames 的设置。");
            }
            //原来的代码已经优化
          
        }
        
        #endregion

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

        /// <summary>
        /// 获取实体类属性改变过的属性名称值对象
        /// </summary>
        /// <returns></returns>
        public PropertyNameValues GetChangedValues()
        {
            PropertyNameValues result = new PropertyNameValues();
            List<string> names = new List<string>();
            List<object> values = new List<object>();
            for (int i = 0; i < this.PropertyNames.Length; i++)
            {
                if (this.changedlist[i])
                {
                    names.Add(this.PropertyNames[i]);
                    values.Add(this.PropertyValues[i]);                
                }
            }

            result.PropertyNames = names.ToArray();
            result.PropertyValues = values.ToArray();
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
            protected internal set {
                //重新设置实体类的值得时候，重只修改状态 感谢网友 Panke 发现此Bug
                changedlist = new bool[names.Length];
                values = value; 
            }
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

        #region 索引器
        /// <summary>
        /// 获取或者设置指定属性名称的值，属性名必须是一个PDF.NET实体类的属性（调用了getProperty 和 setProperty方法），不能是普通属性。
        /// 如果属性不存在，获取该属性值将为null，而设置该属性值将抛出异常。
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
                        Type fieldType = ef.GetPropertyType(fieldName);
                        if (value.GetType() != fieldType)
                            throw new ArgumentException("实体类的属性字段" + propertyName + " 需要"
                                + fieldType.Name + " 类型的值，但准备赋予的值不是该类型！");
                    }
                    this.setProperty(fieldName, value);
                }
                else
                {
                    //设置虚拟的字段值
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

        #region 外键处理
        /// <summary>
        /// 设置对应于父实体类的外键字段名称
        /// </summary>
        /// <typeparam name="Parent">父实体类</typeparam>
        /// <param name="fieldName">外键字段名称，必须是当前实体类使用的一个字段</param>
      protected internal void SetForeignKey<Parent>(string fieldName) where Parent:EntityBase,new()
        {
            Parent p = new Parent();
            this.foreignKeys += "," + fieldName+"@"+p.GetTableName();
        }

       /// <summary>
       /// 获取对应的父实体类的外键字段名称，如果没有，返回空字符串
       /// </summary>
      /// <typeparam name="Parent">父实体类</typeparam>
      /// <returns>外键字段名称，必须是当前实体类使用的一个字段</returns>
      public string GetForeignKey<Parent>() where Parent : EntityBase, new()
        {
            Parent p = new Parent();
            string tableName = p.GetTableName();
            foreach (string str in this.foreignKeys.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries))
            {
                string[] arr = str.Split('@');
                if (arr[1] == tableName)
                    return arr[0];
            }
            return "";
        }

        #endregion

        #region 实体类跟POCO类的相互映射

      /// <summary>
        /// 从POCO实体类获取跟当前实体类的属性名称相同的属性的值，拷贝到当前实体类中，完成数据的映射。
        /// 要求拷贝的同名属性是读写属性且类型相同。
        /// </summary>
        /// <param name="pocoClass">POCO实体类，提供源数据</param>
        /// <param name="isChange">是否改变属性的修改状态</param>
        /// <returns>映射成功的属性数量</returns>
        public int MapFrom(object pocoClass,bool isChange)
        {
          if (pocoClass == null)
              return 0;
          int count = 0;
          int fcount=this.PropertyNames.Length;
          INamedMemberAccessor[] accessors = new INamedMemberAccessor[fcount];
          DelegatedReflectionMemberAccessor drm = new DelegatedReflectionMemberAccessor();
          Type type = pocoClass.GetType();
          var ef= EntityFieldsCache.Item(this.GetType());

          for (int i = 0; i < fcount; i++)
          {
              //实体类的属性字段可能跟属性名称不一样 edit at 2015.2.11
              string perpertyName= ef.GetPropertyName(PropertyNames[i]);
              accessors[i] = drm.TryFindAccessor(type, perpertyName);
          }
          for (int i = 0; i < fcount; i++)
          {
              if (accessors[i] != null)
              {
                  this.PropertyValues[i] = accessors[i].GetValue(pocoClass);
                  if (isChange) //设置属性修改状态
                      this.changedlist[i] = true; 
                  count++;              
              }
          }
          return count;
        }

        /// <summary>
        /// 将当前实体类的属性值（非空值）映射到相同属性名称的POCO实体类中。要求拷贝的同名属性是读写属性且类型相同。
        /// </summary>
        /// <param name="pocoClass">POCO实体类</param>
        /// <returns>映射成功的属性数量</returns>
        public int MapToPOCO(object pocoClass)
        {
            if (pocoClass == null)
                return 0;
            int count = 0;
            int fcount = this.PropertyNames.Length;
            INamedMemberAccessor[] accessors = new INamedMemberAccessor[fcount];
            DelegatedReflectionMemberAccessor drm = new DelegatedReflectionMemberAccessor();
            Type type = pocoClass.GetType();
            var ef = EntityFieldsCache.Item(this.GetType());
            for (int i = 0; i < fcount; i++)
            {
                //实体类的属性字段可能跟属性名称不一样 edit at 2015.10.24
                string perpertyName = ef.GetPropertyName(PropertyNames[i]);
                accessors[i] = drm.TryFindAccessor(type, perpertyName);
            }
            for (int i = 0; i < fcount; i++)
            {
                if (accessors[i] != null)
                {
                    //this.PropertyValues[i] 可能为空，这会导致下面的赋值报错
                    //这种情况可能发生在实体类的数据是经过OQL部分Select 字段造成的，或者字段本身的值为NULL
                    object Value = this.PropertyValues[i];
                    if (Value != null && Value != DBNull.Value)
                    {
                        accessors[i].SetValue(pocoClass, this.PropertyValues[i]);
                        count++;
                    }
                }
            }
            return count;
        }

      #endregion

    }
}
