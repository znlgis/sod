using System;
using System.Collections.Generic;
using SuperMarketBLL;
using SuperMarketModel;

namespace SuperMarketTestConsole
{
    public class Class1
    {
        private static void Main(string[] args)
        {
            TestBIZ();

            Console.Read();
        }

        private static void TestBIZ()
        {
            //我们创建几样商品
            var RedWine = new GoodsStock
                { GoodsName = "红酒", GoodsPrice = 1800, GoodsNumber = 10, SerialNumber = "J000111" };
            var Condoms = new GoodsStock
                { GoodsName = "安全套", GoodsPrice = 35, GoodsNumber = 10, SerialNumber = "T213000" };

            //我们创建几位顾客
            var Chunge = new Customer { CustomerName = "春哥" };
            var Beianqi = new Customer { CustomerName = "贝安琪" };
            var Noname = new Customer();

            //有一台收银机
            var crManchines = new CashierRegisterMachines { CashRegisterNo = "CR00011" };
            //当然，我们需要收银员啊
            var CashierMM = new Cashier(crManchines) { CashierName = "收银员MM", WorkNumber = "SYY10011" };

            //顾客逛了一圈，选了自己想要的商品
            Chunge.LikeBuy(RedWine.TakeOut(1));
            Beianqi.LikeBuy(RedWine.TakeOut(1));
            Beianqi.LikeBuy(Condoms.TakeOut(1));
            Noname.LikeBuy(Condoms.TakeOut(2));

            //调用收银业务类
            var biz = new CashierRegisterBIZ(CashierMM, crManchines);
            biz.AddQueue(Chunge);
            biz.AddQueue(Beianqi);
            biz.AddQueue(Noname);

            biz.CashierRegister();
        }


        private static void TestModel()
        {
            //http://www.cnblogs.com/assion/archive/2011/05/13/2045253.html

            //我们创建几样商品
            var RedWine = new GoodsStock { GoodsName = "红酒", GoodsPrice = 1800, GoodsNumber = 10 };
            var Condoms = new GoodsStock { GoodsName = "安全套", GoodsPrice = 35, GoodsNumber = 10 };

            //我们创建几位顾客
            var Chunge = new Customer { CustomerName = "春哥" };
            var Beianqi = new Customer { CustomerName = "贝安琪" };
            var Noname = new Customer();

            //有一台收银机
            var crManchines = new CashierRegisterMachines { CashRegisterNo = "CR00011" };
            //当然，我们需要收银员啊
            var CashierMM = new Cashier(crManchines) { CashierName = "收银员MM", WorkNumber = "SYY10011" };


            //顾客开始排队结帐了
            var customerQueue = new Queue<Customer>();
            customerQueue.Enqueue(Chunge);
            customerQueue.Enqueue(Beianqi);
            customerQueue.Enqueue(Noname);

            //队伍过来，按先后顺序挨个收银喽
            foreach (var customer in customerQueue)
                //收银
                CashierMM.CashRegister(customer);
        }
    }
}