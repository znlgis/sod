using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace PWMIS.EnterpriseFramework.Common
{
    /// <summary>
    /// DOS命令输出类
    /// </summary>
    public class DosCommandOutput
    {
        /// <summary>
        /// 执行DOS命令，返回DOS命令的输出
        /// </summary>
        /// <param name="dosCommand">dos命令</param>
        /// <returns>返回输出，如果发生异常，返回空字符串</returns>
        public static string Execute(string dosCommand)
        {
            return Execute(dosCommand, 60 * 1000);
        }
        /// <summary>
        /// 执行DOS命令，返回DOS命令的输出
        /// </summary>
        /// <param name="dosCommand">dos命令</param>
        /// <param name="milliseconds">等待命令执行的时间（单位：毫秒），如果设定为0，则无限等待</param>
        /// <returns>返回输出，如果发生异常，返回空字符串</returns>
        public static string Execute(string dosCommand, int milliseconds)
        {
            string output = "";     //输出字符串
            if (dosCommand != null && dosCommand != "")
            {
                Process process = new Process();     //创建进程对象
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "cmd.exe";      //设定需要执行的命令
                startInfo.Arguments = "/C " + dosCommand;   //设定参数，其中的“/C”表示执行完命令后马上退出
                startInfo.UseShellExecute = false;     //不使用系统外壳程序启动
                startInfo.RedirectStandardInput = false;   //不重定向输入
                startInfo.RedirectStandardOutput = true;   //重定向输出
                startInfo.CreateNoWindow = true;     //不创建窗口
                process.StartInfo = startInfo;
                try
                {
                    if (process.Start())       //开始进程
                    {
                        if (milliseconds == 0)
                            process.WaitForExit();     //这里无限等待进程结束
                        else
                            process.WaitForExit(milliseconds);  //这里等待进程结束，等待时间为指定的毫秒
                        output = process.StandardOutput.ReadToEnd();//读取进程的输出
                    }
                }
                catch
                {
                }
                finally
                {
                    if (process != null)
                        process.Close();
                }
            }
            return output;
        }

        /// <summary>
        /// 执行指定的进程命令（不使用命令外壳程序）并附件命令的参数
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static string ExecuteProcess(string command, string parameters, int milliseconds)
        {
            string output = "";     //输出字符串
            if (command != null && command != "")
            {
                Process process = new Process();     //创建进程对象
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = command;      //设定需要执行的命令
                startInfo.Arguments = parameters;   //设定参数
                startInfo.UseShellExecute = false;     //不使用系统外壳程序启动
                startInfo.RedirectStandardInput = false;   //不重定向输入
                startInfo.RedirectStandardOutput = true;   //重定向输出,这可能导致XCopy无法执行
                startInfo.CreateNoWindow = true;     //不创建窗口
                process.StartInfo = startInfo;
                try
                {
                    if (process.Start())       //开始进程
                    {
                        if (milliseconds == 0)
                            process.WaitForExit();     //这里无限等待进程结束
                        else
                            process.WaitForExit(milliseconds);  //这里等待进程结束，等待时间为指定的毫秒
                        output = process.StandardOutput.ReadToEnd();//读取进程的输出
                    }
                }
                catch
                {
                }
                finally
                {
                    if (process != null)
                        process.Close();
                }
            }
            return output;
        }

        /// <summary>
        /// 使用XCopy命令复制新文件，包括所有文件夹和子文件，如果目标文件存在将自动覆盖。
        /// </summary>
        /// <param name="sourceFolder">原目录</param>
        /// <param name="descFolder">目标目录，如果没有，将创建一个</param>
        /// <param name="excludeFile">要排除的文件信息说明文件，详细参见XCopy说明</param>
        /// <param name="afterData">要复制的新文件的日期，包括以及之后的文件都将被复制</param>
        /// <param name="waiteMilliseconds">执行本命令等待的时间，毫秒</param>
        /// <returns>执行是否成功</returns>
        public static bool CopyNewFile(string sourceFolder, string descFolder, string excludeFile, DateTime afterData, int waiteMilliseconds)
        {
            #region 路径有空格需处理
            sourceFolder = sourceFolder.Contains(" ") ? "\"" + sourceFolder + "\"" : sourceFolder;
            descFolder = descFolder.Contains(" ") ? "\"" + descFolder + "\"" : descFolder;
            #endregion

            string argument = sourceFolder + " " + descFolder + " /i /s /y ";
            if (!string.IsNullOrEmpty(excludeFile))
                argument += " /EXCLUDE:" + excludeFile;
            if (afterData != default(DateTime))
                argument += " /d:" + afterData.ToString("M-d-yyyy");
            return ProcessXCopy(argument, waiteMilliseconds);
        }

        /// <summary>
        /// 执行XCopy，如果命令执行包含任何错误，返回真，但也应该在命令执行完成后，检查目标目录是否存在。
        /// </summary>
        /// <param name="XCopyArguments">命令参数</param>
        /// <param name="milliseconds">等待时间，毫秒</param>
        /// <returns>是否成功</returns>
        public static bool ProcessXCopy(string XCopyArguments, int milliseconds)
        {
            //string XCopyArguments = "yourargumentshere";
            Process XCopyProcess = new Process();
            ProcessStartInfo XCopyStartInfo = new ProcessStartInfo();

            XCopyStartInfo.FileName = "CMD.exe ";

            //do not write error output to standard stream
            XCopyStartInfo.RedirectStandardError = false;
            //do not write output to Process.StandardOutput Stream
            XCopyStartInfo.RedirectStandardOutput = false;
            //do not read input from Process.StandardInput (i/e; the keyboard)
            XCopyStartInfo.RedirectStandardInput = false;

            XCopyStartInfo.UseShellExecute = false;
            //Dont show a command window
            XCopyStartInfo.CreateNoWindow = true;

            XCopyStartInfo.Arguments = "/D /c XCOPY " + XCopyArguments;

            XCopyProcess.EnableRaisingEvents = true;
            XCopyProcess.StartInfo = XCopyStartInfo;

            //start cmd.exe & the XCOPY process
            XCopyProcess.Start();


            if (milliseconds == 0)
                XCopyProcess.WaitForExit();     //这里无限等待进程结束
            else
                XCopyProcess.WaitForExit(milliseconds);

            int ExitCode = XCopyProcess.ExitCode;
            bool XCopySuccessful = true;

            //Now we need to see if the process was successful
            if (ExitCode > 0 & !XCopyProcess.HasExited)
            {
                XCopyProcess.Kill();
                XCopySuccessful = false;
            }

            //now clean up after ourselves
            XCopyProcess.Dispose();
            XCopyStartInfo = null;
            return XCopySuccessful;
        }
    }
}
