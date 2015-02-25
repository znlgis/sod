/*
 * PDF.NET 数据开发框架
 * http://www.pwmis.com/sqlmap
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperMarketModel;

namespace SuperMarketBLL
{
    /// <summary>
    /// 超市业务对象
    /// </summary>
    public class SuperMarketBIZ
    {
        private SuperMarketBIZ()
        { 
        
        }

        private static object lock_obj = new object(); 
        private static  SuperMarketBIZ _instance = null;
        /// <summary>
        /// 获取超市对象的唯一实例
        /// </summary>
        public static  SuperMarketBIZ Instance
        {
            get {
                if (_instance == null)
                {
                    lock (lock_obj)
                    {
                        _instance = new SuperMarketBIZ();
                    }
                }
                return _instance;
            }
        }

        public string Message { get; set; }
        /// <summary>
        /// 是否营业中
        /// </summary>
        public bool InBusiness
        {
            get;
            private set;
        }

        private List<CashierRegisterBIZ> _cashierConsole;
        /// <summary>
        /// 超市的收银台，需确保已经“开始营业”，否则不可用
        /// </summary>
        public List<CashierRegisterBIZ> CashierConsole
        {
            get { return _cashierConsole; }
            set { _cashierConsole = value; }
        }

        /// <summary>
        /// 开始营业
        /// </summary>
        /// <returns></returns>
        public bool StartBusiness()
        {
            Message = "";
            if (this.InBusiness)
            {
                Message = "已经在营业中！";
                return false ;
            }
           
            GoodsManageBIZ gm = new GoodsManageBIZ();
           
            if (gm.GetAllGoodsBaseInfoCount() <= 0)
            {
                Message += "没有商品基础信息；\r\n";
                return false;
            }
            if (gm.GetGoodsStockCount() <= 0)
            {
                Message += "没有商品存货信息；\r\n";
                return false;
            }
            if (CashierManageBIZ.WorkingCashier.Count == 0)
            {
                Message += "没有收银员到岗，请管理员先进行【收银台管理】！\r\n";
                return false;
            }
            //实例化收银台属性
            this.CashierConsole = new List<CashierRegisterBIZ>();

            foreach (Cashier cashier in CashierManageBIZ.WorkingCashier)
            {
                this.CashierConsole.Add(new CashierRegisterBIZ(cashier));
            }
            this.InBusiness = true;
            return true ;
        }

        /// <summary>
        /// 停止营业
        /// </summary>
        /// <returns></returns>
        public bool CloseBusiness()
        {
            //执行一些清理工作
            return false;
        }

    }
}
