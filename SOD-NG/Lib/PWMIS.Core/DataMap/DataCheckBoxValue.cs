using System;
using PWMIS.Common;

namespace PWMIS.DataMap
{
    public class DataCheckBoxValue
    {
        private readonly IDataCheckBox dataCheckBox;

        public DataCheckBoxValue(IDataCheckBox dataCheckBox)
        {
            this.dataCheckBox = dataCheckBox;
        }

        public bool Checked
        {
            get { return dataCheckBox.Checked; }
            set { dataCheckBox.Checked = value; }
        }

        public void SetValue(object obj)
        {
            Checked = false;
            if (obj == null || obj == DBNull.Value)
            {
                return;
            }
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
                    return DBNull.Value; //"1900-1-1";

                case TypeCode.Double:
                {
                    if (strValue != "")
                    {
                        return Convert.ToDouble(strValue);
                    }
                    //return 0;
                    return DBNull.Value;
                }
                case TypeCode.Boolean:
                {
                    if (strValue != "")
                    {
                        try
                        {
                            return Convert.ToBoolean(strValue);
                        }
                        catch
                        {
                            return DBNull.Value; //"1900-1-1";
                        }
                    }
                    return DBNull.Value; //"1900-1-1";
                }
                default:
                    if (strValue == "")
                    {
                        return DBNull.Value;
                    }
                    return strValue;
            }
        }
    }
}