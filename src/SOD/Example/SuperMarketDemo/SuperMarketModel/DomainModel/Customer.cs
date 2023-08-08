using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperMarketModel
{
    public class Customer
    {
        private string _customerID;

        private string _customerName;

        /// <summary>
        ///     客户标识
        /// </summary>
        public string CustomerID
        {
            get
            {
                if (string.IsNullOrEmpty(_customerID)) _customerID = Guid.NewGuid().ToString();
                return _customerID;
            }
            set => _customerID = value;
        }

        /// <summary>
        ///     顾客姓名
        /// </summary>
        public string CustomerName
        {
            get
            {
                if (string.IsNullOrEmpty(_customerName)) _customerName = "匿名客户";
                return _customerName;
            }
            set => _customerName = value;
        }

        /// <summary>
        ///     顾客选购的商品
        /// </summary>
        public List<Goods> Goodss { get; set; } = new List<Goods>();

        /// <summary>
        ///     销售回单
        /// </summary>
        public string SalesNote { get; set; }

        /// <summary>
        ///     选购想买的商品
        /// </summary>
        public void LikeBuy(Goods goods)
        {
            Goodss.Add(goods);
        }

        /// <summary>
        ///     当前选购的商品总金额
        /// </summary>
        /// <returns></returns>
        public decimal GoodsAmount()
        {
            decimal amount = 0;
            foreach (var goods in Goodss) amount += goods.GoodsPrice * goods.GoodsNumber;
            return amount;
        }


        /// <summary>
        ///     听收银员说要收多少RMB，以决定是否付款
        /// </summary>
        public bool ListenAmount(decimal amount)
        {
            var count = Goodss.Sum(g => g.GoodsNumber);
            var msg = string.Format("我是[{0}],我买了{1}件商品。我共花了{2}元RMB。", CustomerName, count, amount.ToString("f2"));
            Console.WriteLine(msg);
            return true;
        }
    }
}