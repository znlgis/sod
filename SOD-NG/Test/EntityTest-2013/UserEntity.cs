using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityTest
{
    public class UserEntity:EntityBase, IUser
    {
        public UserEntity()
        {
            TableName = "Users";
            IdentityName = "User ID";
            PrimaryKeys.Add("User ID");
        }

        public int UserID
        {
            get { return getProperty<int>("User ID"); }
            set { setProperty("User ID", value); }
        }

        public string FirstName
        {
            get { return getProperty<string>("First Name"); }
            set { setProperty("First Name", value,20); }
        }

        public string LasttName
        {
            get { return getProperty<string>("Last Name"); }
            set { setProperty("Last Name", value,10); }
        }

        public int Age
        {
            get { return getProperty<int>("Age"); }
            set { setProperty("Age", value); }
        }
    }
}
