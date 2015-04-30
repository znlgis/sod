using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PWMIS.DataMap.Entity;

namespace SODTest
{
    /// <summary>
    /// OQL 多实体类查询 动态条件构造测试 ，原始程序由网友  红枫星空  提供
    /// <seealso cref="http://www.cnblogs.com/bluedoctor/p/3225176.html"/>
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            SalesOrder order = new SalesOrder();
            //model.iOrderTypeID = "123";
            BCustomer customer = new BCustomer();

            //请注意方法 GetCondtion1，GetCondtion2,GetCondtion3 中变量 iCityID 的不同而带来的构造条件语句的不同
            OQLCompareFunc<BCustomer, SalesOrder> cmpFun = GetCondtion1();

            OQL q = OQL.From(order)
                    .LeftJoin(customer).On(order.iCustomerID, customer.ISID)
                    .Select()
                    .Where(cmpFun)
                    .OrderBy(order.iBillID, "desc")
                .END;

            Console.WriteLine(q);
            Console.WriteLine(q.PrintParameterInfo());
            //此OQL 可以由 EntityContainer 对象的方法执行
            Console.ReadLine();
        }

        private static OQLCompareFunc<BCustomer, SalesOrder> GetCondtion1()
        {
            OQLCompareFunc<BCustomer, SalesOrder> cmpFun = (cmp, C, S) =>
            {
                OQLCompare cmpResult = null;
                cmpResult = cmp.Comparer(S.iBillID, OQLCompare.CompareType.Equal, 1);

                if (!string.IsNullOrEmpty(S.iOrderTypeID))
                    cmpResult = cmpResult & cmp.Comparer(S.iOrderTypeID, OQLCompare.CompareType.Equal, S.iOrderTypeID);

                int iCityID = 30;
                //由于调用了关联实体类的 S.iOrderTypeID 用于条件比较，所以下面的比较需要注意：
                //必须确保 Comparer 方法第一个参数调用为实体类属性，而不是待比较的值
                //    且第一个参数的值不能等于第三个参数的值，否则需要调用NewCompare() 方法
                cmpResult = cmpResult & cmp.Comparer(C.iCityID, OQLCompare.CompareType.Equal, iCityID);
                return cmpResult;
            };
            return cmpFun;
        }

        private static OQLCompareFunc<BCustomer, SalesOrder> GetCondtion2()
        {
            OQLCompareFunc<BCustomer, SalesOrder> cmpFun = (cmp, C, S) =>
            {
                OQLCompare cmpResult = null;
                cmpResult = cmp.Comparer(S.iBillID, OQLCompare.CompareType.Equal, 1);

                if (!string.IsNullOrEmpty(S.iOrderTypeID))
                    cmpResult = cmpResult & cmp.Comparer(S.iOrderTypeID, OQLCompare.CompareType.Equal, S.iOrderTypeID);

                int iCityID = 0;
                //由于调用了关联实体类的 S.iOrderTypeID 用于条件比较【上面的IsNullOrEmpty 调用】，并且C.iCityID==iCityID==0 ，
                //所以下面需要调用 cmp.NewCompare()，以清除OQL字段堆栈中的数据对Comparer 方法的影响 
                //感谢网友 红枫星空 发现此问题
                //如果 C.iCityID != iCityID ,尽管上面调用了关联实体类的属性，但 Comparer 方法不受影响，也不需要调用 NewCompare 方法
                cmpResult = cmpResult & cmp.NewCompare().Comparer(C.iCityID, OQLCompare.CompareType.Equal, iCityID);
                return cmpResult;
            };
            return cmpFun;
        }

        private static OQLCompareFunc<BCustomer, SalesOrder> GetCondtion3()
        {
            OQLCompareFunc<BCustomer, SalesOrder> cmpFun = (cmp, C, S) =>
            {
                OQLCompare cmpResult = null;
                cmpResult = cmp.Comparer(S.iBillID, OQLCompare.CompareType.Equal, 1);

                if (!string.IsNullOrEmpty(S.iOrderTypeID))
                    cmpResult = cmpResult & cmp.Comparer(S.iOrderTypeID, OQLCompare.CompareType.Equal, S.iOrderTypeID);
                else
                    cmp.NewCompare();

                int iCityID = 0;
                //由于调用了关联实体类的 S.iOrderTypeID 用于条件比较【上面的IsNullOrEmpty 调用】，并且C.iCityID==iCityID==0 ，
                //所以下面需要调用 cmp.NewCompare()，以清除OQL字段堆栈中的数据对Comparer 方法的影响 
                //感谢网友 红枫星空 发现此问题
                //如果 C.iCityID != iCityID ,尽管上面调用了关联实体类的属性，但 Comparer 方法不受影响，也不需要调用 NewCompare 方法
                cmpResult = cmpResult & cmp.Comparer(C.iCityID, OQLCompare.CompareType.Equal, iCityID);
                return cmpResult;
            };
            return cmpFun;
        }
    }
}
