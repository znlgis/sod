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
    /// 销售价格清单
    /// </summary>
    public class GoodsSalePriceVM
    {
        public GoodsSalePriceVM() { }

        public GoodsSalePriceVM(Goods goods)
        {
            this.SerialNumber = goods.SerialNumber;
            this.GoodsNumber = goods.GoodsNumber;
            this.GoodsName = goods.GoodsName;
            this.GoodsPrice = goods.GoodsPrice;
        }
        /// <summary>
        /// 条码号
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal  GoodsPrice { get; set; }
        /// <summary>
        /// 折扣价
        /// </summary>
        public decimal  DiscountPrice { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public int GoodsNumber { get; set; }

        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal GoodsMoney {
            get {
                if (DiscountPrice > 0)
                    return DiscountPrice * GoodsNumber;
                else
                    return GoodsPrice * GoodsNumber;
            }
        }
    }
}
