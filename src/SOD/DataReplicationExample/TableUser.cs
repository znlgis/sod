/*
 本类由PWMIS 实体类生成工具(Ver 4.1)自动生成
 http://www.pwmis.com/sqlmap
 使用前请先在项目工程中引用 PWMIS.Core.dll
 2013/5/25 9:51:18
*/

using System;
using PWMIS.Common;
using PWMIS.DataMap.Entity;

namespace DataReplicationExample
{
    [Serializable]
    public class UserEntity : EntityBase
    {
        public UserEntity()
        {
            TableName = "Table_User";
            EntityMap = EntityMapType.Table;
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

        /// <summary>
        /// </summary>
        public byte[] Photo
        {
            get => getProperty<byte[]>("Photo");
            set => setProperty("Photo", value, 1024);
        }


        protected override void SetFieldNames()
        {
            PropertyNames = new[] { "UID", "Name", "Sex", "Height", "Birthday", "Photo" };
        }
    }
}