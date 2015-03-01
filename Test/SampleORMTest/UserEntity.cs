using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleORMTest
{
    public class User : EntityBase
    {
        public User()
        {
            TableName="Tb_User";
            IdentityName = "UserID";
            PrimaryKeys.Add("UserID");
        }

        public int ID
        {
            get { return getProperty<int>("UserID"); }
            set { setProperty("UserID", value); }
        }

        public string Name
        {
            get { return getProperty<string>("Name"); }
            set { setProperty("Name", value, 50); }
        }

        public string Pwd
        {
            get { return getProperty<string>("Pwd"); }
            set { setProperty("Pwd", value, 50); }
        }

        public DateTime RegistedDate
        {
            get { return getProperty<DateTime>("RegistedDate"); }
            set { setProperty("RegistedDate", value); }
        }

    }
}
