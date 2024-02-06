﻿/*
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
 * 修改者：广州-玄离       时间：2015-2-28                
 * 修改说明：解决实体类属性字段长度未定义，需要生成text （SqlServer是varchar(max)）字段类型的问题。
 * 
 * 修改者：      时间：2015-6-3   
 * 修改说明：增加 GetPropertyFieldSize，获取属性字段的长度
 * ========================================================================
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using PWMIS.DataProvider.Data;
using System.Data;
using System.Data.SqlClient;
using PWMIS.Common;

namespace PWMIS.DataMap.Entity
{
    /// <summary>
    /// 简单字段信息结构
    /// </summary>
    public struct SimplyField
    {
        public SimplyField(int length,DbType dbType)
        {
            this.FieldDbType = dbType;
            this.FieldLength = length;
        }
        public int FieldLength;
        public DbType FieldDbType;
    }
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
        private Type entityType = null;//当前实体类类型
        private PropertyInfo currPropertyInfo = null;

        private List<string> fieldList = null;
        private List<string> propertyNameList = null;
        private List<Type> typeNameList = null;
        /// <summary>
        /// 获取实体类对应的表字段名称数组
        /// </summary>
        public string[] Fields
        {
            get { return fields; }
        }

        /* 此功能很少使用，移除
         * 
        /// <summary>
        /// 获取或者设置字段对应的描述，跟 Fields相对应，优先采用实体类自身的定义
        /// </summary>
        public string[] FieldDescriptions
        {
            get {
                if (fieldDescriptions == null)
                {
                     EntityBase   entity = Activator.CreateInstance(this.entityType) as EntityBase;
                     fieldDescriptions = entity.SetFieldDescriptions();
                }
                return fieldDescriptions; 
            }
            set { fieldDescriptions = value; }
        }
        */

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
        /// 根据字段名获取对应的属性名
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public string GetPropertyName(string fieldName)
        {
            if (fields != null && propertyNames != null)
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    if (fields[i] == fieldName)
                    {
                        return propertyNames[i];
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 根据字段名称获取对应的实体属性类型
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public Type GetPropertyType(string fieldName)
        {
            if (fields != null && PropertyType != null)
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
        /// 根据实体类内部的属性字段名称，获取对应的数据库字段长度
        /// </summary>
        /// <param name="fieldName">属性字段名称</param>
        /// <param name="entity">要访问的实体类对象</param>
        /// <returns></returns>
        public SimplyField GetPropertyFieldSize(string fieldName, EntityBase entity)
        {
            if(entity==null)
                entity = Activator.CreateInstance(this.entityType) as EntityBase;
            return entity.GetStringFieldSize(fieldName);
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
        /// 初始化实体信息（已经过时）
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public bool Init(Type entityType)
        {
            //未来版本，考虑不以EntityBase 明确类型来操作，避免在VS设计器无法类型转换到父类的问题
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
                    try
                    {
                        propertys[i].GetValue(entity, null);//获取属性，引发事件
                    }
                    catch
                    {
                        this.currPropName = null;
                    }

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
                            try
                            {
                                //这里需要设置属性，以便获取字段长度
                                object Value = null;// 感谢网友 stdbool 发现byte[] 判断的问题
                                if (typeNames[count] != typeof(string) && typeNames[count] != typeof(byte[]))
                                    Value = Activator.CreateInstance(typeNames[count]);
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

        /// <summary>
        /// 初始化实体类信息，必须确保单线程调用本方法
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public bool InitEntity(Type entityType)
        {
            //未来版本，考虑不以EntityBase 明确类型来操作，避免在VS设计器无法类型转换到父类的问题
            object entity = Activator.CreateInstance(entityType) ;

            if (entityType.BaseType.FullName == "PWMIS.DataMap.Entity.EntityBase")
            {
                EntityBase instance = entity as EntityBase;
                if (instance != null)
                {
                    instance.PropertyChanging += instance_PropertyChanging; 
                }

                this.tableName = (string)entityType.GetMethod("GetTableName").Invoke(entity, null);

                //var methodInfo = entityType.GetMethod("GetSetPropertyFieldName");
                //var testMethodInfo = entityType.GetMethod("TestWriteProperty", BindingFlags.Instance | BindingFlags.NonPublic);
                //testMethodInfo.Invoke(entity, null);//设置虚拟属性写入标记

                PropertyInfo[] propertys = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                //下面的方式弃用 dth 2015.2.8
                //int count = propertys.Length;
                //this.fields = new string[count];
                //this.propertyNames = new string[count];
                //this.typeNames = new Type[count];

                fieldList = new List<string>();
                propertyNameList = new List<string>();
                typeNameList = new List<Type>();

                //count = 0;
                string last_field = string.Empty;

                for (int i = 0; i < propertys.Length; i++)
                {
                    //获得调用的字段名称
                    //propertyNames[count] = propertys[i].Name;//获得调用的实体类属性名称
                    //typeNames[count] = propertys[i].PropertyType;
                    Type currPropType = propertys[i].PropertyType;

                    if (!propertys[i].CanWrite) //只读属性，跳过
                    {
                        
                        continue;
                    }
                    try
                    {
                        currPropertyInfo = propertys[i];
                        //这里需要设置属性，以便获取字段长度
                        object Value = null;// 感谢网友 stdbool 发现byte[] 判断的问题
                        if (currPropType != typeof(string) && currPropType != typeof(byte[]))
                            Value = Activator.CreateInstance(currPropType);
                        currPropertyInfo.SetValue(entity, Value, null); //这里可能有普通属性在被赋值 
                        //string field= (string)methodInfo.Invoke(entity,null);
                        //if (last_field != field)
                        //{
                        //    //跟之前的对比，确定当前是属性字段对应的属性
                        //    //fields[count] = field;
                        //    fieldList.Add(field);
                        //    propertyNameList.Add(currPropertyInfo.Name);
                        //    typeNameList.Add(currPropType);

                        //    last_field = field;
                        //}
                    }
                    catch
                    {
                        //return false;
                    }
                    //count++;
                }
                this.fields = fieldList.ToArray();
                this.propertyNames = propertyNameList.ToArray();
                this.typeNames = typeNameList.ToArray();
                this.entityType = entityType;

                if (instance != null)
                {
                    instance.PropertyChanging -= instance_PropertyChanging;
                }
                return true;
            }//end if
            return false;
        }

        void instance_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            fieldList.Add(e.PropertyName);
            propertyNameList.Add(currPropertyInfo.Name);
            typeNameList.Add(currPropertyInfo.PropertyType);

            EntityBase entity = sender as EntityBase;
            entity.SetStringFieldSize(e.PropertyName, e.MaxValueLength,e.FieldDbType);
            e.IsCancel = true;
        }

        void entity_PropertyGetting(object sender, PropertyGettingEventArgs e)
        {
            this.currPropName = e.PropertyName;
        }

        /// <summary>
        /// 为实体类的一个属性创建对应的数据库表的列的脚本
        /// </summary>
        /// <param name="db"></param>
        /// <param name="entity"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public string CreateTableColumnScript(AdoHelper db, EntityBase entity, string field)
        {
            Type t = this.GetPropertyType(field);
            object defaultValue = null;
            if (t == typeof(string))
                defaultValue = "";
            else if (t == typeof(byte[]))
                defaultValue = new byte[1];//这里只是构造默认值，不需要实际的长度
            else
                defaultValue = Activator.CreateInstance(t);

            IDataParameter para = db.GetParameter(field, defaultValue);
            //需要再获取参数长度

            string temp = "";
            if (t == typeof(string))
            {
                SimplyField sf= entity.GetStringFieldSize(field);
                int length = sf.FieldLength;
                if (length == 0) //实体类未定义属性字段的长度
                {
                    string fieldType = "text";
                    if (db is SqlServer) //此处要求SqlServer 2005以上，SqlServer2000 不支持
                        fieldType = "varchar(max)";
                    if(db.CurrentDBMSType == DBMSType.SqlServerCe)
                        fieldType = "ntext";
                    temp = temp + "[" + field + "] " + fieldType;
                }
                else if (length > 0)
                {
                    //并不是所有数据库都支持nvarchar,有关数据库字符串类型的详细支持，请看 http://www.cnblogs.com/hantianwei/p/3152517.html
                    string fieldType = string.Empty;
                    if (sf.FieldDbType == DbType.String)
                        fieldType = "nvarchar";
                    else if (sf.FieldDbType == DbType.AnsiString)
                        fieldType = "varchar";
                    else if (sf.FieldDbType == DbType.AnsiStringFixedLength)
                        fieldType = "char";
                    else if (sf.FieldDbType == DbType.StringFixedLength)
                        fieldType = "nchar";
                    else
                        fieldType = "varchar";


                    if (db.CurrentDBMSType == DBMSType.Access)
                    {
                        if (fieldType == "nvarchar")
                            fieldType = "varchar";
                        else if (fieldType == "nchar")
                            fieldType = "char";
                    }
                    
                    temp = temp + "[" + field + "] " +fieldType + "(" + length + ")";
                }
                else if (length < 0)
                {
                    temp = temp + "[" + field + "] varchar" + "(" + length + ")";
                }
            }
            else if (t == typeof(byte[])) //感谢CSDN网友 ccliushou 发现此问题，原贴：http://bbs.csdn.net/topics/391967495
            {
                int length = entity.GetStringFieldSize(field).FieldLength;
                temp = temp + "[" + field + "] " + db.GetNativeDbTypeName(para);
                if (length == 0)
                {
                    temp = temp + "(max)";
                }
                else
                {
                    temp = temp + "("+length+")";
                }
            }
            else if (t == typeof(decimal))
            {
                //增加 decimal支持 时间：2017.6.28
                //decimal(n,m)
                int n = 30;
                int m = 8;
                temp = temp + "[" + field + "] decimal(" + n + "," + m + ")";
            }
            else
            {
                temp = temp + "[" + field + "] " + db.GetNativeDbTypeName(para);
            }
           
            if (field == entity.IdentityName)
            {
                if (db.CurrentDBMSType == PWMIS.Common.DBMSType.SqlServer || db.CurrentDBMSType == PWMIS.Common.DBMSType.SqlServerCe)
                {
                    temp = temp + " IDENTITY(1,1)";
                }
                else if (db.CurrentDBMSType == PWMIS.Common.DBMSType.Access )
                {
                    temp = temp.Replace("Integer", " autoincrement");
                }
                else if (db.CurrentDBMSType == PWMIS.Common.DBMSType.SQLite)
                {
                    temp = temp + " autoincrement";
                }
                else if (db.CurrentDBMSType == PWMIS.Common.DBMSType.MySql)
                {
                    temp = temp + " AUTO_INCREMENT";
                }
                else if (db.CurrentDBMSType == PWMIS.Common.DBMSType.PostgreSQL)
                {
                    temp = temp + " DEFAULT nextval('" + entity.TableName + "_" + entity.IdentityName + "_" + "seq'::regclass) NOT NULL";
                }
                else
                {
                    //Oracle 采用序列和触发器,这里不处理 
                }
            }
            //identity(1,1) primary key
           
            if (entity.PrimaryKeys.Contains(field))
            {
                if (db.CurrentDBMSType == PWMIS.Common.DBMSType.SQLite)
                {
                    //SQLite 要求主键申明必须在自增之前，否则语法错误
                    temp = temp.Replace(" autoincrement", " PRIMARY KEY autoincrement");
                }
                else
                {
                    //Access 要求主键申明必须在自增之后，否则语法错误
                    temp = temp + " PRIMARY KEY";
                }
            }
            return db.GetPreparedSQL(temp);
        }
    }

    /// <summary>
    /// 实体字段缓存
    /// </summary>
    public class EntityFieldsCache
    {
        private static Dictionary<string, EntityFields> dict = new Dictionary<string, EntityFields>();
        private static object _syncObj = new object();
        /// <summary>
        /// 获取缓存项，如果没有，将自动创建一个
        /// </summary>
        /// <param name="entityType">实体类类型</param>
        /// <returns></returns>
        public static EntityFields Item(Type entityType)
        {
            if (dict.ContainsKey(entityType.FullName))
                return dict[entityType.FullName];
            lock (_syncObj)
            {
                if (dict.ContainsKey(entityType.FullName))
                {
                    return dict[entityType.FullName];
                }
                else
                {
                    EntityFields ef = new EntityFields();
                    if (ef.InitEntity(entityType)) //2015.2.5 修改
                        dict.Add(entityType.FullName, ef);
                    return ef;
                }
            }
           
        }
    }
}
