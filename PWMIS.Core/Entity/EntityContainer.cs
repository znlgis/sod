/*
 * PDF.NET 数据开发框架
 * http://www.pwmis.com/sqlmap
 * 
 * 详细内容，请参看“打造轻量级的实体类数据容器”
 * （ http://www.cnblogs.com/bluedoctor/archive/2011/05/23/2054541.html）
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Data;
using PWMIS.Core;

namespace PWMIS.DataMap.Entity
{
    /// <summary>
    /// 实体数据容器
    /// </summary>
    public class EntityContainer
    {
        private OQL oql;
        private AdoHelper db;
        private string[] fieldNames;
        private List<object[]> Values;
        private object[] currValue;

        /// <summary>
        /// 以 TResult为输入参数，并返回此类型的函数的委托定义
        /// </summary>
        /// <typeparam name="TResult">输入类型</typeparam>
        /// <param name="arg">参数</param>
        /// <returns></returns>
        public delegate TResult Func<TResult>(TResult arg);

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public EntityContainer()
        { 
        
        }

        /// <summary>
        /// 使用OQL表达式和数据访问对象实例初始化
        /// </summary>
        /// <param name="oql">OQL表达式</param>
        /// <param name="db">数据访问对象实例</param>
        public EntityContainer(OQL oql, AdoHelper db)
        {
            this.OQL = oql;
            this.DataBase = db;
        }

        /// <summary>
        /// 使用查询表达式初始化
        /// </summary>
        /// <param name="oql"></param>
        public EntityContainer(OQL oql)
        {
            this.OQL = oql;
        }

        /// <summary>
        /// 查询表达式
        /// </summary>
        public OQL OQL
        {
            get { return this.oql; }
            set { this.oql = value; }
        }

        /// <summary>
        /// 数据访问对象
        /// </summary>
        public AdoHelper DataBase
        {
            get
            {
                if (db == null)
                    db = PWMIS.DataProvider.Adapter.MyDB.Instance;
                return db;
            }
            set { db = value; }
        }

        private IDataReader ExecuteDataReader(OQL oql, AdoHelper db)
        {
            string sql = "";
            sql = oql.ToString();

            //处理实体类分页 2010.6.20
            if (oql.PageEnable)
            {
                switch (db.CurrentDBMSType)
                {
                    case PWMIS.Common.DBMSType.Access:
                    case PWMIS.Common.DBMSType.SqlServer:
                    case PWMIS.Common.DBMSType.SqlServerCe:
                        if (oql.HaveJoinOpt )
                        {
                            if (oql.PageNumber <= 1) //仅限定记录条数
                            {
                                sql = "Select Top " +oql.PageSize +" "+ sql.Trim().Substring("SELECT ".Length);

                            }
                            else //必须采用复杂分页方案
                            {
                                sql = PWMIS.Common.SQLPage.MakeSQLStringByPage(PWMIS.Common.DBMSType.SqlServer, sql, "", oql.PageSize, oql.PageNumber, 999);
                            
                            }
                        }
                        else
                        {
                            //单表查询的情况
                            if (oql.PageOrderDesc)
                                sql = PWMIS.Common.SQLPage.GetDescPageSQLbyPrimaryKey(oql.PageNumber, oql.PageSize, oql.sql_fields, oql.sql_table, oql.PageField, oql.sql_condition);
                            else
                                sql = PWMIS.Common.SQLPage.GetAscPageSQLbyPrimaryKey(oql.PageNumber, oql.PageSize, oql.sql_fields, oql.sql_table, oql.PageField, oql.sql_condition);
                        }
                        
                        break;
                    case PWMIS.Common.DBMSType.Oracle:
                        sql = PWMIS.Common.SQLPage.MakeSQLStringByPage(PWMIS.Common.DBMSType.Oracle, sql, "", oql.PageSize, oql.PageNumber, 999);
                        break;
                    case PWMIS.Common.DBMSType.MySql:
                        sql = PWMIS.Common.SQLPage.MakeSQLStringByPage(PWMIS.Common.DBMSType.MySql, sql, "", oql.PageSize, oql.PageNumber, 999);
                        break;
                    case PWMIS.Common.DBMSType.PostgreSQL:
                        sql = PWMIS.Common.SQLPage.MakeSQLStringByPage(PWMIS.Common.DBMSType.PostgreSQL, sql, "", oql.PageSize, oql.PageNumber, 999);
                        break;
                    default:
                        throw new Exception("实体类分页错误：不支持此种类型的数据库分页。");

                }

            }

            IDataReader reader = null;
            if (oql.Parameters != null && oql.Parameters.Count > 0)
            {
                int fieldCount = oql.Parameters.Count;
                IDataParameter[] paras = new IDataParameter[fieldCount];
                int index = 0;

                foreach (string name in oql.Parameters.Keys)
                {
                    paras[index] = db.GetParameter(name, oql.Parameters[name]);
                    index++;
                }
                reader = db.ExecuteDataReader(sql, CommandType.Text, paras);
            }
            else
            {
                reader = db.ExecuteDataReader(sql);
            }
            return reader;
        }

        /// <summary>
        /// 执行OQL查询，并将查询结果缓存
        /// </summary>
        /// <returns>结果的行数</returns>
        public int Execute()
        {
            IDataReader reader = ExecuteDataReader(this.OQL, this.DataBase);
            return Execute(reader);
        }

        /// <summary>
        /// 执行DataReader查询，并将查询结果缓存
        /// </summary>
        /// <param name="reader">数据阅读器</param>
        /// <returns>结果行数</returns>
        public int Execute(IDataReader reader)
        {
            List<object[]> list = new List<object[]>();
            using (reader)
            {
                int fcount = reader.FieldCount;
                fieldNames = new string[fcount];
                if (reader.Read())
                {
                    
                    object[] values = null;

                    for (int i = 0; i < fcount; i++)
                        fieldNames[i] = reader.GetName(i);

                    do
                    {
                        values = new object[fcount];
                        reader.GetValues(values);
                        list.Add(values);
                    } while (reader.Read());

                }
            }
            this.Values = list;
            return list.Count;
        }

        /// <summary>
        /// 获取容器数据中的字段名数组
        /// </summary>
        public string[] FieldNames
        {
            get { return this.fieldNames; }
        }
        /// <summary>
        /// 获取容器数据中的每一行的值
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object[]> GetItemValues()
        {
            if (this.Values != null && this.fieldNames != null)
            {
                foreach (object[] itemValues in this.Values)
                {
                    yield return itemValues;
                }
            }
        }

        /// <summary>
        /// 将数据从容器中映射到实体中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> Map<T>() where T : EntityBase
        {
            if (this.Values == null)
            {
                int rowsCount = this.Execute();
                if(rowsCount<=0)
                    yield break;
            
            }
            if (this.Values != null && this.fieldNames != null)
            {
                if (this.Values.Count == 0)
                    yield break;

                Dictionary<string, int> dictNameIndex = new Dictionary<string, int>();
                T entity = Activator.CreateInstance<T>();
                //查找字段匹配情况
                //entity.PropertyNames 存储的仅仅是查询出来的列名称，由于有连表查询，
                //如果要映射到指定的实体，还得检查当前列对应的表名称
                if (this.OQL.sql_fields.Contains("[" + entity.TableName + "]"))
                {
                    //是连表查询
                    for (int i = 0; i < this.fieldNames.Length; i++)
                    {
                        for (int j = 0; j < entity.PropertyNames.Length; j++)
                        {
                            string cmpString = "[" + entity.TableName + "].[" + entity.PropertyNames[j]+"]";
                            if (this.OQL.sql_fields.Contains(cmpString ) )
                            {
                                dictNameIndex[this.fieldNames[i]] = i;
                            }
                        }


                    }
                }
                else
                {
                    for (int i = 0; i < this.fieldNames.Length; i++)
                    {
                        for (int j = 0; j < entity.PropertyNames.Length; j++)
                        {
                            if (this.fieldNames[i] == entity.PropertyNames[j])
                            {
                                dictNameIndex[this.fieldNames[i]] = i;
                            }
                        }
                    }
                }
                
                //没有匹配的，提前结束
                if (dictNameIndex.Count == 0)
                    yield break;

                int length = entity.PropertyValues.Length;
                foreach (object[] itemValues in this.Values)
                {
                    for (int m = 0; m < length; m++)
                    {
                        //将容器的值赋值给实体的值元素
                        string key = entity.PropertyNames[m];
                        if(dictNameIndex.ContainsKey (key))
                            entity.PropertyValues[m] = itemValues[dictNameIndex[key]];
                    }
                    yield return entity;
                    //创建一个新实例
                    entity = Activator.CreateInstance<T>();
                }
            }
            else
            {
                throw new Exception("EntityContainer 错误，执行查询没有返回任何行。");
            }
        }

        private object propertyList(string propertyName, object[] itemValues)
        {
            for (int i = 0; i < fieldNames.Length; i++)
            {
                if (string.Compare(fieldNames[i], propertyName, true) == 0)
                {
                    return itemValues[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 根据字段名，从当前行获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public T GetItemValue<T>(string fieldName)
        {
            if (this.currValue != null)
                return CommonUtil.ChangeType<T>(propertyList(fieldName, this.currValue));
            else
                return default(T);
        }

        /// <summary>
        /// 根据字段索引，从当前行获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldIndex"></param>
        /// <returns></returns>
        public T GetItemValue<T>(int fieldIndex)
        {
            if (this.currValue != null)
                return CommonUtil.ChangeType<T>(this.currValue[fieldIndex]);
            else
                return default(T);
        }

        /// <summary>
        /// 采用自定义的映射方式，将数据容器中的数据映射到指定的类中 
        /// </summary>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <param name="fun">处理数据的方法</param>
        /// <returns></returns>
        public IEnumerable<TResult> Map<TResult>(Func<TResult> fun) where TResult : class, new()
        {
            if (this.Values == null)
                this.Execute();
            if (this.Values != null && this.fieldNames != null)
            {
                foreach (object[] itemValues in this.Values)
                {
                    TResult t = new TResult();
                    this.currValue = itemValues;
                    fun(t);
                    yield return t;
                }
            }
            else
            {
                throw new Exception("EntityContainer 错误，执行查询没有返回任何行。");
            }
        }
    }
}
