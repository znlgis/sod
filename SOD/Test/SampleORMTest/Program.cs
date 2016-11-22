/*
 * 有关本程序的详细说明，请参考博客文章
 * 《.NET ORM 的 “SOD蜜”--零基础入门篇》-- http://www.cnblogs.com/bluedoctor/p/4306131.html  
 */
using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Adapter;
using PWMIS.DataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;


namespace SampleORMTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("====**************** PDF.NET SOD ORM 控制台测试程序 **************====");
            Assembly coreAss = Assembly.GetAssembly(typeof(AdoHelper));//获得引用程序集
            Console.WriteLine("框架核心程序集 PWMIS.Core Version:{0}", coreAss.GetName().Version.ToString());
            Console.WriteLine();
            Console.WriteLine(@"====应用程序配置文件默认的数据库配置信息：===========================
当前使用的数据库类型是：{0}
连接字符串为:{1}
请确保数据库服务器和数据库是否有效（SqlServer,Access 会自动创建数据库），
继续请回车，退出请输入字母 Q ."
                , MyDB.Instance.CurrentDBMSType.ToString(), MyDB.Instance.ConnectionString);
            Console.WriteLine("=====Power by Bluedoctor,2015.3.1 http://www.pwmis.com/sqlmap =======");
            string read = Console.ReadLine();
            if (read.ToUpper() == "Q")
                return;

            Console.WriteLine();
            Console.WriteLine("-------PDF.NET SOD ORM 测试 开始 ---------");
            //自动创建数据库和表
            LocalDbContext context = new LocalDbContext();
            int count = 0;

            //测试查询&&更新
            User zhang_san = new User() { Name = "zhang san" };
            User dbZhangSan = EntityQuery<User>.QueryObject(
                OQL.From(zhang_san)
                    .Select(zhang_san.ID, zhang_san.Name, zhang_san.Pwd)
                    .Where(cmp => cmp.EqualValue(zhang_san.Name))
                .END
                );
            if (dbZhangSan != null)
            {
                dbZhangSan.Pwd = "111111";
                dbZhangSan.RegistedDate = DateTime.Now;

                int updateCount = EntityQuery<User>.Instance.Update(dbZhangSan);
                Console.WriteLine("测试：用户{0} 的密码和注册日期已经更新",zhang_san.Name);
            }
            else
            {
                Console.WriteLine("测试：用户{0} 的记录不存在，如果需要测试查询和更新，请先添加一条记录。继续测试，请按任意键。",zhang_san.Name );
                Console.Read();
            }
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            //删除 测试数据-----------------------------------------------------
            User user = new User();
            OQL deleteQ = OQL.From(user)
                .Delete()
                .Where(cmp => cmp.Comparer(user.ID, ">", 0)) //为了安全，不带Where条件是不会全部删除数据的
                .END;
            count= EntityQuery<User>.ExecuteOql(deleteQ, context.CurrentDataBase);
            Console.WriteLine("--删除 {0}条数据--",count);
            //-----------------------------------------------------------------

            //插入 测试数据-----------------------------------------------------
            count = 0;
            zhang_san = new User() { Name = "zhang san", Pwd = "123" };
            count += context.Add<User>(zhang_san);//采用 DbContext 方式插入数据

            User li_si = new User() { Name = "li si", Pwd = "123" };
            OQL insertQ= OQL.From(li_si)
                            .Insert(li_si.Name);//仅仅插入用户名，不插入密码

            //采用OQL方式插入指定的数据
            //同时演示事务使用方法
            AdoHelper db = MyDB.GetDBHelperByConnectionName("local");
            EntityQuery<User> userQuery = new EntityQuery<User>(db);
            //db.BeginTransaction();
            //try
            //{
            //    count += userQuery.ExecuteOql(insertQ);
                
            //    // userQuery.GetInsertIdentity(insertQ); 获取插入标识，用词方法代替下面的方式
            //    //OQL 方式没法取得自增数据，所以下面单独查询
            //    object obj_id = db.ExecuteScalar(db.InsertKey);
            //    db.Commit();
            //    li_si.ID = Convert.ToInt32( obj_id);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("AdoHelper 执行事务异常："+ex.Message );
            //    db.Rollback();
            //    Console.WriteLine("按任意键退出！");
            //    return;
            //}

            //上面的代码注释，采用下面封装的代码：ExecuteInsrtOql
            li_si.ID =userQuery.ExecuteInsrtOql(insertQ);
            for (int i = 0; i < 1000; i++)
            {
                User zhang_yeye = new User() { ID=1000+i, Name = "zhang yeye" + i, Pwd = "pwd" + i };
                count += EntityQuery<User>.Instance.Insert(zhang_yeye);//采用泛型 EntityQuery 方式插入数据
            }
           

            Console.WriteLine("--插入 {0}条数据--", count);
            //-----------------------------------------------------------------

            //修改 测试数据----------------------------------------------------
            li_si.Pwd = "123123";
            count = context.Update<User>(li_si);//采用 DbContext 方式更新数据

            li_si.Pwd = "123456";
            OQL updateQ= OQL.From(li_si)
                            .Update(li_si.Pwd) //仅仅更新密码
                            .END;
            count += EntityQuery<User>.Instance.ExecuteOql(updateQ);//采用OQL方式更新指定的数据

            li_si.Pwd = "888888";
            count += EntityQuery<User>.Instance.Update(li_si);//采用泛型 EntityQuery 方式修改数据
            Console.WriteLine("--修改 {0}次数据，User ID：{1}--", count,li_si.ID);
            //-----------------------------------------------------------------

            //查询数据---------------------------------------------------------
            Console.WriteLine("SOD ORM的 6种 查询方式，开始----");
            UserLoginService service = new UserLoginService();
            Console.WriteLine("Login0:{0}", service.Login(zhang_san));
            Console.WriteLine("Login1:{0}", service.Login1(zhang_san));
            Console.WriteLine("Login2:{0}", service.Login2("zhang san","123"));
            Console.WriteLine("Login3:{0}", service.Login3("zhang san", "123"));
            Console.WriteLine("Login4:{0}", service.Login4("zhang san", "123"));
            Console.WriteLine("Login5:{0}", service.Login5("zhang san", "123"));
            Console.WriteLine("Login6:{0}", service.Login6("zhang yeye", "pwd100"));
            //查询列表
            var users=service.FuzzyQueryUser("zhang");
            Console.WriteLine("模糊查询姓 张 的用户，数量:{0}",users.Count );
            //-----------------------------------------------------------------

            Console.WriteLine("-------PDF.NET SOD ORM 测试 全部结束----- ");

            watch.Stop();
            Console.WriteLine("耗时：(ms)"+watch.ElapsedMilliseconds);
            Console.Read();
        }
    }
}
