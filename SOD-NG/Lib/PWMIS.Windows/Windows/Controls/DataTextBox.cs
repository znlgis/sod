using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PWMIS.Common;
using PWMIS.DataMap;
using PWMIS.Windows.Validate;
using System.Text.RegularExpressions;
using System.Drawing.Design;

namespace PWMIS.Windows.Controls
{
    [System.Drawing.ToolboxBitmap(typeof(ControlIcon), "DataTextBox.bmp")]
    public class DataTextBox : TextBox, IDataTextBox, IQueryControl, PWMIS.Windows.IValidationControl
    {
        
        public DataTextBox()
        { }


        #region IDataControl 成员
        //[Category("Data"), Description("设定对应的数据源，格式：FullClassName,AssemblyName 。如果需要绑定实体类，可以设置该属性。")]
        //public string DataProvider { get; set; }

        private string _LinkProperty;

        /// <summary>
        /// 设定与数据库字段对应的数据名
        /// </summary>
        [Category("Data"), Description("设定与数据库字段对应的数据名")]
        [Editor(typeof(PropertyUITypeEditor), typeof(UITypeEditor))]
        public string LinkProperty
        {
            get
            {
                return _LinkProperty;
            }
            set
            {
                _LinkProperty=value;
            }
        }

        private string _LinkObject;

        [Category("Data"), Description("设定与数据库字段对应的数据表名")]
        public string LinkObject
        {
            get
            {
                return _LinkObject;
            }
            set
            {
                _LinkObject = value;
            }
        }


        private TypeCode _SysTypeCode;

        [Category("Data"), Description("设定对应的数据字段类型")]
        public TypeCode SysTypeCode
        {
            get
            {
                return _SysTypeCode;
            }
            set
            {
                _SysTypeCode=value;
            }
        }

       

        private bool _isNull=true;

        [Category("控件验证"), Description("设定控件值是否可以为空"), DefaultValue(true)]
        public bool IsNull
        {
            get { return _isNull; }
            set { _isNull=value; }
        }

        bool _PrimaryKey;

        /// <summary>
        /// 设定对应的数据字段是否是主键
        /// </summary>
        [Category("Data"), Description("设定对应的数据字段是否是主键")]
        public bool PrimaryKey
        {
            get
            {
                return _PrimaryKey;
            }
            set
            {
                _PrimaryKey = value;
            }
        }

        public void SetValue(object obj)
        {
            DataTextBoxValue dtbv = new DataTextBoxValue(this);
            dtbv.SetValue(obj);
        }

        private string _DataFormatString;
        /// <summary>
        /// 数据呈现格式
        /// </summary>
        [Category("外观"), Description("数据呈现格式")]
        public string DataFormatString
        {
            get
            {
                return _DataFormatString;
            }
            set
            {
                _DataFormatString = value;
            }
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

        public bool Validate()
        {
            //如果开启控件验证
            if (!this.IsClientValidation)
            {
                if (this.Text.Trim() != "")
                {
                    try
                    {
                        switch (Type)
                        {
                            case ValidationDataType.String:
                                //执行正则表达式
                                if (!string.IsNullOrEmpty(this.RegexString))
                                {
                                    return Regex.IsMatch(this.Text.Trim(), this.RegexString);
                                }
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

        #region IQueryControl 成员

        string _CompareSymbol;

        public string CompareSymbol
        {
            get
            {
                return _CompareSymbol;
            }
            set
            {
                _CompareSymbol = value;
            }
        }

        string _QueryFormatString;

        public string QueryFormatString
        {
            get
            {
                return _QueryFormatString;
                
            }
            set
            {
                _QueryFormatString = value;
            }
        }

        #endregion


        #region 控件验证

        private ValidationDataType _validationDataType = ValidationDataType.String;
        /// <summary>
        /// 执行服务器验证时的数据类型
        /// </summary>
        [Category("控件验证"), Description("执行服务器验证时的数据类型")]
        public ValidationDataType Type
        {
            get;
            set;
        }

        /// <summary>
        /// 是否通过服务器验证默认为true
        /// </summary>
        [Category("控件验证"), Description("是否通过服务器验证默认为true")]
        public bool IsValid
        {
            get
            {
                if (!IsClientValidation)
                {
                    return Validate();
                }
                else
                {
                    string str = this.Text.Trim();
                    if (!this.IsNull && str == "")
                        return false;
                    if (str!="" && ClientValidationFunctionString != "")
                    {
                        return Regex.IsMatch(str, ClientValidationFunctionString);
                    }
                }
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

        private string _oftenType = "无";
        /// <summary>
        /// 需要验证的常用数据类型，如果设定为“无”，将停止控件验证。
        /// </summary>
        [Category("控件验证"), Description("需要验证的常用数据类型，如果设定为“无”，将停止控件验证。")]
        [TypeConverter(typeof(StandardRegexListConvertor))]
        public string OftenType
        {
            get
            {
                return _oftenType;
            }
            set
            {
                _oftenType = value;
                if (value == "无")
                {
                    this.RegexString = "";
                    this.IsClientValidation = false;
                }
                else
                {
                    this.RegexString = "^" + RegexStatic.GetGenerateRegex()[value].ToString() + "$";
                    this.ErrorMessage = "要求内容为"+value;
                }
                  
            }
        }

        /// <summary>
        /// 设定控件验证的正则表达式
        /// </summary>
        [Category("控件验证"), Description("设定控件验证的正则表达式")]
        public string RegexString
        {
            get;
            set;
        }

        /// <summary>
        /// 验证的名称
        /// </summary>
        [Category("控件验证"), Description("验证的名称")]
        public string RegexName
        {
            get;
            set;
        }

        /// <summary>
        /// 设定是否点击控件提示信息
        /// </summary>
        [Category("控件验证"), Description("设定是否点击控件提示信息"), DefaultValue(false)]
        public bool OnClickShowInfo
        {

            get;
            set;
        }

        #endregion

        #region 外观属性
        /// <summary>
        /// 验证失败呈现的信息
        /// </summary>
        [Category("外观"), Description("验证失败呈现的信息")]
        public string ErrorMessage
        {
            get;
            set;
        }
        #endregion

        #region 自定义验证方法

        /// <summary>
        /// 设定自定义验证正则表达式
        /// </summary>
        [Category("自定义验证"), Description("设定自定义验证正则表达式"), DefaultValue("")]
        public string ClientValidationFunctionString
        {
            get;
            set;
        }

        /// <summary>
        /// 设定控件是否采取自定义验证(停止控件验证)
        /// </summary>
        [Category("自定义验证"), Description("设定控件是否采取自定义验证(停止控件验证)"), DefaultValue(false)]
        public bool IsClientValidation
        {
            get;
            set;
        }

        #endregion
    }
}
