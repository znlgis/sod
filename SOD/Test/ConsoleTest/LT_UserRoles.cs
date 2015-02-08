using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PWMIS.DataMap.Entity;
using PWMIS.Common;

namespace ConsoleTest
{
    public class LT_UserRoles : EntityBase
    {
   public LT_UserRoles()
        {
            TableName = "LT_UserRoles";
            EntityMap = EntityMapType.Table;
            //IdentityName = "标识字段名";
            IdentityName = "ID";

            //PrimaryKeys.Add("主键字段名");
            PrimaryKeys.Add("ID");


        }


        //protected override void SetFieldNames()
        //{
        //    PropertyNames = new string[] { "ID", "RoleName", "RoleNickName", "Description", "AddTime" };
        //}



        /// <summary>
        /// 
        /// </summary>
        public System.Int32 ID
        {
            get { return getProperty<System.Int32>("ID"); }
            set { setProperty("ID", value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String RoleName
        {
            get { return getProperty<System.String>("RoleName"); }
            set { setProperty("RoleName", value, 50); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String NickName
        {
            get { return getProperty<System.String>("RoleNickName"); }
            set { setProperty("RoleNickName", value, 50); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String Description
        {
            get { return getProperty<System.String>("Description"); }
            set { setProperty("Description", value, 250); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.DateTime AddTime
        {
            get { return getProperty<System.DateTime>("AddTime"); }
            set { setProperty("AddTime", value); }
        }

        //关联的实体类集合
        public List<LT_Users> Users
        {
            get;
            set;
        }

    }

}
