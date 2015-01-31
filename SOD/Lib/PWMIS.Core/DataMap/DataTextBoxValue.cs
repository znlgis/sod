using System;
using System.Collections.Generic;
using System.Text;
using PWMIS.Common;

namespace PWMIS.DataMap
{
    /// <summary>
    /// 数据文本框、标签控件值读写通用类，可以处理WinForm，WebForm等窗体的数据文本框控件和标签控件。
    /// </summary>
    public class DataTextBoxValue
    {
        private IDataTextBox dataTextBox;
        public string Text
        {
            get { return this.dataTextBox.Text; }
            set { this.dataTextBox.Text = value; }
        }

        public DataTextBoxValue(IDataTextBox dataTextBox)
        {
            this.dataTextBox = dataTextBox;
        }

        public void SetValue(object obj)
        {
            if (obj == null || obj.ToString() == "")
            {
                this.Text = "";
                return;
            }

            // 邓太华 2006.8.11 添加单精度型和默认类型的实现
            switch (dataTextBox.SysTypeCode)
            {
                case TypeCode.String:
                    if (obj != DBNull.Value)
                    {
                        if (!string.IsNullOrEmpty( dataTextBox.DataFormatString ))
                            this.Text = String.Format(dataTextBox.DataFormatString, obj.ToString());
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
                    if (obj != DBNull.Value && (obj.GetType() == typeof(decimal) || obj.GetType() == typeof(double)))
                    {
                        if (!string.IsNullOrEmpty(dataTextBox.DataFormatString))
                            this.Text = String.Format(dataTextBox.DataFormatString, obj);
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
                        if (!string.IsNullOrEmpty(dataTextBox.DataFormatString))
                        {
                            this.Text = String.Format(dataTextBox.DataFormatString, obj);
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
                        if (!string.IsNullOrEmpty(dataTextBox.DataFormatString))
                            this.Text = String.Format(dataTextBox.DataFormatString, obj);
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
            #region 被注释的代码 2014.4.16
            //switch (dataTextBox.SysTypeCode)
            //{
            //    case TypeCode.String:
            //        {
            //            return this.Text.Trim();
            //        }
            //    case TypeCode.Int32:
            //        {
            //            if (this.Text.Trim() != "")
            //            {
            //                return Convert.ToInt32(this.Text.Trim());
            //            }
            //            //return 0;
            //            return DBNull.Value;
            //        }
            //    case TypeCode.Decimal:
            //        {
            //            if (this.Text.Trim() != "")
            //            {
            //                return Convert.ToDecimal(this.Text.Trim());
            //            }
            //            //return 0;
            //            return DBNull.Value;
            //        }
            //    case TypeCode.DateTime:
            //        if (this.Text.Trim() != "")
            //        {
            //            try
            //            {
            //                return Convert.ToDateTime(this.Text.Trim());
            //            }
            //            catch
            //            {
            //                return DBNull.Value; //"1900-1-1";
            //            }
            //        }
            //        return DBNull.Value;//"1900-1-1";

            //    case TypeCode.Double:
            //        {
            //            if (this.Text.Trim() != "")
            //            {
            //                return Convert.ToDouble(this.Text.Trim());
            //            }
            //            //return 0;
            //            return DBNull.Value;
            //        }
            //    case TypeCode.Boolean:
            //        {
            //            if (this.Text.Trim() != "")
            //            {
            //                try
            //                {
            //                    return Convert.ToBoolean(this.Text.Trim());
            //                }
            //                catch
            //                {
            //                    return DBNull.Value; //"1900-1-1";
            //                }
            //            }
            //            return DBNull.Value;//"1900-1-1";
            //        }
            //    default:
            //        if (this.Text.Trim() == "")
            //        {
            //            return DBNull.Value;
            //        }
            //        else
            //        {
            //            return this.Text.Trim();
            //        }
            //}
            #endregion

            if (dataTextBox.SysTypeCode == TypeCode.String)
            {
                return this.Text;
            }
            else
            {
                string text=this.Text.Trim();
                if ( !string.IsNullOrEmpty(text))
                {
                    try
                    {
                        return Convert.ChangeType(text, dataTextBox.SysTypeCode);
                    }
                    catch
                    {
                        return DBNull.Value; 
                    }
                }
                return DBNull.Value;
            
            }
        }
    }
}
