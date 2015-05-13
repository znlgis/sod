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
           
            SqlMapDemo.SqlMapDAL.TestSqlMapClass test = new SqlMapDAL.TestSqlMapClass();

            //AdoHelper db = new SqlServer();
            //取最后一个连接配置
            AdoHelper db = MyDB.GetDBHelper();
            //SQL-MAP DAL 默认也会取最后一个连接配置，所以下面一行代码可以注释
            //test.CurrentDataBase = db;
            DataSet data = test.QueryStudentSores();

            Console.WriteLine("查询到记录数量：{0}",data.Tables[0].Rows.Count);
            Console.WriteLine("测试完成。");
            Console.Read();
        }
    }
}
