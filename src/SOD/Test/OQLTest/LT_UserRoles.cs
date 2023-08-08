using System;
using PWMIS.Common;
using PWMIS.DataMap.Entity;

//using System.Linq;

namespace OQLTest
{
    public class UserRoles : EntityBase
    {
        public UserRoles()
        {
            TableName = "LT_UserRoles";
            EntityMap = EntityMapType.Table;
            //IdentityName = "标识字段名";
            IdentityName = "ID";

            //PrimaryKeys.Add("主键字段名");
            PrimaryKeys.Add("ID");
        }


        /// <summary>
        /// </summary>
        public RoleNames ID
        {
            get => getProperty<RoleNames>("ID");
            set => setProperty("ID", value);
        }

        /// <summary>
        /// </summary>
        public string RoleName
        {
            get => getProperty<string>("RoleName");
            set => setProperty("RoleName", value, 50);
        }

        /// <summary>
        /// </summary>
        public string NickName
        {
            get => getProperty<string>("RoleNickName");
            set => setProperty("RoleNickName", value, 50);
        }

        /// <summary>
        /// </summary>
        public string Description
        {
            get => getProperty<string>("Description");
            set => setProperty("Description", value, 250);
        }

        /// <summary>
        /// </summary>
        public DateTime AddTime
        {
            get => getProperty<DateTime>("AddTime");
            set => setProperty("AddTime", value);
        }


        protected override void SetFieldNames()
        {
            PropertyNames = new[] { "ID", "RoleName", "RoleNickName", "Description", "AddTime" };
        }
    }
}