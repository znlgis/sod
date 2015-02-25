/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap 
 * ========================================================================
 * 该类的作用
 * 
 * 作者：邓太华     时间：2008-10-12
 * 版本：V4.5
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


namespace PWMIS.Web.Controls
{
    /// <summary>
    /// BrainCheckBox 的摘要说明。2008.7.6
    /// </summary>
    [System.Drawing.ToolboxBitmap(typeof(ControlIcon), "DataCheckBox.bmp")]
    public class DataCheckBox : CheckBox, IDataCheckBox, IQueryControl
    {
        public DataCheckBox()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }



        #region IBrainControl 成员

        #region 数据属性
        [Category("Data"), Description("设定对应的数据源，格式：FullClassName,AssemblyName 。如果需要绑定实体类，可以设置该属性。")]
        public string DataProvider { get; set; }
        /// <summary>
        /// 指定是否是主键
        /// </summary>
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

        [Category("Data"), Description("设定与数据库字段对应的数据值")]
        public string Value
        {
            get
            {
                if (ViewState["CheckBoxvalue"] != null)
                    return (string)ViewState["CheckBoxvalue"];
                return "";
            }
            set
            {
                ViewState["CheckBoxvalue"] = value;
            }
        }

        [Category("Data"), Description("设定与数据库字段对应的数据名")]
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

        [Category("Data"), Description("设定与数据表对应的数据表名")]
        public string LinkObject
        {
            get
            {
                if (ViewState["LinkObject"] != null)
                    return (string)ViewState["LinkObject"];
                return "";
                // TODO:  添加 BrainDropDownList.LinkObject getter 实现
            }
            set
            {
                ViewState["LinkObject"] = value;
                // TODO:  添加 BrainDropDownList.LinkObject setter 实现
            }
        }

        #endregion

        #region 默认属性

        public bool IsValid
        {
            get
            {
                // TODO:  添加 BrainListBox.IsValid getter 实现
                return true;
            }
        }
        public System.TypeCode SysTypeCode
        {
            get
            {
                if (ViewState["SysTypeCode"] != null)
                    return (System.TypeCode)ViewState["SysTypeCode"];
                return System.TypeCode.String;
            }
            set
            {
                ViewState["SysTypeCode"] = value;
            }
        }

        public virtual bool Validate()
        {
            // TODO:  添加 BrainListBox.Validate 实现
            return true;
        }
        #endregion

        #region 其他属性

        public bool ReadOnly
        {
            get
            {
                if (!this.Checked)//如果未选择，那么设定为只读数据属性。
                    return true;
                return !base.Enabled;
            }
            set
            {
                base.Enabled = !value;
            }
        }

        [Category("Data"), Description("设定与数据是否必添")]
        public bool IsNull
        {

            get
            {
                // TODO:  添加 BrainTextBox.isClientValidation getter 实现
                if (ViewState["isNull"] != null)
                    return (bool)ViewState["isNull"];
                return true;
            }
            set
            {
                // TODO:  添加 BrainTextBox.isClientValidation setter 实现
                ViewState["isNull"] = value;

            }
        }
        #endregion

        #region 接口方法

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
        #endregion

        #region IQueryControl 成员

        public string CompareSymbol
        {
            get
            {
                if (ViewState["CompareSymbol"] != null)
                    return (string)ViewState["CompareSymbol"];
                return "";
            }
            set
            {
                ViewState["CompareSymbol"] = value;
            }
        }

        public string QueryFormatString
        {
            get
            {
                if (ViewState["QueryFormatString"] != null)
                    return (string)ViewState["QueryFormatString"];
                return "";
            }
            set
            {
                ViewState["QueryFormatString"] = value;
            }
        }

        #endregion


        #endregion
    }
}
