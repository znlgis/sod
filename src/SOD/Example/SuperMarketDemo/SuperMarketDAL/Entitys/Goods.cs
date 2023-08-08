/*
 * PDF.NET 数据开发框架
 * http://www.pwmis.com/sqlmap
 */

using PWMIS.DataMap.Entity;

namespace SuperMarketDAL.Entitys
{
    /// <summary>
    ///     商品基本信息实体
    /// </summary>
    public class GoodsBaseInfo : EntityBase
    {
        public GoodsBaseInfo()
        {
            TableName = "商品信息表";
            PrimaryKeys.Add("条码号");
        }

        public string SerialNumber
        {
            get => getProperty<string>("条码号", 0);
            set => setProperty("条码号", 0, value);
        }

        public string GoodsName
        {
            get => getProperty<string>("商品名称", 1);
            set => setProperty("商品名称", 1, value);
        }

        public string Manufacturer
        {
            get => getProperty<string>("厂商名称", 2);
            set => setProperty("厂商名称", 2, value);
        }

        public int CanUserMonth
        {
            get => getProperty<int>("保质期", 3);
            set => setProperty("保质期", 3, value);
        }

        protected override void SetFieldNames()
        {
            PropertyNames = new[] { "条码号", "商品名称", "厂商名称", "保质期" };
        }
    }
}