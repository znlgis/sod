
#region << 版 本 注 释 >>
/*
 * ========================================================================
 * Copyright(c) 2008-2017 拼威&敏思--PWMIS, All Rights Reserved.
 * ========================================================================
 *  
 * 内存数据库，参见 http://www.cnblogs.com/bluedoctor/archive/2011/09/20/2182722.html
 *  
 *  
 * 作者：转自网上     时间：2011/9/2 15:19:30
 * 版本：V2.0.0
 * 
 * 修改者：         时间： 2013.5.15              
 * 修改说明：使用.NET 4.0 线程安全的集合
 * ========================================================================
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PWMIS.MemoryStorage
{
    /// <summary>
    /// 内存数据库引擎，dth 2011.9.5 详细请看 http://www.pwmis.com/sqlmap
    /// </summary>
    public class MemDBEngin
    {
        /// <summary>
        /// 获取引擎实例，实例保存在系统缓存工厂中
        /// </summary>
        /// <param name="source">要持久化的对象数据保存的路径</param>
        /// <returns></returns>
        public static MemDB GetDB(string source)
        {
            MemDB result = CacheProviderFactory.GetCacheProvider().Get<MemDB>(source, () =>
            {
                MemDB db = new MemDB(source);
                db.AutoSaveData();
                return db;
            },
                 new System.Runtime.Caching.CacheItemPolicy()
                 {
                     SlidingExpiration = new TimeSpan(0, 2, 0), //距离上次调用10分钟后过期
                     RemovedCallback = args =>
                     {
                         MemDB db = (MemDB)args.CacheItem.Value;
                         db.Flush();
                         db.Close();
                     }
                 }
                 );

            return result;

        }

        private static string defaultDbSource = "";

        /// <summary>
        /// 数据源
        /// </summary>
        public static string DbSource
        {
            get
            {
                if (defaultDbSource.Length == 0)
                {
                    defaultDbSource = GetDataSource("MemoryDB");
                }
                return defaultDbSource;
            }
        }

        /// <summary>
        /// 获取默认的内存数据库引擎
        /// </summary>
        /// <returns></returns>
        public static MemDB GetDB()
        {
            return GetDB(DbSource);
        }

        /// <summary>
        /// 获取数据源（路径）
        /// </summary>
        /// <param name="connectionName">ConnectionStrings配置节的名称</param>
        /// <returns>如果没有，返回当前应用程序（或网站）目录下面的MemoryDB目录</returns>
        public static string GetDataSource(string connectionName)
        {
            string source = "~\\MemoryDB";
            var config = System.Configuration.ConfigurationManager.ConnectionStrings[connectionName];
            if (config != null)
            {
                string connnString = config.ConnectionString;
                string[] arr = connnString.Split(';');
                string temp = arr.Where(p => p.Split('=')[0].Trim().Equals("data source", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if (temp != null)
                {
                    source = temp.Split('=')[1];
                }
            }

            PWMIS.Core.CommonUtil.ReplaceWebRootPath(ref source);
            return source;
        }
    }
}
