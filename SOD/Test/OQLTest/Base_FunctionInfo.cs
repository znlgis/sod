using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PWMIS.DataMap.Entity;

namespace UPMS.Core.Model
{
    /// <summary>
    /// 瀹炰綋鏄犲皠锛氫笟鍔″姛鑳借〃
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
        /// 鍔熻兘缂栧彿
        /// </summary>
        public string FunctionId
        {
            get { return getProperty<string>("FunctionId"); }
			set { setProperty("FunctionId", value); }
        }

        /// <summary>
        /// 涓氬姟鍔熻兘鍚嶇О锛堣鍚嶇О涓哄乏渚у鑸簩绾х洰褰曞悕绉帮級
        /// </summary>
        public string FunctionName
        {
            get { return getProperty<string>("FunctionName"); }
			set { setProperty("FunctionName", value); }
        }

        /// <summary>
        /// 澶囨敞锛堜富瑕佺敤浜庡尯鍒悓鍚嶇殑鍔熻兘锛?
        /// </summary>
        public string Remarks
        {
            get { return getProperty<string>("Remarks"); }
			set { setProperty("Remarks", value); }
        }

        /// <summary>
        /// 涓氬姟妯″潡缂栧彿锛堝閿級
        /// </summary>
        public string ModuleId
        {
            get { return getProperty<string>("ModuleId"); }
			set { setProperty("ModuleId", value); }
        }

        /// <summary>
        /// 瀵艰埅鍦板潃锛堝鏋滀负绌哄垯涓烘棤瀵艰埅锛?
        /// </summary>
        public string NavigateAddress
        {
            get { return getProperty<string>("NavigateAddress"); }
			set { setProperty("NavigateAddress", value); }
        }

        /// <summary>
        /// 鎺掑簭锛堟ā鍧楀唴鎺掑簭锛?
        /// </summary>
        public int OrderIndex
        {
            get { return getProperty<int>("OrderIndex"); }
			set { setProperty("OrderIndex", value); }
        }

        /// <summary>
        /// 鍒犻櫎鏍囪
        /// </summary>
        public int DeleteFlag
        {
            get { return getProperty<int>("DeleteFlag"); }
			set { setProperty("DeleteFlag", value); }
        }

        /// <summary>
        /// 鎵╁睍鏍囪锛堝父鐢級
        /// </summary>
        public int ExtendFlag
        {
            get { return getProperty<int>("ExtendFlag"); }
			set { setProperty("ExtendFlag", value); }
        }

        /// <summary>
        /// 鎵╁睍鏍囪锛堝鐢級
        /// </summary>
        public int ExtendFlagBak
        {
            get { return getProperty<int>("ExtendFlagBak"); }
			set { setProperty("ExtendFlagBak", value); }
        }

    }
}

