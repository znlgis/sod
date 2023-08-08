using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using PWMIS.Common;

namespace PWMIS.Windows.Controls
{
    [ToolboxBitmap(typeof(ControlIcon), "DataDropDownList.bmp")]
    public partial class DataDropDownList : ComboBox, IDataControl, IQueryControl
    {
        #region IDataControl 成员

        //[Category("Data"), Description("设定对应的数据源，格式：FullClassName,AssemblyName 。如果需要绑定实体类，可以设置该属性。")]
        //public string DataProvider { get; set; }

        /// <summary>
        ///     设定与数据库字段对应的数据名
        /// </summary>
        [Category("Data")]
        [Description("设定与数据库字段对应的数据名")]
        [Editor(typeof(PropertyUITypeEditor), typeof(UITypeEditor))]
        public string LinkProperty { get; set; }

        [Category("Data")]
        [Description("设定与数据库字段对应的数据表名")]
        public string LinkObject { get; set; }


        [Category("Data")]
        [Description("设定对应的数据字段类型")]
        public TypeCode SysTypeCode { get; set; }


        [Category("控件验证")]
        [Description("设定控件值是否可以为空")]
        [DefaultValue(true)]
        public bool IsNull { get; set; } = true;

        /// <summary>
        ///     设定对应的数据字段是否是主键
        /// </summary>
        [Category("Data")]
        [Description("设定对应的数据字段是否是主键")]
        public bool PrimaryKey { get; set; }

        private bool _readOnly;

        /// <summary>
        ///     是否只读
        /// </summary>
        public bool ReadOnly
        {
            get => _readOnly;
            set
            {
                _readOnly = value;
                Enabled = !_readOnly;
            }
        }

        public bool IsValid => true;


        public void SetValue(object value)
        {
            if (value != null)
                //如果绑定的是实体类列表，Items是实体类集合，此时 value是单值，无法通过这个判断。
                //感谢网友 上海-bingoyin 发现此问题。
                //if (this.Items.Contains(value))
                //{
                SelectedValue = value;
            //}
            //如果 value 是下拉框控件不存在的值，那么赋值之后，SelectedValue 将是 null
        }

        public object GetValue()
        {
            return SelectedValue;
        }

        public virtual bool Validate()
        {
            if (!IsNull)
                if (SelectedValue != null)
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