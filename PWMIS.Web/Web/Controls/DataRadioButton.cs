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
 * 修改者：         时间：2013-3-1                
 * 修改说明：完善了控件
 * ========================================================================
*/
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using PWMIS.Common;


namespace PWMIS.Web.Controls
{
	/// <summary>
    /// 数据复选框控件
	/// </summary>
    [System.Drawing.ToolboxBitmap(typeof(ControlIcon), "DataRadioButton.bmp")]
    public class DataRadioButton : RadioButton, IDataControl, IQueryControl
	{
		public DataRadioButton()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		#region IBrainControl 成员

		#region 数据属性
		[Category("Data"),Description("设定对应的数据库字段是否是主键，用于自动数据查询和更新的依据")]
		public bool PrimaryKey
		{
			get
			{
				if(ViewState["PrimaryKey"]!=null)
					return (bool)ViewState["PrimaryKey"];
				return false;
			}
			set
			{
				ViewState["PrimaryKey"]=value;
			}
		}

		[Category("外观"),Description("设定与数据库字段对应的数据值")]
		public string Value
		{
			get
			{
				if(ViewState["CheckBoxvalue"]!=null)
					return (string)ViewState["CheckBoxvalue"];
				return "";
			}
			set
			{
				ViewState["CheckBoxvalue"]=value;
			}
		}

		[Category("Data"),Description("设定与数据库字段对应的数据名")]
		public string LinkProperty
		{
			get
			{
				// TODO:  添加 BrainDropDownList.LinkProperty getter 实现
				if(ViewState["LinkProperty"]!=null)
					return (string)ViewState["LinkProperty"];
				return "";
			}
			set
			{
				ViewState["LinkProperty"]=value;
				// TODO:  添加 BrainDropDownList.LinkProperty setter 实现
			}
		}

		[Category("Data"),Description("设定与数据表对应的数据表名")]
		public string LinkObject
		{
			get
			{
				if(ViewState["LinkObject"]!=null)
					return (string)ViewState["LinkObject"];
				return "";
				// TODO:  添加 BrainDropDownList.LinkObject getter 实现
			}
			set
			{
				ViewState["LinkObject"]=value;
				// TODO:  添加 BrainDropDownList.LinkObject setter 实现
			}
		}

		#endregion

		#region 默认属性

		public bool IsValid
		{
			get
			{
				return true;
			}
		}
		public System.TypeCode SysTypeCode
		{
			get
			{
				if(ViewState["SysTypeCode"]!=null)
					return (TypeCode)ViewState["SysTypeCode"];
				return System.TypeCode.String;
			}
			set
			{
				ViewState["SysTypeCode"]=value;
			}
		}

		public bool ReadOnly
		{
			get
			{
                if (this.Checked)
                    return false ;
				return !base.Enabled;
			}
			set
			{
				base.Enabled=!value;
			}
		}

		#endregion

		#region 借口方法

		public bool isNull
		{

			get
			{
				if(ViewState["isNull"]!=null)
					return (bool)ViewState["isNull"];
				return true;
			}
			set
			{
				ViewState["isNull"] = value;
			}
		}

		public void SetValue(object obj)
		{
			this.Checked = false;
			if(obj!=null)
			{
				string SelItemValues = obj.ToString().Trim();
                string strValue = this.Value.Trim();
                if (strValue == SelItemValues.Trim())
				{
					this.Checked=true;
                    return;
				}
                //布尔值特殊处理，数据库中可能存储的值为0或者1
                if (this.SysTypeCode == TypeCode.Boolean)
                {
                    if (strValue.ToLower() == "true" && SelItemValues == "1")
                        this.Checked = true;
                    else if (strValue.ToLower() == "false" && SelItemValues == "0")
                        this.Checked = true;
                }

			}

		}

		public object GetValue()
		{
            return this.Checked ? GetValueInner() : DBNull.Value;
		}

        private object GetValueInner()
        {
            switch (this.SysTypeCode)
            {
                case TypeCode.String:
                    {
                        return this.Value.Trim();
                    }
                case TypeCode.Int32:
                    {
                        if (this.Value.Trim() != "")
                        {
                            return Convert.ToInt32(this.Value.Trim());
                        }
                        //return 0;
                        return DBNull.Value;
                    }
                case TypeCode.Decimal:
                    {
                        if (this.Value.Trim() != "")
                        {
                            return Convert.ToDecimal(this.Value.Trim());
                        }
                        //return 0;
                        return DBNull.Value;
                    }
                case TypeCode.DateTime:
                    if (this.Value.Trim() != "")
                    {
                        try
                        {
                            return Convert.ToDateTime(this.Value.Trim());
                        }
                        catch
                        {
                            return DBNull.Value; //"1900-1-1";
                        }
                    }
                    return DBNull.Value;//"1900-1-1";

                case TypeCode.Double:
                    {
                        if (this.Value.Trim() != "")
                        {
                            return Convert.ToDouble(this.Value.Trim());
                        }
                        //return 0;
                        return DBNull.Value;
                    }
                case TypeCode.Boolean:
                    {
                        if (this.Value.Trim() != "")
                        {
                            try
                            {
                                return Convert.ToBoolean(this.Value.Trim());
                            }
                            catch
                            {
                                return DBNull.Value; //"1900-1-1";
                            }
                        }
                        return DBNull.Value;//"1900-1-1";
                    }
                default:
                    if (this.Value.Trim() == "")
                    {
                        return DBNull.Value;
                    }
                    else
                    {
                        return this.Value.Trim();
                    }
            }
        }

		public virtual bool Validate()
		{
			return true;
		}

		#endregion

        #region IQueryControl 成员

        public string CompareSymbol
        {
            get
            {
                if (ViewState["CompareSymbol"] != null)
                    return (string)ViewState["CompareSymbol"];
                return "";
            }
            set
            {
                ViewState["CompareSymbol"] = value;
            }
        }

        public string QueryFormatString
        {
            get
            {
                if (ViewState["QueryFormatString"] != null)
                    return (string)ViewState["QueryFormatString"];
                return "";
            }
            set
            {
                ViewState["QueryFormatString"] = value;
            }
        }

        #endregion


		#endregion
	}
}
