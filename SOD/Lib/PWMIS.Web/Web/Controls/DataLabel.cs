/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap 
 * ========================================================================
 * 该类的作用
 * 
 * 作者：邓太华     时间：2008-10-12
 * 版本：V3.0
 * 
 * 修改者：         时间：2013-3-1                
 * 修改说明：完善了控件
 * ========================================================================
*/
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using PWMIS.Common;
using PWMIS.DataMap;
using System.Drawing.Design;


namespace PWMIS.Web.Controls
{
    /// <summary>
    /// Summary description for BrainLabel.
    /// </summary>
    [System.Drawing.ToolboxBitmap(typeof(ControlIcon), "DataLable.bmp")]
    public class DataLabel : Label, IDataTextBox
    {
        public DataLabel()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region 外观属性

        /// <summary>
        /// 数据呈现格式
        /// </summary>
        [Category("外观"), Description("数据呈现格式")]
        public string DataFormatString
        {
            get
            {
                if (ViewState["DataFormatString"] != null)
                    return (string)ViewState["DataFormatString"];
                return "";
            }
            set
            {
                ViewState["DataFormatString"] = value;
            }
        }

        #endregion

        #region IBrainControl 成员

        #region 数据属性
        [Category("Data"), Description("设定对应的数据源，格式：FullClassName,AssemblyName 。如果需要绑定实体类，可以设置该属性。")]
        public string DataProvider { get; set; }

        [Category("Data"), Description("设定对应的数据库字段是否是主键，用于自动数据查询和更新的依据")]
        public bool PrimaryKey
        {
            get
            {
                if (ViewState["PrimaryKey"] != null)
                    return (bool)ViewState["PrimaryKey"];
                return false;
            }
            set
            {
                ViewState["PrimaryKey"] = value;
            }
        }

        [Category("Data"), Description("设定对应的数据字段类型")]
        public System.TypeCode SysTypeCode
        {
            get
            {
                if (ViewState["SysTypeCode"] != null)
                    return (System.TypeCode)ViewState["SysTypeCode"];
                return new System.TypeCode();
            }
            set
            {
                ViewState["SysTypeCode"] = value;
            }
        }

        [Category("Data"), Description("设定与数据库字段对应的数据名")]
        [Editor(typeof(PropertyUITypeEditor), typeof(UITypeEditor))]
        public string LinkProperty
        {
            get
            {
                if (ViewState["LinkProperty"] != null)
                    return (string)ViewState["LinkProperty"];
                return "";
            }
            set
            {
                ViewState["LinkProperty"] = value;
            }
        }

        [Category("Data"), Description("设定与数据库字段对应的数据表名")]
        public string LinkObject
        {
            get
            {
                if (ViewState["LinkObject"] != null)
                    return (string)ViewState["LinkObject"];
                return "";
            }
            set
            {
                ViewState["LinkObject"] = value;
            }
        }

        #endregion

        #region 接口方法

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


        public virtual bool Validate()
        {

            return true;
        }

        #endregion

        #region 默认属性

        public bool isClientValidation
        {
            get
            {

                return false;
            }
        }

        public bool IsNull
        {

            get
            {
                return true;
            }
            set
            {

            }
        }

        public bool IsValid
        {
            get
            {
                return Validate();
            }
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

        public int MaxLength
        {
            get;
            set;
        }
        #endregion

        #endregion
    }
}
