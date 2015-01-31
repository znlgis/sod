
/* 
 本类由PWMIS 实体类生成工具(Ver 4.1)自动生成
 http://www.pwmis.com/sqlmap
 使用前请先在项目工程中引用 PWMIS.Core.dll
 2015/1/19 23:01:18
*/

using System;
using PWMIS.Common;
using PWMIS.DataMap.Entity;

namespace FreeDrag.Core.ORM.Models
{
    [Serializable()]
    public partial class TblTestModel : EntityBase
    {
        public TblTestModel()
        {
            TableName = "FreeDrag_TblTest";
            EntityMap = EntityMapType.Table;
            //IdentityName = "标识字段名";
            IdentityName = "ID";

            //PrimaryKeys.Add("主键字段名");
            PrimaryKeys.Add("ID");


        }


        protected override void SetFieldNames()
        {
            PropertyNames = new string[] { "ID", "Title", "ArtContent", "ReadCount", "CreateTime" };
        }



        /// <summary>
        /// 
        /// </summary>
        public System.Int32 ID
        {
            get { return getProperty<System.Int32>("ID"); }
            set { setProperty("ID", value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String Title
        {
            get { return getProperty<System.String>("Title"); }
            set { setProperty("Title", value, 255); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String ArtContent
        {
            get { return getProperty<System.String>("ArtContent"); }
            set { setProperty("ArtContent", value, 536870910); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32 ReadCount
        {
            get { return getProperty<System.Int32>("ReadCount"); }
            set { setProperty("ReadCount", value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.DateTime CreateTime
        {
            get { return getProperty<System.DateTime>("CreateTime"); }
            set { setProperty("CreateTime", value); }
        }
    }
}
