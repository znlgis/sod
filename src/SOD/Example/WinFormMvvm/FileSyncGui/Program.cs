
using FileSyncGui.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileSyncGui
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("文件同步程序");
            string strIsAutoSync="";
            string strSyncInterval="";
            
            string runStyle = ConfigurationHelper.Instance.GetAppSetting("RunStyle");
            if ( (string.IsNullOrEmpty(runStyle) || runStyle.ToLower() == "cmd") && args.Length == 0)
            {
                MessageBox.Show("启动参数不正确，可能缺少命令行参数！","文件同步程序");
                MessageBox.Show(CmdLineHelpMsg(),"文件同步程序");
                return;
            }
            if (args.Length >0)
            {
                foreach(string item in args)
                {
                    string[] arr = item.Split('=');
                    if (arr.Length == 2)
                    {
                        switch (arr[0].Trim())
                        {
                            case "/n":
                            case "ServerIP":
                                ServerIP = arr[1].Trim();
                                break;
                            case "/p":
                            case "ServerPort":
                                ServerPort = arr[1].Trim();
                                break;
                            case "/a":
                            case "IsAutoSync":
                                strIsAutoSync = arr[1].Trim();
                                break;
                            case "/i":
                            case "SyncInterval":
                                strSyncInterval = arr[1].Trim();
                                break;
                            default:
                                MessageBox.Show("命令行参数不正确:"+ arr[0].Trim());
                                MessageBox.Show(CmdLineHelpMsg(), "文件同步程序");
                                return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("命令行参数不正确:" + item);
                        MessageBox.Show(CmdLineHelpMsg(), "文件同步程序");
                        return;
                    }
                }
              
                strIsAutoSync = "True";
                runStyle = "cmd";
            }
            else
            {
                ServerIP = ConfigurationHelper.Instance.GetAppSetting("ServerIP");
                ServerPort = ConfigurationHelper.Instance.GetAppSetting("ServerPort");
                strIsAutoSync = ConfigurationHelper.Instance.GetAppSetting("IsAutoSync");
                strSyncInterval = ConfigurationHelper.Instance.GetAppSetting("SyncInterval");
            }
            if (string.IsNullOrEmpty(ServerIP) || string.IsNullOrEmpty(ServerPort))
            {
                MessageBox.Show("服务器的地址参数不正确！", "文件同步程序");
                MessageBox.Show(CmdLineHelpMsg());
                return;
            }
            if (strIsAutoSync.ToUpper() == "TRUE")
                IsAutoSync = true;
            
            int temp = 5;
            int.TryParse(strSyncInterval, out temp);
            SyncInterval = temp;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm form = new MainForm();
            if (runStyle == "cmd")
            {
                form.WindowState = FormWindowState.Minimized;
                form.ShowInTaskbar = false;
            }
            Application.Run(form);
        }

        private static string CmdLineHelpMsg()
        {
            return @"
命令行格式：
FileSyncGui.exe [/n=Value1] [/p=Value2] [/a=Value3] [/i=Value4]
参数说明：
/n 或者 ServerIP：服务主机的IP地址或者计算机名字
/p 或者 ServerPort：服务监听的端口号
/a 或者 IsAutoSync：是否自动启动，通过命令行启动本程序，始终设置该参数为“是”
/i 或者 SyncInterval：同步间隔时间，单位分钟，默认值是5分钟
";
        }

        /// <summary>
        /// 服务的主机名或者IP
        /// </summary>
        public static string ServerIP { get; private set; }
        /// <summary>
        /// 服务监听的端口号
        /// </summary>
        public static string ServerPort { get; private set; }
        /// <summary>
        /// 是否自动启动，默认为False
        /// </summary>
        public static bool IsAutoSync { get; private set; }
        /// <summary>
        /// 同步间隔，单位分钟，默认为5分钟
        /// </summary>
        public static int SyncInterval { get; private set; }
    }
}
