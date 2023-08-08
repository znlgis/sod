using System;
using System.Collections.Generic;
using PWMIS.Common;
using PWMIS.DataMap.Entity;

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
        /// </summary>
        public int ID
        {
            get => getProperty<int>("ID");
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

        //关联的实体类集合
        public List<LT_Users> Users { get; set; }
    }
}