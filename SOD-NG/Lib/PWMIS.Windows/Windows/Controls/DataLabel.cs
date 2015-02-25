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
    /// <summary>
    /// 数据标签控件
    /// </summary>
    [System.Drawing.ToolboxBitmap(typeof(ControlIcon), "DataLable.bmp")]
    public class DataLabel : Label, IDataTextBox
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
            get { return true; }
        }

        private TypeCode _sysTypeCode = TypeCode.String;

        /// <summary>
        /// 数据类型，默认是String
        /// </summary>
        [Category("Data"), Description("设定对应的数据字段类型"), DefaultValue(TypeCode.String)]
        public TypeCode SysTypeCode
        {
            get { return _sysTypeCode; }
            set { _sysTypeCode = value; }
        }

        public bool ReadOnly
        {
            get
            {
                return true;
            }
            set
            {
               
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

        public string DataFormatString 
        { 
            get; 
            set; 
        }

        public void SetValue(object value)
        {
            DataTextBoxValue dtbv = new DataTextBoxValue(this);
            dtbv.SetValue(value);
        }

        public object GetValue()
        {
            DataTextBoxValue dtbv = new DataTextBoxValue(this);
            return dtbv.GetValue();
        }

        public bool Validate()
        {
            throw new NotImplementedException();
        }

        public int MaxLength
        {
            get;
            set;
        }
        #endregion

        
    }
}
