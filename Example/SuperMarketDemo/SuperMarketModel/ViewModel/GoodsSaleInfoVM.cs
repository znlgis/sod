/*
 * PDF.NET 数据开发框架
 * http://www.pwmis.com/sqlmap
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarketModel.ViewModel
{
    /// <summary>
    /// 商品销售价格信息
    /// </summary>
    public class GoodsSaleInfoVM
    {
        /// <summary>
        /// 厂商名称
        /// </summary>
        public string Manufacturer { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 条码号
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// 售价
        /// </summary>
        public decimal GoodsPrice { get; set; }
        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime MakeOnDate { get; set; }
        /// <summary>
        /// 保质期
        /// </summary>
        public int CanUserMonth { get; set; }
        /// <summary>
        /// 库存数量
        /// </summary>
        public int Stocks { get; set; }
        /// <summary>
        /// 存货记录号
        /// </summary>
        public int GoodsID { get; set; }


    }
}
