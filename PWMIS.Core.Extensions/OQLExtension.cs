using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Data;
using PWMIS.DataProvider.Adapter;


namespace PWMIS.Core.Extensions
{
    /// <summary>
    /// OQL扩展类
    /// </summary>
    public static class OQLExtension
    {
        /// <summary>
        /// OQL 扩展，可以直接返回查询的列表
        /// <example>
        /// <code>
        /// <![CDATA[
        /// 
        ///   User user=new User();
        ///   OQL q=OQL.From(user).Select(user.ID,user.Name).End;
        ///   List<User> list = q.ToList<User>();
        /// 
        /// ]]>
        /// </code>
        /// </example>
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="q">OQL对象</param>
        /// <returns>实体类列表</returns>
        public static List<T> ToList<T>(this OQL q) where T:EntityBase,new()
        {
            return EntityQuery<T>.QueryList(q);
        }
        /// <summary>
        /// OQL 扩展，可以直接返回查询的列表
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="q">OQL对象</param>
        /// <param name="db">数据访问对象</param>
        /// <returns>实体类列表</returns>
        public static List<T> ToList<T>(this OQL q,AdoHelper db) where T : EntityBase, new()
        {
            return EntityQuery<T>.QueryList(q,db);
        }
        /// <summary>
        /// OQL 扩展，可以直接返回查询的实体类
        /// <example>
        /// <code>
        /// <![CDATA[
        ///   User user=new User(){ID=100};
        ///   OQL q=OQL.From(user).Select(user.ID,user.Name).Where(user.ID).End;
        ///   User result = q.ToEntity<User>();
        /// ]]>
        /// </code>
        /// </example>
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="q">OQL对象</param>
        /// <returns>实体类</returns>
        public static T ToEntity<T>(this OQL q) where T : EntityBase, new()
        {
            return EntityQuery<T>.QueryObject(q);
        }
        /// <summary>
        ///  OQL 扩展，可以直接返回查询的实体类
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="q">OQL对象</param>
        /// <param name="db">数据访问对象</param>
        /// <returns>实体类</returns>
        public static T ToEntity<T>(this OQL q, AdoHelper db) where T : EntityBase, new()
        {
            return EntityQuery<T>.QueryObject(q,db);
        }
        /// <summary>
        /// 直接返回实体类列表查询结果
        /// <example>
        /// <code>
        /// <![CDATA[
        /// AdoHelper dbHelper=new SqlServer(){ConnectionString="....."};
        /// var list= OQL.From(entity1)
        ///          .Join(entity2).On(entity1.PK,entity2.FK)
        ///          .Select(entity1.Field1,entity2.Field2)
        ///       .End
        ///       .ToObjectList(dbHelper, e =>
        ///          {
        ///             return new {
        ///                          Property1=e.GetItemValue<int>(0), 
        ///                          Property2=e.GetItemValue<string>(1) 
        ///                        };
        ///          });
        /// 
        /// foreache(var item in list)
        /// {
        ///     Console.WriteLine("Property1={0},Property2={1}",item.Property1,item.Property2);
        /// }
        /// ]]>
        /// ]]>
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="q">OQL对象</param>
        /// <param name="db">数据访问对象</param>
        /// <param name="ecFun">结果对象映射委托</param>
        /// <typeparam name="TResult">结果元素类型</typeparam>
        /// <returns>实体类列表</returns>
        public static IList<TResult> ToObjectList<TResult>(this OQL q, AdoHelper db, ECResultFunc<TResult> ecFun) where TResult : class
        {
            EntityContainer ec = new EntityContainer(q, db);
            return ec.ToObjectList<TResult>(ecFun);
        }


        /// <summary>
        /// 直接返回实体类列表查询结果
        /// <example>
        /// <code>
        /// <![CDATA[
        /// var list= OQL.From(entity1)
        ///          .Join(entity2).On(entity1.PK,entity2.FK)
        ///          .Select(entity1.Field1,entity2.Field2)
        ///       .End
        ///       .ToObjectList( e =>
        ///          {
        ///             return new MyViewModel{
        ///                          Property1=e.GetItemValue<int>(0), 
        ///                          Property2=e.GetItemValue<string>(1) 
        ///                        };
        ///          });
        /// 
        /// foreache(MyViewModel item in list)
        /// {
        ///     Console.WriteLine("Property1={0},Property2={1}",item.Property1,item.Property2);
        /// }
        /// ]]>
        /// ]]>
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="q">OQL对象</param>
        /// <param name="ecFun">结果对象映射委托</param>
        /// <typeparam name="TResult">结果元素类型</typeparam>
        /// <returns>实体类列表</returns>
        public static IList<TResult> ToObjectList<TResult>(this OQL q, ECResultFunc<TResult> ecFun) where TResult : class
        {
            EntityContainer ec = new EntityContainer(q);
            return ec.ToObjectList<TResult>(ecFun);
        }

        /// <summary>
        /// 执行OQL
        /// </summary>
        /// <param name="oql">OQL</param>
        /// <param name="db">数据访问对象</param>
        /// <returns>操作受影响的行数</returns>
        public static int Execute(this OQL oql, AdoHelper db)
        {
            return EntityQuery.ExecuteOql(oql, db);
        }
        /// <summary>
        /// 使用默认连接，执行OQL
        /// </summary>
        /// <param name="oql">OQL</param>
        /// <returns>操作受影响的行数</returns>
        public static int Execute(this OQL oql)
        {
            return EntityQuery.ExecuteOql(oql, MyDB.Instance);
        }
      
    }
}
