using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityTest
{
    public class UserLnag : EntityBase
    {
        public UserLnag()
        {
            TableName = "UsersLang";
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

        /// <summary>
        /// 关联主表的ID
        /// </summary>
        public int RelateId
        {
            get { return getProperty<int>("iRelateId"); }
            set { setProperty("iRelateId", value); }
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
    }

}
