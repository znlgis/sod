using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace PWMIS.DataMap.Entity
{
    /// <summary>
    /// 存储实体类的全局字段信息，以一种更为方便的方式访问实体类属性和对应的表字段
    /// </summary>
    public class EntityFields
    {
        private string currPropName = null;
        private string[] fields = null;
        private string[] propertyNames = null;
        private Type[] typeNames = null;
        private string tableName = null;

        /// <summary>
        /// 获取实体类对应的表字段名称数组
        /// </summary>
        public string[] Fields
        {
            get { return fields; }
        }

        /// <summary>
        /// 获取实体属性名称数组
        /// </summary>
        public string[] PropertyNames
        {
            get { return propertyNames; }
        }

        /// <summary>
        /// 获取实体类对应的表名称
        /// </summary>
        public string TableName
        {
            get { return tableName; }
        }
        /// <summary>
        /// 获取实体属性的类型
        /// </summary>
        public Type[] PropertyType
        {
            get { return typeNames; }
        }

        /// <summary>
        /// 根据字段名称获取对应的实体属性类型
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public Type GetPropertyType(string fieldName)
        {
            if (fields != null && PropertyType!=null)
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    if (fields[i] == fieldName)
                    {
                        return PropertyType[i];
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 获取属性名对应的字段名
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public string GetPropertyField(string propertyName)
        {
            if (propertyNames != null && fields != null)
            {
                for (int i = 0; i < propertyNames.Length; i++)
                {
                    if (propertyNames[i] == propertyName)
                    {
                        return fields[i];
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 初始化实体信息
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public bool Init(Type entityType)
        {
            EntityBase entity = Activator.CreateInstance(entityType) as EntityBase;
            if (entity != null)
            {
                entity.PropertyGetting += new EventHandler<PropertyGettingEventArgs>(entity_PropertyGetting);
                int count = entity.PropertyNames.Length;
                this.fields = new string[count];
                this.propertyNames = new string[count];
                this.typeNames = new Type[count];
                this.tableName = entity.TableName;

                PropertyInfo[] propertys = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                count = 0;

                for (int i = 0; i < propertys.Length; i++)
                {
                    this.currPropName = null;
                    propertys[i].GetValue(entity, null);//获取属性，引发事件
                    if (this.currPropName != null)
                    {
                        //如果在分布类中引用了原来的属性，currPropName 可能会有重复
                        bool flag = false;
                        foreach (string str in fields)
                        {
                            if (str == this.currPropName)
                            {
                                flag = true;
                                break;                            
                            }
                        }
                        if (!flag)
                        {
                            fields[count] = this.currPropName;       //获得调用的字段名称
                            propertyNames[count] = propertys[i].Name;//获得调用的实体类属性名称
                            typeNames[count] = propertys[i].PropertyType;
                            count++;
                        }
                        
                    }
                }
                return true;
            }
            return false;
        }

        void entity_PropertyGetting(object sender, PropertyGettingEventArgs e)
        {
            this.currPropName = e.PropertyName;
        }
    }

    /// <summary>
    /// 实体字段缓存
    /// </summary>
    public class EntityFieldsCache
    {
        private static Dictionary<string, EntityFields> dict = new Dictionary<string, EntityFields>();
        /// <summary>
        /// 获取缓存项，如果没有，将自动创建一个
        /// </summary>
        /// <param name="entityType">实体类类型</param>
        /// <returns></returns>
        public static EntityFields Item(Type entityType)
        {
            if (dict.ContainsKey(entityType.FullName))
                return dict[entityType.FullName];

            EntityFields ef = new EntityFields();
            if (ef.Init(entityType))
                dict.Add(entityType.FullName, ef);
            return ef;
        }
    }
}
