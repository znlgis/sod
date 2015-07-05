/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
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
 * 版本：V4.5
 * 
 * 修改者：         时间：2012-11-16                
 * 修改说明：修复使用自定义查询（类似建立一个视图查询）的实体类的时候，在OQL中使用Where方法的一个问题。
 * Bug发现：JasonWong²º¹²
 * 
 * 修改者：         时间：2012-11-29                
 * 修改说明：修复使用自定义查询（类似建立一个视图查询）的实体类的时候，多次以不同的条件使用这个实体查询的问题。
 * Bug发现：JasonWong²º¹²
 * 
 * 修改者：         时间：2013-2-25                
 * 修改说明：处理非SQLSERVER的参数名前缀问题
 * Bug发现：大大宝
 * 
 * 修改者：         时间：2013-2-27                
 * 修改说明：配合解决WebForm参数化查询的问题
 * 
 ＊修改者：         时间：2013-9-17                
 * 修改说明：使用 sourceType.IsSubclassOf(typeof(EntityBase) 来检查是否是实体类的派生类问题，感谢网友 有事M我 发现此问题。
 * 
 * 修改者：         时间：2014-4-7                
 * 修改说明：在事务的方法中，使用DefaultNewDataBase，避免事务方法与非事务方法混用导致在多线程环境下连接无法关闭的问题。
 *           感谢网友 null 等发现此问题。
 *      
 * 修改者：         时间：2014-10-21    
 * 增加字符串长度的检查,如果值得长度大于定义的长度,抛出异常提示
 * 
 * 修改者：         时间：2015-1-6  
 * 增加判断tnf.Name!=null,这可能是因为使用了自定义查询的SqlMap的OQL,感谢网友 吉林-stdbool 发现此问题
 * 
 * 修改者：         时间：2015-3-7  
 * 修复查询中如果字段是 text,varchar(max)类型的时候参数长度设置的问题。
 * ========================================================================
*/

#define CMD_FAST //定义快速的命令对象等方案，用于解决大批量快速更新的问题
//#define CMD_NORMAR //普通模式

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

namespace PWMIS.DataMap.Entity
{
    /// <summary>
    /// 匿名实体类查询，在只知道实体类类型但没有直接的实体类实例的情况下很有用
    /// </summary>
    public class EntityQueryAnonymous
    {
        private AdoHelper _DefaultDataBase = null;
        /// <summary>
        /// 操作需要的数据库实例，如果不设定将采用默认实例
        /// </summary>
        public  AdoHelper DefaultDataBase {
            get {
                if (_DefaultDataBase == null)
                    _DefaultDataBase = MyDB.Instance ;
                return _DefaultDataBase;
            }
            set {
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

        #region 导入数据
        /// <summary>
        /// 将实体集合中的所有数据导入数据库，如果数据已经存在则修改（先删除再插入）否则直接插入。如果实体中的数据只包含部分字段的数据，请勿使用该方法。
        /// </summary>
        /// <param name="entityList">同一实体类集合</param>
        /// <param name="bulkCopyModel">是否使用批量插入的方式更新，只支持SQLSERVER。
        /// 取值含义：0，不使用批量复制，1，批量复制前删除数据库中对应的重复记录，2，不检查重复，直接批量插入
        /// </param>
        /// <returns>操作受影响的行数</returns>
        public int ImportData(List<EntityBase> entityList, int bulkCopyModel)
        {
            int count = 0;
            if (entityList == null || entityList.Count == 0)
                return 0;

            AdoHelper db = DefaultNewDataBase;
#if(CMD_FAST)

            //如果是SQLSERVER，考虑批量复制的方式
            if (bulkCopyModel>0 && db is SqlServer)
            {
                if (bulkCopyModel == 1)
                {
                    //将目标数据库中对应的数据删除
                    db.BeginTransaction();
                    try
                    {
                        count = DeleteDataInner(entityList, db);
                        db.Commit();
                    }
                    catch (Exception ex)
                    {
                        db.Rollback();
                        throw ex;
                    }
                }
                
                //执行大批量复制
                DataTable source = EntitysToDataTable<EntityBase>(entityList);
                SqlServer.BulkCopy(source, db.ConnectionString, source.TableName, 500);
                return entityList.Count;
            }
            else
            {
                db.BeginTransaction();
                try
                {
                    count = ImportDataInner(entityList, db);
                    db.Commit();
                }
                catch (Exception ex)
                {
                    db.Rollback();
                    throw ex;
                }
            }
#else
                db.BeginTransaction();
                try
                {
                    count = ImportDataInner(entityList, db);
                    db.Commit();
                }
                catch (Exception ex)
                {
                    db.Rollback();
                    throw ex;
                }

#endif
            return count;
        }

        /// <summary>
        /// 获取目标数据库表中的实际字段名称列表，目标库的字段可能跟实体类定义的字段数量不一样
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="PropertyNames"></param>
        /// <param name="DB"></param>
        /// <returns></returns>
        private List<string> GetTargetFields(string tableName, string[] PropertyNames,CommonDB DB)
        {
            //有可能目标库的字段数量跟实体类定义的不一致，需要先到目标库查询有哪些实际的字段
            DataSet dsTemp = DB.ExecuteDataSetSchema("select * from [" + tableName +"]", CommandType.Text, null);
            List<string> targetFields = new List<string>();
            if (dsTemp != null && dsTemp.Tables.Count > 0)
            {
                foreach (DataColumn col in dsTemp.Tables[0].Columns)
                {
                    foreach (string field in PropertyNames)
                    {
                        if (string.Compare(col.ColumnName, field, true) == 0)
                        {
                            targetFields.Add(field);
                            break;
                        }
                    }

                }
            }
            else
            {
                throw new Exception("EntityQuery Error:获取目标表架构失败，表名称：" + tableName);
            }
            if (targetFields.Count == 0)
                throw new Exception("EntityQuery Error:获取目标表没有和当前实体类匹配的字段，表名称：" + tableName);
            return targetFields;
        }

        /// <summary>
        /// 将实体集合中的所有数据导入数据库，如果数据已经存在则修改（先删除再插入）否则直接插入。如果实体中的数据只包含部分字段的数据，请勿使用该方法。
        /// </summary>
        /// <param name="entityList">同一实体类集合</param>
        /// <param name="DB">数据访问对象实例</param>
        /// <returns>操作受影响的行数</returns>
        private  int ImportDataInner(List<EntityBase> entityList, CommonDB DB)
        {
            //必须保证集合中的元素都是同一个类型
            if (entityList == null || entityList.Count == 0)
                return 0;

            EntityBase entity = entityList[0];
            if (entity.PrimaryKeys.Count == 0)
                throw new Exception("EntityQuery Error:当前实体类未指定主键字段");
            int fieldCount = entity.PropertyNames.Length;
            if (fieldCount == 0)
                throw new Exception("EntityQuery Error:实体类属性字段数量为0");

            string tableName = entity.TableName;
            for (int i =1; i < entityList.Count; i++)
            {
                if (entityList[i].TableName != tableName)
                    throw new Exception("当前实体类集合的元素类型不一致，对应的表是：" + (entityList[i].TableName));
            }
            //先将主键对应的记录删除，再插入
            #region 构造查询语句
            //构造Delete 语句：
            //IDataParameter[] paras_delete = new IDataParameter[entity.PrimaryKeys.Count ];
            //string sql_delte = "DELETE FROM " + entity.TableName + " WHERE ";
            //string values = "";
            //string condition = "";
            //int index = 0;

            //foreach (string key in entity.PrimaryKeys)
            //{
            //    string paraName = "@P" + index.ToString();
            //    condition += " AND " + key + "=" + paraName;
            //    paras_delete[index] = DB.GetParameter();
            //    paras_delete[index].ParameterName = paraName;
            //    index++;
            //}
            //sql_delte = sql_delte + values.TrimStart(',') + " " + condition.Substring(" AND ".Length);

            ////构造Insert语句
            //string sql_insert = "INSERT INTO " + entity.TableName;
            //string fields = "";
          
            //IDataParameter[] paras_insert = new IDataParameter[fieldCount];
            //index = 0;

            //List<string> targetFields = GetTargetFields(tableName, entity.PropertyNames, DB);

            //foreach (string field in targetFields)
            //{
            //    //if (entity.IdentityName != field)//由于是导入数据，不必理会自增列
            //    //{
            //        fields += "," + field;
            //        string paraName = "@P" + index.ToString();
            //        values += "," + paraName;
            //        paras_insert[index] = DB.GetParameter();
            //        paras_insert[index].ParameterName = paraName;
            //        index++;
            //    //}
            //}
            //sql_insert = sql_insert + "(" + fields.TrimStart(',') + ") VALUES (" + values.TrimStart(',') + ")";

            EntityCommand ec = new EntityCommand(entity, DB);
            ec.IdentityEnable = true;//导入数据，不考虑自增列问题
            ec.TargetFields = GetTargetFields(tableName, entity.PropertyNames, DB).ToArray ();

            string sql_delte = ec.DeleteCommand;
            IDataParameter[] paras_delete = ec.DeleteParameters;

            string sql_insert = ec.InsertCommand;
            IDataParameter[] paras_insert = ec.InsertParameters;

            #endregion

            int count = 0;

            foreach (EntityBase item in entityList)
            { 
                //执行删除
                foreach (IDataParameter para in paras_delete)
                {
                    para.Value = item.PropertyList(para.SourceColumn );
                }
                count += DB.ExecuteNonQuery(sql_delte, CommandType.Text, paras_delete);
                //执行插入
                foreach (IDataParameter para in paras_insert)
                {
                    //if (entity.IdentityName != field)//由于是导入数据，不必理会自增列
                    //{
                    para.Value = item.PropertyList(para.SourceColumn);
                    //}
                }
                count += DB.ExecuteNonQuery(sql_insert, CommandType.Text, paras_insert);
            }
            
            return count ;
        }

        private int DeleteDataInner(List<EntityBase> entityList, CommonDB DB)
        {
            //必须保证集合中的元素都是同一个类型
            if (entityList == null || entityList.Count == 0)
                return 0;

            EntityBase entity = entityList[0];
            if (entity.PrimaryKeys.Count == 0)
                throw new Exception("EntityQuery Error:当前实体类未指定主键字段");
            int fieldCount = entity.PropertyNames.Length;
            if (fieldCount == 0)
                throw new Exception("EntityQuery Error:实体类属性字段数量为0");

             string tableName = entity.TableName;
            for (int i =1; i < entityList.Count; i++)
            {
                if (entityList[i].TableName != tableName)
                    throw new Exception("当前实体类集合的元素类型不一致，对应的表是：" + (entityList[i].TableName));
            }
            //先将主键对应的记录删除，再插入
            #region 构造查询语句
            

            EntityCommand ec = new EntityCommand(entity, DB);
            ec.IdentityEnable = true;//导入数据，不考虑自增列问题
            ec.TargetFields = GetTargetFields(tableName, entity.PropertyNames, DB).ToArray();

            string sql_delte = ec.DeleteCommand;
            IDataParameter[] paras_delete = ec.DeleteParameters;

            #endregion

            int count = 0;

            foreach (EntityBase item in entityList)
            {
                //执行删除
                foreach (IDataParameter para in paras_delete)
                {
                    para.Value = item.PropertyList(para.SourceColumn);
                }
                count += DB.ExecuteNonQuery(sql_delte, CommandType.Text, paras_delete);
                
            }

            return count;
        }

        #endregion

        private int InsertOrUpdateInner(List<EntityBase> entityList, CommonDB DB)
        {
            //必须保证集合中的元素都是同一个类型
            if (entityList == null || entityList.Count == 0)
                return 0;

            EntityBase entity = entityList[0];
            if (entity.PrimaryKeys.Count == 0)
                throw new Exception("EntityQuery Error:当前实体类未指定主键字段");
            int fieldCount = entity.PropertyNames.Length;
            if (fieldCount == 0)
                throw new Exception("EntityQuery Error:实体类属性字段数量为0");

            //CommonDB DB = MyDB.GetDBHelper();
            //IDataParameter[] paras = new IDataParameter[fieldCount];
            //string sql = "UPDATE " + entity.TableName + " SET ";
            //string values = "";
            //string condition = "";
            //int index = 0;

            //List<string> targetFields = GetTargetFields(entity.TableName, entity.PropertyNames, DB);
            ////构造查询语句
            //foreach (string field in targetFields)
            //{
            //    string paraName = "@P" + index.ToString();
            //    if (entity.PrimaryKeys.Contains(field))
            //    {
            //        //当前字段为主键，不能被更新
            //        condition += " AND " + field + "=" + paraName;
            //        //paras[index] = DB.GetParameter(paraName, entity.PropertyList(field));
            //    }
            //    else
            //    {
            //        values += "," + field + "=" + paraName;
            //        //paras[index] = DB.GetParameter(paraName, entity.PropertyList(field));

            //    }
            //    paras[index] = DB.GetParameter();
            //    paras[index].ParameterName = paraName;
            //    index++;
            //}

           
            //sql = sql + values.TrimStart(',') + " WHERE " + condition.Substring(" AND ".Length);

            EntityCommand ec = new EntityCommand(entity, DB);
            ec.TargetFields = GetTargetFields(entity.TableName , entity.PropertyNames, DB).ToArray();
            
            int all_count = 0;
            int updateCount = 0;
            int insertCount = 0;
            List<IDataParameter> paraList = new List<IDataParameter>();

#if ( !CMD_FAST)
            foreach (EntityBase item in entityList)
            {
                paraList.Clear();
                foreach (string field in ec.TargetFields)
                {
                    string paraName = "@" + field.Replace(" ", "");
                    IDataParameter para = DB.GetParameter(paraName, item.PropertyList(field));
                    paraList.Add(para);
                }
                

            //先做一部分修改，如果不成功就插入
                int count = DB.ExecuteNonQuery(ec.UpdateCommand, CommandType.Text, paraList.ToArray ());
                if (count <= 0)
                    insertCount += DB.ExecuteNonQuery(ec.InsertCommand, CommandType.Text, paraList.ToArray ());
                else
                    updateCount += count;
            }

#else
            IDbConnection conn = DB.GetDbConnection();

            IDbCommand insertCmd = conn.CreateCommand();
            insertCmd.CommandText = ec.InsertCommand;
            insertCmd.CommandType = CommandType.Text;
            if (ec.InsertParameters != null)
            {
                foreach (IDataParameter para in ec.InsertParameters)
                    insertCmd.Parameters.Add(para);
            }

            IDbCommand updateCmd = conn.CreateCommand();
            updateCmd.CommandText = ec.UpdateCommand;
            updateCmd.CommandType = CommandType.Text;
            if (ec.UpdateParameters != null)
            {
                foreach (IDataParameter para in ec.UpdateParameters)
                    updateCmd.Parameters.Add(para);
            }

            foreach (EntityBase item in entityList)
            {
                foreach (string field in ec.TargetFields)
                {
                    string paraName =DB.GetParameterChar + field.Replace(" ", "");
                    ((IDataParameter)insertCmd.Parameters[paraName]).Value = item.PropertyList(field);
                    ((IDataParameter)updateCmd.Parameters[paraName]).Value = item.PropertyList(field);
                }
                //先做一部分修改，如果不成功就插入
                //直接使用Command对象的 ExecuteNonQuery ，加快处理速度
                int count = updateCmd.ExecuteNonQuery ();
                if (count <= 0)
                    insertCount += insertCmd.ExecuteNonQuery ();
                else
                    updateCount += count;
            }
           
           

#endif
            all_count = insertCount + updateCount * (entityList.Count +1);
            /* 更新或者修改计算方式
             * x + y * (C+1)=Z;{c=List Count;}
             * x + y = C;
             *   => y-y * (C+1)=C-Z => y(1-C-1)=C-Z => y * -C =C-Z => y= (C-Z) / -C 
             * 在本例中，y=update,x=insert
             */ 
            return all_count;
        }

        /// <summary>
        /// 解析更新或者修改的条数
        /// </summary>
        /// <param name="allCount">InsertOrUpdate 方法取得的总条数</param>
        /// <param name="listCount">记录的总条数</param>
        /// <param name="insertCount">插入的条数</param>
        /// <param name="updateCount">修改的条数</param>
        /// <returns></returns>
        public bool ParseInsertOrUpdateCount(int allCount,int listCount,out int insertCount,out int updateCount)
        {
            insertCount = 0;
            updateCount = 0;
            if (allCount < listCount || listCount<=0)
                return false;

            updateCount = (listCount - allCount) /  - listCount;
            insertCount = listCount - updateCount;
            return true;
        }
        /// <summary>
        /// 将实体类集合中实体类的数据插入或者修改到数据库中，适用于更新数据，如果需要大批量导入数据，请考虑使用 ImportData 方法。
        /// </summary>
        /// <param name="entityList">实体类集合</param>
        /// <returns>操作受影响的行数</returns>
        public int InsertOrUpdate(List<EntityBase> entityList)
        {
            int count = 0;
            AdoHelper db = DefaultNewDataBase;
            db.BeginTransaction();
            try
            {
                count = InsertOrUpdateInner(entityList, db);
                db.Commit();
            }
            catch (Exception ex)
            {
                db.Rollback();
                throw ex;
            }
            return count;
        }

        #region 查询数据

        /// <summary>
        /// 根据实体查询表达式对象，查询实体对象集合
        /// </summary>
        /// <param name="oql">实体对象查询表达式</param>
        /// <param name="factEntityType">具体实体类的类型</param>
        /// <returns></returns>
        public static IList QueryList(OQL oql, Type factEntityType)
        {
            return QueryList(oql, MyDB.Instance, factEntityType);
        }

        /// <summary>
        /// 根据数据阅读器对象，查询实体对象集合(注意查询完毕将自动释放该阅读器对象)
        /// </summary>
        /// <param name="reader">数据阅读器对象</param>
        /// <param name="factEntityType">具体实体类的类型</param>
        /// <returns>实体类集合</returns>
        public static IList QueryList(System.Data.IDataReader reader, Type factEntityType)
        {
            //下面一行代码被注释，以便于实体类多级派生，感谢网友　有事M我 发现此问题
            //if (factEntityType.BaseType != typeof(EntityBase))
            if (!factEntityType.IsSubclassOf(typeof(EntityBase)))
                throw new Exception("当前类型不是 EntityBase 的派生类型：" + factEntityType.FullName);

            IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(factEntityType));

            using (reader)
            {

                if (reader.Read())
                {
                    int fcount = reader.FieldCount;
                    string[] names = new string[fcount];
                    object[] values = null;

                    for (int i = 0; i < fcount; i++)
                        names[i] = reader.GetName(i);

                    do
                    {
                        values = new object[fcount];
                        reader.GetValues(values);

                        EntityBase t = (EntityBase)Activator.CreateInstance(factEntityType);
                        t.PropertyNames = names;
                        t.PropertyValues = values;

                        list.Add(t);
                    } while (reader.Read());

                }
            }
            return list;
        }

        /// <summary>
        /// 根据实体查询表达式对象，和当前数据库操作对象，查询实体对象集合
        /// </summary>
        /// <param name="oql">实体查询表达式对象</param>
        /// <param name="db">数据库操作对象</param>
        /// <param name="factEntityType">实体类的实际类型</param>
        /// <returns>实体对象集合</returns>
        public static IList QueryList(OQL oql, AdoHelper db, Type factEntityType)
        {
            IDataReader reader = ExecuteDataReader(oql, db, factEntityType);
            return QueryList(reader, factEntityType);
        }

        /// <summary>
        /// 根据OQL查询数据获得DataReader
        /// </summary>
        /// <param name="oql">OQL表达式</param>
        /// <param name="db">当前数据库访问对象</param>
        /// <param name="factEntityType">实体类类型</param>
        /// <returns>DataReader</returns>
        public static IDataReader ExecuteDataReader(OQL oql, AdoHelper db, Type factEntityType)
        {
            return ExecuteDataReader(oql, db, factEntityType, false);
        }
        /// <summary>
        /// 寻找SQL语句中参数名对应的字段名称
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paraName"></param>
        /// <param name="paraChar"></param>
        /// <returns></returns>
        private static string FindFieldNameInSql(string sql, string paraName, string paraChar)
        {
            if (!paraName.StartsWith(paraChar))
                paraName = paraChar + paraName;
            string whereTempStr = sql.Substring(sql.IndexOf("Where",StringComparison.OrdinalIgnoreCase) + 5).Trim();
            string fildTempStr = whereTempStr.Substring(0, whereTempStr.IndexOf(paraName));
            int a = fildTempStr.LastIndexOf('[');
            int b = fildTempStr.LastIndexOf(']');
            string fieldStr = fildTempStr.Substring(a + 1, b - a - 1);
            return fieldStr;
        }

        internal static SqlInfo GetSqlInfoFromOQL(OQL oql, AdoHelper db, Type factEntityType,bool single)
        {
            string sql = "";
            Dictionary<string, TableNameField> Parameters = null;
            //处理用户查询映射的实体类
            if (oql.EntityMap == PWMIS.Common.EntityMapType.SqlMap)
            {
                if (CommonUtil.CacheEntityMapSql == null)
                    CommonUtil.CacheEntityMapSql = new Dictionary<string, string>();
                if (!CommonUtil.CacheEntityMapSql.ContainsKey(oql.sql_table))
                {
                    string tempView = GetMapSql(factEntityType);
                    CommonUtil.CacheEntityMapSql.Add(oql.sql_table, tempView);
                }
                sql = oql.GetMapSQL(CommonUtil.CacheEntityMapSql[oql.sql_table]);


                //如果用户本身没有初始化参数对象，则这里声明一个 edit at 2012.11.16
                Parameters = new Dictionary<string, TableNameField>();
                if (oql.InitParameters != null)
                {
                    foreach (string name in oql.InitParameters.Keys)
                    {
                        TableNameField tnf = new TableNameField();
                        tnf.FieldValue=oql.InitParameters[name];
                        Parameters.Add(name, tnf);
                    }
                }
                
                if (oql.Parameters != null && oql.Parameters.Count > 0)
                {
                    foreach (string name in oql.Parameters.Keys)
                    {
                        Parameters.Add(name, oql.Parameters[name]);
                    }
                }

            }
            else if (oql.EntityMap == PWMIS.Common.EntityMapType.StoredProcedure)
            {
                string script = "";
                if (CommonUtil.CacheEntityMapSql == null)
                    CommonUtil.CacheEntityMapSql = new Dictionary<string, string>();
                //获取SQL-MAP脚本
                if (CommonUtil.CacheEntityMapSql.ContainsKey(oql.sql_table))
                    script = CommonUtil.CacheEntityMapSql[oql.sql_table];
                else
                {
                    script = GetMapSql(factEntityType);
                    CommonUtil.CacheEntityMapSql.Add(oql.sql_table, script);
                }
                //对SQL-MAP格式的参数进行解析
                SqlMap.SqlMapper mapper = new PWMIS.DataMap.SqlMap.SqlMapper();
                mapper.DataBase = db;
                //解析存储过程名称
                sql = mapper.FindWords(mapper.GetScriptInfo(script), 0, 255); //由于是存储过程，需要特殊处理，调用 FindWords方法
                //解析参数
                IDataParameter[] paras = mapper.GetParameters(script);
                if (oql.InitParameters != null && oql.InitParameters.Count > 0)
                {
                    Parameters = new Dictionary<string, TableNameField>();
                    try
                    {
                        foreach (IDataParameter para in paras)
                        {
                            string key = para.ParameterName.TrimStart(db.GetParameterChar.ToCharArray());
                            para.Value = oql.InitParameters[key];
                            Parameters.Add(key, new TableNameField() {  FieldValue=para});
                        }
                    } 
                    catch (KeyNotFoundException exKey)
                    {
                        throw new KeyNotFoundException("'存储过程实体类'的初始化参数中没有找到指定的参数名，请检查参数定义和设置。", exKey);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
                else
                {
                    if (paras.Length > 0)
                        throw new Exception("当前'存储过程实体类'需要提供初始化参数，请设置OQL对象的InitParameters属性");
                }

                SqlInfo si = new SqlInfo(sql, Parameters);
                si.CommandType = CommandType.StoredProcedure;
                //return db.ExecuteDataReader(sql, CommandType.StoredProcedure, paras);
                return si;
            }
            else
            {
                sql = oql.ToString();
                Parameters = oql.Parameters;
            }


           if (oql.PageEnable && (!single || oql.PageWithAllRecordCount<=0))
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
                            //如果是字符串类型的主键，下面的分页可能不准确
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
            SqlInfo result = new SqlInfo(sql, Parameters);
            result.CommandType = CommandType.Text;
            result.TableName = oql.sql_table;
            return result;
        }

        /// <summary>
        ///  根据OQL查询数据获得DataReader。如果指定single=真，将执行优化的查询以获取单条记录
        /// </summary>
        /// <param name="oql">OQL表达式</param>
        /// <param name="db">当前数据库访问对象</param>
        /// <param name="factEntityType">实体类类型</param>
        /// <param name="single">是否只查询一条记录</param>
        /// <returns>DataReader</returns>
        public static IDataReader ExecuteDataReader(OQL oql, AdoHelper db, Type factEntityType,bool single)
        {
            SqlInfo si = GetSqlInfoFromOQL(oql, db, factEntityType, single);
            oql.Dispose();
            return ExecuteDataReader(si, db, single);
        }

        //public static void CheckStringPara(string SQL,string tableName, IDataParameter[] paras,string parameterChar)
        //{
        //    for (int index = 0; index < paras.Length; index++)
        //    {
        //        //为字符串类型的参数指定长度 edit at 2012.4.23
        //        if (paras[index].Value != null && paras[index].Value.GetType() == typeof(string))
        //        {
        //            string field = FindFieldNameInSql(SQL, paras[index].ParameterName, parameterChar);
        //            ((IDbDataParameter)paras[index]).Size = EntityBase.GetStringFieldSize(tableName, field);
        //        }
        //    }
        //}

        /// <summary>
        /// 使用SQL查询命令信息 执行数据阅读器查询。注意，查询单行数据不支持存储过程。
        /// </summary>
        /// <param name="si">SQL查询命令信息</param>
        /// <param name="db">数据访问对象</param>
        /// <param name="single">是否仅查询单行数据</param>
        /// <returns>数据阅读器</returns>
        public static IDataReader ExecuteDataReader(SqlInfo si, AdoHelper db,  bool single)
        {
            IDataReader reader = null;
            if (si.Parameters != null && si.Parameters.Count > 0)
            {
                IDataParameter[] paras = GetParameters(si.Parameters,db);

                if (single)
                    reader = db.ExecuteDataReaderWithSingleRow(si.SQL, paras);
                else
                    reader = db.ExecuteDataReader(si.SQL, si.CommandType, paras);
            }
            else
            {
                if (single)
                    reader = db.ExecuteDataReaderWithSingleRow(si.SQL);
                else
                    reader = db.ExecuteDataReader(si.SQL, si.CommandType,null);
            }
            return reader;
        }

        /// <summary>
        /// 根据EntityMapSql的全名称 "名称空间名字.SQL名字" 获取映射的SQL语句
        /// </summary>
        /// <param name="entityType">根据当前实体类所在程序集，获取其中的嵌入式EntityMapSql 文件</param>
        /// <returns>映射的SQL语句</returns>
        public static string GetMapSql(Type entityType)
        {
            string typeFullName = entityType.FullName;
            string[] arrTemp = new string[2];
           
            int at = typeFullName.LastIndexOf('.');
            if (at > 0)
            {
                arrTemp[0] = typeFullName.Substring(0, at);
                arrTemp[1] = typeFullName.Substring(at + 1);
            }
            else
                throw new Exception("EntityMapSql 实体类要求具有名称空间！");
            //string[] arrTemp =entityType.FullName.Split('.');
            //if (arrTemp.Length != 2)
            //    throw new Exception("EntityMapSql的全名称格式错误，正确的格式应该： 名称空间名字.SQL名字");


            string resourceName = "EntitySqlMap.config";
            string xmlConfig = null;
            //如果存在配置文件，则从文件读取
            if (System.IO.File.Exists(resourceName))
                xmlConfig = System.IO.File.ReadAllText(resourceName);
            else
                xmlConfig = CommonUtil.GetAssemblyResource(entityType, resourceName);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlConfig);
            XmlNode SqlText = default(XmlNode);
            XmlElement root = doc.DocumentElement;
            string objPath = "/configuration/Namespace[@name='" + arrTemp[0] + "']/Map[@name='" + arrTemp[1] + "']/Sql";
            SqlText = root.SelectSingleNode(objPath);
            if ((SqlText != null) && SqlText.HasChildNodes)
            {
                return SqlText.InnerText;
            }
            return "";
            
        }

        ///// <summary>
        /////  根据EntityMapSql的全名称 "名称空间名字.SQL名字" 获取映射的SQL语句
        ///// </summary>
        ///// <param name="fullName">EntityMapSql的全名称，格式： "名称空间名字.SQL名字"</param>
        ///// <returns>映射的SQL语句</returns>
        //public static string GetMapSql(string fullName)
        //{
            
        //}

        /// <summary>
        /// 执行返回单值的查询，通常用于OQL的Count,Max等查询
        /// </summary>
        /// <param name="oql">查询表达式</param>
        /// <param name="db">数据访问对象</param>
        /// <returns>单值</returns>
        public static object ExecuteScalar(OQL oql, AdoHelper db)
        {
            if (oql.Parameters != null && oql.Parameters.Count > 0)
            {
                IDataParameter[] paras = GetParameters(oql.Parameters,db);
                return db.ExecuteScalar(oql.ToString(), CommandType.Text, paras);
            }
            else
            {
                return db.ExecuteScalar(oql.ToString());
            }
           
        }

        /// <summary>
        /// 执行OQL查询，统计该查询对应的记录数量（一般用于分页前使用，某些方法会自动调用该方法，请注意方法说明）
        /// </summary>
        /// <param name="oql"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static object ExecuteOQLCount(OQL oql, AdoHelper db)
        {
            string sql = PWMIS.Common.SQLPage.MakeSQLStringByPage(db.CurrentDBMSType, oql.ToString(), "", oql.PageSize, oql.PageNumber, 0);
            if (oql.Parameters != null && oql.Parameters.Count > 0)
            {
                IDataParameter[] paras = GetParameters(oql.Parameters, db);
                return db.ExecuteScalar(sql, CommandType.Text, paras);
            }
            else
            {
                return db.ExecuteScalar(sql);
            }
        }

        /// <summary>
        /// 执行返回列表数据的查询
        /// </summary>
        /// <typeparam name="T">实体类型，支持普通的POCO实体类，仅需实现IReadData 接口</typeparam>
        /// <param name="reader">数据阅读器</param>
        /// <returns>实体列表</returns>
        public static List<T> ExecuteDataList<T>(IDataReader reader) where T : IReadData, new()
        {
            List<T> list = new List<T>();
            if (reader.Read())
            {
                int fcount = reader.FieldCount;
                string[] names = new string[fcount];

                for (int i = 0; i < fcount; i++)
                    names[i] = reader.GetName(i);

                do
                {
                    T t = new T();
                    ((IReadData)t).ReadData(reader, fcount, names);
                    list.Add(t);
                } while (reader.Read());

            }
            return list;
        }
        #endregion

        /// <summary>
        /// 将实体类转换成数据表
        /// </summary>
        /// <typeparam name="Entity">实体类类型</typeparam>
        /// <param name="entitys">实际的实体类</param>
        /// <returns>数据表</returns>
        public static DataTable EntitysToDataTable<Entity>(List<Entity> entitys) where Entity : EntityBase
        {
            if (entitys == null)
                throw new ArgumentException("参数错误，不能为空!");
            if (entitys.Count == 0)
                return null;

            //Entity e = entitys.Count > 0 ? entitys[0] : new Entity();
            Entity entity = entitys[0];
            string tableName = entity.TableName == null ? "Table1" : entity.TableName;
            DataTable dt = new DataTable(tableName);
            foreach (string str in entity.PropertyNames)
            {
                DataColumn col = new DataColumn(str);
                object V = entity.PropertyList(str);
                col.DataType = V == null || V == DBNull.Value ? typeof(string) : V.GetType();
                dt.Columns.Add(col);
            }

            foreach (Entity item in entitys)
            {
                dt.Rows.Add(item.PropertyValues);
            }
            return dt;
        }

        /// <summary>
        /// 将字典形式的参数数据，转换成数据库格式的参数素组
        /// </summary>
        /// <param name="dictPara"></param>
        /// <returns></returns>
        protected internal static IDataParameter[] GetParameters(Dictionary<string, TableNameField> dictPara,AdoHelper db)
        {
            if (dictPara == null)
                return null;
            IDataParameter[] paras = new IDataParameter[dictPara.Count];
            int index = 0;

            foreach (string key in dictPara.Keys)
            {
                object Value = dictPara[key];
                if (Value is IDataParameter)
                {
                    paras[index] = (IDataParameter)Value;
                }
                else
                {
                    string paraName = key.StartsWith("@")?key.Substring(1):key;
                    //参数名无需加上　ParameterChar
                    //if (!key.StartsWith(db.GetParameterChar))
                    //    paraName = db.GetParameterChar + key.Substring(1);

                    var tnf = dictPara[key];
                    paras[index] = db.GetParameter(paraName, tnf.FieldValue);
                    //为字符串类型的参数指定长度 edit at 2012.4.23
                    //增加判断tnf.Name!=null,这可能是因为使用了自定义查询的SqlMap的OQL,感谢网友 吉林-stdbool 发现此问题
                    if (tnf.Name!=null && paras[index].Value != null && paras[index].Value.GetType() == typeof(string))
                    {
                        //增加字符串长度的检查,如果值得长度大于定义的长度,抛出异常提示 2014.10.21
                        int size = tnf.Entity.GetStringFieldSize(tnf.Field);
                        if (size != -1) //如果字段不是text等类型
                        {
                            int length = paras[index].Value.ToString().Length;
                            if (length > size)
                                throw new NotSupportedException("当前实体类映射的字段" + paraName + " 长度没有定义或者长度小于了当前实际值的长度："
                                    + length + "，请在实体类定义里面使用 setProperty 的重载方法指定合适的字段长度。");
                            ((IDbDataParameter)paras[index]).Size = size;
                        }
                    }
                }


                index++;
            }
            return paras;
        }

    }
}
