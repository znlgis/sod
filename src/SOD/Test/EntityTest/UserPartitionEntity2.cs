using System.Data;
using PWMIS.DataMap.Entity;

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

        //不重写GetTableName，直接分库来分表
        //public override string GetTableName()
        //{
        //    return "Users";
        //}

        public string GetDatabaseName()
        {
            if (UserID < 1000)
                return "UserDB1";
            if (UserID < 2000)
                return "UserDB2";
            return "UserDB3";
        }

        public string GetServerName()
        {
            return "localhost";
        }

        public int UserID
        {
            get => getProperty<int>("User ID");
            set => setProperty("User ID", value);
        }

        //指定 DbType.StringFixedLengt 类型，将对应 nchar 字段类型
        public string FirstName
        {
            get => getProperty<string>("First Name");
            set => setProperty("First Name", value, 20, DbType.StringFixedLength);
        }

        public string LasttName
        {
            get => getProperty<string>("Last Name");
            set => setProperty("Last Name", value, 10);
        }

        public int Age
        {
            get => getProperty<int>("Age");
            set => setProperty("Age", value);
        }

        //重写GetTableName方法后，必须重写SetFieldNames方法，否则可能堆栈溢出
        protected override void SetFieldNames()
        {
            PropertyNames = new[] { "User ID", "First Name", "Last Name", "Age" };
        }
    }
}