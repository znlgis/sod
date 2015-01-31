
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
    [Serializable()]
    public partial class p_hege : EntityBase
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


        protected override void SetFieldNames()
        {
            PropertyNames = new string[] { "id", "barcode", "product_type", "depoter", "dates", "passtype", "remarks", "chejian", "pinguanbu", "checks" };
        }



        /// <summary>
        /// 
        /// </summary>
        public System.Int32 id
        {
            get { return getProperty<System.Int32>("id"); }
            set { setProperty("id", value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String barcode
        {
            get { return getProperty<System.String>("barcode"); }
            set { setProperty("barcode", value, 80); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String product_type
        {
            get { return getProperty<System.String>("product_type"); }
            set { setProperty("product_type", value, 50); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String depoter
        {
            get { return getProperty<System.String>("depoter"); }
            set { setProperty("depoter", value, 50); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.DateTime dates
        {
            get { return getProperty<System.DateTime>("dates"); }
            set { setProperty("dates", value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32 passtype
        {
            get { return getProperty<System.Int32>("passtype"); }
            set { setProperty("passtype", value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String remarks
        {
            get { return getProperty<System.String>("remarks"); }
            set { setProperty("remarks", value, 100); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String chejian
        {
            get { return getProperty<System.String>("chejian"); }
            set { setProperty("chejian", value, 50); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String pinguanbu
        {
            get { return getProperty<System.String>("pinguanbu"); }
            set { setProperty("pinguanbu", value, 50); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String checks
        {
            get { return getProperty<System.String>("checks"); }
            set { setProperty("checks", value, 10); }
        }


    }
}
