using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityTest
{
    public class UserPartitionEntity : EntityBase, IUser
    {
        public UserPartitionEntity()
        {
            TableName = "Users";
            IdentityName = "User ID";
            PrimaryKeys.Add("User ID");
            Schema = "dbo";
        }

        //重写GetTableName方法后，必须重写SetFieldNames方法，否则可能堆栈溢出
        protected override void SetFieldNames()
        {
            PropertyNames = new string[] { "User ID", "First Name", "Last Name", "Age"};
        }

        //重写GetTableName，实现分表方法
        public override string GetTableName()
        {
            if (this.UserID < 1000)
                return "Users";
            else if (this.UserID < 2000)
                return "Users1000"; //分表
            else
                Schema = "DbPart1].[dbo"; //指定架构分库
            return "Users2000"; 
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
