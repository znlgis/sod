using PWMIS.DataMap.Entity;
using System;

namespace ConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            NotifyingArrayList<string> nc = new NotifyingArrayList<string>("ID");
            NotifyingArrayList<string> nc1 = nc;
            var nc2= nc.Add("ID2");

            OldSimpleEntity ose = new OldSimpleEntity();
            SimpleEntity se = new SimpleEntity();
            SimpleEntity se2 = new SimpleEntity();
            se2.MapNewTableName("Table_2");
            Console.ReadLine();
        }
    }
}
