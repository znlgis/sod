/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap 
 * ========================================================================
 * 该类的作用
 * 
 * 作者：田杰     时间：2020-7-9
 * 版本：V1.0
 * ========================================================================
*/
using System;
using System.Data;
using Dm;


namespace PWMIS.DataProvider.Data
{
    /// <summary>
    /// OracleServer 数据处理
    /// </summary>
    public sealed class Dameng : AdoHelper
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Dameng()
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
            get { return PWMIS.Common.DBMSType.Dameng; }
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
                conn = new DmConnection(base.ConnectionString);
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
            IDbDataAdapter ada = new DmDataAdapter((DmCommand)command);
            return ada;
        }

        /// <summary>
        /// 获取一个新参数对象
        /// </summary>
        /// <returns>特定于数据源的参数对象</returns>
        public override IDataParameter GetParameter()
        {
            return new DmParameter();
        }

        //达梦数据库自增问题，请参考官方技术文档：https://eco.dameng.com/document/dm/zh-cn/faq/faq-sql-gramm.html

        private string _insertKey;
        /// <summary>
        /// 在插入具有自增列的数据后，获取刚才自增列的数据的
        /// </summary>
        public override string InsertKey
        {
            get
            {
                if (string.IsNullOrEmpty(_insertKey))
                    //注意必须在一个连接中有效
                    return "SELECT @@IDENTITY ";
                else
                    return _insertKey;
            }
            set
            {
                _insertKey = value;
            }
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
            DmParameter para = new DmParameter();
            para.ParameterName = paraName;
            if (size > 2000)
            {
                //长度大于2000，将引发clob类型错误的问题，详细请参考 http://blog.csdn.net/pojianbing/article/details/2789426
                para.DmSqlType = DmDbType.Clob;
                para.Size = size;
            }
            else
            {
                para.DbType = dbType;
                para.Size = size;
            }

            return para;
        }

        public override string GetNativeDbTypeName(IDataParameter para)
        {
            DmParameter oraPara = (DmParameter)para;
            DmDbType oraType = oraPara.DmSqlType;
            if (oraType == DmDbType.Date)
                return "Date";
            else if (oraType == DmDbType.Int32)
                return "INT";
            else
                return oraType.ToString();
           
        }

        

        /// <summary>
        /// 预处理SQL语句，语句中不能包含"["，"]"左右中括号，如果需要，请使用参数化查询。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        protected override string PrepareSQL( string sql, IDataParameter[] parameters = null)
        {
            string temp= sql.Replace("[", "\"").Replace("]", "\"");
            //select @@IDENTITY; 未避免出现替换掉获取插入的自增值，判断SQL语句的长度，考虑下面的SQL语句：
            //select * from t where f=@a
            //真正又参数的SQL长度至少超过20个字符。
            return sql.Length>=20? temp.Replace("@", ":") : temp;
        }

        public override System.Data.Common.DbConnectionStringBuilder ConnectionStringBuilder
        {
            get { return new DmConnectionStringBuilder(this.ConnectionString); }
        }

        public override string ConnectionUserID
        {
            get { return ((DmConnectionStringBuilder)ConnectionStringBuilder).User; }
        }

        /// <summary>
        /// 获取Oracle数据库参数前缀资费
        /// <remarks>由网友路人甲.aspx 补充</remarks>
        /// </summary>
        public override string GetParameterChar
        {
            get
            {
                return ":";
            }
        }

       

    }
}
