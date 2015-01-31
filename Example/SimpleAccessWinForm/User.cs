using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PWMIS.DataMap.Entity;

namespace SimpleAccessWinForm
{
    public class User:EntityBase
    {
        public User()
        {
            this.TableName = "会员用户表";
            this.IdentityName = "标识";
            this.PrimaryKeys.Add("标识");
        }

        protected override void SetFieldNames()
        {
            PropertyNames = new string[] {"标识","用户名","用户类型","注册时间","消费金额" };
        }

        public int UserID
        {
            get { return getProperty<int>("标识"); }
            set { setProperty("标识", value); }
        }

        public string UserName
        {
            get { return getProperty<string>("用户名"); }
            set { setProperty("用户名", value, 50); }
        }

        public int UserType
        {
            get { return getProperty<int>("用户类型"); }
            set { setProperty("用户类型", value); }
        }


        public DateTime RegisterDate
        {
            get { return getProperty<DateTime>("注册时间"); }
            set { setProperty("注册时间", value); }
        }

        public Single Expenditure
        {
            get { return getProperty<Single>("消费金额"); }
            set { setProperty("消费金额", value); }
        }
    }
}
