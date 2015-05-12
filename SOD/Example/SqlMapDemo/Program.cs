/*
 * 详细使用过程，请参考 http://www.cnblogs.com/bluedoctor/p/4498066.html
 * 使用前，请先按照 http://www.cnblogs.com/tianxue/p/4493260.html 准备数据
 */
using PWMIS.DataProvider.Adapter;
using PWMIS.DataProvider.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SqlMapDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("PDF.NET SOD框架 SqlMap示例程序--2015.5.12-------");
            Console.WriteLine("http://www.pwmis.com/sqlmap ---------");
            //AdoHelper db = new SqlServer();
            AdoHelper db = MyDB.GetDBHelper();
            SqlMapDemo.SqlMapDAL.TestSqlMapClass test = new SqlMapDAL.TestSqlMapClass();
            test.CurrentDataBase = db;
            DataSet data = test.QueryStudentSores();
            Console.WriteLine("查询到记录数量：{0}",data.Tables[0].Rows.Count);
            Console.WriteLine("测试完成。");
            Console.Read();
        }
    }
}
