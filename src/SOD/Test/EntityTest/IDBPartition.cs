using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityTest
{
    /// <summary>
    /// 数据库分库接口
    /// </summary>
    interface IDBPartition
    {
        /// <summary>
        /// 获取当前分库的数据库名字
        /// </summary>
        /// <returns></returns>
        string GetDatabaseName();
        /// <summary>
        /// 获取当前分库的数据库服务器名字
        /// </summary>
        /// <returns></returns>
        string GetServerName();
    }
}
