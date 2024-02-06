using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityTest
{
    /// <summary>
    /// 
    /// </summary>
    public class UserEntity:EntityBase, IUser
    {
        public UserEntity()
        {
            TableName = "Users";
            IdentityName = "User ID";
            PrimaryKeys.Add("User ID");
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        /// <remarks>
        /// 自增
        /// </remarks>
        public int UserID
        {
            get { return getProperty<int>("User ID"); }
            set { setProperty("User ID", value); }
        }

        //指定 DbType.StringFixedLengt 类型，将对应 nchar 字段类型
        public string FirstName
        {
            get { return getProperty<string>("First Name"); }
            set { setProperty("First Name", value,20,System.Data.DbType.StringFixedLength); }
        }

        public string LasttName
        {
            get { return getProperty<string>("Last Name"); }
            set { setProperty("Last Name", value,10); }
        }

        public int Age
        {
            get { return getProperty<int>("Age"); }
            set { setProperty("Age", value); }
        }
    }
}
