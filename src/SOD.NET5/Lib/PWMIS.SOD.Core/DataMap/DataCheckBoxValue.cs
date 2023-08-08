//Edit @ FileVersion 5.6.3.0305 :DataCheckBoxValue 的GetValue方法根据设定的值判断，比如当前选项按钮选择了，但其值设定为 "False"

using System;
using PWMIS.Common;

namespace PWMIS.DataMap
{
    /// <summary>
    ///     数据选择框控件值处理类，用于WinFrom,WebForm的复选框控件和单选框控件
    /// </summary>
    public class DataCheckBoxValue
    {
        private readonly IDataCheckBox dataCheckBox;
        private readonly bool singleSelect;

        /// <summary>
        ///     初始化数据控件
        /// </summary>
        /// <param name="dataCheckBox">当前控件实例</param>
        /// <param name="singleSelect">是否单选</param>
        public DataCheckBoxValue(IDataCheckBox dataCheckBox, bool singleSelect)
        {
            this.dataCheckBox = dataCheckBox;
            this.singleSelect = singleSelect;
        }

        public bool Checked
        {
            get => dataCheckBox.Checked;
            set => dataCheckBox.Checked = value;
        }

        public void SetValue(object obj)
        {
            Checked = false;
            if (obj == null || obj == DBNull.Value) return;
            var SelItemValues = "";
            SelItemValues = obj.ToString().Trim();


            var SelItemobj = SelItemValues.Split(',');
            var strValue = dataCheckBox.Value.Trim();

            var strTemp = strValue.ToLower();
            var strBoolInt = "";
            if (strTemp == "true") strBoolInt = "1";
            else if (strTemp == "false") strBoolInt = "0";

            foreach (var s in SelItemobj)
            {
                var s1 = s.Trim();
                if (string.IsNullOrEmpty(s1))
                    continue;
                if (strValue == s1 || strBoolInt == s1)
                {
                    Checked = true;
                    break; //add 2008.7.26
                }
            }
        }

        public object GetValue()
        {
            //对于布尔型也不能直接处理返回值，比如成组的单选按钮控件，当前控件如果没有选择，则不应该收集当前控件的值
            if (!singleSelect && dataCheckBox.SysTypeCode == TypeCode.Boolean)
                return Checked;

            if (!Checked)
                return DBNull.Value;

            var strValue = dataCheckBox.Value == null ? "" : dataCheckBox.Value.Trim();
            switch (dataCheckBox.SysTypeCode)
            {
                case TypeCode.String:
                {
                    return strValue;
                }
                case TypeCode.Int32:
                {
                    if (strValue != "") return Convert.ToInt32(strValue);
                    //return 0;
                    return DBNull.Value;
                }
                case TypeCode.Decimal:
                {
                    if (strValue != "") return Convert.ToDecimal(strValue);
                    //return 0;
                    return DBNull.Value;
                }
                case TypeCode.DateTime:
                    if (strValue != "")
                        try
                        {
                            return Convert.ToDateTime(strValue);
                        }
                        catch
                        {
                            return DBNull.Value; //"1900-1-1";
                        }

                    return DBNull.Value; //"1900-1-1";

                case TypeCode.Double:
                {
                    if (strValue != "") return Convert.ToDouble(strValue);
                    //return 0;
                    return DBNull.Value;
                }
                case TypeCode.Boolean:
                {
                    //根据设定的值判断，比如当前选项按钮选择了，但其值设定为 "False",EDIT at 2019.3.5
                    if (strValue != "") return Convert.ToBoolean(strValue);
                    return Checked;
                }
                default:
                    if (strValue == "")
                        return DBNull.Value;
                    return strValue;
            }
        }
    }
}