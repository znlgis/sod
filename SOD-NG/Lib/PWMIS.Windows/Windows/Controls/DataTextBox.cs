﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using PWMIS.Common;
using PWMIS.DataMap;
using PWMIS.Windows.Validate;

namespace PWMIS.Windows.Controls
{
    [ToolboxBitmap(typeof (ControlIcon), "DataTextBox.bmp")]
    public class DataTextBox : TextBox, IDataTextBox, IQueryControl, IValidationControl
    {
        #region 外观属性

        /// <summary>
        ///     验证失败呈现的信息
        /// </summary>
        [Category("外观"), Description("验证失败呈现的信息")]
        public string ErrorMessage { get; set; }

        #endregion

        #region IDataControl 成员

        //[Category("Data"), Description("设定对应的数据源，格式：FullClassName,AssemblyName 。如果需要绑定实体类，可以设置该属性。")]
        //public string DataProvider { get; set; }

        /// <summary>
        ///     设定与数据库字段对应的数据名
        /// </summary>
        [Category("Data"), Description("设定与数据库字段对应的数据名")]
        [Editor(typeof (PropertyUITypeEditor), typeof (UITypeEditor))]
        public string LinkProperty { get; set; }

        [Category("Data"), Description("设定与数据库字段对应的数据表名")]
        public string LinkObject { get; set; }


        [Category("Data"), Description("设定对应的数据字段类型")]
        public TypeCode SysTypeCode { get; set; }


        private bool _isNull = true;

        [Category("控件验证"), Description("设定控件值是否可以为空"), DefaultValue(true)]
        public bool IsNull
        {
            get { return _isNull; }
            set { _isNull = value; }
        }

        /// <summary>
        ///     设定对应的数据字段是否是主键
        /// </summary>
        [Category("Data"), Description("设定对应的数据字段是否是主键")]
        public bool PrimaryKey { get; set; }

        public void SetValue(object obj)
        {
            var dtbv = new DataTextBoxValue(this);
            dtbv.SetValue(obj);
        }

        /// <summary>
        ///     数据呈现格式
        /// </summary>
        [Category("外观"), Description("数据呈现格式")]
        public string DataFormatString { get; set; }

        //数据收集 
        //为空时string 返回 “”
        //其他类型  一律返回dbnull.value
        //邓太华 2006.8.23 修改，如果数字型的值为空字符串，那么它的值修改为 DBNull.Value 而不是默认的 "0"
        public object GetValue()
        {
            var dtbv = new DataTextBoxValue(this);
            return dtbv.GetValue();
        }

        public bool Validate()
        {
            //如果开启控件验证
            if (!IsClientValidation)
            {
                if (Text.Trim() != "")
                {
                    try
                    {
                        switch (Type)
                        {
                            case ValidationDataType.String:
                                //执行正则表达式
                                if (!string.IsNullOrEmpty(RegexString))
                                {
                                    return Regex.IsMatch(Text.Trim(), RegexString);
                                }
                                break;
                            case ValidationDataType.Integer:
                                Convert.ToInt32(Text.Trim());
                                break;

                            case ValidationDataType.Currency:
                                Convert.ToDecimal(Text.Trim());
                                break;

                            case ValidationDataType.Date:
                                Convert.ToDateTime(Text.Trim());
                                break;
                            case ValidationDataType.Double:
                                Convert.ToDouble(Text.Trim());
                                break;
                        }
                        return true;
                    }
                    catch
                    {
                        return false; //异常 数据类型 不符合
                    }
                }
                //邓太华 2006.05.8 修改，如果输入值为空在进行判断，上面部分已被注释
                //return true;
                if (!IsNull) //不允许为空
                {
                    return false;
                }
                return true;
            }
            return true;
        }

        #endregion

        #region IQueryControl 成员

        public string CompareSymbol { get; set; }

        public string QueryFormatString { get; set; }

        #endregion

        #region 控件验证

        private ValidationDataType _validationDataType = ValidationDataType.String;

        /// <summary>
        ///     执行服务器验证时的数据类型
        /// </summary>
        [Category("控件验证"), Description("执行服务器验证时的数据类型")]
        public ValidationDataType Type { get; set; }

        /// <summary>
        ///     是否通过服务器验证默认为true
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
                var str = Text.Trim();
                if (!IsNull && str == "")
                    return false;
                if (str != "" && ClientValidationFunctionString != "")
                {
                    return Regex.IsMatch(str, ClientValidationFunctionString);
                }
                return true;
            }
        }


        /// <summary>
        ///     提示信息的类型
        /// </summary>
        [Category("控件验证"), Description("提示信息的类型")]
        [TypeConverter(typeof (EnumConverter))]
        public EnumMessageType MessageType { get; set; }

        private string _oftenType = "无";

        /// <summary>
        ///     需要验证的常用数据类型，如果设定为“无”，将停止控件验证。
        /// </summary>
        [Category("控件验证"), Description("需要验证的常用数据类型，如果设定为“无”，将停止控件验证。")]
        [TypeConverter(typeof (StandardRegexListConvertor))]
        public string OftenType
        {
            get { return _oftenType; }
            set
            {
                _oftenType = value;
                if (value == "无")
                {
                    RegexString = "";
                    IsClientValidation = false;
                }
                else
                {
                    RegexString = "^" + RegexStatic.GetGenerateRegex()[value] + "$";
                    ErrorMessage = "要求内容为" + value;
                }
            }
        }

        /// <summary>
        ///     设定控件验证的正则表达式
        /// </summary>
        [Category("控件验证"), Description("设定控件验证的正则表达式")]
        public string RegexString { get; set; }

        /// <summary>
        ///     验证的名称
        /// </summary>
        [Category("控件验证"), Description("验证的名称")]
        public string RegexName { get; set; }

        /// <summary>
        ///     设定是否点击控件提示信息
        /// </summary>
        [Category("控件验证"), Description("设定是否点击控件提示信息"), DefaultValue(false)]
        public bool OnClickShowInfo { get; set; }

        #endregion

        #region 自定义验证方法

        /// <summary>
        ///     设定自定义验证正则表达式
        /// </summary>
        [Category("自定义验证"), Description("设定自定义验证正则表达式"), DefaultValue("")]
        public string ClientValidationFunctionString { get; set; }

        /// <summary>
        ///     设定控件是否采取自定义验证(停止控件验证)
        /// </summary>
        [Category("自定义验证"), Description("设定控件是否采取自定义验证(停止控件验证)"), DefaultValue(false)]
        public bool IsClientValidation { get; set; }

        #endregion
    }
}