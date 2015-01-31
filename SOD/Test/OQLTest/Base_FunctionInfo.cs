using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PWMIS.DataMap.Entity;

namespace UPMS.Core.Model
{
    /// <summary>
    /// 实体映射：业务功能表
    /// </summary>
    public class Base_FunctionInfo : EntityBase
    {
        public Base_FunctionInfo()
        {
            TableName = "Base_Function";
            PrimaryKeys.Add("FunctionId");
        }

        protected override void SetFieldNames()
        {
            PropertyNames = new string[] {  "FunctionId", "FunctionName", "Remarks", "ModuleId", "NavigateAddress", "OrderIndex", "DeleteFlag", "ExtendFlag", "ExtendFlagBak" };
        }

        /// <summary>
        /// 功能编号
        /// </summary>
        public string FunctionId
        {
            get { return getProperty<string>("FunctionId"); }
			set { setProperty("FunctionId", value); }
        }

        /// <summary>
        /// 业务功能名称（该名称为左侧导航二级目录名称）
        /// </summary>
        public string FunctionName
        {
            get { return getProperty<string>("FunctionName"); }
			set { setProperty("FunctionName", value); }
        }

        /// <summary>
        /// 备注（主要用于区别同名的功能）
        /// </summary>
        public string Remarks
        {
            get { return getProperty<string>("Remarks"); }
			set { setProperty("Remarks", value); }
        }

        /// <summary>
        /// 业务模块编号（外键）
        /// </summary>
        public string ModuleId
        {
            get { return getProperty<string>("ModuleId"); }
			set { setProperty("ModuleId", value); }
        }

        /// <summary>
        /// 导航地址（如果为空则为无导航）
        /// </summary>
        public string NavigateAddress
        {
            get { return getProperty<string>("NavigateAddress"); }
			set { setProperty("NavigateAddress", value); }
        }

        /// <summary>
        /// 排序（模块内排序）
        /// </summary>
        public int OrderIndex
        {
            get { return getProperty<int>("OrderIndex"); }
			set { setProperty("OrderIndex", value); }
        }

        /// <summary>
        /// 删除标记
        /// </summary>
        public int DeleteFlag
        {
            get { return getProperty<int>("DeleteFlag"); }
			set { setProperty("DeleteFlag", value); }
        }

        /// <summary>
        /// 扩展标记（常用）
        /// </summary>
        public int ExtendFlag
        {
            get { return getProperty<int>("ExtendFlag"); }
			set { setProperty("ExtendFlag", value); }
        }

        /// <summary>
        /// 扩展标记（备用）
        /// </summary>
        public int ExtendFlagBak
        {
            get { return getProperty<int>("ExtendFlagBak"); }
			set { setProperty("ExtendFlagBak", value); }
        }

    }
}

