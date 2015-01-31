/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap 
 * ========================================================================
 * 该类的作用
 * 
 * 作者：邓太华     时间：2008-10-12
 * 版本：V4.0
 * 
 * 修改者：         时间：2010-5-25                
 * 修改说明：修改 PropertyList 为方法，增加 PropertyNames 属性
 * ========================================================================
*/
using System;
namespace PWMIS.Common
{
    /// <summary>
    /// 实体类接口
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// 标识字段名称
        /// </summary>
        string GetIdentityName();
        /// <summary>
        /// 获取主键
        /// </summary>
        System.Collections.Generic.List<string> PrimaryKeys { get; }
        /// <summary>
        /// 获取属性列的值
        /// </summary>
        /// <param name="propertyName">属性字段名称</param>
        /// <returns>属性值</returns>
        object PropertyList(string propertyName);
        /// <summary>
        /// 数据表名称
        /// </summary>
        string GetTableName();
        
        /// <summary>
        /// 获取属性字段名数组
        /// </summary>
        string[] PropertyNames { get; }
    }
}
