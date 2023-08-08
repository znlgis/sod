using System;
using PWMIS.DataMap.Entity;

namespace SimpleAccessWinForm
{
    public class User : EntityBase
    {
        public User()
        {
            TableName = "会员用户表";
            IdentityName = "标识";
            PrimaryKeys.Add("标识");
        }

        public int UserID
        {
            get => getProperty<int>("标识");
            set => setProperty("标识", value);
        }

        public string UserName
        {
            get => getProperty<string>("用户名");
            set => setProperty("用户名", value, 50);
        }

        public int UserType
        {
            get => getProperty<int>("用户类型");
            set => setProperty("用户类型", value);
        }


        public DateTime RegisterDate
        {
            get => getProperty<DateTime>("注册时间");
            set => setProperty("注册时间", value);
        }

        public float Expenditure
        {
            get => getProperty<float>("消费金额");
            set => setProperty("消费金额", value);
        }

        protected override void SetFieldNames()
        {
            PropertyNames = new[] { "标识", "用户名", "用户类型", "注册时间", "消费金额" };
        }
    }
}