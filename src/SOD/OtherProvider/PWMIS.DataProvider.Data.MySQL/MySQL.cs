﻿using System;
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
            //类型和名字转换，请参考：http://stackoverflow.com/questions/1230203/how-can-i-determine-the-column-type-represented-by-a-mysql-net-connectors-mysq
            switch (dbType)
            {
                case MySqlDbType.String: return "CHAR";
                case MySqlDbType.NewDecimal: return "NUMERIC";
                case MySqlDbType.Byte:
                case MySqlDbType.UByte:
                    return "TINYINT";
                case MySqlDbType.Int16:
                case MySqlDbType.UInt16:
                    return "SMALLINT";
                case MySqlDbType.Int24:
                case MySqlDbType.UInt24:
                    return "MEDIUMINT";
                case MySqlDbType.Int32:
                case MySqlDbType.UInt32:
                    return "INT";
                case MySqlDbType.Int64:
                case MySqlDbType.UInt64:
                    return "BIGINT";
                case MySqlDbType.Float:
                    return "REAL";
                default:
                    return dbType.ToString();
            }
        }


        /// <summary>
        /// 预处理SQL语句，语句中不能包含"`"(反引号，tab键上面的那个符号)号，如果需要，请使用参数化查询。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        protected override string PrepareSQL( string sql, IDataParameter[] parameters = null)
        {
            return sql.Replace("[", "`").Replace("]", "`").Replace("@", "?");
        }

        /// <summary>
        /// 获取数据库参数前缀
        /// </summary>
        public override string GetParameterChar
        {
            get
            {
                return "?";
            }
        }

        /// <summary>
        /// 连接所使用的用户ID
        /// </summary>
        public override string ConnectionUserID
        {
            get { return ((MySqlConnectionStringBuilder)this.ConnectionStringBuilder).UserID; }
        }

        /// <summary>
        /// 连接字符串生成器
        /// </summary>
        public override System.Data.Common.DbConnectionStringBuilder ConnectionStringBuilder
        {
            get { return new MySqlConnectionStringBuilder(this.ConnectionString); }
        }

        private string _insertKey;
        /// <summary>
        /// 在插入具有自增列的数据后，获取刚才自增列的数据的
        /// </summary>
        public override string InsertKey
        {
            get
            {
                if (string.IsNullOrEmpty(_insertKey))
                    //MySQL 支持 SELECT @@IDENTITY，但是 MariaDB 不支持，换用下面都支持的方式：
                    //注意必须在一个连接中有效
                    return "SELECT LAST_INSERT_ID() ";
                else
                    return _insertKey;
            }
            set
            {
                _insertKey = value;
            }
        }
    }
}
