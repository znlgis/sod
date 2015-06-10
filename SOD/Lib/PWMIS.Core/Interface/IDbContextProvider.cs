using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PWMIS.Core.Interface
{
    /// <summary>
    /// 数据库上下文提供程序接口
    /// </summary>
    public interface IDbContextProvider
    {
        /// <summary>
        /// 当前的数据库访问对象
        /// </summary>
        AdoHelper CurrentDataBase { get; }
        /// <summary>
        /// 检查实体类对应的数据表是否在数据库中存在，需要在子类中实现
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void CheckTableExists<T>() where T : EntityBase, new();
    }
}
