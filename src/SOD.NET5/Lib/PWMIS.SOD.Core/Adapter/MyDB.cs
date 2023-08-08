/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap
 * ========================================================================
 * MyDB.cs
 * 该类的作用：提供便利的方式访问数据库实例和操作数据集
 *
 * 作者：邓太华     时间：2008-10-12
 * 版本：V3.0
 *
 * 修改者：         时间：2011.11.16
 * 修改说明：增加自动保存数据集的功能
 * ========================================================================
 */

using System;
using System.Configuration;
using System.Data;
using PWMIS.Common;
using PWMIS.DataProvider.Data;

namespace PWMIS.DataProvider.Adapter
{
    /// <summary>
    ///     应用程序数据访问实例，提供单例模式和工厂模式创建实例对象，根据应用程序配置文件自动创建特定的数据访问对象。
    ///     2008.5.23 增加动态数据集更新功能,7.24增加线程安全的静态实例。
    ///     2009.4.1  增加SQLite 数据库支持。
    ///     2010.1.6  增加 connectionStrings 配置支持
    /// </summary>
    public class MyDB
    {
        private static AdoHelper _instance;
        private static readonly object lockObj = new();

        #region 获取静态实例

        /// <summary>
        ///     数据访问静态实例对象，已经禁止当前对象执行事务。如果有事务并且有可能存在并发访问，请创建该AdoHelper类的动态实例对象。
        /// </summary>
        public static AdoHelper Instance
        {
            get
            {
                if (_instance == null)
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = GetDBHelper();
                            _instance.AllowTransaction = false;
                        }
                    }

                return _instance;
            }
        }

        #endregion

        public DBMSType DBMSType
        {
            get => throw new NotImplementedException();
            set { }
        }

        public SQLPage SQLPage
        {
            get => throw new NotImplementedException();
            set { }
        }

        #region 获取动态实例对象

        /// <summary>
        ///     通过配置文件获得数据访问对象实例，注意当前版本不再支持AppSettings配置的EngineType内容，默认取connectionStrings 配置节的最后一个 providerName 配置的连接对象
        /// </summary>
        /// <returns>数据访问对象实例</returns>
        public static AdoHelper GetDBHelper()
        {
            AdoHelper helper = null;
            //从 connectionStrings 读取
            if (ConfigurationManager.ConnectionStrings.Count == 0)
                throw new Exception("未在应用程序配置文件的 connectionStrings 配置节配置连接信息");

            helper = GetDBHelperByConnectionSetting(
                ConfigurationManager.ConnectionStrings[ConfigurationManager.ConnectionStrings.Count - 1]);
            return helper;
        }

        /// <summary>
        ///     从 connectionStrings 配置节获取指定 数据连接名称的数据访问对象实例
        /// </summary>
        /// <param name="name">数据连接名称</param>
        /// <returns></returns>
        public static AdoHelper GetDBHelperByConnectionName(string name)
        {
            var connSetting = ConfigurationManager.ConnectionStrings[name];
            if (connSetting == null)
                throw new Exception("未在应用程序配置文件 connectionStrings 配置节找到指定的 连接名称：" + name);

            return GetDBHelperByConnectionSetting(connSetting);
        }

        /// <summary>
        ///     获取数据库连接配置中指定连接名的设置信息元组
        /// </summary>
        /// <param name="connectionName"></param>
        /// <returns>返回元组信息：提供程序名,连接字符串</returns>
        public static Tuple<string, string> GetDBConnectionSettingInfo(string connectionName)
        {
            var connSetting = ConfigurationManager.ConnectionStrings[connectionName];
            if (connSetting == null)
                throw new Exception("未在应用程序配置文件 connectionStrings 配置节找到指定的 连接名称：" + connectionName);
            return new Tuple<string, string>(connSetting.ProviderName, connSetting.ConnectionString);
        }

        private static AdoHelper GetDBHelperByConnectionSetting(ConnectionStringSettings connSetting)
        {
            return GetDBHelperByProviderString(connSetting.ProviderName, connSetting.ConnectionString);
        }

        /// <summary>
        ///     根据提供程序名称字符串和连接字符串，创建提供程序实例
        /// </summary>
        /// <param name="providerName">供程序名称字符串，格式为：提供程序全名称,程序集名称</param>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public static AdoHelper GetDBHelperByProviderString(string providerName, string connectionString)
        {
            var providerInfo = providerName.Split(',');
            var helperAssembly = "";
            string helperType;

            if (providerInfo.Length == 1)
            {
                switch (providerName)
                {
                    case "SqlServer":
                    case "":
                        AdoHelper helper = new SqlServer();
                        helper.ConnectionString = connectionString;
                        return helper;
                    case "Access":
                    case "OleDb":
                        helperAssembly = "PWMIS.SOD.Core.AccessExtensions";
                        break;
                    case "Oracle":
                    case "Odbc":
                    case "SQLite":
                        helperAssembly = "PWMIS.SOD.Core.Extensions";
                        break;
                }

                if (helperAssembly == "")
                    throw new Exception(
                        "数据访问提供程序名错误。如果是简便名称（没有逗号分隔），必须是[SqlServer/Access/Oledb/Oracle/Odbc/SQLite]之一，且大小写一致。 ");

                helperType = "PWMIS.DataProvider.Data." + providerName;
            }
            else
            {
                helperAssembly = providerInfo[1].Trim();
                helperType = providerInfo[0].Trim();
            }

            return GetDBHelper(helperAssembly, helperType, connectionString);
        }


        /// <summary>
        ///     根据数据库管理系统枚举类型和连接字符串创建一个新的数据访问对象实例
        /// </summary>
        /// <param name="DbmsType">数据库类型媒介，有ACCESS/MYSQL/ORACLE/SQLSERVER/SYSBASE/UNKNOWN </param>
        /// <param name="ConnectionString">连接字符串</param>
        /// <returns>数据访问对象</returns>
        public static AdoHelper GetDBHelper(DBMSType DbmsType, string connectionString)
        {
            var EngineType = "";
            switch (DbmsType)
            {
                case DBMSType.Access:
                    EngineType = "OleDb";
                    break;
                case DBMSType.MySql:
                    EngineType = "Odbc";
                    break;
                case DBMSType.Oracle:
                    EngineType = "Oracle";
                    break;
                case DBMSType.SqlServer:
                    EngineType = "SqlServer";
                    break;
                case DBMSType.SqlServerCe:
                    EngineType = "SqlServerCe";
                    break;
                case DBMSType.Sysbase:
                    EngineType = "OleDb";
                    break;
                case DBMSType.SQLite:
                    EngineType = "SQLite";
                    break;
                case DBMSType.UNKNOWN:
                    EngineType = "Odbc";
                    break;
            }

            var helper = GetDBHelperByProviderString(EngineType, connectionString);
            return helper;
        }

        /// <summary>
        ///     根据程序集名称和数据访问对象类型创建一个新的数据访问对象实例。
        /// </summary>
        /// <param name="HelperAssembly">程序集名称</param>
        /// <param name="HelperType">数据访问对象类型</param>
        /// <param name="ConnectionString">连接字符串</param>
        /// <returns>数据访问对象</returns>
        public static AdoHelper GetDBHelper(string HelperAssembly, string HelperType, string ConnectionString)
        {
            AdoHelper helper = null; // CommonDB.CreateInstance(HelperAssembly, HelperType);
            if (HelperType != null && HelperType.ToLower() == "sqlserver")
                helper = new SqlServer();
            else
                helper = CommonDB.CreateInstance(HelperAssembly, HelperType);
            helper.ConnectionString = ConnectionString;
            return helper;
        }

        #endregion

        #region 公共静态方法

        /// <summary>
        ///     更新数据集(采用参数形式)，数据表如果指定了主键那么执行更新操作，否则执行插入操作。
        /// </summary>
        /// <param name="ds">数据集</param>
        /// <returns>查询结果受影响的行数</returns>
        public static int UpdateDataSet(DataSet ds)
        {
            var count = 0;
            foreach (DataTable dt in ds.Tables)
                if (dt.PrimaryKey.Length > 0)
                    count += UpdateDataTable(dt, GetSqlUpdate(dt));
                else
                    count += UpdateDataTable(dt, GetSqlInsert(dt)); // end if
            //end for
            return count;
        } //end function

        /// <summary>
        ///     自动将数据集中的数据更新或者插入到数据库
        ///     <remarks>更新时间：2011.11.16</remarks>
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="DB"></param>
        /// <returns></returns>
        public static int SaveDataSet(DataSet ds, CommonDB DB)
        {
            var count = 0;
            foreach (DataTable dt in ds.Tables)
            {
                var insertSql = GetSqlInsert(dt);
                var updateSql = GetSqlUpdate(dt);
                count += SaveDataTable(dt, insertSql, updateSql, DB);
            } //end for

            return count;
        }

        /// <summary>
        ///     根据数据集中在指定的表中，根据表中的指定列的值在数据源中删除数据
        /// </summary>
        /// <param name="ds">数据集</param>
        /// <param name="tableName">表名称</param>
        /// <param name="columnName">列名</param>
        /// <returns>查询所影响的行数</returns>
        public static int DeleteDataSet(DataSet ds, string tableName, string columnName)
        {
            var dt = ds.Tables[tableName];

            CommonDB DB = GetDBHelper();
            var ParaChar = GetDBParaChar(DB);
            var count = 0;

            var sqlDelete = "DELETE FROM " + tableName + " WHERE " + columnName + "=" + ParaChar + columnName;

            DB.BeginTransaction();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    IDataParameter[] paras = { DB.GetParameter(ParaChar + columnName, dr[columnName]) };
                    count += DB.ExecuteNonQuery(sqlDelete, CommandType.Text, paras);
                    if (DB.ErrorMessage != "")
                        throw new Exception(DB.ErrorMessage);
                    if (count >= dt.Rows.Count) break;
                }

                DB.Commit();
            }
            catch (Exception ex)
            {
                DB.Rollback();
                throw ex;
            }

            return count;
        }

        #endregion

        #region 公共动态实例方法

        /// <summary>
        ///     获取当前操作信息
        /// </summary>
        public string Message { get; private set; } = string.Empty;

        /// <summary>
        ///     更新数据集，带数据访问对象
        /// </summary>
        /// <param name="ds">数据集</param>
        /// <param name="DB">数据访问对象</param>
        /// <returns></returns>
        public int UpdateDataSet(DataSet ds, CommonDB DB)
        {
            var count = 0;
            foreach (DataTable dt in ds.Tables)
                if (dt.PrimaryKey.Length > 0)
                {
                    count += UpdateDataTable(dt, GetSqlUpdate(dt), DB);
                    Message = "已经更新记录" + count + "条";
                }
                else
                {
                    count += UpdateDataTable(dt, GetSqlInsert(dt), DB);
                    Message = "已经插入记录" + count + "条";
                } // end if

            //end for
            return count;
        } //end function

        /// <summary>
        ///     根据数据集中在指定的表中，根据表中的指定列的值在数据源中删除数据,带数据访问对象
        /// </summary>
        /// <param name="ds">数据集</param>
        /// <param name="tableName">表名称</param>
        /// <param name="columnName">列名</param>
        /// <param name="DB">数据访问对象</param>
        /// <returns></returns>
        public int DeleteDataSet(DataSet ds, string tableName, string columnName, CommonDB DB)
        {
            var dt = ds.Tables[tableName];
            var ParaChar = GetDBParaChar(DB);
            var count = 0;
            var sqlDelete = "DELETE FROM " + tableName + " WHERE " + columnName + "=" + ParaChar + columnName;
            foreach (DataRow dr in dt.Rows)
            {
                IDataParameter[] paras = { DB.GetParameter(ParaChar + columnName, dr[columnName]) };
                count += DB.ExecuteNonQuery(sqlDelete, CommandType.Text, paras);
                if (DB.ErrorMessage != "")
                    throw new Exception(DB.ErrorMessage);
                if (count >= dt.Rows.Count) break;
            }

            return count;
        }

        /// <summary>
        ///     根据主键信息从数据源查询数据表到数据集中
        /// </summary>
        /// <param name="tableName">数据源中的表名称</param>
        /// <param name="pkNames">主键名称数组</param>
        /// <param name="pkValues">主键值数组，必须和主键名对应</param>
        /// <returns>数据集</returns>
        public DataSet SelectDataSet(string tableName, string[] pkNames, object[] pkValues)
        {
            return SelectDataSet("*", tableName, pkNames, pkValues);
        }

        /// <summary>
        ///     根据主键信息从数据源查询数据表到数据集中
        /// </summary>
        /// <param name="fields">字段列表</param>
        /// <param name="tableName">数据源中的表名称</param>
        /// <param name="pkNames">主键名称数组</param>
        /// <param name="pkValues">主键值数组，必须和主键名对应</param>
        /// <param name="DB">数据访问对象</param>
        /// <returns></returns>
        public DataSet SelectDataSet(string fields, string tableName, string[] pkNames, object[] pkValues, CommonDB DB)
        {
            var ParaChar = GetDBParaChar(DB);
            var sqlSelect = "SELECT " + fields + " FROM " + tableName + " WHERE 1=1 ";
            var paras = new IDataParameter[pkNames.Length];
            for (var i = 0; i < pkNames.Length; i++)
            {
                sqlSelect += " And " + pkNames[i] + "=" + ParaChar + pkNames[i];
                paras[i] = DB.GetParameter(ParaChar + pkNames[i], pkValues[i]);
            }

            var ds = DB.ExecuteDataSet(sqlSelect, CommandType.Text, paras);
            ds.Tables[0].TableName = tableName;
            return ds;
        }

        /// <summary>
        ///     根据主键信息从数据源查询数据表到数据集中
        /// </summary>
        /// <param name="fields">字段列表</param>
        /// <param name="tableName">数据源中的表名称</param>
        /// <param name="pkNames">主键名称数组</param>
        /// <param name="pkValues">主键值数组，必须和主键名对应</param>
        /// <returns>数据集</returns>
        public DataSet SelectDataSet(string fields, string tableName, string[] pkNames, object[] pkValues)
        {
            CommonDB DB = GetDBHelper();
            var ParaChar = GetDBParaChar(DB);
            var sqlSelect = "SELECT " + fields + " FROM " + tableName + " WHERE 1=1 ";
            var paras = new IDataParameter[pkNames.Length];
            for (var i = 0; i < pkNames.Length; i++)
            {
                sqlSelect += " And " + pkNames[i] + "=" + ParaChar + pkNames[i];
                paras[i] = DB.GetParameter(ParaChar + pkNames[i], pkValues[i]);
            }

            var ds = DB.ExecuteDataSet(sqlSelect, CommandType.Text, paras);
            ds.Tables[0].TableName = tableName;
            return ds;
        }

        /// <summary>
        ///     更新数据集中的字段到数据源中
        /// </summary>
        /// <param name="sDs">源数据集</param>
        /// <param name="tableName">要更新的表</param>
        /// <param name="fieldName">要更新的字段</param>
        /// <param name="fieldValue">字段的值</param>
        /// <param name="pkName">主键名称</param>
        /// <param name="DB">数据访问对象</param>
        /// <returns></returns>
        public int UpdateField(DataSet sDs, string tableName, string fieldName, object fieldValue, string pkName,
            CommonDB DB)
        {
            var ds = sDs.Copy();
            var dt = ds.Tables[tableName];
            fieldName = fieldName.ToUpper();
            pkName = pkName.ToUpper();

            for (var i = 0; i < dt.Columns.Count; i++)
            {
                var colName = dt.Columns[i].ColumnName.ToUpper();
                if (colName == fieldName || colName == pkName)
                    continue;
                dt.Columns.Remove(colName);
                i = 0; //集合元素位置可能已经迁移，所以需要重新从头开始查找
            }

            dt.PrimaryKey = new[] { dt.Columns[pkName] };
            foreach (DataRow dr in dt.Rows) dr[fieldName] = fieldValue;

            var updCount = UpdateDataSet(ds, DB);
            return updCount;
        }

        #endregion

        #region 内部方法

        /// <summary>
        ///     获取特定数据库参数字符
        /// </summary>
        /// <param name="DB">数据库类型</param>
        /// <returns></returns>
        private static string GetDBParaChar(CommonDB DB)
        {
            return DB.CurrentDBMSType == DBMSType.Oracle ? ":" : "@";
        }

        /// <summary>
        ///     更新数据表到数据源中
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="SQL"></param>
        /// <returns></returns>
        private static int UpdateDataTable(DataTable dt, string SQL)
        {
            CommonDB DB = GetDBHelper();
            var ParaChar = GetDBParaChar(DB);
            SQL = SQL.Replace("@@", ParaChar);
            var count = 0;
            DB.BeginTransaction();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var paras = new IDataParameter[dt.Columns.Count];
                    for (var i = 0; i < dt.Columns.Count; i++)
                        paras[i] = DB.GetParameter(ParaChar + dt.Columns[i].ColumnName, dr[i]);
                    count += DB.ExecuteNonQuery(SQL, CommandType.Text, paras);
                    if (DB.ErrorMessage != "")
                        throw new Exception(DB.ErrorMessage);
                }

                DB.Commit();
            }
            catch (Exception ex)
            {
                DB.Rollback();
                throw ex;
            }

            return count;
        }

        /// <summary>
        ///     自动保存数据表中的数据到数据库
        ///     <remarks>更新时间：2011.11.16</remarks>
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="insertSQL"></param>
        /// <param name="updateSQL"></param>
        /// <param name="DB"></param>
        /// <returns></returns>
        private static int SaveDataTable(DataTable dt, string insertSQL, string updateSQL, CommonDB DB)
        {
            //CommonDB DB = MyDB.GetDBHelper();
            var ParaChar = GetDBParaChar(DB);
            insertSQL = insertSQL.Replace("@@", ParaChar);
            updateSQL = updateSQL.Replace("@@", ParaChar);
            var count = 0;
            DB.BeginTransaction();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var paras = new IDataParameter[dt.Columns.Count];
                    for (var i = 0; i < dt.Columns.Count; i++)
                        paras[i] = DB.GetParameter(ParaChar + dt.Columns[i].ColumnName, dr[i]);
                    //先更新，如果没有记录受影响再次尝试执行插入
                    var tempCount = DB.ExecuteNonQuery(updateSQL, CommandType.Text, paras);
                    if (tempCount <= 0)
                        tempCount = DB.ExecuteNonQuery(insertSQL, CommandType.Text, paras);

                    count += tempCount;

                    if (DB.ErrorMessage != "")
                        throw new Exception(DB.ErrorMessage);
                }

                DB.Commit();
            }
            catch (Exception ex)
            {
                DB.Rollback();
                throw ex;
            }

            return count;
        }

        /// <summary>
        ///     更新数据表，带数据访问对象
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="SQL"></param>
        /// <param name="DB"></param>
        /// <returns></returns>
        private int UpdateDataTable(DataTable dt, string SQL, CommonDB DB)
        {
            var ParaChar = GetDBParaChar(DB);
            SQL = SQL.Replace("@@", ParaChar);
            var count = 0;

            foreach (DataRow dr in dt.Rows)
            {
                var paras = new IDataParameter[dt.Columns.Count];
                for (var i = 0; i < dt.Columns.Count; i++)
                    paras[i] = DB.GetParameter(ParaChar + dt.Columns[i].ColumnName, dr[i]);
                count += DB.ExecuteNonQuery(SQL, CommandType.Text, paras);
                if (DB.ErrorMessage != "")
                    throw new Exception(DB.ErrorMessage);
            }

            return count;
        }


        /// <summary>
        ///     为数据表生成更新SQL语句，参数名带@@前缀[不更新主键]
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <returns></returns>
        private static string GetSqlUpdate(DataTable dt)
        {
            var sqlUpdate = "UPDATE " + dt.TableName + " SET ";
            if (dt.PrimaryKey.Length > 0)
            {
                var pks = dt.PrimaryKey;
                foreach (DataColumn dc in dt.Columns)
                {
                    var isPk = false;
                    for (var i = 0; i < pks.Length; i++)
                        if (dc == pks[i])
                        {
                            isPk = true;
                            break;
                        }

                    //不更新主键
                    if (!isPk)
                        sqlUpdate += dc.ColumnName + "=@@" + dc.ColumnName + ",";
                }

                sqlUpdate = sqlUpdate.TrimEnd(',') + " WHERE 1=1 ";
                foreach (var dc in dt.PrimaryKey) sqlUpdate += "And " + dc.ColumnName + "=@@" + dc.ColumnName + ",";
                sqlUpdate = sqlUpdate.TrimEnd(',');
                return sqlUpdate;
            }

            throw new Exception("表" + dt.TableName + "没有指定主键，无法生成Update语句！");
        }

        /// <summary>
        ///     为数据表生成插入SQL语句，参数名带@@前缀
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <returns></returns>
        private static string GetSqlInsert(DataTable dt)
        {
            var Items = "";
            var ItemValues = "";
            var sqlInsert = "INSERT INTO " + dt.TableName;

            foreach (DataColumn dc in dt.Columns)
            {
                Items += dc.ColumnName + ",";
                ItemValues += "@@" + dc.ColumnName + ",";
            }

            sqlInsert += "(" + Items.TrimEnd(',') + ") Values(" + ItemValues.TrimEnd(',') + ")";
            return sqlInsert;
        }

        #endregion
    }
}