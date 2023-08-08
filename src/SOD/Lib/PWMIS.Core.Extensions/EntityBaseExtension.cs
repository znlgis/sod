﻿using System;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Adapter;
using PWMIS.DataProvider.Data;

namespace PWMIS.Core.Extensions
{
    /// <summary>
    ///     EntityBase 扩展，由网友“吉林-stdbool”提供
    /// </summary>
    public static class EntityBaseExtension
    {
        /// <summary>
        ///     通过子表实例获取所对应的父表实例 【**仅仅支持单一主键，不支持联合主键的情况**】
        /// </summary>
        /// <typeparam name="T">父表所对应的类</typeparam>
        /// <param name="entity"></param>
        /// <param name="db"></param>
        /// <returns>所对应的父表实例</returns>
        public static T GetParentEntity<T>(this EntityBase entity, AdoHelper db) where T : EntityBase, new()
        {
            var p = new T();
            if (p.PrimaryKeys.Count != 1) //仅支持单一主键的情况!联合主键，应该也可以吧？没仔细考虑。
                return null;

            var fKey = entity.GetForeignKey<T>();

            if (fKey == "" || fKey == null || entity[fKey] == null)
                return null;

            try
            {
                //可能出现类型不匹配的情况~暂时不做处理，简单返回null。待完善
                //if (!p[p.PrimaryKeys[0]].GetType().Equals(entity[fKey].GetType()))
                //    return null;

                p[p.PrimaryKeys[0]] = entity[fKey];

                var eq = new EntityQuery<T>(db);

                if (eq.FillEntity(p)) return p;

                var q = new OQL(p);
                //没想好，这里改如何写。
                //

                return EntityQuery<T>.QueryObject(q);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                //简单处理：1、对主键字段赋值可能会出现类型不匹配的情况
                //          2、
                return null;
            }
        }

        /// <summary>
        ///     通过子表实例获取所对应的父表实例 【**仅仅支持单一主键，不支持联合主键的情况**】
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static T GetParentEntity<T>(this EntityBase entity) where T : EntityBase, new()
        {
            return entity.GetParentEntity<T>(MyDB.Instance);
        }


        /// <summary>
        ///     通过 父表实例 查找 查询关联的特定子实体类集合 【参照“医生”的QueryListWithChild函数，不足：ParentType应该可以通过 entity获取。。。】
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="ParentType"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static List<T> GetChildList<T, ParentType>(this EntityBase entity, AdoHelper db)
            where T : EntityBase, new()
            where ParentType : EntityBase, new()
        {
            var child = new T();

            var fk = child.GetForeignKey<ParentType>();

            if (fk == "")
                return null;

            var sql = "SELECT * FROM " + child.GetTableName() + " WHERE " + fk + " IN ({0})";
            var paraNames = new List<string>();
            var paras = new List<IDataParameter>();

            var name = db.GetParameterChar + "P0";
            paraNames.Add(name);
            paras.Add(db.GetParameter(name, entity[entity.PrimaryKeys[0]]));

            //会有2100个参数的限制问题，下期解决
            var objSql = string.Format(sql, string.Join(",", paraNames.ToArray()));
            var reader = db.ExecuteDataReader(objSql, CommandType.Text, paras.ToArray());
            //如果字段名跟实体类属性名不一致,下面这样使用会有问题,
            //return AdoHelper.QueryList<TChild>(reader);//还需要分析到对应的父实体类上
            //修改成下面的代码 2014.10.30 感谢 网友 发呆数星星 发现此问题
            return EntityQuery<T>.QueryList(reader, child.GetTableName());
        }

        public static List<T> GetChildList<T, ParentType>(this EntityBase entity)
            where T : EntityBase, new()
            where ParentType : EntityBase, new()
        {
            return entity.GetChildList<T, ParentType>(MyDB.Instance);
        }

        #region 实体类跟JSON的转化

        public static string ToJson(this EntityBase entity, Formatting formatting,
            params JsonConverter[] converters)
        {
            var ef = EntityFieldsCache.Item(entity.GetType());
            var json = new JObject();
            for (var i = 0; i < entity.PropertyNames.Length; i++)
            {
                var name = entity.PropertyNames[i];
                var value = entity.PropertyValues[i];

                json.Add(new JProperty(ef.GetPropertyName(name), value));
            }

            return json.ToString(formatting, converters);
        }

        public static void FromJson(this EntityBase entity, string json, JsonLoadSettings settings)
        {
            var ef = EntityFieldsCache.Item(entity.GetType());
            var obj = JObject.Parse(json, settings);
            foreach (var p in obj.Properties())
            {
                var name = ef.GetPropertyField(p.Name);
                if (!string.IsNullOrEmpty(name))
                {
                    string temp = null;
                    var length = name.Length;
                    for (var i = 0; i < entity.PropertyNames.Length; i++)
                    {
                        temp = entity.PropertyNames[i];
                        if (temp != null && temp.Length == length
                                         && string.Equals(temp, name, StringComparison.OrdinalIgnoreCase))
                            entity.PropertyValues[i] = p.Value.Value<object>();
                    }
                }
            }
        }

        public static string ToJson(this EntityBase entity)
        {
            var ef = EntityFieldsCache.Item(entity.GetType());
            var json = new JObject();
            for (var i = 0; i < entity.PropertyNames.Length; i++)
            {
                var name = entity.PropertyNames[i];
                var value = entity.PropertyValues[i];

                json.Add(new JProperty(ef.GetPropertyName(name), value));
            }

            return json.ToString(Formatting.None, null);
        }

        public static void FromJson(this EntityBase entity, string json)
        {
            var ef = EntityFieldsCache.Item(entity.GetType());
            var obj = JObject.Parse(json, null);
            foreach (var p in obj.Properties())
            {
                var name = ef.GetPropertyField(p.Name);
                if (!string.IsNullOrEmpty(name))
                {
                    string temp = null;
                    var length = name.Length;
                    for (var i = 0; i < entity.PropertyNames.Length; i++)
                    {
                        temp = entity.PropertyNames[i];
                        if (temp != null && temp.Length == length
                                         && string.Equals(temp, name, StringComparison.OrdinalIgnoreCase))
                            entity.PropertyValues[i] = p.Value.Value<object>();
                    }
                }
            }
        }

        #endregion
    }
}