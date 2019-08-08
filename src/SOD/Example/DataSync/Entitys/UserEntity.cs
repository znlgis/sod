using PWMIS.DataMap.Entity;

namespace SOD.DataSync.Entitys
{
    public partial class UserEntity : EntityBase, IExportTable
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
        /// 
        /// </summary>
        public System.Int32 UID
        {
            get { return getProperty<System.Int32>("UID"); }
            set { setProperty("UID", value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String Name
        {
            get { return getProperty<System.String>("Name"); }
            set { setProperty("Name", value, 50); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Boolean Sex
        {
            get { return getProperty<System.Boolean>("Sex"); }
            set { setProperty("Sex", value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Single Height
        {
            get { return getProperty<System.Single>("Height"); }
            set { setProperty("Height", value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.DateTime Birthday
        {
            get { return getProperty<System.DateTime>("Birthday"); }
            set { setProperty("Birthday", value); }
        }

        public int BatchNumber
        {
            get { return getProperty<int>("BatchNumber"); }
            set { setProperty("BatchNumber", value); }
        }
    }
}
