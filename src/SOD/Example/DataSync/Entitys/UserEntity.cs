using System;
using PWMIS.DataMap.Entity;

namespace SOD.DataSync.Entitys
{
    public class UserEntity : EntityBase, IExportTable
    {
        public UserEntity()
        {
            TableName = "Table_User";
            //IdentityName = "标识字段名";
            IdentityName = "UID";

            //PrimaryKeys.Add("主键字段名");
            PrimaryKeys.Add("UID");
        }

        /// <summary>
        /// </summary>
        public int UID
        {
            get => getProperty<int>("UID");
            set => setProperty("UID", value);
        }

        /// <summary>
        /// </summary>
        public string Name
        {
            get => getProperty<string>("Name");
            set => setProperty("Name", value, 50);
        }

        /// <summary>
        /// </summary>
        public bool Sex
        {
            get => getProperty<bool>("Sex");
            set => setProperty("Sex", value);
        }

        /// <summary>
        /// </summary>
        public float Height
        {
            get => getProperty<float>("Height");
            set => setProperty("Height", value);
        }

        /// <summary>
        /// </summary>
        public DateTime Birthday
        {
            get => getProperty<DateTime>("Birthday");
            set => setProperty("Birthday", value);
        }

        public int BatchNumber
        {
            get => getProperty<int>("BatchNumber");
            set => setProperty("BatchNumber", value);
        }
    }
}