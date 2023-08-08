/*
 本类由PWMIS 实体类生成工具(Ver 4.1)自动生成
 http://www.pwmis.com/sqlmap
 使用前请先在项目工程中引用 PWMIS.Core.dll
 2013-6-20 14:32:32
*/

using System;
using PWMIS.Common;
using PWMIS.DataMap.Entity;

namespace PDFNETClassLib.Model
{
    [Serializable]
    public class p_hege : EntityBase
    {
        public p_hege()
        {
            TableName = "p_hege";
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
        public string barcode
        {
            get => getProperty<string>("barcode");
            set => setProperty("barcode", value, 80);
        }

        /// <summary>
        /// </summary>
        public string product_type
        {
            get => getProperty<string>("product_type");
            set => setProperty("product_type", value, 50);
        }

        /// <summary>
        /// </summary>
        public string depoter
        {
            get => getProperty<string>("depoter");
            set => setProperty("depoter", value, 50);
        }

        /// <summary>
        /// </summary>
        public DateTime dates
        {
            get => getProperty<DateTime>("dates");
            set => setProperty("dates", value);
        }

        /// <summary>
        /// </summary>
        public int passtype
        {
            get => getProperty<int>("passtype");
            set => setProperty("passtype", value);
        }

        /// <summary>
        /// </summary>
        public string remarks
        {
            get => getProperty<string>("remarks");
            set => setProperty("remarks", value, 100);
        }

        /// <summary>
        /// </summary>
        public string chejian
        {
            get => getProperty<string>("chejian");
            set => setProperty("chejian", value, 50);
        }

        /// <summary>
        /// </summary>
        public string pinguanbu
        {
            get => getProperty<string>("pinguanbu");
            set => setProperty("pinguanbu", value, 50);
        }

        /// <summary>
        /// </summary>
        public string checks
        {
            get => getProperty<string>("checks");
            set => setProperty("checks", value, 10);
        }


        protected override void SetFieldNames()
        {
            PropertyNames = new[]
            {
                "id", "barcode", "product_type", "depoter", "dates", "passtype", "remarks", "chejian", "pinguanbu",
                "checks"
            };
        }
    }
}