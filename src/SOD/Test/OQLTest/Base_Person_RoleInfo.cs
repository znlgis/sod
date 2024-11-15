﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PWMIS.DataMap.Entity;

namespace UPMS.Core.Model
{
    /// <summary>
    /// 
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
        public int RoleId
        {
            get { return getProperty<int>("RoleId"); }
			set { setProperty("RoleId", value); }
        }

    }
}

