using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PWMIS.Common;
using PWMIS.DataMap;
using System.Drawing.Design;

namespace PWMIS.Windows.Controls
{
    [System.Drawing.ToolboxBitmap(typeof(ControlIcon), "DataRadioButton.bmp")]
    public partial class DataRadioButton : RadioButton, IDataCheckBox, IQueryControl
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
                if (this.Checked)
                    return false;
                return !base.Enabled;
            }
            set
            {
                base.Enabled = !value;
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

        [Category("外观"), Description("设定与数据库字段对应的数据值")]
        public string Value
        {
            get;
            set;
        }

        public void SetValue(object value)
        {
            DataCheckBoxValue dcbv = new DataCheckBoxValue(this);
            dcbv.SetValue(value);
        }

        public object GetValue()
        {
            DataCheckBoxValue dcbv = new DataCheckBoxValue(this);
            return dcbv.GetValue();
        }

        public bool Validate()
        {
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
