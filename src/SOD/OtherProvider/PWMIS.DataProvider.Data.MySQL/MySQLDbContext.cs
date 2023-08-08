using MySql.Data.MySqlClient;
using PWMIS.Common;
using PWMIS.Core.Interface;
using PWMIS.DataMap.Entity;

namespace PWMIS.DataProvider.Data
{
    /// <summary>
    ///     MySQL 数据上下文提供程序
    /// </summary>
    public class MySQLDbContext : IDbContextProvider
    {
        public MySQLDbContext(AdoHelper db)
        {
            CurrentDataBase = db;
        }

        public AdoHelper CurrentDataBase { get; }

        /// <summary>
        ///     检查表是否存在，如果不存在，则创建
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public bool CheckTableExists<T>(T entity = null) where T : EntityBase, new()
        {
            //创建表
            if (CurrentDataBase.CurrentDBMSType == DBMSType.MySql)
            {
                if (entity == null) entity = new T();
                var dsScheme = CurrentDataBase.GetSchema("Tables", new[] { null, null, null, "BASE TABLE" });
                var rows = dsScheme.Select("table_name='" + entity.GetTableName() + "'");
                if (rows.Length == 0)
                {
                    var ecmd = new EntityCommand(entity, CurrentDataBase);
                    var sql = ecmd.CreateTableCommand;
                    CurrentDataBase.ExecuteNonQuery(sql);
                    return false;
                }

                return true;
            }

            return false;
        }

        /* 一般情况下将会调用基类的InitializeTable 方法，此处仅供示例 */
        /// <summary>
        ///     检查实体类对应的表是否存在，如果不存在则创建表并执行可选的SQL语句，比如为表增加索引等。
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="initSql">要初始化执行的SQL语句</param>
        public void InitializeTable<T>(string initSql, T entity = null) where T : EntityBase, new()
        {
            //创建表
            if (CurrentDataBase.CurrentDBMSType == DBMSType.MySql)
            {
                if (entity == null) entity = new T();
                var dsScheme = CurrentDataBase.GetSchema("Tables", new[] { null, null, null, "BASE TABLE" });
                var rows = dsScheme.Select("table_name='" + entity.GetTableName() + "'");
                if (rows.Length == 0)
                {
                    var ecmd = new EntityCommand(entity, CurrentDataBase);
                    var sql = ecmd.CreateTableCommand;
                    if (!string.IsNullOrEmpty(initSql)) sql = sql + ";\r\n" + initSql + ";\r\n";
                    CurrentDataBase.ExecuteNonQuery(sql);
                }
            }
        }


        /// <summary>
        ///     检查MySQL数据库是否存在，如果不存在会自动创建
        /// </summary>
        /// <returns></returns>
        public bool CheckDB()
        {
            var connBuilder = CurrentDataBase.ConnectionStringBuilder as MySqlConnectionStringBuilder;
            var database = connBuilder.Database;
            if (!string.IsNullOrEmpty(database))
            {
                //utf8_bin 区分大小写 ，utf8_general_ci 不区分大小写
                var sqlformat = "CREATE DATABASE IF NOT EXISTS `{0}`  DEFAULT CHARSET utf8 COLLATE utf8_bin";
                var sql = string.Format(sqlformat, database);
                //移除初始化的数据库名称，否则下面的执行打不开数据库
                connBuilder.Database = "";
                AdoHelper db = new MySQL();
                db.ConnectionString = connBuilder.ConnectionString;
                db.ExecuteNonQuery(sql);
            }

            return true;
        }
    }
}