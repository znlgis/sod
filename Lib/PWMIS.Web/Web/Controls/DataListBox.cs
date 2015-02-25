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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using PWMIS.Common;


namespace PWMIS.Web.Controls
{
	/// <summary>
	/// 数据列表控件
	/// </summary>
    [System.Drawing.ToolboxBitmap(typeof(ControlIcon), "DataListBox.bmp")]
    public class DataListBox : ListBox, IDataControl, IQueryControl
	{
		public DataListBox()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		#region IBrainControl 成员

		#region 数据属性
        [Category("Data"), Description("设定对应的数据源，格式：FullClassName,AssemblyName 。如果需要绑定实体类，可以设置该属性。")]
        public string DataProvider { get; set; }

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

		[Category("Data"),Description("设定与数据表对应的数据表名")]
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

		#region 默认属性

		public bool ReadOnly
		{
			get
			{
                //未选择，设置为只读数据属性，将不更新数据库。dth,2008.7.27
                if (this.SelectedIndex == -1)
                    return true;
				return ! this.Enabled ;
			}
			set
			{
                this.Enabled = !value;
			}
		}

		#endregion

		#region 接口方法

		public void SetValue(object obj)
		{
            if (obj == null) return;
			foreach(ListItem Item in Items)
			{
				Item.Selected=false;
			}
			string SelItemValues = "";
			//if(obj!=null)
				 SelItemValues = obj.ToString().Trim();
            //string delimStr = ",";
            //char [] delimiter = delimStr.ToCharArray();

			string [] SelItemobj = SelItemValues.Split(',');// SelItemValues.Split(delimiter);
			
			foreach(string s in SelItemobj)
			{
				ListItem item=Items.FindByValue(s);
				item.Selected=true;
			}
		}

		public object GetValue()
		{
			string SelItemValues = "";

			foreach(ListItem Item in Items)
			{
				if(Item.Selected)
				{
					if(SelItemValues == string.Empty)
					{
						SelItemValues+=Item.Value.Trim();
					}
					else
					{
						SelItemValues+=","+Item.Value.Trim();
					}
					
				}

			}
			return SelItemValues;
		}

		public virtual bool Validate()
		{
			if(!IsNull)
			{
				if(this.SelectedValue.Trim()==string.Empty)
				{
					return false;
				}

			}
			return true;
		}

		#endregion

        #region 其他属性

		public bool IsNull
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

		public bool IsValid
		{
			get
			{
				return Validate();
			}
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
