/*
 * ========================================================================
 * Copyright(c) 2006-2015 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap 
 * ========================================================================
 * PDF.NET 数据开发框架
 * http://www.pwmis.com/sqlmap
 * 
 * 详细内容，请参看“打造轻量级的实体类数据容器”
 * （ http://www.cnblogs.com/bluedoctor/archive/2011/05/23/2054541.html）
 * 
 * 作者：邓太华     时间：2008-10-12
 * 版本：V5.1.2
 * 
 * 修改者：         时间：2015-2-5                
 * 修改说明：修复实体类有多个普通属性（即POCO属性）的时候，获取实体类数据库元数据不正确的问题。 
 * 
 * 
 * ========================================================================
*/

using System;
using System.Collections.Generic;
using System.Reflection;
using PWMIS.Common;
using PWMIS.DataProvider.Data;

namespace PWMIS.DataMap.Entity
{
    /// <summary>
    ///     存储实体类的全局字段信息，以一种更为方便的方式访问实体类属性和对应的表字段
    /// </summary>
    public class EntityFields
    {
        private string currPropName;

        public EntityFields()
        {
            PropertyType = null;
            TableName = null;
            PropertyNames = null;
            Fields = null;
        }

        /// <summary>
        ///     获取实体类对应的表字段名称数组
        /// </summary>
        public string[] Fields { get; private set; }

        /// <summary>
        ///     获取实体属性名称数组
        /// </summary>
        public string[] PropertyNames { get; private set; }

        /// <summary>
        ///     获取实体类对应的表名称
        /// </summary>
        public string TableName { get; private set; }

        /// <summary>
        ///     获取实体属性的类型
        /// </summary>
        public Type[] PropertyType { get; private set; }

        /// <summary>
        ///     根据字段名获取对应的属性名
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public string GetPropertyName(string fieldName)
        {
            if (Fields != null && PropertyNames != null)
            {
                for (var i = 0; i < Fields.Length; i++)
                {
                    if (Fields[i] == fieldName)
                    {
                        return PropertyNames[i];
                    }
                }
            }
            return null;
        }

        /// <summary>
        ///     根据字段名称获取对应的实体属性类型
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public Type GetPropertyType(string fieldName)
        {
            if (Fields != null && PropertyType != null)
            {
                for (var i = 0; i < Fields.Length; i++)
                {
                    if (Fields[i] == fieldName)
                    {
                        return PropertyType[i];
                    }
                }
            }
            return null;
        }

        /// <summary>
        ///     获取属性名对应的字段名
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public string GetPropertyField(string propertyName)
        {
            if (PropertyNames != null && Fields != null)
            {
                for (var i = 0; i < PropertyNames.Length; i++)
                {
                    if (PropertyNames[i] == propertyName)
                    {
                        return Fields[i];
                    }
                }
            }
            return null;
        }

        /// <summary>
        ///     初始化实体信息（已经过时）
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public bool Init(Type entityType)
        {
            //未来版本，考虑不以EntityBase 明确类型来操作，避免在VS设计器无法类型转换到父类的问题
            var entity = Activator.CreateInstance(entityType) as EntityBase;
            if (entity != null)
            {
                entity.PropertyGetting += entity_PropertyGetting;
                var count = entity.PropertyNames.Length;
                Fields = new string[count];
                PropertyNames = new string[count];
                PropertyType = new Type[count];
                TableName = entity.TableName;

                var propertys =
                    entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                count = 0;

                for (var i = 0; i < propertys.Length; i++)
                {
                    currPropName = null;
                    try
                    {
                        propertys[i].GetValue(entity, null); //获取属性，引发事件
                    }
                    catch
                    {
                        currPropName = null;
                    }

                    if (currPropName != null)
                    {
                        //如果在分布类中引用了原来的属性，currPropName 可能会有重复
                        var flag = false;
                        foreach (var str in Fields)
                        {
                            if (str == currPropName)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            Fields[count] = currPropName; //获得调用的字段名称
                            PropertyNames[count] = propertys[i].Name; //获得调用的实体类属性名称
                            PropertyType[count] = propertys[i].PropertyType;
                            try
                            {
                                //这里需要设置属性，以便获取字段长度
                                object Value = null; // 感谢网友 stdbool 发现byte[] 判断的问题
                                if (PropertyType[count] != typeof (string) && PropertyType[count] != typeof (byte[]))
                                    Value = Activator.CreateInstance(PropertyType[count]);
                                propertys[i].SetValue(entity, Value, null);
                            }
                            catch
                            {
                            }
                            count++;
                        }
                    }
                }
                return true;
            }
            return false;
        }

        public bool InitEntity(Type entityType)
        {
            //未来版本，考虑不以EntityBase 明确类型来操作，避免在VS设计器无法类型转换到父类的问题
            var entity = Activator.CreateInstance(entityType);

            if (entityType.BaseType.FullName == "PWMIS.DataMap.Entity.EntityBase")
            {
                TableName = (string) entityType.GetMethod("GetTableName").Invoke(entity, null);

                var methodInfo = entityType.GetMethod("GetSetPropertyFieldName");
                var testMethodInfo = entityType.GetMethod("TestWriteProperty",
                    BindingFlags.Instance | BindingFlags.NonPublic);
                testMethodInfo.Invoke(entity, null); //设置虚拟属性写入标记

                var propertys =
                    entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                //下面的方式弃用 dth 2015.2.8
                //int count = propertys.Length;
                //this.fields = new string[count];
                //this.propertyNames = new string[count];
                //this.typeNames = new Type[count];

                var fieldList = new List<string>();
                var propertyNameList = new List<string>();
                var typeNameList = new List<Type>();

                //count = 0;
                var last_field = string.Empty;

                for (var i = 0; i < propertys.Length; i++)
                {
                    //获得调用的字段名称
                    //propertyNames[count] = propertys[i].Name;//获得调用的实体类属性名称
                    //typeNames[count] = propertys[i].PropertyType;
                    var currPropType = propertys[i].PropertyType;

                    if (!propertys[i].CanWrite) //只读属性，跳过
                    {
                        continue;
                    }
                    try
                    {
                        //这里需要设置属性，以便获取字段长度
                        object Value = null; // 感谢网友 stdbool 发现byte[] 判断的问题
                        if (currPropType != typeof (string) && currPropType != typeof (byte[]))
                            Value = Activator.CreateInstance(currPropType);
                        propertys[i].SetValue(entity, Value, null); //这里可能有普通属性在被赋值 
                        var field = (string) methodInfo.Invoke(entity, null);
                        if (last_field != field)
                        {
                            //跟之前的对比，确定当前是属性字段对应的属性
                            //fields[count] = field;
                            fieldList.Add(field);
                            propertyNameList.Add(propertys[i].Name);
                            typeNameList.Add(currPropType);

                            last_field = field;
                        }
                    }
                    catch
                    {
                        //return false;
                    }
                    //count++;
                }
                Fields = fieldList.ToArray();
                PropertyNames = propertyNameList.ToArray();
                PropertyType = typeNameList.ToArray();

                return true;
            }
            return false;
        }

        private void entity_PropertyGetting(object sender, PropertyGettingEventArgs e)
        {
            currPropName = e.PropertyName;
        }

        /// <summary>
        ///     为实体类的一个属性创建对应的数据库表的列的脚本
        /// </summary>
        /// <param name="db"></param>
        /// <param name="entity"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public string CreateTableColumnScript(AdoHelper db, EntityBase entity, string field)
        {
            var t = GetPropertyType(field);
            object defaultValue = null;
            if (t == typeof (string))
                defaultValue = "";
            else
                defaultValue = Activator.CreateInstance(t);

            var para = db.GetParameter(field, defaultValue);
            //需要再获取参数长度

            var temp = "[" + field + "] " + db.GetNativeDbTypeName(para);
            if (t == typeof (string))
            {
                temp = temp + "(" + entity.GetStringFieldSize(field) + ")";
            }
            //identity(1,1) primary key
            if (entity.PrimaryKeys.Contains(field))
            {
                temp = temp + " PRIMARY KEY";
            }
            if (field == entity.IdentityName)
            {
                if (db.CurrentDBMSType == DBMSType.SqlServer || db.CurrentDBMSType == DBMSType.SqlServerCe)
                {
                    temp = temp + " IDENTITY(1,1)";
                }
                else if (db.CurrentDBMSType == DBMSType.Access && entity.PrimaryKeys.Contains(field))
                {
                    temp = "[" + field + "] " + " autoincrement PRIMARY KEY ";
                }
                else
                {
                    if (db.CurrentDBMSType == DBMSType.SQLite)
                        temp = temp + " autoincrement";
                }
            }
            return db.GetPreparedSQL(temp);
        }
    }

    /// <summary>
    ///     实体字段缓存
    /// </summary>
    public class EntityFieldsCache
    {
        private static readonly Dictionary<string, EntityFields> dict = new Dictionary<string, EntityFields>();

        /// <summary>
        ///     获取缓存项，如果没有，将自动创建一个
        /// </summary>
        /// <param name="entityType">实体类类型</param>
        /// <returns></returns>
        public static EntityFields Item(Type entityType)
        {
            if (dict.ContainsKey(entityType.FullName))
                return dict[entityType.FullName];

            var ef = new EntityFields();
            if (ef.InitEntity(entityType)) //2015.2.5 修改
                dict.Add(entityType.FullName, ef);
            return ef;
        }
    }
}