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
            //如果未实现SetFieldNames方法，不可以在构造函数中调用持久化属性，否则会导致递归调用错误，例如：
            this.AtTime = DateTime.Now;
        }

        
        protected override void SetFieldNames()
        {
            PropertyNames = new string[] { "ID","Name", "AtTime" };
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
