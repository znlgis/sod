using SOD.DataSync.Entitys;
using PWMIS.DataProvider.Data;
using PWMIS.MemoryStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOD.DataSync
{
    class Program
    {
        static void Main(string[] args)
        {
            //命令行格式：
            // DataSync.exe mode dbName auditworkProjectID
            System.Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string mode = "";
            string dbName = "DemoDB";
            string projectID = "PRJID-TEST-1111";
            //数据提交类型：提交复核/提交归档
         
            if (args.Length > 0)
                mode = args[0].ToLower();
            if (args.Length > 1)
                dbName = args[1];
            if (args.Length > 2)
                projectID = args[2];
          

            try
            {
                if (mode == "/export")
                {
                    Export(dbName, projectID);
                }
                else if (mode == "/import")
                {
                    Import(dbName, projectID, "TargetDB");
                }
                else if (mode == "/auto")
                {
                    var result = Export(dbName, projectID);
                    if (result.Item2)
                    {
                        //导出成功才上传和远程导入
                        string dbPath = result.Item1;
                        Import(dbName, projectID, "TargetDB");

                        //服务器导入成功，删除本地数据文件，需确保之前已经备份（导出后就会备份）
                        string dataFolder = System.IO.Path.Combine(dbPath, "Data");
                        var files = System.IO.Directory.GetFiles(dataFolder);
                        foreach (string file in files)
                        {
                            if (file.Contains("PWMIS.MemoryStorage.ExportBatchInfo.pmdb"))
                                continue;
                            System.IO.File.Delete(file);
                        }
                        Console.WriteLine("导入数据成功，数据文件备份和清理完成。");

                        string logFile = System.IO.Path.Combine(dbPath, "AutoDataSync.log");
                        string logText = string.Format("{0} 自动导出、导入成功，本地数据文件已经清除，详细请看数据导出日志。\r\n",
                            DateTime.Now);
                        System.IO.File.AppendAllText(logFile, logText);
                    }
                    else
                    {
                        //-1 没有数据需要提交
                        System.Environment.Exit(-1);
                    }
                }
                else
                {
                    Console.WriteLine("数据导出 程序测试，请输入项目ID，输入 exit 退出");
                    string input = Console.ReadLine();
                    if (input == "exit")
                    {
                        //如果抛出错误，控制台程序会得到一个非0的退出代码
                        //调用程序需要检查此退出代码
                        //DOS 下查看退出代码：echo %errorlevel%
                        //throw new Exception("user exit.");
                        System.Environment.Exit(1);
                        return;
                    }
                    //初始化数据
                    InitData(dbName,input);
                    //导出数据
                    var result = Export(dbName, input);
                    Console.WriteLine();
                    if (result.Item2)
                    {
                        Console.WriteLine("数据导入 程序测试，按任意键开始。");
                        Console.ReadLine();
                        Import(dbName, input, "TargetDB");
                        //
                        string dbPath = result.Item1;
                        //目标数据库导入成功，删除本地数据文件，需确保之前已经备份（导出后就会备份）
                        string dataFolder = System.IO.Path.Combine(dbPath, "Data");
                        var files = System.IO.Directory.GetFiles(dataFolder);
                        foreach (string file in files)
                        {
                            if (file.Contains("PWMIS.MemoryStorage.ExportBatchInfo.pmdb"))
                                continue;
                            System.IO.File.Delete(file);
                        }
                        Console.WriteLine("导入数据成功，数据文件备份和清理完成。");
                    }
                   
                    Console.WriteLine("----------测试全部完成------------");
                    Console.WriteLine("按任意键退出");
                    Console.Read();
                }
                System.Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("DataSync Error:{0}", ex.ToString());
                System.Environment.Exit(20171122);
            }
        }

        static void InitData(string dbName,string prjId)
        {
            DemoDbContext context = new DemoDbContext(AdoHelper.CreateHelper(dbName));
            //分类ID

            int delId = 0;
            for (int i = 0; i < 100; i++)
            {
                TestEntity test = new TestEntity();
                test.Name = "Name"+i;
                test.AtTime = DateTime.Now;
                test.Classification = prjId;
                context.Add(test);

                UserEntity user = new UserEntity();
                user.Name = "User"+i;
                user.Sex = false ;
                user.Height = 1.6f + i / 10;
                user.Birthday = new DateTime(1990, 1, 1).AddDays (i);
                context.Add(user);

                if (i == 50)
                    delId = test.ID;
            }
            //删除数据，确保被删除的ID写入到ID删除记录表中
            using (IWriteDataWarpper warpper = WriteDataWarpperFactory.Create(context))
            {
                context.Remove(new TestEntity() { ID =delId });
                context.Remove(new UserEntity () { UID = delId });
            }
        }

        /// <summary>
        /// 导出数据，返回数据文件路径和成功标记
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        private static Tuple<string, bool> Export(string dbName, string projectID)
        {
            string DbPath = string.Empty;
            bool result;
            string dataSource = MemDBEngin.DbSource;
            string objDataSource = System.IO.Path.Combine(dataSource, dbName + "_" + projectID);
            //在此路径下写入标记文件，如果文件存在表示曾经导出了数据包但是上传导入没有成功，需要再次上传和导入，本次不导出。
            //考虑合并没有上传完成的内存数据库数据
            using (MemDB mem = MemDBEngin.GetDB(objDataSource))
            {
                result = ExportData(mem, dbName, projectID);
                Console.WriteLine("数据源{0} 导出数据完成，结果：{1}",dbName , result);
                DbPath = mem.Path;
            }
            return new Tuple<string, bool>(DbPath, result);
        }


        /// <summary>
        /// 导入数据至远程作业库(BS端)
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="projectID"></param>
        /// <param name="targetDataSource">提交复核/提交归档</param>
        private static void Import(string dbName, string projectID,string targetDataSource)
        {
            string dataSource = MemDBEngin.DbSource;
            string objDataSource = System.IO.Path.Combine(dataSource, dbName + "_" + projectID);
            Console.WriteLine("数据源:" + objDataSource);
            using (MemDB mem = MemDBEngin.GetDB(objDataSource))
            {
                ImportData(mem, targetDataSource, projectID);
                Console.WriteLine("向目标数据源{0} 导入数据完成", targetDataSource);
            }
        }


        private static void CopyFiles(string sourceFolder, string descFolder)
        {
            if (!System.IO.Directory.Exists(descFolder))
                System.IO.Directory.CreateDirectory(descFolder);
            foreach (string path in System.IO.Directory.GetFiles(sourceFolder))
            {
                string fileName = System.IO.Path.GetFileName(path);
                string targetPath = System.IO.Path.Combine(descFolder, fileName);
                System.IO.File.Copy(path, targetPath, true);

                Console.WriteLine("copy \"{0}\" to \"{1}\" ,OK.", path, targetPath);
            }
        }
        /// <summary>
        /// 数据导出
        /// </summary>
        /// <param name="mem"></param>
        /// <param name="dbName"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        private static bool ExportData(MemDB mem, string dbName, string projectID)
        {
            //AdoHelper db= AuditWorkDbManage.GetAuditWorkDbAdoByCustomerID("");
            AdoHelper db = AdoHelper.CreateHelper(dbName);
            DemoDbContext localDbContext = new DemoDbContext(db);
            //导出数据
            SimpleExportEntitys ee = new SimpleExportEntitys(mem, localDbContext);
            ee.ClassificationID  = projectID;
           
            ee.DoExportData ();

            Console.WriteLine("AllSucceed:{0},Have Data Table Count:{1}", ee.AllSucceed, ee.HaveDataTableCount);
            Console.WriteLine("数据文件备份目录：{0}", ee.DataBackFolder);
            return ee.AllSucceed && ee.HaveDataTableCount > 0;
        }
        /// <summary>
        /// 数据导入
        /// </summary>
        /// <param name="mem"></param>
        /// <param name="dbName"></param>
        /// <param name="projectID"></param>
        /// <param name="dataSyncType">提交复核/提交归档</param>
        private static void ImportData(MemDB mem, string dbName, string projectID)
        {
            //AduitWorkDbContext remoteDbContext = new AduitWorkDbContext(AdoHelper.CreateHelper("RemoteAuditWork"));
            AdoHelper db = AdoHelper.CreateHelper(dbName);
            DemoDbContext remoteDbContext = new DemoDbContext(db);
            SimpleImportEntitys importer = new SimpleImportEntitys(mem, remoteDbContext);
            importer.Classification  = projectID;

            importer.DoImportData ();
        }



    }
}
