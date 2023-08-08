using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using PWMIS.Common;

namespace PWMIS.Windows.Controls
{
    [ToolboxBitmap(typeof(ControlIcon), "DataListBox.bmp")]
    public partial class DataListBox : ListBox, IDataControl, IQueryControl
    {
        #region IDataControl 成员

        //[Category("Data"), Description("设定对应的数据源，格式：FullClassName,AssemblyName 。如果需要绑定实体类，可以设置该属性。")]
        //public string DataProvider { get; set; }

        [Category("Data")]
        [Description("设定与数据库字段对应的数据名")]
        [Editor(typeof(PropertyUITypeEditor), typeof(UITypeEditor))]
        public string LinkProperty { get; set; }

        /// <summary>
        ///     设定与数据库字段对应的数据表名
        /// </summary>
        [Category("Data")]
        [Description("设定与数据库字段对应的数据表名")]
        public string LinkObject { get; set; }

        public bool IsValid => true;

        public TypeCode SysTypeCode { get; set; }

        public bool ReadOnly
        {
            get
            {
                //未选择，设置为只读数据属性，将不更新数据库。dth,2008.7.27
                if (SelectedIndex == -1)
                    return true;
                return !Enabled;
            }
            set => Enabled = !value;
        }

        public bool IsNull => true;

        /// <summary>
        ///     设定对应的数据字段是否是主键
        /// </summary>
        [Category("Data")]
        [Description("设定对应的数据字段是否是主键")]
        public bool PrimaryKey { get; set; }

        public void SetValue(object obj)
        {
            if (obj == null) return;
            ClearSelected();

            var SelItemValues = "";
            //if(obj!=null)
            SelItemValues = obj.ToString().Trim();
            //string delimStr = ",";
            //char [] delimiter = delimStr.ToCharArray();

            var SelItemobj = SelItemValues.Split(','); // SelItemValues.Split(delimiter);

            foreach (var s in SelItemobj)
            foreach (var item in Items)
                if (item.ToString() == s)
                {
                    SelectedItem = item;
                    break;
                }
        }

        public object GetValue()
        {
            var SelItemValues = "";
            foreach (var item in SelectedItems)
                if (item != null)
                {
                    if (SelItemValues == string.Empty)
                        SelItemValues += item.ToString();
                    else
                        SelItemValues += "," + item;
                }

            return SelItemValues;
        }

        public bool Validate()
        {
            if (!IsNull)
                if (SelectedValue == null)
                    return false;
            return true;
        }

        #endregion

        #region IQueryControl 成员

        public string CompareSymbol { get; set; }

        public string QueryFormatString { get; set; }

        #endregion
    }
}