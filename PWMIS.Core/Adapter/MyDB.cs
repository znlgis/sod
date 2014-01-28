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
using System.Data;
using System.Configuration;
using PWMIS.DataProvider.Data;
using PWMIS.Common;

namespace PWMIS.DataProvider.Adapter
{
    /// <summary>
    /// 应用程序数据访问实例，提供单例模式和工厂模式创建实例对象，根据应用程序配置文件自动创建特定的数据访问对象。
    /// 2008.5.23 增加动态数据集更新功能,7.24增加线程安全的静态实例。
    /// 2009.4.1  增加SQLite 数据库支持。
    /// 2010.1.6  增加 connectionStrings 配置支持
    /// </summary>
    public class MyDB
    {
        private static AdoHelper _instance = null;
        private string _msg = string.Empty;
        private static object lockObj = new object();

        #region 获取静态实例

        /// <summary>
        /// 数据访问静态实例对象，如果有事务并且有可能存在并发访问，请勿使用该属性，而是创建该类的动态实例对象。
        /// </summary>
        public static AdoHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = MyDB.GetDBHelper();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region 获取动态实例对象
        /// <summary>
        /// 通过配置文件获得数据访问对象实例，
        /// 请在应用程序配置文件中创建 EngineType 键，值为[DB]，同时需要创建 [DB]HelperAssembly，[DB]HelperType ，[DB]ConnectionString 键，[DB]值为SQLSERVER/OLEDB/ODBC/ORACLE 之一
        /// 如果未指定 EngineType 键，则使用 connectionStrings 配置节的第一个连接配置信息，必须指明 providerName，可以使用下面的形式：
        /// providerName="PWMIS.DataProvider.Data.SqlServer,PWMIS.Core"
        /// 也可以直接使用 下面的形式：
        /// providerName="SqlServer" ，当然，这种形式的提供程序程序集默认就是 PWMIS.CommonDataProvider.Data 。
        /// 如果有多个，默认取最后一个 providerName
        /// </summary>
        /// <returns>数据访问对象实例</returns>
        public static AdoHelper GetDBHelper()
        {
            string engineType = ConfigurationSettings.AppSettings["EngineType"];
            AdoHelper helper = null;
            if (string.IsNullOrEmpty(engineType))
            {
                //从 connectionStrings 读取
                if (ConfigurationManager.ConnectionStrings.Count == 0)
                    throw new Exception("appSettings 未指明 EngineType 配置键，也未在 connectionStrings 配置节配置连接信息");

                helper = GetDBHelperByConnectionSetting(ConfigurationManager.ConnectionStrings[ConfigurationManager.ConnectionStrings.Count - 1]);
            }
            else
            {
                helper = GetDBHelper(engineType);
                helper.ConnectionString = GetConnectionString();
            }
            return helper;
        }

        /// <summary>
        /// 从 connectionStrings 配置节获取指定 数据连接名称的数据访问对象实例
        /// </summary>
        /// <param name="name">数据连接名称</param>
        /// <returns></returns>
        public static AdoHelper GetDBHelperByConnectionName(string name)
        {
            ConnectionStringSettings connSetting = ConfigurationManager.ConnectionStrings[name];
            if (connSetting == null)
                throw new Exception("未在 connectionStrings 配置节找到指定的 连接名称：" + name);

            return GetDBHelperByConnectionSetting(connSetting);
        }

        private static AdoHelper GetDBHelperByConnectionSetting(ConnectionStringSettings connSetting)
        {
            return GetDBHelperByProviderString(connSetting.ProviderName, connSetting.ConnectionString);
        }

        /// <summary>
        /// 根据提供程序名称字符串和连接字符串，创建提供程序实例
        /// </summary>
        /// <param name="providerName">供程序名称字符串，格式为：提供程序全名称,程序集名称</param>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public static AdoHelper GetDBHelperByProviderString(string providerName, string connectionString)
        {
            string[] providerInfo = providerName.Split(',');
            string helperAssembly;
            string helperType;

            if (providerInfo.Length == 1)
            {
                helperAssembly = "PWMIS.Core";
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
        /// 通过指定的数据库类型（值为SQLSERVER/OLEDB/ODBC/ORACLE 之一）和连接字符串创建一个新的数据访问对象，
        /// 需要配置[DB]HelperAssembly，[DB]HelperType 键，[DB]值为SQLSERVER/OLEDB/ODBC/ORACLE 之一
        /// </summary>
        /// <param name="EngineType">数据库类型（值为SQLSERVER/OLEDB/ODBC/ORACLE 之一）</param>
        /// <param name="ConnectionString">连接字符串</param>
        /// <returns>数据访问对象</returns>
        public static AdoHelper GetDBHelper(string EngineType, string ConnectionString)
        {
            AdoHelper helper = GetDBHelper(EngineType);
            helper.ConnectionString = ConnectionString;
            return helper;
        }

        /// <summary>
        /// 根据数据库管理系统枚举类型和连接字符串创建一个新的数据访问对象实例
        /// </summary>
        /// <param name="DbmsType">数据库类型媒介，有ACCESS/MYSQL/ORACLE/SQLSERVER/SYSBASE/UNKNOWN </param>
        /// <param name="ConnectionString">连接字符串</param>
        /// <returns>数据访问对象</returns>
        public static AdoHelper GetDBHelper(DBMSType DbmsType, string ConnectionString)
        {
            string EngineType = "";
            switch (DbmsType)
            {
                case DBMSType.Access:
                    EngineType = "OleDb"; break;
                case DBMSType.MySql:
                    EngineType = "Odbc"; break;
                case DBMSType.Oracle:
                    EngineType = "Oracle"; break;
                case DBMSType.SqlServer:
                    EngineType = "SqlServer"; break;
                case DBMSType.SqlServerCe:
                    EngineType = "SqlServerCe"; break;
                case DBMSType.Sysbase:
                    EngineType = "OleDb"; break;
                case DBMSType.SQLite:
                    EngineType = "SQLite"; break;
                case DBMSType.UNKNOWN:
                    EngineType = "Odbc"; break;
            }
            AdoHelper helper = GetDBHelper(EngineType);
            helper.ConnectionString = ConnectionString;
            return helper;
        }

        /// <summary>
        /// 根据程序集名称和数据访问对象类型创建一个新的数据访问对象实例。
        /// </summary>
        /// <param name="HelperAssembly">程序集名称</param>
        /// <param name="HelperType">数据访问对象类型</param>
        /// <param name="ConnectionString">连接字符串</param>
        /// <returns>数据访问对象</returns>
        public static AdoHelper GetDBHelper(string HelperAssembly, string HelperType, string ConnectionString)
        {
            AdoHelper helper = null;// CommonDB.CreateInstance(HelperAssembly, HelperType);
            if (HelperAssembly == "PWMIS.Core")
            {
                switch (HelperType)
                {
                    case "PWMIS.DataProvider.Data.SqlServer": helper = new SqlServer(); break;
                    case "PWMIS.DataProvider.Data.Oracle": helper = new Oracle(); break;
                    case "PWMIS.DataProvider.Data.OleDb": helper = new OleDb(); break;
                    case "PWMIS.DataProvider.Data.Odbc": helper = new Odbc(); break;
                    case "PWMIS.DataProvider.Data.Access": helper = new Access(); break;
                    case "PWMIS.DataProvider.Data.SqlServerCe": helper = new SqlServerCe(); break;
                    default: helper = new SqlServer(); break;
                }
            }
            else
            {
                helper = CommonDB.CreateInstance(HelperAssembly, HelperType);
            }
            helper.ConnectionString = ConnectionString;
            return helper;
        }


        /// <summary>
        /// 获得数据访问对象实例，EngineType值为SQLSERVER/OLEDB/ODBC/ORACLE 之一，默认使用 PWMIS.CommonDataProvider.Data.SqlServer
        /// </summary>
        /// <param name="EngineType">数据库引擎类型</param>
        /// <returns>数据访问对象实例</returns>
        private static AdoHelper GetDBHelper(string EngineType)
        {
            string assembly = null;
            string type = null;

            switch (EngineType.ToUpper())
            {
                case "SQLSERVER":
                    assembly = ConfigurationSettings.AppSettings["SqlServerHelperAssembly"];
                    type = ConfigurationSettings.AppSettings["SqlServerHelperType"];
                    if (string.IsNullOrEmpty(assembly))
                    {
                        assembly = "PWMIS.Core";
                        type = "PWMIS.DataProvider.Data.SqlServer";
                    }
                    break;
                case "SQLSERVERCE":
                    assembly = ConfigurationSettings.AppSettings["SqlServerCeHelperAssembly"];
                    type = ConfigurationSettings.AppSettings["SqlServerCeHelperType"];
                    if (string.IsNullOrEmpty(assembly))
                    {
                        assembly = "PWMIS.Core";
                        type = "PWMIS.DataProvider.Data.SqlServerCe";
                    }
                    break;
                case "OLEDB":
                    assembly = ConfigurationSettings.AppSettings["OleDbHelperAssembly"];
                    type = ConfigurationSettings.AppSettings["OleDbHelperType"];
                    if (string.IsNullOrEmpty(assembly))
                    {
                        assembly = "PWMIS.Core";
                        type = "PWMIS.DataProvider.Data.OleDb";
                    }
                    break;
                case "ACCESS":
                    assembly = ConfigurationSettings.AppSettings["OleDbHelperAssembly"];
                    type = ConfigurationSettings.AppSettings["OleDbHelperType"];
                    if (string.IsNullOrEmpty(assembly))
                    {
                        assembly = "PWMIS.Core";
                        type = "PWMIS.DataProvider.Data.Access";
                    }
                    break;
                case "ODBC":
                    assembly = ConfigurationSettings.AppSettings["OdbcHelperAssembly"];
                    type = ConfigurationSettings.AppSettings["OdbcHelperType"];
                    if (string.IsNullOrEmpty(assembly))
                    {
                        assembly = "PWMIS.Core";
                        type = "PWMIS.DataProvider.Data.Odbc";
                    }
                    break;
                case "ORACLE":
                    assembly = ConfigurationSettings.AppSettings["OracleHelperAssembly"];
                    type = ConfigurationSettings.AppSettings["OracleHelperType"];
                    if (string.IsNullOrEmpty(assembly))
                    {
                        assembly = "PWMIS.Core";
                        type = "PWMIS.DataProvider.Data.Oracle";
                    }
                    break;
                //不再支持这里的SQLite类型，因为它在64位，32位，windows,linux 下面有不同的驱动程序，无法在这里确定
                //case "SQLITE":
                //    assembly = ConfigurationSettings.AppSettings["SQLiteHelperAssembly"];
                //    type = ConfigurationSettings.AppSettings["SQLiteHelperType"];
                //    if (string.IsNullOrEmpty(assembly))
                //    {
                //        assembly = "PWMIS.DataProvider.Data.SQLite";
                //        type = "PWMIS.DataProvider.Data.SQLite";
                //    }
                //    break;
                default:
                    assembly = "PWMIS.DataProvider.Data";
                    type = "PWMIS.DataProvider.Data.SqlServer";
                    break;
            }

            return GetDBHelper(assembly, type, null);
        }

        #endregion

        #region 公共静态方法

        /// <summary>
        /// 获得数据访问连接字符串，请在应用程序配置文件中创建 EngineType，[DB]HelperAssembly，[DB]HelperType ,[DB]ConnectionString键，值为SQLSERVER/OLEDB/ODBC/ORACLE 之一
        /// 如果没有找到 [DB]ConnectionString 键，也可以直接使用 ConnectionString 键
        /// </summary>
        /// <returns>数据访问连接字符串</returns>
        public static string GetConnectionString()
        {

            string connectionString = null;

            switch (ConfigurationSettings.AppSettings["EngineType"].ToUpper())
            {
                case "SQLSERVER":
                    connectionString = ConfigurationSettings.AppSettings["SqlServerConnectionString"];
                    break;
                case "OLEDB":
                    connectionString = ConfigurationSettings.AppSettings["OleDbConnectionString"];
                    break;
                case "ODBC":
                    connectionString = ConfigurationSettings.AppSettings["OdbcConnectionString"];
                    break;
                case "ORACLE":
                    connectionString = ConfigurationSettings.AppSettings["OracleConnectionString"];
                    break;
                case "SQLITE":
                    connectionString = ConfigurationSettings.AppSettings["SQLiteConnectionString"];
                    break;
            }
            if (string.IsNullOrEmpty(connectionString))
                connectionString = ConfigurationSettings.AppSettings["ConnectionString"];

            return connectionString;
        }

        /// <summary>
        /// 更新数据集(采用参数形式)，数据表如果指定了主键那么执行更新操作，否则执行插入操作。
        /// </summary>
        /// <param name="ds">数据集</param>
        /// <returns>查询结果受影响的行数</returns>
        public static int UpdateDataSet(DataSet ds)
        {
            int count = 0;
            foreach (DataTable dt in ds.Tables)
            {
                if (dt.PrimaryKey.Length > 0)
                {
                    count += UpdateDataTable(dt, GetSqlUpdate(dt));
                }
                else
                {
                    count += UpdateDataTable(dt, GetSqlInsert(dt));
                }// end if
            }//end for
            return count;
        }//end function

        /// <summary>
        /// 自动将数据集中的数据更新或者插入到数据库
        /// <remarks>更新时间：2011.11.16</remarks>
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="DB"></param>
        /// <returns></returns>
        public static int SaveDataSet(DataSet ds, CommonDB DB)
        {
            int count = 0;
            foreach (DataTable dt in ds.Tables)
            {
                string insertSql = GetSqlInsert(dt);
                string updateSql = GetSqlUpdate(dt);
                count += SaveDataTable(dt, insertSql, updateSql, DB);
            }//end for
            return count;
        }
        /// <summary>
        /// 根据数据集中在指定的表中，根据表中的指定列的值在数据源中删除数据
        /// </summary>
        /// <param name="ds">数据集</param>
        /// <param name="tableName">表名称</param>
        /// <param name="columnName">列名</param>
        /// <returns>查询所影响的行数</returns>
        public static int DeleteDataSet(DataSet ds, string tableName, string columnName)
        {
            DataTable dt = ds.Tables[tableName];

            CommonDB DB = MyDB.GetDBHelper();
            string ParaChar = GetDBParaChar(DB);
            int count = 0;

            string sqlDelete = "DELETE FROM " + tableName + " WHERE " + columnName + "=" + ParaChar + columnName;

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
        /// 获取当前操作信息
        /// </summary>
        public string Message
        {
            get { return _msg; }
        }
        /// <summary>
        /// 更新数据集，带数据访问对象
        /// </summary>
        /// <param name="ds">数据集</param>
        /// <param name="DB">数据访问对象</param>
        /// <returns></returns>
        public int UpdateDataSet(DataSet ds, CommonDB DB)
        {
            int count = 0;
            foreach (DataTable dt in ds.Tables)
            {
                if (dt.PrimaryKey.Length > 0)
                {
                    count += UpdateDataTable(dt, GetSqlUpdate(dt), DB);
                    _msg = "已经更新记录" + count + "条";
                }
                else
                {
                    count += UpdateDataTable(dt, GetSqlInsert(dt), DB);
                    _msg = "已经插入记录" + count + "条";
                }// end if
            }//end for
            return count;
        }//end function

        /// <summary>
        /// 根据数据集中在指定的表中，根据表中的指定列的值在数据源中删除数据,带数据访问对象
        /// </summary>
        /// <param name="ds">数据集</param>
        /// <param name="tableName">表名称</param>
        /// <param name="columnName">列名</param>
        /// <param name="DB">数据访问对象</param>
        /// <returns></returns>
        public int DeleteDataSet(DataSet ds, string tableName, string columnName, CommonDB DB)
        {
            DataTable dt = ds.Tables[tableName];
            string ParaChar = GetDBParaChar(DB);
            int count = 0;
            string sqlDelete = "DELETE FROM " + tableName + " WHERE " + columnName + "=" + ParaChar + columnName;
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
        /// 根据主键信息从数据源查询数据表到数据集中
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
        /// 根据主键信息从数据源查询数据表到数据集中
        /// </summary>
        /// <param name="fields">字段列表</param>
        /// <param name="tableName">数据源中的表名称</param>
        /// <param name="pkNames">主键名称数组</param>
        /// <param name="pkValues">主键值数组，必须和主键名对应</param>
        /// <param name="DB">数据访问对象</param>
        /// <returns></returns>
        public DataSet SelectDataSet(string fields, string tableName, string[] pkNames, object[] pkValues, CommonDB DB)
        {
            string ParaChar = GetDBParaChar(DB);
            string sqlSelect = "SELECT " + fields + " FROM " + tableName + " WHERE 1=1 ";
            IDataParameter[] paras = new IDataParameter[pkNames.Length];
            for (int i = 0; i < pkNames.Length; i++)
            {
                sqlSelect += " And " + pkNames[i] + "=" + ParaChar + pkNames[i];
                paras[i] = DB.GetParameter(ParaChar + pkNames[i], pkValues[i]);
            }
            DataSet ds = DB.ExecuteDataSet(sqlSelect, CommandType.Text, paras);
            ds.Tables[0].TableName = tableName;
            return ds;
        }

        /// <summary>
        /// 根据主键信息从数据源查询数据表到数据集中
        /// </summary>
        /// <param name="fields">字段列表</param>
        /// <param name="tableName">数据源中的表名称</param>
        /// <param name="pkNames">主键名称数组</param>
        /// <param name="pkValues">主键值数组，必须和主键名对应</param>
        /// <returns>数据集</returns>
        public DataSet SelectDataSet(string fields, string tableName, string[] pkNames, object[] pkValues)
        {
            CommonDB DB = MyDB.GetDBHelper();
            string ParaChar = GetDBParaChar(DB);
            string sqlSelect = "SELECT " + fields + " FROM " + tableName + " WHERE 1=1 ";
            IDataParameter[] paras = new IDataParameter[pkNames.Length];
            for (int i = 0; i < pkNames.Length; i++)
            {
                sqlSelect += " And " + pkNames[i] + "=" + ParaChar + pkNames[i];
                paras[i] = DB.GetParameter(ParaChar + pkNames[i], pkValues[i]);
            }
            DataSet ds = DB.ExecuteDataSet(sqlSelect, CommandType.Text, paras);
            ds.Tables[0].TableName = tableName;
            return ds;
        }

        /// <summary>
        /// 更新数据集中的字段到数据源中
        /// </summary>
        /// <param name="sDs">源数据集</param>
        /// <param name="tableName">要更新的表</param>
        /// <param name="fieldName">要更新的字段</param>
        /// <param name="fieldValue">字段的值</param>
        /// <param name="pkName">主键名称</param>
        /// <param name="DB">数据访问对象</param>
        /// <returns></returns>
        public int UpdateField(DataSet sDs, string tableName, string fieldName, object fieldValue, string pkName, CommonDB DB)
        {
            DataSet ds = sDs.Copy();
            DataTable dt = ds.Tables[tableName];
            fieldName = fieldName.ToUpper();
            pkName = pkName.ToUpper();

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                string colName = dt.Columns[i].ColumnName.ToUpper();
                if (colName == fieldName || colName == pkName)
                    continue;
                dt.Columns.Remove(colName);
                i = 0;//集合元素位置可能已经迁移，所以需要重新从头开始查找
            }
            dt.PrimaryKey = new DataColumn[] { dt.Columns[pkName] };
            foreach (DataRow dr in dt.Rows)
            {
                dr[fieldName] = fieldValue;
            }

            int updCount = UpdateDataSet(ds, DB);
            return updCount;
        }

        #endregion

        #region 内部方法
        /// <summary>
        /// 获取特定数据库参数字符
        /// </summary>
        /// <param name="DB">数据库类型</param>
        /// <returns></returns>
        private static string GetDBParaChar(CommonDB DB)
        {
            return DB is Oracle ? ":" : "@";
        }

        /// <summary>
        /// 更新数据表到数据源中
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="SQL"></param>
        /// <returns></returns>
        private static int UpdateDataTable(DataTable dt, string SQL)
        {
            CommonDB DB = MyDB.GetDBHelper();
            string ParaChar = GetDBParaChar(DB);
            SQL = SQL.Replace("@@", ParaChar);
            int count = 0;
            DB.BeginTransaction();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    IDataParameter[] paras = new IDataParameter[dt.Columns.Count];
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        paras[i] = DB.GetParameter(ParaChar + dt.Columns[i].ColumnName, dr[i]);
                    }
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
        /// 自动保存数据表中的数据到数据库
        /// <remarks>更新时间：2011.11.16</remarks>
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="insertSQL"></param>
        /// <param name="updateSQL"></param>
        /// <param name="DB"></param>
        /// <returns></returns>
        private static int SaveDataTable(DataTable dt, string insertSQL, string updateSQL, CommonDB DB)
        {
            //CommonDB DB = MyDB.GetDBHelper();
            string ParaChar = GetDBParaChar(DB);
            insertSQL = insertSQL.Replace("@@", ParaChar);
            updateSQL = updateSQL.Replace("@@", ParaChar);
            int count = 0;
            DB.BeginTransaction();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    IDataParameter[] paras = new IDataParameter[dt.Columns.Count];
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        paras[i] = DB.GetParameter(ParaChar + dt.Columns[i].ColumnName, dr[i]);
                    }
                    //先更新，如果没有记录受影响再次尝试执行插入
                    int tempCount = DB.ExecuteNonQuery(updateSQL, CommandType.Text, paras);
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
        /// 更新数据表，带数据访问对象
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="SQL"></param>
        /// <param name="DB"></param>
        /// <returns></returns>
        private int UpdateDataTable(DataTable dt, string SQL, CommonDB DB)
        {
            string ParaChar = GetDBParaChar(DB);
            SQL = SQL.Replace("@@", ParaChar);
            int count = 0;

            foreach (DataRow dr in dt.Rows)
            {
                IDataParameter[] paras = new IDataParameter[dt.Columns.Count];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    paras[i] = DB.GetParameter(ParaChar + dt.Columns[i].ColumnName, dr[i]);
                }
                count += DB.ExecuteNonQuery(SQL, CommandType.Text, paras);
                if (DB.ErrorMessage != "")
                    throw new Exception(DB.ErrorMessage);
            }

            return count;

        }


        /// <summary>
        /// 为数据表生成更新SQL语句，参数名带@@前缀[不更新主键]
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <returns></returns>
        private static string GetSqlUpdate(DataTable dt)
        {
            string sqlUpdate = "UPDATE " + dt.TableName + " SET ";
            if (dt.PrimaryKey.Length > 0)
            {
                DataColumn[] pks = dt.PrimaryKey;
                foreach (DataColumn dc in dt.Columns)
                {
                    bool isPk = false;
                    for (int i = 0; i < pks.Length; i++)
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
                foreach (DataColumn dc in dt.PrimaryKey)
                {
                    sqlUpdate += "And " + dc.ColumnName + "=@@" + dc.ColumnName + ",";
                }
                sqlUpdate = sqlUpdate.TrimEnd(',');
                return sqlUpdate;

            }
            else
            {
                throw new Exception("表" + dt.TableName + "没有指定主键，无法生成Update语句！");
            }
        }

        /// <summary>
        /// 为数据表生成插入SQL语句，参数名带@@前缀
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <returns></returns>
        private static string GetSqlInsert(DataTable dt)
        {
            string Items = "";
            string ItemValues = "";
            string sqlInsert = "INSERT INTO " + dt.TableName;

            foreach (DataColumn dc in dt.Columns)
            {
                Items += dc.ColumnName + ",";
                ItemValues += "@@" + dc.ColumnName + ",";
            }
            sqlInsert += "(" + Items.TrimEnd(',') + ") Values(" + ItemValues.TrimEnd(',') + ")";
            return sqlInsert;
        }

        #endregion

        public DBMSType DBMSType
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public SQLPage SQLPage
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

    }
}

