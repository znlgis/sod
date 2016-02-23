using System;
using System.Collections.Generic;
using System.Text;
using PWMIS.Common;

namespace PWMIS.DataMap
{
    /// <summary>
    /// 数据选择框控件值处理类，用于WinFrom,WebForm的复选框控件和单选框控件
    /// </summary>
    public class DataCheckBoxValue
    {
        private IDataCheckBox dataCheckBox;
        private bool singleSelect;

        public bool Checked
        {
            get { return dataCheckBox.Checked; }
            set { dataCheckBox.Checked = value; }
        }

        /// <summary>
        /// 初始化数据控件
        /// </summary>
        /// <param name="dataCheckBox">当前控件实例</param>
        /// <param name="singleSelect">是否单选</param>
        public DataCheckBoxValue(IDataCheckBox dataCheckBox, bool singleSelect)
        {
            this.dataCheckBox = dataCheckBox;
            this.singleSelect = singleSelect;
        }

        public void SetValue(object obj)
        {
            this.Checked = false;
            if (obj == null || obj == DBNull.Value)
            {
                return;
            }
            string SelItemValues = "";
            SelItemValues = obj.ToString().Trim();
           

            string[] SelItemobj = SelItemValues.Split(',');
            string strValue=dataCheckBox.Value.Trim();

            string strTemp = strValue.ToLower();
            string strBoolInt = "";
            if (strTemp == "true") strBoolInt = "1";
            else if (strTemp == "false") strBoolInt = "0";

            foreach (string s in SelItemobj)
            {
                string s1 = s.Trim();
                if (string.IsNullOrEmpty(s1))
                    continue;
                if (strValue == s1 || strBoolInt==s1)
                {
                    this.Checked = true;
                    break;//add 2008.7.26
                }
            }
        }

        public object GetValue()
        {
            //对于布尔型也不能直接处理返回值，比如成组的单选按钮控件，当前控件如果没有选择，则不应该收集当前控件的值
            if (!this.singleSelect && dataCheckBox.SysTypeCode == TypeCode.Boolean)
                return this.Checked;

            if (!this.Checked)
                return DBNull.Value;

            string strValue =dataCheckBox.Value==null?"": dataCheckBox.Value.Trim();
            switch (dataCheckBox.SysTypeCode)
            {
                case TypeCode.String:
                    {
                        return strValue;
                    }
                case TypeCode.Int32:
                    {
                        if (strValue != "")
                        {
                            return Convert.ToInt32(strValue);
                        }
                        //return 0;
                        return DBNull.Value;
                    }
                case TypeCode.Decimal:
                    {
                        if (strValue != "")
                        {
                            return Convert.ToDecimal(strValue);
                        }
                        //return 0;
                        return DBNull.Value;
                    }
                case TypeCode.DateTime:
                    if (strValue != "")
                    {
                        try
                        {
                            return Convert.ToDateTime(strValue);
                        }
                        catch
                        {
                            return DBNull.Value; //"1900-1-1";
                        }
                    }
                    return DBNull.Value;//"1900-1-1";

                case TypeCode.Double:
                    {
                        if (strValue != "")
                        {
                            return Convert.ToDouble(strValue);
                        }
                        //return 0;
                        return DBNull.Value;
                    }
                case TypeCode.Boolean :
                    {
                        return this.Checked;
                    }
                default:
                    if (strValue == "")
                    {
                        return DBNull.Value;
                    }
                    else
                    {
                        return strValue;
                    }
            }
        }
    }
}
