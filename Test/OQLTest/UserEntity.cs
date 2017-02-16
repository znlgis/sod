using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OQLTest
{
    class UserEntity:EntityBase
    {
        public UserEntity()
        {
            TableName = "User";
            //PrimaryKeys.Add("主键字段名");
            PrimaryKeys.Add("ID");
        }

        public int ID
        {
            get { return getProperty<int>("Id"); }
            set { setProperty("Id", value); }
        }

        public int Name
        {
            get { return getProperty<int>("User Name"); }
            set { setProperty("User Name", value,50); }
        }

        public int Age
        {
            get { return getProperty<int>("Age"); }
            set { setProperty("Age", value); }
        }

    }
}
