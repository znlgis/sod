using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleORMTest
{
    public class User : EntityBase
    {
        public User()
        {
            TableName="Tb_User";
            IdentityName = "UserID";
            PrimaryKeys.Add("UserID");
        }

        /// <summary>
        /// 设置字段名数组，如果不实现该方法，框架会自动反射获取到字段名数组，因此从效率考虑，建议实现该方法
        /// </summary>
        protected override void SetFieldNames()
        {
            PropertyNames = new string[] { "UserID", "Name", "Pwd", "RegistedDate" };
        }

        /// <summary>
        /// 获取实体类全局唯一标识；重写该方法，可以加快访问效率
        /// </summary>
        /// <returns></returns>
        public override string GetGolbalEntityID()
        {
            //使用工具-》创建GUID 生成
            return "F1344072-AB1E-4BCF-A28C-769C7C4AA06A";
        }

        public int ID
        {
            get { return getProperty<int>("UserID"); }
            set { setProperty("UserID", value); }
        }

        public string Name
        {
            get { return getProperty<string>("Name"); }
            set { setProperty("Name", value, 50); }
        }

        public string Pwd
        {
            get { return getProperty<string>("Pwd"); }
            set { setProperty("Pwd", value, 4000); }
        }

        public DateTime RegistedDate
        {
            get { return getProperty<DateTime>("RegistedDate"); }
            set { setProperty("RegistedDate", value); }
        }

    }
}
