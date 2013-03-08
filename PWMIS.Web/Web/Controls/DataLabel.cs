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
	/// Summary description for BrainLabel.
	/// </summary>
    [System.Drawing.ToolboxBitmap(typeof(ControlIcon), "DataLable.bmp")]
	public class DataLabel:Label,IDataControl
	{
		public DataLabel()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		#region 外观属性

		/// <summary>
		/// 数据呈现格式
		/// </summary>
		[Category("外观"),Description("数据呈现格式")]
		public string DataFormatString
		{
			get
			{
				if(ViewState["DataFormatString"]!=null)
					return (string)ViewState["DataFormatString"];
				return "";
			}
			set
			{
				ViewState["DataFormatString"]=value;
			}
		}

		#endregion

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

		[Category("Data"),Description("设定对应的数据字段类型")]
		public System.TypeCode SysTypeCode
		{
			get
			{
				if(ViewState["SysTypeCode"]!=null)
					return (System.TypeCode)ViewState["SysTypeCode"];
				return new System.TypeCode ();
			}
			set
			{
				ViewState["SysTypeCode"] = value;
			}
		}

		[Category("Data"),Description("设定与数据库字段对应的数据名")]
		public string LinkProperty
		{
			get
			{
				if(ViewState["LinkProperty"]!=null)
					return (string)ViewState["LinkProperty"];
				return "";
			}
			set
			{
				ViewState["LinkProperty"]=value;
			}
		}

		[Category("Data"),Description("设定与数据库字段对应的数据表名")]
		public string LinkObject
		{
			get
			{
				if(ViewState["LinkObject"]!=null)
					return (string)ViewState["LinkObject"];
				return "";
			}
			set
			{
				ViewState["LinkObject"]=value;
			}
		}

		#endregion

		#region 接口方法

        public void SetValue(object obj)
		{
            //if(value!=null)
            //    if(DataFormatString != "")
            //    {
            //        this.Text=String.Format(DataFormatString,value);
            //    }
            //    else
            //    {
            //        this.Text=value.ToString ();
            //    }
			    
            //else
            //    this.Text="";
            if (obj == null || obj.ToString() == "")
            {
                this.Text = "";
                return;
            }

            // 邓太华 2006.8.11 添加单精度型和默认类型的实现
            switch (this.SysTypeCode)
            {
                case TypeCode.String:
                    if (obj != DBNull.Value)
                    {
                        if (DataFormatString != "")
                            this.Text = String.Format(DataFormatString, obj.ToString());
                        else
                            this.Text = obj.ToString().Trim();
                    }
                    else
                    {
                        this.Text = "";
                    }
                    break;
                case TypeCode.Int32:
                    if (obj != DBNull.Value && obj.GetType() == typeof(int))
                    {
                        this.Text = obj.ToString().Trim();
                    }
                    else
                    {
                        this.Text = "";
                    }
                    break;
                case TypeCode.Decimal:
                    if (obj != DBNull.Value && obj.GetType() == typeof(decimal))
                    {
                        if (DataFormatString != "")
                            this.Text = String.Format(DataFormatString, obj);
                        else
                            this.Text = obj.ToString().Trim();
                    }
                    else
                    {
                        this.Text = "";
                    }
                    break;
                case TypeCode.DateTime:
                    if (obj != DBNull.Value && obj.GetType() == typeof(DateTime))
                    {
                        if (DataFormatString != "")
                        {
                            this.Text = String.Format(DataFormatString, obj);
                        }
                        else
                        {
                            //this.Text=((DateTime)obj).ToShortDateString().Trim();
                            //没有格式化信息，保留原有数据格式 dth,2008.4.4
                            this.Text = ((DateTime)obj).ToString();
                        }
                    }
                    else
                    {
                        this.Text = "";
                    }
                    break;
                case TypeCode.Double:
                case TypeCode.Single:
                    if (obj != DBNull.Value && (obj.GetType() == typeof(double) || obj.GetType() == typeof(float)))
                    {
                        if (DataFormatString != "")
                            this.Text = String.Format(DataFormatString, obj);
                        else
                            this.Text = obj.ToString().Trim();
                    }
                    else
                    {
                        this.Text = "";
                    }
                    break;
                default:
                    this.Text = obj.ToString().Trim();
                    break;
            }

		}

		public object GetValue()
		{

            switch (this.SysTypeCode)
            {
                case TypeCode.String:
                    {
                        return this.Text.Trim();
                    }
                case TypeCode.Int32:
                    {
                        if (this.Text.Trim() != "")
                        {
                            return Convert.ToInt32(this.Text.Trim());
                        }
                        //return 0;
                        return DBNull.Value;
                    }
                case TypeCode.Decimal:
                    {
                        if (this.Text.Trim() != "")
                        {
                            return Convert.ToDecimal(this.Text.Trim());
                        }
                        //return 0;
                        return DBNull.Value;
                    }
                case TypeCode.DateTime:
                    if (this.Text.Trim() != "")
                    {
                        try
                        {
                            return Convert.ToDateTime(this.Text.Trim());
                        }
                        catch
                        {
                            return DBNull.Value; //"1900-1-1";
                        }
                    }
                    return DBNull.Value;//"1900-1-1";

                case TypeCode.Double:
                    {
                        if (this.Text.Trim() != "")
                        {
                            return Convert.ToDouble(this.Text.Trim());
                        }
                        //return 0;
                        return DBNull.Value;
                    }
                case TypeCode.Boolean:
                    {
                        if (this.Text.Trim() != "")
                        {
                            try
                            {
                                return Convert.ToBoolean(this.Text.Trim());
                            }
                            catch
                            {
                                return DBNull.Value; //"1900-1-1";
                            }
                        }
                        return DBNull.Value;//"1900-1-1";
                    }
                default:
                    if (this.Text.Trim() == "")
                    {
                        return DBNull.Value;
                    }
                    else
                    {
                        return this.Text.Trim();
                    }
            }
		}

		public virtual bool Validate()
		{

			return true;
		}

		#endregion

		#region 默认属性

		public bool isClientValidation
		{
			get
			{
				
				return false;
			}
		}

		public bool isNull
		{

			get
			{
				return true;
			}
			set
			{

			}
		}

		public bool IsValid
		{
			get
			{
				return Validate();
			}
		}

		public bool ReadOnly
		{
			get
			{
				return true;
			}
			set
			{
			}
		}
		#endregion

		#endregion
	}
}
