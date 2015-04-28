using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PWMIS.DataMap.Entity;

namespace SODTest
{
    /// <summary>
    /// OQL 多实体类查询 动态条件构造测试 网友  红枫星空  提供
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            SalesOrder model = new SalesOrder();
            //model.iOrderTypeID = "123";

            //string orderTypeID = model.iOrderTypeID;
            BCustomer bCustomer = new BCustomer();

            OQLCompareFunc<SalesOrder,BCustomer> cmpFun = (cmp,S,C) =>
            {
                OQLCompare cmpResult = null;
                cmpResult = cmp.Comparer<int>(S.iBillID, OQLCompare.CompareType.Equal, 0);
                if (!string.IsNullOrEmpty(S.iOrderTypeID))
                    cmpResult = cmpResult & cmp.Comparer<string>(S.iOrderTypeID, OQLCompare.CompareType.Equal, S.iOrderTypeID);

                int iCityID = 39;
                //由于调用了关联实体类的 S.iOrderTypeID 用于条件比较，所以下面需要调用 cmp.NewCompare()
                //cmpResult = cmpResult & cmp.NewCompare().Comparer<int>(C.iCityID, OQLCompare.CompareType.Equal, iCityID);
                //感谢网友 红枫星空 发现此问题

                //或者继续采用下面的写法，但是必须确定Comparer 方法第一个参数调用为实体类属性，而不是待比较的值
                cmpResult = cmpResult & cmp.Comparer<int>(C.iCityID, OQLCompare.CompareType.Equal, iCityID);
                return cmpResult;
            };
           

            OQL oQL = OQL.From(model).LeftJoin(bCustomer).On(model.iCustomerID, bCustomer.ISID)
                .Select()
                .Where(cmpFun)
                .OrderBy(model.iBillID, "desc")
                .END;

            Console.WriteLine(oQL);
            Console.WriteLine(oQL.PrintParameterInfo());
            Console.ReadLine();
        }
    }
}
