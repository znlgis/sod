using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PWMIS.DataMap.Entity;
using UPMS.Core.Model;
using System.Threading;
using PWMIS.Core;
using PWMIS.Core.Extensions;
using PWMIS.Common;

namespace OQLTest
{
    class Program
    {
        static void Main(string[] args)
        {
            HotDictionaryTest();
            //TestThread();
            //TestProperty();
            TestCached();
            
            Console.WriteLine("OQL 测试，按任意键开始");
            Console.Read();

            Program p = new Program();
            p.TestMapOql();
            p.Test1();
            p.Test2();
            p.Test3();
            p.Test4();
            p.Test5();
            p.Test6();
            p.TestLimit();
            p.TestIfCondition();
            p.TestIfCondition2();
            p.TestChild();
            p.TestChild2();
            p.TestOQLOrder();
            p.TestUpdate();
            p.TestDelete();
            p.TestInsert();
            p.TestInsertFrom();
            p.TestSqlLock();
            p.Test2FieldOpt();
            p.TestNotCondition();
            p.GetRoleFunctionList("123");

            Console.WriteLine("---测试全部完成(输入 OQL 退出)----");
            while( Console.ReadLine().ToUpper()!="OQL");
        }

        void Test1()
        {
            Users user = new Users() { NickName = "pdf.net", RoleID = RoleNames.Admin };
            UserRoles roles = new UserRoles() { RoleName = "role1" };
            //测试字段直接比较
            OQL q00 = OQL.From(user)
                .Select()
                .Where(cmp => cmp.Comparer(user.AddTime, "=", user.LastLoginTime))
            .END;
            Console.WriteLine("q00:one table and select all fields \r\n{0}", q00);
            Console.WriteLine(q00.PrintParameterInfo());

            OQL q0 = OQL.From(user)
               .Select()
               .Where(user.NickName, user.RoleID)
               .OrderBy(user.ID)
               .END;
            q0.SelectStar = true;
            Console.WriteLine("q0:one table and select all fields \r\n{0}", q0);
            Console.WriteLine(q0.PrintParameterInfo());

            OQL q01 = OQL.From(user)
               .Select()
               .Where(cmp=> cmp.Comparer(user.ID, OQLCompare.CompareType.Equal,0 ))
               .OrderBy(user.ID)
               .END;
            q01.SelectStar = true;
            Console.WriteLine("q01:one table and select all fields \r\n{0}", q01);
            Console.WriteLine(q01.PrintParameterInfo());


            //var userList = EntityQuery<Users>.QueryList(q0);
            //if (userList.Count > 0)
            //{
            //    Users u = userList[0];
            //    Console.WriteLine("User Type is:" + u.RoleID.ToString());
            //    u.RoleID = RoleNames.User;
            //    EntityQuery<Users>.Instance.Update(u);
            //}
            OQL q = OQL.From(user)
                .Select(user.ID, user.UserName, user.RoleID)
                .END;
            q.Select(user.LastLoginIP).Where(user.NickName);

            Console.WriteLine("q1:one table and select some fields\r\n{0}", q);
            Console.WriteLine(q.PrintParameterInfo());

            //动态指定查询的字段和比较关心、值
            q = OQL.From(user).Select().Where(new QueryParameter[] 
            { 
                new QueryParameter("ID", "=", 1)
            }
                ).END;

            Console.WriteLine("q1:QueryParameter Test\r\n{0}", q);
            Console.WriteLine(q.PrintParameterInfo());


            OQL q2 = OQL.From(user)
                .InnerJoin(roles).On(user.RoleID, roles.ID)
                .Select(user.RoleID, roles.RoleName)
                .Where(user.NickName, roles.RoleName)
                .GroupBy(user.RoleID, roles.RoleName)
                .OrderBy(user.ID)
                .END;

            Console.WriteLine("q2:two table query use join\r\n{0}", q2);
            Console.WriteLine(q2.PrintParameterInfo());

            OQL q3 = OQL.From(user, roles)
                .Select(user.ID, user.UserName, roles.ID, roles.RoleName)
                .Where(cmp => cmp.Comparer(user.RoleID, "=", roles.ID)
                    & cmp.EqualValue(roles.RoleName))
                .OrderBy(user.ID)
                .END;
            Console.WriteLine("q3:two table query not use join\r\n{0}", q3);
            Console.WriteLine(q3.PrintParameterInfo());

            OQL q4 = OQL.From(user).InnerJoin(roles).On(user.RoleID, roles.ID)
                .Select(user.RoleID).Count(user.RoleID, "roldid_count") //
                .Where(user.NickName)
                .GroupBy(user.RoleID)
                .END;
            Console.WriteLine("q4:count from two table query \r\n{0}", q4);
            Console.WriteLine(q4.PrintParameterInfo());

            OQL q5 = OQL.From(user)
                .Select(user.RoleID).Count(user.RoleID, "count_rolid")
                .GroupBy(user.RoleID)
                .Having(p => p.Count(user.RoleID, OQLCompare.CompareType.GreaterThanOrEqual, 2))
                .END;

            Console.WriteLine("q5:having Test: \r\n{0}", q5);
            Console.WriteLine(q5.PrintParameterInfo());

            OQL q6 = OQL.From(user).Select()
                .Where(cmp =>
                     cmp.Comparer(user.RoleID, "is not", null) &
                     cmp.Comparer(user.AddTime, ">=", DateTime.Now.AddDays(-1)) &
                     cmp.Comparer(user.AddTime, "<", DateTime.Now)
                     )
                .END;
            q6.SelectStar = true;
            Console.WriteLine("q6:SQL 'IS' Test: \r\n{0}", q6);
            Console.WriteLine(q6.PrintParameterInfo());

            OQL q7 = OQL.From(user).Select()
               .Where(cmp => cmp.Between(user.ID, 5, 10))
               .END;
            q7.SelectStar = true;
            Console.WriteLine("q7:SQL Between Test: \r\n{0}", q7);
            Console.WriteLine(q7.PrintParameterInfo());

            //Compare 对象使用 ComparerSqlFunction 方法，解决SQL函数操作结果的类型跟字段类型不一致的问题
            //感谢网友 【有事M我】发现此问题 2014.3.11
            GOQL<Users> q8 = OQL.FromObject<Users>()
                .Select()
                .Where((cmp, u) => cmp.ComparerSqlFunction(u.NickName, ">", 0, "CHARINDEX( 'xiao',{0} )"))
                .END;
            string sql;
            q8.Print(out sql);
            Console.WriteLine("q8:SQL Function Test: \r\n{0}", sql);
        }

        void Test2()
        {
            Users user = new Users();
            UserRoles roles = new UserRoles() { RoleName = "role1" };

            OQL q2 = new OQL(user);
            q2.InnerJoin(roles).On(user.RoleID, roles.ID);

            OQLCompare cmp = new OQLCompare(q2);
            OQLCompare cmpResult =
                   (
                     cmp.Property(user.UserName) == "ABC" &
                     cmp.Comparer(user.Password, "=", "111") &
                     cmp.EqualValue(roles.RoleName)
                   )
                      |
                   (
                     (cmp.Comparer(user.UserName, "=", "CDE") &
                       cmp.Property(user.Password) == "222" &
                       cmp.Comparer(roles.RoleName, "like", "%Role2")
                     )
                     |
                     (cmp.Property(user.LastLoginTime) > DateTime.Now.AddDays(-1))
                   )
                   ;

            q2.Select().Where(cmpResult);
            Console.WriteLine("OQL by OQLCompare Test:\r\n{0}", q2);
            Console.WriteLine(q2.PrintParameterInfo());
        }

        void Test3()
        {
            Users user = new Users();
            UserRoles roles = new UserRoles() { RoleName = "role1" };

            OQLCompareFunc cmpResult = cmp =>
                   (
                     cmp.Property(user.UserName) == "ABC" &
                     cmp.Comparer(user.Password, "=", "111") &
                     cmp.EqualValue(roles.RoleName)
                   )
                      |
                   (
                     (cmp.Comparer(user.UserName, OQLCompare.CompareType.Equal, "BCD") &
                       cmp.Property(user.Password) == 222 &
                       cmp.Comparer(roles.ID, "in", new RoleNames[] { RoleNames.User, RoleNames.Manager, RoleNames.Admin })
                     )
                     |
                     (cmp.Property(user.LastLoginTime) > DateTime.Now.AddDays(-1))
                   )
                   ;
            OQL q3 = OQL.From(user).InnerJoin(roles)
               .On(user.RoleID, roles.ID)
               .Select()
               .Where(cmpResult)
               .END;
            Console.WriteLine("OQL by OQLCompareFunc Test:\r\n{0}", q3);
            Console.WriteLine(q3.PrintParameterInfo());
        }

        void Test4()
        {
            OQLCompareFunc<Users, UserRoles> cmpResult = (cmp, U, R) =>
                   (
                     cmp.Property(U.UserName) == "ABC" &
                     cmp.Comparer(U.Password, "=", "111") &
                     cmp.Comparer(R.RoleName, "=", "Role1")
                   )
                      |
                   (
                     (cmp.Comparer(U.UserName, "=", "CDE") &
                       cmp.Property(U.Password) == "222" &
                       cmp.Comparer(R.RoleName, "like", "%Role2")
                     )
                     |
                     (cmp.Property(U.LastLoginTime) > DateTime.Now.AddDays(-1))
                   )
                   ;
            Users user = new Users();
            UserRoles roles = new UserRoles() { RoleName = "role1" };

            OQL q4 = OQL.From(user).InnerJoin(roles)
                .On(user.RoleID, roles.ID)
                .Select()
                .Where(cmpResult)
                .END;
            Console.WriteLine("OQL by OQLCompareFunc<T1,T2>  Test:\r\n{0}", q4);
            Console.WriteLine(q4.PrintParameterInfo());
            q4.Dispose();
        }

        void Test5()
        {
            Users user = new Users();
            OQLCompareFunc cmpResult = cmp =>
                (
                  cmp.Property(user.AddTime) > new DateTime(2013, 2, 1)
                & cmp.Comparer(user.AddTime, "<", new DateTime(2013, 3, 1), "dateadd(hour,24,{0})")
                )
                |
                (
                  cmp.Property(user.Authority) == "ABC"
                | cmp.Property(user.Authority) == "CDE"
                )
                ;
            OQL q5 = OQL.From(user).Select().Where(cmpResult).END;
            Console.WriteLine("OQL by OQLCompareFunc 括号化简 Test:\r\n{0}", q5);
            Console.WriteLine(q5.PrintParameterInfo());
        }

        void Test6()
        {
            OQLCompareFunc<Users> cmpResult = (cmp, u) =>
                cmp.Comparer(u.UserName, OQLCompare.CompareType.IN, new string[] { "zhang aa", "li bb", "wang cc" }); //
                  
            Users user = new Users();
           
            OQL q6 = OQL.From(user)
                .Select()
                .Where(cmpResult)
                .END;
            Console.WriteLine("OQL by OQLCompareFunc<T1>  Test:\r\n{0}", q6);
            Console.WriteLine(q6.PrintParameterInfo());
            q6.Dispose();
        }

        void TestIfCondition()
        {
            Users user = new Users() { ID = 1, UserName = "zhagnsan", Password = "pwd.",NickName = "" };//NickName = "abc",
            OQLCompareFunc cmpFun = cmp =>
            {
                //5.2.2.0428 版本之前,仅支持被注释掉的写法,用一个独立的对象来接收条件比较结果,否则会出错.
                //           之后的版本,按照现在的写法没问题,不过仍然建议使用原来的写法.
                OQLCompare cmpResult = null;
                if (user.NickName != "")
                    //cmpResult = cmp.Property(user.AddTime) > new DateTime(2013, 2, 1);
                    cmp = cmp.Property(user.AddTime) > new DateTime(2013, 2, 1);
                if (user.ID > 0)
                    //cmpResult = cmpResult & cmp.Property(user.UserName) == "ABC" & cmp.Comparer(user.Password, "=", "111");
                    cmp = cmp & cmp.Property(user.UserName) == "ABC" & cmp.Comparer(user.Password, "=", "111");
                //return cmpResult;
                cmpResult = cmp;
                return cmpResult;
            };

            OQL q6 = OQL.From(user).Select().Where(cmpFun).END;
            Console.WriteLine("OQL by 动态构建 OQLCompare Test(Lambda方式):\r\n{0}", q6);
            Console.WriteLine(q6.PrintParameterInfo());
        }

        //动态处理查询条件，详细见 http://www.cnblogs.com/bluedoctor/p/4470526.html
        void TestIfCondition2()
        {
            Users user = new Users() { ID = 0, NickName = "abc", UserName="zhang san", Password="pwd111" };
            OQL q7 = OQL.From(user)
                .Select()
                .Where<Users>(CreateCondition)
                .END;
            Console.WriteLine("OQL by 动态构建 OQLCompare Test(委托函数方式):\r\n{0}", q7);
            Console.WriteLine(q7.PrintParameterInfo());
        }

        OQLCompare CreateCondition(OQLCompare cmp, Users user)
        {
            Users testUser = new Users {  NickName =user.NickName , ID =user.ID};

            OQLCompare cmpResult = null;
            if (testUser.NickName != "")
                cmpResult = cmp.Comparer(user.NickName, "=", user.NickName);
            // 上面一行，也可以采用这样的写法： cmpResult = cmp.EqualValue(user.NickName);
            if (testUser.ID > 0)
                cmpResult = cmpResult & cmp.Comparer(user.ID, "=", user.ID);
            else
                cmpResult = cmpResult & cmp.Comparer(user.UserName,"=", "zhang san") 
                                      & cmp.Comparer(user.Password, "=", "pwd111");
            return cmpResult;
        }


        void TestLimit()
        {
            Users user = new Users() { NickName = "pdf.net" };
            OQL q0 = OQL.From(user)
               .Select()
               .Where(user.NickName)
               .OrderBy(user.ID)
               .END;

            q0.Limit(10, 2);

            Console.WriteLine("one table and select page number 2,page size 10: \r\n{0}", q0);
            Console.WriteLine("因为OQL是抽象的SQL，而分页语法又是特定于数据库的，所以具体的分页SQL要到查询真正执行的时候才会生成。");
            Console.WriteLine(q0.PrintParameterInfo());
        }

        void TestChild()
        {
            Users user = new Users();
            UserRoles roles = new UserRoles();
            OQL child = OQL.From(roles)
                .Select(roles.ID)
                .Where(p => p.Comparer(roles.NickName, "like", "%ABC"))
                .END;

            OQL q = OQL.From(user)
                .Select(user.ID, user.UserName)
                .Where(cmp => cmp.Comparer(user.RoleID, "in", child))
                .END;

            Console.WriteLine("OQL by 子查询Test:\r\n{0}", q);
            Console.WriteLine(q.PrintParameterInfo());
        }

        void TestChild2()
        {
            /*
             SELECT * FROM [LT_Users]  WHERE RoleID =
  (SELECT ID FROM dbo.LT_UserRoles r WHERE  [LT_Users].NickName=r.NickName)
             */
            Users user = new Users() { NickName = "_nickName" };
            UserRoles roles = new UserRoles() { NickName = "_roleNickName" };

            OQLChildFunc childFunc = parent => OQL.From(parent, roles)
                .Select(roles.ID)
                .Where(cmp => cmp.Comparer(user.NickName, "=", roles.NickName) //比较的字段顺序无所谓
                            & cmp.Property(roles.AddTime) > DateTime.Now.AddDays(-3))
                .END;

            OQL q = OQL.From(user)
                .Select()
                .Where(cmp => cmp.Comparer(user.RoleID, "=", childFunc))
                .END;

            q.SelectStar = true;
            Console.WriteLine("OQL by 高级子查询Test:\r\n{0}", q);
            Console.WriteLine(q.PrintParameterInfo());
        }

        void TestUpdate()
        {
            Users user = new Users()
            {
                AddTime = DateTime.Now.AddDays(-1),
                Authority = "Read",
                NickName = "菜鸟"
            };
            OQL q = OQL.From(user)
                .Update(user.AddTime, user.Authority, user.NickName)
                .Where(cmp => cmp.Property(user.RoleID) == 100)
                .END;

            //OQL q = OQL.From(user)
            //    .Update(user.AddTime)
            //    .Where(user.Authority, user.NickName)
            //    .END;
            Console.WriteLine("OQL update:\r\n{0}\r\n", q);

            Console.WriteLine(q.PrintParameterInfo());
        }

        void TestDelete()
        {
            Users user = new Users() {  ID=99};

            OQL q = OQL.From(user)
                .Delete()
                //.Where(cmp => cmp.Property(user.RoleID) == 100)
                .END;

            Console.WriteLine("OQL 安全删除数据测试:\r\n{0}\r\n", q);
            Console.WriteLine(q.PrintParameterInfo());
        }

        void TestOQLOrder()
        {
            Users user = new Users();
            //OQLOrderAction<Users> action = this.OQLOrder;
            OQL q = OQL.From(user)
                .Select(user.UserName, user.ID)
                //.OrderBy(p => p.Desc(user.UserName).Asc(user.ID))
                //.OrderBy(action,user)
                //.OrderBy<Users>(OQLOrder,user) //4种OQLOrder 对象的使用方法
                .OrderBy(new string[] { "UserName desc", "ID asc" })
                //.OrderBy<Users>((o, u) => { o.Desc(u.UserName); })
                .END;


            Console.WriteLine("OQL test OQLOrder object:\r\n{0}\r\n", q);
        }

        void OQLOrder(OQLOrder p, Users user)
        {
            p.Desc(user.UserName).Asc(user.ID);
        }

        void TestInsert()
        {
            Users user = new Users()
            {
                AddTime = DateTime.Now.AddDays(-1),
                Authority = "Read",
                NickName = "菜鸟"
            };

            OQL q = OQL.From(user)
                .Insert(user.AddTime, user.Authority, user.NickName);

            Console.WriteLine("OQL insert:\r\n{0}\r\n", q);
            Console.WriteLine(q.PrintParameterInfo());
        }

        void TestInsertFrom()
        {
            Users user = new Users();
            UserRoles roles = new UserRoles();

            OQL child = OQL.From(roles)
                .Select(roles.ID)
                .Where(cmp => cmp.Comparer(roles.ID, ">", RoleNames.User))
                .END;

            OQL q = OQL.From(user)
                .InsertFrom(child, user.RoleID);

            Console.WriteLine("OQL insert from:\r\n{0}\r\n", q);
            Console.WriteLine(q.PrintParameterInfo());
        }

        void TestSqlLock()
        {
            Users user = new Users();
            UserRoles roles = new UserRoles() { RoleName = "role1" };
            OQL q = OQL.From(user)
                 .InnerJoin(roles).On(user.RoleID, roles.ID)
                //.With(OQL.SqlServerLock.NOLOCK)
                .With("nolock")
                .Select(user.ID, user.UserName, user.NickName)
                .END;
            Console.WriteLine("OQL Test SQL NoLock:\r\n{0}\r\n", q);
        }

        void Test2FieldOpt()
        {
            Users user = new Users();
            //    user.LastLoginTime-user.AddTime>'23:00:00' 
            // => user.LastLoginTime -'23:00:00'>user.AddTime
            OQL q = OQL.From(user)
                .Select()
                .Where(cmp => cmp.Comparer(user.LastLoginTime, ">", user.AddTime, "{0}-'23:00:00'"))
                .END;
            q.SelectStar = true;
            Console.WriteLine("OQL Test SQL Field compute:\r\n{0}\r\n", q);
            Console.WriteLine(q.PrintParameterInfo());
        }

        void TestNotCondition()
        {
            Users user = new Users();
            OQL q = OQL.From(user)
                .Select(user.ID, user.UserName, user.Password)
                .Where<Users>((cmp, u) => OQLCompare.Not(
                    cmp.Property(u.UserName) == "ABC" & cmp.Property(u.Password) == "123")
                    )
                .END;

            Console.WriteLine("OQL Test NOT Condition:\r\n{0}\r\n", q);
            Console.WriteLine(q.PrintParameterInfo());
        }

        public void GetRoleFunctionList(string personId)
        {
            //子查询
            Base_Person_FunctionInfo personFun = new Base_Person_FunctionInfo() { PersonId = personId, DirectionFlag = -1 };
            OQLChildFunc oqlChild = parent => OQL.From(parent, personFun)
                .Select(true, personFun.FunctionId)
                .Where(personFun.PersonId, personFun.DirectionFlag)
                .END;

            //查询所有功能，并从中过滤掉子查询的结果
            Base_FunctionInfo fun = new Base_FunctionInfo();
            Base_Person_RoleInfo personRole = new Base_Person_RoleInfo() { PersonId = personId };
            Base_Role_FunctionInfo roleFun = new Base_Role_FunctionInfo();
            OQL oql = OQL.From(personRole)
                .InnerJoin(roleFun).On(personRole.RoleId, roleFun.RoleId)
                .InnerJoin(fun).On(roleFun.FunctionId, fun.FunctionId)
                .Select(true, fun.FunctionId, fun.FunctionName, fun.NavigateAddress)
                .Where(cmp => cmp.EqualValue(personRole.PersonId) 
                    & cmp.Comparer(fun.FunctionId, OQLCompare.CompareType.NotIn, oqlChild))
                .END;
            //return EntityQuery<Base_FunctionInfo>.QueryList(oql);

            Console.WriteLine("OQL Test Child Query:\r\n{0}\r\n", oql);
            Console.WriteLine(oql.PrintParameterInfo());
        }

        void TestMapOql()
        {
            Users user = new Users() { NickName = "pdf.net", RoleID = RoleNames.Admin };
            //插入一个数据便于测试
            EntityQuery<Users>.Instance.Insert(user);

            OQL q = OQL.From(user).Select().Where(user.RoleID).END;
            EntityContainer ec = new EntityContainer(q);
            //第一种映射方式，适用于OQL单实体类查询：
            var list = ec.MapToList(user, u => new
                    {
                        P1 = u.ID,
                        P2 = u.NickName
                    }
                );

            UserRoles role = new UserRoles();
            OQL q2 = OQL.From(user)
                .Join(role).On(user.RoleID, role.ID)
                .Select()
                .Where(cmp => cmp.Comparer(role.ID, "=", RoleNames.User))
                .OrderBy(role.RoleName)
                .END;
            q2.Limit(10,2);
            q2.PageWithAllRecordCount = 0;

            //分页，需要在 select 子句中包含排序的字段，然后排序使用字段别名
            string pageSql = @"
SELECT  	
 M.[ID],	
 M.[NickName],	
 T0.[ID] AS [T0_ID],	
 T0.[Description] AS [T0_Description]
FROM [LT_Users] M   
INNER JOIN [LT_UserRoles] T0  ON  M.[RoleID] = T0.[ID] 
WHERE  T0.[ID] = 1
ORDER BY T0.[RoleName] ASC 
";
            //如果上面的SQL语句升序排序，每页10行，取第二页，那么应该生成下面的分页语句
            /*
             SELECT Top 10 * FROM(
				SELECT Top 10 * FROM ( 
						SELECT  top 20	
						 M.[ID],	
						 M.[NickName],	
						 T0.[ID] AS [T0_ID],	
						 T0.[Description] AS [T0_Description],
						 T0.[RoleName]  
						FROM [LT_Users] M   
						INNER JOIN [LT_UserRoles] T0  ON  M.[RoleID] = T0.[ID] 
						WHERE  T0.[ID] = 1
						ORDER BY T0.[RoleName] ASC 
									) P_T0  
				ORDER BY [RoleName] DESC 
                   ) P_T1
             ORDER BY [RoleName] ASC
             * 
             * 注意内部的分页语句，应该附加上排序的字段 T0.[RoleName] 才可以供外部的排序条件使用，
             * 所以OQL处理的时候应该考虑这个问题，比如下面的查询：
             SELECT Top 10 * FROM(
				SELECT Top 10 * FROM ( 
						SELECT  top 20	
						 M.[ID],	
						 M.[NickName],	
						 T0.[ID] AS [T0_ID],	
						 T0.[Description] AS [T0_Description],
						 T0.[RoleName] AS [T0_RoleName]
						FROM [LT_Users] M   
						INNER JOIN [LT_UserRoles] T0  ON  M.[RoleID] = T0.[ID] 
						WHERE  T0.[ID] = 1
						ORDER BY T0.[RoleName] ASC 
									) P_T0  
				ORDER BY [T0_RoleName] DESC 
                   ) P_T1
             ORDER BY [T0_RoleName] ASC
             * 
             */

            EntityContainer ec2 = new EntityContainer(q2);
            //第二种映射方式，推荐用于OQL多个实体类关联查询的情况：
            var list2 = ec2.MapToList(() => new { 
                     P1= user.ID,
                     P2= user.NickName,
                     P3= role.ID ,
                     P4 =role.Description 
                });

            Console.WriteLine("OQL TestMapOql Query:\r\n{0}\r\n", q2);
            Console.WriteLine(q2.PrintParameterInfo());
        }

        static void TestThread()
        {
            //下面的代码会导致死循环，参加下面的文章：
            //http://www.cnblogs.com/LoveJenny/archive/2011/05/29/2060718.html
            bool complete = false;
            var t = new Thread(() =>
            {
                bool toggle = false;
                while (!complete) toggle = !toggle;
            });
            t.Start();
            Thread.Sleep(1000);
            complete = true;
            t.Join();
            Console.WriteLine("Thread Test OK.");
        }

        static void TestProperty()
        {
            var props = typeof(TestA).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (var p in props)
            {
               //bool flag= p.GetType().IsSubclassOf(typeof(IEnumerable<>));
                if (p.PropertyType.IsGenericType)
                {
                    Type t = p.PropertyType.GetGenericTypeDefinition();
                    //if (t == typeof(IEnumerable<>))
                    //{
                    //    Console.WriteLine("IEnumerable<>");
                    //}
                    //else if (t == typeof(List<>))
                    //{
                    //    Console.WriteLine("List<>");
                    //}
                    //if (t.GetInterface("IEnumerable`1") != null)
                    //{
                    //    Console.WriteLine("IsSubtypeOf IEnumerable<>");
                    //}


                    if (t == typeof(IEnumerable<>) || t.GetInterface("IEnumerable`1") != null)
                    {
                        Console.WriteLine("IS IEnumerable<>");
                        Type[] tArr = p.PropertyType.GetGenericArguments();
                    }
                }
            }
        }

        static void HotDictionaryTest()
        {
            string hexString = "ffff";//
            Console.WriteLine("Hex string:{0},number:{1}", hexString, int.Parse(hexString, System.Globalization.NumberStyles.HexNumber));
            hexString = "00ff";
            Console.WriteLine("Hex string:{0},number:{1}", hexString, int.Parse(hexString, System.Globalization.NumberStyles.HexNumber));

            hexString = "1fffffff";
            Console.WriteLine("Hex string:{0},number:{1}", hexString, int.Parse(hexString, System.Globalization.NumberStyles.HexNumber));

            Console.WriteLine("int max value:{0}", int.MaxValue);

            MakeWordKey mwk = new MakeWordKey();
            hexString = mwk.MakeHexString("123");
            hexString = mwk.MakeHexString("123456");
            hexString = mwk.MakeHexString("a123");
            hexString = mwk.MakeHexString("a1234");
            hexString = mwk.MakeHexString("A1234");
            hexString = mwk.MakeHexString("Key001");
            hexString = mwk.MakeHexString("Key002");
            hexString = mwk.MakeHexString("MyKey1");
            hexString = mwk.MakeHexString("MyKey2");

            hexString = mwk.MakeHexString("ABIK");
            hexString = mwk.MakeHexString("ABJK");
            hexString = mwk.MakeHexString("ABKL");
            hexString = mwk.MakeHexString("ABKM");
            hexString = mwk.MakeHexString("ABKN");
            hexString = mwk.MakeHexString("ABKO");
            hexString = mwk.MakeHexString("ABKP");
            hexString = mwk.MakeHexString("ABKQ");

            int flag1 = mwk.Hex2Int(hexString);
            int flag2 = mwk.String2Int("ABKQ");
            /*
             * 测试16进制数转换效率：
             * make hex string and convert to int,1000000 repeated used time(ms):252
               quick string convert to int,1000000 repeated used time(ms):73
             */
            //System.Diagnostics.Stopwatch sw1 = new System.Diagnostics.Stopwatch();
            //sw1.Start();
            //for (int i = 0; i < int.MaxValue/20;i++ )
            //{
            //    hexString = mwk.MakeHexString("6789");
            //    int flag3 = mwk.Hex2Int(hexString);
            //}
            //sw1.Stop();
            //Console.WriteLine("make hex string and convert to int,\r\n{0} repeated used time(ms):{1}",int.MaxValue/20,sw1.Elapsed.TotalSeconds);

            //System.Diagnostics.Stopwatch sw2 = new System.Diagnostics.Stopwatch();
            //sw2.Start();
            //for(int i=0;i<int.MaxValue/20;i++)
            //{
            //    int flag4 = mwk.String2Int("6789");
            //}
            //sw2.Stop();
            //Console.WriteLine("quick string convert to int,\r\n{0} repeated used time(ms):{1}", int.MaxValue / 20, sw2.Elapsed.TotalSeconds);
            //Console.Read();

            HotNameValue<int> lur = new HotNameValue<int>();
            lur.Set("A", 10);
            lur.Set("B", 20);
            lur.Set("C", 30);
            lur.Set("D", 40);
            lur.Set("E", 50);
            //测试碰撞A,a,0 都在同一个位置
            lur.Set("0", 20);
            lur.Set("a", 30);
            lur.Set("0a", 100);

            Console.WriteLine("Find A={0},at:{1}", lur.Get("A"), lur.At);
            Console.WriteLine("Find B={0},at:{1}", lur.Get("B"), lur.At);
            Console.WriteLine("Find A={0},at:{1}", lur.Get("A"), lur.At);
            Console.WriteLine("Find C={0},at:{1}", lur.Get("C"), lur.At);
            Console.WriteLine("Find A={0},at:{1}", lur.Get("A"), lur.At);
            Console.WriteLine("Find C={0},at:{1}", lur.Get("C"), lur.At);
            Console.WriteLine("Find 0={0},at:{1}", lur.Get("0"), lur.At);

            //测试逐个依次删除
            lur.Remove("A");
            lur.Remove("0");
            Console.WriteLine("Find a={0},at:{1}", lur.Get("a"), lur.At);
            lur.Remove("a");
            Console.WriteLine("Find 0a={0},at:{1}", lur.Get("0a"), lur.At);
            lur.Remove("0a");
            //复用刚才删除的空间
            lur.Set("b",1000);

            //测试删除链表的中间元素
            //lur.Remove("a");
            //Console.WriteLine("Find 0a={0},at:{1}", lur.Get("0a"), lur.At);
            //lur.Remove("0a");
            //Console.WriteLine("Find 0={0},at:{1}", lur.Get("0"), lur.At);
            //lur.Remove("0");
            //Console.WriteLine("Find A={0},at:{1}", lur.Get("A"), lur.At);
        }

        static void TestCached()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
           
            TestA a = new TestA();
            sw.Start();
            for (int i = 0; i < 1000000; i++)
            {
                a.ToDo();
            }
            sw.Stop();
            Console.WriteLine("Reflection NoCache ElapsedMilliseconds {0}", sw.ElapsedMilliseconds);

            sw.Reset();
            sw.Start();

            for (int i = 0; i < 1000000; i++)
            {
                a.ToDo2();
            }
            sw.Stop();
            Console.WriteLine("Reflection Cached ElapsedMilliseconds {0}", sw.ElapsedMilliseconds);

        }

        private static void TestSqlOrderBuilder()
        {
            string sql = @"select Age,t.[User Name] As UserName from tab t 
                   where ID>1000 order   by    t.[User Name]  , id desc 
                   option(hash group,fast 10) ";
            SqlOrderBuilder sob = new SqlOrderBuilder(sql);
            string sqlOrder1 = sob.Build(50);
            string sqlOrder2 = sob.Build(50, "Age>=20");
        }

    }

    
    class TestA
    {
        public string P1 { get; set; }
        public List<int> P2 { get; set; }
        public IEnumerable<string> P3 { get; set; }

        System.Reflection.MethodInfo miStringArg = null;
       
        private void DoSomething<T>(int i) where T : class
        {
            //Console.WriteLine("i={0}", i);
            int j = i++;
        }

        public void ToDo()
        {
            var method = GetType().GetMethod("DoSomething",System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var realMethod = method.MakeGenericMethod(typeof(string));
            realMethod.Invoke(this, new object[] { 123 });
        }

        public void ToDo2()
        {
            if (miStringArg == null)
            {
                var method = GetType().GetMethod("DoSomething", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                var realMethod = method.MakeGenericMethod(typeof(string));
                miStringArg = realMethod;
            }
            miStringArg.Invoke(this, new object[] { 123 });
        }

        

        /// <summary>
        /// 在 source句子字符串中搜索一个短语，并忽略source和短语中多余1个的空白字符，忽略大小写
        /// </summary>
        /// <param name="source"></param>
        /// <param name="words"></param>
        /// <returns>返回短语在句子中最后一个开始的位置和结束位置的结构</returns>
        public Point SearchWordsIndex(string source, string words)
        {
            string[] matchWrodArray = words.Split(new char[]{' ','\r','\n','\t'}, StringSplitOptions.RemoveEmptyEntries);
            int firstMatchIndex = source.LastIndexOf(matchWrodArray[0], source.Length -1, StringComparison.OrdinalIgnoreCase);
            if (firstMatchIndex == -1) return new Point(-1,0);
            int matchWordIndex = 1;
            int startIndex = firstMatchIndex + matchWrodArray[0].Length;
            bool needSpace = true;
            List<char> charList = new List<char>(); 
            for (int i = startIndex; i < source.Length; i++)
            {
                //匹配一个单词后，需要至少一个空白字符
                if (needSpace)
                {
                    if (char.IsWhiteSpace(source[i]))
                    {
                        needSpace = false;
                        continue;
                    }
                }
                if ( !char.IsWhiteSpace(source[i]))
                { 
                    //遇到非空白字符，处理字符串叠加
                    charList.Clear();
                    while (i < source.Length)
                    {
                        char c = source[i];
                        if (!char.IsWhiteSpace(c))
                        {
                            charList.Add(c);
                            i++;                        
                        }
                        else
                        {
                            break;
                        }
                    }
                    //遇到空白字符，循环结束
                    string findWord = new string(charList.ToArray());
                    string matchWord = matchWrodArray[matchWordIndex];
                    if (string.Equals(findWord, matchWord, StringComparison.OrdinalIgnoreCase))
                    {
                        //如果当前句子中找到的单词跟当前要匹配的单词一致，则匹配成功，则进行下一轮匹配，直到要匹配的词语结束
                        matchWordIndex++;
                        if (matchWordIndex >= matchWrodArray.Length)
                            return new Point ( firstMatchIndex ,i);
                        else
                            needSpace = true;
                    }
                    else
                    {
                        //否则，当前不匹配，中断处理，结束句子查找
                        break;
                    }
                }
            }//end for
            return new Point(-1, 0);
        }

        /// <summary>
        /// 从输入字符串的指定位置开始寻找一个临近的单词，忽略前面的空白字符，直到遇到单词之后第一个空白字符或者分割符或者标点符号为止。
        /// </summary>
        /// <param name="inputString">输入的字符串</param>
        /// <param name="startIndex">在输入字符串中要寻找单词的起始位置</param>
        /// <param name="maxLength">单词的最大长度，忽略超出部分</param>
        /// <param name="targetIndex">所找到目标单词在源字符串中的位置索引，如果没有找到，返回-1</param>
        /// <returns>找到的新单词</returns>
        public string FindNearWords(string inputString, int startIndex, int maxLength, out int targetIndex)
        {
            return FindNearWords(inputString, startIndex, maxLength, '[', ']', out targetIndex);
        }

        /// <summary>
        /// 从输入字符串的指定位置开始寻找一个临近的单词，忽略前面的空白字符，直到遇到单词之后第一个空白字符或者分割符或者标点符号为止。
        /// </summary>
        /// <param name="inputString">输入的字符串</param>
        /// <param name="startIndex">在输入字符串中要寻找单词的起始位置</param>
        /// <param name="maxLength">要索搜的最大长度，忽略超出部分，注意目标单词可能有空白字符，所以该长度应该大于单词的长度</param>
        /// <param name="leftSqlSeparator">SQL名字左分隔符</param>
        /// <param name="rightSqlSeparator">SQL名字右分隔符</param>
        /// <param name="targetIndex">所找到目标单词在源字符串中的位置索引，如果没有找到，返回-1</param>
        /// <returns>找到的新单词</returns>
        public string FindNearWords(string inputString, int startIndex, int maxLength, char leftSqlSeparator, char rightSqlSeparator,out int targetIndex)
        {
            if (startIndex + maxLength > inputString.Length)
                maxLength = inputString.Length - startIndex;
            bool start = false;
            char[] words = new char[maxLength];//存储过程名字，最大长度255；
            int index = 0;
            bool sqlSeparator = false;//SQL 分割符，比如[],如果遇到，则忽略中间的空格，标点符号等
            int countWhiteSpace = 0;
            targetIndex = -1;

            foreach (char c in inputString.ToCharArray(startIndex, maxLength))
            {
                if (Char.IsWhiteSpace(c))
                {
                    countWhiteSpace++;
                    if (!start)
                        continue;//过滤前面的空白字符
                    else
                    {
                        if (!sqlSeparator)
                            break;//已经获取过字母字符，又遇到了空白字符，说明单词已经结束，跳出。
                        else
                            words[index++] = c;//空白字符在SQL分隔符内部，放入字母，找单词
                    }
                }
                else
                {
                    if (Char.IsSeparator(c) || Char.IsPunctuation(c))
                    {
                        if (sqlSeparator && c == rightSqlSeparator)
                            sqlSeparator = false;
                        else if (c == leftSqlSeparator) 
                            sqlSeparator = true;
                        
                        //需要处理[] 之间的的字符串
                        if (c == '.' || c == '_' || c == '-' || c == leftSqlSeparator || c == rightSqlSeparator)
                        {
                            words[index++] = c;//放入字母，找单词
                        }
                        else
                        {
                            if (!sqlSeparator)
                                break;//分割符（比如空格）或者标点符号，跳出。
                        }

                    }
                    else
                    {
                        words[index++] = c;//放入字母，找单词

                    }
                    if (!start)
                    {
                        start = true;
                        targetIndex = startIndex + countWhiteSpace;
                    }
                }
            }
            return new string(words, 0, index);
        }

        /// <summary>
        /// 从一个句子中指定的位置开始搜索目标字符，直到遇到非空白字符为止，返回该字符的位置索引。该字符不包括在单引号内
        /// </summary>
        /// <param name="inputString">要搜索的源字符串</param>
        /// <param name="startIndex">在源中搜索的其实位置</param>
        /// <param name="targetChar">目标字符</param>
        /// <returns>目标字符的在源字符串的位置索引，如果没有找到，返回-1</returns>
        public int FindPunctuationBeforWord(string inputString, int startIndex, char targetChar)
        {
            if (startIndex >= inputString.Length - 1)
                return -1;
            bool isSqlStringFlag = false;
            for (int i = startIndex; i < inputString.Length; i++)
            {
                if (isSqlStringFlag)
                {
                    if (inputString[i] != '\'')
                        continue;
                    else
                        isSqlStringFlag = false;
                }
                else
                {
                    if (inputString[i] == targetChar)
                    {
                        return i;
                    }
                    else if (inputString[i] == '\'')
                    {
                        isSqlStringFlag = true;
                    }
                    else if (!char.IsWhiteSpace(inputString[i]))
                    {
                        break;
                    }
                }//end if
            }//end for
            return -1;
        }
    }

}
