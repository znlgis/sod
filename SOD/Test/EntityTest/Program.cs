using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Adapter;
using PWMIS.DataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace EntityTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("====**************** PDF.NET SOD 控制台测试程序 **************====");
            Assembly coreAss = Assembly.GetAssembly(typeof(AdoHelper));//获得引用程序集
            Console.WriteLine("框架核心程序集 PWMIS.Core Version:{0}", coreAss.GetName().Version.ToString());
            Console.WriteLine();
            Console.WriteLine("  应用程序配置文件默认的数据库配置信息：\r\n  当前使用的数据库类型是：{0}\r\n  连接字符串为:{1}\r\n  请确保数据库服务器和数据库是否有效，\r\n继续请回车，退出请输入字母 Q ."
                , MyDB.Instance.CurrentDBMSType.ToString(), MyDB.Instance.ConnectionString);
            Console.WriteLine("=====Power by Bluedoctor,2015.2.10 http://www.pwmis.com/sqlmap ====");
            string read = Console.ReadLine();
            if (read.ToUpper() == "Q")
                return;

            Console.WriteLine();
            Console.WriteLine("-------PDF.NET SOD 实体类 测试---------");
            //注册实体类
            EntityBuilder.RegisterType(typeof(IUser), typeof(UserEntity));

            UserEntity user = EntityBuilder.CreateEntity<IUser>() as UserEntity;
            bool flag = (user["User ID"] == null);//true
            Console.WriteLine("user[\"User ID\"] == null :{0}",flag);
            Console.WriteLine("user.UserID:{0}", user.UserID);

            Console.WriteLine("第一次运行，将检查并创建数据表");
            LocalDbContext context = new LocalDbContext();//自动创建表
            //删除测试数据
            OQL deleteQ = OQL.From(user)
                .Delete()
                .Where(cmp=>cmp.Comparer(user.UserID,">",0)) //为了安全，不带Where条件是不会全部删除数据的
                .END;
            context.UserQuery.ExecuteOql(deleteQ);
            Console.WriteLine("插入3条测试数据");
            //插入几条测试数据
            context.Add<UserEntity>(new UserEntity() {  FirstName ="zhang", LasttName="san" });
            context.Add<IUser>(new UserDto() { FirstName = "li", LasttName = "si", Age = 21 });
            context.Add<IUser>(new UserEntity() { FirstName = "wang", LasttName = "wu", Age = 22 });

            //查找姓张的一个用户
            UserEntity uq = new UserEntity() { FirstName = "zhang" };
            OQL q = OQL.From(uq)
               .Select(uq.UserID, uq.FirstName, uq.Age)
               .Where(uq.FirstName)
            .END;

            //下面的语句等效
            //UserEntity user2 = EntityQuery<UserEntity>.QueryObject(q,context.CurrentDataBase);
            UserEntity user2 = context.UserQuery.GetObject(q);
            //zhang san 的Age 未插入值，此时查询该字段的值应该是 NULL
            bool flag2 = (user2["Age"] == DBNull.Value);//true 
            Console.WriteLine("user[\"Age\"] == DBNULL.Value :{0}", flag);
            Console.WriteLine("user.Age:{0}", user2.Age);

            OQL q3 = OQL.From(uq)
              .Select(uq.UserID, uq.FirstName) //未查询 user.Age 字段
              .Where(uq.FirstName)
           .END;
            UserEntity user3 = context.UserQuery.GetObject(q3);
            //未查询 user.Age 字段，此时查询该字段的值应该是 null
            bool flag3 = (user3["Age"] == null);//true 
            Console.WriteLine("user[\"Age\"] == null :{0}", flag);
            Console.WriteLine("user.Age:{0}", user3.Age);

            Console.WriteLine("实体类序列化测试");
            var entityNameValues= user3.GetNameValues();
            PropertyNameValuesSerializer ser = new PropertyNameValuesSerializer(entityNameValues);
            string strEntity = ser.Serializer();
            Console.WriteLine(strEntity);
            Console.WriteLine("成功");
            //
            Console.WriteLine("反序列化测试");
            PropertyNameValuesSerializer des = new PropertyNameValuesSerializer(null);
            UserEntity desUser = des.Deserialize<UserEntity>(strEntity);
            Console.WriteLine("成功");

            Console.WriteLine();
            Console.WriteLine("----测试完毕，回车结束-----");
            Console.ReadLine();
        }
    }
}
