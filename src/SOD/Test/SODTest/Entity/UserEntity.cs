using System;
using PWMIS.DataMap.Entity;

namespace SampleORMTest
{
    public class User : EntityBase
    {
        public User()
        {
            TableName = "Tb_User1";

            IdentityName = "UserID";
            PrimaryKeys.Add("UserID");
        }

        public int ID
        {
            get => getProperty<int>("UserID");
            set => setProperty("UserID", value);
        }

        public string Name
        {
            get => getProperty<string>("Name");
            set => setProperty("Name", value, 50);
        }

        public string Pwd
        {
            get => getProperty<string>("Pwd");
            set => setProperty("Pwd", value, 50);
        }

        public DateTime RegistedDate
        {
            get => getProperty<DateTime>("RegistedDate");
            set => setProperty("RegistedDate", value);
        }

        /// <summary>
        ///     设置字段名数组，如果不实现该方法，框架会自动反射获取到字段名数组，因此从效率考虑，建议实现该方法
        /// </summary>
        protected override void SetFieldNames()
        {
            PropertyNames = new[] { "UserID", "Name", "Pwd", "RegistedDate" };
        }

        /// <summary>
        ///     获取实体类全局唯一标识；重写该方法，可以加快访问效率
        /// </summary>
        /// <returns></returns>
        public override string GetGolbalEntityID()
        {
            //使用工具-》创建GUID 生成
            return "F1344072-AB1E-4BCF-A28C-769C7C4AA06B";
        }
    }

    public class UserDto
    {
        public int UserID { get; set; }

        public string Name { get; set; }

        public string Pwd { get; set; }

        public DateTime RegistedDate { get; set; }
    }
}