using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarketModel
{
    /// <summary>
    /// 销售的商品
    /// </summary>
    public class Goods
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public int GoodsID { get; set; }
        /// <summary>
        /// 商品条码
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 商品销售的单价
        /// </summary>
        public decimal GoodsPrice { get; set; }
        /// <summary>
        /// 选购的商品数量
        /// </summary>
        public int GoodsNumber { get; set; }
        /// <summary>
        /// 购买该商品可以获得的积分
        /// </summary>
        public int Integral { get; set; }

    }

    /// <summary>
    /// 库存商品
    /// </summary>
    public class GoodsStock
    {
        /// <summary>
        /// 商品条码
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 商品单价
        /// </summary>
        public decimal GoodsPrice { get; set; }
        /// <summary>
        /// 库存的商品数量
        /// </summary>
        public int GoodsNumber { get; set; }
        /// <summary>
        /// 取出该商品，并使得库存减少响应数量，得到选购的商品
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public Goods TakeOut(int number)
        {
            Goods goods = new Goods();
            goods.GoodsName = this.GoodsName;
            goods.GoodsPrice = this.GoodsPrice;
            goods.SerialNumber = this.SerialNumber;
            if (number > this.GoodsNumber)
                number = this.GoodsNumber;
            this.GoodsNumber -= number;
            goods.GoodsNumber = number;
            return goods;
        }
    }
}
