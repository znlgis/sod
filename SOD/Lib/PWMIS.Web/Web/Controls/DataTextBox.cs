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

//**************************************************************************
//	文 件 名：  
//	用	  途：  TextBox扩展，收集数据
//	创 建 人：  马彦杰
//  创建日期：  2006.03.09
//	版 本 号：	V1.1
//	修改记录：  邓太华 2006.04.25 添加对于字符串超长的验证
//              邓太华 20060608 修改，对于只读状态下采用样式定义而不采用背景区分，
//              规定所有的只读样式名为： CssReadOnly
//              取消样式控制功能，由用户样式表定义；
//              邓太华 2008.2.15 增加“主键”属性，用于自动数据更新的依据
//                     2009.12.29 增加验证功能
//**************************************************************************
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using PWMIS.Common;
using PWMIS.Web.Validate;
using PWMIS.DataMap;
using System.Drawing.Design;


namespace PWMIS.Web.Controls
{
    /// <summary>
    /// 数据文本框控件.
    /// </summary>
    [System.Drawing.ToolboxBitmap(typeof(ControlIcon), "DataTextBox.bmp")]
    public class DataTextBox : TextBox, IDataTextBox, IQueryControl
    {
        //private string _BaseText=null ;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public DataTextBox()
        {
            EnsureChildControls();
        }

        #region 控件验证
        /// <summary>
        /// 执行服务器验证时的数据类型
        /// </summary>
        [Category("控件验证"), Description("执行服务器验证时的数据类型")]
        public ValidationDataType Type
        {
            get
            {
                if (ViewState["ValidationDataType"] != null)
                    return (ValidationDataType)ViewState["ValidationDataType"];
                return ValidationDataType.String;
            }
            set
            {
                ViewState["ValidationDataType"] = value;
                //取消样式控制功能，用用户样式表定义
                //				switch(value)
                //				{
                //					case ValidationDataType.Currency:
                //					case ValidationDataType.Double:
                //					case ValidationDataType.Integer:
                //						this.Style.Add("TEXT-ALIGN","right");
                //						break;
                //					default:
                //						this.Style.Remove("TEXT-ALIGN");
                //						break;
                //				}
            }
        }

        /// <summary>
        /// 是否通过服务器验证默认为true
        /// </summary>
        [Category("控件验证"), Description("是否通过服务器验证默认为true")]
        public bool IsValid
        {
            get
            {
                if (!isClientValidation)
                {
                    return Validate();
                }
                else
                    return true;
            }
        }


        private EnumMessageType _messageType;

        /// <summary>
        /// 提示信息的类型
        /// </summary>
        [Category("控件验证"), Description("提示信息的类型")]
        [TypeConverter(typeof(EnumConverter))]
        public EnumMessageType MessageType
        {
            get
            {
                return _messageType;
            }
            set
            {
                _messageType = value;
            }
        }

        /// <summary>
        /// 需要验证的常用数据类型，如果设定为“无”，将停止控件验证。
        /// </summary>
        [Category("控件验证"), Description("需要验证的常用数据类型，如果设定为“无”，将停止控件验证。")]
        [TypeConverter(typeof(StandardRegexListConvertor))]
        public string OftenType
        {
            get
            {
                if (ViewState["OftenType"] != null)
                    return ViewState["OftenType"].ToString();
                return "无";
            }
            set
            {
                ViewState["OftenType"] = value;
                if (value == "无")
                {
                    this.RegexString = "";
                    this.isClientValidation = false;
                }
                else
                    this.RegexString = "^" + RegexStatic.GetGenerateRegex()[value].ToString() + "$";
            }
        }

        /// <summary>
        /// 设定控件验证的正则表达式
        /// </summary>
        [Category("控件验证"), Description("设定控件验证的正则表达式")]
        public string RegexString
        {
            get
            {
                if (ViewState["RegexString"] != null)
                    return (string)ViewState["RegexString"];
                return "";
            }
            set
            {
                ViewState["RegexString"] = value;
            }
        }

        /// <summary>
        /// 验证的名称
        /// </summary>
        [Category("控件验证"), Description("验证的名称")]
        public string RegexName
        {
            get
            {
                if (ViewState["RegexName"] != null)
                    return (string)ViewState["RegexName"];
                return "";
            }
            set
            {
                ViewState["RegexName"] = value;

            }
        }

        /// <summary>
        /// 设定是否点击控件提示信息
        /// </summary>
        [Category("控件验证"), Description("设定是否点击控件提示信息"), DefaultValue(false)]
        public bool OnClickShowInfo
        {

            get
            {
                if (ViewState["OnClickShowInfo"] != null)
                    return (bool)ViewState["OnClickShowInfo"];
                return false;
            }
            set
            {
                ViewState["OnClickShowInfo"] = value;
            }
        }

        /// <summary>
        /// 设定脚本路径
        /// </summary>
        [Category("Data"), Description("设定脚本路径")]
        public string ScriptPath
        {
            get
            {
                if (ViewState["ScriptPath"] != null)
                    return (string)ViewState["ScriptPath"];
                return Root + "System/WebControls/script.js";
            }
            set
            {
                ViewState["ScriptPath"] = value;

            }
        }

        private string Root
        {
            get
            {
                if (!this.DesignMode && System.Web.HttpContext.Current.Request.ApplicationPath != "/")
                {
                    return System.Web.HttpContext.Current.Request.ApplicationPath + "/";
                }
                else
                {
                    return "/";
                }
            }
        }

        #endregion

        #region 外观属性
        /// <summary>
        /// 验证失败呈现的信息
        /// </summary>
        [Category("控件验证"), Description("验证失败呈现的信息")]
        public string ErrorMessage
        {
            get
            {
                if (ViewState["ErrorMessage"] != null)
                    return (string)ViewState["ErrorMessage"];
                return "";
            }
            set
            {
                ViewState["ErrorMessage"] = value;
            }
        }
        #endregion

        #region 数据属性
        [Category("Data"), Description("设定对应的数据源，格式：FullClassName,AssemblyName 。如果需要绑定实体类，可以设置该属性。")]
        public string DataProvider { get; set; }

        /// <summary>
        /// 设定对应的数据库字段是否是主键，用于自动数据查询和更新的依据
        /// </summary>
        [Category("Data"), Description("设定对应的数据库字段是否是主键，用于自动数据查询和更新的依据"), DefaultValue(false)]
        public bool PrimaryKey
        {
            get
            {
                if (ViewState["PrimaryKey"] != null)
                    return (bool)ViewState["PrimaryKey"];
                return false;
            }
            set
            {
                ViewState["PrimaryKey"] = value;
            }
        }

        /// <summary>
        /// 设定对应的数据字段类型
        /// </summary>
        [Category("Data"), Description("设定对应的数据字段类型")]
        public System.TypeCode SysTypeCode
        {
            get
            {
                if (ViewState["SysTypeCode"] != null)
                    return (System.TypeCode)ViewState["SysTypeCode"];
                return new System.TypeCode();
            }
            set
            {
                ViewState["SysTypeCode"] = value;
            }
        }

        /// <summary>
        /// 数据呈现格式
        /// </summary>
        [Category("外观"), Description("数据呈现格式")]
        public string DataFormatString
        {
            get
            {
                if (ViewState["DataFormatString"] != null)
                    return (string)ViewState["DataFormatString"];
                return "";
            }
            set
            {
                ViewState["DataFormatString"] = value;
            }
        }
        #endregion

        #region 脚本输出

        protected override void OnLoad(EventArgs e)
        {
            string rootScript = "\r\n<script  type=\"text/javascript\" language=\"javascript\">var RootSitePath='" + Root + "';</" + "script>\r\n";
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "JS", rootScript + "\r\n<script src=\"" + ScriptPath + "\"></script>\r\n");
            base.OnLoad(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            string messageType = "";
            switch (MessageType)
            {
                case EnumMessageType.层:
                    messageType = "tip";
                    break;
                case EnumMessageType.提示框:
                    messageType = "alert";
                    break;
            }
            //点击控件提示信息
            if (this.OnClickShowInfo)
            {
                //点击的提示方式始终以层来显示
                this.Attributes.Add("onclick", "DTControl_SetInputBG(this);ShowMessage('请填写" + this.RegexName + "',this,'tip');");
                this.Attributes.Add("onblur", "DTControl_CleInputBG(this);DTControl_Hide_TIPDIV();");
            }


            if (this.IsNull && this.OftenType == "无")
            {
                base.OnPreRender(e);
                return;
            }
            else
            {
                if (!this.IsNull)
                {    //不可为空
                    this.Page.ClientScript.RegisterOnSubmitStatement(this.GetType(), this.UniqueID, "if(document.all." + this.ClientID + ".value==''){\r\n ShowMessage('该项不能为空!',document.all." + this.ClientID + ",'" + messageType + "');\r\n document.all." + this.ClientID + ".focus();return false;\r\n}\r\n");
                }


                //switch (this.OftenType)
                //{
                //    case "日期":
                //        string path = Root + "System/JS/My97DatePicker/WdatePicker.js";
                //        this.Page.ClientScript.RegisterStartupScript(this.GetType (),"JS_calendar", "\r\n<script language='javascript' type='text/javascript' src='"+path+"'></script>\r\n");
                //        this.Attributes.Add("onfocus", "new WdatePicker(this)");

                //        break;

                //}
                if (this.RegexString != "" && this.OnClickShowInfo && !isClientValidation)
                {
                    string RegexString = this.RegexString.Replace(@"\", @"\\");
                    this.Attributes.Add("onchange", "return isCustomRegular(this,'" + RegexString + "','" + this.RegexName + "没有填写正确','" + messageType + "');");
                }


            }
            ////
            if (!isClientValidation)//控件验证脚本
            {
                string script = @"javascript:var key= (event.keyCode | event.which);if( !(( key>=48 && key<=57)|| key==46 || key==37 || key==39 || key==45 || key==43 || key==8 || key==46  ) ) {try{ event.returnValue = false;event.preventDefault();}catch(ex){} alert('" + this.ErrorMessage + "');}";
                switch (Type)
                {
                    case ValidationDataType.String:
                        //Convert.ToString(this.Text.Trim());
                        //邓太华 2006.04.25 添加对于字符串超长的验证
                        if (this.MaxLength > 0 && this.TextMode == TextBoxMode.MultiLine)
                        {
                            string maxlen = this.MaxLength.ToString();
                            this.Attributes.Add("onblur", "javascript:if(this.value.length>" + maxlen + "){this.select();alert('输入的文字不能超过 " + maxlen + " 个字符！');MaxLenError=true;}else{MaxLenError=false;}");
                        }
                        break;
                    case ValidationDataType.Integer:
                        this.Attributes.Add("onkeypress", script);
                        break;

                    case ValidationDataType.Currency:
                        this.Attributes.Add("onkeypress", script);
                        break;

                    case ValidationDataType.Date:
                        string path = Root + "System/JS/My97DatePicker/WdatePicker.js";
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "JS_calendar", "\r\n<script language='javascript' type='text/javascript' src='" + path + "'></script>\r\n");
                        this.Attributes.Add("onfocus", "new WdatePicker(this)");
                        break;
                    case ValidationDataType.Double:
                        this.Attributes.Add("onkeypress", script);
                        break;
                }
            }
            else//执行自定义验证，输出自定义脚本
            {
                this.RegisterClientScript();
                if (this.ClientValidationFunctionString != "")
                {
                    this.Attributes.Add("onblur", @"if(strCheck_" + base.ID + "(this.value)==false) {this.value = '';alert('" + this.ErrorMessage + "');}");

                }
            }
            base.OnPreRender(e);
        }


        /// <summary>
        /// 输出脚本
        /// </summary>
        protected virtual void RegisterClientScript()
        {
            string versionInfo = System.Reflection.Assembly.GetAssembly(this.GetType()).FullName;
            int start = versionInfo.IndexOf("Version=") + 8;
            int end = versionInfo.IndexOf(",", start);
            versionInfo = versionInfo.Substring(start, end - start);
            string info = @"
<!--
 ********************************************
 * ServerControls " + versionInfo + @"
 ********************************************
-->";

            string ClientFunctionString = @"<SCRIPT language=javascript >
function strCheck_" + base.ID + @"(str)
{
var pattern =/" + ClientValidationFunctionString + @"/;
if(pattern.test(str)) 
{
return true; 
}
  return false;}
</SCRIPT>";

            if (this.ClientValidationFunctionString == "")
            {
                ClientFunctionString = "";
            }
            if (ClientFunctionString != string.Empty)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), base.ID + "_Info", info);
            }


            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), base.ID + "_ValidationFunction", ClientFunctionString);
        }


        #endregion

        #region IBrainControl 成员

        #region 数据属性
        /// <summary>
        /// 设定与数据库字段对应的数据名
        /// </summary>
        [Category("Data"), Description("设定与数据库字段对应的数据名")]
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
            set
            {
                ViewState["LinkProperty"] = value;
                // TODO:  添加 BrainTextBox.LinkProperty setter 实现
            }
        }

        /// <summary>
        /// 设定与数据库字段对应的数据表名
        /// </summary>
        [Category("Data"), Description("设定与数据库字段对应的数据表名")]
        public string LinkObject
        {
            get
            {
                if (ViewState["LinkObject"] != null)
                    return (string)ViewState["LinkObject"];
                return "";
            }
            set
            {
                ViewState["LinkObject"] = value;

            }
        }

        #endregion

        #region 其他属性

        //是否只读
        public override bool ReadOnly
        {
            get
            {
                return base.ReadOnly;
            }
            set
            {
                base.ReadOnly = value;
                if (value)
                    //邓太华 20060608 修改，对于只读状态下采用样式定义而不采用背景区分，下面一行被注释
                    //this.BackColor=System.Drawing.Color.FromName("#E0E0E0");
                    this.CssClass = "CssReadOnly";
                else
                    this.BackColor = System.Drawing.Color.Empty;
            }
        }

        //		/// <summary>
        //		/// 获取或者设置文本，如果设置了格式字符串，那么显示文本为格式化后的文本，但是内部处理的时候仍然使用格式化前的文本
        //		/// </summary>
        //		public override string Text
        //		{
        //			get
        //			{
        ////				if(_BaseText==null)
        ////				{
        ////					if(ViewState["BaseText"]!=null)
        ////						_BaseText=ViewState["BaseText"].ToString ();
        ////					else
        ////						_BaseText= base.Text;
        ////				}
        ////				return _BaseText;
        //				return base.Text;
        //				
        //			}
        //			set
        //			{
        //				if(DataFormatString!="")
        //					base.Text =String.Format(DataFormatString,value); 
        //				else
        //					base.Text =value;
        ////				_BaseText=value;
        ////				ViewState["BaseText"]=value;
        //			}
        //		}

        #endregion

        #region 接口方法

        //呈现数据
        public void SetValue(object obj)
        {
            DataTextBoxValue dtbv = new DataTextBoxValue(this);
            dtbv.SetValue(obj);
        }

        //数据收集 
        //为空时string 返回 “”
        //其他类型  一律返回dbnull.value
        //邓太华 2006.8.23 修改，如果数字型的值为空字符串，那么它的值修改为 DBNull.Value 而不是默认的 "0"
        public object GetValue()
        {
            DataTextBoxValue dtbv = new DataTextBoxValue(this);
            return dtbv.GetValue();
        }

        #endregion

        #region 控件验证方法
        public bool Validate()
        {
            // TODO:  添加 BrainTextBox.Validate 实现

            //如果开启控件验证
            if (!this.isClientValidation)
            {
                if (this.Text.Trim() != "")
                {
                    try
                    {
                        switch (Type)
                        {
                            case ValidationDataType.String:
                                Convert.ToString(this.Text.Trim());
                                break;
                            case ValidationDataType.Integer:
                                Convert.ToInt32(this.Text.Trim());
                                break;

                            case ValidationDataType.Currency:
                                Convert.ToDecimal(this.Text.Trim());
                                break;

                            case ValidationDataType.Date:
                                Convert.ToDateTime(this.Text.Trim());
                                break;
                            case ValidationDataType.Double:
                                Convert.ToDouble(this.Text.Trim());
                                break;

                        }
                        return true;
                        //						if(!this.isNull)//不允许为空
                        //						{
                        //							return false;
                        //						}
                        //						else//允许为空
                        //						{
                        //							return true;
                        //						}
                    }
                    catch
                    {
                        return false;//异常 数据类型 不符合
                    }
                }
                else
                {
                    //邓太华 2006.05.8 修改，如果输入值为空在进行判断，上面部分已被注释
                    //return true;
                    if (!this.IsNull)//不允许为空
                    {
                        return false;
                    }
                    else//允许为空
                    {
                        return true;
                    }
                }
            }
            else//不开启控件验证
            {
                return true;
            }
        }
        #endregion

        #region 自定义验证方法

        /// <summary>
        /// 设定自定义验证正则表达式
        /// </summary>
        [Category("自定义验证"), Description("设定自定义验证正则表达式"), DefaultValue("")]
        public string ClientValidationFunctionString
        {
            get
            {
                if (ViewState["ClientValidationFunctionString"] != null)
                    return (string)ViewState["ClientValidationFunctionString"];
                return "";
            }
            set
            {
                ViewState["ClientValidationFunctionString"] = value;
            }
        }

        /// <summary>
        /// 设定控件是否采取自定义验证(停止控件验证)
        /// </summary>
        [Category("自定义验证"), Description("设定控件是否采取自定义验证(停止控件验证)"), DefaultValue(false)]
        public bool isClientValidation
        {

            get
            {
                if (ViewState["ClientValidation"] != null)
                    return (bool)ViewState["ClientValidation"];
                return false;
            }
            set
            {
                ViewState["ClientValidation"] = value;

            }
        }

        #endregion

        #region 控件验证
        /// <summary>
        /// 控件验证--设定控件值是否可以为空
        /// </summary>
        [Category("控件验证"), Description("设定控件值是否可以为空"), DefaultValue(true)]
        public bool IsNull
        {

            get
            {
                if (ViewState["isNull"] != null)
                    return (bool)ViewState["isNull"];
                return true;
            }
            set
            {
                ViewState["isNull"] = value;

            }
        }
        #endregion

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
    }
}
