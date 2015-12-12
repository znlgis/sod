using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace PWMIS.DataProvider.Data
{
    /// <summary>
    /// MySQL数据访问类，需要MySQL.NET 6.3.6支持，这里使用的是For2.0类库
    /// </summary>
    public class MySQL : AdoHelper
    {
        /// <summary>
		/// 默认构造函数
		/// </summary>
        public MySQL()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}
        /// <summary>
        /// 获取当前数据库类型的枚举
        /// </summary>
        public override PWMIS.Common.DBMSType CurrentDBMSType
        {
            get { return PWMIS.Common.DBMSType.MySql ; }
        }

        /// <summary>
        /// 创建并且打开数据库连接
        /// </summary>
        /// <returns>数据库连接</returns>
        protected override IDbConnection GetConnection()
        {
            IDbConnection conn = base.GetConnection();
            if (conn == null)
            {
                conn = new MySqlConnection(base.ConnectionString);
                //conn.Open ();
            }
            return conn;
        }

        /// <summary>
        /// 获取数据适配器实例
        /// </summary>
        /// <returns>数据适配器</returns>
        protected override IDbDataAdapter GetDataAdapter(IDbCommand command)
        {
            IDbDataAdapter ada = new MySqlDataAdapter((MySqlCommand)command);
            return ada;
        }

        /// <summary>
        /// 获取一个新参数对象
        /// </summary>
        /// <returns>特定于数据源的参数对象</returns>
        public override IDataParameter GetParameter()
        {
            return new MySqlParameter();
        }

        /// <summary>
        ///  获取一个新参数对象
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="dbType">参数数据类型</param>
        /// <param name="size">参数大小</param>
        /// <returns>特定于数据源的参数对象</returns>
        public override IDataParameter GetParameter(string paraName, System.Data.DbType dbType, int size)
        {
            MySqlParameter para = new MySqlParameter();
            para.ParameterName = paraName;
            para.DbType = dbType;
            para.Size = size;
            return para;
        }

        public override string GetNativeDbTypeName(IDataParameter para)
        {
            MySqlParameter mysqlPara = para as MySqlParameter;
            MySqlDbType dbType = mysqlPara.MySqlDbType;
            if (dbType == MySqlDbType.Int32)
                return "INT";
            else
                return dbType.ToString();
        }

        /// <summary>
        /// 返回此 MySqlConnection 的数据源的架构信息。
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="restrictionValues">请求的架构的一组限制值</param>
        /// <returns>数据库架构信息表</returns>
        public override DataTable GetSchema(string collectionName, string[] restrictionValues)
        {
            using (MySqlConnection conn = (MySqlConnection)this.GetConnection())
            {
                conn.Open();
                if (restrictionValues == null && string.IsNullOrEmpty(collectionName))
                    return conn.GetSchema();
                else if (restrictionValues == null && !string.IsNullOrEmpty(collectionName))
                    return conn.GetSchema(collectionName);
                else
                    return conn.GetSchema(collectionName, restrictionValues);
            }
        }

        /// <summary>
        /// 预处理SQL语句，语句中不能包含"`"(反引号，tab键上面的那个符号)号，如果需要，请使用参数化查询。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        protected override string PrepareSQL( string sql)
        {
            return sql.Replace("[", "`").Replace("]", "`").Replace("@", "?");
        }

        /// <summary>
        /// 获取数据库参数前缀资费
        /// </summary>
        public override string GetParameterChar
        {
            get
            {
                return "?";
            }
        }

        public override string ConnectionUserID
        {
            get { throw new NotImplementedException(); }
        }

        public override System.Data.Common.DbConnectionStringBuilder ConnectionStringBuilder
        {
            get { throw new NotImplementedException(); }
        }
    }
}
