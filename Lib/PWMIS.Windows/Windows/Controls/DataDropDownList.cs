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
    [System.Drawing.ToolboxBitmap(typeof(ControlIcon), "DataDropDownList.bmp")]
    public partial class DataDropDownList : ComboBox, IDataControl, IQueryControl
    {

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
                _LinkProperty = value;
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
                _SysTypeCode = value;
            }
        }



        private bool _isNull = true;

        [Category("控件验证"), Description("设定控件值是否可以为空"), DefaultValue(true)]
        public bool IsNull
        {
            get { return _isNull; }
            set { _isNull = value; }
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

        private bool _readOnly;
        /// <summary>
        /// 是否只读
        /// </summary>
        public bool ReadOnly
        {
            get { return _readOnly; }
            set
            {
                _readOnly = value;
                this.Enabled = !_readOnly;
            }
        }

        public bool IsValid
        {
            get { return true; }
        }


        public void SetValue(object value)
        {
            if (value != null)
            {
                //如果绑定的是实体类列表，Items是实体类集合，此时 value是单值，无法通过这个判断。
                //感谢网友 上海-bingoyin 发现此问题。
                //if (this.Items.Contains(value))
                //{
                this.SelectedValue = value;
                //}
                //如果 value 是下拉框控件不存在的值，那么赋值之后，SelectedValue 将是 null
            }
        }

        public object GetValue()
        {
            return this.SelectedValue;
        }

        public virtual bool Validate()
        {
            if (!IsNull)
            {
                if (this.SelectedValue!=null)
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
