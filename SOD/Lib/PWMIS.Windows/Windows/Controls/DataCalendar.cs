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
    [System.Drawing.ToolboxBitmap(typeof(ControlIcon), "DataCalendar.bmp")]
    public partial class DataCalendar : DateTimePicker, IDataControl, IQueryControl
    {

        #region IDataControl 成员
        //[Category("Data"), Description("设定对应的数据源，格式：FullClassName,AssemblyName 。如果需要绑定实体类，可以设置该属性。")]
        //public string DataProvider { get; set; }

        /// <summary>
        /// 设定与数据库字段对应的数据名
        /// </summary>
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
            get {
                return true;
            }
        }

        [Category("Data"), Description("设定对应的数据字段类型"), DefaultValue(TypeCode.String)]
        public TypeCode SysTypeCode
        {
            get;
            set;
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

        public bool IsNull
        {
            get { return true; }
        }

        [Category("Data"), Description("设定对应的数据字段是否是主键")]
        public bool PrimaryKey
        {
            get;
            set;
        }

        string _dataFormatString;
        /// <summary>
        /// 数据呈现格式
        /// </summary>
        [Category("外观"), Description("日期文本数据呈现格式"), DefaultValue("{0:yyyy-MM-dd}")]
        public string DataFormatString
        {
            get
            {
                if (_dataFormatString != null)
                    return _dataFormatString;
                return "{0:yyyy-MM-dd}";
            }
            set
            {
                _dataFormatString = value.Trim();
            }
        }

        public void SetValue(object value)
        {
            if (value != DBNull.Value)
            {
                //邓太华 2006.7.26 修改	日期格式转换
                try
                {
                    if (DataFormatString != "")
                        this.Text = String.Format(DataFormatString, Convert.ToDateTime(value));
                    else
                        this.Text = ((DateTime)value).ToString();
                }
                catch
                {
                    this.Text = "";
                }

            }
            else
            {
                this.Text = "";
            }
        }

        public object GetValue()
        {
            return this.Value;
        }

        public bool Validate()
        {
            throw new NotImplementedException();
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
