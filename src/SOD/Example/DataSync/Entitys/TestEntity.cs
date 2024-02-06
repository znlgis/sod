using PWMIS.DataMap.Entity;
using System;

namespace SOD.DataSync.Entitys
{
    public class TestEntity : EntityBase, IExportTable
    {
        public TestEntity()
        {
            TableName = "Table_Test";
            //IdentityName = "标识字段名";
            IdentityName = "ID";

            //PrimaryKeys.Add("主键字段名");
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
            set { setProperty("Name", value,100); }
        }

        public DateTime AtTime
        {
            get { return getProperty<DateTime>("AtTime"); }
            set { setProperty("AtTime", value); }
        }

        public int BatchNumber {
            get { return getProperty<int>("BatchNumber"); }
            set { setProperty("BatchNumber", value); }
        }

        public string Classification
        {
            get { return getProperty<string>("Classification"); }
            set { setProperty("Classification", value,50); }
        }
    }
}
