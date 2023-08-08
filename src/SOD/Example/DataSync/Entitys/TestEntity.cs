using System;
using PWMIS.DataMap.Entity;

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
            get => getProperty<int>("ID");
            set => setProperty("ID", value);
        }

        public string Name
        {
            get => getProperty<string>("Name");
            set => setProperty("Name", value, 100);
        }

        public DateTime AtTime
        {
            get => getProperty<DateTime>("AtTime");
            set => setProperty("AtTime", value);
        }

        public string Classification
        {
            get => getProperty<string>("Classification");
            set => setProperty("Classification", value, 50);
        }

        public int BatchNumber
        {
            get => getProperty<int>("BatchNumber");
            set => setProperty("BatchNumber", value);
        }
    }
}