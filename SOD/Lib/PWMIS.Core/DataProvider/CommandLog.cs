/*
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
using System.IO ;
using System.Data ;
using PWMIS.Core;
using System.Collections.Generic;

namespace PWMIS.DataProvider.Data
{
	/// <summary>
    /// 命令对象日志2008.7.18 增加线程处理,2011.5.9 增加执行时间记录 2016.4.7 增加批量写入功能
	/// </summary>
	public class CommandLog : PWMIS.Common.ICommonLog
	{
        private   List<string> _logBuffer = new List<string>();
        private   DateTime _lastWrite=DateTime.Now ;
        private const int WriteTime = 30;//30秒写入一次
        private const int BufferCount = 20;//每20条写入一次

		//日志相关
        private static  string _dataLogFile;
        /// <summary>
        /// 获取或者设置日志文件的路径，可以带Web相对路径
        /// </summary>
        public static string DataLogFile
        {
            get {
                if (string.IsNullOrEmpty(_dataLogFile))
                {
                    _dataLogFile = System.Configuration.ConfigurationManager.AppSettings["DataLogFile"];
                    setDataLogFile();
                }
                return _dataLogFile;
            }
            set { 
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
        public  static  bool SaveCommandLog
        {
            get {
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
            get {
                if (_logExecutedTime ==-1)
                {
                    string temp = System.Configuration.ConfigurationManager.AppSettings["LogExecutedTime"];
                    if(string.IsNullOrEmpty (temp ))
                        _logExecutedTime =0;
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
            get {
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

		private static CommandLog _Instance;
        private static object lockObj = new object();
        private System.Diagnostics.Stopwatch watch = null;
        

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
		public void WriteLog(IDbCommand command,string who,out long elapsedMilliseconds)
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
                        RecordCommandLog(command, who);
                        WriteLog("Execueted Time(ms):" + elapsedMilliseconds + "\r\n", who);
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

		/// <summary>
		///写入日志消息
		/// </summary>
		/// <param name="msg">消息</param>
		/// <param name="who">发送者</param>
		public void WriteLog(string msg,string who)
		{
			if(SaveCommandLog)
				WriteLog ("//"+DateTime.Now.ToString ()+ " @"+who+" ："+msg+"\r\n");
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
			StreamReader sr= File.OpenText(DataLogFile );
			string text=sr.ReadToEnd ();
			sr.Close();
			return text;
		}

		/// <summary>
		/// 记录命令信息
		/// </summary>
		/// <param name="command">命令对象</param>
        /// <param name="who">执行人</param>
		private void RecordCommandLog(IDbCommand command,string who)
		{
			string temp="//"+DateTime.Now.ToString ()+ " @"+who+" 执行命令：\r\nSQL=\""+command.CommandText+"\"\r\n//命令类型："+command.CommandType.ToString ();
			if(command.Transaction !=null)
				temp=temp.Replace ("执行命令","执行事务");
			WriteLog(temp);
			if(command.Parameters.Count >0)
			{
				WriteLog("//"+command.Parameters.Count+"个命令参数：");
				for(int i=0;i<command.Parameters.Count ;i++)
				{
					IDataParameter p=(IDataParameter)command.Parameters[i];
					WriteLog ("Parameter[\""+p.ParameterName+"\"]\t=\t\""+Convert.ToString ( p.Value)+"\"  \t\t\t//DbType=" +p.DbType.ToString ());
				}
			}
			

		}

		/// <summary>
		/// 批量写入日志
		/// </summary>
		/// <param name="log"></param>
		private void WriteLog(string log)
		{
            if (!string.IsNullOrEmpty(log))
                _logBuffer.Add(log);

            if (_logBuffer.Count > 0 && (DateTime.Now.Subtract(_lastWrite).TotalSeconds > WriteTime || _logBuffer.Count > LogBufferCount))
            {
                List<string> tempList = _logBuffer;
                _logBuffer = new List<string>();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (string item in tempList)
                {
                    sb.Append(item);
                    sb.Append("\r\n");
                }
                string writeText = sb.ToString ();

                //edit at 2012.10.17 改成无锁异步写如日志文件
                using (FileStream fs = new FileStream(DataLogFile, FileMode.Append, FileAccess.Write, FileShare.Write, 2048, FileOptions.Asynchronous))
                {
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(writeText );
                    IAsyncResult writeResult = fs.BeginWrite(buffer, 0, buffer.Length,
                        (asyncResult) =>
                        {
                            FileStream fStream = (FileStream)asyncResult.AsyncState;
                            fStream.EndWrite(asyncResult);
                            //fs.Close();//这里加了会报错
                        },
                        fs);
                    //fs.EndWrite(writeResult);//这种方法异步起不到效果
                    fs.Flush();
                    //fs.Close();//可以不用加
                }
                _lastWrite = DateTime.Now;
            }
		}

        /// <summary>
        /// 将日志全部写入
        /// </summary>
        public void Dispose()
        {
            _lastWrite = DateTime.Now.AddMinutes(-10);
            WriteLog(null);
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
