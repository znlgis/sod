using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PWMIS.DataMap.Entity;

namespace UPMS.Core.Model
{
    /// <summary>
    /// 实体映射：个人_角色关系表
    /// </summary>
    public class Base_Person_RoleInfo : EntityBase
    {
        public Base_Person_RoleInfo()
        {
            TableName = "Base_Person_Role";
            PrimaryKeys.Add("Id");
        }

        protected override void SetFieldNames()
        {
            PropertyNames = new string[] {  "Id", "PersonId", "RoleId" };
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
        /// 个人编号
        /// </summary>
        public string PersonId
        {
            get { return getProperty<string>("PersonId"); }
			set { setProperty("PersonId", value); }
        }

        /// <summary>
        /// 角色编号
        /// </summary>
        public int RoleId
        {
            get { return getProperty<int>("RoleId"); }
			set { setProperty("RoleId", value); }
        }

    }
}

