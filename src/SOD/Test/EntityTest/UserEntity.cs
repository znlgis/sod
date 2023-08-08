using System.Data;
using PWMIS.DataMap.Entity;

namespace EntityTest
{
    /// <summary>
    /// </summary>
    public class UserEntity : EntityBase, IUser
    {
        public UserEntity()
        {
            TableName = "Users";
            IdentityName = "User ID";
            PrimaryKeys.Add("User ID");
        }

        /// <summary>
        ///     用户ID
        /// </summary>
        /// <remarks>
        ///     自增
        /// </remarks>
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
    }
}