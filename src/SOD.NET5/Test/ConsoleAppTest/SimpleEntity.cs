using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppTest
{
    class SimpleEntity:EntityBase
    {
        public SimpleEntity()
        {
            Meta = EntityMetaData.SharedMeta<SimpleEntity>(meta=> {
                meta.TableName = "Table_1";
                meta.IdentityName = "ID";
                meta.AddPrimaryKey("ID");
            });
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

    }
}
