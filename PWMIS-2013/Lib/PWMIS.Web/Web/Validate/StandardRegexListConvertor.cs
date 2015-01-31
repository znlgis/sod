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
using System.ComponentModel;
using System.Collections;

namespace PWMIS.Web.Validate
{
    /// <summary>
    /// StandardRegexListConvertor 的摘要说明。
    /// </summary>
    public class StandardRegexListConvertor : TypeConverter
    {
        public StandardRegexListConvertor()
        {

        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            Hashtable ht = RegexStatic.GetGenerateRegex();
            ht.Add("无", "");
            return new StandardValuesCollection(ht.Keys);
        }
    }
}
