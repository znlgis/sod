using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppTest
{
    class OldSimpleEntity:EntityBase
    {
        public OldSimpleEntity()
        {
            Meta= new EntityMetaData();
            TableName = "Table_1";
            IdentityName  = "ID";
            PrimaryKeys.Add("ID");
        }


        public int ID
        {
            get { return getProperty<int>("ID"); }
            set { setProperty("ID", value); }
        }

        public string Name
        {
            get { return getProperty<string>(nameof(Name)); }
            set { setProperty(nameof(Name), value, 100); }
        }
    }
}
