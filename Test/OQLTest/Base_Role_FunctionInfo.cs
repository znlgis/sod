using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PWMIS.DataMap.Entity;

namespace UPMS.Core.Model
{
    /// <summary>
    /// 瀹炰綋鏄犲皠锛氳鑹插姛鑳藉叧绯昏〃
    /// </summary>
    public class Base_Role_FunctionInfo : EntityBase
    {
        public Base_Role_FunctionInfo()
        {
            TableName = "Base_Role_Function";
            PrimaryKeys.Add("Id");
        }

        protected override void SetFieldNames()
        {
            PropertyNames = new string[] {  "Id", "RoleId", "FunctionId" };
        }

        /// <summary>
        /// 缂栧彿
        /// </summary>
        public string Id
        {
            get { return getProperty<string>("Id"); }
			set { setProperty("Id", value); }
        }

        /// <summary>
        /// 瑙掕壊缂栧彿
        /// </summary>
        public int RoleId
        {
            get { return getProperty<int>("RoleId"); }
			set { setProperty("RoleId", value); }
        }

        /// <summary>
        /// 鍔熻兘缂栧彿
        /// </summary>
        public string FunctionId
        {
            get { return getProperty<string>("FunctionId"); }
			set { setProperty("FunctionId", value); }
        }

    }
}

