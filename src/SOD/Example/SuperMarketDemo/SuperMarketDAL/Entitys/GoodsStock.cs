/*
 * PDF.NET 数据开发框架
 * http://www.pwmis.com/sqlmap
 */

using System;
using PWMIS.DataMap.Entity;

namespace SuperMarketDAL.Entitys
{
    /// <summary>
    ///     商品存货信息
    /// </summary>
    public class GoodsStock : EntityBase
    {
        public GoodsStock()
        {
            TableName = "存货信息表";
            PrimaryKeys.Add("存货记录号");
            IdentityName = "存货记录号";
        }

        public int GoodsID
        {
            get => getProperty<int>("存货记录号");
            set => setProperty("存货记录号", value);
        }

        public string SerialNumber
        {
            get => getProperty<string>("条码号");
            set => setProperty("条码号", value);
        }

        public decimal GoodsPrice
        {
            get => getProperty<decimal>("售价");
            set => setProperty("售价", value);
        }

        public decimal CostPrice
        {
            get => getProperty<decimal>("成本价");
            set => setProperty("成本价", value);
        }

        public DateTime MakeOnDate
        {
            get => getProperty<DateTime>("生产日期");
            set => setProperty("生产日期", value);
        }

        public DateTime StockDate
        {
            get => getProperty<DateTime>("上货时间");
            set => setProperty("上货时间", value);
        }

        public int Stocks
        {
            get => getProperty<int>("库存数量");
            set => setProperty("库存数量", value);
        }

        protected override void SetFieldNames()
        {
            PropertyNames = new[] { "存货记录号", "条码号", "售价", "成本价", "生产日期", "上货时间", "库存数量" };
        }
    }
}