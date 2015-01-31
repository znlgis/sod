using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperMarketDAL.Entitys;
using PWMIS.DataMap.Entity;
using SuperMarketModel.ViewModel;

namespace SuperMarketBLL
{
    public class GoodsManageBIZ
    {
        #region 商品基础信息管理
        /// <summary>
        /// 增加商品信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool AddGoodsBaseInfo(GoodsBaseInfo info)
        {
           return  EntityQuery<GoodsBaseInfo>.Instance.Insert(info)>0;
        }

        /// <summary>
        /// 修改商品信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool UpdateGoodsBaseInfo(GoodsBaseInfo info)
        {
            return EntityQuery<GoodsBaseInfo>.Instance.Update (info) > 0;
            
        }

        /// <summary>
        /// 删除商品存货信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool DeleteGoodsStock(GoodsStock info)
        {
            return EntityQuery<GoodsStock>.Instance.Delete(info) > 0;

        }

        /// <summary>
        /// 保存商品信息
        /// </summary>
        /// <param name="info">商品信息</param>
        /// <returns></returns>
        /// <remarks>该方法会自动探测实体对象是新增的还是修改的，如果要求较快速度，请调用Insert或者Update方法</remarks>
        public bool SaveGoodsBaseInfo(GoodsBaseInfo info)
        {
            EntityQuery<GoodsBaseInfo> query = new EntityQuery<GoodsBaseInfo>(info);
            return query.SaveAllChanges()>0;
        }

        private List<GoodsBaseInfoVM> GetGoodsBaseInfoList(OQL oql)
        {
            List<GoodsBaseInfo> list = EntityQuery<GoodsBaseInfo>.QueryList(oql);
            //将Entity对象转化成ViewModel对象
            return list.ConvertAll<GoodsBaseInfoVM>(p =>
            {
                return new GoodsBaseInfoVM()
                {
                    CanUserMonth = p.CanUserMonth,
                    GoodsName = p.GoodsName,
                    Manufacturer = p.Manufacturer,
                    SerialNumber = p.SerialNumber
                };
            });
        }

        public List<GoodsBaseInfoVM> GetAllGoodsBaseInfo()
        {
            GoodsBaseInfo info = new GoodsBaseInfo();
            OQL q = OQL.From(info).Select().END;
            return GetGoodsBaseInfoList(q);
        }

        public List<GoodsBaseInfoVM> GetGoodsBaseInfoList(int pageSize, int pageNumber,int allCount)
        {
            GoodsBaseInfo info = new GoodsBaseInfo();
            OQL q = new OQL(info);
            q.Select()
                .OrderBy (info.SerialNumber ,"asc");
            q.Limit(pageSize, pageNumber);
            q.PageWithAllRecordCount = allCount;

            return GetGoodsBaseInfoList(q);
        }

        public int GetAllGoodsBaseInfoCount()
        {
            GoodsBaseInfo info = new GoodsBaseInfo();
            OQL q = new OQL(info);
            q.Select()
                .Count(info.SerialNumber,"Count");

            GoodsBaseInfo infoCount = EntityQuery<GoodsBaseInfo>.QueryObject(q);
            return Convert.ToInt32(infoCount.PropertyList("Count"));
        }

        /// <summary>
        /// 获取一条商品信息
        /// </summary>
        /// <param name="sn">条码号</param>
        /// <returns></returns>
        public GoodsBaseInfo GetGoodsBaseInfo(string sn)
        {
            GoodsBaseInfo info = new GoodsBaseInfo() { SerialNumber = sn };
            EntityQuery<GoodsBaseInfo>.Fill(info);
            return info;
        }

        /// <summary>
        /// 根据商品名称为顺序获取所有商品基本信息
        /// </summary>
        /// <returns></returns>
        public List<GoodsBaseInfoVM> GetAllGoodsBaseInfoOrderByNames()
        {
            //GoodsBaseInfo info = new GoodsBaseInfo();
            //OQL q = new OQL(info);
            //q.Select(info.SerialNumber,info.GoodsName,info.Manufacturer,info.CanUserMonth )
            //    .OrderBy(info.GoodsName, "asc");

            OQL q = OQL.FromObject<GoodsBaseInfo>()
                .Select(info=>new object[]{  
                    info.SerialNumber, 
                    info.GoodsName, 
                    info.Manufacturer, 
                    info.CanUserMonth
                })
                .OrderBy((o,info)=>o.Asc(info.GoodsName))
            .END;
            return GetGoodsBaseInfoList(q);
        }

        /// <summary>
        /// 获取指定商品名称下面的商品信息
        /// </summary>
        /// <param name="goodsName">商品名称</param>
        /// <returns></returns>
        public List<GoodsBaseInfoVM> GetGoodsBaseInfoWhithGoodsName(string goodsName)
        {
            GoodsBaseInfo info = new GoodsBaseInfo();
            info.GoodsName = goodsName;
            OQL q = new OQL(info);
            q.Select(info.SerialNumber, info.GoodsName, info.Manufacturer, info.CanUserMonth)
                .Where(info.GoodsName)
                .OrderBy(info.GoodsName, "asc");
            return GetGoodsBaseInfoList(q);
        }

        /// <summary>
        /// 获取商品名称分组
        /// </summary>
        /// <returns></returns>
        public List<string>  GetAllGoodsNames()
        {
            GoodsBaseInfo info = new GoodsBaseInfo();
            OQL q = new OQL(info);
            q.Select(info.GoodsName)
                .GroupBy(info.GoodsName)
                .OrderBy(info.GoodsName, "asc");
            List<GoodsBaseInfo> list = EntityQuery<GoodsBaseInfo>.QueryList(q);
            var result = list.ConvertAll<string>(p => p.GoodsName);
            return result;
        }

        #endregion

        /// <summary>
        /// 获取商品销售单视图
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GoodsSellNoteVM> GetGoodsSellNote()
        {
            GoodsSellNote note = new GoodsSellNote();
            Employee emp = new Employee();
            CustomerContactInfo cst=new CustomerContactInfo ();
            OQL joinQ = OQL.From(note)
                .InnerJoin(emp).On(note.SalesmanID, emp.WorkNumber)
                .InnerJoin(cst).On(note.CustomerID, cst.CustomerID)
                .Select(note.NoteID, cst.CustomerName, note.ManchinesNumber, emp.EmployeeName, note.SalesType, note.SellDate)
                .OrderBy(note.NoteID, "desc")
                .END;

            PWMIS.DataProvider.Data.AdoHelper db = PWMIS.DataProvider.Adapter.MyDB.GetDBHelper();
            EntityContainer ec = new EntityContainer(joinQ, db);  
            ec.Execute();
            //可以使用下面的方式获得各个成员元素列表
            //var noteList = ec.Map<GoodsSellNote>().ToList();
            //var empList = ec.Map<Employee>().ToList();
            //var cstList = ec.Map<CustomerContactInfo>().ToList();
            //直接使用下面的方式获得新的视图对象
            var result = ec.Map<GoodsSellNoteVM>(e =>
                {
                    e.NoteID = ec.GetItemValue<int>(0);
                    e.CustomerName = ec.GetItemValue<string>(1);
                    e.ManchinesNumber = ec.GetItemValue<string>(2);
                    e.EmployeeName = ec.GetItemValue<string>(3);
                    e.SalesType = ec.GetItemValue<string>(4);
                    e.SellDate = ec.GetItemValue<DateTime>(5);
                    return e;
                }
            );
            return result;
        }

        /// <summary>
        /// 获取商品销售价格信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GoodsSaleInfoVM> GetGoodsSaleInfo()
        {
            GoodsBaseInfo bInfo = new GoodsBaseInfo();
            GoodsStock stock = new GoodsStock();
            OQL joinQ = OQL.From(bInfo)
                .Join(stock).On(bInfo.SerialNumber, stock.SerialNumber)
                .Select(bInfo.GoodsName, bInfo.Manufacturer, bInfo.SerialNumber, stock.GoodsPrice, stock.MakeOnDate, bInfo.CanUserMonth, stock.Stocks, stock.GoodsID)
                .OrderBy(bInfo.GoodsName, "asc")
                .END;

            PWMIS.DataProvider.Data.AdoHelper db = PWMIS.DataProvider.Adapter.MyDB.GetDBHelper();
            EntityContainer ec = new EntityContainer(joinQ, db);
            ec.Execute();
            var result = ec.Map<GoodsSaleInfoVM>(e =>
                {
                    e.GoodsName = ec.GetItemValue<string>(0); 
                    e.Manufacturer = ec.GetItemValue<string>(1);
                    e.SerialNumber  = ec.GetItemValue<string>(2);
                    e.GoodsPrice  = ec.GetItemValue<decimal>(3);
                    e.MakeOnDate = ec.GetItemValue<DateTime>(4);
                    e.CanUserMonth = ec.GetItemValue<int>(5);
                    e.Stocks = ec.GetItemValue<int>(6);
                    e.GoodsID = ec.GetItemValue<int>(7);
                    return e;
                }
            );
            return result;
        }

        /// <summary>
        /// 获取所有的可售商品总数
        /// </summary>
        /// <returns></returns>
        public int GetGoodsStockCount()
        {
            GoodsStock stock = new GoodsStock();
            OQL q = new OQL(stock);
            q.Select()
                .Count(stock.Stocks, "库存数量")
                .Where(q.Condition.AND(stock.Stocks, ">", 0));

            stock = EntityQuery<GoodsStock>.QueryObject(q);
            return stock.Stocks;
        }

        private int GetOverDays(string serialNumber, int goodsID, out decimal goodsPrice, out int stockCount)
        {
            //计算距离过期时间
            GoodsBaseInfo bInfo = new GoodsBaseInfo();
            bInfo.SerialNumber = serialNumber;
            OQL q = OQL.From(bInfo)
                .Select(bInfo.CanUserMonth)
                .Where(bInfo.SerialNumber)
                .END;

            bInfo = EntityQuery<GoodsBaseInfo>.QueryObject(q);
            int canUseMonth = bInfo.CanUserMonth;

            GoodsStock stock = new GoodsStock();
            stock.GoodsID = goodsID;
            EntityQuery<GoodsStock>.Fill(stock);
            goodsPrice = stock.GoodsPrice;
            stockCount = stock.Stocks;
            //距离过期时间
            int overDays = canUseMonth *30 - DateTime.Now.Subtract(stock.MakeOnDate).Days;
            return overDays;
        }

        //该方法已经废弃，合并在 GetOverDays 中
        private int GetStockCount(int goodsID)
        {
            GoodsStock stock = new GoodsStock();
            stock.GoodsID = goodsID;
            OQL q = new OQL(stock);
            q.Select(stock.Stocks)
                    .Where(stock.GoodsID);

            stock = EntityQuery<GoodsStock>.QueryObject(q);
            int stockCount = stock.Stocks;
            return stockCount;
        }

        //private bool IsCustomer(string customerID)
        //{
        //    CustomerContactInfo info = new CustomerContactInfo();
        //    info.CustomerID = customerID;
        //    return EntityQuery<CustomerContactInfo>.Fill(info);
        //}

        /// <summary>
        /// 根据商品和顾客信息，计算销售的单价
        /// </summary>
        /// <param name="goodsID">商品编号</param>
        /// <param name="serialNumber">商品条码</param>
        /// <param name="buyNumber">购买的数量</param>
        /// <param name="customerID">客户号</param>
        /// <param name="integral">会员积分</param>
        /// <returns></returns>
        public decimal GoodsSellPrice(int goodsID, string serialNumber, int buyNumber, bool  isCustomer, out int integral)
        {
            /*
             * 价格计算策略：
             * 当库存大于100件的时候，对所有顾客享受98折折扣；
             * 当库存小于100大于50件的时候，对会员任然享受98折折扣；
             * 当库存小于等于50件的时候，不进行折扣；
             * 
             * 如果商品距离过期时间小于10天，进行95折促销；小于5天，进行9折促销，小于3天，进行8折促销，小于1天，7折促销；
             * 优先处理过期的折扣情况；
             * 
             * 一次购买数量大于10件或者金额大于3000的会员，在以上价格基础上再进行95折；
             * 
             * 如果商品有进行折扣，会员不累计积分
             */
            //计算商品库存
            int stockCount = 0;// GetStockCount(goodsID);
            decimal goodsPrice;
            integral = 0;

            int overDays = GetOverDays(serialNumber, goodsID, out goodsPrice, out stockCount);
            if (stockCount == 0)
                return 0;//库存为0，不可销售
            if (buyNumber == 0)
                return 0;
            if (buyNumber > stockCount)
                buyNumber = stockCount;

            //bool isCustomer = IsCustomer(customerID);

            decimal result=0m;
            if (overDays <= 10)
            {
                //根据过期时间计算折扣后的价格;
                if (overDays <= 1)
                    result = goodsPrice * 0.7m;
                else if (overDays <= 3)
                    result = goodsPrice * 0.8m;
                else if (overDays <= 5)
                    result = goodsPrice * 0.9m;
                else if (overDays <= 10)
                    result = goodsPrice * 0.95m;
                else
                    result = goodsPrice;
               

            }
            else
            {
                //根据库存计算折扣
                if (stockCount >= 100)
                    result = goodsPrice * 0.98m;
                else if (stockCount >= 50 && isCustomer)
                    result = goodsPrice * 0.98m;
                else
                    result = goodsPrice;
            }

            //会员根据购物数量等再计算折扣
            if ( (buyNumber >= 10 || result >= 3000m ) && isCustomer )
                result = result * 0.95m;
            //计算积分
            if( result == goodsPrice && isCustomer)
                integral=(int)result / 10;
            return result;
        }
    }
}
