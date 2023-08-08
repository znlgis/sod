using System;
using System.Data.SqlClient;
using System.Reflection;
using PWMIS.Core.Extensions;
using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Adapter;
using PWMIS.DataProvider.Data;

namespace EntityTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("====**************** PDF.NET SOD 控制台测试程序 **************====");
            var coreAss = Assembly.GetAssembly(typeof(AdoHelper)); //获得引用程序集
            Console.WriteLine("框架核心程序集 PWMIS.Core Version:{0}", coreAss.GetName().Version);
            Console.WriteLine();
            Console.WriteLine(
                "  应用程序配置文件默认的数据库配置信息：\r\n  当前使用的数据库类型是：{0}\r\n  连接字符串为:{1}\r\n  请确保数据库服务器和数据库是否有效，\r\n继续请回车，退出请输入字母 Q ."
                , MyDB.Instance.CurrentDBMSType.ToString(), MyDB.Instance.ConnectionString);
            Console.WriteLine("=====Power by Bluedoctor,2015.2.10 http://www.pwmis.com/sqlmap ====");
            var read = Console.ReadLine();
            if (read.ToUpper() == "Q")
                return;

            Console.WriteLine();
            Console.WriteLine("-------PDF.NET SOD 实体类 测试---------");

            //注册实体类
            EntityBuilder.RegisterType(typeof(IUser), typeof(UserEntity));

            var user = EntityBuilder.CreateEntity<IUser>() as UserEntity;
            var flag = user["User ID"] == null; //true
            Console.WriteLine("虚拟字段User ID: user[\"User ID\"] == null :{0}", flag);
            Console.WriteLine("读取属性 user.UserID:{0}", user.UserID);

            user["User ID"] = "123";
            Console.WriteLine("虚拟字段赋值 user[\"User ID\"] = \"123\" :{0}", true);
            Console.WriteLine("读取虚拟字段 user[\"User ID\"]:{0}", user["User ID"]);
            user["Age"] = "";
            Console.WriteLine("属性赋值 user[\"Age\"] = \"\" :{0}", true);
            Console.WriteLine("读取属性 user.Age:{0}", user.Age);
            Console.WriteLine("空值判断 user[\"Age\"]==null:{0}", user["Age"] == null);

            //分库分表测试
            Console.WriteLine("-------------测试分表分库----------------");
            TestPartitionEntity(100);
            TestPartitionEntity(1000);
            TestPartitionEntity(2000);

            TestDBPartition(100);
            TestDBPartition(1000);
            TestDBPartition(2000);

            Console.WriteLine("第一次运行，将检查并创建数据表");
            var context = new LocalDbContext(); //自动创建表
            //删除测试数据
            var deleteQ = OQL.From(user)
                .Delete()
                .Where(cmp => cmp.Comparer(user.UserID, ">", 0)) //为了安全，不带Where条件是不会全部删除数据的
                .END;
            context.UserQuery.ExecuteOql(deleteQ);
            Console.WriteLine("插入3条测试数据");
            //插入几条测试数据
            context.Add(new UserEntity { FirstName = "zhang", LasttName = "san" });
            context.Add<IUser>(new UserDto { FirstName = "li", LasttName = "si", Age = 21 });
            context.Add<IUser>(new UserEntity { FirstName = "wang", LasttName = "wu", Age = 22 });

            //查找姓张的一个用户
            var uq = new UserEntity { FirstName = "zhang" };
            var q = OQL.From(uq)
                .Select(uq.UserID, uq.FirstName, uq.Age)
                .Where(uq.FirstName)
                .END;

            //下面的语句等效
            //UserEntity user2 = EntityQuery<UserEntity>.QueryObject(q,context.CurrentDataBase);
            var user2 = context.UserQuery.GetObject(q);
            //zhang san 的Age 未插入值，此时查询该字段的值应该是 NULL
            var flag2 = user2["Age"] == DBNull.Value; //true 
            Console.WriteLine("user[\"Age\"] == DBNULL.Value :{0}", flag);
            Console.WriteLine("user.Age:{0}", user2.Age);

            var q3 = OQL.From(uq)
                .Select(uq.UserID, uq.FirstName) //未查询 user.Age 字段
                .Where(uq.FirstName)
                .END;
            var user3 = context.UserQuery.GetObject(q3);
            //未查询 user.Age 字段，此时查询该字段的值应该是 null
            var flag3 = user3["Age"] == null; //true 
            Console.WriteLine("user[\"Age\"] == null :{0}", flag);
            Console.WriteLine("user.Age:{0}", user3.Age);

            //模糊查询测试
            var q5 = OQL.From(uq)
                .Select(uq.UserID, uq.FirstName, uq.LasttName) //未查询 user.Age 字段
                .Where(cmp => cmp.Comparer(uq.LasttName, "like", "%san456789%"))
                .END;
            var user5 = context.UserQuery.GetObject(q5);

            Console.WriteLine("实体类序列化测试");
            var entityNameValues = user3.GetNameValues();
            //序列化之后的属性是否修改的情况测试,下面的实体类,LastName 属性没有被修改
            var user4 = new UserEntity { UserID = 100, Age = 20, FirstName = "zhang san" };
            entityNameValues = user4.GetChangedValues();
            var ser = new PropertyNameValuesSerializer(entityNameValues);
            var strEntity = ser.Serializer();
            Console.WriteLine(strEntity);
            Console.WriteLine("成功");
            //
            Console.WriteLine("反序列化测试");
            var des = new PropertyNameValuesSerializer(null);
            var desUser = des.Deserialize<UserEntity>(strEntity);
            Console.WriteLine("成功");

            //实体类属性拷贝
            var userTemp = new { FirstName = "zhang ", LasttName = "san" };
            var userTest = new UserEntity();
            userTest.MapFrom(userTemp, true);
            userTest.Age = 20;
            userTest.MapToPOCO(new UserDto());

            user3 = context.UserQuery.GetObject(q3);
            user3.MapToPOCO(new UserDto());

            var ue = new UserEntity();
            var q4 = OQL.From(ue).Select().END;
            var list4 = EntityQuery<UserEntity>.QueryList(q4);
            var ud = new UserDto { Age = 20 };
            //请注意list4[0].Age属性，由于数据库对应的值为DBNull.Value，所以下面的代码不会被覆盖该属性值。
            list4[0].MapToPOCO(ud);


            //实体类JSON测试
            var model = new UserEntity();
            model.UserID = 1;
            model.FirstName = "tian";
            model.LasttName = "jie";
            model.Age = 2;
            var json = model.ToJson();
            var obj = new UserEntity();
            obj.FromJson(json);
            Console.WriteLine("实体类JSON测试成功");


            Console.WriteLine();
            Console.WriteLine("----测试完毕，回车结束-----");
            Console.ReadLine();
        }

        private static void TestPartitionEntity(int userId)
        {
            var upe = new UserPartitionEntity();
            upe.UserID = userId;
            upe.FirstName = "zhang";
            upe.LasttName = "san";
            upe.Age = 20;
            var insertQ1 = OQL.From(upe).Insert(upe.UserID, upe.FirstName, upe.LasttName, upe.Age);

            Console.WriteLine("Partition When user id={0}, Insert SQL:\r\n{1}", userId, insertQ1);
            Console.WriteLine("{0}", insertQ1.PrintParameterInfo());

            upe.Age = 30;
            var updateQ1 = OQL.From(upe).Update(upe.Age).END;
            Console.WriteLine("Partition When user id={0}, Update SQL:\r\n{1}", userId, updateQ1);
            Console.WriteLine("{0}", updateQ1.PrintParameterInfo());


            var selectQ1 = OQL.From(upe).Select().Where(cmp => cmp.Comparer(upe.Age, "<=", 35)).END;
            Console.WriteLine("Partition When user id={0}, SELECT SQL:\r\n{1}", userId, selectQ1);
            Console.WriteLine("{0}", selectQ1.PrintParameterInfo());
        }

        private static void TestDBPartition(int userId)
        {
            var upe = new UserPartitionEntity2();
            upe.UserID = userId;
            upe.FirstName = "zhang";
            upe.LasttName = "san";
            upe.Age = 20;
            var insertQ1 = OQL.From(upe).Insert(upe.UserID, upe.FirstName, upe.LasttName, upe.Age);

            Console.WriteLine("Partition When user id={0}, Insert SQL:\r\n{1}", userId, insertQ1);
            Console.WriteLine("{0}", insertQ1.PrintParameterInfo());

            AdoHelper helper = new SqlServer();
            var stringBuilder = helper.ConnectionStringBuilder;
            var sqlConnStrBuilder = (SqlConnectionStringBuilder)stringBuilder;
            sqlConnStrBuilder.InitialCatalog = upe.GetDatabaseName();
            sqlConnStrBuilder.DataSource = upe.GetServerName();
            sqlConnStrBuilder.UserID = "sa";
            sqlConnStrBuilder.Password = "sa123";
            //重写设置ConnectionString
            helper.ConnectionString = sqlConnStrBuilder.ConnectionString;
            Console.WriteLine("When user id={0}, DB Partition Connection String :\r\n  {1}",
                userId, helper.ConnectionString);
            //查询分片的数据库，下面仅示例修改连接字符串，先注释下面一行代码
            //EntityQuery.ExecuteOql(insertQ1, helper);
        }
    }
}