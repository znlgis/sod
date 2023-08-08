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
    [Serializable]
    public class TblTestModel : EntityBase
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


        /// <summary>
        /// </summary>
        public int ID
        {
            get => getProperty<int>("ID");
            set => setProperty("ID", value);
        }

        /// <summary>
        /// </summary>
        public string Title
        {
            get => getProperty<string>("Title");
            set => setProperty("Title", value, 255);
        }

        /// <summary>
        /// </summary>
        public string ArtContent
        {
            get => getProperty<string>("ArtContent");
            set => setProperty("ArtContent", value, 536870910);
        }

        /// <summary>
        /// </summary>
        public int ReadCount
        {
            get => getProperty<int>("ReadCount");
            set => setProperty("ReadCount", value);
        }

        /// <summary>
        /// </summary>
        public DateTime CreateTime
        {
            get => getProperty<DateTime>("CreateTime");
            set => setProperty("CreateTime", value);
        }


        protected override void SetFieldNames()
        {
            PropertyNames = new[] { "ID", "Title", "ArtContent", "ReadCount", "CreateTime" };
        }
    }
}