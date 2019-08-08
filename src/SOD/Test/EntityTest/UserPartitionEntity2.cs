using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityTest
{
    public class UserPartitionEntity2 : EntityBase, IUser, IDBPartition
    {
        public UserPartitionEntity2()
        {
            TableName = "Users";
            IdentityName = "User ID";
            PrimaryKeys.Add("User ID");
            //Schema = "dbo";
        }

        //重写GetTableName方法后，必须重写SetFieldNames方法，否则可能堆栈溢出
        protected override void SetFieldNames()
        {
            PropertyNames = new string[] { "User ID", "First Name", "Last Name", "Age" };
        }

        //不重写GetTableName，直接分库来分表
        //public override string GetTableName()
        //{
        //    return "Users";
        //}

        public string GetDatabaseName()
        {
            if (this.UserID < 1000)
                return "UserDB1";
            else if (this.UserID < 2000)
                return "UserDB2"; 
            else
                return "UserDB3"; 
        }

        public string GetServerName()
        {
            return "localhost";
        }

        public int UserID
        {
            get { return getProperty<int>("User ID"); }
            set { setProperty("User ID", value); }
        }

        //指定 DbType.StringFixedLengt 类型，将对应 nchar 字段类型
        public string FirstName
        {
            get { return getProperty<string>("First Name"); }
            set { setProperty("First Name", value, 20, System.Data.DbType.StringFixedLength); }
        }

        public string LasttName
        {
            get { return getProperty<string>("Last Name"); }
            set { setProperty("Last Name", value, 10); }
        }

        public int Age
        {
            get { return getProperty<int>("Age"); }
            set { setProperty("Age", value); }
        }
    }
}
