/*
 * 详细使用过程，请参考 http://www.cnblogs.com/bluedoctor/p/4498066.html
 * 使用前，请先按照 http://www.cnblogs.com/tianxue/p/4493260.html 准备数据
 */

using System;
using PWMIS.DataProvider.Adapter;
using SqlMapDemo.SqlMapDAL;

namespace SqlMapDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("PDF.NET SOD框架 SqlMap示例程序--2015.5.12-------");
            Console.WriteLine("http://www.pwmis.com/sqlmap ---------");
            Console.WriteLine("使用前，请确保目录下有TestDB.mdf 文件存在，详细内容情况[使用说明.txt],按任意键继续");
            Console.Read();

            Console.WriteLine("---简单查询（CRUD）测试----------");
            var smm = new ScoreManagement();
            var ds = smm.GetAllStudents();
            Console.WriteLine("当前共有学生{0} 名。", ds.Tables[0].Rows.Count);

            var list = smm.GetStudentScore2(1);
            var entityList = smm.GetStudentScoreEntitys(1);

            Console.Read();
            Console.WriteLine("请输入学生名字（直接回车忽略）：");
            var studentName = Console.ReadLine();
            if (!string.IsNullOrEmpty(studentName))
            {
                var deptId = 1; //1,计算机；2，生物；3，数学
                var flag = smm.AddStudent(studentName, deptId);
                if (flag > 0)
                    Console.WriteLine("增加学生信息成功！");
            }

            Console.WriteLine("---复杂查询测试----------");
            //取最后一个连接配置
            var db = MyDB.GetDBHelper();
            //SQL-MAP DAL 默认也会取最后一个连接配置，所以下面一行代码可以注释
            var test = new TestSqlMapClass();
            //test.CurrentDataBase = db;
            var data = test.QueryStudentSores();
            Console.WriteLine("查询到记录数量：{0}", data.Tables[0].Rows.Count);

            Console.WriteLine("测试完成。");
            Console.Read();
        }
    }
}