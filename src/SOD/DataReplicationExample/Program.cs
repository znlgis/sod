using PWMIS.Common;
using PWMIS.Core.Extensions;
using PWMIS.DataProvider.Adapter;
using PWMIS.DataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DataReplicationExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("====**************** PDF.NET SOD 控制台测试程序 **************====");
            Assembly coreAss = Assembly.GetAssembly(typeof(AdoHelper));//获得引用程序集
            Console.WriteLine("框架核心程序集 PWMIS.Core Version:{0}", coreAss.GetName().Version.ToString());
            Console.WriteLine();

            AdoHelper sourceDb = AdoHelper.CreateHelper("SourceDB");
            AdoHelper targetDb = AdoHelper.CreateHelper("ReplicatedDB");

            Console.WriteLine(" 源数据库配置信息：\r\n  当前使用的数据库类型是：{0}\r\n  连接字符串为:{1}"
                , sourceDb.CurrentDBMSType, sourceDb.ConnectionString);
            Console.WriteLine("目标数据库配置信息：\r\n  当前使用的数据库类型是：{0}\r\n  连接字符串为:{1}"
               , targetDb.CurrentDBMSType, targetDb.ConnectionString);

            Console.WriteLine("\r\n  请确保数据库服务器和数据库是否有效(如果是SqlServer，框架会自动创建数据库)，\r\n继续请回车，退出请输入字母 Q .");
            Console.WriteLine("=====Power by Bluedoctor,2018.3.12 http://www.pwmis.com/sqlmap ====");
            string read = Console.ReadLine();
            if (read.ToUpper() == "Q")
                return;

            
            //初始化数据库
            TransLogDbContext sourceCtx = new TransLogDbContext(sourceDb);
            sourceCtx.CheckTableExists<UserEntity>();
            Console.WriteLine("----源数据库初始化检查成功-------");

            Console.Write("是否在源库写入数据？(Y/N)");
            var ckey= Console.ReadKey();
            if (ckey.KeyChar == 'y' || ckey.KeyChar == 'Y')
            {
                TransactionLogHandle logHandle = new TransactionLogHandle();
                logHandle.BeforLog = log => {
                    //只有在指定范围内的表并且符合对此表指定的操作行为，才会记录事务日志
                    //定义操作策略
                    Dictionary<string, SQLOperatType> tableStrategy = new Dictionary<string, SQLOperatType>() {
                        { "Table_User",     SQLOperatType.Insert | SQLOperatType.Update  }
                    };
                    //执行策略
                    foreach (string key in tableStrategy.Keys)
                    {
                        if (string.Compare(log.CommandName, key) == 0)
                        {
                            SQLOperatType sqlType = tableStrategy[key];
                            return (sqlType & log.SQLType) != 0;
                        }
                    }
                    return false;
                };
                //在源数据库注册事务日志命令处理器
                sourceDb.RegisterCommandHandle(logHandle);

                //在源添加和修改数据
                UserEntity user = new UserEntity();
                user.Name = "zhang san";
                user.Height = 1.8f;
                user.Birthday = new DateTime(1980, 1, 1);
                user.Sex = true;
                sourceCtx.Add(user);

                user.Name = "lisi";
                user.Height = 1.6f;
                user.Birthday = new DateTime(1982, 3, 1);
                user.Sex = false;
                sourceCtx.Add(user);

                user.Birthday = new DateTime(1990, 1, 1);
                sourceCtx.Update(user);

                Console.WriteLine("----测试 生成事务日志 成功！（此操作将写入事务日志信息到数据库中。）----");
            }
            
            Console.WriteLine("----准备复制数据---------");

            //初始化目标数据库
            TransLogDbContext targetCtx = new TransLogDbContext(targetDb);
            targetCtx.CheckTableExists<UserEntity>();
            Console.WriteLine("----目标数据库初始化检查成功-------");

            //从源读取数据
            sourceCtx.OnReadLog += SourceCtx_OnReadLog;
            int count= sourceCtx.ReadLog(5, list => {
                //复制数据到目标库
                foreach (var log in list)
                {
                    bool result = targetCtx.DataReplication(log);
                    if (!result)
                    {
                        Console.WriteLine("****复制数据遇到错误，日志ID:{0}，命令：{1}，错误原因：{2}", log.CommandID, log.CommandText, targetCtx.ErrorMessage);
                        return false;
                    }
                    else
                    {
                        Console.WriteLine("操作成功，日志ID:{0}，执行状态：{1}， 命令：{2}",log.CommandID,targetCtx.CurrentStatus,log.CommandText);
                    }
                }
                return true;

            });
            Console.WriteLine();
            Console.WriteLine("操作完成，从源数据库读取 {0}条记录。", count);
            Console.WriteLine();
            Console.WriteLine("测试全部完成，按回车键退出。");
            Console.ReadLine();
        }

        private static void SourceCtx_OnReadLog(object sender, ReadLogEventArgs e)
        {
            Console.WriteLine("----当前日志读取进度 ({0}/{1})={2}% -------------", e.ReadCount, e.AllCount, 100 * e.ReadCount / e.AllCount);
        }
    }
}
