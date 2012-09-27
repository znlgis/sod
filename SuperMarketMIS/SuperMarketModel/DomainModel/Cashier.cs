using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarketModel
{
    /// <summary>
    /// 收银员对象
    /// </summary>
    public class Cashier
    {
        CashierRegisterMachines _cashRegister;
        public Cashier()
        { 
        
        }

        /// <summary>
        /// 使用的收银机
        /// </summary>
        public CashierRegisterMachines UsingCashierRegister
        {
            get { return _cashRegister; }
            set { _cashRegister = value; }
        }

        /// <summary>
        /// 收银员使用一台收银机开始工作
        /// </summary>
        /// <param name="cashRegister"></param>
        public Cashier(CashierRegisterMachines cashRegister)
        {
            this._cashRegister = cashRegister;
        }
        /// <summary>
        /// 收银员姓名
        /// </summary>
        public string CashierName
        {
            get;
            set;
        }

        /// <summary>
        /// 工号
        /// </summary>
        public string WorkNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 收银
        /// </summary>
        /// <param name="customer">顾客</param>
        public bool  CashRegister(Customer customer)
        {
            //打开使用收银机
            //CashierRegisterMachines cashRegister = new CashierRegisterMachines();
            
            //对顾客的商品进行收银机扫码，收银
            foreach (var goods in customer.Goodss)
            {
                //使用收银机扫商品进行收银
                _cashRegister.CashRegisters(goods);
            }

            //通知顾客一共收多少钱
            return customer.ListenAmount(_cashRegister.ShowAmount());
        }

    }
}
