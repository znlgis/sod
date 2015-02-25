using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace PWMIS.DataMap.Entity
{
    /// <summary>
    /// 属性名值对对象
    /// </summary>
    
    public class PropertyNameValues
    {
       
        public string[] PropertyNames { get; set; }

        public object[] PropertyValues { get; set; }

        

    }

    /// <summary>
    /// 属性名值对序列化工具类 
    /// </summary>
    public class PropertyNameValuesSerializer
    {
        public PropertyNameValuesSerializer(PropertyNameValues nameValues)
        {
            this.CurrNameValues = nameValues;
        }
        public PropertyNameValues CurrNameValues { get; private set; }

        public string Serializer()
        {
            StringBuilder sb = new StringBuilder();
            XmlWriter xw = XmlWriter.Create(sb);
            XmlSerializer xs = new XmlSerializer(typeof(PropertyNameValues));
            xs.Serialize(xw, CurrNameValues);
            string strEntity = sb.ToString();
            return strEntity;
        }

        public T Deserialize<T>(string input) where T : EntityBase, new()
        {
            XmlSerializer xs = new XmlSerializer(typeof(PropertyNameValues));
            //XmlReader xw2 = XmlReader.Create(new System.IO.StringReader(strEntity));
            var desObj = xs.Deserialize(new System.IO.StringReader(input));
            this.CurrNameValues = desObj as PropertyNameValues;
            T des = CreateEntity<T>();
            return des;
        }

        /// <summary>
        /// 根据当前类，构造一个新的实体类，注意当前类的属性和值必须跟要构造的实体类匹配，否则将引发异常
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <returns>实体类</returns>
        public T CreateEntity<T>() where T : EntityBase, new()
        {
            if (CurrNameValues.PropertyNames == null)
                throw new FormatException("属性名称数组不能为空！");
            if (CurrNameValues.PropertyValues == null)
                throw new FormatException("属性值数组不能为空！");
            if (CurrNameValues.PropertyNames.Length != CurrNameValues.PropertyValues.Length)
                throw new FormatException("属性名和值数组元素数量不匹配");
            T entity = new T();
            for (int i = 0; i < CurrNameValues.PropertyNames.Length; i++)
            {
                string name = CurrNameValues.PropertyNames[i];
                entity[name] = CurrNameValues.PropertyValues[i];
            }

            return entity;
        }

        /// <summary>
        /// 填充实体类
        /// </summary>
        /// <param name="entity"></param>
        public void FillEntity(EntityBase entity)
        {
            if (CurrNameValues.PropertyNames == null)
                throw new FormatException("属性名称数组不能为空！");
            if (CurrNameValues.PropertyValues == null)
                throw new FormatException("属性值数组不能为空！");
            if (CurrNameValues.PropertyNames.Length != CurrNameValues.PropertyValues.Length)
                throw new FormatException("属性名和值数组元素数量不匹配");

            for (int i = 0; i < CurrNameValues.PropertyNames.Length; i++)
            {
                string name = CurrNameValues.PropertyNames[i];
                entity[name] = CurrNameValues.PropertyValues[i];
            }

        }
    }
}
