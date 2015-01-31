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
using System.Data;

namespace PWMIS.Common
{
	/// <summary>
	/// 数据映射控件接口
	/// </summary>
	public interface IDataControl
	{
		
		/// <summary>
		/// 与数据库数据项相关联的数据
		/// </summary>
		string LinkProperty
		{
			get;
			set;
		}
		
		/// <summary>
		/// 与数据关联的表名
		/// </summary>
		string LinkObject
		{
			get;
			set;
		}

		/// <summary>
		/// 是否通过服务器验证默认为true
		/// </summary>
		bool IsValid
		{
			get;
		}

//		/// <summary>
//		/// 数据类型
//		/// </summary>
//		DbType DataType
//		{
//			get;
//			set;
//		}

		/// <summary>
		/// 数据类型
		/// </summary>
		TypeCode SysTypeCode
		{
			get;
			set;
		}

		/// <summary>
		/// 只读标记
		/// </summary>
		bool ReadOnly
		{
			get;
			set;
		}

		/// <summary>
		/// 是否客户端验证
		/// </summary>
//		bool isClientValidation
//		{
//			get;
//			set;
//		}

		/// <summary>
		/// 是否允许空值
		/// </summary>
		bool IsNull
		{
			get;
//			set;
		}

		/// <summary>
		/// 是否是主键
		/// </summary>
		bool PrimaryKey
		{
			get;
			set;
        }

//		object 

//		/// <summary>
//		/// 客户端验证脚本
//		/// </summary>
//		string ClientValidationFunctionString
//		{
//			get;
//			set;
//		}

		/// <summary>
		/// 设置值
		/// </summary>
		/// <param name="obj"></param>
		void SetValue(object value);

		/// <summary>
		/// 获取值
		/// </summary>
		/// <returns></returns>
		object GetValue();

		/// <summary>
		/// 服务端验证
		/// </summary>
		/// <returns></returns>
		bool Validate();
        ///// <summary>
        ///// 设定对应的数据源，格式：FullClassName,AssemblyName 。如果需要绑定实体类，可以设置该属性。
        ///// </summary>
        //string DataProvider { get; set; }

	}

    public interface IDataTextBox : IDataControl
    {
        string Text { get; set; }
        string DataFormatString { get; set; }
        int MaxLength { get; set; }
    }

    public interface IDataCheckBox : IDataControl
    {
        string Value { get; set; }
        bool Checked { get; set; }
        string Text { get; set; }
        event EventHandler CheckedChanged;
    }

    
}
