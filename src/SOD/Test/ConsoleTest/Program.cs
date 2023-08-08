using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using LocalDB;
using MvcApplication1.Models;
using PDFNETClassLib.Model;
using PWMIS.Core;
using PWMIS.Core.Extensions;
using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Adapter;
using PWMIS.DataProvider.Data;
using RoadTeam.Model.CS;
using TranstarAuction.Repository.Entitys;

namespace ConsoleTest
{
    internal class Program
    {
        public delegate void SelectFieldFun(params object[] field);

        private static SelectFieldFun TestFun = param => { };

        private static void Main(string[] args)
        {
            Console.WriteLine("====**************** PDF.NET SOD 控制台测试程序 **************====");
            var coreAss = Assembly.GetAssembly(typeof(AdoHelper)); //获得引用程序集
            Console.WriteLine("框架核心程序集 PWMIS.Core Version:{0}", coreAss.GetName().Version);
            Console.WriteLine();
            Console.WriteLine(
                "  应用程序配置文件默认的数据库配置信息：\r\n  当前使用的数据库类型是：{0}\r\n  连接字符串为:{1}\r\n  请确保数据库服务器和数据库是否有效且已经初始化过建表脚本（项目下的2个sql脚本文件），\r\n继续请回车，退出请输入字母 Q ."
                , MyDB.Instance.CurrentDBMSType.ToString(), MyDB.Instance.ConnectionString);
            Console.WriteLine("=====Power by Bluedoctor,2015.2.8 http://www.pwmis.com/sqlmap ====");
            var read = Console.ReadLine();
            if (read.ToUpper() == "Q")
                return;


            Console.WriteLine("当前机器的分布式ID：{0}", CommonUtil.CurrentMachineID());
            Console.WriteLine("测试分布式ID：秒级有序");
            for (var i = 0; i < 50; i++)
            {
                Console.Write(CommonUtil.NewSequenceGUID());
                Console.Write(",");
            }

            Console.WriteLine();
            Console.WriteLine("测试分布式ID：唯一且有序");
            for (var i = 0; i < 20000; i++)
            {
                var seq = CommonUtil.NewUniqueSequenceGUID();
                if (i <= 50)
                {
                    Console.Write(CommonUtil.NewUniqueSequenceGUID());
                    Console.Write(",");
                }
            }

            Console.WriteLine();


            IDataParameter[] paraArr =
            {
                MyDB.Instance.GetParameter("P1", 111),
                MyDB.Instance.GetParameter("P2", "abc'ee<edde/>e"),
                MyDB.Instance.GetParameter("P3", DBNull.Value)
            };

            var str = DbParameterSerialize.Serialize(paraArr);
            Console.WriteLine("测试参数序列化：{0}", str);
            var paraArr2 = DbParameterSerialize.DeSerialize(str, MyDB.Instance);
            Console.WriteLine("测试反序列化成功！");

            var localDb = new LocalDbContext();
            var entitys = localDb.ResolveAllEntitys();

            localDb.CurrentDataBase.RegisterCommandHandle(new TransactionLogHandle());
            var user = new Table_User();
            user.Name = "zhang san";
            user.Height = 1.8f;
            user.Birthday = new DateTime(1980, 1, 1);
            user.Sex = true;
            localDb.Add(user);

            user.Name = "lisi";
            user.Height = 1.6f;
            user.Birthday = new DateTime(1982, 3, 1);
            user.Sex = false;
            localDb.Add(user);
            Console.WriteLine("测试 生成事务日志 成功！（此操作将写入事务日志信息到数据库中。）");

            //var logList = localDb.QueryList<MyCommandLogEntity>(OQL.From(new MyCommandLogEntity()).Select().END);
            var db = MyDB.GetDBHelperByConnectionName("local");
            var logList = OQL.From<MyCommandLogEntity>().Select().END.ToList(db);
            foreach (var log in logList)
            {
                var paras = DbParameterSerialize.DeSerialize(log.ParameterInfo, db);
                var count = db.ExecuteNonQuery(log.CommandText, log.CommandType, paras);
                Console.WriteLine("执行语句：{0} \r\n 受影响行数：{1}", log.CommandText, count);
            }

            Console.WriteLine("测试 运行事务日志 成功！（此操作将事务日志的操作信息回放执行。）");

            //写入10000条日志，有缓存，可能不会写完
            Console.WriteLine("测试日志写入10000 条信息...");
            var loger = new CommandLog();
            for (var t = 0; t <= 1; t++)
            {
                var thread = new Thread(WriteLog);
                thread.Name = "thread" + t;
                thread.Start(loger);
            }

            loger.Flush();
            Console.WriteLine("按任意键继续");
            Console.ReadLine();

            var etu = new EntityUser();
            var itu = etu.AsEntity();
            var dtt = itu.Birthday;


            //测试 AdoHelper的并发能力
            //for (int i = 0; i < 100; i++)
            //{
            //    System.Threading.Thread t = new System.Threading.Thread(
            //        new System.Threading.ParameterizedThreadStart(TestDataSetAndOQL));
            //    t.Name = "thread "+i;
            //    t.Start();

            //}

            //测试生成列的脚本
            var ecmd = new EntityCommand(new Table_User(), new SqlServer());
            var table_script = ecmd.CreateTableCommand;

            Console.Write("1，测试 OpenSession 长连接数据库访问...");
            TestDataSetAndOQL(null);
            Console.WriteLine("OK");
            //
            Console.WriteLine("2，测试OQL 转SQL...");
            var CsEvent = new TbCsEvent();
            CsEvent.EventID = 1;
            var oql = OQL.From(CsEvent)
                .Select(CsEvent.EventCheck, CsEvent.EventCheckInfo, CsEvent.EventCheckor, CsEvent.EventCheckTime)
                .Where(CsEvent.EventID)
                .END;
            Console.WriteLine(oql.ToString());
            Console.WriteLine("-----------------------");

            var CsEvent2 = new TbCsEvent();
            CsEvent.EventID = 1;
            var oql2 = OQL.From(CsEvent2)
                .Select(true, CsEvent2.EventCheck, CsEvent2.EventCheckInfo, CsEvent2.EventCheckor,
                    CsEvent2.EventCheckTime)
                .Where(CsEvent2.EventID)
                .END;
            Console.WriteLine(oql2.ToString());
            Console.WriteLine("-----------------------");
            Console.WriteLine("OK");
            //
            Console.Write("3，测试实体类动态增加虚拟属性...");
            var um1 = new UserModels();
            um1.AddPropertyName("TestName");
            um1["TestName"] = 123;
            var testi = (int)um1["TestName"];

            um1["TestName"] = "abc";
            var teststr = (string)um1["TestName"];
            Console.WriteLine("OK");
            //
            Console.Write("4，测试缓存...");
            var cache = MemoryCache<EntityBase>.Default;
            cache.Add("UserModels", um1);
            var cacheData = cache.Get("UserModels");
            cache.Remove("UserModels");
            Console.WriteLine("OK");
            //
            Console.Write("5，测试自动创建实体类数据库表...");
            AutoCreateEntityTable<LT_Users>();
            AutoCreateEntityTable<LT_UserRoles>();
            Console.WriteLine("OK");

            Console.WriteLine("------------测试暂时停止，回车继续运行------");
            Console.ReadLine();
            //return;

            Console.Write("6，测试实体类的外键查询...");
            TestEntityFK();
            Console.WriteLine("OK");
            Console.Write("7，测试实体类批量插入...");
            OqlInTest();
            Console.WriteLine("OK");

            Console.WriteLine("8，测试SOD POCO实体类性能...");
            Console.WriteLine("SELECT top 100000 UID,Sex,Height,Birthday,Name FROM Table_User");
            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine("-------------Testt No.{0}----------------", i + 1);
                TestPocoQuery();
            }

            Console.WriteLine("--------OK---------------");

            Console.Write("9，测试OQL IN 子查询...");
            TestInChild();
            Console.WriteLine("OK");
            //TestFun(1, 2, 3);

            Console.WriteLine("10，测试泛型 OQL --GOQL");
            TestGOQL();
            Console.WriteLine("OK");

            Console.WriteLine("11，测试OQL 批量更新（带条件更新）...");
            UpdateTest();
            Console.WriteLine("OK");

            Console.WriteLine("12，测试批量数据插入性能....");
            //InsertTest();


            Console.WriteLine("13，OQL 自连接...");
            OqlJoinTest();
            //
            Console.Write("14，根据接口类型，自动创建实体类测试...");
            TestDynamicEntity();
            Console.WriteLine("OK");

            //
            Console.WriteLine("15，Sql 格式化查询测试( SOD 微型ORM功能)...");
            AdoHelper dbLocal = new SqlServer();
            dbLocal.ConnectionString = MyDB.Instance.ConnectionString;
            //DataSet ds = dbLocal.ExecuteDataSet("SELECT * FROM Table_User WHERE UID={0} AND Height>={1:5.2}", 1, 1.80M);
            /*
             * 下面的写法过时
            var dataList = dbLocal.GetList(reader =>
            {
                return new
                {
                    UID=reader.GetInt32(0),
                    Name=reader.GetString(1)
                };
            }, "SELECT UID,Name FROM Table_User WHERE Sex={0} And Height>={0:5.2}",1, 1.60);
            */
            var dataList = dbLocal
                .ExecuteMapper("SELECT UID,Name FROM Table_User WHERE Sex={0} And Height>={1:5.2}", 1, 1.60)
                .MapToList(reader => new
                {
                    UID = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            Console.WriteLine("OK");

            //
            Console.Write("16，测试属性拷贝...");
            var vum = new V_UserModels();
            vum.BIGTEAM_ID = 123; //可空属性，如果目标对象不是的话，无法拷贝
            vum.REGION_ID = 456;
            vum.SMALLTEAM_ID = 789;

            var um = vum.CopyTo<UserModels>();
            Console.WriteLine("OK");

            //
            Console.Write("17，测试【自定义查询】的实体类...");
            var up = new UserPropertyView();
            var q11 = new OQL(up);
            var order = new OQLOrder(q11);
            q11.Select()
                .Where(q11.Condition.AND(up.PropertyName, "=", "总成绩").AND(up.PropertyValue, ">", 1000))
                .OrderBy(order.Asc(up.UID));
            var db11 = MyDB.GetDBHelperByConnectionName("local");
            var result = EntityQuery<UserPropertyView>.QueryList(q11, db11);
            //下面2行不是必须
            q11.Dispose();
            Console.WriteLine("OK");

            //EntityContainer ec = new EntityContainer(q11);
            //var ecResult = ec.MapToList(() => {
            //    return new { AAA = ec.GetItemValue<int>(0), BBB = ec.GetItemValue<string>(1) };
            //});

            /////////////////////////////////////////////////////
            Console.WriteLine("18，测试实体类【自动保存】数据...");
            TestAutoSave();

            Console.WriteLine("19，测试OQL上使用聚合函数...");
            OQLAvgTest();

            /////////////////测试事务////////////////////////////////////
            Console.WriteLine("20，测试测试事务...");
            TestTransaction();
            TestTransaction2();
            Console.WriteLine("事务测试完成！");
            Console.WriteLine("-------PDF.NET SOD 测试全部完成-------");

            Console.ReadLine();
        }

        private static void WriteLog(object obj)
        {
            var loger = obj as CommandLog;
            for (var i = 0; i < 100; i++)
            {
                var text = Thread.CurrentThread.Name + " write text " + i;
                loger.LogWriter.WriteLog("test", text);
                //Console.WriteLine(text);
            }
        }

        private static void TestTransaction()
        {
            var db = MyDB.GetDBHelper();
            var query = new EntityQuery<AuctionOperationLog>(db);

            var optLog = new AuctionOperationLog();
            optLog.OperaterID = 1000;
            optLog.Module = "Login";
            optLog.Operation = "登录成功1";
            optLog.LogSource = "PC";

            db.BeginTransaction();
            try
            {
                query.Insert(optLog);

                //必须设置为全部属性已经修改，否则仅会更新 Operation 字段
                optLog.ResetChanges(true);
                optLog.Operation = "退出登录";
                query.Insert(optLog);

                //optLog.Module = "Login";
                //OQL q = OQL.From(optLog).Select().Where(optLog.Module).END;
                var q = new OQL(optLog);
                //q.Select().Where(q.Condition.AND(optLog.Operation, "like", "%登录%"));

                q.Select().Count(optLog.OperaterID, ""); //使用空字符串参数，这样统计的值将放到 OperaterID 属性中
                //必须指定db参数，否则不再一个事务中，无法进行统计查询
                optLog = EntityQuery<AuctionOperationLog>.QueryObject(q, db);
                var allCount = optLog.OperaterID;

                //optLog 已经使用过，在生成OQL的查询前，必须使用新的实体对象，
                //       否则下面的查询仅会使用OperaterID 字段从而导致分页查询出错
                optLog = new AuctionOperationLog();
                q = new OQL(optLog);

                q.Select().OrderBy(optLog.Module, "asc").OrderBy(optLog.AtDateTime, "desc");
                q.Limit(10, 2);
                q.PageEnable = true;
                q.PageWithAllRecordCount = allCount;

                //查询列表并更新到数据库
                var list = EntityQuery<AuctionOperationLog>.QueryList(q, db);
                foreach (var logItem in list) logItem.AtDateTime = DateTime.Now;
                query.Update(list);

                db.Commit();

                Console.WriteLine("事务操作成功。");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);
                db.Rollback();
            }
        }

        private static void TestTransaction2()
        {
            using (var db = MyDB.GetDBHelper())
            {
                var query = new EntityQuery<AuctionOperationLog>(db);

                var optLog = new AuctionOperationLog();
                optLog.OperaterID = 1000;
                optLog.Module = "Login";
                optLog.Operation = "登录成功2";
                optLog.LogSource = "PC";
                //开启事务
                db.BeginTransaction();
                var count = query.Insert(optLog);

                //模拟数据操作失败，抛出异常
                if (count > 0)
                {
                    //throw new Exception("测试在事务过程中数据操作异常");
                    Console.WriteLine("测试在事务过程中数据操作异常 退出");
                    return;
                }

                //必须设置为全部属性已经修改，否则仅会更新 Operation 字段
                optLog.ResetChanges(true);
                optLog.Operation = "退出登录";
                query.Insert(optLog);

                db.Commit();
            }
        }

        private static void TestAutoSave()
        {
            var log = new AuctionOperationLog();
            log.OptID = 1;
            log.Module = "TestTest";
            log.Operation = "ppppppppp";
            log.LogSource = "test";
            EntityQuery<AuctionOperationLog>.Instance.FillEntity(log);
            var eq = new EntityQuery(log);
            //第一次，插入数据
            var ac = eq.Save(log.Module); //仅插入Module 字段，其它字段不插入，要求其它字段可为空。
            Console.WriteLine("测试插入部分字段，成功");

            var logQuery = new EntityQuery<AuctionOperationLog>(log, true);
            log.OperaterID = 999;
            log.Module = "Test";
            log.Operation = "Test Opt";
            log.LogSource = "Test";

            var affectCount = logQuery.Save();
            Console.WriteLine("自动保存成功(insert),id={0}", log.OptID);


            log.Operation = "Test Opt No.2";
            affectCount = logQuery.Save();
            Console.WriteLine("自动保存成功(update)");
        }

        /// <summary>
        ///     自动创建实体类表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private static void AutoCreateEntityTable<T>() where T : EntityBase, new()
        {
            var entity = new T();
            var ecmd = new EntityCommand(entity, MyDB.Instance);
            Console.WriteLine(ecmd.CreateTableCommand);
            //如果表表　不存在，则执行下面以行
            try
            {
                MyDB.Instance.ExecuteNonQuery(ecmd.CreateTableCommand);
                Console.WriteLine("Create Table {0} OK!", entity.GetTableName());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void TestDynamicEntity()
        {
            var user = EntityBuilder.CreateEntity<ITable_User>();
            //如果接口的名称不是"ITableName" 这样的格式，那么需要调用 MapNewTableName方法指定
            //((EntityBase)user).MapNewTableName("Table_User");

            var qUser = OQL.From((EntityBase)user).Select(user.UID, user.Name, user.Sex).END;
            var users = EntityQuery.QueryList<ITable_User>(qUser, MyDB.Instance);
        }

        private static void OqlInTest()
        {
            var roles = new LT_UserRoles { NickName = "Role1" };
            var users = new LT_Users();
            var qRole = OQL.From(roles).Select(roles.ID).Where(
                cmp => cmp.Comparer(roles.NickName, "like", "123%")
            ).END;

            var qUser = new OQL(users);
            qUser.Select().Where(qUser.Condition
                .AND(users.LastLoginTime, ">=", DateTime.Now.AddDays(-10))
                .NotIn(users.RoleID, qRole));
            Console.WriteLine("OQL to SQL:\r\n{0},\r\n{1}", qUser, qUser.PrintParameterInfo());
        }

        private static void OqlJoinTest()
        {
            var roles = new LT_UserRoles { NickName = "Role1" };
            var roles2 = new LT_UserRoles();
            var q2 = OQL.From(roles)
                .Join(roles2).On(roles.ID, roles2.ID)
                .Select(roles.ID, roles2.RoleName)
                .Where(cmp => cmp.EqualValue(roles.NickName))
                .END;
            Console.WriteLine("OQL 自连接：{0}", q2);

            var users = new LT_Users();
            var q = OQL.From(users)
                .Join(roles).On(users.RoleID, roles.ID)
                .Select(
                    users.ID,
                    users.UserName,
                    roles.ID,
                    roles.RoleName
                )
                .Where(
                    cmp => cmp.EqualValue(roles.NickName)
                )
                .END;
            Console.WriteLine("OQL to SQL:\r\n{0}", q);

            var ec = new EntityContainer(q);
            var list = ec.Map<UserRoleDto>(u =>
            {
                u.UserID = ec.GetItemValue<int>(0);
                u.UserName = ec.GetItemValue<string>(1);
                u.RolesID = ec.GetItemValue<int>(2);
                u.RoleName = ec.GetItemValue<string>(3);
                return u;
            });
        }

        private static void SaveLogingLog(int tvuid)
        {
            var optLog = new AuctionOperationLog();
            optLog.OperaterID = tvuid;
            optLog.Module = "Login";
            optLog.Operation = "登录成功1";
            optLog.LogSource = "PC";
            EntityQuery<AuctionOperationLog>.Instance.Insert(optLog);
        }

        private static void InsertTest()
        {
            MyDB.Instance.ExecuteNonQuery("delete from LT_Users");
            var userList = new List<LT_Users>();
            for (var i = 0; i < 10000; i++)
                userList.Add(new LT_Users
                    {
                        UserName = "Name" + i,
                        Password = "1111",
                        RoleID = 1,
                        AddTime = DateTime.Now
                    }
                );

            var st = new Stopwatch();
            st.Start();
            Console.WriteLine("PDF.NET 插入数据开始{0}条...", userList.Count());
            var count = EntityQuery<LT_Users>.Instance.QuickInsert(userList);
            st.Stop();
            Console.WriteLine("成功插入数据{0}条，耗时{1}ms", count, st.ElapsedMilliseconds);
        }

        private static void UpdateTest()
        {
            var userCmp = new LT_Users { Authority = "admin", IsEnable = true, Remarks = "add" };
            // LT_Users userQ = new LT_Users();
            //OQLCompare cmp = new OQLCompare(userCmp);
            //OQL q = new OQL(userQ);
            var q = new OQL(userCmp);
            var cmp = new OQLCompare(q);

            cmp = cmp.Comparer(userCmp.ID, "in", new[] { 1, 2, 3 })
                  & cmp.Comparer(userCmp.LastLoginIP, "=", "127.0.0.1");
            //------分界线-------

            q.Update(userCmp.Authority, userCmp.IsEnable, userCmp.Remarks).Where(cmp);

            Console.WriteLine("update test:{0}\r\n{1}", q, q.PrintParameterInfo());
        }

        //95行源码，一行代码调用实现带字段选取＋条件判断＋排序＋分页功能的增强ＯＲＭ框架
        private static void TestGOQL()
        {
            var sqlInfo = "";
            //下面使用　ITable_User　或者　Table_User均可
            var userList =
                OQL.FromObject<ITable_User>()
                    //.Select()
                    .Select(s => new object[] { s.UID, s.Name, s.Sex }) //仅选取３个字段
                    .Where((cmp, user) => cmp.Property(user.UID) < 100)
                    .OrderBy((o, user) => o.Asc(user.UID))
                    .Limit(5, 1) //限制５条记录每页，取第一页
                    .Print(out sqlInfo)
                    .ToList();

            Console.WriteLine(sqlInfo);
            Console.WriteLine("User List item count:{0}", userList.Count);
            if (userList.Count > 0)
                Console.WriteLine("User Entity Type:{0}", userList[0].GetType());
        }

        private static void Test1(SelectFieldFun sfun)
        {
        }

        private static void TestInChild()
        {
            var detailObj = new p_hege_detail();

            var phegeObj = new p_hege();
            var phegeOq = new OQL(phegeObj);
            phegeOq.Select(phegeObj.id).OrderBy(phegeObj.id, "DESC");
            phegeOq.TopCount = 1;

            var detailOq = new OQL(detailObj);

            detailOq.Select(detailObj.coName, detailObj.coType, detailObj.coMessage)
                .Where(cmp =>
                    cmp.Comparer(detailObj.coType, "=", "Status") & cmp.Comparer(detailObj.hegeID, "in", phegeOq)) //
                .OrderBy(detailObj.id, "DESC");

            Console.WriteLine("SQL:\r\n{0}\r\n,{1}", detailOq, detailOq.PrintParameterInfo());
        }

        private static void TestPocoQuery()
        {
            var sql = "SELECT top 100000 UID,Sex,Height,Birthday,Name FROM Table_User";
            var db = MyDB.Instance;
            var sw = new Stopwatch();
            Console.Write("query by DataSet,begin...");
            sw.Start();
            var ds = db.ExecuteDataSet(sql);
            sw.Stop();
            Console.WriteLine("end,used time(ms){0}", sw.ElapsedMilliseconds);


            //PocoQuery query = new PocoQuery();
            ////预热
            //List<UserPoco> list1 = query.QueryList<UserPoco>(db.ExecuteDataReader("SELECT top 1 * FROM Table_User"));

            sw.Reset();

            Console.Write("query by AdoHelper Poco Query ,begin...");
            sw.Start();
            //for (int i = 0; i < 100; i++)
            //{
            var list = db.QueryList<UserPoco>(sql);
            //}
            sw.Stop();
            Console.WriteLine("end,used time(ms){0}", sw.ElapsedMilliseconds);

            sw.Reset();
            Console.Write("query by PDF.NET EntityQuery,begin...");
            sw.Start();
            var list3 = EntityQuery<Table_User>.QueryList(db.ExecuteDataReader(sql));
            sw.Stop();
            Console.WriteLine("end,used time(ms){0}", sw.ElapsedMilliseconds);

            sw.Reset();
            Console.Write("query by PDF.NET AdoHelper (handle),begin...");
            sw.Start();
            //UID,Sex,Height,Birthday,Name
            var list4 = db.GetList(reader =>
            {
                return new UserPoco
                {
                    UID = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    Sex = reader.IsDBNull(1) ? false : reader.GetBoolean(1),
                    Height = reader.IsDBNull(2) ? 0 : reader.GetFloat(2),
                    Birthday = reader.IsDBNull(3) ? default : reader.GetDateTime(3),
                    Name = reader.IsDBNull(4) ? null : reader.GetString(4)
                };
            }, sql, null);
            sw.Stop();
            Console.WriteLine("end,used time(ms){0}", sw.ElapsedMilliseconds);


            Thread.Sleep(1000);
        }

        /// <summary>
        ///     测试实体类的外键查询
        /// </summary>
        private static void TestEntityFK()
        {
            var roles = new LT_UserRoles { RoleName = "admin" };
            var q = OQL.From(roles)
                .Select()
                .Where(roles.RoleName)
                .END;
            var list = EntityQuery<LT_UserRoles>.QueryListWithChild(q, MyDB.Instance);
        }

        private static void TestDataSetAndOQL(object para)
        {
            var sql = "select top 10 * from Table_User";
            var user = new Table_User();
            user.MapNewTableName("Table_User");
            //user.MapNewTableName("Table_User");
            var qt = new OQL(user);
            qt.Select().Where(qt.Condition.IN(user.Name, new object[] { "a", "b", "c" })).OrderBy(user.Name);
            //qt.Select().Where(cmp => cmp.Comparer(user.Name, "in", new string[] { "a", "b", "c" }));

            //OQL qt = OQL.From(user).Select().END;
            //qt.TopCount = 10;
            qt.Limit(10, 10);
            qt.PageWithAllRecordCount = 0;

            var db = MyDB.GetDBHelper();
            //测试下面2种方式对连接数量的影响，经测试，发现 MyDB.Instance 跟 MyDB.GetDBHelper() 没有区别。
            //List<Table_User> list = EntityQuery<Table_User>.QueryList(qt);
            //DataSet ds = MyDB.Instance.ExecuteDataSet(sql);

            //测试连接会话，下面db的连接会在using 结束后关闭
            using (db.OpenSession())
            {
                var list = EntityQuery<Table_User>.QueryList(qt, db);
                var ds = db.ExecuteDataSet(sql);
            }
        }

        private static void OQLAvgTest()
        {
            var user = new Table_User();
            var q = OQL.From(user)
                .Select().Avg(user.Height, "AvgHeight")
                .GroupBy(user.Sex)
                .END;
            var ec = new EntityContainer(q);
            var result = ec.MapToList(() => new
            {
                //获取聚合函数的值，用下面一行代码的方式
                AvgHeight = ec.GetItemValue<double>("AvgHeight"),
                Sex = user.Sex ? "男" : "女"
            });
            Console.WriteLine("get AVG record count:" + result.Count);
        }
    }

    public class UserRoleDto
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int RolesID { get; set; }
        public string RoleName { get; set; }
    }

    public class UserPoco : ITable_User
    {
        #region ITable_User 成员

        public DateTime Birthday { get; set; }

        public float Height { get; set; }

        public string Name { get; set; }

        public bool Sex { get; set; }

        public int UID { get; set; }

        #endregion
    }

    public class Entity2<T> : EntityBase where T : class
    {
        protected T dynObj;

        public Entity2()
        {
            dynObj = EntityBuilder.CreateEntity<T>();
            var e = dynObj as EntityBase;
            TableName = e.GetTableName();
            PropertyNames = e.PropertyNames;
            PropertyValues = e.PropertyValues;
        }

        public T AsEntity()
        {
            return dynObj;
        }
    }

    /*
      //使用示例：
      EntityUser etu = new EntityUser();
      ITable_User itu = etu.AsEntity();
      DateTime dtt = itu.Birthday;

     */
    public class EntityUser : Entity2<ITable_User>
    {
    }
}