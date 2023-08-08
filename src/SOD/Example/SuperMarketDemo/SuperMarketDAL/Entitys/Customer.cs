/*
 * PDF.NET 数据开发框架
 * http://www.pwmis.com/sqlmap
 */

using System;
using PWMIS.DataMap.Entity;

namespace SuperMarketDAL.Entitys
{
    public class CustomerContactInfo : EntityBase
    {
        public CustomerContactInfo()
        {
            TableName = "客户表";
            PrimaryKeys.Add("客户号"); //主键
        }

        public string CustomerID
        {
            get => getProperty<string>("客户号");
            set => setProperty("客户号", value);
        }

        public string CustomerName
        {
            get => getProperty<string>("姓名");
            set => setProperty("姓名", value);
        }

        public DateTime Birthday
        {
            get => getProperty<DateTime>("出生日期");
            set => setProperty("出生日期", value);
        }

        public bool Sex
        {
            get => getProperty<bool>("性别");
            set => setProperty("性别", value);
        }

        public string PhoneNumber
        {
            get => getProperty<string>("联系电话");
            set => setProperty("联系电话", value);
        }

        public string Address
        {
            get => getProperty<string>("联系地址");
            set => setProperty("联系地址", value);
        }

        public int Integral
        {
            get => getProperty<int>("积分");
            set => setProperty("积分", value);
        }

        protected override void SetFieldNames()
        {
            PropertyNames = new[] { "客户号", "姓名", "性别", "出生日期", "联系电话", "联系地址", "积分" };
        }
    }
}