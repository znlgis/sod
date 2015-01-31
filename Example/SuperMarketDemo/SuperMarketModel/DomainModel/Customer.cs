using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarketModel
{
    public class Customer
    {
        private List<Goods> _goodss = new List<Goods>();
        private  string _customerID;
        /// <summary>
        /// 客户标识
        /// </summary>
        public string CustomerID
        {
            get {
                if (string.IsNullOrEmpty(_customerID))
                {
                    _customerID = Guid.NewGuid().ToString();
                }
                return _customerID;
            }
            set { _customerID = value; }
        }

        private string _customerName;
        /// <summary>
        /// 顾客姓名
        /// </summary>
        public string CustomerName
        {
            get {
                if (string.IsNullOrEmpty(_customerName))
                {
                    _customerName = "匿名客户";
                }
                return _customerName;
            }
            set {
                _customerName = value;
            }
        }

        /// <summary>
        /// 顾客选购的商品
        /// </summary>
        public List<SuperMarketModel.Goods> Goodss
        {
            get
            {
                return _goodss;
            }
            set
            {
                _goodss = value;
            }
        }

        /// <summary>
        /// 选购想买的商品
        /// </summary>
        public void LikeBuy(Goods goods)
        {
            this._goodss.Add(goods);

        }

        /// <summary>
        /// 当前选购的商品总金额
        /// </summary>
        /// <returns></returns>
        public decimal GoodsAmount()
        {
            decimal amount = 0;
            foreach (Goods goods in this._goodss)
            {
                amount += goods.GoodsPrice * goods.GoodsNumber;
            }
            return amount;
        }

       
        /// <summary>
        /// 听收银员说要收多少RMB，以决定是否付款
        /// </summary>
        public bool  ListenAmount(decimal amount)
        {
            int count = this.Goodss.Sum (g => g.GoodsNumber);
            string msg = string.Format("我是[{0}],我买了{1}件商品。我共花了{2}元RMB。", this.CustomerName, count, amount.ToString("f2"));
            Console.WriteLine(msg);
            return true;
        }

        /// <summary>
        /// 销售回单
        /// </summary>
        public string SalesNote
        {
            get;
            set;
        }
    }
}
