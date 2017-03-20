using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using PWMIS.EnterpriseFramework.Common;
using PWMIS.EnterpriseFramework.Common.Encrypt;
using PWMIS.EnterpriseFramework.Service.Client.Model;


namespace TranstarAuction.Service.Runtime
{
    /// <summary>
    /// 管理端服务类
    /// </summary>
    public class ManageService:ServiceBase
    {
        /// <summary>
        /// 连接服务,仅允许内网IP连接本服务
        /// </summary>
        /// <param name="identityName">标识名称，例如用户的域登陆名称</param>
        /// <returns>是否允许连接管理服务</returns>
        public bool Connect(string identityName)
        {
            string logFileName = base.CurrentContext.Host.LogDirectory + "ManageLog.txt";
            WriteLogFile(logFileName, identityName+" 连接服务器；");
            string ip = base.CurrentContext.Request.ClientIP;
            IpUtility ipu = new IpUtility();
            if (!ipu.IsInner(ip))
            {
                WriteLogFile(logFileName, ip+" [拒绝]该IP连接服务器；");
                return false;
            }
            WriteLogFile(logFileName, ip + " [允许]该IP连接服务器；");
            return true;
        }
        /// <summary>
        /// 是否是管理员，只有管理密码正确，才是管理员
        /// </summary>
        /// <param name="password">管理密码，默认为空密码</param>
        /// <returns></returns>
        public bool IsManager(string password)
        {
            string localPwd = getLocalPassword();
            if (localPwd == "")
                return true;

            if (EncryptHelper.MD5Encrypt(password) == localPwd)
                return true;
            else
                return false;

        }
        /// <summary>
        /// 修改管理密码，只有旧密码正确，且新密码不为空，则可修改成功
        /// </summary>
        /// <param name="oldPwd">旧密码</param>
        /// <param name="newPwd">新密码</param>
        /// <returns></returns>
        public bool ChangePassword(string oldPwd, string newPwd)
        {
            if (string.IsNullOrEmpty(newPwd))
                return false;

            string localPwd = getLocalPassword();
            if ((localPwd=="" && oldPwd=="") ||  EncryptHelper.MD5Encrypt(oldPwd) == localPwd)
            {
                localPwd = EncryptHelper.MD5Encrypt(newPwd);
                string fullFileName = getPwdFileName();
                string folder= Path.GetDirectoryName(fullFileName);
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                File.WriteAllText(fullFileName, localPwd);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取所有的日志文件信息
        /// </summary>
        /// <returns></returns>
        public List<ServiceFileInfo> GetAllLogFiles()
        {
            List<ServiceFileInfo> result = new List<ServiceFileInfo>();
            string _folder = base.CurrentContext.Host.LogDirectory;
            foreach (string file in Directory.GetFiles(_folder))
            {
                FileInfo info = new FileInfo(file);
                result.Add(new ServiceFileInfo() {  Name=info.Name, LastWriteTime=info.LastWriteTime, Length=info.Length});
            }
            return result;
        }

        /// <summary>
        /// 获取当前系统目录下的文件信息(子目录以dir 表示)
        /// </summary>
        /// <param name="directory">相对目录</param>
        /// <returns>文件信息</returns>
        public List<ServiceFileInfo> GetDirectoryFiles(string directory)
        {
            if (directory.StartsWith("<DIR>"))
                directory = directory.Substring(5);
            if (directory.StartsWith(".\\") || directory.StartsWith("./"))
                directory=directory.Substring(2);
            else if (directory.StartsWith("..\\") || directory.StartsWith("../"))
                 directory=directory.Substring(3);
            else if (directory.StartsWith("\\") || directory.StartsWith("/"))
                directory = directory.Substring(1);

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(baseDir, directory);
            List<ServiceFileInfo> result = new List<ServiceFileInfo>();
            foreach (string folder in Directory.GetDirectories(path))
            {
                DirectoryInfo di = new DirectoryInfo(folder);
                result.Add(new ServiceFileInfo() { Name="<DIR>"+folder.Replace(baseDir,""),  LastWriteTime=di.LastWriteTime });
            }

            foreach (string file in Directory.GetFiles(path))
            {
                FileInfo info = new FileInfo(file);
                result.Add(new ServiceFileInfo() { Name = info.Name, LastWriteTime = info.LastWriteTime, Length = info.Length });
            }
            return result;
        }

        /// <summary>
        /// 获取日志文件的总行数
        /// </summary>
        /// <param name="fileName">日志文件名</param>
        /// <returns></returns>
        public int GetLogFileRowsCount(string fileName)
        {
            int result = 0;
            string _folder = base.CurrentContext.Host.LogDirectory;
            fileName = Path.Combine(_folder, fileName);
            System.Text.StringBuilder sb = new StringBuilder();
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader reader = new StreamReader(fs, true);
            while (!reader.EndOfStream)
            {
                reader.ReadLine();
                result++;
            }
            reader.Close();
            fs.Close();
            return result;
        }

        /// <summary>
        /// 获取日志文件的文本
        /// </summary>
        /// <param name="fileName">日志文件名</param>
        /// <param name="beginRow">起始行</param>
        /// <param name="endRow">结束行</param>
        /// <returns>文本串</returns>
        public string GetLogFileText(string fileName,int beginRow,int endRow)
        {
            string _folder = base.CurrentContext.Host.LogDirectory;
            fileName = Path.Combine(_folder, fileName);

            if (beginRow > endRow)
            {
                int temp = endRow;
                endRow = beginRow;
                beginRow = temp;
            }
            if (beginRow <= 0)
                beginRow = 0;
            if (endRow < beginRow)
                endRow = beginRow + 1000;

            System.Text.StringBuilder sb = new StringBuilder();
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader reader = new StreamReader (fs,true);
           
            for (int i = 0; i <= endRow; i++)
            {
                if (reader.EndOfStream)
                    break;

                if (i >= beginRow)
                {
                    sb.Append(reader.ReadLine());
                    sb.Append("\r\n");
                }
                else
                {
                    reader.ReadLine();
                }
            }
            reader.Close();
            fs.Close();
            return sb.ToString();
        }

        /// <summary>
        /// 读取文件返回字节流
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns></returns>
        public byte[] CurReadFile(string fileName)
        {
            string _folder = base.CurrentContext.Host.LogDirectory;
            if(!fileName.StartsWith("."))
                fileName = Path.Combine(_folder, fileName);
            FileStream pFileStream = null;
            byte[] pReadByte = new byte[0];
            try
            {
                pFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader r = new BinaryReader(pFileStream);
                r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开
                pReadByte = r.ReadBytes((int)r.BaseStream.Length);
                return pReadByte;
            }
            catch
            {
                return pReadByte;
            }
            finally
            {
                if (pFileStream != null)
                    pFileStream.Close();
            }
        }

        /// <summary>
        /// 读取大文件,需要回调客户端方法，一边读一边写
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>文件最后读的位置</returns>
        public long ReadLargeFile(string fileName)
        {
            string _folder = base.CurrentContext.Host.LogDirectory;
            if (!fileName.StartsWith("."))
                fileName = Path.Combine(_folder, fileName);
            FileStream pFileStream = null;
            byte[] pReadByte = new byte[0];
            const int maxReadSize = 1024 * 1024;//每次读1M
            try
            {
                pFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read,FileShare.ReadWrite);
                BinaryReader r = new BinaryReader(pFileStream);
                long length = r.BaseStream.Length;
                long position = 0;
                r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开
                //告诉客户端当前文件的长度
                bool flag=  base.CurrentContext.PreCallBackFunction<long, bool>(length);

                while (position < length)
                {
                    byte[] buffer = r.ReadBytes(maxReadSize);
                    //将数据写回到客户端
                    int clientWriteSize= base.CurrentContext.CallBackFunction<byte[], int>(buffer);
                    if (buffer.Length < maxReadSize)
                    { 
                        //已经阅读到最后一块数据
                        position += buffer.Length;
                        break;
                    }
                    position += maxReadSize;
                }

                return position;
            }
            catch
            {
                return 0;
            }
            finally
            {
                if (pFileStream != null)
                    pFileStream.Close();
            }
        }      

        string getLocalPassword()
        {
            string fullFileName = getPwdFileName();
            //第一次使用，没有密钥文件，默认为空密码
            if (File.Exists(fullFileName))
            {
                string text = File.ReadAllText(fullFileName);
                return text;
            }
            return "";
        }
        string getPwdFileName()
        {
            string fileName = "En" + base.CurrentContext.Host.RegServerPort;
            string fullFileName = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "ManageService", fileName);
            return fullFileName;
        }

        void WriteLogFile(string fileName, string logMsg)
        {
            try
            {
                string text = string.Format("\r\n[{0}] {1}\r\n", DateTime.Now.ToString(), logMsg);
                System.IO.File.AppendAllText(fileName, text);
            }
            catch
            {

            }
        }
    }
}
