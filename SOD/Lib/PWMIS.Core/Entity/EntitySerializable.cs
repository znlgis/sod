using System;
using System.Collections.Generic;
using System.Text;
using PWMIS.Core;

namespace PWMIS.DataMap.Entity
{
    /// <summary>
    /// 实体类可以序列化的辅助类,该类已经过时
    /// </summary>
    [Obsolete("请使用PropertyNameValuesSerializer 类替代")]
    public class EntitySerializable
    {
        protected internal string[] PropertyNames { 
            get {
                if (this.nameValues == null)
                    throw new Exception("在获取或者设置属性前，请先确定已经调用过一次 SetNameValues 方法。");
                return this.nameValues.PropertyNames;
            } 
        }

        protected internal object[] PropertyValues { 
            get {
                if (this.nameValues == null)
                    throw new Exception("在获取或者设置属性前，请先确定已经调用过一次 SetNameValues 方法。");
                return this.nameValues.PropertyValues; 
            } 
        }

        private PropertyNameValues nameValues;

        public EntitySerializable() {
           
        }


        public void SetNameValues(PropertyNameValues prop)
        {
            this.nameValues = prop;
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

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="propertyName">属性字段名称</param>
        /// <param name="t">属性类型代码</param>
        /// <returns>属性值</returns>
        protected object getProperty(string propertyName, TypeCode t)
        {
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
            if (Value != null && maxLength > 0 && Value.Length > maxLength)
                throw new Exception("字段" + name + "的实际长度超出了最大长度" + maxLength);
            else
                setProperty(name, Value);
        }
    }
}
