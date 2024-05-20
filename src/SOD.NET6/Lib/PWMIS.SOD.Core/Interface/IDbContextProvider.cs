﻿using PWMIS.DataMap.Entity;
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
        /// 检查数据库，检查表是否已经初始化。如果是Access 数据库，还会检查数据库文件是否存在，可以在系统中设置DBFilePath 字段。
        /// 如果需要更多的检查，可以重写该方法，但一定请保留 base.CheckDB();这行代码。
        /// </summary>
        /// <returns>检查是否通过</returns>
        bool CheckDB();
        /// <summary>
        /// 检查实体类对应的数据表是否在数据库中存在，需要在子类中实现
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">检查和创建表所依据的实体类，可以通过这种方式来指定不同的表名称</param>
        /// <returns>返回检查的时候，表是否存在</returns>
        bool CheckTableExists<T>(T entity=null) where T : EntityBase, new();
        /// <summary>
        /// 检查实体类对应的表是否存在，如果不存在则创建表并执行可选的SQL语句，比如为表增加索引等。
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="entity">检查和创建表所依据的实体类，可以通过这种方式来指定不同的表名称</param>
        /// <param name="initSql">要初始化执行的SQL语句</param>
        void InitializeTable<T>(string initSql, T entity = null) where T : EntityBase, new();

    }
}
