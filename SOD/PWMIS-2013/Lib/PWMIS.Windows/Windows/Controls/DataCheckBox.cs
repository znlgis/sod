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
    [System.Drawing.ToolboxBitmap(typeof(ControlIcon), "DataCheckBox.bmp")]
    public partial class DataCheckBox : CheckBox, IDataCheckBox, IQueryControl
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

        public TypeCode _sysTypeCode;
        public TypeCode SysTypeCode
        {
            get{return _sysTypeCode;}
            set{
                _sysTypeCode = value;
                if (_sysTypeCode == TypeCode.Boolean)
                {
                    this.Value = "True";
                }
                else if (_sysTypeCode == TypeCode.Int32 || _sysTypeCode == TypeCode.Int64)
                {
                    this.Value = "0";
                }
                else
                {
                    this.Value = "";
                }
            }
        }

        private bool _readOnly;
        /// <summary>
        /// 是否只读
        /// </summary>
        public bool ReadOnly
        {
            get { return _readOnly; }
            set {
                _readOnly = value;
                this.Enabled = !_readOnly;
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
