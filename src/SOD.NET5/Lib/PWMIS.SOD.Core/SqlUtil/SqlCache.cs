using System.Collections.Generic;
using System.Data;
using PWMIS.DataMap.Entity;

namespace PWMIS.Common
{
    /// <summary>
    ///     (与具体数据库无关的)SQL查询命令信息
    /// </summary>
    public class SqlInfo
    {
        private static Dictionary<string, SqlInfo> _dictSqlCache;
        private static readonly object sync_obj = new();

        public SqlInfo(string sql)
        {
            SQL = sql;
        }

        public SqlInfo(string sql, Dictionary<string, TableNameField> para)
        {
            SQL = sql;
            Parameters = para;
        }

        public string SQL { get; private set; }
        public Dictionary<string, TableNameField> Parameters { get; private set; }
        public CommandType CommandType { get; set; }
        public string TableName { get; set; }

        private static Dictionary<string, SqlInfo> DictSqlCache
        {
            get
            {
                if (_dictSqlCache == null)
                    lock (sync_obj)
                    {
                        if (_dictSqlCache == null)
                        {
                            var temp = new Dictionary<string, SqlInfo>();
                            _dictSqlCache = temp;
                        }
                    }

                return _dictSqlCache;
            }
        }

        /// <summary>
        ///     从缓存中获取项
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static SqlInfo GetFromCache(string key)
        {
            SqlInfo Value = null;
            DictSqlCache.TryGetValue(key, out Value);
            return Value;
        }

        /// <summary>
        ///     增加一项到缓存中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool AddToCache(string key, SqlInfo item)
        {
            var Value = GetFromCache(key);
            if (Value != null)
                return false;
            lock (sync_obj)
            {
                DictSqlCache.Add(key, item);
            }

            return true;
        }
    }
}