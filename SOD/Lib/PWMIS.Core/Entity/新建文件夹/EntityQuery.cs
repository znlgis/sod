/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap 
 * ========================================================================
 * 该类的作用
 * 
 * 作者：邓太华     时间：2008-10-12
 * 版本：V3.0
 * 
 * 修改者：         时间：2010-07-12                
 * 修改说明：改进对象操作数据的线程安全性
 * ========================================================================
*/

/*
 * ====================实体对象查询类===============================
 * 邓太华 2009.12.29 ver 1.0
 *        2010.1.20  ver 1.1 实例对象支持事务操作
 *        2010.6.20  ver 4.0 支持实体类高效分页
 * -----------------------------------------------------------------
 * 以下例子说明了如何使用本类：
 * 
 class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("PWMIS 实体对象查询表达式 (ver1.0) 测试,2009.12.27");
            Console.WriteLine("--------------------------------------------------");
            User u = new User();
            OQL oql = new OQL(u);
            oql.Select(u.Name, u.Uid)
                .Where(oql.Condition.IN(u.Uid, new object[] { 111, 222}).OR(u.Name, "=", "ABC").OR (u.Uid,"=",567));

            Console.WriteLine("\r\n复杂条件形式：");
            Console.WriteLine("SQL=\r\n\""+oql.ToString ()+"\"");
            if (oql.Parameters != null)
            {
                Console.WriteLine("\r\n============== Parameters =================");
                foreach (string key in oql.Parameters.Keys)
                    Console.WriteLine("para name={0},value={1}", key, oql.Parameters[key].ToString());
            }

            ///////////////////////////////////////////////////////////
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("\r\n简单条件形式：");
            //采用静态表达式
            oql = OQL.From(u);
            u.Name = "CDE";
            u.Uid = 1000;
            oql.Select(u.Uid, u.Name)
                .Where(u.Name,u.Uid)
                .OrderBy (u.Name ,"desc").OrderBy(u.Uid,"");
            Console.WriteLine("SQL=\r\n\"" + oql.ToString() + "\"");
            if (oql.Parameters != null)
            {
                Console.WriteLine("\r\n============== Parameters =================");
                foreach (string key in oql.Parameters.Keys)
                    Console.WriteLine("para name={0},value={1}", key, oql.Parameters[key].ToString());
            }

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("\r\n实体对象持久化测试：");

            User zhang_san = new User();
            //zhang_san.Uid = 5;
            EntityQuery<User> query = new EntityQuery<User>(zhang_san,true );
            zhang_san.Name = "张三1";
            query.Save();//新增
            Console.WriteLine("新增实体对象OK");

            zhang_san.Birthday = new DateTime (1977,3,10);
            query.Save();//修改
            Console.WriteLine("修改实体对象OK");

            Console.Read();
        }
    } 
 */

using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System.Data;
using PWMIS.DataProvider.Adapter;
using PWMIS.DataProvider.Data;
using PWMIS.Core;
using System.Collections;

namespace PWMIS.DataMap.Entity
{
    /// <summary>
    /// 实体类查询条件委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="target"></param>
    /// <returns></returns>
    public delegate bool ConditionHandle<T>(T target);

    /// <summary>
    /// 实体对象查询查询类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityQuery<T> where T : EntityBase, new()
    {

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public EntityQuery()
        { 
        
        }

        /// <summary>
        /// 采用一个数据库操作对象实例初始化本类
        /// </summary>
        /// <param name="db">数据库操作对象实例</param>
        public EntityQuery(AdoHelper db)
        {
            DefaultDataBase = db;
        }

        private static EntityQuery<T> _instance;
        /// <summary>
        /// 获取实体查询对象的实例
        /// </summary>
        public static EntityQuery<T> Instance
        {
            get {
                if (_instance == null)
                    _instance = new EntityQuery<T>();
                return _instance;
            }
        }

        private  AdoHelper _DefaultDataBase;
        /// <summary>
        /// 获取或者设置默认的数据库操作对象，如果未设置将采用默认的配置进行实例化数据库操作对象
        /// </summary>
        public  AdoHelper DefaultDataBase
        {
            get {
                if (_DefaultDataBase == null)
                    _DefaultDataBase = MyDB .Instance ;
                return _DefaultDataBase;
            }
            set {
                _DefaultDataBase = value;
            }
        }
        #region 实体操作静态方法

        /// <summary>
        /// 填充实体对象，必须有主键值才可以填充成功
        /// </summary>
        /// <param name="entity">实体对象实例，必须为主键字段属性设置值</param>
        /// <returns>返回填充是否成功</returns>
        public  bool FillEntity(T entity)
        {
            if (entity.PrimaryKeys.Count == 0)
                throw new Exception("EntityQuery Error:当前实体类未指定主键字段");
            if (entity.PropertyNames == null)
                throw new Exception("EntityQuery Error:当前实体类属性字段未初始化");
            int fieldCount = entity.PropertyNames.Length  ;
            if (fieldCount == 0)
                throw new Exception("EntityQuery Error:实体类属性字段数量为0");
            IDataParameter[] paras = new IDataParameter[fieldCount];
            string sql = "SELECT ";
            //string fields = "";
            string condition = "";
            int index = 0;

            //foreach (string field in entity.PropertyNames)
            //{
            //    if (entity.PrimaryKeys.Contains(field))
            //    {
            //        //当前字段时主键，
            //        string paraName = "@P" + index.ToString();
            //        condition += " AND " + field + "=" + paraName;
            //        paras[index] = DefaultDataBase.GetParameter(paraName, entity.PropertyList(field));
            //        index++;
            //    }
            //    else
            //    {
            //        fields += "," + field;
            //    }

            //}
            //sql = sql + fields.TrimStart(',') + " FROM " + entity.TableName + " WHERE " + condition.Substring(" AND ".Length);

            foreach (string key in entity.PrimaryKeys)
            {
                string paraName = "@P" + index.ToString();
                condition += " AND " + key + "=" + paraName;
                paras[index] = DefaultDataBase.GetParameter(paraName, entity.PropertyList(key));
                index++;
            }

            sql = sql + string.Join (",",entity.PropertyNames) + " FROM " + entity.TableName + " WHERE " + condition.Substring(" AND ".Length);
            IDataReader reader = DefaultDataBase.ExecuteDataReader(sql, CommandType.Text, paras);

            bool flag = false;
            if (reader != null)
            {
                try
                {
                    if (reader.Read())
                    {
                        //3.5 之前的代码，已经过时。
                        //for (int i = 0; i < reader.FieldCount; i++)
                        //{
                        //    if (!reader.IsDBNull(i))
                        //        entity.dbSetProperty(reader.GetName(i), reader.GetValue(i));
                        //    else
                        //    {
                        //        string propName=reader.GetName (i);
                        //        //处理数据库空字段
                        //       Type type= reader.GetFieldType(i);
                        //        if(type ==typeof(  string)) 
                        //            entity.dbSetProperty(propName,null );
                        //        else if(type==typeof(  DateTime ) )
                        //           entity.dbSetProperty (propName ,default ( DateTime) );
                        //        else
                        //            entity.dbSetProperty(propName, 0);
                        //    }
                        //}

                        int fcount = reader.FieldCount;
                        //string[] names =fields.TrimStart(',').Split (',');//  new string[fcount];
                        
                        //for (int i = 0; i < fcount; i++)
                        //    names[i] = reader.GetName(i);

                        object[] values = new object[fcount];
                        reader.GetValues(values);


                        //entity.PropertyNames = names;
                        entity.PropertyValues = values;

                        flag = true;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    reader.Close();
                }
            }
            return flag;
        }

        /// <summary>
        /// 采用默认的数据操作对象，填充实体对象，必须有主键值才可以填充成功。注意：该方法可能线程不安全，请使用实例对象的方法 FillEntity 。
        /// </summary>
        /// <param name="entity">实体对象实例，必须为主键字段属性设置值</param>
        /// <returns>返回填充是否成功</returns>
        public static bool Fill(T entity)
        {
            return EntityQuery<T>.Instance.FillEntity(entity); 
        }

        /// <summary>
        /// 根据实体查询表达式对象，查询实体对象集合
        /// </summary>
        /// <param name="oql">实体对象查询表达式</param>
        /// <returns></returns>
        public static  List<T> QueryList(OQL oql)
        {
            return QueryList(oql, MyDB.Instance);
        }

        /// <summary>
        /// 根据实体查询表达式对象，和当前数据库操作对象，查询实体对象集合
        /// </summary>
        /// <param name="oql">实体查询表达式对象</param>
        /// <param name="db">数据库操作对象</param>
        /// <returns>实体对象集合</returns>
        public static  List<T> QueryList(OQL oql,AdoHelper db)
        {
            //string sql = "";
            ////处理用户查询映射的实体类
            //if (oql.EntityMap == PWMIS.Common.EntityMapType.SqlMap)
            //{
            //    if (CommonUtil.CacheEntityMapSql == null)
            //        CommonUtil.CacheEntityMapSql = new Dictionary<string, string>();
            //    if (CommonUtil.CacheEntityMapSql.ContainsKey(oql.sql_table))
            //        sql = CommonUtil.CacheEntityMapSql[oql.sql_table];
            //    else
            //    {
            //        sql = oql.GetMapSQL(GetMapSql(oql.sql_table, typeof(T)));
            //        CommonUtil.CacheEntityMapSql.Add(oql.sql_table, sql);
            //    }
            
            //}
            //else
            //    sql = oql.ToString();

            ////处理实体类分页 2010.6.20
            //if (oql.PageEnable)
            //{
            //    if (db is SqlServer || db is Access)
            //    {
            //        if (oql.PageOrderDesc)
            //            sql = PWMIS.Common.SQLPage.GetDescPageSQLbyPrimaryKey(oql.PageNumber, oql.PageSize, oql.sql_fields, oql.sql_table, oql.PageField, oql.sql_condition);
            //        else
            //            sql = PWMIS.Common.SQLPage.GetAscPageSQLbyPrimaryKey(oql.PageNumber, oql.PageSize, oql.sql_fields, oql.sql_table, oql.PageField, oql.sql_condition);
            //    }
            //    else if (db is Oracle)
            //    {
            //        sql = PWMIS.Common.SQLPage.MakeSQLStringByPage(PWMIS.Common.DBMSType.Oracle, sql, "", oql.PageSize, oql.PageNumber, 999);
            //    }
            //    else
            //    {
            //        throw new Exception("实体类分页错误：不支持此种类型的数据库分页。");
            //    }
            //}

            //IDataReader reader = null;
            //if (oql.Parameters != null && oql.Parameters.Count > 0)
            //{
            //    int fieldCount = oql.Parameters.Count;
            //    IDataParameter[] paras = new IDataParameter[fieldCount];
            //    int index = 0;

            //    foreach (string name in oql.Parameters.Keys)
            //    {
            //        paras[index] = db.GetParameter(name, oql.Parameters[name]);
            //        index++;
            //    }
            //    reader = db.ExecuteDataReader(sql, CommandType.Text, paras);
            //}
            //else
            //{
            //    reader = db.ExecuteDataReader(sql);
            //}
            IDataReader reader = EntityQueryAnonymous.ExecuteDataReader(oql, db, typeof(T));
            return QueryList(reader);
        }

        /// <summary>
        /// 根据EntityMapSql的全名称 "名称空间名字.SQL名字" 获取映射的SQL语句
        /// </summary>
        /// <param name="fullName">EntityMapSql的全名称，格式： "名称空间名字.SQL名字"</param>
        /// <param name="entityType">根据当前实体类所在程序集，获取其中的嵌入式EntityMapSql 文件</param>
        /// <returns>映射的SQL语句</returns>
        public static  string GetMapSql(string fullName,Type entityType)
        {
            //string[] arrTemp=fullName .Split ('.');
            //if (arrTemp.Length != 2)
            //    throw new Exception("EntityMapSql的全名称格式错误，正确的格式应该： 名称空间名字.SQL名字");
         
            //string resourceName = "EntitySqlMap.config";
            //string xmlConfig=   CommonUtil.GetAssemblyResource(entityType, resourceName);
           
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(xmlConfig);
            //XmlNode SqlText = default(XmlNode);
            //XmlElement root = doc.DocumentElement;
            //string objPath = "/configuration/Namespace[@name='" + arrTemp[0] + "']/Map[@name='" + arrTemp[1] + "']/Sql";
            //SqlText = root.SelectSingleNode(objPath);
            //if ((SqlText != null) && SqlText.HasChildNodes)
            //{
            //    return SqlText.InnerText;
            //}
            //return "";
            return EntityQueryAnonymous.GetMapSql(fullName, entityType);
        }

        /// <summary>
        /// 根据数据阅读器对象，查询实体对象集合(注意查询完毕将自动释放该阅读器对象)
        /// </summary>
        /// <param name="reader">数据阅读器对象</param>
        /// <returns>实体类集合</returns>
        public static List<T> QueryList(System.Data.IDataReader reader)
        {
            List<T> list = new List<T>();
            using (reader)
            {

                if (reader.Read())
                {
                    int fcount = reader.FieldCount;
                    string[] names = new string[fcount];
                    object[] values = null;// new object[fcount];

                    for (int i = 0; i < fcount; i++)
                        names[i] = reader.GetName(i);

                    do
                    {
                        values = new object[fcount];
                        reader.GetValues(values);

                        T t = new T();
                        t.PropertyNames = names;
                        t.PropertyValues = values;

                        list.Add(t);
                    } while (reader.Read());

                }
            }
            return list;
        }

        /// <summary>
        /// 根据指定的条件，从数据阅读器中赛选出需要的实体对象。本方法适用于数据量不是很大的数据库表，要加强效率，请使用OQL表达式。
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>实体对象列表</returns>
        public static List<T> QueryList(ConditionHandle<T> condition)
        {
            AdoHelper db = MyDB.Instance;
            T entity = new T();
            string sql = "SELECT " + string.Join(",", entity.PropertyNames) + " FROM " + entity.TableName;
            IDataReader reader = db.ExecuteDataReader(sql);
            List<T> list = new List<T>();
            using (reader)
            {
                int fcount = entity.PropertyNames.Length ;
                while (reader.Read())
                {
                    object[] values =  new object[fcount];
                    reader.GetValues(values);

                    T t = new T();
                    t.PropertyNames = entity.PropertyNames;
                    t.PropertyValues = values;

                    if(condition(t)) //根据条件过滤
                        list.Add(t);
                }
            }
            return list;
        }

        
        /// <summary>
        /// 插入一个实体对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public  int Insert(T entity)
        {
            int fieldCount = entity.PropertyNames .Length ;
            if (fieldCount == 0)
                throw new Exception("EntityQuery Error:实体类属性字段数量为0");

            //List<string> list = new List<string>();
            //foreach (string key in entity.PropertyList.Keys)
            //    if(entity.PropertyChangedList [key ])//只插入属性更改过的字段
            //        list.Add(key);

            return InsertInner(entity, entity.PropertyChangedList, DefaultDataBase);
        }

        /// <summary>
        /// 插入一个实体对象集合（使用事务方式）
        /// </summary>
        /// <param name="entityList">实体对象集合</param>
        /// <returns>受影响的行数</returns>
        public  int Insert(List<T> entityList )
        {
            int count = 0;
            if (entityList.Count > 0)
            {
                //List<string> list = new List<string>();

                AdoHelper db = DefaultDataBase;
                db.BeginTransaction();
                try
                {
                    foreach (T entity in entityList)
                    {
                        //list.Clear();
                        //foreach (string key in entity.PropertyList.Keys)
                        //    if(entity.PropertyChangedList [key ])//只插入属性更改过的字段
                        //        list.Add(key);
                        count += InsertInner(entity, entity.PropertyChangedList, db);
                    }
                    db.Commit();
                }
                catch (Exception ex)
                {
                    db.Rollback();
                    throw ex;
                }
            }
            return count;
        }
       

        /// <summary>
        /// 修改一个实体对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public  int Update(T entity)
        {
            //T temp = new T();
            
            //List<string> list = new List<string>();
            //foreach (string key in entity.PropertyList.Keys)
            //{ 
            //    //只有不等于默认值的字段才需要更新
            //    if (entity.PropertyList[key]!=null && !entity.PropertyList[key].Equals(temp.PropertyList[key]))
            //       list.Add(key);
            //}

            //List<string> list = new List<string>();
            //foreach (string key in entity.PropertyList.Keys)
            //    if (entity.PropertyChangedList[key])//只修改属性更改过的字段
            //        list.Add(key);

            return UpdateInner(entity, entity.PropertyChangedList, DefaultDataBase);
        }

        /// <summary>
        /// 更新一个实体类集合（内部采用事务方式）
        /// </summary>
        /// <param name="entityList">实体类集合</param>
        /// <returns>受影响的行数</returns>
        public  int Update(List<T> entityList)
        {
            T temp = new T();
            //List<string> list = new List<string>();
            int count = 0;
            AdoHelper db = DefaultDataBase;
            db.BeginTransaction();
            try
            {
                foreach (T entity in entityList)
                {
                    //list.Clear();
                    //foreach (string key in entity.PropertyList.Keys)
                    //{
                    //    //只有不等于默认值的字段才需要更新
                    //    if (entity.PropertyList[key] != null && !entity.PropertyList[key].Equals(temp.PropertyList[key]))
                    //        list.Add(key);
                    //}
                    //foreach (string key in entity.PropertyList.Keys)
                    //    if (entity.PropertyChangedList[key])//只修改属性更改过的字段
                    //        list.Add(key);
                    count += UpdateInner(entity, entity.PropertyChangedList, db);
                }
                db.Commit();
            }
            catch (Exception ex)
            {
                db.Rollback();
                throw ex;
            }
            return count;
        }

       /// <summary>
        /// 从数据库删除实体对象对应的记录
       /// </summary>
       /// <param name="entity"></param>
       /// <param name="DB">数据访问对象实例</param>
       /// <returns></returns>
        private  static int DeleteInnerByDB(T entity, CommonDB DB)
        {
            if (entity.PrimaryKeys.Count == 0)
                throw new Exception("EntityQuery Error:当前实体类未指定主键字段");
            int fieldCount = entity.PropertyNames.Length;
            if (fieldCount == 0)
                throw new Exception("EntityQuery Error:实体类属性字段数量为0");

            //CommonDB DB = MyDB.GetDBHelper();

            IDataParameter[] paras = new IDataParameter[fieldCount];
            string sql = "DELETE FROM " + entity.TableName + " WHERE ";
            string values = "";
            string condition = "";
            int index = 0;

            foreach (string key in entity.PrimaryKeys)
            {
                string paraName = "@P" + index.ToString();
                condition += " AND " + key + "=" + paraName;
                paras[index] = DB.GetParameter(paraName, entity.PropertyList(key));
                index++;
            }


            sql = sql + values.TrimStart(',') + " " + condition.Substring(" AND ".Length);
            int count = DB.ExecuteNonQuery(sql, CommandType.Text, paras);
            return count;
        }

        /// <summary>
        /// 从数据库删除实体对象对应的记录
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public  int Delete(T entity)
        {
            return DeleteInnerByDB(entity, DefaultDataBase);
        }

        /// <summary>
        /// 删除一个实体类集合（内部采用事务方式）
        /// </summary>
        /// <param name="entityList">实体类集合</param>
        /// <returns>受影响的行数</returns>
        public  int Delete(List<T> entityList)
        {
            int count = 0;
            AdoHelper db = DefaultDataBase;
            db.BeginTransaction();
            try
            {
                foreach (T entity in entityList)
                {
                    count += DeleteInnerByDB(entity, db);
                }
                db.Commit();
            }
            catch (Exception ex)
            {
                db.Rollback();
                throw ex;
            }
            return count;

        }

        /// <summary>
        /// 执行一个不返回结果集的OQL查询表达式，例如更新，删除实体类的操作
        /// </summary>
        /// <param name="oql">实体查询表达式</param>
        /// <returns>受影响的行数</returns>
        public  int ExecuteOql(OQL oql)
        {
            //string sql = oql.ToString();

            //if (oql.Parameters.Count > 0)
            //{
            //    IDataParameter[] paras = new IDataParameter[oql.Parameters.Count];
            //    int index = 0;

            //    foreach (string key in oql.Parameters.Keys)
            //    {
            //        paras[index] = DefaultDataBase.GetParameter(key, oql.Parameters[key]);
            //        index++;
            //    }
            //    return DefaultDataBase.ExecuteNonQuery(sql, CommandType.Text, paras);
            //}
            //else
            //{
            //    return DefaultDataBase.ExecuteNonQuery(sql);
            //}
            return ExecuteOql(oql, DefaultDataBase);
        }

        /// <summary>
        ///  执行一个不返回结果集的OQL查询表达式，例如更新，删除实体类的操作。使用自定义的数据访问对象进行操作
        /// </summary>
        /// <param name="oql"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static int ExecuteOql(OQL oql,AdoHelper db)
        {
            string sql = oql.ToString();

            if (oql.Parameters.Count > 0)
            {
                IDataParameter[] paras = new IDataParameter[oql.Parameters.Count];
                int index = 0;

                foreach (string key in oql.Parameters.Keys)
                {
                    paras[index] = db.GetParameter(key, oql.Parameters[key]);
                    index++;
                }
                return db.ExecuteNonQuery(sql, CommandType.Text, paras);
            }
            else
            {
                return db.ExecuteNonQuery(sql);
            }

        }

        private static int InsertInner(T entity, List<string> objFields, CommonDB DB)
        {
            if (objFields == null || objFields.Count == 0)
                return 0;

            IDataParameter[] paras = new IDataParameter[objFields.Count];
            //CommonDB DB = MyDB.GetDBHelper();


            string sql = "INSERT INTO " + entity.TableName;
            string fields = "";
            string values = "";
            int index = 0;

            foreach (string field in objFields)
            {
                if (entity.IdentityName != field)
                {
                    fields += "," + field;
                    string paraName = "@P" + index.ToString();
                    values += "," + paraName;
                    paras[index] = DB.GetParameter(paraName, entity.PropertyList(field));
                    index++;
                }
            }
            sql = sql + "(" + fields.TrimStart(',') + ") VALUES (" + values.TrimStart(',') + ")";

            int count = 0;

            if (entity.IdentityName != "")
            {
                //有自增字段
                object id = entity.PropertyList(entity.IdentityName);
                count = DB.ExecuteInsertQuery(sql, CommandType.Text, paras, ref id);
                entity.setProperty(entity.IdentityName, Convert.ToInt32(id));
            }
            else
            {
                count = DB.ExecuteNonQuery(sql, CommandType.Text, paras);
            }

            return count;

        }

        private static int UpdateInner(T entity, List<string> objFields, CommonDB DB)
        {
            if (entity.PrimaryKeys.Count == 0)
                throw new Exception("EntityQuery Error:当前实体类未指定主键字段");
            int fieldCount = entity.PropertyNames .Length ;
            if (fieldCount == 0)
                throw new Exception("EntityQuery Error:实体类属性字段数量为0");

            //CommonDB DB = MyDB.GetDBHelper();
            IDataParameter[] paras = new IDataParameter[fieldCount];
            string sql = "UPDATE " + entity.TableName + " SET ";
            string values = "";
            string condition = "";
            int index = 0;


            foreach (string field in objFields)
            {
                string paraName = "@P" + index.ToString();
                if (entity.PrimaryKeys.Contains(field))
                {
                    //当前字段为主键，不能被更新
                    condition += " AND " + field + "=" + paraName;
                    paras[index] = DB.GetParameter(paraName, entity.PropertyList(field));
                }
                else
                {
                    values += "," + field + "=" + paraName;
                    paras[index] = DB.GetParameter(paraName, entity.PropertyList(field));

                }
                index++;
            }

            if (condition == "")
            {
                foreach (string key in entity.PrimaryKeys)
                {
                    string paraName = "@P" + index.ToString();
                    condition += " AND " + key + "=" + paraName;
                    paras[index] = DB.GetParameter(paraName, entity.PropertyList(key));
                    index++;
                }
            }
            sql = sql + values.TrimStart(',') + " WHERE " + condition.Substring(" AND ".Length);
            int count = DB.ExecuteNonQuery(sql, CommandType.Text, paras);
            return count;
        }


       


        #endregion

        #region 实体操作实例方法

        private T currEntity;
        private bool isNew=false;//当前实体是否是一个新实体（对应数据库而言）
        private List<string> changedFields = new List<string>();
        private List<string> selectFields = new List<string>();

        /// <summary>
        /// 使用一个实体类初始化本类，将探测数据库中是否存在本实体类对应的记录（为提高效率，建议使用另外一个重载）
        /// </summary>
        /// <param name="entity">实体类</param>
        public EntityQuery(T entity)
        {
            isNew =! Fill(entity);//可以填充，说明实体是旧的
            init(entity);
        }

      

        /// <summary>
        /// 使用一个实体类初始化本类，并指明该实体类持久化时新增还是修改
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="newEntity">是否是新实体</param>
        public EntityQuery(T entity,bool newEntity)
        {
            isNew = newEntity;
            init(entity);
        }

        void entity_PropertyGetting(object sender, PropertyGettingEventArgs e)
        {
            if (!selectFields.Contains(e.PropertyName))
                selectFields.Add(e.PropertyName);
        }

        void entity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!changedFields.Contains(e.PropertyName))
                changedFields.Add(e.PropertyName);
        }

        private void init(T entity)
        {
            entity.PropertyGetting += new EventHandler<PropertyGettingEventArgs>(entity_PropertyGetting);
            entity.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(entity_PropertyChanged);
            currEntity = entity;
        }

        /// <summary>
        /// 保存(使用指定的属性)数据到数据库。如果属性值自使用本类以来没有改变过或者没有为本方法指定实体对象的属性字段，那么将不会更新任何数据。
        /// </summary>
        /// <param name="fields">实体类属性值列表</param>
        /// <returns>操作数据库影响的行数</returns>
        public int Save(params object [] fields)
        {
            return Save(DefaultDataBase, fields);
        }

        /// <summary>
        /// 使用带事务的数据库对象，保存(使用指定的属性)数据到数据库。
        /// 如果属性值自使用本类以来没有改变过或者没有为本方法指定实体对象的属性字段，那么将不会更新任何数据。
        /// </summary>
        /// <param name="db">数据库对象，可以在外部开启事务</param>
        /// <param name="fields">实体类属性值列表</param>
        /// <returns>操作数据库影响的行数</returns>
        public int Save(CommonDB db, params object[] fields)
        {
            List<string> objFields = fields.Length > 0 ? selectFields : changedFields;
            if (objFields.Count == 0)
                return 0;
            return InnerSaveAllChanges(db, objFields);
        }

        /// <summary>
        /// 保存自实体类申明以来，所有做过的修改到数据库。
        /// </summary>
        /// <param name="db">数据库访问对象实例</param>
        /// <returns>操作受影响的行数</returns>
        public int SaveAllChanges(CommonDB db)
        {
            //List<string> list = new List<string>();
            //foreach (string key in this.currEntity.PropertyList.Keys)
            //    if (this.currEntity.PropertyChangedList[key])//只修改属性更改过的字段
            //        list.Add(key);

            return InnerSaveAllChanges(db, this.currEntity.PropertyChangedList);
        }

        /// <summary>
        /// 保存自实体类申明以来，所有做过的修改到数据库。
        /// </summary>
        /// <returns>操作受影响的行数</returns>
        public int SaveAllChanges()
        {
            return SaveAllChanges(DefaultDataBase);
        }



        private int InnerSaveAllChanges(CommonDB db, List<string> objFields)
        {
            int count = 0;
            if (objFields.Count == 0)
                return 0;

            if (isNew)
            {
                count = InsertInner(this.currEntity, objFields, db);
                if (count > 0)
                    isNew = false;//保存成功，当前不再是新实体，下次保存应该使用修改

            }
            else
            {
                count = UpdateInner(this.currEntity, objFields, db);
            }
            return count;
        }

        /// <summary>
        /// 删除实体对象的动态实例方法，CommonDB 可以是开启了事务的对象
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public int Delete(T entity, CommonDB db)
        {
            return DeleteInnerByDB(entity, db);
        }

        /// <summary>
        /// 更新实体对象的动态实例方法，CommonDB 可以是开启了事务的对象
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="db">数据库对象</param>
        /// <returns>返回操作受影响的行数</returns>
        public int Update(T entity, CommonDB db)
        {
            T temp = new T();

            //List<string> list = new List<string>();
            //foreach (string key in entity.PropertyList.Keys)
            //{
            //    //只有不等于默认值的字段才需要更新
            //    if (entity.PropertyList[key] != null && !entity.PropertyList[key].Equals(temp.PropertyList[key]))
            //        list.Add(key);
            //}


            return UpdateInner(entity, entity.PropertyChangedList , db);
        }



        /// <summary>
        /// 插入实体对象的动态实例方法，CommonDB 可以是开启了事务的对象
        /// </summary>
        /// <param name="entity">实体类对象</param>
        /// <param name="db">数据库对象</param>
        /// <returns>返回操作受影响的行数</returns>
        public int Insert(T entity, CommonDB db)
        {
            int fieldCount = entity.PropertyNames.Length;
            if (fieldCount == 0)
                throw new Exception("EntityQuery Error:实体类属性字段数量为0");

            //List<string> list = new List<string>();
            //foreach (string key in entity.PropertyList.Keys)
            //    list.Add(key);

            return InsertInner(entity, entity.PropertyChangedList , db);
        }



        
        #endregion

    }

    
}
