/*
 * SOD5 使用.NET Standard 2.0发布，它可以被.NET 4.7使用。有关.NET Standard 版本兼容性问题，请参考下面链接：
 * https://learn.microsoft.com/zh-cn/dotnet/standard/net-standard?tabs=net-standard-1-0
 * 
 * SOD6 将直接使用.NET 6.0
 */
using PWMIS.DataMap.Entity;
using System;
using System.Linq;

namespace ConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            NotifyingArrayList<string> nc = new NotifyingArrayList<string>("ID");
            NotifyingArrayList<string> nc1 = nc;    //nc1=["ID"]
            var nc2= nc.Add("ID2");                 //nc2=["ID","ID2"],nc=["ID"]

            EntityMetaData meta = new EntityMetaData() {IdentityName="ID", TableName = "table1" };
            EntityMetaData meta2 = meta with { TableName = "table2" };
            meta.TableName = "table1_1";

            SharedStringList<SimpleEntity> ssl_a = new SharedStringList<SimpleEntity>();
            SharedStringList<SimpleEntity> ssl_b = new SharedStringList<SimpleEntity>();
            SharedStringList<SimpleEntity> ssl_c = new SharedStringList<SimpleEntity>();
            ssl_a.Add("A");//首次操作，添加数据到共享实例,    ssl_a=["A"]
            ssl_b.Add("A");//使用共享实例数据，不会重复添加,  ssl_b=["A"]
            ssl_a.Add("B");//非首次操作，添加数据到当前实例,  ssl_a=["A","B"]
            ssl_a.Add("A");//在当前实例上不会重复添加,        ssl_a=["A","B"]
            string str1 = ssl_c.First();//获取共享实例数据 "A"
            Console.WriteLine("Shared Data:{0}", str1);

            //OldSimpleEntity ose = new OldSimpleEntity();
            SimpleEntity se = new SimpleEntity() { Name = "name1" };
            SimpleEntity se2 = new SimpleEntity() { Name = "name2" };
            se2.PrimaryKeys.Add("ID2");
            se.PrimaryKeys.Clear();
            se.PrimaryKeys.Add("ID0");
            se2.PrimaryKeys.Remove("ID2");
            se2.MapNewTableName("Table_2");
            Console.WriteLine("TableName:{0}",se2.GetTableName());
            Console.ReadLine();
        }
    }
}
