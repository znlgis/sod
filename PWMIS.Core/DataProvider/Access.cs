/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap 
 * ========================================================================
 * 该类的作用
 * 
 * 作者：邓太华     时间：2008-10-12
 * 版本：V4.3
 * 
 * 修改者：         时间：                
 * 修改说明：
 * ========================================================================
*/
using System;
using System.Data;
using System.Data.OleDb;

namespace PWMIS.DataProvider.Data
{
   /// <summary>
   /// Access 数据库访问类
   /// </summary>
    public sealed class Access : OleDb 
    {

        /// <summary>
        /// 获取当前数据库类型的枚举
        /// </summary>
        public override PWMIS.Common.DBMSType CurrentDBMSType
        {
            get { return PWMIS.Common.DBMSType.Access ; }
        }

        private string _insertKey;
        /// <summary>
        /// 在插入具有自增列的数据后，获取刚才自增列的数据的
        /// </summary>
        public override string InsertKey
        {
            get
            {
                if (string.IsNullOrEmpty(_insertKey))
                    return "@@IDENTITY";
                else
                    return _insertKey;
            }
            set
            {
                _insertKey = value;
            }
        }
    }
    
}
