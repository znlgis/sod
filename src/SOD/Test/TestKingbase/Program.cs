using PWMIS.DataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestKingbase
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AdoHelper kdb = new Kingbase();
            kdb.ConnectionString = "Server=192.168.13.117;User Id=network;Password=network123;Database=network;Port=54322";
            kdb.OpenSession();
            kdb.CloseSession();
            Console.WriteLine("连接成功。");
        }
    }
}
