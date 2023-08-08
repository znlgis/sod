using System;
using PWMIS.Common;
using PWMIS.DataMap.Entity;

//using System.Linq;

namespace OQLTest
{
    public enum RoleNames
    {
        User = 1,
        Manager = 2,
        Admin = 3
    }

    public class Users : EntityBase
    {
        //用下面的方式处理实体类的子实体类问题
        private UserRoles _roles;

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

        /// <summary>
        /// </summary>
        public int ID
        {
            get => getProperty<int>("ID");
            set => setProperty("ID", value);
        }

        /// <summary>
        /// </summary>
        public string UserName
        {
            get => getProperty<string>("UserName");
            set => setProperty("UserName", value, 50);
        }

        /// <summary>
        /// </summary>
        public string Password
        {
            get => getProperty<string>("Password");
            set => setProperty("Password", value, 50);
        }

        /// <summary>
        /// </summary>
        public string NickName
        {
            get => getProperty<string>("NickName");
            set => setProperty("NickName", value, 50);
        }

        /// <summary>
        /// </summary>
        public RoleNames RoleID
        {
            get => getProperty<RoleNames>("RoleID");
            set => setProperty("RoleID", value);
        }

        public UserRoles Roles
        {
            get
            {
                if (_roles == null)
                {
                    _roles = new UserRoles { ID = RoleID };
                    EntityQuery<UserRoles>.Fill(_roles);
                }

                return _roles;
            }
        }

        /// <summary>
        /// </summary>
        public string Authority
        {
            get => getProperty<string>("Authority");
            set => setProperty("Authority", value, 250);
        }

        /// <summary>
        /// </summary>
        public bool IsEnable
        {
            get => getProperty<bool>("IsEnable");
            set => setProperty("IsEnable", value);
        }

        /// <summary>
        /// </summary>
        public DateTime? LastLoginTime
        {
            get => getProperty<DateTime?>("LastLoginTime");
            set => setProperty("LastLoginTime", value);
        }

        /// <summary>
        /// </summary>
        public string LastLoginIP
        {
            get => getProperty<string>("LastLoginIP");
            set => setProperty("LastLoginIP", value, 20);
        }

        /// <summary>
        /// </summary>
        public string Remarks
        {
            get => getProperty<string>("Remarks");
            set => setProperty("Remarks", value, 150);
        }

        /// <summary>
        /// </summary>
        public DateTime? AddTime
        {
            get => getProperty<DateTime?>("AddTime");
            set => setProperty("AddTime", value);
        }


        protected override void SetFieldNames()
        {
            PropertyNames = new[]
            {
                "ID", "UserName", "Password", "NickName", "RoleID", "Authority", "IsEnable", "LastLoginIP", "Remarks",
                "AddTime"
            };
        }

        public override string GetTableName()
        {
            if (ID < 100000)
                return "LT_Users";
            return "LT_Users_01";
        }

        //public int Age
        //{
        //    get { return getProperty<int>("Age"); }
        //    set { setProperty("Age", value); }
        //}
    }
}