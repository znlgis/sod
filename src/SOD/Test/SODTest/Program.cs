using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Adapter;
using PWMIS.DataProvider.Data;
using SampleORMTest;

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
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            AdoHelper db = MyDB.GetDBHelperByConnectionName("local");
            InitData(db, watch);

            long[] useTime1 = new long[10];
            long[] useTime2 = new long[10];
            long[] useTime3 = new long[10];

            for (int i = 0; i < 10; i++)
            {
                useTime1[i]= HandQuery(db, watch);
                System.Threading.Thread.Sleep(1000); //便于观察CPU、内存等资源变化

                useTime2[i] = EntityQuery(db, watch);
                System.Threading.Thread.Sleep(1000);

                useTime3[i] = EntityQuery2(db, watch);
                System.Threading.Thread.Sleep(1000);

                Console.WriteLine("run test No.{0},sleep 1000 ms", i + 1);
                Console.WriteLine();
            }
            //去掉热身的第一次
            useTime1[0] = 0;
            useTime2[0] = 0;
            useTime3[0] = 0;
            Console.WriteLine("Avg HandQuery={0} ms, \r\n Avg SOD EntityQuery={1} ms,\r\n Avg EntityQuery2={2} ms"
                ,useTime1.Average(),useTime2.Average(),useTime3.Average());
            
            Console.ReadLine();
        }

        //手写DataReader查询
        private static long HandQuery(AdoHelper db, System.Diagnostics.Stopwatch watch)
        {
            watch.Restart();
            string sql = "select  UserID, Name, Pwd, RegistedDate from Tb_User";
            IList<UserDto> list = db.ExecuteMapper(sql).MapToList<UserDto>(reader => new UserDto
            {
                ID = reader.IsDBNull(0)? default(int): reader.GetInt32(0),
                Name = reader.IsDBNull(1) ? default(string) : reader.GetString(1),
                Pwd = reader.IsDBNull(2) ? default(string) : reader.GetString(2),
                RegistedDate = reader.IsDBNull(3) ? default(DateTime) : reader.GetDateTime(3)
            });
            watch.Stop();
            Console.WriteLine("HandQuery List (100000 item) 耗时：(ms)" + watch.ElapsedMilliseconds);
            return watch.ElapsedMilliseconds;
        }

        //SOD 先有查询方式
        private static long EntityQuery(AdoHelper db, System.Diagnostics.Stopwatch watch)
        {
            watch.Restart();
            User user = new User();
            OQL q = OQL.From(user).Select(user.ID, user.Name, user.Pwd, user.RegistedDate).END;
            //q.Limit(5000);
            var list = EntityQuery<User>.QueryList(q, db);
            watch.Stop();
            Console.WriteLine("SOD QueryList List (100000 item) 耗时：(ms)" + watch.ElapsedMilliseconds);
            return watch.ElapsedMilliseconds;
        }

        //模拟手写DataReader,尝试优化的方式
        private static long EntityQuery2(AdoHelper db, System.Diagnostics.Stopwatch watch)
        {
            watch.Restart();
            string sql = "select  UserID, Name, Pwd, RegistedDate from Tb_User";


            Action<IDataReader, int, object[]> readInt = (r, i, o) => { if (r.IsDBNull(i))  o[i] = DBNull.Value;  else  o[i] = r.GetInt32(i); };
            Action<IDataReader, int, object[]> readString = (r, i, o) => { if (r.IsDBNull(i)) o[i] = DBNull.Value; else o[i] = r.GetString(i); };
            Action<IDataReader, int, object[]> readDateTime = (r, i, o) => { if (r.IsDBNull(i)) o[i] = DBNull.Value; else o[i] = r.GetDateTime(i); };
            
            Action<IDataReader, int, object[]>[] readerActions = {
                 readInt,readString,readString,readDateTime
            };

            //User userEntity = new User();
            //IList<User> list = db.ExecuteMapper(sql).MapToList<User>(reader => 
            //{
            //    User item = (User)userEntity.Clone();
            //    for (int i = 0; i < readerActions.Length; i++)
            //    {
            //        readerActions[i](reader, i, item.PropertyValues);
            //    }
            //    return item;
            //}
            //);
            string tableName = "";
            User entity = new User();
            IDataReader reader = db.ExecuteDataReader(sql);
            List<User> list = new List<User>();
            using (reader)
            {
                if (reader.Read())
                {
                    int fcount = reader.FieldCount;
                    string[] names = new string[fcount];

                    for (int i = 0; i < fcount; i++)
                        names[i] = reader.GetName(i);
                    User t0 = new User();
                    if (!string.IsNullOrEmpty(tableName))
                        t0.MapNewTableName(tableName);
                    //正式，下面放开
                    // t0.PropertyNames = names;
                    do
                    {
                        User item = (User)t0.Clone(false);
                        for (int i = 0; i < readerActions.Length; i++)
                        {
                            readerActions[i](reader, i, item.PropertyValues);
                        }

                        list.Add(item);
                    } while (reader.Read());

                }
            }

            //return list;
            watch.Stop();
            Console.WriteLine("EntityQuery2 List (10000 item) 耗时：(ms)" + watch.ElapsedMilliseconds);
            return watch.ElapsedMilliseconds;
        }

        private static void InitData(AdoHelper db, System.Diagnostics.Stopwatch watch)
        {
            //自动创建数据库和表
            LocalDbContext context = new LocalDbContext();
            Console.WriteLine("需要初始化数据吗？(Y/N) [请勿重复初始化，否则出错。]");
            string input= Console.ReadLine();
            if (input.ToLower() != "Y") return;
            Console.WriteLine("正在初始化数据，请稍后。。。。");
            watch.Restart();

            List<User> batchList = new List<User>();
            for (int i = 0; i < 100000; i++)
            {
                User zhang_yeye = new User() { ID = 1000 + i, Name = "zhang yeye" + i, Pwd = "pwd" + i };
                //count += EntityQuery<User>.Instance.Insert(zhang_yeye);//采用泛型 EntityQuery 方式插入数据
                batchList.Add(zhang_yeye);
            }
            watch.Stop();
            Console.WriteLine("准备数据 耗时：(ms)" + watch.ElapsedMilliseconds);

            watch.Restart();
            int count = EntityQuery<User>.Instance.QuickInsert(batchList);
            watch.Stop();
            Console.WriteLine("QuickInsert List (100000 item) 耗时：(ms)" + watch.ElapsedMilliseconds);
            System.Threading.Thread.Sleep(1000);
        }

        private static void TestOQL()
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
