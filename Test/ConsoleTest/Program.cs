using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TranstarAuction.Repository.Entitys;
using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Data;
using PWMIS.DataProvider.Adapter;
using PWMIS.Core.Extensions;
using MvcApplication1.Models;
using System.Data;
using LocalDB;
using PDFNETClassLib.Model;
using System.Reflection;

namespace ConsoleTest
{

    class Program
    {
        public delegate void SelectFieldFun(params object[] field);

        static void Main(string[] args)
        {
            Console.WriteLine("====PDF.NET SOD 控制台测试程序====");
            Assembly coreAss = Assembly.GetAssembly(typeof(AdoHelper));//获得引用程序集
            Console.WriteLine("PWMIS.Core Version:{0}", coreAss.GetName().Version.ToString());
            Console.WriteLine();
            Console.WriteLine("应用程序配置文件默认的数据库配置信息：\r\n当前使用的数据库类型是：{0}\r\n连接字符串为:{1}\r\n请确保数据库服务器和数据库是否有效且已经初始化过建表脚本（项目下的2个sql脚本文件），\r\n继续请回车，退出请输入字母 Q ."
                , MyDB.Instance.CurrentDBMSType.ToString(), MyDB.Instance.ConnectionString);
            string read = Console.ReadLine();
            if (read.ToUpper() == "Q")
                return;

            //测试 AdoHelper的并发能力
            //for (int i = 0; i < 100; i++)
            //{
            //    System.Threading.Thread t = new System.Threading.Thread(
            //        new System.Threading.ParameterizedThreadStart(TestDataSetAndOQL));
            //    t.Name = "thread "+i;
            //    t.Start();
              
            //}
            TestDataSetAndOQL(null);

            //
            RoadTeam.Model.CS.TbCsEvent CsEvent = new RoadTeam.Model.CS.TbCsEvent();
            CsEvent.EventID = 1;
            OQL oql = OQL.From(CsEvent)
                .Select(CsEvent.EventCheck, CsEvent.EventCheckInfo, CsEvent.EventCheckor, CsEvent.EventCheckTime)
                .Where(CsEvent.EventID)
                .END;
            Console.WriteLine(oql.ToString());
            Console.WriteLine("-----------------------");

            RoadTeam.Model.CS.TbCsEvent CsEvent2 = new RoadTeam.Model.CS.TbCsEvent();
            CsEvent.EventID = 1;
            OQL oql2 = OQL.From(CsEvent2)
                .Select(true,CsEvent2.EventCheck, CsEvent2.EventCheckInfo, CsEvent2.EventCheckor, CsEvent2.EventCheckTime)
                .Where(CsEvent2.EventID)
                .END;
            Console.WriteLine(oql2.ToString());
            Console.WriteLine("-----------------------");
                
            //
            Console.WriteLine("测试实体类动态增加虚拟属性");
            UserModels um1 = new UserModels();
            um1.AddPropertyName("TestName");
            um1["TestName"] = 123;
            int testi =(int) um1["TestName"];

            um1["TestName"] = "abc";
            string teststr = (string)um1["TestName"];

            //
            Console.WriteLine("测试缓存");
            var cache = PWMIS.Core.MemoryCache<EntityBase>.Default;
            cache.Add("UserModels", um1);
            var cacheData = cache.Get("UserModels");
            cache.Remove("UserModels");

            //测试创建表脚本：
            EntityCommand ecmd = new EntityCommand(new LT_Users(), MyDB.Instance);
            Console.WriteLine(ecmd.CreateTableCommand);
            //如果表LT_UserRoles　不存在，则执行下面以行
            try
            {
                MyDB.Instance.ExecuteNonQuery(ecmd.CreateTableCommand);
                Console.WriteLine("Create Table LT_UserRoles OK!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("------------测试暂时停止，请勿继续，下面还没有验证正确运行------");
            Console.ReadLine();
            return;

            Console.WriteLine("测试实体类的外键查询");
            TestEntityFK();
            OqlInTest();

            Console.WriteLine("SELECT top 100000 UID,Sex,Height,Birthday,Name FROM Table_User");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("-------------Testt No.{0}----------------",i+1);
                TestPocoQuery();
            }
            Console.WriteLine("-----------------------");

            //TestInChild();
            TestFun(1, 2, 3);

            TestGOQL();
            UpdateTest();
            InsertTest();
           
           

            
            OqlJoinTest();
            //自动创建实体类测试
            TestDynamicEntity();

            //Sql 格式化查询测试
            AdoHelper dbLocal = new SqlServer();
            dbLocal.ConnectionString = "Data Source=.;Initial Catalog=LocalDB;Integrated Security=True";
            //DataSet ds = dbLocal.ExecuteDataSet("SELECT * FROM Table_User WHERE UID={0} AND Height>={1:5.2}", 1, 1.80M);

            var dataList = dbLocal.GetList(reader =>
            {
                return new
                {
                    UID=reader.GetInt32(0),
                    Name=reader.GetString(1)
                };
            }, "SELECT UID,Name FROM Table_User WHERE Sex={0} And Height>={0:5.2}",1, 1.60);

            //测试属性拷贝
            V_UserModels vum = new V_UserModels();
            vum.BIGTEAM_ID = 123;//可空属性，如果目标对象不是的话，无法拷贝
            vum.REGION_ID = 456;
            vum.SMALLTEAM_ID = 789;

            UserModels um = vum.CopyTo<UserModels>();

           
            //测试自定义查询的实体类
            UserPropertyView up = new UserPropertyView();
            OQL q11 = new OQL(up);
            OQLOrder order = new OQLOrder(q11);
            q11.Select()
                .Where(q11.Condition.AND(up.PropertyName, "=", "总成绩").AND(up.PropertyValue,">",1000))
                .OrderBy(order.Asc(up.UID));
            AdoHelper db11 = MyDB.GetDBHelperByConnectionName("local");
            var result = EntityQuery<UserPropertyView>.QueryList(q11,db11);
            //下面2行不是必须
            q11.Dispose();
            

            //EntityContainer ec = new EntityContainer(q11);
            //var ecResult = ec.MapToList(() => {
            //    return new { AAA = ec.GetItemValue<int>(0), BBB = ec.GetItemValue<string>(1) };
            //});
            
            /////////////////////////////////////////////////////

            AuctionOperationLog log = new AuctionOperationLog();
            log.OptID = 1;
            log.Module = "TestTest";
            log.Operation = "ppppppppp";
            EntityQuery<AuctionOperationLog>.Instance.FillEntity(log);
            EntityQuery eq = new EntityQuery(log);
           
            int ac = eq.Save(log.Module);

            EntityQuery<AuctionOperationLog> logQuery = new EntityQuery<AuctionOperationLog>(log, true);
            log.OperaterID = 999;
            log.Module = "Test";
            log.Operation = "Test Opt";
            log.LogSource = "Test";

            int affectCount = logQuery.Save();
            Console.WriteLine("自动保存成功(insert),id={0}",log.OptID);


            log.Operation = "Test Opt No.2";
            affectCount = logQuery.Save();
            Console.WriteLine("自动保存成功(update)");

            /////////////////测试事务////////////////////////////////////
            AdoHelper db = MyDB.GetDBHelper();
            EntityQuery<AuctionOperationLog> query = new EntityQuery<AuctionOperationLog>(db);

            AuctionOperationLog optLog = new AuctionOperationLog();
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
                OQL q = new OQL(optLog);
                //q.Select().Where(q.Condition.AND(optLog.Operation, "like", "%登录%"));

                q.Select().Count(optLog.OperaterID, "");//使用空字符串参数，这样统计的值将放到 OperaterID 属性中
                //必须指定db参数，否则不再一个事务中，无法进行统计查询
                optLog= EntityQuery<AuctionOperationLog>.QueryObject(q,db);
                int allCount = optLog.OperaterID;

                //optLog 已经使用过，在生成OQL的查询前，必须使用新的实体对象，
                //       否则下面的查询仅会使用OperaterID 字段从而导致分页查询出错
                optLog = new AuctionOperationLog();
                q = new OQL(optLog);

                q.Select().OrderBy(optLog.Module, "asc").OrderBy(optLog.AtDateTime, "desc");
                q.Limit(10, 2);
                q.PageEnable = true;
                q.PageWithAllRecordCount = allCount;
                
                //查询列表并更新到数据库
                List<AuctionOperationLog> list = EntityQuery<AuctionOperationLog>.QueryList(q,db);
                foreach (AuctionOperationLog logItem in list)
                {
                    logItem.AtDateTime = DateTime.Now;
                }
                query.Update(list);

                db.Commit();

                Console.WriteLine("事务操作成功。");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error:"+ex.Message );
                db.Rollback();
            }

            Console.Read();
        }

        static void TestDynamicEntity()
        {
            ITable_User user = EntityBuilder.CreateEntity<ITable_User>();
            //如果接口的名称不是"ITableName" 这样的格式，那么需要调用 MapNewTableName方法指定
            //((EntityBase)user).MapNewTableName("Table_User");

            OQL qUser = OQL.From((EntityBase)user).Select(user.UID, user.Name, user.Sex).END;
            List<ITable_User> users = EntityQuery.QueryList<ITable_User>(qUser, MyDB.Instance);
        }

        static void OqlInTest()
        {
            LT_UserRoles roles = new LT_UserRoles() { NickName="Role1"};
            LT_Users users = new LT_Users();
            OQL qRole = OQL.From(roles).Select(roles.ID).Where(
                cmp=> cmp.Comparer(roles.NickName,"like","123%")
                ).END;

            OQL qUser = new OQL(users);
            qUser.Select().Where(qUser.Condition
                .AND(users.LastLoginTime,">=",DateTime.Now.AddDays(-10))
                .NotIn(users.RoleID, qRole));
            Console.WriteLine("OQL to SQL:\r\n{0},\r\n{1}",qUser,qUser.PrintParameterInfo());
        
        }

        static void OqlJoinTest()
        {
            LT_UserRoles roles = new LT_UserRoles() { NickName = "Role1" };
            LT_UserRoles roles2 = new LT_UserRoles();
            OQL q2 = OQL.From(roles).Join(roles2).On(roles.ID, roles2.ID)
                .Select(roles.ID, roles2.RoleName)
                .Where(cmp => cmp.EqualValue(roles.NickName))
                .END;
            Console.WriteLine("OQL 自连接：{0}", q2);

            LT_Users users = new LT_Users();
            OQL q = OQL.From(users)
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

            EntityContainer ec = new EntityContainer(q);
            var list = ec.MapToList(() => {
                return new { 
                    UserID = ec.GetItemValue<int>(0),
                    UserName = ec.GetItemValue<string>(1),
                    RolesID=ec.GetItemValue<int>(2),
                    RoleName = ec.GetItemValue<string>(3)
                }; 
            });

        }

        static void SaveLogingLog(int tvuid)
        {
            AuctionOperationLog optLog = new AuctionOperationLog();
            optLog.OperaterID = tvuid;
            optLog.Module = "Login";
            optLog.Operation = "登录成功1";
            optLog.LogSource = "PC";
            EntityQuery<AuctionOperationLog>.Instance.Insert(optLog);
        }

        static void InsertTest()
        {
            MyDB.Instance.ExecuteNonQuery("delete from LT_Users");
            List<LT_Users> userList = new List<LT_Users>();
            for (int i = 0; i < 20000; i++)
            {
                userList.Add(new LT_Users() 
                {
                    UserName = "Name"+i, 
                    Password = "1111",
                    RoleID = 1, 
                    AddTime = DateTime.Now 
                }
                );
            }

            //System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
            //st.Start();
            //Console.WriteLine("PDF.NET 插入数据开始...");
            //int count=  EntityQuery<LT_Users>.Instance.QuickInsert(userList);
            //st.Stop();
            //Console.WriteLine("成功插入数据{0}条，耗时{1}ms",count,st.ElapsedMilliseconds);
            //Console.Read();
        }

        static void UpdateTest()
        {
            LT_Users userCmp = new LT_Users() {  Authority="admin", IsEnable=true, Remarks="add"};
           // LT_Users userQ = new LT_Users();
            //OQLCompare cmp = new OQLCompare(userCmp);
            //OQL q = new OQL(userQ);
            OQL q = new OQL(userCmp);
            OQLCompare cmp = new OQLCompare(q);

            cmp = cmp.Comparer(userCmp.ID, "in", new int[]{1,2,3})
                & cmp.Comparer(userCmp.LastLoginIP, "=", "127.0.0.1");
            //------分界线-------

            q.Update(userCmp.Authority,userCmp.IsEnable,userCmp.Remarks).Where(cmp);

            Console.WriteLine("update test:{0}\r\n{1}",q,q.PrintParameterInfo());
        }

        //95行源码，一行代码调用实现带字段选取＋条件判断＋排序＋分页功能的增强ＯＲＭ框架
        static void TestGOQL()
        {
            string sqlInfo="";
            //下面使用　ITable_User　或者　Table_User均可
            List<ITable_User> userList =
                OQL.FromObject<ITable_User>()
                    //.Select()
                    .Select(s => new object[] { s.UID, s.Name, s.Sex }) //仅选取３个字段
                    .Where((cmp, user) => cmp.Property(user.UID) < 100)
                    .OrderBy((o,user)=>o.Asc(user.UID))
                .Limit(5, 1) //限制５条记录每页，取第一页
                .Print(out sqlInfo)
                .ToList();

            Console.WriteLine(sqlInfo);
            Console.WriteLine("User List item count:{0}",userList.Count);
            if (userList.Count > 0)
                Console.WriteLine("User Entity Type:{0}",userList[0].GetType());
        }

        static void Test1(SelectFieldFun sfun)
        { }

        static SelectFieldFun TestFun = (object[] param) => { };

        static void TestInChild()
        {
            p_hege_detail detailObj = new p_hege_detail();

            p_hege phegeObj = new p_hege();
            OQL phegeOq = new OQL(phegeObj);
            phegeOq.Select(phegeObj.id).OrderBy(phegeObj.id, "DESC");
            phegeOq.TopCount = 1;

            OQL detailOq = new OQL(detailObj);

            detailOq.Select(detailObj.coName, detailObj.coType, detailObj.coMessage)
                    .Where(cmp => cmp.Comparer(detailObj.coType, "=", "Status") & cmp.Comparer(detailObj.hegeID, "in", phegeOq)) //
                    .OrderBy(detailObj.id, "DESC");

            Console.WriteLine("SQL:\r\n{0}\r\n,{1}", detailOq.ToString(), detailOq.PrintParameterInfo());

        }

        static void TestPocoQuery()
        {
           
            string sql = "SELECT top 100000 UID,Sex,Height,Birthday,Name FROM Table_User";
            AdoHelper db = MyDB.Instance;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            Console.Write("query by DataSet,begin...");
            sw.Start();
            DataSet ds = db.ExecuteDataSet(sql);
            sw.Stop();
            Console.WriteLine("end,used time(ms){0}",sw.ElapsedMilliseconds);

           
            //PocoQuery query = new PocoQuery();
            ////预热
            //List<UserPoco> list1 = query.QueryList<UserPoco>(db.ExecuteDataReader("SELECT top 1 * FROM Table_User"));

            sw.Reset();
            Console.Write("query by AdoHelper Poco Query,begin...");
            sw.Start();
            List<UserPoco> list = AdoHelper.QueryList<UserPoco>(db.ExecuteDataReader(sql));
            sw.Stop();
            Console.WriteLine("end,used time(ms){0}", sw.ElapsedMilliseconds);

            sw.Reset();
            Console.Write("query by PDF.NET EntityQuery,begin...");
            sw.Start();
            List<Table_User> list3 = EntityQuery<Table_User>.QueryList(db.ExecuteDataReader(sql));
            sw.Stop();
            Console.WriteLine("end,used time(ms){0}", sw.ElapsedMilliseconds);

            sw.Reset();
            Console.Write("query by PDF.NET AdoHelper (handle),begin...");
            sw.Start();
            //UID,Sex,Height,Birthday,Name
            IList<UserPoco> list4=db.GetList<UserPoco>(reader => {
                return new UserPoco() { 
                    UID=reader.IsDBNull(0)?0: reader.GetInt32(0),
                    Sex = reader.IsDBNull(0) ? false : reader.GetBoolean(1),
                    Height = reader.IsDBNull(0) ? 0 : reader.GetFloat(2),
                    Birthday = reader.IsDBNull(0) ? default(DateTime) : reader.GetDateTime(3),
                    Name = reader.IsDBNull(0) ? null : reader.GetString(4)
                };
            }, sql, null);
            sw.Stop();
            Console.WriteLine("end,used time(ms){0}", sw.ElapsedMilliseconds);


            System.Threading.Thread.Sleep(1000);
        }

        /// <summary>
        /// 测试实体类的外键查询
        /// </summary>
        static void TestEntityFK()
        {
            LT_UserRoles roles = new LT_UserRoles() {  RoleName="admin"};
            OQL q= OQL.From(roles)
                    .Select()
                    .Where(roles.RoleName)
                .END;
            var list = EntityQuery<LT_UserRoles>.QueryListWithChild(q, MyDB.Instance);
        }

        static void TestDataSetAndOQL(object para)
        {
            string sql = "select top 10 * from Table_User";
            Table_User user = new Table_User();
            user.MapNewTableName("Table_User");
            //user.MapNewTableName("Table_User");
            OQL qt = new OQL(user);
            qt.Select().Where(qt.Condition.IN(user.Name, new object[] { "a", "b", "c" })).OrderBy(user.Name);
            //qt.Select().Where(cmp => cmp.Comparer(user.Name, "in", new string[] { "a", "b", "c" }));
            
            //OQL qt = OQL.From(user).Select().END;
            //qt.TopCount = 10;
            qt.Limit(10, 10);
            qt.PageWithAllRecordCount = 0;

            AdoHelper db = MyDB.GetDBHelper();
            //测试下面2种方式对连接数量的影响，经测试，发现 MyDB.Instance 跟 MyDB.GetDBHelper() 没有区别。
            //List<Table_User> list = EntityQuery<Table_User>.QueryList(qt);
            //DataSet ds = MyDB.Instance.ExecuteDataSet(sql);

            //测试连接会话，下面db的连接会在using 结束后关闭
            using (db.OpenSession())
            {
                List<Table_User> list = EntityQuery<Table_User>.QueryList(qt, db);
                DataSet ds = db.ExecuteDataSet(sql);
            }
            
        }
    }

    public class UserPoco : ITable_User
    {

        #region ITable_User 成员

        public DateTime Birthday
        {
            get;
            set;
        }

        public float Height
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public bool Sex
        {
            get;
            set;
        }

        public int UID
        {
            get;
            set;
        }

        #endregion
    }
}
