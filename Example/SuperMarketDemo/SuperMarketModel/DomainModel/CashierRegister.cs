using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperMarketModel.ViewModel;

namespace SuperMarketModel
{
    /// <summary>
    /// 收银机对象
    /// </summary>
    public class CashierRegisterMachines
    {
        /// <summary>
        /// 总价格
        /// </summary>
        private decimal _totalAmount { get; set; }

        public void ReSet()
        {
            _totalAmount = 0;
            GoodsSalePriceList = null;
        }

        public CashierRegisterMachines()
        {
            //收银总额置0
            this._totalAmount = 0;

        }
        /// <summary>
        /// 收银机编号
        /// </summary>
        public string CashRegisterNo
        {
            get;
            set;
        }

        /// <summary>
        /// 收银
        /// </summary>
        public void CashRegisters(Goods goods)
        {
            this._totalAmount += goods.GoodsPrice * goods.GoodsNumber;
            //同一中商品，只要条码相同，则不论批次是否相同，都是一个价格
            GoodsSalePriceVM spVM=GoodsSalePriceList.Where(p => p.SerialNumber == goods.SerialNumber).First();
            goods.GoodsPrice = spVM.DiscountPrice;//更新实收的价格

        }

        /// <summary>
        /// 显示收银总额
        /// </summary>
        /// <returns></returns>
        public decimal ShowAmount()
        {
            return this._totalAmount;
        }

        /// <summary>
        /// 商品销售价格信息表
        /// </summary>
        public List<GoodsSalePriceVM> GoodsSalePriceList
        {
            get;
            set;
        }

    }
}
