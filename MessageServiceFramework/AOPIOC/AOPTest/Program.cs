using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AOPTestLib;
using System.Diagnostics;
using PWMIS.EnterpriseFramework.AOP;

namespace AOPTest
{
    class Program
    {
        static void Main(string[] args)
        {
            AOPFactory factory = new AOPFactory();
            Stopwatch sw = new Stopwatch();
            Console.WriteLine("AOP 测试开始...");
            sw.Start();
            int allCount = 35000;
            for (int i = 0; i < allCount; i++)
            {
                BizObject obj = new BizObject();
                int result = obj.GetValue();
            }
            sw.Stop();

            Console.WriteLine("New Object 测试，执行{0}次耗时{1} ms", allCount, sw.Elapsed.TotalMilliseconds);
            Console.WriteLine("New Object 测试，平均每次耗时{0} ms", sw.Elapsed.TotalMilliseconds / allCount);
            sw.Reset();
            sw.Start();
            for (int i = 0; i < allCount; i++)
            {
                IBizObject obj = factory.Create<IBizObject>(new BizObject());
                int result = obj.GetValue();
            }
            sw.Stop();
            Console.WriteLine("AOP 测试，执行{0}次耗时{1} ms", allCount, sw.Elapsed.TotalMilliseconds);
            Console.WriteLine("AOP 测试，平均每次耗时{0} ms", sw.Elapsed.TotalMilliseconds / allCount);

            sw.Reset();
            sw.Start();
            for (int i = 0; i < allCount; i++)
            {
                IBizObject obj = factory.Create<IBizObject>();
                int result = obj.GetValue();
            }
            sw.Stop();
            Console.WriteLine("AOP+IOC 测试，执行{0}次耗时{1} ms", allCount, sw.Elapsed.TotalMilliseconds);
            Console.WriteLine("AOP+IOC 测试，平均每次耗时{0} ms", sw.Elapsed.TotalMilliseconds / allCount);
            Console.Read();
        }
    }


   
}
