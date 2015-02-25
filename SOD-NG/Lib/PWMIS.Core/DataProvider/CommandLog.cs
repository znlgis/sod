/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap 
 * ========================================================================
 * 该类的作用
 * 
 * 作者：邓太华     时间：2008-10-12
 * 版本：V3.0
 * 
 * 修改者：         时间：2010-4-13                
 * 修改说明：       增加适时查看执行的SQL属性 CommandText
 * ========================================================================
*/

using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using PWMIS.Core;

namespace PWMIS.DataProvider.Data
{
    /// <summary>
    ///     命令对象日志2008.7.18 增加线程处理,2011.5.9 增加执行时间记录
    /// </summary>
    public class CommandLog
    {
        //日志相关
        private static string _dataLogFile;
        private static bool _saveCommandLog;
        private static long _logExecutedTime = -1;
        private static CommandLog _Instance;
        private static readonly object lockObj = new object();
        private readonly Stopwatch watch;

        /// <summary>
        ///     默认构造函数
        /// </summary>
        public CommandLog()
        {
        }

        /// <summary>
        ///     是否开启执行时间记录
        /// </summary>
        /// <param name="startStopwatch"></param>
        public CommandLog(bool startStopwatch)
        {
            if (startStopwatch)
            {
                watch = new Stopwatch();
                watch.Start();
            }
        }

        /// <summary>
        ///     获取或者设置日志文件的路径，可以带Web相对路径
        /// </summary>
        public static string DataLogFile
        {
            get
            {
                if (string.IsNullOrEmpty(_dataLogFile))
                {
                    _dataLogFile = ConfigurationSettings.AppSettings["DataLogFile"];
                    if (!string.IsNullOrEmpty(_dataLogFile))
                    {
                        CommonUtil.ReplaceWebRootPath(ref _dataLogFile);
                        var temp = ConfigurationSettings.AppSettings["SaveCommandLog"];
                        if (temp != null)
                            _saveCommandLog = temp.ToUpper() == "TRUE";
                    }
                }
                return _dataLogFile;
            }
            set { _dataLogFile = value; }
        }

        /// <summary>
        ///     是否记录日志文件
        /// </summary>
        public static bool SaveCommandLog
        {
            get
            {
                var temp = DataLogFile; //必须先调用下，以计算_saveCommandLog
                return _saveCommandLog;
            }
            set { _saveCommandLog = value; }
        }

        /// <summary>
        ///     需要记录的时间，只有该值等于0会记录所有查询，否则只记录大于该时间的查询。单位毫秒。
        /// </summary>
        public static long LogExecutedTime
        {
            get
            {
                if (_logExecutedTime == -1)
                {
                    var temp = ConfigurationSettings.AppSettings["LogExecutedTime"];
                    if (string.IsNullOrEmpty(temp))
                        _logExecutedTime = 0;
                    else
                        long.TryParse(temp, out _logExecutedTime);
                }
                return _logExecutedTime;
            }
            set { _logExecutedTime = value; }
        }

        /// <summary>
        ///     获取单例对象
        /// </summary>
        public static CommandLog Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (lockObj)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new CommandLog();
                        }
                    }
                }
                return _Instance;
            }
        }

        /// <summary>
        ///     获取当前执行的实际SQL语句
        /// </summary>
        public string CommandText { private set; get; }

        /// <summary>
        ///     重新开始记录执行时间
        /// </summary>
        public void ReSet()
        {
            if (watch != null)
                watch.Reset();
        }

        /// <summary>
        ///     写命令日志和执行时间（如果开启的话）
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="who">调用命令的源名称</param>
        /// <param name="elapsedMilliseconds">执行时间</param>
        public void WriteLog(IDbCommand command, string who, out long elapsedMilliseconds)
        {
            CommandText = command.CommandText;
            elapsedMilliseconds = 0;
            if (SaveCommandLog)
            {
                if (watch != null)
                {
                    elapsedMilliseconds = watch.ElapsedMilliseconds;
                    if ((LogExecutedTime > 0 && elapsedMilliseconds > LogExecutedTime) || LogExecutedTime == 0)
                    {
                        RecordCommandLog(command, who);
                        WriteLog("Execueted Time(ms):" + elapsedMilliseconds + "\r\n", who);
                    }
                }
                else
                {
                    RecordCommandLog(command, who);
                }
            }
        }

        /// <summary>
        ///     写入日志消息
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="who">发送者</param>
        public void WriteLog(string msg, string who)
        {
            if (SaveCommandLog)
                WriteLog("//" + DateTime.Now + " @" + who + " ：" + msg + "\r\n");
        }

        /// <summary>
        ///     写错误日志，将使用 DataLogFile 配置键的文件名写文件，不受SaveCommandLog 影响，除非 DataLogFile 未设置或为空。
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="errmsg">调用命令的源名称</param>
        public void WriteErrLog(IDbCommand command, string errmsg)
        {
            if (!string.IsNullOrEmpty(DataLogFile))
            {
                errmsg += ":Error";
                RecordCommandLog(command, errmsg);
            }
        }

        /// <summary>
        ///     获取日志文本
        /// </summary>
        /// <returns>日志文本</returns>
        public string GetLog()
        {
            var sr = File.OpenText(DataLogFile);
            var text = sr.ReadToEnd();
            sr.Close();
            return text;
        }

        /// <summary>
        ///     记录命令信息
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="who">执行人</param>
        private void RecordCommandLog(IDbCommand command, string who)
        {
            var temp = "//" + DateTime.Now + " @" + who + " 执行命令：\r\nSQL=\"" + command.CommandText + "\"\r\n//命令类型：" +
                       command.CommandType;
            if (command.Transaction != null)
                temp = temp.Replace("执行命令", "执行事务");
            WriteLog(temp);
            if (command.Parameters.Count > 0)
            {
                WriteLog("//" + command.Parameters.Count + "个命令参数：");
                for (var i = 0; i < command.Parameters.Count; i++)
                {
                    var p = (IDataParameter) command.Parameters[i];
                    WriteLog("Parameter[\"" + p.ParameterName + "\"]\t=\t\"" + Convert.ToString(p.Value) +
                             "\"  \t\t\t//DbType=" + p.DbType);
                }
            }
        }

        /// <summary>
        ///     写入日志
        /// </summary>
        /// <param name="log"></param>
        private void WriteLog(string log)
        {
            //edit at 2012.10.17 改成无锁异步写如日志文件
            using (
                var fs = new FileStream(DataLogFile, FileMode.Append, FileAccess.Write, FileShare.Write, 1024,
                    FileOptions.Asynchronous))
            {
                var buffer = Encoding.UTF8.GetBytes(log + "\r\n");
                var writeResult = fs.BeginWrite(buffer, 0, buffer.Length,
                    asyncResult =>
                    {
                        var fStream = (FileStream) asyncResult.AsyncState;
                        fStream.EndWrite(asyncResult);
                        //fs.Close();//这里加了会报错
                    },
                    fs);
                //fs.EndWrite(writeResult);//这种方法异步起不到效果
                fs.Flush();
                //fs.Close();//可以不用加
            }

            //lock (lockObj)
            //{

            //    //using (StreamWriter sw = File.AppendText(DataLogFile))
            //    //{

            //    //    sw.WriteLine(log);
            //    //    sw.Flush();
            //    //    sw.Close();
            //    //}
            //}
        }
    }
}