using PWMIS.Common;

namespace PWMIS.DataMap.Entity
{
    /// <summary>
    ///     查询参数类
    /// </summary>
    public class QueryParameter
    {
        /// <summary>
        ///     默认构造函数
        /// </summary>
        public QueryParameter()
        {
        }

        /// <summary>
        ///     使用参数构造本类
        /// </summary>
        /// <param name="filedName">字段名</param>
        /// <param name="compareType">比较类型</param>
        /// <param name="fieldValue">字段值</param>
        public QueryParameter(string filedName, enumCompare compareType, object fieldValue)
        {
            FieldName = filedName;
            FieldValue = fieldValue;
            CompareType = compareType;
        }

        /// <summary>
        ///     使用比较字符串构造本类
        /// </summary>
        /// <param name="filedName">字段名</param>
        /// <param name="compareTypeString">比较字符串，比如=，like，is 等SQL比较符号</param>
        /// <param name="fieldValue">要比较的值</param>
        public QueryParameter(string filedName, string compareTypeString, object fieldValue)
        {
            FieldName = filedName;
            FieldValue = fieldValue;
            switch (compareTypeString.Trim().ToLower())
            {
                case "=":
                    CompareType = enumCompare.Equal;
                    break;
                case "<>":
                    CompareType = enumCompare.NotEqual;
                    break;
                case ">":
                    CompareType = enumCompare.Greater;
                    break;
                case "<":
                    CompareType = enumCompare.Smaller;
                    break;
                case ">=":
                    CompareType = enumCompare.NoSmaller;
                    break;
                case "<=":
                    CompareType = enumCompare.NoGreater;
                    break;
                case "like":
                    CompareType = enumCompare.Like;
                    break;
                case "is null":
                    CompareType = enumCompare.IsNull;
                    break;
                case "is not null":
                    CompareType = enumCompare.IsNotNull;
                    break;
                case "in":
                    CompareType = enumCompare.IN;
                    break;
                default:
                    CompareType = enumCompare.Equal;
                    break;
            }
        }

        /// <summary>
        ///     字段名称
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        ///     字段值
        /// </summary>
        public object FieldValue { get; set; }

        /// <summary>
        ///     比较类型
        /// </summary>
        public enumCompare CompareType { get; set; }
    }

    /// <summary>
    ///     查询参数泛型类  QueryParameter;
    /// </summary>
    /// <example>
    ///     <code>
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
        private readonly EntityBase currEntity;
        private string currPropName;

        /// <summary>
        ///     以一个实体类初始化本类
        /// </summary>
        /// <param name="entity">实体类实例</param>
        public QueryParameter(EntityBase entity)
        {
            currEntity = entity;
            currEntity.PropertyGetting += currEntity_PropertyGetting;
        }

        private void currEntity_PropertyGetting(object sender, PropertyGettingEventArgs e)
        {
            currPropName = e.PropertyName;
        }

        /// <summary>
        ///     构造实体查询参数
        /// </summary>
        /// <param name="entityProperty">实体类的属性</param>
        /// <param name="compareType">要比较的类型枚举</param>
        /// <param name="fieldValue">要比较的值</param>
        /// <returns>实体查询参数</returns>
        public QueryParameter CreatePrameter(object entityProperty, enumCompare compareType, object fieldValue)
        {
            return new QueryParameter(currPropName, compareType, fieldValue);
        }

        /// <summary>
        ///     构造实体查询参数，指定要比较的类型，以当前实体属性的值为比较的值。
        /// </summary>
        /// <param name="entityProperty">实体类的属性</param>
        /// <param name="compareType">要比较的类型枚举</param>
        /// <returns>实体查询参数</returns>
        public QueryParameter CreatePrameter(object entityProperty, enumCompare compareType)
        {
            return new QueryParameter(currPropName, compareType, entityProperty);
        }

        /// <summary>
        ///     构造实体查询参数，将以“等于”为比较条件，以当前实体属性的值为比较的值。
        /// </summary>
        /// <param name="entityProperty">实体类的属性</param>
        /// <returns>实体查询参数</returns>
        public QueryParameter CreatePrameter(object entityProperty)
        {
            return new QueryParameter(currPropName, enumCompare.Equal, entityProperty);
        }

        /// <summary>
        ///     构造实体查询参数
        /// </summary>
        /// <param name="entityProperty">实体类的属性</param>
        /// <param name="compareTypeString">要比较的SQL 条件比较字符串。</param>
        /// <param name="fieldValue">要比较的值</param>
        /// <returns>实体查询参数</returns>
        public QueryParameter CreatePrameter(object entityProperty, string compareTypeString, object fieldValue)
        {
            return new QueryParameter(currPropName, compareTypeString, fieldValue);
        }
    }
}