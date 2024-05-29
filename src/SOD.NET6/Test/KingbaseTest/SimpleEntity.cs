using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace KingbaseTest
{
    class SimpleEntity:EntityBase
    {
        public SimpleEntity()
        {
            TableName = "SimpleTable6";
            IdentityName = "ID";
            PrimaryKeys.Add("ID");
        }

        public int ID 
        {
            get { return getProperty<int>("ID"); }
            set { setProperty("ID", value); }
        }

        public string Name
        {
            get { return getProperty<string>( nameof( Name)); }
            set { setProperty(nameof(Name), value,100); }
        }

        public DateTime AtTime
        {
            get { return getProperty<DateTime>(nameof(AtTime)); }
            set { setProperty(nameof(AtTime), value); }
        }
    }
}
