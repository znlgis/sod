using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using PWMIS.DataMap.Entity;

namespace TestWebAppModel
{
    public class Ser_UserInfo : EntitySerializable
    {
        public Ser_UserInfo() { }

        /// <summary>
        /// 设置用户信息（此构造函数可选）
        /// </summary>
        /// <param name="user"></param>
        public Ser_UserInfo(Tb_UserInfo user)
        {
            base.SetNameValues(user.GetNameValues());
        }

        public System.Int32 ID
        {
            get { return (System.Int32)getProperty("ID", TypeCode.Int32); }
            set { setProperty("ID", value); }
        }

        public System.String UserName
        {
            get { return (System.String)getProperty("UserName", TypeCode.String); }
            set { setProperty("UserName", value); }
        }

        public System.Boolean Sex
        {
            get { return (System.Boolean)getProperty("Sex", TypeCode.Boolean); }
            set { setProperty("Sex", value); }
        }

        public System.String IDCode
        {
            get { return (System.String)getProperty("IDCode", TypeCode.String); }
            set { setProperty("IDCode", value); }
        }

        public System.String Nation
        {
            get { return (System.String)getProperty("Nation", TypeCode.String); }
            set { setProperty("Nation", value); }
        }

        public System.Double Stature
        {
            get { return (System.Double)getProperty("Stature", TypeCode.Double); }
            set { setProperty("Stature", value); }
        }

        public System.String Remark
        {
            get { return (System.String)getProperty("Remark", TypeCode.String); }
            set { setProperty("Remark", value); }
        }

        public System.DateTime Birthday
        {
            get { return (System.DateTime)getProperty("Birthday", TypeCode.DateTime); }
            set { setProperty("Birthday", value); }
        }
    }
}
