using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperMarketModel;
using SuperMarketDAL.Entitys;
using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Data;
using PWMIS.DataProvider.Adapter;
using SuperMarketModel.ViewModel;
using PWMIS.Core.Extensions;

namespace SuperMarketBLL
{
    /// <summary>
    /// 收银业务逻辑类（收银台）
    /// </summary>
    public class CashierRegisterBIZ
    {
        public Cashier CurrCashier { get; set; }
        public CashierRegisterMachines CurrCRManchines { get; set; }
        private Queue<Customer> customerQueue;
        private Customer processingCustomer;//当前正在处理的顾客
        private DateTime lastProcessTime;//上次处理的时间，避免超时无效客户的排队

        public CashierRegisterBIZ(Cashier cashier, CashierRegisterMachines crm)
        {
            this.CurrCashier = cashier;
            this.CurrCashier.UsingCashierRegister = crm; 
            this.CurrCRManchines = crm;
            customerQueue = new Queue<Customer>();
        }

        public CashierRegisterBIZ(Cashier cashier):this(cashier,cashier.UsingCashierRegister )
        {
            CashierManageBIZ biz = new CashierManageBIZ();
            if (!biz.TestAssignedCashier(cashier))
                throw new Exception("当前收银员未被指派，不能创建收银台。收银员名称："+cashier.CashierName );
            
        }

        /// <summary>
        /// 顾客排队
        /// </summary>
        /// <param name="customer"></param>
        public void AddQueue(Customer customer)
        {
            customerQueue.Enqueue(customer);
        }

        /// <summary>
        /// 退出队列，返回购物
        /// </summary>
        /// <param name="customer"></param>
        public void ExitQueue(Customer customer)
        {
            if (this.processingCustomer == customer)
                this.processingCustomer = null;
        }
        /// <summary>
        /// (顾客排队)的人数
        /// </summary>
        public int QueueLength
        {
            get { return customerQueue.Count; }
        }

        /// <summary>
        /// 指定的客户是否还需要继续等待
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="salePriceVM">销售价格单</param>
        /// <returns></returns>
        public bool Waite(Customer customer)
        {
            if (this.processingCustomer == customer)
            {
                //是当前用户，开始收银
                //如果客户同意付款
                //对客户所购物品的售价进行实际的计算
                List<GoodsSalePriceVM> salesPriceInfo = new List<GoodsSalePriceVM>();

                GoodsManageBIZ biz = new GoodsManageBIZ();
                CustomerManageBIZ cbiz = new CustomerManageBIZ();
                bool isCustomer = (cbiz.GetCustomerContactInfo(customer.CustomerID) != null);

                foreach (Goods goods in customer.Goodss)
                {
                    GoodsSalePriceVM vm = new GoodsSalePriceVM(goods);
                    int integral;
                    decimal sellPrice = biz.GoodsSellPrice(goods.GoodsID, goods.SerialNumber, goods.GoodsNumber, isCustomer, out integral);
                    
                    if (sellPrice == 0)
                    {
                        vm.GoodsNumber = 0;
                        goods.GoodsNumber = 0;
                    }
                    vm.DiscountPrice = sellPrice;
                    //goods.GoodsPrice = sellPrice;
                    goods.Integral = integral;
                    salesPriceInfo.Add(vm);
                }

                this.CurrCRManchines.ReSet();
                this.CurrCRManchines.GoodsSalePriceList = salesPriceInfo; 
                return false;
            }
            else
            {
                if (this.processingCustomer == null)
                {
                    if (customerQueue.Count > 0)
                    {
                        //准备进行下一个客户的处理
                        this.processingCustomer = customerQueue.Dequeue();
                        this.lastProcessTime = DateTime.Now;
                    }
                }
                else
                { 
                    //当前客户可能已经离开，放弃购物了，需要强行清除，否则后面的客户将会一直等待
                    if (DateTime.Now.Subtract(lastProcessTime).Minutes > 5)
                        this.processingCustomer = null;
                }
               
                return true;
            }
        }

        /// <summary>
        /// （待客户确认后的）收银处理
        /// </summary>
        public void Processing()
        {
            if (this.processingCustomer != null)
            {
                if (this.CurrCashier.CashRegister(this.processingCustomer))
                {
                    //收款
                    if (Gathering(this.CurrCRManchines.ShowAmount()))
                    {
                        //本次销售可获得的总积分
                        int allIntegral = this.processingCustomer.Goodss.Sum(p => p.Integral); 
                        
                        //写入销售记录
                        GoodsSellNote note = SaveSalesInfo(this.processingCustomer, allIntegral);
                        //打印销售回单
                        PrintSalesNote(note, this.processingCustomer);

                        Console.Write(this.processingCustomer.SalesNote);
                    }
                }
                this.processingCustomer.Goodss.Clear(); 
            }

            if (customerQueue.Count > 0)
            {
                //准备进行下一个客户的处理
                this.processingCustomer = customerQueue.Dequeue();
            }
            else
            {
                this.processingCustomer = null;
            }
            
        }

        /// <summary>
        /// 等待并对所有客户收银（循环等待模式）
        /// </summary>
        public void CashierRegister()
        {
            //队伍过来，按先后顺序挨个收银喽
            do
            {
                if (customerQueue.Count > 0)
                {
                    var customer = customerQueue.Dequeue();
                    //收银，如果客户同意付款
                    if (this.CurrCashier.CashRegister(customer))
                    {
                        //收款
                        if (Gathering(this.CurrCRManchines.ShowAmount()))
                        {
                            //写入销售记录
                            GoodsSellNote note = SaveSalesInfo(customer,0);
                            //打印销售回单
                            PrintSalesNote(note, customer);

                            Console.Write(customer.SalesNote);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("收银台空闲，等待顾客中...");
                    System.Threading.Thread.Sleep(10000);
                }
               
               
            } while (true);

        }

        /// <summary>
        /// 收款（POS或者现金）
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        private bool Gathering(decimal money)
        {
            //详细代码略
            return true;
        }

        private void PrintSalesNote(GoodsSellNote note, Customer customer)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            sb.Append("\r\n=======销售单信息=========================");
            sb.Append("\r\n流水号：" + note.NoteID.ToString("N8"));
            sb.Append("\r\n客户号：" + note.CustomerID);
            sb.Append("\r\n终端号：" + note.ManchinesNumber);
            sb.Append("\r\n销售员：" + note.SalesmanID);
            sb.Append("\r\n日  期：" + note.SellDate);
            sb.Append("\r\n------- 销售明细--------------------------");
            sb.Append("\r\n商品名称|\t单价    |\t数量    |\t价格    " );
            sb.Append("\r\n------------------------------------------");

            int allIntegral = 0;
            foreach (Goods goods in customer.Goodss)
            {
                sb.Append("\r\n" + goods.GoodsName);
                sb.Append("\t\t" + goods.GoodsPrice.ToString ("c"));
                sb.Append("\t" + goods.GoodsNumber);
                sb.Append("\t" + (goods.GoodsPrice * goods.GoodsNumber).ToString ("c"));
                allIntegral += goods.Integral;
            }
            sb.Append("\r\n------------------------------------------");
            sb.Append("\r\n金额合计：" + this.CurrCRManchines.ShowAmount().ToString ("c"));
            sb.Append("\r\n");
            if (allIntegral > 0)
                sb.Append("本次消费可以获得的积分："+allIntegral );

            customer.SalesNote = sb.ToString();
        }

        /// <summary>
        /// 保存销售信息
        /// </summary>
        /// <param name="customer">客户信息</param>
        /// <param name="integral">要增加的积分</param>
        /// <returns></returns>
        private GoodsSellNote SaveSalesInfo(Customer customer, int integral)
        {
            GoodsSellNote note = new GoodsSellNote();
            note.CustomerID = customer.CustomerID;
            note.ManchinesNumber = this.CurrCRManchines.CashRegisterNo;
            note.SalesmanID = this.CurrCashier.WorkNumber;
            note.SalesType = "店内销售";
            note.SellDate = DateTime.Now;
            note.GoodsSellDetails = new List<GoodsSellDetail>();

            AdoHelper db = MyDB.GetDBHelper();
            db.BeginTransaction();
            try
            {
                EntityQuery<GoodsSellNote> query = new EntityQuery<GoodsSellNote>(db);
                if (query.Insert(note) > 0)
                {
                    foreach (Goods goods in customer.Goodss)
                    {
                        if (goods.GoodsNumber > 0)
                        {
                            //处理详单
                            GoodsSellDetail detail = new GoodsSellDetail();
                            detail.GoodsPrice = goods.GoodsPrice;
                            detail.NoteID = note.NoteID;
                            detail.SellNumber = goods.GoodsNumber;
                            detail.SerialNumber = goods.SerialNumber;

                            note.GoodsSellDetails.Add(detail);

                            //更新库存
                            SuperMarketDAL.Entitys.GoodsStock stock = new SuperMarketDAL.Entitys.GoodsStock();
                            stock.GoodsID = goods.GoodsID;
                            stock.Stocks = goods.GoodsNumber;
                            
                            //OQL q = OQL.From(stock)
                            //    .UpdateSelf ('-', stock.Stocks)
                            //    .Where(stock.GoodsID)
                            //    .END;
                            //EntityQuery<SuperMarketDAL.Entitys.GoodsStock>.ExecuteOql(q, db);
                            //V5 版本扩展后，支持下面的写法：
                            int count = OQL.From(stock)
                                    .UpdateSelf('-', stock.Stocks)
                                    .Where(stock.GoodsID)
                                .END
                                .Execute(db);

                        }
                    }

                    EntityQuery<GoodsSellDetail> queryDetail = new EntityQuery<GoodsSellDetail>(db);
                    queryDetail.Insert(note.GoodsSellDetails);

                    //更新会员的积分
                    if (integral > 0)
                    {
                        SuperMarketDAL.Entitys.CustomerContactInfo ccInfo = new CustomerContactInfo();
                        ccInfo.CustomerID = customer.CustomerID;
                        ccInfo.Integral = integral;
                        //OQL qc = OQL.From(ccInfo)
                        //        .UpdateSelf('+', ccInfo.Integral )
                        //        .Where(ccInfo.CustomerID )
                        //        .END;
                        //EntityQuery<SuperMarketDAL.Entitys.GoodsStock>.ExecuteOql(qc, db);
                        OQL.From(ccInfo)
                                .UpdateSelf('+', ccInfo.Integral)
                                .Where(ccInfo.CustomerID)
                        .END
                        .Execute(db);
                    }
                }
                db.Commit();
            }
            catch (Exception ex)
            {
                db.Rollback();
                throw new Exception("插入销售记录失败，内部错误原因：" + ex.Message);
            }
            return note;
        }
    }
}
