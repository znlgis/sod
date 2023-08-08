﻿/*
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
using System.Drawing;
using System.Drawing.Design;
using System.Web.UI;
using System.Web.UI.WebControls;
using PWMIS.Common;

namespace PWMIS.Web.Controls
{
    /// <summary>
    ///     Calendar 的摘要说明。2008.7.26
    /// </summary>
    [ToolboxBitmap(typeof(ControlIcon), "DataCalendar.bmp")]
    [ToolboxData("<{0}:DataCalendar runat=server></{0}:DataCalendar>")]
    public class DataCalendar : WebControl, IDataControl, IQueryControl, INamingContainer
    {
        private TextBox objDateBox;

//		private RegularExpressionValidator REV;

        /// <summary>
        ///     默认构造函数
        /// </summary>
        public DataCalendar()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        #region 重写方法

        /// <summary>
        ///     将此控件呈现给指定的输出参数。
        /// </summary>
        /// <param name="output"> 要写出到的 HTML 编写器 </param>
        /// onclick="selectDate('txtEndDate')" alt="单击选择日期" src="in4_05.gif" align="absBottom"
        protected override void Render(HtmlTextWriter writer)
        {
            EnsureChildControls();
            objDateBox.RenderControl(writer);

            writer.AddAttribute(HtmlTextWriterAttribute.Align, "absBottom");
            writer.AddAttribute(HtmlTextWriterAttribute.Src, ScriptPath + "in4_05.gif");
            writer.AddAttribute("alt", "单击选择日期");
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "WdatePicker({el:" + objDateBox.ClientID + "})");
            //writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "WdatePicker()");
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
        }

        /// <summary>
        ///     控件的子控件集合
        /// </summary>
        public override ControlCollection Controls
        {
            get
            {
                EnsureChildControls();
                return base.Controls;
            }
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();

            objDateBox = new TextBox();

            objDateBox.ID = base.ID + "_DateBox";
            //默认文本框只读，在动态加载子控件的时候，无法获得选择的值，所以默认改为非只读
            //
            //objDateBox.ReadOnly=true;
            //objDateBox.Text = " ";

            objDateBox.EnableViewState = true;
            //文本输入区增加日期格式验证 邓太华 2008.2.20
            //objDateBox.Attributes.Add ("onblur","if(this.value!='')TestDate(this);");
            Controls.Add(objDateBox);
//			RegularExpressionValidator REV = new RegularExpressionValidator();
//			REV.ErrorMessage = "错了！";
//			REV.ControlToValidate = objDateBox.ID;
//			REV.ValidationExpression = @"^[_a-z0-9]+@([_a-z0-9]+\.)+[a-z0-9]{2,3}$";
//			Controls.Add(REV);
        }

        /*
        protected virtual void RegisterClientScript()
        {
            string versionInfo = System.Reflection.Assembly.GetAssembly(this.GetType()).FullName;
            int start = versionInfo.IndexOf("Version=")+8;
            int end = versionInfo.IndexOf(",",start);
            versionInfo = versionInfo.Substring(start,end-start);
            string info = @"
<!--
 ********************************************
 * Calendar " + versionInfo + @"
 * by myj,dth
 ********************************************
-->";
            //无需注册多个脚本
            //Page.RegisterClientScriptBlock(base.ID + "_Info",info);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(),versionInfo + "_Info",info);

            string selDate = @"
<script language=""JavaScript"">
function TestDate(obj)
{
    //控件只读不进行验证
    if(obj.readOnly) return true;
    var ex=/(([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})-(((0[13578]|1[02])-(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)-(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8]))))|((([0-9]{2})(0[48]|[2468][048]|[13579][26])|((0[48]|[2468][048]|[3579][26])00))-02-29)/;
    var r=ex.test(obj.value);
    if(r)
    {
        var sd=obj.value.split(""-"")[2];
        if(sd.length!=2)
            r=false;
        else
            if(sd>""31"")
                r=false;

    }
    if(!r) {
        alert(""日期或者日期格式无效（正确格式：YYYY-MM-DD）"");
        obj.focus();
        obj.select();
    }
    return r;
}


    function selectDate(objname)
{
//move window to screen center,by dth,2006.5.23
var height=265;
var width=310;
var top= (screen.height/2 - height/2) ;
var left= (screen.width/2 - width/2);
//邓太华 2008.4.26 改为模式窗体应对现在的标签浏览方式
var objwin=window.showModalDialog('"+this.ScriptPath+@"calendar.htm',window,'dialogHeight='+height+'px;dialogWidth='+width+'px;status=no;toolbar=no;menubar=no;location=no;');
//alert(objwin);
//alert(objname);
document.getElementById(objname).value=objwin;
}
</script>
";

            //无需注册多个脚本
            //Page.RegisterClientScriptBlock(base.ID + "_Info",selDate);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType (),versionInfo + "_script",selDate);
        }
            *   * */

        protected override void OnPreRender(EventArgs e)
        {
            //this.RegisterClientScript();
            //string path = string.IsNullOrEmpty(ScriptPath) ? "System/JS/My97DatePicker/WdatePicker.js" : ScriptPath;
            var path = "/System/JS/My97DatePicker/WdatePicker.js";
            Page.ClientScript.RegisterStartupScript(GetType(), "JS_calendar",
                "\r\n<script language='javascript' type='text/javascript' src='" + path + "'></script>\r\n");
            base.OnPreRender(e);
        }


        //
//		[Category("Data"),Description("设定对应的数据字段类型")]
//		public System.TypeCode SysTypeCode
//		{
//			get
//			{
//				return TypeCode.DateTime;
//			}
//		}

        #endregion

        #region IBrainControl 成员

        [Category("Data")]
        [Description("设定对应的数据源，格式：FullClassName,AssemblyName 。如果需要绑定实体类，可以设置该属性。")]
        public string DataProvider { get; set; }

        [Description("是否只允许数据为空")]
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool IsNull
        {
            get =>
                // TODO:  添加 Calendar.isNull getter 实现
                true;
            set
            {
                // TODO:  添加 Calendar.isNull setter 实现
            }
        }

        #region 数据属性

        [Category("Data")]
        [Description("设定对应的数据库字段是否是主键，用于自动数据查询和更新的依据")]
        public bool PrimaryKey
        {
            get
            {
                if (ViewState["PrimaryKey"] != null)
                    return (bool)ViewState["PrimaryKey"];
                return false;
            }
            set => ViewState["PrimaryKey"] = value;
        }

        /// <summary>
        ///     设定与数据库字段对应的数据名
        /// </summary>
        [Category("Data")]
        [Description("设定与数据库字段对应的数据名")]
        [Editor(typeof(PropertyUITypeEditor), typeof(UITypeEditor))]
        public string LinkProperty
        {
            get
            {
                // TODO:  添加 BrainTextBox.LinkProperty getter 实现
                if (ViewState["LinkProperty"] != null)
                    return (string)ViewState["LinkProperty"];
                return "";
            }
            set => ViewState["LinkProperty"] = value;
            // TODO:  添加 BrainTextBox.LinkProperty setter 实现
        }

        /// <summary>
        ///     设定与数据库字段对应的数据表名
        /// </summary>
        [Category("Data")]
        [Description("设定与数据库字段对应的数据表名")]
        public string LinkObject
        {
            get
            {
                if (ViewState["LinkObject"] != null)
                    return (string)ViewState["LinkObject"];
                return "";
            }
            set => ViewState["LinkObject"] = value;
        }

        #endregion

        [Description("是否只允许在客户端选择日期值")]
        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool ReadOnly
        {
            get
            {
                // TODO:  添加 Calendar.ReadOnly getter 实现
                EnsureChildControls();
                return objDateBox.ReadOnly;
            }
            set
            {
                EnsureChildControls();
                objDateBox.ReadOnly = value;
            }
        }

        /// <summary>
        ///     服务端验证方法
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            // TODO:  添加 Calendar.Validate 实现
            return true;
        }

        /// <summary>
        ///     获取值
        /// </summary>
        /// <returns></returns>
        public object GetValue()
        {
            if (Text != "")
                try
                {
                    return Convert.ToDateTime(Text);
                }
                catch
                {
                    return DBNull.Value;
                }

            return DBNull.Value;
        }

        public void SetValue(object obj)
        {
            if (obj != DBNull.Value)
                //邓太华 2006.7.26 修改	日期格式转换
                try
                {
                    if (DataFormatString != "")
                        Text = string.Format(DataFormatString, Convert.ToDateTime(obj));
                    else
                        Text = ((DateTime)obj).ToString();
                }
                catch
                {
                    Text = "";
                }
            else
                Text = "";
        }

        public string ClientValidationFunctionString
        {
            get =>
                // TODO:  添加 Calendar.ClientValidationFunctionString getter 实现
                null;
            set
            {
                // TODO:  添加 Calendar.ClientValidationFunctionString setter 实现
            }
        }

        public bool isClientValidation
        {
            get =>
                // TODO:  添加 Calendar.isClientValidation getter 实现
                false;
            set
            {
                // TODO:  添加 Calendar.isClientValidation setter 实现
            }
        }

        public bool IsValid =>
            // TODO:  添加 Calendar.IsValid getter 实现
            true;

        [Category("Data")]
        [Description("设定数据类型代码")]
        public TypeCode SysTypeCode
        {
            get =>
                // TODO:  添加 Calendar.Weltop.ServerControls.IBrainControl.SysTypeCode getter 实现
                TypeCode.DateTime;
            set
            {
                // TODO:  添加 Calendar.Weltop.ServerControls.IBrainControl.SysTypeCode setter 实现
            }
        }

        #endregion

        #region 公共属性

        /// <summary>
        ///     数据呈现格式
        /// </summary>
        [Category("外观")]
        [Description("日期文本数据呈现格式")]
        [DefaultValue("{0:yyyy-MM-dd}")]
        public string DataFormatString
        {
            get
            {
                if (ViewState["DataFormatString"] != null)
                    return (string)ViewState["DataFormatString"];
                return "{0:yyyy-MM-dd}";
            }
            set => ViewState["DataFormatString"] = value.Trim();
        }

        public string Text
        {
            get
            {
                EnsureChildControls();
                if (!string.IsNullOrEmpty(objDateBox.Text))
                    return objDateBox.Text;
                if (ViewState["Date_Text"] != null)
                    return ViewState["Date_Text"].ToString();
                return "";
            }
            set
            {
                EnsureChildControls();
                //this.objDateBox.Text=value;
                if (DataFormatString != "" && value != null && value != "")
                    objDateBox.Text = string.Format(DataFormatString, Convert.ToDateTime(value));
                else
                    objDateBox.Text = value; //Convert.ToDateTime (value).ToString ("yyyy-MM-dd").Trim();

                ViewState["Date_Text"] = objDateBox.Text;
            }
        }

        public string ScriptPath
        {
            get
            {
                if (ViewState["ScriptPath"] != null)
                    return ViewState["ScriptPath"].ToString();
                return "";
            }
            set
            {
                if (value != "")
                    if (value.Trim().Substring(value.Length - 1, 1) != "/")
                        value += "/";
                ViewState["ScriptPath"] = value;
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
            set => ViewState["CompareSymbol"] = value;
        }

        public string QueryFormatString
        {
            get
            {
                if (ViewState["QueryFormatString"] != null)
                    return (string)ViewState["QueryFormatString"];
                return "";
            }
            set => ViewState["QueryFormatString"] = value;
        }

        #endregion
    }
}