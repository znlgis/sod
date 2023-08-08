Imports PWMIS.DataProvider.Adapter
Imports PWMIS.DataProvider.Data
Imports SqlMapDemo.SqlMapDemo.SqlMapDAL

Module Module1
    Sub Main()
        Console.WriteLine("PDF.NET SOD框架 SqlMap示例程序--2015.5.12-------")
        Console.WriteLine("http://www.pwmis.com/sqlmap ---------")
        Console.WriteLine("使用前，请确保目录下有TestDB.mdf 文件存在，详细内容情况[使用说明.txt],按任意键继续")
        Console.Read()
        Dim test As New TestSqlMapClass

        '取最后一个连接配置
        Dim db As AdoHelper = MyDB.GetDBHelper()
        'SQL-MAP DAL 默认也会取最后一个连接配置，所以下面一行代码可以注释
        'test.CurrentDataBase = db;
        Dim data As DataSet = test.QueryStudentSores()
        Console.WriteLine("查询到记录数量：{0}", data.Tables(0).Rows.Count)
        Dim list = test.GetStudentScore2(1)
        Console.WriteLine("测试完成。")
        Console.Read()
    End Sub
End Module
