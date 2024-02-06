﻿/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap 
 * ========================================================================
 * 使用方法：
 * 在使用该对象的时候，应该在应用程序配置文件的  <appSettings> 增加下面三个配置项：
 * 
    <add key="SaveCommandLog" value="True"/>
 * 如果设置成 True,不区分大小写，才可以记录日志，所以这是一个日志开关；
 * 
    <add key="DataLogFile" value="~\SqlLog.txt"/>
 * 日志路径，支持Web日志路径格式，如上表示在当前Web站点根目录（不是bin）下面的日志文件SqlLog.txt；
 * 
    <add key="LogExecutedTime" value ="0"/>
 * 需要记录的时间，如果该值等于0会记录所有查询，否则只记录大于该时间的查询，单位毫秒；
 * 
 *  <add key="LogBufferCount" value ="0"/>
 * 日志信息缓存的数量，如果该值等于0会立即写入日志文件，默认缓存20条信息；注意一次查询可能会写入多条日志信息。
 * 
 * 作者：邓太华     时间：2008-10-12
 * 版本：V5.5
 * 
 * 修改者：         时间：2010-4-13                
 * 修改说明：       增加适时查看执行的SQL属性 CommandText
 * 
 * 修改者：         时间：2016-7-7                
 * 修改说明：       批量写入日志并增加配置缓存数量
 * ========================================================================
*/
using System;
using System.IO;
using System.Data;
using PWMIS.Core;
using System.Collections.Generic;
using System.Diagnostics;

namespace PWMIS.DataProvider.Data
{
    /// <summary>
    /// 命令对象日志2008.7.18 增加线程处理,2011.5.9 增加执行时间记录 2016.4.7 增加批量写入功能
    /// </summary>
    public class CommandLog
    {
        private const int BufferCount = 20;//每20条写入一次
        private static CommandLog _Instance;
        private static object lockObj = new object();
        private System.Diagnostics.Stopwatch watch = null;
        private bool needLog = true;

        private PWMIS.Common.ICommonLog _logWriter;
        /// <summary>
        /// 获取或者设置日志写入对象
        /// </summary>
        public PWMIS.Common.ICommonLog LogWriter
        {
            get
            {
                if (_logWriter == null)
                {
                    InnerLogWriter logger = new InnerLogWriter();
                    logger.DataLogFile = DataLogFile;
                    logger.SaveCommandLog = SaveCommandLog;
                    logger.LogBufferCount = LogBufferCount;

                    _logWriter = logger;
                }

                return _logWriter;
            }
            set
            {
                _logWriter = value;
            }
        }

        #region 日志配置相关属性设置
        //日志相关
        private static string _dataLogFile;
        /// <summary>
        /// 获取或者设置日志文件的路径，可以带Web相对路径
        /// </summary>
        public static string DataLogFile
        {
            get
            {
                if (string.IsNullOrEmpty(_dataLogFile))
                {
                    _dataLogFile = System.Configuration.ConfigurationManager.AppSettings["DataLogFile"];
                    setDataLogFile();
                }
                return _dataLogFile;
            }
            set
            {
                _dataLogFile = value;
                setDataLogFile();
            }
        }

        private static void setDataLogFile()
        {
            if (!string.IsNullOrEmpty(_dataLogFile))
            {
                CommonUtil.ReplaceWebRootPath(ref _dataLogFile);
                string temp = System.Configuration.ConfigurationManager.AppSettings["SaveCommandLog"];
                if (temp != null)
                    _saveCommandLog = temp.ToUpper() == "TRUE";
            }
        }

        private static bool _saveCommandLog;
        /// <summary>
        /// 是否记录日志文件
        /// </summary>
        public static bool SaveCommandLog
        {
            get
            {
                string temp = DataLogFile;//必须先调用下，以计算_saveCommandLog
                return _saveCommandLog;
            }
            set { _saveCommandLog = value; }
        }

        private static long _logExecutedTime = -1;
        /// <summary>
        /// 需要记录的时间，只有该值等于0会记录所有查询，否则只记录大于该时间的查询。单位毫秒。
        /// </summary>
        public static long LogExecutedTime
        {
            get
            {
                if (_logExecutedTime == -1)
                {
                    string temp = System.Configuration.ConfigurationManager.AppSettings["LogExecutedTime"];
                    if (string.IsNullOrEmpty(temp))
                        _logExecutedTime = 0;
                    else
                        long.TryParse(temp, out _logExecutedTime);
                }
                return _logExecutedTime;
            }
            set { _logExecutedTime = value; }
        }

        private static int _logBufferCount = -1;
        /// <summary>
        /// 日志信息的缓存数量，默认是20条
        /// </summary>
        public static int LogBufferCount
        {
            get
            {
                if (_logBufferCount == -1)
                {
                    string temp = System.Configuration.ConfigurationManager.AppSettings["LogBufferCount"];
                    if (string.IsNullOrEmpty(temp))
                    {
                        _logBufferCount = BufferCount;
                    }
                    else
                    {
                        if (!int.TryParse(temp, out _logBufferCount))
                            _logBufferCount = BufferCount;
                    }
                }
                return _logBufferCount;
            }
            set
            {
                _logBufferCount = value;
            }
        }

        #endregion


        /// <summary>
        /// 获取单例对象(不开启执行时间记录)
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
        /// 默认构造函数
        /// </summary>
        public CommandLog()
        {

        }

        /// <summary>
        /// 是否开启执行时间记录
        /// </summary>
        /// <param name="startStopwatch"></param>
        public CommandLog(bool startStopwatch)
        {
            if (startStopwatch)
            {
                watch = new System.Diagnostics.Stopwatch();
                watch.Start();
            }
        }

        /// <summary>
        /// 重新开始记录执行时间
        /// </summary>
        public void ReSet()
        {
            if (watch != null)
            {
                watch.Reset();
                watch.Start();
            }
        }

        /// <summary>
        /// 获取当前执行的实际SQL语句
        /// </summary>
        public string CommandText
        {
            private set;
            get;
        }

        /// <summary>
        /// 写命令日志和执行时间（如果开启的话）
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="who">调用命令的源名称</param>
        /// <param name="elapsedMilliseconds">执行时间</param>
        public void WriteLog(IDbCommand command, string who, out long elapsedMilliseconds)
        {
            CommandText = command.CommandText;
            elapsedMilliseconds = 0;
            if (watch != null)
            {
                elapsedMilliseconds = watch.ElapsedMilliseconds;
                if (SaveCommandLog)
                {
                    if ((LogExecutedTime > 0 && elapsedMilliseconds > LogExecutedTime) || LogExecutedTime == 0)
                    {
                        this.needLog = true;
                        RecordCommandLog(command, who);
                        this.LogWriter.WriteLog("Execueted Time(ms):" + elapsedMilliseconds + "\r\n", who);
                    }
                    else
                    {
                        this.needLog = false;
                    }
                }
                watch.Stop();
            }
            else
            {
                if (SaveCommandLog)
                    RecordCommandLog(command, who);
            }
        }

        public void WriteLog(string msg, string who)
        {
            if (SaveCommandLog && this.needLog)
                this.LogWriter.WriteLog(msg, who);
        }

        /// <summary>
        /// 写错误日志，将使用 DataLogFile 配置键的文件名写文件，不受SaveCommandLog 影响，除非 DataLogFile 未设置或为空。
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
        /// 获取日志文本
        /// </summary>
        /// <returns>日志文本</returns>
        public string GetLog()
        {
            StreamReader sr = File.OpenText(DataLogFile);
            string text = sr.ReadToEnd();
            sr.Close();
            return text;
        }

        /// <summary>
        /// 记录命令信息
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="who">执行人</param>
        private void RecordCommandLog(IDbCommand command, string who)
        {
            string temp = "//" + DateTime.Now.ToString() + " @" + who + " 执行命令：\r\nSQL=\"" + command.CommandText + "\"\r\n//命令类型：" + command.CommandType.ToString();
            if (command.Transaction != null)
                temp = temp.Replace("执行命令", "执行事务");
            this.LogWriter.WriteLog(temp);
            if (command.Parameters.Count > 0)
            {
                this.LogWriter.WriteLog("//" + command.Parameters.Count + "个命令参数：");
                for (int i = 0; i < command.Parameters.Count; i++)
                {
                    IDataParameter p = (IDataParameter)command.Parameters[i];
                    this.LogWriter.WriteLog("Parameter[\"" + p.ParameterName + "\"]\t=\t\"" + Convert.ToString(p.Value) + "\"  \t\t\t//DbType=" + p.DbType.ToString());
                }
            }
        }


        /// <summary>
        /// 将日志全部写入
        /// </summary>
        public void Flush()
        {
            this.LogWriter.Dispose();
        }
    }

    /// <summary>
    /// 日志写入器，每20秒或者每满20条写入一次
    /// </summary>
    class InnerLogWriter : PWMIS.Common.ICommonLog
    {
        public InnerLogWriter()
        {
            this.LogBufferCount = 20;
            this._logBuffer.Add("--SQLLog (Thread ID " + System.Threading.Thread.CurrentThread.ManagedThreadId + ") Init----");
        }
        public string DataLogFile { get; set; }
        public bool SaveCommandLog { get; set; }
        /// <summary>
        /// 默认每满20条写入
        /// </summary>
        public int LogBufferCount { get; set; }

        private List<string> _logBuffer = new List<string>();
        private DateTime _lastWrite = DateTime.Now;
        private const int WriteTime = 20;//20秒写入一次
        private static object sync_obj = new object();

        /// <summary>
        ///写入日志消息
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="who">发送者</param>
        public void WriteLog(string msg, string who)
        {
            if (SaveCommandLog)
                WriteLog("//" + DateTime.Now.ToString() + " @" + who + " ：" + msg);
        }

        /// <summary>
        /// 批量写入日志
        /// </summary>
        /// <param name="log"></param>
        public void WriteLog(string log)
        {
            if (string.IsNullOrEmpty(DataLogFile))
                return;
            if (string.IsNullOrEmpty(log))
                return;
            lock (sync_obj)
            {
                _logBuffer.Add(log);

                if (_logBuffer.Count > 0 && (DateTime.Now.Subtract(_lastWrite).TotalSeconds > WriteTime || _logBuffer.Count > LogBufferCount))
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendLine("--SQLLog (Thread ID " + System.Threading.Thread.CurrentThread.ManagedThreadId + ") Write Buffer:----");
                    bool flag = false;
                    foreach (string item in _logBuffer)
                    {
                        if (!item.StartsWith("--SQLLog"))
                            flag = true;
                        sb.AppendLine(item);
                    }
                    _logBuffer.Clear();
                    if (!flag)
                    {
                        return;//没有实质性的日志，不写入日志文件
                    }
                    string writeText = sb.ToString();
                    try
                    {
                        //日志文件超过5M，备份日志文件
                        if (!System.IO.File.Exists(DataLogFile))
                            File.AppendAllText(DataLogFile, "--SOD Log Ver 5.6--\r\n",System.Text.Encoding.UTF8);
                        var fileInfo = new FileInfo(DataLogFile);
                        if (fileInfo.Length > 5 * 1024 * 1024)
                        {
                            string bakFile = string.Format("{0}_{1}.{2}",
                               System.IO.Path.GetFileNameWithoutExtension(DataLogFile),
                               DateTime.Now.ToString("yyyyMMddHHmmss"),
                               System.IO.Path.GetExtension(DataLogFile));
                            string bakFilePath = System.IO.Path.Combine( System.IO.Path.GetDirectoryName(DataLogFile),bakFile);
                            System.IO.File.Move(DataLogFile, bakFilePath);
                        }
                       
                        //edit at 2012.10.17 改成无锁异步写如日志文件，2017.9.30日修改，增加说明
                        //不能将fs 放到Using下面，会导致句柄无效
                        FileStream fs = new FileStream(DataLogFile, FileMode.Append, FileAccess.Write,
                            FileShare.Write, 2048, FileOptions.Asynchronous);

                        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(writeText);
                        IAsyncResult writeResult = fs.BeginWrite(buffer, 0, buffer.Length,
                            (asyncResult) =>
                            {
                                bool isOK = false;
                                FileStream fStream = (FileStream)asyncResult.AsyncState;
                                try
                                {
                                    fStream.EndWrite(asyncResult);
                                    fStream.Flush();
                                    isOK = true;
                                }
                                catch (Exception ex2)
                                {
                                    writeText = "*** 异步结束 写日志文件异常，错误原因：" + ex2.Message + "\r\n 原始日志信息：" + writeText + " \r\n***\r\n";
                                }
                                finally
                                {
                                    fStream.Close();
                                }
                                if(!isOK)
                                    ReTryWriteLog(DataLogFile, writeText);
                            },
                            fs);
                        //fs.EndWrite(writeResult);//在这里调用异步起不到效果
                        //fs.Flush();//异步写入时，刷新缓存可能会导致写入两条数据到文件


                    }
                    catch (Exception ex)
                    {
                        writeText = "*** 异步开始写日志文件异常，错误原因：" + ex.Message + "\r\n 原始日志信息：" + writeText + " \r\n***\r\n";
                        ReTryWriteLog(DataLogFile, writeText);
                    }

                    _lastWrite = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// 重试同步写日志文件，与SOD for .NET 5之前的版本不同，如果还失败，将忽略而不是写入系统日志
        /// </summary>
        /// <param name="logFile"></param>
        /// <param name="logText"></param>
        private void ReTryWriteLog(string logFile, string logText)
        {
            try
            {
                File.AppendAllText(logFile, logText);
            }
            catch(Exception ex)
            {
                string errLogText = "【SOD】重试写日志失败，错误原因:"+ex.Message+"\r\n源日志信息："+logText;
                System.Diagnostics.Debug.WriteLine(errLogText);
                /*
                EventLog eventLog = new EventLog();
                eventLog.Source = "Application";
                eventLog.WriteEntry(errLogText, EventLogEntryType.Error);
                */
            }
        }

        public void Dispose()
        {
            Flush();
        }

        public void Flush()
        {
            _lastWrite = DateTime.Now.AddMinutes(-10);
            WriteLog("--SQLLog (Thread ID " + System.Threading.Thread.CurrentThread.ManagedThreadId + ") Flush----\r\n");
        }

    }


    public class BufferTextWriter : IDisposable
    {

        private bool disposed = false;
        private static BufferTextWriter _Instance;
        private static object lockObj = new object();

        public static BufferTextWriter Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (lockObj)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new BufferTextWriter();
                        }
                    }
                }
                return _Instance;
            }
        }

        public void AddText(string text)
        {

        }

        public void WriteBufferText()
        {

        }

        #region Dispose模式实现

        public void Dispose()
        {
            Dispose(true);
        }

        public void Close()
        {
            Dispose(true);
        }

        ~BufferTextWriter()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Console.WriteLine("调用引用对象的Dispose()方法");
                }
                Console.WriteLine("释放类本身的非托管资源");
                disposed = true;
                if (disposing)
                {
                    GC.SuppressFinalize(this);
                }
            }

        }
        #endregion

    }


}
