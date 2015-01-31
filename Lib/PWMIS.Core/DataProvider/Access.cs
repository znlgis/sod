/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap 
 * ========================================================================
 * 该类的作用
 * 
 * 作者：邓太华     时间：2008-10-12
 * 版本：V4.5
 * 
 * 修改者：         时间：2013-2- 24               
 * 修改说明：解决Access 某些版本在更新DateTime类型的时候，参数化查询出现 “准表达式中数据类型不匹配”的问题。
 * 
 *  * 修改者：         时间：2013-3- 19               
 * 修改说明：解决Access 生成建表脚本的问题 
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
            get { return PWMIS.Common.DBMSType.Access; }
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
                    return "SELECT @@IDENTITY";
                else
                    return _insertKey;
            }
            set
            {
                _insertKey = value;
            }
        }

        /// <summary>
        /// 获取查询参数对象，如果参数值是日期类型，请调用此方法，否则调用其它重载方法后，给日期类型的参数值去除毫秒部分。
        /// </summary>
        /// <param name="paraName"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public override IDataParameter GetParameter(string paraName, object Value)
        {
            IDataParameter para = base.GetParameter(paraName, Value);
            if (Value is DateTime)
            {
                ((OleDbParameter)para).OleDbType = OleDbType.DBTimeStamp;//时间带毫秒将失败
                DateTime dt = (DateTime)Value;
                DateTime objDt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
                para.Value = objDt;
            }
            return para;
        }

        public override IDataParameter GetParameter(string paraName, DbType dbType)
        {
            IDataParameter para = this.GetParameter();
            para.ParameterName = paraName;
            para.DbType = dbType;
            if (dbType == DbType.DateTime)
                ((OleDbParameter)para).OleDbType = OleDbType.DBDate;
            return para;
        }

        public override IDataParameter GetParameter(string paraName, System.Data.DbType dbType, int size)
        {
            OleDbParameter para = new OleDbParameter();
            para.ParameterName = paraName;
            para.DbType = dbType;
            para.Size = size;
            if (dbType == DbType.DateTime)
                ((OleDbParameter)para).OleDbType = OleDbType.DBDate;
            return para;
        }

        public override string GetNativeDbTypeName(IDataParameter para)
        {
            OleDbType type = ((OleDbParameter)para).OleDbType;
            if (type == OleDbType.VarWChar)
                type = OleDbType.VarChar;
            else if (type == OleDbType.DBDate)
                type = OleDbType.Date;
            else if (type == OleDbType.DBTimeStamp) //2014.5.14 增加，感谢网友 广州-晓伟 发现此问题
                type = OleDbType.Date;
            return type.ToString();
        }
    }

}
