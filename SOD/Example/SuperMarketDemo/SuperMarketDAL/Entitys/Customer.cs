/*
 * PDF.NET 数据开发框架
 * http://www.pwmis.com/sqlmap
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PWMIS.DataMap.Entity; 

namespace SuperMarketDAL.Entitys
{
    public class CustomerContactInfo : EntityBase
    {
        public CustomerContactInfo()
        {
            TableName = "客户表";
            PrimaryKeys.Add("客户号");//主键
        }

        protected override void SetFieldNames()
        {
            PropertyNames = new string[] { "客户号", "姓名", "性别", "出生日期", "联系电话", "联系地址", "积分" };
        }

        public string CustomerID
        {
            get { return getProperty<string>("客户号"); }
            set { setProperty("客户号", value); }
        }

        public string CustomerName
        {
            get { return getProperty<string>("姓名"); }
            set { setProperty("姓名", value); }
        }

        public DateTime Birthday
        {
            get { return getProperty<DateTime>("出生日期"); }
            set { setProperty("出生日期", value); }
        }

        public bool Sex
        {
            get { return getProperty<bool>("性别"); }
            set { setProperty("性别", value); }
        }

        public string PhoneNumber
        {
            get { return getProperty<string>("联系电话"); }
            set { setProperty("联系电话", value); }
        }

        public string Address
        {
            get { return getProperty<string>("联系地址"); }
            set { setProperty("联系地址", value); }
        }

        public int Integral
        {
            get { return getProperty<int>("积分"); }
            set { setProperty("积分", value); }
        }
    }
}
