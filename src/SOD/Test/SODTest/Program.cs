using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using PWMIS.Core;
using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Adapter;
using PWMIS.DataProvider.Data;
using SampleORMTest;

namespace SODTest
{
    /// <summary>
    ///     OQL 多实体类查询 动态条件构造测试 ，原始程序由网友  红枫星空  提供
    ///     <seealso cref="http://www.cnblogs.com/bluedoctor/p/3225176.html" />
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            //AdoHelper 基础使用
            //AdoHelper db2 = MyDB.GetDBHelperByConnectionName("local2");
            var db2 = AdoHelper.CreateHelper("local2");
            //异常处理示例
            var sql_createUser = @"
Create table [TbUser](
[ID] int identity primary key,
[Name] nvarchar(100),
[LoginName] nvarchar(50),
[Password] varchar(50),
[Sex] bit,
[BirthDate] datetime
)";
            try
            {
                db2.ExecuteNonQuery(sql_createUser);
                Console.WriteLine("表[TbUser] 创建成功！");
            }
            catch (QueryException qe)
            {
                Console.WriteLine("SOD查询错误，错误原因：{0}", qe.InnerException.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("错误：{0}", ex.Message);
            }

            //微型ORM
            var sql_query = "SELECT [ID],[Name],[Sex],[BirthDate] FROM [TbUser] WHERE [LoginName]={0}";
            var mapUsers = db2.ExecuteMapper(sql_query, "zhangsan")
                .MapToList(reader => new
                {
                    ID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Sex = reader.GetBoolean(2),
                    BirthDate = reader.GetDateTime(3)
                });

            var userList = db2.QueryList<UserInfo>(sql_query, "zhangsan");

            //参数化查询
            var sql_insert =
                "INSERT INTO [TbUser] ([Name],[LoginName],[Password],[Sex],[BirthDate]) VALUES(@Name,@LoginName,@Password,@Sex,@BirthDate)";
            IDataParameter[] paras =
            {
                db2.GetParameter("Name", "张三"),
                db2.GetParameter("LoginName", "zhangsan"),
                db2.GetParameter("Password", "888888"),
                db2.GetParameter("Sex", true),
                db2.GetParameter("BirthDate", new DateTime(1990, 2, 1))
            };

            var rc = db2.ExecuteNonQuery(sql_insert, CommandType.Text, paras);
            if (rc > 0)
                Console.WriteLine("插入数据成功！用户名：{0}", paras[0].Value);


            //------------SOD ORM 示例---------------------------------------------
            //GOQL示例，适合单表查询
            //GOQL简单示例
            //GOQL使用接口类型进行查询
            var goql = OQL.FromObject<ITbUser>()
                .Select()
                .Where((cmp, obj) => cmp.Comparer(obj.LoginName, "=", "zhangsan"))
                .END;
            var list1 = goql.ToList(db2);

            //GOQL使用实体类类型进行查询
            var list11 = OQL.FromObject<UserEntity2>()
                .Select()
                .Where((cmp, obj) => cmp.Comparer(obj.LoginName, "=", "zhangsan"))
                .END
                .ToList(db2);

            //GOQL复杂示例
            var list2 = OQL.FromObject<ITbUser>()
                .Select(s => new object[] { s.ID, s.Name, s.Sex, s.BirthDate }) //选取指定字段属性查询
                .Where((cmp, obj) => cmp.Property(obj.LoginName) == "zhangsan") //使用操作符重载的条件比较
                .OrderBy((order, obj) => order.Desc(obj.ID))
                .ToList(db2);

            //OQL查询示例
            var ue = new UserEntity();
            ue.LoginName = "zhangsan";
            ue.Password = "888888";
            //OQL简单查询示例
            var oql = OQL.From(ue)
                .Select()
                .Where(ue.LoginName, ue.Password)
                .END;
            var userObj = EntityQuery<UserEntity>.QueryObject(oql, db2);

            var userObj2 = OQL.FromObject<UserEntity>()
                .Select()
                .Where((cmp, obj) => cmp.Comparer(obj.LoginName, "=", "zhangsan") &
                                     cmp.Comparer(obj.Password, "=", "888888"))
                .END
                .ToObject(db2);

            var userObj3 = OQL.FromObject<UserEntity>()
                .Select()
                .Where((cmp, obj) => (cmp.Property(obj.LoginName) == "zhangsan") &
                                     (cmp.Property(obj.Password) == "888888"))
                .END
                .ToObject(db2);

            var list3 = EntityQuery<UserEntity>.QueryList(oql, db2);

            //OQL复杂查询示例
            var oql2 = OQL.From(ue)
                .Select(ue.ID, ue.Name, ue.Sex, ue.BirthDate)
                .Where(cmp => (cmp.Property(ue.LoginName) == "zhangsan") & cmp.EqualValue(ue.Password))
                .OrderBy(order => order.Desc(ue.ID))
                .END;
            oql2.Limit(5, 1);
            var list4 = EntityQuery<UserEntity>.QueryList(oql2, db2);

            ue.Password = "8888";
            var oql_update = OQL.From(ue).Update(ue.Password).END;
            var ru = EntityQuery<UserEntity>.ExecuteOql(oql_update, db2);

            //增删改数据
            var ue2 = new UserEntity();
            ue2.LoginName = "lisi";
            ue2.Password = "8888";
            ue2.Name = "李四";
            var ic = EntityQuery<UserEntity>.Instance.Insert(ue2, db2);
            if (ic > 0)
                Console.WriteLine("保存数据成功，用户ID={0}", ue2.ID);

            ue2.BirthDate = new DateTime(1990, 1, 2);
            ue2.Sex = false;
            var uc = EntityQuery<UserEntity>.Instance.Update(ue2, db2);
            if (uc > 0)
                Console.WriteLine("保存数据成功，用户ID={0}", ue2.ID);

            ue2.PrimaryKeys.Clear();
            ue2.PrimaryKeys.Add("LoginName");
            var dc = EntityQuery<UserEntity>.Instance.Delete(ue2, db2);
            if (dc > 0)
                Console.WriteLine("删除用户[{0}]成功！", ue2.LoginName);


            //自动创建表
            var db2_ctx = new SimpleDbContext();
            //使用事务添加实体对象
            var order1 = new SimpleOrderEntity();
            order1.OrderID = CommonUtil.NewSequenceGUID();
            order1.OrderName = "笔记本订单_某想XL型号_" + DateTime.Now.ToString("yyyyMMdd");
            order1.UserID = 1;
            order1.OrderDate = DateTime.Now;
            order1.OrderPrice = 5000;

            var orderItems = new[]
            {
                new SimpleOrderItemEntity
                {
                    OrderID = order1.OrderID,
                    GoodsID = "123456_7890_abc",
                    GoodsName = "某想XL型号",
                    UnitPrice = 4500,
                    Number = 1
                },
                new SimpleOrderItemEntity
                {
                    OrderID = order1.OrderID,
                    GoodsID = "1526656_7670_bcd",
                    GoodsName = "蓝牙键盘",
                    UnitPrice = 500,
                    Number = 1
                }
            };
            //插入订单
            var addResult = db2_ctx.Transaction(ctx =>
            {
                ctx.Add(order1);
                ctx.AddList(orderItems);
            }, out var errorMessage);
            if (addResult)
                Console.WriteLine("保存订单信息成功!");
            else
                Console.WriteLine("保存订单信息失败，原因：{0}", errorMessage);

            //更新订单
            //方式一：使用DbContext方式
            order1.OrderPrice = 4999;
            var ur1 = db2_ctx.Update(order1);
            if (ur1 > 0)
                Console.WriteLine("订单价格更新成功，点单号：{0}，价格：{1}", order1.OrderID, order1.OrderPrice);
            //方式二：使用EntityQuery方式
            order1.OrderPrice = 4998;
            var ur2 = EntityQuery<SimpleOrderEntity>.Instance.Update(order1, db2);
            if (ur2 > 0)
                Console.WriteLine("订单价格更新成功，点单号：{0}，价格：{1}", order1.OrderID, order1.OrderPrice);

            //查询指定用户的订单
            var order2 = new SimpleOrderEntity { UserID = 1 };
            var oql_order = OQL.From(order2)
                .Select()
                .Where(order2.UserID)
                .END;
            var list_order = EntityQuery<SimpleOrderEntity>.QueryListWithChild(oql_order, db2);

            //多实体类联合查询示例
            //查询最近10条购买了笔记本的订单用户记录，包括用户年龄、性别
            var soe = new SimpleOrderEntity();
            //UserEntity ue = new UserEntity();
            var oql_OrderUser = OQL.From(soe)
                .InnerJoin(ue).On(soe.UserID, ue.ID)
                .Select()
                .Where(cmp => cmp.Comparer(soe.OrderName, "like", "笔记本订单%"))
                .OrderBy(soe.OrderID, "desc")
                .END;

            oql_OrderUser.Limit(10);

            var ec = new EntityContainer(oql_OrderUser, db2);
            var listView = ec.MapToList(() => new
            {
                soe.OrderID,
                soe.OrderName,
                OrderPrice = soe.OrderPrice.ToString("#0.00") + "￥",
                UserID = ue.ID,
                UserName = ue.Name,
                Sex = ue.Sex ? "男" : "女",
                UserAge = DateTime.Now.Year - ue.BirthDate.Year,
                soe.OrderDate
            });

            Console.ReadKey();
        }

        private static void PerformanceTest()
        {
            var watch = new Stopwatch();
            watch.Start();
            var db = MyDB.GetDBHelperByConnectionName("local");
            InitData(db, watch);

            var useTime1 = new long[10];
            var useTime2 = new long[10];
            var useTime3 = new long[10];
            var useTime4 = new long[10];

            for (var i = 0; i < 10; i++)
            {
                useTime1[i] = HandQuery(db, watch);
                Thread.Sleep(1000); //便于观察CPU、内存等资源变化

                useTime2[i] = QueryPOCO(db, watch);
                Thread.Sleep(1000);

                useTime3[i] = EntityQuery(db, watch);
                Thread.Sleep(1000);

                useTime4[i] = EntityQuery2(db, watch);
                Thread.Sleep(1000);

                Console.WriteLine("run test No.{0},sleep 1000 ms", i + 1);
                Console.WriteLine();
            }

            //去掉热身的第一次
            useTime1[0] = 0;
            useTime2[0] = 0;
            useTime3[0] = 0;
            useTime4[0] = 0;
            Console.WriteLine(
                "Avg HandQuery={0} ms, \r\n Avg QueryPOCO={1} ms, \r\n Avg SOD EntityQuery={2} ms,\r\n Avg EntityQuery2={3} ms"
                , useTime1.Average(), useTime2.Average(), useTime3.Average(), useTime4.Average());

            Console.ReadLine();
        }

        //手写DataReader查询
        private static long HandQuery(AdoHelper db, Stopwatch watch)
        {
            watch.Restart();
            var sql = "select  UserID, Name, Pwd, RegistedDate from Tb_User1";
            var list = db.ExecuteMapper(sql).MapToList(reader => new UserDto
            {
                UserID = reader.IsDBNull(0) ? default : reader.GetInt32(0),
                Name = reader.IsDBNull(1) ? default : reader.GetString(1),
                Pwd = reader.IsDBNull(2) ? default : reader.GetString(2),
                RegistedDate = reader.IsDBNull(3) ? default : reader.GetDateTime(3)
            });
            watch.Stop();
            Console.WriteLine("HandQuery List (100000 item) 耗时：(ms)" + watch.ElapsedMilliseconds);
            return watch.ElapsedMilliseconds;
        }

        private static long QueryPOCO(AdoHelper db, Stopwatch watch)
        {
            watch.Restart();
            var sql = "select  UserID, Name, Pwd, RegistedDate from Tb_User1";
            IList<UserDto> list = db.QueryList<UserDto>(sql);
            watch.Stop();
            Console.WriteLine("QueryPOCO List (100000 item) 耗时：(ms)" + watch.ElapsedMilliseconds);
            return watch.ElapsedMilliseconds;
        }

        //SOD 先有查询方式
        private static long EntityQuery(AdoHelper db, Stopwatch watch)
        {
            watch.Restart();
            var user = new User();
            var q = OQL.From(user).Select(user.ID, user.Name, user.Pwd, user.RegistedDate).END;
            //q.Limit(5000);
            var list = EntityQuery<User>.QueryList(q, db);
            watch.Stop();
            Console.WriteLine("SOD QueryList List (100000 item) 耗时：(ms)" + watch.ElapsedMilliseconds);
            return watch.ElapsedMilliseconds;
        }

        //模拟手写DataReader,尝试优化的方式，证明类型化读取器遇到装箱，效率较慢。
        private static long EntityQuery2(AdoHelper db, Stopwatch watch)
        {
            watch.Restart();
            var sql = "select  UserID, Name, Pwd, RegistedDate from Tb_User1";

            //Action<IDataReader, int, object[]> readInt = (r, i, o) => { if (r.IsDBNull(i)) o[i] = DBNull.Value; else o[i] = r.GetInt32(i); };
            //Action<IDataReader, int, object[]> readString = (r, i, o) => { if (r.IsDBNull(i)) o[i] = DBNull.Value; else o[i] = r.GetString(i); };
            //Action<IDataReader, int, object[]> readDateTime = (r, i, o) => { if (r.IsDBNull(i)) o[i] = DBNull.Value; else o[i] = r.GetDateTime(i); };

            //Action<IDataReader, int, object[]>[] readerActions = {
            //     readInt,readString,readString,readDateTime
            //};

            var tableName = "";
            var entity = new User();
            var reader = db.ExecuteDataReader(sql);
            var list = new List<User>();
            using (reader)
            {
                if (reader.Read())
                {
                    var fcount = reader.FieldCount;
                    var names = new string[fcount];

                    for (var i = 0; i < fcount; i++)
                        names[i] = reader.GetName(i);
                    var t0 = new User();
                    if (!string.IsNullOrEmpty(tableName))
                        t0.MapNewTableName(tableName);
                    //正式，下面放开
                    // t0.PropertyNames = names;
                    //
                    Action<int, object[]> readInt = (i, o) =>
                    {
                        if (reader.IsDBNull(i)) o[i] = DBNull.Value;
                        else o[i] = reader.GetInt32(i);
                    };
                    Action<int, object[]> readString = (i, o) =>
                    {
                        if (reader.IsDBNull(i)) o[i] = DBNull.Value;
                        else o[i] = reader.GetString(i);
                    };
                    Action<int, object[]> readDateTime = (i, o) =>
                    {
                        if (reader.IsDBNull(i)) o[i] = DBNull.Value;
                        else o[i] = reader.GetDateTime(i);
                    };
                    Action<int, object[]>[] readerActions =
                    {
                        readInt, readString, readString, readDateTime
                    };
                    //
                    do
                    {
                        var item = (User)t0.Clone(false);
                        for (var i = 0; i < readerActions.Length; i++) readerActions[i](i, item.PropertyValues);

                        list.Add(item);
                    } while (reader.Read());
                }
            }

            //return list;
            watch.Stop();
            Console.WriteLine("EntityQuery2 List (10000 item) 耗时：(ms)" + watch.ElapsedMilliseconds);
            return watch.ElapsedMilliseconds;
        }

        private static void InitData(AdoHelper db, Stopwatch watch)
        {
            //自动创建数据库和表
            var context = new LocalDbContext();
            Console.WriteLine("需要初始化数据吗？(Y/N) ");
            var input = Console.ReadLine();
            if (input.ToLower() != "y") return;
            Console.WriteLine("正在初始化数据，请稍后。。。。");
            context.TruncateTable<User>();
            Console.WriteLine("...");
            watch.Restart();
            var batchList = new List<User>();
            for (var i = 0; i < 100000; i++)
            {
                var zhang_yeye = new User
                    { ID = 1000 + i, Name = "zhang yeye" + i, Pwd = "pwd" + i, RegistedDate = DateTime.Now };
                //count += EntityQuery<User>.Instance.Insert(zhang_yeye);//采用泛型 EntityQuery 方式插入数据
                batchList.Add(zhang_yeye);
            }

            watch.Stop();
            Console.WriteLine("准备数据 耗时：(ms)" + watch.ElapsedMilliseconds);

            watch.Restart();
            var count = EntityQuery<User>.Instance.QuickInsert(batchList);
            watch.Stop();
            Console.WriteLine("QuickInsert List (100000 item) 耗时：(ms)" + watch.ElapsedMilliseconds);
            Thread.Sleep(1000);
        }

        private static void TestOQL()
        {
            var order = new SalesOrder();
            //model.iOrderTypeID = "123";
            var customer = new BCustomer();

            //请注意方法 GetCondtion1，GetCondtion2,GetCondtion3 中变量 iCityID 的不同而带来的构造条件语句的不同
            var cmpFun = GetCondtion1();

            var q = OQL.From(order)
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

                var iCityID = 30;
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

                var iCityID = 0;
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

                var iCityID = 0;
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