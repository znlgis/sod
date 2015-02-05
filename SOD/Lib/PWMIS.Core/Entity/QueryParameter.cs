using System;
using System.Collections.Generic;
using System.Text;
using PWMIS.Common;

namespace PWMIS.DataMap.Entity
{
    /// <summary>
    /// 查询参数类
    /// </summary>
    public  class QueryParameter
    {
        /// <summary>
        /// 字段名称
        /// </summary>
       public  string FieldName { get; set; }
        /// <summary>
        /// 字段值
        /// </summary>
       public object FieldValue { get; set; }
        /// <summary>
        /// 比较类型
        /// </summary>
       public enumCompare CompareType { get; set; }
        /// <summary>
        /// 默认构造函数
        /// </summary>
       public QueryParameter()
       { 
       
       }

        /// <summary>
        /// 使用参数构造本类
        /// </summary>
        /// <param name="filedName">字段名</param>
        /// <param name="compareType">比较类型</param>
        /// <param name="fieldValue">字段值</param>
       public QueryParameter(string filedName, enumCompare compareType, object fieldValue)
       {
           this.FieldName = filedName;
           this.FieldValue = fieldValue;
           this.CompareType = compareType;
       }

        /// <summary>
        /// 使用比较字符串构造本类
        /// </summary>
        /// <param name="filedName">字段名</param>
        /// <param name="compareTypeString">比较字符串，比如=，like，is 等SQL比较符号</param>
        /// <param name="fieldValue">要比较的值</param>
       public QueryParameter(string filedName, string compareTypeString, object fieldValue)
       {
           this.FieldName = filedName;
           this.FieldValue = fieldValue;
           switch (compareTypeString.Trim ())
           {
               case "=": this.CompareType = enumCompare.Equal; break;
               case "<>": this.CompareType = enumCompare.NotEqual; break;
               case ">": this.CompareType = enumCompare.Greater; break;
               case "<": this.CompareType = enumCompare.Smaller ; break;
               case ">=": this.CompareType = enumCompare.NoSmaller ; break;
               case "<=": this.CompareType = enumCompare.NoGreater ; break;
               case "like":
               case "LIKE":
               this.CompareType = enumCompare.Like ; break;
               case "is null":
               case "IS NULL":
                   this.CompareType = enumCompare.IsNull ; break;
               case "is not null":
               case "IS NOT NULL":
                   this.CompareType = enumCompare.IsNotNull ; break;
               default :
                   this.CompareType = enumCompare.Equal; break;
                   
           }
           
       }
    }

    /// <summary>
    /// 查询参数泛型类  QueryParameter;
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// //实体类 Customers
    /// Northwind.Customers cm = new Northwind.Customers();
    /// cm.Country="中国";
    /// ....
    /// 
    /// QueryParameter<Northwind.Customers> qp = new QueryParameter<Northwind.Customers>(cm);
    /// 
    /// QueryParameter para1=qp.CreatePrameter(cm.Country)
    /// QueryParameter para2=qp.CreatePrameter(cm.City, enumCompare.Like, "%" + txtCity.Text + "%")
    /// 
    /// QueryParameter[] paras={para1,para2};
    /// 
    /// OQL q = OQL.From(cm).Select().Where(queryParas).END;
    /// 
    /// //获取查询结果列表
    /// return  EntityQuery&lt;Northwind.Customers&gt;.QueryList(q);
    /// ]]>
    /// </code>
    /// </example>
    /// <typeparam name="T"></typeparam>
    public class QueryParameter<T> where T : EntityBase
    {
        private EntityBase currEntity = null;
        private string currPropName = null;

        /// <summary>
        /// 以一个实体类初始化本类
        /// </summary>
        /// <param name="entity">实体类实例</param>
        public QueryParameter(EntityBase entity)
        {
            this.currEntity = entity;
            this.currEntity.PropertyGetting += new EventHandler<PropertyGettingEventArgs>(currEntity_PropertyGetting);
                         
        }

        void currEntity_PropertyGetting(object sender, PropertyGettingEventArgs e)
        {
            this.currPropName = e.PropertyName;
        }
        /// <summary>
        /// 构造实体查询参数
        /// </summary>
        /// <param name="entityProperty">实体类的属性</param>
        /// <param name="compareType">要比较的类型枚举</param>
        /// <param name="fieldValue">要比较的值</param>
        /// <returns>实体查询参数</returns>
        public QueryParameter CreatePrameter(object entityProperty, enumCompare compareType, object fieldValue)
        {
            return new QueryParameter(this.currPropName, compareType, fieldValue);
        }
        /// <summary>
        /// 构造实体查询参数，指定要比较的类型，以当前实体属性的值为比较的值。
        /// </summary>
        /// <param name="entityProperty">实体类的属性</param>
        /// <param name="compareType">要比较的类型枚举</param>
        /// <returns>实体查询参数</returns>
        public QueryParameter CreatePrameter(object entityProperty, enumCompare compareType)
        {
            return new QueryParameter(this.currPropName, compareType, entityProperty);
        }
        /// <summary>
        /// 构造实体查询参数，将以“等于”为比较条件，以当前实体属性的值为比较的值。
        /// </summary>
        /// <param name="entityProperty">实体类的属性</param>
        /// <returns>实体查询参数</returns>
        public QueryParameter CreatePrameter(object entityProperty)
        {
            return new QueryParameter(this.currPropName, enumCompare.Equal, entityProperty);
        }
        /// <summary>
        /// 构造实体查询参数
        /// </summary>
        /// <param name="entityProperty">实体类的属性</param>
        /// <param name="compareTypeString">要比较的SQL 条件比较字符串。</param>
        /// <param name="fieldValue">要比较的值</param>
        /// <returns>实体查询参数</returns>
        public QueryParameter CreatePrameter(object entityProperty, string compareTypeString, object fieldValue)
        {
            return new QueryParameter(this.currPropName, compareTypeString, fieldValue);
        }
    }
}
