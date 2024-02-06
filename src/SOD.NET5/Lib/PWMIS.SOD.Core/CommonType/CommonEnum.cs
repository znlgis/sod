﻿/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap 
 * ========================================================================
 * 该类的作用
 * 
 * 作者：邓太华     时间：2008-10-12
 * 版本：V3.0
 * 
 * 修改者：         时间：                
 * 修改说明：
 * ========================================================================
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace PWMIS.Common
{
    /// <summary>
    /// 数据库管理系统枚举
    /// </summary>
    public enum DBMSType
    {
        Access,
        SqlServer,
        SqlServerCe,
        Oracle,
        DB2,
        Sysbase,
        MySql,
        SQLite,
        PostgreSQL,
        Dameng,
        UNKNOWN=999
    }

    //下面的委托定义，用于.NET 2.0 没有的，替代.NET 3.5之后的委托方法

    /// <summary>
    /// 返回一个结果类型的泛型委托函数
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public delegate TResult MyFunc<TResult>();

    public delegate TResult MyFunc<T, TResult>(T arg);

    public delegate TResult MyFunc<T1,T2, TResult>(T1 arg1,T2 arg2);

    public delegate void MyAction();

    public delegate void MyAction<T1, T2>(T1 arg1, T2 arg2);

    public delegate void MyAction<T1, T2,T3>(T1 arg1, T2 arg2,T3 arg3);

    /// <summary>
    /// 命令执行的查询类型
    /// </summary>
    public enum CommandExecuteType
    {
        /// <summary>
        /// 执行没有结果集的查询，例如写查询
        /// </summary>
        ExecuteNonQuery,
        /// <summary>
        /// 执行有返回值的查询
        /// </summary>
        ExecuteQuery,
        /// <summary>
        /// 不限制的查询类型
        /// </summary>
        Any
    }

    /// <summary>
    /// SQL操作类型
    /// </summary>
    public enum SQLOperatType
    {
        /// <summary>
        /// 增加操作，值为1
        /// </summary>
        Insert=1,
        /// <summary>
        /// 删除操作，值为2
        /// </summary>
        Delete=2,
        /// <summary>
        /// 删除操作,值为4
        /// </summary>
        Update=4,
        /// <summary>
        /// 查询数据，值为8
        /// </summary>
        Select=8
    }
}
