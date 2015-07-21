using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Adapter;
using PWMIS.DataProvider.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PWMIS.Core.Extensions
{
    /// <summary>
    /// EntityBase 扩展，由网友“吉林-stdbool”提供
    /// </summary>
    public static class EntityBaseExtension
    {


        /// <summary>
        /// 通过子表实例获取所对应的父表实例 【**仅仅支持单一主键，不支持联合主键的情况**】
        /// </summary>
        /// <typeparam name="T">父表所对应的类</typeparam>
        /// <param name="entity"></param>
        /// <param name="db"></param>
        /// <returns>所对应的父表实例</returns>
        public static T GetParentEntity<T>(this EntityBase entity, AdoHelper db) where T : EntityBase, new()
        {
            T p = new T();
            if (p.PrimaryKeys.Count != 1)//仅支持单一主键的情况!联合主键，应该也可以吧？没仔细考虑。
                return null;

            string fKey = entity.GetForeignKey<T>();

            if (fKey == "" || fKey == null || entity[fKey] == null)
                return null;

            try
            {
                //可能出现类型不匹配的情况~暂时不做处理，简单返回null。待完善
                //if (!p[p.PrimaryKeys[0]].GetType().Equals(entity[fKey].GetType()))
                //    return null;

                p[p.PrimaryKeys[0]] = entity[fKey];

                EntityQuery<T> eq = new EntityQuery<T>(db);

                if (eq.FillEntity(p))
                {
                    return p;
                }
                else
                {
                    OQL q = new OQL(p);
                    //没想好，这里改如何写。
                    //

                    return EntityQuery<T>.QueryObject(q);
                }
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
        /// 通过子表实例获取所对应的父表实例 【**仅仅支持单一主键，不支持联合主键的情况**】
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static T GetParentEntity<T>(this EntityBase entity) where T : EntityBase, new()
        {
            return entity.GetParentEntity<T>(MyDB.Instance);
        }



        /// <summary>
        /// 通过 父表实例 查找 查询关联的特定子实体类集合 【参照“医生”的QueryListWithChild函数，不足：ParentType应该可以通过 entity获取。。。】
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="ParentType"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static List<T> GetChildList<T, ParentType>(this EntityBase entity, AdoHelper db)
            where T : EntityBase, new()
            where ParentType : EntityBase, new()
        {
            T child = new T();

            string fk = child.GetForeignKey<ParentType>();

            if (fk == "")
                return null;

            string sql = "SELECT * FROM " + child.GetTableName() + " WHERE " + fk + " IN ({0})";
            List<string> paraNames = new List<string>();
            List<IDataParameter> paras = new List<IDataParameter>();

            string name = db.GetParameterChar + "P0";
            paraNames.Add(name);
            paras.Add(db.GetParameter(name, entity[entity.PrimaryKeys[0]]));

            //会有2100个参数的限制问题，下期解决
            string objSql = string.Format(sql, string.Join(",", paraNames.ToArray()));
            IDataReader reader = db.ExecuteDataReader(objSql, CommandType.Text, paras.ToArray());
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

    }

}
