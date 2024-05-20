/*
 * SOD  使用.NET 4x发布
 * SOD5 使用.NET Standard 2.0发布，它可以被.NET 4.7使用。有关.NET Standard 版本兼容性问题，请参考下面链接：
 * https://learn.microsoft.com/zh-cn/dotnet/standard/net-standard?tabs=net-standard-1-0
 * 
 * SOD6 将直接使用.NET 6.0，支持.NET 6以及之后的版本。
 */
using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;

namespace ConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            JsonObject json = new JsonObject();
            var node1 = System.Text.Json.JsonSerializer.SerializeToNode("aaa", typeof(string));
            var node2 = System.Text.Json.JsonSerializer.SerializeToNode(DateTime.Now, typeof(DateTime));
            json.Add("name", node1);
            json.Add("date",node2 );
            string jsonString= json.ToString();
            Console.WriteLine("System.Text.Json test:{0}", jsonString);
            var jobj = JsonObject.Parse(jsonString);
            var jo2 = jobj.AsObject();
            foreach (var node in jo2.AsEnumerable())
            {
                Console.WriteLine("key:{0},value:{1}", node.Key, node.Value);
            }


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

            se["AtTime"] = DateTime.Now.ToString();
            Console.WriteLine("AtTime:{0}", se.AtTime);
            try
            {
                //字段长度超长测试
                se.Name = new string('a', 101);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
            Console.ReadLine();
        }

       
    }
}
