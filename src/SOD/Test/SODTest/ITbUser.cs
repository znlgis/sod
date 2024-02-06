using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SODTest
{
    //接口类型必须是公开类型，否则无法创建匿名实现类

    public interface ITbUser
    {
        int ID { get; set; }
        string Name { get; set; }
        string LoginName { get; set; }
        string Password { get; set; }
        bool Sex { get; set; }
        DateTime BirthDate { get; set; }
    }

    public class UserInfo: ITbUser
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public bool Sex { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public class UserEntity : EntityBase, ITbUser
    {
        public UserEntity()
        {
            TableName = "TbUser";
            IdentityName = "ID";
            PrimaryKeys.Add("ID");
        }

        public int ID 
        {
            get { return getProperty<int>("ID"); }
            set { setProperty("ID", value); }
        }
        public string Name
        {
            get { return getProperty<string>("Name"); }
            set { setProperty("Name", value,100); } //长度 100
        }
        public string LoginName
        {
            get { return getProperty<string>("LoginName"); }
            set { setProperty("LoginName", value,50); }
        }
        public string Password
        {
            get { return getProperty<string>("Password"); }
            set { setProperty("Password", value,50); }
        }
        public bool Sex
        {
            get { return getProperty<bool>("Sex"); }
            set { setProperty("Sex", value); }
        }
        public DateTime BirthDate
        {
            get { return getProperty<DateTime>("BirthDate"); }
            set { setProperty("BirthDate", value); }
        }
    }

    public class UserEntity2 : EntityBase
    {
        public UserEntity2()
        {
            TableName = "TbUser";
            IdentityName = "ID";
            PrimaryKeys.Add("ID");
        }

        /// <summary>
        /// 设置字段名数组，如果不实现该方法，框架会自动反射获取到字段名数组，因此从效率考虑，建议实现该方法
        /// </summary>
        protected override void SetFieldNames()
        {
            PropertyNames = new string[] { "ID", "Name", "LoginName", "Password", "Sex", "BirthDate" };
        }


        public int UserID
        {
            get { return getProperty<int>("ID",0); } //带属性字段名索引，加快属性访问
            set { setProperty("ID",0, value); }
        }
        public string Name
        {
            get { return getProperty<string>("Name",1); }
            set { setProperty("Name",1, value, 100); }
        }
        public string LoginName
        {
            get { return getProperty<string>("LoginName",2); }
            set { setProperty("LoginName",2, value, 50); }
        }
        public string PWD
        {
            get { return getProperty<string>("Password",3); }
            set { setProperty("Password",3, value, 50); }
        }
        public bool Sex
        {
            get { return getProperty<bool>("Sex",4); }
            set { setProperty("Sex",4, value); }
        }
        public DateTime BirthDate
        {
            get { return getProperty<DateTime>("BirthDate",5); }
            set { setProperty("BirthDate",5, value); }
        }
    }
}
