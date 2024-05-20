using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PWMIS.DataMap.Entity;

namespace UPMS.Core.Model
{
    /// <summary>
    /// 
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
        /// 
        /// </summary>
        public string FunctionId
        {
            get { return getProperty<string>("FunctionId"); }
			set { setProperty("FunctionId", value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FunctionName
        {
            get { return getProperty<string>("FunctionName"); }
			set { setProperty("FunctionName", value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Remarks
        {
            get { return getProperty<string>("Remarks"); }
			set { setProperty("Remarks", value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ModuleId
        {
            get { return getProperty<string>("ModuleId"); }
			set { setProperty("ModuleId", value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string NavigateAddress
        {
            get { return getProperty<string>("NavigateAddress"); }
			set { setProperty("NavigateAddress", value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public int OrderIndex
        {
            get { return getProperty<int>("OrderIndex"); }
			set { setProperty("OrderIndex", value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public int DeleteFlag
        {
            get { return getProperty<int>("DeleteFlag"); }
			set { setProperty("DeleteFlag", value); }
        }

        /// <summary>
        ///
        /// </summary>
        public int ExtendFlag
        {
            get { return getProperty<int>("ExtendFlag"); }
			set { setProperty("ExtendFlag", value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ExtendFlagBak
        {
            get { return getProperty<int>("ExtendFlagBak"); }
			set { setProperty("ExtendFlagBak", value); }
        }

    }
}

