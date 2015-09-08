/*
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
        UNKNOWN=999
    }

    /// <summary>
    /// 数据源类型
    /// </summary>
    public enum DataSourceType
    { 
        OleDb,
        SqlServer,
        SqlServerCe,
        SQLite,
        Oracle,
        Odbc,
        TextFile,
        XML
    }

    public delegate TResult MyFunc<T, TResult>(T arg);

    public delegate TResult MyFunc<T1,T2, TResult>(T1 arg1,T2 arg2);

    public delegate void MyAction<T1, T2>(T1 arg1, T2 arg2);

    public delegate void MyAction<T1, T2,T3>(T1 arg1, T2 arg2,T3 arg3);
}
