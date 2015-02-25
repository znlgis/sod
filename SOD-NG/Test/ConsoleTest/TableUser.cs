
/* 
 本类由PWMIS 实体类生成工具(Ver 4.1)自动生成
 http://www.pwmis.com/sqlmap
 使用前请先在项目工程中引用 PWMIS.Core.dll
 2013/5/25 9:51:18
*/

using System;
using PWMIS.Common;
using PWMIS.DataMap.Entity;

namespace LocalDB
{
    [Serializable()]
    public partial class Table_User : EntityBase, ConsoleTest.ITable_User
    {
        public Table_User()
        {
            TableName = "Table_User";
            EntityMap = EntityMapType.Table;
            //IdentityName = "标识字段名";
            IdentityName = "UID";

            //PrimaryKeys.Add("主键字段名");
            PrimaryKeys.Add("UID");


        }


        protected override void SetFieldNames()
        {
            PropertyNames = new string[] { "UID", "Name", "Sex", "Height", "Birthday" };
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


    }
}
