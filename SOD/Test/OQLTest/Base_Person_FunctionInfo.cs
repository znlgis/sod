using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PWMIS.DataMap.Entity;

namespace UPMS.Core.Model
{
    /// <summary>
    /// 瀹炰綋鏄犲皠锛氫釜浜哄姛鑳藉叧绯昏〃
    /// </summary>
    public class Base_Person_FunctionInfo : EntityBase
    {
        public Base_Person_FunctionInfo()
        {
            TableName = "Base_Person_Function";
            PrimaryKeys.Add("Id");
        }

        protected override void SetFieldNames()
        {
            PropertyNames = new string[] {  "Id", "PersonId", "FunctionId", "DirectionFlag" };
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
        /// 涓汉缂栧彿锛堝閿級
        /// </summary>
        public string PersonId
        {
            get { return getProperty<string>("PersonId"); }
			set { setProperty("PersonId", value); }
        }

        /// <summary>
        /// 鍔熻兘缂栧彿锛堝閿級
        /// </summary>
        public string FunctionId
        {
            get { return getProperty<string>("FunctionId"); }
			set { setProperty("FunctionId", value); }
        }

        /// <summary>
        /// 鏂瑰悜鏍囪锛?1锛氭帓闄よ鍔熻兘锛?锛氬鍔犺鍔熻兘
        /// </summary>
        public int DirectionFlag
        {
            get { return getProperty<int>("DirectionFlag"); }
			set { setProperty("DirectionFlag", value); }
        }

    }
}

