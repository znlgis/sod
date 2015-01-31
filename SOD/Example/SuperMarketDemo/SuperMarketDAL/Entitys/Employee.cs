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
    public class Employee:EntityBase
    {
        public Employee()
        {
            TableName = "雇员表";
            PrimaryKeys.Add("工号");//主键
        }

        protected override void SetFieldNames()
        {
            PropertyNames = new string[] { "工号", "姓名", "性别","出生日期","入职时间","职务名称" };
        }

        public string WorkNumber
        {
            get { return getProperty<string>("工号"); }
            set { setProperty("工号", value,38); }
        }

        public string EmployeeName
        {
            get { return getProperty<string>("姓名"); }
            set { setProperty("姓名", value,20); }
        }

        public bool Sex
        {
            get { return getProperty<bool>("性别"); }
            set { setProperty("性别", value); }
        }

        public DateTime Birthday
        {
            get { return getProperty<DateTime>("出生日期"); }
            set { setProperty("出生日期", value); }
        }

        public DateTime JobInDate
        {
            get { return getProperty<DateTime>("入职时间"); }
            set { setProperty("入职时间", value); }
        }

        public string JobName
        {
            get { return getProperty<string>("职务名称"); }
            set { setProperty("职务名称", value,10); }
        }
    }
}
