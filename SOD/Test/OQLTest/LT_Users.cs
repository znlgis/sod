using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using PWMIS.DataMap.Entity;
using PWMIS.Common;

namespace OQLTest
{
    public enum RoleNames
    { 
        User,
        Manager,
        Admin
    }
    public partial class Users : EntityBase
    {
        public Users()
        {
            TableName = "LT_Users";
            //TableName = GetTableName();
            EntityMap = EntityMapType.Table;
            //IdentityName = "标识字段名";
            IdentityName = "ID";

            //PrimaryKeys.Add("主键字段名");
            PrimaryKeys.Add("ID");


        }


        protected override void SetFieldNames()
        {
            PropertyNames = new string[] { "ID", "UserName", "Password", "NickName", "RoleID", "Authority", "IsEnable", "LastLoginTime", "LastLoginIP", "Remarks", "AddTime" };
        }

        public override string GetTableName()
        {
            if (this.ID < 100000)
                return "LT_Users";
            else
                return "LT_Users_01";
        }

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
        public System.String UserName
        {
            get { return getProperty<System.String>("UserName"); }
            set { setProperty("UserName", value, 50); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String Password
        {
            get { return getProperty<System.String>("Password"); }
            set { setProperty("Password", value, 50); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String NickName
        {
            get { return getProperty<System.String>("NickName"); }
            set { setProperty("NickName", value, 50); }
        }

        /// <summary>
        /// 
        /// </summary>
        public RoleNames RoleID
        {
            get { return getProperty<RoleNames>("RoleID"); }
            set { setProperty("RoleID", value); }
        }

        //用下面的方式处理实体类的子实体类问题
        UserRoles _roles;
        public UserRoles Roles
        {
            get
            {
                if (_roles == null)
                {
                    _roles = new UserRoles() { ID = this.RoleID };
                    EntityQuery<UserRoles>.Fill(_roles);
                }
                return _roles;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String Authority
        {
            get { return getProperty<System.String>("Authority"); }
            set { setProperty("Authority", value, 250); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Boolean IsEnable
        {
            get { return getProperty<System.Boolean>("IsEnable"); }
            set { setProperty("IsEnable", value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? LastLoginTime
        {
            get { return getProperty<System.DateTime?>("LastLoginTime"); }
            set { setProperty("LastLoginTime", value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String LastLoginIP
        {
            get { return getProperty<System.String>("LastLoginIP"); }
            set { setProperty("LastLoginIP", value, 20); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String Remarks
        {
            get { return getProperty<System.String>("Remarks"); }
            set { setProperty("Remarks", value, 150); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? AddTime
        {
            get { return getProperty<System.DateTime?>("AddTime"); }
            set { setProperty("AddTime", value); }
        }

    }

}
