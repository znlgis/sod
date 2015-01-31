/*
 * PDF.NET 数据开发框架
 * http://www.pwmis.com/sqlmap
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PWMIS.DataMap.Entity;

namespace SuperMarketDAL.Entitys
{

    /// <summary>
    /// 商品销售信息实体
    /// </summary>
    public class GoodsSellDetail : EntityBase
    {
        public GoodsSellDetail()
        {
            TableName = "商品销售记录表";
            PrimaryKeys.Add("销售记录号");
            IdentityName = "销售记录号";
        }

        protected override void SetFieldNames()
        {
            PropertyNames = new string[] { "销售记录号", "销售单号", "商品条码", "单价", "数量" };
        }

        public int DetailID
        {
            get { return getProperty<int>("销售记录号"); }
            set { setProperty("销售记录号", value); }
        }

        public int NoteID
        {
            get { return getProperty<int>("销售单号"); }
            set { setProperty("销售单号", value); }
        }

        public string SerialNumber
        {
            get { return getProperty<string>("商品条码"); }
            set { setProperty("商品条码", value); }
        }

        public decimal GoodsPrice
        {
            get { return getProperty<decimal>("单价"); }
            set { setProperty("单价", value); }
        }

        public int SellNumber
        {
            get { return getProperty<int>("数量"); }
            set { setProperty("数量", value); }
        }
    }

    /// <summary>
    /// 商品销售单据实体
    /// </summary>
    public class GoodsSellNote : EntityBase
    {
        public GoodsSellNote()
        {
            TableName = "商品销售单据表";
            PrimaryKeys.Add("销售单号");
            IdentityName = "销售单号";
        }

        protected override void SetFieldNames()
        {
            PropertyNames = new string[] { "销售单号", "销售日期", "终端号","客户号", "销售员号", "销售类别" };
        }


        public int NoteID
        {
            get { return getProperty<int>("销售单号"); }
            set { setProperty("销售单号", value); }
        }

        public DateTime SellDate
        {
            get { return getProperty<DateTime>("销售日期"); }
            set { setProperty("销售日期", value); }
        }

        public string ManchinesNumber
        {
            get { return getProperty<string>("终端号"); }
            set { setProperty("终端号", value); }
        }

        public string CustomerID
        {
            get { return getProperty<string>("客户号"); }
            set { setProperty("客户号", value); }
        }

        public string SalesmanID
        {
            get { return getProperty<string>("销售员号"); }
            set { setProperty("销售员号", value); }
        }

        public string SalesType
        {
            get { return getProperty<string>("销售类别"); }
            set { setProperty("销售类别", value); }
        }

        /// <summary>
        /// 详细销售记录
        /// </summary>
        public List<GoodsSellDetail> GoodsSellDetails
        {
            get;
            set;
        }
    }
}
