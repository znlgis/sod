using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PWMIS.DataMap.Entity;

namespace UPMS.Core.Model
{
    /// <summary>
    /// 实体映射：个人功能关系表
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
        /// 编号
        /// </summary>
        public string Id
        {
            get { return getProperty<string>("Id"); }
			set { setProperty("Id", value); }
        }

        /// <summary>
        /// 个人编号（外键）
        /// </summary>
        public string PersonId
        {
            get { return getProperty<string>("PersonId"); }
			set { setProperty("PersonId", value); }
        }

        /// <summary>
        /// 功能编号（外键）
        /// </summary>
        public string FunctionId
        {
            get { return getProperty<string>("FunctionId"); }
			set { setProperty("FunctionId", value); }
        }

        /// <summary>
        /// 方向标记，-1：排除该功能，1：增加该功能
        /// </summary>
        public int DirectionFlag
        {
            get { return getProperty<int>("DirectionFlag"); }
			set { setProperty("DirectionFlag", value); }
        }

    }
}

