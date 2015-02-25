using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PWMIS.Common;
using System.Drawing.Design;

namespace PWMIS.Windows.Controls
{
    [System.Drawing.ToolboxBitmap(typeof(ControlIcon), "DataListBox.bmp")]
    public partial class DataListBox : ListBox, IDataControl, IQueryControl
    {

        #region IDataControl 成员
        //[Category("Data"), Description("设定对应的数据源，格式：FullClassName,AssemblyName 。如果需要绑定实体类，可以设置该属性。")]
        //public string DataProvider { get; set; }

        [Category("Data"), Description("设定与数据库字段对应的数据名")]
        [Editor(typeof(PropertyUITypeEditor), typeof(UITypeEditor))]
        public string LinkProperty
        {
            get;
            set;
        }

        /// <summary>
        /// 设定与数据库字段对应的数据表名
        /// </summary>
        [Category("Data"), Description("设定与数据库字段对应的数据表名")]
        public string LinkObject
        {
            get;
            set;
        }
        public bool IsValid
        {
            get { return true; }
        }

        public TypeCode SysTypeCode
        {
            get;
            set;
        }

        public bool ReadOnly
        {
            get
            {
                //未选择，设置为只读数据属性，将不更新数据库。dth,2008.7.27
                if (this.SelectedIndex == -1)
                    return true;
                return !this.Enabled;
            }
            set
            {
                this.Enabled = !value;
            }
        }

        public bool IsNull
        {
            get { return true; }
        }

        /// <summary>
        /// 设定对应的数据字段是否是主键
        /// </summary>
        [Category("Data"), Description("设定对应的数据字段是否是主键")]
        public bool PrimaryKey
        {
            get;
            set;
        }

        public void SetValue(object obj)
        {
            if (obj == null) return;
            this.ClearSelected();
           
            string SelItemValues = "";
            //if(obj!=null)
            SelItemValues = obj.ToString().Trim();
            //string delimStr = ",";
            //char [] delimiter = delimStr.ToCharArray();

            string[] SelItemobj = SelItemValues.Split(',');// SelItemValues.Split(delimiter);

            foreach (string s in SelItemobj)
            {
                foreach (var item in this.Items)
                {
                    if (item.ToString() == s)
                    {
                        this.SelectedItem = item;
                        break;                    
                    }
                }
            }
        }

        public object GetValue()
        {
            string SelItemValues = "";
            foreach (var item in this.SelectedItems)
            {
                if (item != null)
                {
                    if (SelItemValues == string.Empty)
                    {
                        SelItemValues += item.ToString();
                    }
                    else
                    {
                        SelItemValues += "," + item.ToString();
                    }

                }
            }
            return SelItemValues;
        }

        public bool Validate()
        {
            if (!IsNull)
            {
                if (this.SelectedValue==null)
                {
                    return false;
                }

            }
            return true;
        }

        #endregion

        #region IQueryControl 成员

        public string CompareSymbol
        {
            get;
            set;
        }

        public string QueryFormatString
        {
            get;
            set;
        }

        #endregion
    }
}
