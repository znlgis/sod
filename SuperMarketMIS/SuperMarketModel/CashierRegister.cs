using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        }

        /// <summary>
        /// 显示收银总额
        /// </summary>
        /// <returns></returns>
        public decimal ShowAmount()
        {
            return this._totalAmount;
        }

    }
}
