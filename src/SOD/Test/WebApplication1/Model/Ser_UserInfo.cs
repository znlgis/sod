using System;
using PWMIS.DataMap.Entity;

//using System.Linq;

namespace TestWebAppModel
{
    public class Ser_UserInfo : EntitySerializable
    {
        public Ser_UserInfo()
        {
        }

        /// <summary>
        ///     设置用户信息（此构造函数可选）
        /// </summary>
        /// <param name="user"></param>
        public Ser_UserInfo(Tb_UserInfo user)
        {
            SetNameValues(user.GetNameValues());
        }

        public int ID
        {
            get => (int)getProperty("ID", TypeCode.Int32);
            set => setProperty("ID", value);
        }

        public string UserName
        {
            get => (string)getProperty("UserName", TypeCode.String);
            set => setProperty("UserName", value);
        }

        public bool Sex
        {
            get => (bool)getProperty("Sex", TypeCode.Boolean);
            set => setProperty("Sex", value);
        }

        public string IDCode
        {
            get => (string)getProperty("IDCode", TypeCode.String);
            set => setProperty("IDCode", value);
        }

        public string Nation
        {
            get => (string)getProperty("Nation", TypeCode.String);
            set => setProperty("Nation", value);
        }

        public double Stature
        {
            get => (double)getProperty("Stature", TypeCode.Double);
            set => setProperty("Stature", value);
        }

        public string Remark
        {
            get => (string)getProperty("Remark", TypeCode.String);
            set => setProperty("Remark", value);
        }

        public DateTime Birthday
        {
            get => (DateTime)getProperty("Birthday", TypeCode.DateTime);
            set => setProperty("Birthday", value);
        }
    }
}