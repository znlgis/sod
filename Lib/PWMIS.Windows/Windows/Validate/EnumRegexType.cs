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


namespace PWMIS.Windows.Validate
{
    /// <summary>
    /// 数据验证方式
    /// </summary>
    public enum EnumRegexType
    {
        /// <summary>
        /// 未定义
        /// </summary>
        无,

        数字,

        日期,

        身份证号,

        电话号码,

        邮政编码,

        IP,

        Email,

        Url,

        中文字符集,

        自定义

    }

    /// <summary>
    /// BrainTextBox控件信息提示方式
    /// </summary>
    public enum EnumMessageType
    {
        /// <summary>
        /// 未定义
        /// </summary>
        层,

        提示框

    }

    /// <summary>
    /// BrainLabel控件数据显示方式
    /// </summary>
    public enum EnumDisplayType
    {
        /// <summary>
        /// 未定义
        /// </summary>
        无,

        日期,

        货币

    }

    /// <summary>
    /// 图片新闻控件数据显示方式
    /// </summary>
    //public enum EnumPicType
    //{
    //    /// <summary>
    //    /// 未定义
    //    /// </summary>
    //    经典,

    //    波浪,

    //    滑动,

    //    奥运,

    //    幻灯,

    //    影院,

    //    缩略,

    //    三维



    //}
}
