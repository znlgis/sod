using System;
using System.Data;
using System.Data.Common;
using Npgsql;
using PWMIS.Common;
using PWMIS.PostgreSQLClient.Properties;
//using System.Linq;

namespace PWMIS.DataProvider.Data
{
    /// <summary>
    ///     PostgreSQL数据访问类
    /// </summary>
    public class PostgreSQL : AdoHelper
    {
        /// <summary>
        ///     获取当前数据库类型的枚举
        /// </summary>
        public override DBMSType CurrentDBMSType
        {
            get { return DBMSType.PostgreSQL; }
        }

        /// <summary>
        ///     获取数据库参数前缀资费
        /// </summary>
        public override string GetParameterChar
        {
            get { return ":"; }
        }

        /// <summary>
        ///     获取或者设置自增列对应的序列名称
        /// </summary>
        public override string InsertKey
        {
            get { return string.Format("select currval('\"{0}\"')", base.InsertKey); }
            set { base.InsertKey = value; }
        }

        public override DbConnectionStringBuilder ConnectionStringBuilder
        {
            get { throw new NotImplementedException(); }
        }

        public override string ConnectionUserID
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        ///     创建并且打开数据库连接
        /// </summary>
        /// <returns>数据库连接</returns>
        protected override IDbConnection GetConnection()
        {
            var conn = base.GetConnection();
            if (conn == null)
            {
                conn = new NpgsqlConnection(ConnectionString);
                //conn.Open ();
            }
            return conn;
        }

        /// <summary>
        ///     获取数据适配器实例
        /// </summary>
        /// <returns>数据适配器</returns>
        protected override IDbDataAdapter GetDataAdapter(IDbCommand command)
        {
            IDbDataAdapter ada = new NpgsqlDataAdapter((NpgsqlCommand) command);
            return ada;
        }

        /// <summary>
        ///     获取一个新参数对象
        /// </summary>
        /// <returns>特定于数据源的参数对象</returns>
        public override IDataParameter GetParameter()
        {
            return new NpgsqlParameter();
        }

        /// <summary>
        ///     获取一个新参数对象
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="dbType">参数数据类型</param>
        /// <param name="size">参数大小</param>
        /// <returns>特定于数据源的参数对象</returns>
        public override IDataParameter GetParameter(string paraName, DbType dbType, int size)
        {
            var para = new NpgsqlParameter();
            para.ParameterName = paraName;
            para.DbType = dbType;
            para.Size = size;
            return para;
        }

        public override string GetNativeDbTypeName(IDataParameter para)
        {
            return ((NpgsqlParameter) para).NpgsqlDbType.ToString();
        }

        /// <summary>
        ///     返回此 NpgsqlConnection 的数据源的架构信息。
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="restrictionValues">请求的架构的一组限制值</param>
        /// <returns>数据库架构信息表</returns>
        public override DataTable GetSchema(string collectionName, string[] restrictionValues)
        {
            using (var conn = (NpgsqlConnection) GetConnection())
            {
                conn.Open();
                if (restrictionValues == null && string.IsNullOrEmpty(collectionName))
                    return conn.GetSchema();
                if (restrictionValues == null && !string.IsNullOrEmpty(collectionName))
                {
                    if (collectionName == "Procedures")
                        return getProcedures();
                    return conn.GetSchema(collectionName); //Procedures
                }
                if (collectionName == "ProcedureParameters")
                    return getFunctionArgsInfo(restrictionValues[2]);
                return conn.GetSchema(collectionName, restrictionValues);
            }
        }

        /// <summary>
        ///     预处理SQL语句，语句中不能包含"`"(反引号，tab键上面的那个符号)号，如果需要，请使用参数化查询。
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns></returns>
        protected override string PrepareSQL(ref string SQL)
        {
            return SQL.Replace("[", "\"").Replace("]", "\"").Replace("@", ":");
        }

        /// <summary>
        ///     定义获取PostgreSQL的函数参数的函数
        ///     <seealso cref="http://www.alberton.info/postgresql_meta_info.html" />
        /// </summary>
        private void createFunctionArgsInfo()
        {
            //由于函数定义语句较长，放到了资源文件中
            var sql = Resources.sql_function_args;
            SqlServerCompatible = false;
            ExecuteNonQuery(sql);
        }

        /// <summary>
        ///     获取函数的参数信息
        /// </summary>
        /// <param name="functionName">函数名</param>
        /// <returns></returns>
        private DataTable getFunctionArgsInfo(string functionName)
        {
            var sql = string.Format("select * from function_args('{0}','public');", functionName);
            DataSet ds = null;
            try
            {
                ds = ExecuteDataSet(sql);
            }
            catch
            {
                createFunctionArgsInfo();
                ds = ExecuteDataSet(sql);
            }

            var dt = ds.Tables[0];
            dt.Columns["pos"].ColumnName = "ordinal_position";
            dt.Columns["argname"].ColumnName = "PARAMETER_NAME";
            dt.Columns["datatype"].ColumnName = "DATA_TYPE";
            dt.Columns["direction"].ColumnName = "PARAMETER_MODE";
            dt.Columns.Add("IS_RESULT", typeof (string));
            dt.Columns.Add("CHARACTER_MAXIMUM_LENGTH", typeof (int));

            foreach (DataRow row in dt.Rows)
            {
                if (row["PARAMETER_NAME"] == DBNull.Value) row["PARAMETER_NAME"] = "";
                row["IS_RESULT"] = row["PARAMETER_NAME"].ToString() == "RETURN VALUE" ? "YES" : "NO";
                row["PARAMETER_MODE"] = row["PARAMETER_MODE"].ToString() == "o"
                    ? "OUT"
                    : row["PARAMETER_MODE"].ToString() == "i" ? "IN" : row["PARAMETER_MODE"];
            }
            return dt;
        }

        private DataTable getProcedures()
        {
            var sql = @"SELECT routine_name
  FROM information_schema.routines
 WHERE specific_schema NOT IN
       ('pg_catalog', 'information_schema')
   AND type_udt_name != 'trigger';";
            return ExecuteDataSet(sql).Tables[0];
        }
    }
}