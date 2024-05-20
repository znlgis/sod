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
        /// 
        /// </summary>
        public string Id
        {
            get { return getProperty<string>("Id"); }
			set { setProperty("Id", value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PersonId
        {
            get { return getProperty<string>("PersonId"); }
			set { setProperty("PersonId", value); }
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
        public int DirectionFlag
        {
            get { return getProperty<int>("DirectionFlag"); }
			set { setProperty("DirectionFlag", value); }
        }

    }
}

