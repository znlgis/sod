/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap 
 * ========================================================================
 * 该类的作用
 * 
 * 作者：邓太华     时间：2008-10-12
 * 版本：V4.6.4.0525
 * 
 * 修改者：         时间：2010-07-12                
 * 修改说明：改进对象操作数据的线程安全性
 * 
 * 修改者：         时间：2012-12-20                
 * 修改说明：更新数据的时候，Access 参数顺序问题（感谢网友THIRDEYE 发现的问题）
 * 
 * 修改者：         时间：2013-1-13                
 * 修改说明：支持读写分离的查询
 * 
 * 修改者：         时间：2013-2-1                
 * 修改说明：解决了在启用分页查询的情况下，记录总数设置为0，
 *           然后执行QueryList 返回的集合中有一个“空实体”元素的问题。
 * 修改者：         时间：2013-5-25                
 * 修改说明：增加了使用接口定义实体类，并创建动态实体类的功能，为此新增非泛型类型的 EntityQuery
 *    
 * 修改者：         时间：2013-10-7                
 * 修改说明：修改了QuetyList方法，加快生成实体类的效率
 * 
 * 修改者：         时间：2014-3-4                
 * 修改说明：修改了QuetyList方法，当OQL.PageWithAllRecordCount==0 将会多执行一次 Count 查询，
 *           然后改变 oql.PageWithAllRecordCount 的实际值。感谢网友 发呆数星星 发现此问题。
 *           
 * 修改者：         时间：2014-4-7                
 * 修改说明：在事务的方法中，使用DefaultNewDataBase，避免事务方法与非事务方法混用导致在多线程环境下连接无法关闭的问题。
 *           感谢网友 null 等发现此问题。
 *           
 * 修改者：         时间：2014-4-15                
 * 修改说明：增加 QueryList,QueryObject 对应的实例方法，以配合 DbContext。
 *           
 * 修改者：         时间：2014-4-18                
 * 修改说明：修复EntityQuery<T> 中记录总数为0的时候，返回的结果集有一条无效记录的问题。
 * 
 *  修改者：         时间：2014-10-30                
 * 修改说明：修复查询子实体类，子实体类如果属性名根字段名不一样查询出错的问题。
 * 
 *  修改者：         时间：2014-11-9                
 * 修改说明：修正查询子实体类的时候，原来只能查询出一个子实体类集合的问题。
 * 
 *  修改者：         时间：2015-3-7                
 * 修改说明：修正Varchar(max),text 字段查询设置字段长度的问题。
 * 
 *  修改者：         时间：2015-7-21                
 * 修改说明：修正当实体类映射了新的表名字，OQL查询出来的实体类列表表名字不是新名字的问题
 * 
 * 修改者：         时间：2015-11-24                
 * 修改说明：修改Insert内部方法，在参数上传递 InsertKey，避免Oracle多线程插入自增数据的问题
 * 
 * 修改者：         时间：2015-11-25                
 * 修改说明：修改ExecuteInsrtOql 内部实现，使用EntityCommand的 GetInsertKey 方法
 * 
 * ========================================================================
*/

/*
 * ====================实体对象查询类===============================
 * 邓太华 2009.12.29 ver 1.0
 *        2010.1.20  ver 1.1 实例对象支持事务操作
 *        2010.6.20  ver 4.0 支持实体类高效分页
 *        2011.4.13  ver 4.1 增加了查询单个实体对象的方法
 *        2012.7.12  ver 4.3 增加了以实体类改变了值的属性，作为查询条件的方法（网友[左眼]贡献代码）
 *        2012.8.27  ver 4.5 增加了对POCO实体类的支持，该实体类实现IReadData 接口即可。
 *        可以单独调用EntityQueryAnonymous.ExecuteDataList<T>(IDataReader reader) 来实现。 
 *        2012.11.4  ver 4.5 修正了实体类的时候没有属性被修改但试图更新实体类到数据库发生的错。
 *        
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
 * 
 * 
 * /////////////////////////////////////////////////////////////////////////////
 * 
 *  以下例子说明如何定义一个POCO实体类来使用PDF.NET框架：
 *   public partial class Table_User : EntityBase, IReadData
 *   {
 *     public Table_User(){
 *        //省略实现
 *     }
 * 
 *      public System.Int32 UID
        {
          get{
              //this.OnPropertyGeting("UID");
              return _UID;
          }
          set { 
              _UID = value; 
              //setProperty("UID", value);
          }
       }
 * 
 *     //省略其它属性的实现
 *     
      #region difinePrivateVar
 
      System.Int32 _UID;
      System.String _Name;
      System.Boolean _Sex;
      System.Double _Height;
      System.DateTime _Birthday;

      #endregion

      #region 接口方法

      public void ReadData(IDataReader reader,int fieldCount,string[] fieldNames)
      {
          for (int i = 0; i < fieldCount; i++)
          {
              if (reader.IsDBNull(i))
                  continue;
              switch (fieldNames[i])
              {
                  case "UID":
                      _UID = reader.GetInt32(i); break;
                  case "Name":
                      _Name = reader.GetString(i); break;
                  case "Sex":
                      _Sex = reader.GetBoolean(i); break;
                  case "Height":
                      _Height = reader.GetFloat(i); break;
                  case "Birthday":
                      _Birthday = reader.GetDateTime(i); break;
              }
          }
      }

      #endregion
 * 
 *   }
 * 
 * 
 * 
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
using PWMIS.Common;
using System.Reflection;


namespace PWMIS.DataMap.Entity
{
    /// <summary>
    /// 实体类查询条件委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="target"></param>
    /// <returns></returns>
    public delegate bool ConditionHandle<T>(T target);
    public delegate void SqlInfoAction(SqlInfo si, object paraValueObject);

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
            get
            {
                if (_instance == null)
                    _instance = new EntityQuery<T>();
                return _instance;
            }
        }

        private AdoHelper _DefaultDataBase;
        /// <summary>
        /// 获取或者设置默认的数据库操作对象，如果未设置将采用默认的配置进行实例化数据库操作对象。
        /// 支持读写分离模式
        /// </summary>
        public AdoHelper DefaultDataBase
        {
            get
            {
                if (_DefaultDataBase == null)
                    return MyDB.Instance;
                else
                    return _DefaultDataBase;
            }
            set
            {
                _DefaultDataBase = value;
            }
        }

        /// <summary>
        /// 获取当期类设定的数据连接对象，与DefaultDataBase 属性不同的是，如果未设置过 DefaultDataBase，
        /// 则使用MyDB.GetDBHelper() 获得一个新的默认配置的数据访问对象实例；
        /// 如果设置过，则使用 DefaultDataBase 设置的数据访问对象。
        /// </summary>
        public AdoHelper DefaultNewDataBase
        {
            get
            {
                if (_DefaultDataBase == null)
                    return MyDB.GetDBHelper();
                else
                    return _DefaultDataBase;
            }
        }

        #region 实体操作静态方法
        /// <summary>
        /// 检测实体类是否在数据库存在
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ExistsEntity(T entity)
        {
            return EntityQuery.ExistsEntity(entity, DefaultDataBase);
        }

        /// <summary>
        /// 填充实体对象，必须有主键值才可以填充成功
        /// </summary>
        /// <param name="entity">实体对象实例，必须为主键字段属性设置值</param>
        /// <returns>返回填充是否成功</returns>
        public bool FillEntity(T entity)
        {
            return EntityQuery.FillEntity(entity, DefaultDataBase);
        }

        /// <summary>
        /// [网友左眼提供]以OQL为查询，并且附加当前类型的实体类中改变了值的属性 为查询条件
        /// </summary>
        /// <param name="oql">OQL表达式</param>
        /// <param name="entity">实体类实例对象</param>
        /// <returns>实体类集合</returns>
        public List<T> FillEntityList(OQL oql, T entity)
        {
            string sql = string.Empty;
            if (entity.PropertyChangedList.Count > 0)
            {
                IDataParameter[] paras;
                sql = FillParameter(oql, entity, out paras);

                IDataReader reader = DefaultDataBase.ExecuteDataReader(sql, CommandType.Text, paras);
                return QueryList(reader,oql.GetEntityTableName());
            }
            else
                return QueryList(oql);
        }

        /// <summary>
        /// 根据实体填充参数,返回sql
        /// </summary>
        /// <param name="oql"></param>
        /// <param name="entity"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public string FillParameter(OQL oql, T entity, out IDataParameter[] paras)
        {
            string sql = oql.ToString();
            Dictionary<string, TableNameField> Parameters = oql.Parameters;
            int index = 0;
            string condition = string.Empty;

            //为DateTime类型增加
            int tempCount = 0;
            foreach (string temp in entity.PropertyChangedList)
            {
                if (entity.PropertyList(temp).GetType() == typeof(DateTime))
                    tempCount++;
            }

            if (Parameters == null || Parameters.Count < 1)//不存在Where
            {

                paras = new IDataParameter[entity.PropertyChangedList.Count + tempCount];
                foreach (string key in entity.PropertyChangedList)
                {
                    IDataParameter para = DefaultDataBase.GetParameter();
                    string paraName = DefaultDataBase.GetParameterChar + "P" + index.ToString();
                    if (entity.PropertyList(key).GetType() == typeof(string))
                    {
                        condition += " AND [" + key + "] like " + paraName;
                        para.ParameterName = paraName;
                        para.Value = "%" + entity.PropertyList(key) + "%";
                    }
                    else
                    {
                        if (entity.PropertyList(key).GetType() == typeof(DateTime))
                        {
                            condition += " AND [" + key + "] >= " + paraName;
                            para.ParameterName = paraName;
                            para.Value = entity.PropertyList(key);
                            //---时间比较当前时间-明天之间
                            paras[index] = para;
                            index++;
                            para = DefaultDataBase.GetParameter();
                            paraName = DefaultDataBase.GetParameterChar + "P" + index.ToString();
                            condition += " AND [" + key + "] < " + paraName;
                            para.ParameterName = paraName;
                            para.Value = ((DateTime)entity.PropertyList(key)).AddDays(1).ToShortDateString();
                            //---时间比较当前时间-明天之间
                        }
                        else
                        {

                            condition += " AND [" + key + "] = " + paraName;
                            para.ParameterName = paraName;
                            para.Value = entity.PropertyList(key);
                        }
                    }
                    paras[index] = para;
                    index++;
                }
                string tableName = entity.GetSchemeTableName ();
                int whereIndex = sql.IndexOf(tableName);
                sql = sql.Substring(0, whereIndex + tableName.Length) + (string.IsNullOrEmpty(condition) ? " " : (" WHERE " + condition.Substring(" AND ".Length)))
                    + sql.Substring(whereIndex + tableName.Length);
            }
            else
            {
                int fieldCount = Parameters.Count;
                paras = new IDataParameter[fieldCount + entity.PropertyChangedList.Count + tempCount];
                foreach (string name in Parameters.Keys)
                {
                    IDataParameter para = DefaultDataBase.GetParameter();
                    para.ParameterName = name;
                    para.Value = Parameters[name].FieldValue;
                    paras[index] = para;
                    index++;
                }
                foreach (string key in entity.PropertyChangedList)
                {
                    IDataParameter para = DefaultDataBase.GetParameter();
                    string paraName = DefaultDataBase.GetParameterChar + "P" + index.ToString();
                    if (entity.PropertyList(key).GetType() == typeof(string))
                    {
                        condition += " AND [" + key + "] like " + paraName;
                        para.ParameterName = paraName;
                        para.Value = "%" + entity.PropertyList(key) + "%";
                    }
                    else
                    {
                        if (entity.PropertyList(key).GetType() == typeof(DateTime))
                        {
                            condition += " AND [" + key + "] >= " + paraName;
                            para.ParameterName = paraName;
                            para.Value = entity.PropertyList(key);
                            //---时间比较当前时间-明天之间
                            paras[index] = para;
                            index++;
                            para = DefaultDataBase.GetParameter();
                            paraName = DefaultDataBase.GetParameterChar + "P" + index.ToString();
                            condition += " AND [" + key + "] < " + paraName;
                            para.ParameterName = paraName;
                            para.Value = ((DateTime)entity.PropertyList(key)).AddDays(1).ToShortDateString();
                            //---时间比较当前时间-明天之间
                        }
                        else
                        {

                            condition += " AND [" + key + "] = " + paraName;
                            para.ParameterName = paraName;
                            para.Value = entity.PropertyList(key);
                        }
                    }
                    paras[index] = para;
                    index++;
                }
                sql = sql.ToUpper();
                int whereIndex = sql.IndexOf(" WHERE", System.StringComparison.Ordinal);

                condition = string.IsNullOrEmpty(condition) ? "" : condition.Substring(" AND ".Length) + " AND";
                sql = sql.Substring(0, whereIndex + 6) + condition
                    + sql.Substring(whereIndex + 6);
            }

            #region 分页

            //处理实体类分页 2010.6.20
            AdoHelper db = MyDB.Instance;
            if (oql.PageEnable)
            {
                switch (db.CurrentDBMSType)
                {
                    case PWMIS.Common.DBMSType.Access:
                    case PWMIS.Common.DBMSType.SqlServer:
                    case PWMIS.Common.DBMSType.SqlServerCe:
                        //如果含有Order By 子句，则不能使用主键分页
                        if (oql.haveJoinOpt || sql.IndexOf("order by", StringComparison.OrdinalIgnoreCase) > 0)
                        {
                            sql = PWMIS.Common.SQLPage.MakeSQLStringByPage(PWMIS.Common.DBMSType.SqlServer, sql, "", oql.PageSize, oql.PageNumber, oql.PageWithAllRecordCount);
                        }
                        else
                        {
                            if (oql.PageOrderDesc)
                                sql = PWMIS.Common.SQLPage.GetDescPageSQLbyPrimaryKey(oql.PageNumber, oql.PageSize, oql.sql_fields, oql.sql_table, oql.PageField, oql.sql_condition);
                            else
                                sql = PWMIS.Common.SQLPage.GetAscPageSQLbyPrimaryKey(oql.PageNumber, oql.PageSize, oql.sql_fields, oql.sql_table, oql.PageField, oql.sql_condition);
                        }
                        break;
                    default:
                        sql = PWMIS.Common.SQLPage.MakeSQLStringByPage(db.CurrentDBMSType, sql, "", oql.PageSize, oql.PageNumber, oql.PageWithAllRecordCount);
                        break;
                }
            }

            #endregion

            return sql;
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
        /// [网友左眼提供]以OQL为查询，并且附加当前类型的实体类中改变了值的属性 为查询条件
        /// </summary>
        /// <param name="oql">OQL表达式</param>
        /// <param name="entity">实体类实例对象</param>
        /// <returns>实体类集合</returns>
        public static List<T> QueryList(OQL oql, T entity)
        {
            return EntityQuery<T>.Instance.FillEntityList(oql, entity);
        }

        /// <summary>
        /// 根据实体查询表达式对象，查询符合条件的一个实体对象
        /// </summary>
        /// <param name="oql">OQL表达式</param>
        /// <returns>实体对象</returns>
        public T GetObject(OQL oql)
        {
            return QueryObject(oql,this.DefaultDataBase);
        }

        /// <summary>
        /// 根据实体查询表达式对象，查询符合条件的一个实体对象
        /// </summary>
        /// <param name="oql">OQL表达式</param>
        /// <returns>实体对象</returns>
        public static T QueryObject(OQL oql)
        {
            return QueryObject(oql, MyDB.Instance);
        }

        /// <summary>
        /// 根据实体查询表达式对象，查询符合条件的一个实体对象。如果数据访问对象未在事务中，方法执行完后将自动关闭数据库连接。
        /// </summary>
        /// <param name="oql">OQL表达式</param>
        /// <param name="db">数据访问对象实例</param>
        /// <returns>实体对象</returns>
        public static T QueryObject(OQL oql, AdoHelper db)
        {
            //using (IDataReader reader = EntityQueryAnonymous.ExecuteDataReader(oql, db, typeof(T), true))
            //{
            //    if (reader.Read())
            //    {
            //        int fcount = reader.FieldCount;
            //        string[] names = new string[fcount];
            //        object[] values = null;// new object[fcount];

            //        for (int i = 0; i < fcount; i++)
            //            names[i] = reader.GetName(i);

            //        values = new object[fcount];
            //        reader.GetValues(values);

            //        T t = new T();
            //        t.PropertyNames = names;
            //        t.PropertyValues = values;

            //        return t;
            //    }
            //}
            //return null;
            return QueryObject(EntityQueryAnonymous.ExecuteDataReader(oql, db, typeof(T), true));
        }

        /// <summary>
        /// 根据数据阅读器，返回指定的对象。如果数据访问对象未在事务中，方法执行完后将自动关闭数据库连接。
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T QueryObject(IDataReader reader)
        {
            using (reader)
            {
                if (reader.Read())
                {
                    int fcount = reader.FieldCount;
                    string[] names = new string[fcount];

                    for (int i = 0; i < fcount; i++)
                        names[i] = reader.GetName(i);

                    T t = new T();
                    if (t is IReadData)
                    {
                        ((IReadData)t).ReadData(reader, fcount, names);
                    }
                    else
                    {
                        object[] values = new object[fcount];
                        reader.GetValues(values);
                        t.PropertyNames = names;
                        t.PropertyValues = values;
                    }
                    return t;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据实体查询表达式对象，查询实体对象集合，如果未查询到结果，集合元素数量为0。
        /// </summary>
        /// <param name="oql">实体对象查询表达式</param>
        /// <returns></returns>
        public List<T> GetList(OQL oql)
        {
            return QueryList(oql, this.DefaultDataBase);
        }

        /// <summary>
        /// 根据实体查询表达式对象，查询实体对象集合，如果未查询到结果，集合元素数量为0。
        /// 注意，该方法将使用默认的数据访问对象 MyDB.Instance，如果你在事务中请使用另一个重载方法，传入事务相关的数据访问对象。
        /// </summary>
        /// <param name="oql">实体对象查询表达式</param>
        /// <returns></returns>
        public static List<T> QueryList(OQL oql)
        {
            return QueryList(oql, MyDB.Instance);
        }

        /// <summary>
        /// 根据实体查询表达式对象，和当前数据库操作对象，查询实体对象集合
        /// 如果OQL的PageWithAllRecordCount 等于0且指定了分页，则会执行一次统计记录数量的查询，
        /// 执行本方法后，OQL对象PageWithAllRecordCount 会得到实际的值
        /// </summary>
        /// <param name="oql">实体查询表达式对象</param>
        /// <param name="db">数据库操作对象</param>
        /// <returns>实体对象集合</returns>
        public static List<T> QueryList(OQL oql, AdoHelper db)
        {
            //如果开启了分页且记录总数为0，直接返回空集合
            if (oql.PageEnable )
            {
                if (oql.PageWithAllRecordCount == 0)
                {
                    if (!oql.haveOrderBy)
                        throw new Exception("如果要进行分页并且同时获得记录总数，OQL必须有排序操作，请调用OrderBy方法。");
                    T t = QueryObject(oql, db);
                    oql.PageWithAllRecordCount = CommonUtil.ChangeType<int>(t.PropertyValues[0]);
                    //如果记录总数仍然是0，直接返回空集合。
                    //感谢网友 koumi 发现此问题。
                    if (oql.PageWithAllRecordCount == 0)
                        return new List<T>();
                }
            }
            IDataReader reader = EntityQueryAnonymous.ExecuteDataReader(oql, db, typeof(T));
            return QueryList(reader,oql.GetEntityTableName());
        }

        /// <summary>
        /// 缓存OQL的结果然后从数据库查询数据，使得不必每次处理OQL对象。
        /// </summary>
        /// <param name="cacheKey">要缓存的Ｋｅｙ</param>
        /// <param name="oqlFun">如果没有缓存的项，要初始化缓存项的委托函数</param>
        /// <param name="action">要处理的查询的委托</param>
        /// <param name="paraValueObject">相关的初始化操作的参数对象</param>
        /// <param name="db">数据访问对象</param>
        /// <returns>实体类集合</returns>
        public static List<T> QueryListByCache(string cacheKey, OQLCacheFunc oqlFun, SqlInfoAction action
            , object paraValueObject, AdoHelper db)
        {
            SqlInfo si = SqlInfo.GetFromCache(cacheKey);
            if (si == null)
            {
                OQL q = oqlFun(paraValueObject);
                si = EntityQueryAnonymous.GetSqlInfoFromOQL(q, db, typeof(T), false);
                SqlInfo.AddToCache(cacheKey, si);
            }
            IDataReader reader = EntityQueryAnonymous.ExecuteDataReader(si, db, false);
            return QueryList(reader,si.TableName);
        }

        /// <summary>
        /// 根据EntityMapSql的全名称 "名称空间名字.SQL名字" 获取映射的SQL语句
        /// </summary>
        /// <param name="entityType">根据当前实体类所在程序集，获取其中的嵌入式EntityMapSql 文件</param>
        /// <returns>映射的SQL语句</returns>
        public static string GetMapSql(Type entityType)
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
            return EntityQueryAnonymous.GetMapSql(entityType);
        }

        public static List<T> QueryList(System.Data.IDataReader reader)
        {
            return QueryList(reader, "");
        }

        /// <summary>
        /// 根据数据阅读器对象，查询实体对象集合(注意查询完毕将自动释放该阅读器对象)
        /// </summary>
        /// <param name="reader">数据阅读器对象</param>
        /// <param name="tableName">指定实体类要映射的表名字,默认不指定</param>
        /// <returns>实体类集合</returns>
        public static List<T> QueryList(System.Data.IDataReader reader,string tableName)
        {
            List<T> list = new List<T>();
            using (reader)
            {
                if (reader.Read())
                {
                    int fcount = reader.FieldCount;
                    string[] names = new string[fcount];

                    for (int i = 0; i < fcount; i++)
                        names[i] = reader.GetName(i);
                    T t0 = new T();
                    if (!string.IsNullOrEmpty(tableName))
                        t0.MapNewTableName(tableName);
                    t0.PropertyNames = names;
                    do
                    {
                        object[] values = new object[fcount];
                        reader.GetValues(values);

                        T t = (T)t0.Clone();

                        //t.PropertyNames = names;
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
            string sql = "SELECT " + string.Join(",", CommonUtil.PrepareSqlFields(entity.PropertyNames)) + " FROM " + entity.GetSchemeTableName();
            IDataReader reader = db.ExecuteDataReader(sql);
            List<T> list = new List<T>();
            using (reader)
            {
                int fcount = entity.PropertyNames.Length;
                while (reader.Read())
                {
                    object[] values = new object[fcount];
                    reader.GetValues(values);

                    T t = new T();
                    t.PropertyNames = entity.PropertyNames;
                    t.PropertyValues = values;

                    if (condition(t)) //根据条件过滤
                        list.Add(t);
                }
            }
            return list;
        }

        /// <summary>
        /// 查询实体类集合关联的子实体类
        /// </summary>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="entitys"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        private  List<TChild> QueryChild<TChild>(IEnumerable<T> entitys,AdoHelper db) where TChild:EntityBase,new()
        {
            TChild child = new TChild();
            
            string sql = "SELECT * FROM "+child.GetTableName()+" WHERE "+child.GetForeignKey<T>()+" IN ({0})";
            List<string> paraNames = new List<string>();
            List<IDataParameter> paras = new List<IDataParameter>();
            int count = 0;
            foreach(EntityBase e in entitys)
            {
                string name = db.GetParameterChar+ "P"+count;
                paraNames.Add(name);
                paras.Add(db.GetParameter(name,e[e.PrimaryKeys[0]]));
                count++;
            }
            //会有2100个参数的限制问题，下期解决
            string objSql = string.Format(sql, string.Join(",", paraNames.ToArray()));
            IDataReader reader = db.ExecuteDataReader(objSql, CommandType.Text, paras.ToArray());
            //如果字段名跟实体类属性名不一致,下面这样使用会有问题,
            //return AdoHelper.QueryList<TChild>(reader);//还需要分析到对应的父实体类上
            //修改成下面的代码 2014.10.30 感谢 网友 发呆数星星 发现此问题
            return EntityQuery<TChild>.QueryList(reader, child.GetTableName());
        }

        private  void QueryAndSetChild<TChild>(List<T> entitys, AdoHelper db) where TChild : EntityBase, new()
        {
            List<TChild> childList = QueryChild<TChild>(entitys, db);
            string childName = GetChildEntityListPropertyName<TChild>();
            DelegatedReflectionMemberAccessor drm = new DelegatedReflectionMemberAccessor();
            foreach (T e in entitys)
            {
                List<TChild> newChilds = new List<TChild>();
                foreach (TChild child in childList)
                {
                    if (child[child.GetForeignKey<T>()].Equals( e[e.PrimaryKeys[0]])) //待优化
                    {
                        newChilds.Add(child);
                    }
                }
                //为当前实体类设置子实体类集合
                var accessor= drm.FindAccessor<T>(childName);
                accessor.SetValue(e, newChilds);
            }
        }

        /// <summary>
        /// 获取当前实体类的具有枚举功能的属性的名字
        /// </summary>
        /// <typeparam name="TChild">子实体类集合属性的元素类型</typeparam>
        /// <returns></returns>
        private  string GetChildEntityListPropertyName<TChild>() where TChild : EntityBase, new()
        {
            var props = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (var p in props)
            {
                if (p.PropertyType.IsGenericType)
                {
                    Type t = p.PropertyType.GetGenericTypeDefinition();
                    if (t == typeof(IEnumerable<>) || t.GetInterface("IEnumerable`1") != null)
                    {
                        //"IS IEnumerable<>"
                        Type[] tArr = p.PropertyType.GetGenericArguments();
                        if(tArr.Length==1 && tArr[0]==typeof(TChild))
                            return p.Name;
                    }
                }
            }
            return null;
        }

        private  string[] GetAllChildEntityListPropertyName()
        {
            List<string> result = new List<string>();
            var props = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (var p in props)
            {
                if (p.PropertyType.IsGenericType)
                {
                    if (p.PropertyType.GetGenericArguments()[0].BaseType == typeof(EntityBase))
                    {
                        Type t = p.PropertyType.GetGenericTypeDefinition();
                        if (t == typeof(IEnumerable<>) || t.GetInterface("IEnumerable`1") != null)
                        {
                            //"IS IEnumerable<>"
                            result.Add(p.Name);
                        }
                    }
                }
            }
            return result.ToArray();
        }

        private void QueryChildResult(string[] childPropNames, List<T> entitys, AdoHelper db)
        {
            string keyTemp = "QCR_" + typeof(T).FullName+"_";
            foreach (string name in childPropNames)
            {
                var cache = PWMIS.Core.MemoryCache<object>.Default;
                string key = keyTemp + name;//修正原来只能有一个子实体类集合的问题
                object cacheValue = cache.Get<string>(
                    key,
                    p => GetRealQueryAndSetChild(p),
                    name);

                RuntimeHandle handle = (RuntimeHandle)cacheValue;

                MethodInfo realMethod = (MethodInfo)MethodInfo.GetMethodFromHandle(handle.MethodHandle, handle.TypeHandle);
                MyAction<List<T>, AdoHelper> d = Delegate.CreateDelegate(typeof(MyAction<List<T>, AdoHelper>), this, realMethod) 
                    as MyAction<List<T>, AdoHelper>;

                d(entitys, db);//等同于 realMethod.Invoke(this, new object[] { entitys,db });
            }
        }

        private RuntimeHandle GetRealQueryAndSetChild(string propName)
        {
            Type thisType = GetType();
            Type propType = typeof(T).GetProperty(propName).PropertyType.GetGenericArguments()[0];
            var method = thisType.GetMethod("QueryAndSetChild", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var realMethod = method.MakeGenericMethod(propType);

            System.RuntimeTypeHandle typeHandle = thisType.TypeHandle;
            return new RuntimeHandle(realMethod.MethodHandle, typeHandle);
        }

        /// <summary>
        /// 查询实体类集合，并同时查询关联的所有的子实体类集合
        /// </summary>
        /// <param name="oql"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static List<T> QueryListWithChild(OQL oql, AdoHelper db)
        {
            List<T> result = QueryList(oql, db);
            if (result.Count > 0)
            {
                EntityQuery<T> query = new EntityQuery<T>();
                //下面采取缓存方式获取属性名字数组
                var cache = PWMIS.Core.MemoryCache<object>.Default;
                string key = "CELPN_"+typeof(T).FullName;
                object cacheValue= cache.Get<EntityQuery<T>>(
                    key, 
                    p => p.GetAllChildEntityListPropertyName(), 
                    query);

                var propNames = (string[])cacheValue;
                query.QueryChildResult(propNames, result, db);
            }
            return result;
        }

        /// <summary>
        /// 插入一个实体对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert(T entity)
        {
            int fieldCount = entity.PropertyNames.Length;
            if (fieldCount == 0)
                throw new Exception("EntityQuery Error:实体类属性字段数量为0");

            //List<string> list = new List<string>();
            //foreach (string key in entity.PropertyList.Keys)
            //    if(entity.PropertyChangedList [key ])//只插入属性更改过的字段
            //        list.Add(key);

            return EntityQuery.InsertInner(entity, entity.PropertyChangedList, DefaultDataBase);
        }

        /// <summary>
        /// 插入一个实体对象集合（使用事务方式）
        /// </summary>
        /// <param name="entityList">实体对象集合</param>
        /// <returns>受影响的行数</returns>
        public int Insert(List<T> entityList)
        {
            int count = 0;
            if (entityList.Count > 0)
            {
                //List<string> list = new List<string>();
                //避免事务方法与非事务方法混用导致连接无法释放，因此这种可能的情况下必须使用DefaultNewDataBase。
                //感谢网友 null 发现此问题 。2014.4.7
                AdoHelper db = DefaultNewDataBase; //原来是 DefaultDataBase;
                db.BeginTransaction();
                try
                {
                    foreach (T entity in entityList)
                    {
                        //list.Clear();
                        //foreach (string key in entity.PropertyList.Keys)
                        //    if(entity.PropertyChangedList [key ])//只插入属性更改过的字段
                        //        list.Add(key);
                        count += EntityQuery.InsertInner(entity, entity.PropertyChangedList, db);
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
        /// 快速插入实体类到数据库，注意，该方法假设要插入的实体类集合中每个实体修改的字段都是一样的。
        /// 同时，插入完成后不会处理“自增”实体的属性，也不会重置实体类的修改状态。
        /// 如果不符合这些要求，请直接调用Insert 方法。
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        public int QuickInsert(List<T> entityList)
        {
            int count = 0;
            if (entityList.Count > 0)
            {
                AdoHelper db = DefaultNewDataBase;

                //
                EntityBase entity = entityList[0];
                List<string> objFields = entity.PropertyChangedList;

                IDataParameter[] paras = new IDataParameter[objFields.Count];

                string tableName = entity.TableName;
                string identityName = entity.IdentityName;
                string sql = "INSERT INTO " + entity.GetSchemeTableName();
                string fields = "";
                string values = "";
                int index = 0;

                foreach (string field in objFields)
                {
                    if (identityName != field)
                    {
                        fields += ",[" + field + "]";
                        string paraName = db.GetParameterChar + "P" + index.ToString();
                        values += "," + paraName;
                        paras[index] = db.GetParameter(paraName, entity.PropertyList(field));
                        //为字符串类型的参数指定长度 edit at 2012.4.23
                        //if (paras[index].Value != null && paras[index].Value.GetType() == typeof(string))
                        //{
                        //    int size=entity.GetStringFieldSize(field);
                        //    if(size>0) //==-1 可能是varcharmax 或者 text类型的字段
                        //    {
                        //        ((IDbDataParameter)paras[index]).Size = size;
                        //    }
                        //}
                        EntityQuery.SetParameterSize(paras[index], entity, field);

                        index++;
                    }
                }
                sql = sql + "(" + fields.TrimStart(',') + ") VALUES (" + values.TrimStart(',') + ")";


                db.BeginTransaction();
                try
                {
                    foreach (T item in entityList)
                    {
                        for (int i = 0; i < paras.Length; i++)
                        {
                            paras[i].Value = item.PropertyList(objFields[i]);
                        }
                        count += db.ExecuteNonQuery(sql, CommandType.Text, paras);

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
        public int Update(T entity)
        {
            return EntityQuery.UpdateInner(entity, entity.PropertyChangedList, DefaultDataBase);
        }

        /// <summary>
        /// 更新一个实体类集合（内部采用事务方式）
        /// </summary>
        /// <param name="entityList">实体类集合</param>
        /// <returns>受影响的行数</returns>
        public int Update(List<T> entityList)
        {
            int count = 0;
            AdoHelper db = DefaultNewDataBase;
            db.BeginTransaction();
            try
            {
                foreach (T entity in entityList)
                {
                    count += EntityQuery.UpdateInner(entity, entity.PropertyChangedList, db);
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
        //private static int DeleteInnerByDB(T entity, CommonDB DB)
        //{
        //    if (entity.PrimaryKeys.Count == 0)
        //        throw new Exception("EntityQuery Error:当前实体类未指定主键字段");
        //    int fieldCount = entity.PropertyNames.Length;
        //    if (fieldCount == 0)
        //        throw new Exception("EntityQuery Error:实体类属性字段数量为0");

        //    //CommonDB DB = MyDB.GetDBHelper();

        //    IDataParameter[] paras = new IDataParameter[fieldCount];
        //    string sql = "DELETE FROM [" + entity.TableName + "] WHERE ";
        //    string values = "";
        //    string condition = "";
        //    int index = 0;

        //    foreach (string key in entity.PrimaryKeys)
        //    {
        //        string paraName = DB.GetParameterChar + "P" + index.ToString();
        //        condition += " AND [" + key + "]=" + paraName;
        //        paras[index] = DB.GetParameter(paraName, entity.PropertyList(key));
        //        index++;
        //    }


        //    sql = sql + values.TrimStart(',') + " " + condition.Substring(" AND ".Length);
        //    int count = DB.ExecuteNonQuery(sql, CommandType.Text, paras);
        //    return count;
        //}

        /// <summary>
        /// 从数据库删除实体对象对应的记录
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public int Delete(T entity)
        {
            //return DeleteInnerByDB(entity, DefaultDataBase);
            return EntityQuery.DeleteInnerByDB(entity, DefaultDataBase);
        }

        /// <summary>
        /// 删除一个实体类集合（内部采用事务方式）
        /// </summary>
        /// <param name="entityList">实体类集合</param>
        /// <returns>受影响的行数</returns>
        public int Delete(List<T> entityList)
        {
            int count = 0;
            AdoHelper db = DefaultNewDataBase;
            db.BeginTransaction();
            try
            {
                foreach (T entity in entityList)
                {
                    count += EntityQuery.DeleteInnerByDB(entity, db);
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
        public int ExecuteOql(OQL oql)
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
        /// 执行一个插入数据的OQL查询，返回自增列的值，如果是-1，表示没有自增值，如果是0，表示操作未成功，其它值表示自增值。
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public int ExecuteInsrtOql(OQL q)
        {
            DefaultDataBase.BeginTransaction();
            try
            {
                int count = ExecuteOql(q, DefaultDataBase);
                int result = 0;
                EntityCommand ec = new EntityCommand(q.currEntity, DefaultDataBase);
                string insertKey= ec.GetInsertKey();
                if (!string.IsNullOrEmpty(insertKey))
                {
                    var identity = DefaultDataBase.ExecuteScalar(insertKey);
                    result = Convert.ToInt32(identity);
                }
                else
                {
                    result = -1;
                }
                DefaultDataBase.Commit();
                return result;
            }
            catch (Exception ex)
            {
                DefaultDataBase.Rollback();
                return 0;
            }
          
        }

        /// <summary>
        ///  执行一个不返回结果集的OQL查询表达式，例如更新，删除实体类的操作。使用自定义的数据访问对象进行操作
        /// </summary>
        /// <param name="oql"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static int ExecuteOql(OQL oql, AdoHelper db)
        {
            return EntityQuery.ExecuteOql(oql, db);
        }

        //private static int InsertInner(T entity, List<string> objFields, CommonDB DB)
        //{
        //    if (objFields == null || objFields.Count == 0)
        //        return 0;

        //    IDataParameter[] paras = new IDataParameter[objFields.Count];
        //    //CommonDB DB = MyDB.GetDBHelper();

        //    string tableName = entity.TableName;
        //    string identityName = entity.IdentityName;
        //    string sql = "INSERT INTO [" + tableName + "]";
        //    string fields = "";
        //    string values = "";
        //    int index = 0;

        //    foreach (string field in objFields)
        //    {
        //        if (identityName != field)
        //        {
        //            fields += ",[" + field + "]";
        //            string paraName = DB.GetParameterChar + "P" + index.ToString();
        //            values += "," + paraName;
        //            paras[index] = DB.GetParameter(paraName, entity.PropertyList(field));
        //            //为字符串类型的参数指定长度 edit at 2012.4.23
        //            if (paras[index].Value != null && paras[index].Value.GetType() == typeof(string))
        //            {
        //                ((IDbDataParameter)paras[index]).Size = entity.GetStringFieldSize(field);
        //            }

        //            index++;
        //        }
        //    }
        //    sql = sql + "(" + fields.TrimStart(',') + ") VALUES (" + values.TrimStart(',') + ")";

        //    int count = 0;

        //    if (identityName != "")
        //    {
        //        //有自增字段
        //        object id = entity.PropertyList(identityName);
        //        count = DB.ExecuteInsertQuery(sql, CommandType.Text, paras, ref id);
        //        entity.setProperty(identityName, Convert.ToInt32(id));
        //    }
        //    else
        //    {
        //        count = DB.ExecuteNonQuery(sql, CommandType.Text, paras);
        //    }
        //    if (count > 0)
        //        entity.ResetChanges();

        //    return count;

        //}

        //如果没有字段需要更新,则退出 update at 2012.11.4
        //private static int UpdateInner(T entity, List<string> objFields, CommonDB DB)
        //{
        //    if (objFields == null || objFields.Count == 0)
        //        return 0;
        //    if (entity.PrimaryKeys.Count == 0)
        //        throw new Exception("EntityQuery Error:当前实体类未指定主键字段");
        //    int fieldCount = objFields.Count + entity.PrimaryKeys.Count;
        //    if (fieldCount == 0)
        //        throw new Exception("EntityQuery Error:实体类属性字段数量为0");

        //    //CommonDB DB = MyDB.GetDBHelper();
        //    IDataParameter[] paras = new IDataParameter[fieldCount];
        //    string sql = "UPDATE [" + entity.TableName + "] SET ";
        //    string values = "";
        //    string condition = "";
        //    int index = 0;

        //    //为解决Access问题，必须确保参数的顺序，故对条件参数的处理分开2次循环
        //    List<string> pkFields = new List<string>();
        //    //先处理更新的字段
        //    foreach (string field in objFields)
        //    {
        //        if (entity.PrimaryKeys.Contains(field))
        //        {
        //            pkFields.Add(field);
        //            continue;
        //        }
        //        string paraName = DB.GetParameterChar + "P" + index.ToString();
        //        values += ",[" + field + "]=" + paraName;
        //        paras[index] = DB.GetParameter(paraName, entity.PropertyList(field));

        //        //为字符串类型的参数指定长度 edit at 2012.4.23
        //        if (paras[index].Value != null && paras[index].Value.GetType() == typeof(string))
        //        {
        //            ((IDbDataParameter)paras[index]).Size = entity.GetStringFieldSize(field);
        //        }
        //        index++;
        //    }
        //    //再处理条件
        //    foreach (string field in pkFields)
        //    {
        //        string paraName = DB.GetParameterChar + "P" + index.ToString();
        //        //当前字段为主键，不能被更新
        //        condition += " AND [" + field + "]=" + paraName;
        //        paras[index] = DB.GetParameter(paraName, entity.PropertyList(field));

        //        //为字符串类型的参数指定长度 edit at 2012.4.23
        //        if (paras[index].Value != null && paras[index].Value.GetType() == typeof(string))
        //        {
        //            ((IDbDataParameter)paras[index]).Size = entity.GetStringFieldSize(field);
        //        }
        //        index++;
        //    }

        //    if (condition == "")
        //    {
        //        foreach (string key in entity.PrimaryKeys)
        //        {
        //            string paraName = DB.GetParameterChar + "P" + index.ToString();
        //            condition += " AND [" + key + "]=" + paraName;
        //            paras[index] = DB.GetParameter(paraName, entity.PropertyList(key));
        //            index++;
        //        }
        //    }
        //    sql = sql + values.TrimStart(',') + " WHERE " + condition.Substring(" AND ".Length);
        //    int count = DB.ExecuteNonQuery(sql, CommandType.Text, paras);

        //    if (count > 0)
        //        entity.ResetChanges();

        //    return count;
        //}





        #endregion

        #region 实体操作实例方法

        
        private bool isNew = false;//当前实体是否是一个新实体（对应数据库而言）
        private List<string> changedFields = new List<string>();
        private List<string> selectFields = new List<string>();
        /// <summary>
        /// 当前操作的实体类
        /// </summary>
        public T CurrentEntity { get; set; }
        /// <summary>
        /// 使用一个实体类初始化本类，将探测数据库中是否存在本实体类对应的记录（为提高效率，建议使用另外一个重载）
        /// </summary>
        /// <param name="entity">实体类</param>
        public EntityQuery(T entity)
        {
            isNew = !ExistsEntity(entity);//已经存在，说明实体是旧的
            init(entity);
        }



        /// <summary>
        /// 使用一个实体类初始化本类，并指明该实体类持久化时新增还是修改
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="newEntity">是否是新实体</param>
        public EntityQuery(T entity, bool newEntity)
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
            CurrentEntity = entity;
        }

        /// <summary>
        /// 保存(使用指定的属性)数据到数据库。如果属性值自使用本类以来没有改变过或者没有为本方法指定实体对象的属性字段，那么将不会更新任何数据。
        /// </summary>
        /// <param name="fields">实体类属性值列表</param>
        /// <returns>操作数据库影响的行数</returns>
        public int Save(params object[] fields)
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
            //只修改属性更改过的字段
            if (this.CurrentEntity == null)
                throw new Exception("未指定当前要保存的实体类对象！");
            return InnerSaveAllChanges(db, this.CurrentEntity.PropertyChangedList);
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
                count = EntityQuery.InsertInner(this.CurrentEntity, objFields, db);
                if (count > 0)
                    isNew = false;//保存成功，当前不再是新实体，下次保存应该使用修改

            }
            else
            {
                count = EntityQuery.UpdateInner(this.CurrentEntity, objFields, db);
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
            return EntityQuery.DeleteInnerByDB(entity, db);
        }

        /// <summary>
        /// 更新实体对象的动态实例方法，CommonDB 可以是开启了事务的对象
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="db">数据库对象</param>
        /// <returns>返回操作受影响的行数</returns>
        public int Update(T entity, CommonDB db)
        {
            return EntityQuery.UpdateInner(entity, entity.PropertyChangedList, db);
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

            return EntityQuery.InsertInner(entity, entity.PropertyChangedList, db);
        }


        #endregion

    }

    /// <summary>
    /// 实体类查询，主要用于动态生成的实体类进行查询。如果是明确定义的实体类，推荐使用EntityQuery 的泛型类
    /// </summary>
    public class EntityQuery : IEntityQuery
    {
        #region 类私有变量

        private EntityBase currEntity;
        private bool isNew = false;//当前实体是否是一个新实体（对应数据库而言）
        private List<string> changedFields = new List<string>();
        private List<string> selectFields = new List<string>();
        #endregion

        #region 构造函数
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

        /// <summary>
        /// 使用一个实体类初始化本类，将探测数据库中是否存在本实体类对应的记录（为提高效率，建议使用另外一个重载）
        /// </summary>
        /// <param name="entity">实体类</param>
        public EntityQuery(EntityBase entity)
        {
            isNew = !ExistsEntity(entity);//已经存在，说明实体是旧的
            init(entity);
        }

        /// <summary>
        /// 使用一个实体类初始化本类，并指明该实体类持久化时新增还是修改
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="newEntity">是否是新实体</param>
        public EntityQuery(EntityBase entity, bool newEntity)
        {
            isNew = newEntity;
            init(entity);
        }
        #endregion

        #region 公开的静态方法

        /// <summary>
        ///  执行一个不返回结果集的OQL查询表达式，例如更新，删除实体类的操作。使用自定义的数据访问对象进行操作
        /// </summary>
        /// <param name="oql"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static int ExecuteOql(OQL oql, AdoHelper db)
        {
            string sql = oql.ToString();
            oql.Dispose();

            if (oql.Parameters.Count > 0)
            {
                IDataParameter[] paras = EntityQueryAnonymous.GetParameters(oql.Parameters, db);
                return db.ExecuteNonQuery(sql, CommandType.Text, paras);
            }
            else
            {
                return db.ExecuteNonQuery(sql);
            }

        }


        /// <summary>
        /// 万能根据数据阅读器对象，将结果查询到（实体）对象集合(注意查询完毕将自动释放该阅读器对象)
        /// </summary>
        /// <typeparam name="T">元素类型，可以是EntityBase,IReadData 派生类型，其它接口类型，或者POCO类型的对象</typeparam>
        /// <param name="reader">数据阅读器对象</param>
        /// <param name="tableName">可能要映射的表名字</param>
        /// <returns>实体类集合</returns>
        public static List<T> QueryList<T>(System.Data.IDataReader reader,string tableName) where T : class
        {
            if (tableName == null) tableName = "";
            List<T> list = new List<T>();
            using (reader)
            {
                if (reader.Read())
                {
                    int fcount = reader.FieldCount;
                    string[] names = new string[fcount];

                    for (int i = 0; i < fcount; i++)
                        names[i] = reader.GetName(i);

                    T t = EntityBuilder.CreateEntity<T>();
                    if (t is EntityBase)
                    {
                        EntityBase entity = t as EntityBase;
                        entity.PropertyNames = names;
                        if (!string.IsNullOrEmpty(tableName))
                            entity.MapNewTableName(tableName);
                        do
                        {
                            object[] values = new object[fcount];
                            reader.GetValues(values);

                            EntityBase entityNew = (EntityBase)entity.Clone();
                            entityNew.PropertyValues = values;

                            list.Add(entityNew as T);
                        } while (reader.Read());
                    }
                    else if (t is IReadData)
                    {
                        do
                        {
                            ((IReadData)t).ReadData(reader, fcount, names);
                            list.Add(t);
                            t = EntityBuilder.CreateEntity<T>();

                        } while (reader.Read());
                    }
                    else
                    {
                        INamedMemberAccessor[] accessors = new INamedMemberAccessor[fcount];
                        DelegatedReflectionMemberAccessor drm = new DelegatedReflectionMemberAccessor();
                        for (int i = 0; i < fcount; i++)
                        {
                            accessors[i] = drm.FindAccessor<T>(reader.GetName(i));
                        }

                        do
                        {
                            for (int i = 0; i < fcount; i++)
                            {
                                if (!reader.IsDBNull(i))
                                    accessors[i].SetValue(t, reader.GetValue(i));
                            }
                            list.Add(t);
                            t = EntityBuilder.CreateEntity<T>();
                        } while (reader.Read());
                    }
                }
            }
            return list;
        }

        public static T QueryObject<T>(System.Data.IDataReader reader) where T : class
        {
            using (reader)
            {
                if (reader.Read())
                {
                    int fcount = reader.FieldCount;
                    string[] names = new string[fcount];

                    for (int i = 0; i < fcount; i++)
                        names[i] = reader.GetName(i);


                    object[] values = new object[fcount];
                    reader.GetValues(values);

                    T t = EntityBuilder.CreateEntity<T>();
                    EntityBase entity = t as EntityBase;
                    entity.PropertyNames = names;
                    entity.PropertyValues = values;

                    return t;
                }

            }
            return null;
        }

        /// <summary>
        /// 查询实体类集合。如果开启了分页且OQL设置的记录总数为0，直接返回空集合。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oql"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static List<T> QueryList<T>(OQL oql, AdoHelper db) where T : class
        {
            if (oql.PageEnable && oql.PageWithAllRecordCount <= 0)
                return new List<T>();
            IDataReader reader = EntityQueryAnonymous.ExecuteDataReader(oql, db, typeof(T));
            return QueryList<T>(reader,oql.GetEntityTableName());
        }

        public static T QueryObject<T>(OQL oql, AdoHelper db) where T : class
        {
            IDataReader reader = EntityQueryAnonymous.ExecuteDataReader(oql, db, typeof(T), true);
            return QueryObject<T>(reader);
        }


        #endregion

        #region 程序集内部静态方法
        internal static void SetParameterSize(IDataParameter para, EntityBase entity, string field)
        {
            if (para.Value != null && para.Value.GetType() == typeof(string))
            {
                int size = entity.GetStringFieldSize(field);
                if (size > 0) //==-1 可能是varcharmax 或者 text类型的字段
                {
                    ((IDbDataParameter)para).Size = size;
                }
            }
        }

        internal static int DeleteInnerByDB(EntityBase entity, CommonDB DB)
        {
            if (entity.PrimaryKeys.Count == 0)
                throw new Exception("EntityQuery Error:当前实体类未指定主键字段");
            int fieldCount = entity.PropertyNames.Length;
            if (fieldCount == 0)
                throw new Exception("EntityQuery Error:实体类属性字段数量为0");

            //CommonDB DB = MyDB.GetDBHelper();

            IDataParameter[] paras = new IDataParameter[fieldCount];
            string sql = "DELETE FROM " + entity.GetSchemeTableName() + " WHERE ";
            string values = "";
            string condition = "";
            int index = 0;

            foreach (string key in entity.PrimaryKeys)
            {
                string paraName = DB.GetParameterChar + "P" + index.ToString();
                condition += " AND [" + key + "]=" + paraName;
                paras[index] = DB.GetParameter(paraName, entity.PropertyList(key));
                index++;
            }


            sql = sql + values.TrimStart(',') + " " + condition.Substring(" AND ".Length);
            int count = DB.ExecuteNonQuery(sql, CommandType.Text, paras);
            return count;
        }

        internal static bool ExistsEntity(EntityBase entity, CommonDB DB)
        {
            if (entity.PrimaryKeys.Count == 0)
                throw new Exception("EntityQuery Error:当前实体类未指定主键字段");
            if (entity.PropertyNames == null)
                throw new Exception("EntityQuery Error:当前实体类属性字段未初始化");
            int fieldCount = entity.PropertyNames.Length;
            if (fieldCount == 0)
                throw new Exception("EntityQuery Error:实体类属性字段数量为0");
            IDataParameter[] paras = new IDataParameter[fieldCount];
            string sql = "SELECT Count(*) ";
            //string fields = "";
            string condition = "";
            int index = 0;


            foreach (string key in entity.PrimaryKeys)
            {
                string paraName = DB.GetParameterChar + "P" + index.ToString();
                condition += " AND [" + key + "]=" + paraName;
                paras[index] = DB.GetParameter(paraName, entity.PropertyList(key));
                index++;
            }

            sql = sql + " FROM " + entity.GetSchemeTableName () + " WHERE " + condition.Substring(" AND ".Length);
            object obj = DB.ExecuteScalar(sql, CommandType.Text, paras);
            int count = Convert.ToInt32(obj);

            return count > 0;
        }

        internal static bool FillEntity(EntityBase entity, CommonDB DB)
        {
            if (entity.PrimaryKeys.Count == 0)
                throw new Exception("EntityQuery Error:当前实体类未指定主键字段");
            if (entity.PropertyNames == null)
                throw new Exception("EntityQuery Error:当前实体类属性字段未初始化");
            int fieldCount = entity.PropertyNames.Length;
            if (fieldCount == 0)
                throw new Exception("EntityQuery Error:实体类属性字段数量为0");
            IDataParameter[] paras = new IDataParameter[entity.PrimaryKeys.Count];
            string sql = "SELECT ";
            //string fields = "";
            string condition = "";
            int index = 0;


            foreach (string key in entity.PrimaryKeys)
            {
                string paraName = DB.GetParameterChar + "P" + index.ToString();
                condition += " AND [" + key + "]=" + paraName;
                paras[index] = DB.GetParameter(paraName, entity.PropertyList(key));
                index++;
            }

            sql = sql + string.Join(",", CommonUtil.PrepareSqlFields(entity.PropertyNames)) + " FROM [" + entity.TableName + "] WHERE " + condition.Substring(" AND ".Length);
            IDataReader reader = DB.ExecuteDataReader(sql, CommandType.Text, paras);

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

        internal static int InsertInner(EntityBase entity, List<string> objFields, CommonDB DB)
        {
            if (objFields == null || objFields.Count == 0)
                return 0;

            IDataParameter[] paras = new IDataParameter[objFields.Count];

            string tableName = entity.TableName;
            string identityName = entity.IdentityName;
            string sql = "INSERT INTO " + entity.GetSchemeTableName();
            string fields = "";
            string values = "";
            int index = 0;

            //获取实体属性信息缓存
            var entityFieldsCache = EntityFieldsCache.Item(entity.GetType());

            foreach (string field in objFields)
            {
                if (identityName != field)
                {
                    fields += ",[" + field + "]";
                    string paraName = DB.GetParameterChar + "P" + index.ToString();
                    values += "," + paraName;
                    paras[index] = DB.GetParameter(paraName, entity.PropertyList(field));

                    //从缓存中获取当前field所对应的类型
                    Type fieldType = entityFieldsCache.GetPropertyType(field);

                    if (fieldType == typeof(string) && paras[index].Value != null)
                        //为字符串类型的参数指定长度 edit at 2012.4.23
                        //((IDbDataParameter)paras[index]).Size = entity.GetStringFieldSize(field);
                         SetParameterSize(paras[index], entity, field);
                    else if (fieldType == typeof(byte[]))
                        //为字节类型指定转换类型，防止空值时被当作字符串类型
                        paras[index].DbType = DbType.Binary;

                    index++;
                }
            }
            sql = sql + "(" + fields.TrimStart(',') + ") VALUES (" + values.TrimStart(',') + ")";

            int count = 0;

            if (identityName != "")
            {
                //有自增字段
                object id = entity.PropertyList(identityName);

                EntityCommand ec = new EntityCommand(entity, DB);
                string insertKey = ec.GetInsertKey();

                count = DB.ExecuteInsertQuery(sql, CommandType.Text, paras, ref id,insertKey);
                entity.setProperty(identityName, Convert.ToInt32(id));
            }
            else
            {
                count = DB.ExecuteNonQuery(sql, CommandType.Text, paras);
            }
            if (count > 0)
                entity.ResetChanges();

            return count;

        }

        internal static int UpdateInner(EntityBase entity, List<string> objFields, CommonDB DB)
        {
            if (objFields == null || objFields.Count == 0)
                return 0;
            if (entity.PrimaryKeys.Count == 0)
                throw new Exception("EntityQuery Error:当前实体类未指定主键字段");
            int fieldCount = objFields.Count + entity.PrimaryKeys.Count;
            if (fieldCount == 0)
                throw new Exception("EntityQuery Error:实体类属性字段数量为0");

            //CommonDB DB = MyDB.GetDBHelper();
            IDataParameter[] paras = new IDataParameter[fieldCount];
            string sql = "UPDATE " + entity.GetSchemeTableName() + " SET ";
            string values = "";
            string condition = "";
            int index = 0;

            //获取实体属性信息缓存
            var entityFieldsCache = EntityFieldsCache.Item(entity.GetType());

            //为解决Access问题，必须确保参数的顺序，故对条件参数的处理分开2次循环
            List<string> pkFields = new List<string>();
            //先处理更新的字段，主键字段不会被更新
            foreach (string field in objFields)
            {
                if (entity.PrimaryKeys.Contains(field))
                {
                    pkFields.Add(field);
                    continue;
                }
                string paraName = DB.GetParameterChar + "P" + index.ToString();
                values += ",[" + field + "]=" + paraName;
                paras[index] = DB.GetParameter(paraName, entity.PropertyList(field));

                //从缓存中获取当前field所对应的类型
                Type fieldType = entityFieldsCache.GetPropertyType(field);

                if (fieldType == typeof(string) && paras[index].Value != null)
                    //为字符串类型的参数指定长度 edit at 2012.4.23
                    //((IDbDataParameter)paras[index]).Size = entity.GetStringFieldSize(field);
                    SetParameterSize(paras[index], entity, field);
                else if (fieldType == typeof(byte[]))
                    //为字节类型指定转换类型，防止空值时被当作字符串类型
                    paras[index].DbType = DbType.Binary;

                index++;
            }
            //再处理条件
            foreach (string field in pkFields)
            {
                string paraName = DB.GetParameterChar + "P" + index.ToString();
                //当前字段为主键，不能被更新
                condition += " AND [" + field + "]=" + paraName;
                paras[index] = DB.GetParameter(paraName, entity.PropertyList(field));

                //从缓存中获取当前field所对应的类型
                Type fieldType = entityFieldsCache.GetPropertyType(field);

                if (fieldType == typeof(string) && paras[index].Value != null)
                    //为字符串类型的参数指定长度 edit at 2012.4.23
                    //((IDbDataParameter)paras[index]).Size = entity.GetStringFieldSize(field);
                    SetParameterSize(paras[index], entity, field);
                else if (fieldType == typeof(byte[]))
                    //为字节类型指定转换类型，防止空值时被当作字符串类型
                    paras[index].DbType = DbType.Binary;

                index++;
            }

            if (condition == "")
            {
                foreach (string key in entity.PrimaryKeys)
                {
                    string paraName = DB.GetParameterChar + "P" + index.ToString();
                    condition += " AND [" + key + "]=" + paraName;
                    paras[index] = DB.GetParameter(paraName, entity.PropertyList(key));

                    //从缓存中获取当前field所对应的类型
                    Type fieldType = entityFieldsCache.GetPropertyType(key);

                    if (fieldType == typeof(string) && paras[index].Value != null)
                        //为字符串类型的参数指定长度 edit at 2012.4.23
                        //((IDbDataParameter)paras[index]).Size = entity.GetStringFieldSize(key);
                        SetParameterSize(paras[index], entity, key);
                    else if (fieldType == typeof(byte[]))
                        //为字节类型指定转换类型，防止空值时被当作字符串类型
                        paras[index].DbType = DbType.Binary;

                    index++;
                }
            }
            if (values == "")
                throw new Exception("未指定主键字段之外的要更新的字段，请检查实体类的属性值是否更改过。");
            sql = sql + values.TrimStart(',') + " WHERE " + condition.Substring(" AND ".Length);
            int count = DB.ExecuteNonQuery(sql, CommandType.Text, paras);

            if (count > 0)
                entity.ResetChanges();

            return count;
        }

        #endregion

        #region 类私有方法

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

        private void init(EntityBase entity)
        {
            entity.PropertyGetting += new EventHandler<PropertyGettingEventArgs>(entity_PropertyGetting);
            entity.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(entity_PropertyChanged);
            currEntity = entity;
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
        #endregion

        #region IEntityQuery 成员

        private AdoHelper _DefaultDataBase;
        /// <summary>
        /// 获取或者设置默认的数据库操作对象，如果未设置将采用默认的配置进行实例化数据库操作对象。
        /// 支持读写分离模式
        /// </summary>
        public AdoHelper DefaultDataBase
        {
            get
            {
                if (_DefaultDataBase == null)
                    return MyDB.Instance;
                else
                    return _DefaultDataBase;
            }
            set
            {
                _DefaultDataBase = value;
            }
        }

        /// <summary>
        /// 获取当期类设定的数据连接对象，与DefaultDataBase 属性不同的是，如果未设置过 DefaultDataBase，
        /// 则使用MyDB.GetDBHelper() 获得一个新的默认配置的数据访问对象实例；
        /// 如果设置过，则使用 DefaultDataBase 设置的数据访问对象。
        /// </summary>
        public AdoHelper DefaultNewDataBase
        {
            get
            {
                if (_DefaultDataBase == null)
                    return MyDB.GetDBHelper();
                else
                    return _DefaultDataBase;
            }
        }
        /// <summary>
        /// 删除一个实体类
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Delete(EntityBase entity)
        {
            return DeleteInnerByDB(entity, DefaultDataBase);
        }
        /// <summary>
        /// 删除一个实体类集合
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        public int Delete(List<EntityBase> entityList)
        {
            int count = 0;
            AdoHelper db = DefaultNewDataBase;
            db.BeginTransaction();
            try
            {
                foreach (EntityBase entity in entityList)
                {
                    count += EntityQuery.DeleteInnerByDB(entity, db);
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
        /// 用指定的数据访问对象，来删除一个实体类的数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public int Delete(EntityBase entity, CommonDB db)
        {
            return DeleteInnerByDB(entity, db);
        }
        /// <summary>
        /// 执行OQL查询
        /// </summary>
        /// <param name="oql"></param>
        /// <returns></returns>
        public int ExecuteOql(OQL oql)
        {
            return ExecuteOql(oql, DefaultDataBase);
        }
        /// <summary>
        /// 检测实体类是否在数据库存在
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ExistsEntity(EntityBase entity)
        {
            return ExistsEntity(entity, DefaultDataBase);
        }
        /// <summary>
        /// 填充实体对象，必须有主键值才可以填充成功
        /// </summary>
        /// <param name="entity">实体对象实例，必须为主键字段属性设置值</param>
        /// <returns>返回填充是否成功</returns>
        public bool FillEntity(EntityBase entity)
        {
            return FillEntity(entity, DefaultDataBase);
        }
        /// <summary>
        /// 插入一组实体类到数据库（内部使用事物）
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        public int Insert(List<EntityBase> entityList)
        {
            int count = 0;
            if (entityList.Count > 0)
            {
                //List<string> list = new List<string>();

                AdoHelper db = DefaultNewDataBase;
                db.BeginTransaction();
                try
                {
                    foreach (EntityBase entity in entityList)
                    {
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
        /// 插入一个实体类的数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert(EntityBase entity)
        {
            int fieldCount = entity.PropertyNames.Length;
            if (fieldCount == 0)
                throw new Exception("EntityQuery Error:实体类属性字段数量为0");

            return InsertInner(entity, entity.PropertyChangedList, DefaultDataBase);
        }
        /// <summary>
        /// 使用指定的数据访问对象，插入实体类数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public int Insert(EntityBase entity, CommonDB db)
        {
            int fieldCount = entity.PropertyNames.Length;
            if (fieldCount == 0)
                throw new Exception("EntityQuery Error:实体类属性字段数量为0");

            return InsertInner(entity, entity.PropertyChangedList, db);
        }
        /// <summary>
        /// 使用指定的数据访问对象，将实体类指定的属性的数据保存到数据库
        /// </summary>
        /// <param name="db">数据访问对象</param>
        /// <param name="fields">实体类指定的属性</param>
        /// <returns></returns>
        public int Save(CommonDB db, params object[] fields)
        {
            //注意调试陷阱，如果你在断点后再次到本方法查看变量的值，selectFields 的数量会改变
            List<string> objFields = fields.Length > 0 ? selectFields : changedFields;
            if (objFields.Count == 0)
                return 0;
            return InnerSaveAllChanges(db, objFields);
        }
        /// <summary>
        /// 将实体类指定的属性的数据保存到数据库
        /// </summary>
        /// <param name="fields">实体类指定的属性</param>
        /// <returns></returns>
        public int Save(params object[] fields)
        {
            return Save(DefaultDataBase, fields);
        }
        /// <summary>
        /// 保存自实体类申明以来，所有做过的修改到数据库。
        /// </summary>
        /// <returns>操作受影响的行数</returns>
        public int SaveAllChanges()
        {
            return SaveAllChanges(DefaultDataBase);
        }
        /// <summary>
        /// 保存自实体类申明以来，所有做过的修改到数据库。
        /// </summary>
        /// <param name="db">数据库访问对象实例</param>
        /// <returns>操作受影响的行数</returns>
        public int SaveAllChanges(CommonDB db)
        {
            return InnerSaveAllChanges(db, this.currEntity.PropertyChangedList);
        }
        /// <summary>
        /// 更新一个实体类集合（内部采用事务方式）
        /// </summary>
        /// <param name="entityList">实体类集合</param>
        /// <returns>受影响的行数</returns>
        public int Update(List<EntityBase> entityList)
        {
            int count = 0;
            AdoHelper db = DefaultNewDataBase;
            db.BeginTransaction();
            try
            {
                foreach (EntityBase entity in entityList)
                {
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
        /// 更新一个实体类
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="db">指定的数据访问对象</param>
        /// <returns></returns>
        public int Update(EntityBase entity, CommonDB db)
        {
            return UpdateInner(entity, entity.PropertyChangedList, db);
        }
        /// <summary>
        /// 更新一个实体类
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Update(EntityBase entity)
        {
            return UpdateInner(entity, entity.PropertyChangedList, DefaultDataBase);
        }

        #endregion
    }
}
