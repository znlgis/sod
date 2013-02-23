using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PWMIS.DataProvider.Data;
using PWMIS.DataProvider.Adapter;
using System.Data;

namespace TestConsoleApp
{
    public class Class1
    {
        static void Main(string[] args)
        {
             string sql = @"SELECT `工号`,`姓名` 
FROM `雇员表`
   Where `职务名称`='ss'
       ORDER BY `姓名` asc";

            AdoHelper db = MyDB.Instance;
            try
            {
                DataSet ds = db.ExecuteDataSet(sql);
                Console.WriteLine("test db access ok.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Test Error:"+ ex.Message);
            }
            Console.Read();
        }
       
    }
}
