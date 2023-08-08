/*
 本类由PWMIS 实体类生成工具(Ver 4.1)自动生成
 http://www.pwmis.com/sqlmap
 使用前请先在项目工程中引用 PWMIS.Core.dll
 2013-8-21 14:26:55
*/

using System;
using PWMIS.Common;
using PWMIS.DataMap.Entity;

namespace PDFNETClassLib.Model
{
    [Serializable]
    public class p_hege_detail : EntityBase
    {
        public p_hege_detail()
        {
            TableName = "p_hege_detail";
            EntityMap = EntityMapType.Table;
            //IdentityName = "标识字段名";
            IdentityName = "id";

            //PrimaryKeys.Add("主键字段名");
            PrimaryKeys.Add("id");
        }


        /// <summary>
        /// </summary>
        public int id
        {
            get => getProperty<int>("id");
            set => setProperty("id", value);
        }

        /// <summary>
        /// </summary>
        public int hegeID
        {
            get => getProperty<int>("hegeID");
            set => setProperty("hegeID", value);
        }

        /// <summary>
        /// </summary>
        public string coName
        {
            get => getProperty<string>("coName");
            set => setProperty("coName", value, 50);
        }

        /// <summary>
        /// </summary>
        public string coType
        {
            get => getProperty<string>("coType");
            set => setProperty("coType", value, 10);
        }

        /// <summary>
        /// </summary>
        public string coMessage
        {
            get => getProperty<string>("coMessage");
            set => setProperty("coMessage", value, 50);
        }

        /// <summary>
        /// </summary>
        public string faMessage
        {
            get => getProperty<string>("faMessage");
            set => setProperty("faMessage", value, 50);
        }


        protected override void SetFieldNames()
        {
            PropertyNames = new[] { "id", "hegeID", "coName", "coType", "coMessage", "faMessage" };
        }
    }
}