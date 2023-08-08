using System;
using PWMIS.Common;
using PWMIS.Core.Interface;
using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Data;

namespace PWMIS.Core.Extensions
{
    public sealed class OracleDbContext : IDbContextProvider
    {
        /// <summary>
        ///     用连接字符串名字初始化本类
        /// </summary>
        /// <param name="db">数据访问对象</param>
        public OracleDbContext(AdoHelper db)
        {
            if (db.CurrentDBMSType != DBMSType.Oracle)
                throw new Exception("当前数据库类型不是Oracle ");
            CurrentDataBase = db;
        }

        public AdoHelper CurrentDataBase { get; }

        /// <summary>
        ///     检查实体类对应的数据表是否在数据库中存在
        /// </summary>
        public bool CheckTableExists<T>(T entity = null) where T : EntityBase, new()
        {
            //创建表
            if (entity == null) entity = new T();
            var dsScheme = CurrentDataBase.GetSchema("Tables", null);
            var owner = CurrentDataBase.ConnectionUserID;
            var rows = dsScheme.Select("OWNER='" + owner + "' and table_name='" + entity.GetTableName() + "'");
            if (rows.Length == 0)
            {
                var ecmd = new EntityCommand(entity, CurrentDataBase);
                var sql = ecmd.CreateTableCommand;
                //OracleClient 不能批量执行多条SQL语句
                var sqlArr = sql.Split(new[] { ";--" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in sqlArr)
                    if (item.Length > 10) //去除回车行
                        CurrentDataBase.ExecuteNonQuery(item);
                return false;
            }

            return true;
        }

        /// <summary>
        ///     检查实体类对应的表是否存在，如果不存在则创建表并执行可选的SQL语句，比如为表增加索引等。
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="initSql">要初始化执行的SQL语句</param>
        public void InitializeTable<T>(string initSql, T entity = null) where T : EntityBase, new()
        {
            if (!CheckTableExists(entity)) CurrentDataBase.ExecuteNonQuery(initSql);
        }

        public bool CheckDB()
        {
            //带实现详细的创建数据库的过程
            return true;
        }
    }
}