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


namespace PWMIS.Common
{
    /// <summary>
    /// 数据查询控件接口
    /// </summary>
    public interface IQueryControl
    {
        /// <summary>
        /// 查询的比较符号,例如 =,>=,
        /// </summary>
        string CompareSymbol
        {
            get;
            set;
        }

        /// <summary>
        /// 发送到数据库查询前的字段值格式字符串
        /// </summary>
        string QueryFormatString
        {
            get;
            set;
        }
    }
}
