#region << 版 本 注 释 >>
/*
 * ========================================================================
 * Copyright(c) 2011-2012 BitAuto.com, All Rights Reserved.
 * ========================================================================
 *  
 * 【版本更新控制类】
 *  
 * 作者：[周燕龙]   时间：2011-12-28
 * 文件名：UpdateFilePresenter
 * 版本：V1.0.0
 * 
 * 修改者：[周燕龙]   时间：2012-01-05        
 * 修改说明：添加相关注释
 * ========================================================================
*/
#endregion

using System;
using System.Collections.Generic;
 

using PWMIS.EnterpriseFramework.Service.Basic;
using TranstarAuction.Model;

using System.IO;
using TranstarAuction.Presenters.ViewInterface;
using System.Threading;
using PWMIS.EnterpriseFramework.Common;

namespace TranstarAuction.Presenters.Presenter
{
    public class UpdateFilePresenter : PresenterBase
    {
        #region 属性

        /// <summary>
        /// 临时存储需要下载的文件列表
        /// </summary>
        public List<FileInfos> CurListInfos = null;

        private string _workpath = string.Empty;

        /// <summary>
        /// 文件下载路径
        /// </summary>
        public string WorkPath
        {
            get
            {
                return _workpath;
            }
            set
            {
                _workpath = value;
            }
        }

        private string _tempFolder = string.Empty;
        /// <summary>
        /// 文件下载到此临时目录
        /// </summary>
        public string TempFolder
        {
            get
            {
                string tempPath = System.IO.Path.GetTempPath();// System.Environment.GetEnvironmentVariable("TEMP");Win7下，目录为：C:\Users\zhouyanlong\AppData\Local\Temp\
                DirectoryInfo info = new DirectoryInfo(tempPath);
                tempPath = Path.Combine(info.FullName, "_TartGetTempFolder");
                return tempPath;   
            }
        }

        private bool isMustRestart = false;
        /// <summary>
        /// 是否必须重启更新
        /// </summary>
        public bool IsMustRestart
        {
            get { return isMustRestart; }
            set { isMustRestart = value; }
        }


        /// <summary>
        /// 索引
        /// </summary>
        private int _index = 0;

        private IAuctionMainFormView CurView;
        private bool IsNotifyNoFile = false;

        #endregion

        public void Close()
        {
            base.ServiceProxy.Close();
        }
        #region 自有方法

        /// <summary>
        /// 获取文件并更新
        /// </summary>
        /// <param name="view"></param>
        /// <param name="isNotify"></param>
        public void GetFiles(IAuctionMainFormView view, bool isNotify)
        {
            this.CurView = view;
            this.IsNotifyNoFile = isNotify;
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "FileUpdate";
            request.MethodName = "CurGetAllFiles";
            request.Parameters = new object[] { };
            base.ServiceProxy.RequestService<List<FileInfos>>(request, DataType.Json, (message) =>
            {
                #region 每次重新查询文件目录的时候清空CurListInfos

                if (CurListInfos != null)
                {
                    CurListInfos.Clear();
                }

                #endregion

                List<FileInfos> curInfos = message;
                foreach (var item in curInfos)
                {
                    CurSearchFiles(item.FilePath, item.FileModifyTime);
                }

                //对比本地文件得出需要更新的列表
                List<FileInfos> localFile = CurListInfos;             

                _index = 0;
                if (localFile != null)
                {
                    IsMustRestart = CurListInfos.Exists(o => o.FilePath.ToLower().EndsWith(".dll") || o.FilePath.ToLower().EndsWith(".exe"));
                    DowlaodFile();
                }
                else
                {
                    if (isNotify)
                    {
                        CurView.ShowMessage("没有需要更新的文件");
                    }
                }
            });
        }

        public void GetFiles(IAuctionMainFormView view, bool isNotify,string path)
        {
            this.CurView = view;
            this.IsNotifyNoFile = isNotify;
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "FileUpdate";
            request.MethodName = "CurGetAllFiles";
            request.Parameters = new object[] { };
            base.ServiceProxy.RequestService<List<FileInfos>>(request, DataType.Json, (message) =>
            {
                #region 每次重新查询文件目录的时候清空CurListInfos

                if (CurListInfos != null)
                {
                    CurListInfos.Clear();
                }

                #endregion

                List<FileInfos> curInfos = message;
                foreach (var item in curInfos)
                {
                    CurSearchFiles(item.FilePath, item.FileModifyTime,path);
                }

                //对比本地文件得出需要更新的列表
                List<FileInfos> localFile = CurListInfos;

                _index = 0;
                if (localFile != null)
                {
                    IsMustRestart = CurListInfos.Exists(o => o.FilePath.ToLower().EndsWith(".dll") || o.FilePath.ToLower().EndsWith(".exe"));
                    DowlaodFile();
                }
                else
                {
                    if (isNotify)
                    {
                        CurView.ShowMessage("没有需要更新的文件");
                    }
                }
            });
        }


        private void GetIsMustRestart()
        {
            CurListInfos.Exists(delegate(FileInfos file) { return file.FilePath.EndsWith(".dll") || file.FilePath.EndsWith(".exe"); });
            IsMustRestart = CurListInfos.Exists(o => o.FilePath.EndsWith(".dll") || o.FilePath.EndsWith(".exe"));          
        }

        /// <summary>
        /// 下载单个文件
        /// </summary>
        /// <param name="downInfo"></param>
        private void DowlaodFile(List<FileInfos> downInfo)
        {
            WriteFile(GetfilePath());
        }

        /// <summary>
        /// 下载单个文件
        /// </summary>
        private void DowlaodFile()
        {
            WriteFile(GetfilePath());
        }

        /// <summary>
        /// 获取当前需要下载的文件信息
        /// </summary>
        /// <returns></returns>
        private FileInfos GetfilePath()
        {
            if (_index > CurListInfos.Count - 1)
                return null;
            else
                return CurListInfos[_index++];
        }

        /// <summary>
        /// 下载单个文件
        /// </summary>
        /// <param name="item"></param>
        private void WriteFile(FileInfos item)
        {
            if (item == null)
            {
                CurView.ShowMessage("没有需要更新的文件");
                return;
            }

             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "FileUpdate";
            request.MethodName = "CurReadFile";
            request.Parameters = new object[] { item.FilePath };
            base.ServiceProxy.RequestService<byte[]>(request, DataType.Binary, (message) =>
            {
                byte[] curbyte = message;
                //本地临时临时文件夹，用于存储文件
                WorkPath = Path.Combine(TempFolder, item.FilePath);
                CurWriteFile(curbyte, WorkPath);

                DateTime date;
                if (DateTime.TryParseExact(item.FileModifyTime, "yyyy-MM-dd HH:mm:ss", new System.Globalization.CultureInfo("zh-CN"), System.Globalization.DateTimeStyles.None, out date))
                {
                    System.IO.File.SetLastWriteTime(WorkPath, date);
                }             

                //循环下载文件
                FileInfos curInfos = GetfilePath();
                if (curInfos != null)
                    WriteFile(curInfos);
                else
                {
                    //用于标注下载完成
                    if (IsMustRestart)
                        CurView.ShowMessage("_S200File_Completed");
                    else
                        CurView.ShowMessage("_S200File_Completed_Hidden");
                }
            });
        }

        /// <summary>
        /// 与本地文件信息对比得到需要下载的文件列表（本地不存在的文件和本地修改时间小于服务器修改时间的均满足条件）
        /// </summary>
        /// <param name="tarGetFileName"></param>
        /// <param name="ModifyTime"></param>
        public void CurSearchFiles(string tarGetFileName, string ModifyTime)
        {
            string ClientPath = Environment.CurrentDirectory;
            if (File.Exists(Path.Combine(ClientPath, tarGetFileName)))
            {
                FileInfo fi = new FileInfo(Path.Combine(ClientPath, tarGetFileName));
                //if (fi.LastWriteTime < DateTime.Parse(ModifyTime))
                if ((DateTime.Parse(ModifyTime) - fi.LastWriteTime).TotalSeconds > 10)
                {
                    if (CurListInfos == null)
                    {
                        CurListInfos = new List<FileInfos>();
                    }
                    CurListInfos.Add(new FileInfos() { FilePath = tarGetFileName, FileModifyTime = ModifyTime });
                }
            }
            else
            {
                if (CurListInfos == null)
                {
                    CurListInfos = new List<FileInfos>();
                }
                CurListInfos.Add(new FileInfos() { FilePath = tarGetFileName, FileModifyTime = ModifyTime });
            }
        }

        /// <summary>
        /// 筛选出需要下载的文件列表
        /// </summary>
        /// <param name="tarGetFileName"></param>
        /// <param name="ModifyTime"></param>
        /// <param name="path"></param>
        public void CurSearchFiles(string tarGetFileName, string ModifyTime,string path)
        {
            string ClientPath = path;//Environment.CurrentDirectory;
            if (File.Exists(Path.Combine(ClientPath, tarGetFileName)))
            {
                FileInfo fi = new FileInfo(Path.Combine(ClientPath, tarGetFileName));
                //if (fi.LastWriteTime < DateTime.Parse(ModifyTime))
                if ((DateTime.Parse(ModifyTime) - fi.LastWriteTime).TotalSeconds > 10)
                {
                    if (CurListInfos == null)
                    {
                        CurListInfos = new List<FileInfos>();
                    }
                    CurListInfos.Add(new FileInfos() { FilePath = tarGetFileName, FileModifyTime = ModifyTime });
                }
            }
            else
            {
                if (CurListInfos == null)
                {
                    CurListInfos = new List<FileInfos>();
                }
                CurListInfos.Add(new FileInfos() { FilePath = tarGetFileName, FileModifyTime = ModifyTime });
            }
        }

        /// <summary>
        /// 将服务器端获取到的字节流写入文件
        /// </summary>
        /// <param name="pReadByte">流</param>
        /// <param name="fileName">文件名</param>
        public void CurWriteFile(byte[] pReadByte, string fileName)
        {
            FileStream pFileStream = null;
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                }
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                pFileStream = new FileStream(fileName, FileMode.Create);
                pFileStream.Write(pReadByte, 0, pReadByte.Length);
                
            }
            catch
            {

#if DEBUG
                throw;
#endif
            }

            finally
            {
                if (pFileStream != null)
                    pFileStream.Close();
            }
        }

        /// <summary>
        /// 偷偷拷贝文件
        /// </summary>
        public void CopyFile()
        {
            System.IO.DirectoryInfo folderInfo = new System.IO.DirectoryInfo(TempFolder);
            int fileCount = GetFilesCount(folderInfo);

            if (fileCount > 0)
                DosCommandOutput.CopyNewFile(TempFolder, Environment.CurrentDirectory, "", DateTime.Now.AddYears(-1), 0);

            if (Directory.Exists(TempFolder))
                Directory.Delete(TempFolder, true);

        }

        /// <summary>
        /// 获取文件夹下的文件数目
        /// </summary>
        /// <param name="dirInfo"></param>
        /// <returns></returns>
        public int GetFilesCount(System.IO.DirectoryInfo dirInfo)
        {
            int totalFile = 0;
            totalFile += dirInfo.GetFiles().Length;
            foreach (System.IO.DirectoryInfo subdir in dirInfo.GetDirectories())
            {
                totalFile += GetFilesCount(subdir);
            }
            return totalFile;
        }



        /// <summary>
        /// 是否特殊目录接受UAC控制
        /// </summary>
        /// <returns></returns>
        public bool IsUAC()
        {
            #region 判断当前系统

            //获取系统信息
            System.OperatingSystem osInfo = System.Environment.OSVersion;
            ////获取操作系统ID
            System.PlatformID platformID = osInfo.Platform;
            //获取主版本号
            int versionMajor = osInfo.Version.Major;
            //获取副版本号
            int versionMinor = osInfo.Version.Minor;

            #endregion

            string curWorkPath = Environment.CurrentDirectory;

            if (versionMajor == 6 && versionMinor == 1 && curWorkPath.Contains("Program Files"))
                return true;
            else
                return false;
        }

        #endregion

        #region 服务更新，上传文件

        public List<UploadFileInfos> infos;

        /// <summary>
        /// Upload Local Files to Server
        /// </summary>
        /// <param name="path">LocalDirectory</param>
        /// <param name="uptype">Uptype,Service or Packets</param>
        /// <param name="progresscurrentfile">CurrentUploadFile</param>
        public void CallbackUploadFiles(string path,
            UploadFileType uptype,
            Action<UploadStateArg> action)
        {
            infos = new List<UploadFileInfos>();
            CurGetAllFiles(path, path, ref infos);

             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "FileUpload";
            request.MethodName = "GetUploadFilesFromClient";
            request.Parameters = new object[] { infos, uptype };
            base.ServiceProxy.RequestService<bool, UploadFileInfos, byte[]>(request.ServiceUrl, DataType.Text, (message) =>
            {
                UploadStateArg arg = new UploadStateArg();                
                //上传完成
                if (message)
                {
                    arg.State = UploadStateArg.UploadState.Success;
                    arg.msg = "Upload Complete!";
                }
                else
                {
                    arg.State = UploadStateArg.UploadState.Error;
                    arg.msg = "Upload Failed!";
                }
                action(arg);
            },
            curfile =>
            {
                action(new UploadStateArg() { State = UploadStateArg.UploadState.Uploading, ProgressFile = curfile.FilePath });
                var fullname = Path.Combine(path, curfile.FilePath);
                return CurReadFile(fullname);
            });
        }

        public void RequestUpdate(UploadFileType uptype)
        {
            if (uptype == UploadFileType.ClientPacket)
                UpdatePackets();
            else if (uptype == UploadFileType.ServicePacket)
                UpdateServices();
        }

        private void UpdatePackets()
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "FileUpload";
            request.MethodName = "UpdatePackets";
            request.Parameters = new object[] { };
            base.ServiceProxy.RequestService<bool>(request.ServiceUrl, DataType.Text, (message) =>
            {
                //
                //if (message)
                //    progresscurrentfile("上传完成！");
                //else
                //    progresscurrentfile("出现错误，上传失败！");
            });
        }

        private void UpdateServices()
        {
            base.ServiceProxy.SendCommandMessage("UpdateServiceHost");
        }

        /// <summary>
        /// 获取指定目录下所有文件信息：文件名、最后写入时间
        /// </summary>
        /// <param name="rootFolder">工作根目录</param>
        /// <param name="targetFolder">要遍历的目录</param>
        /// <param name="infos">文件信息</param>
        private void CurGetAllFiles(string rootFolder, string targetFolder, ref List<UploadFileInfos> infos)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(targetFolder);//把文件夹定义为DirectoryInfo形式
                FileSystemInfo[] fsinfo = di.GetFileSystemInfos();//获取此文件夹中的文件和文件夹的信息
                foreach (FileSystemInfo fs in fsinfo)//遍历数组
                {
                    if (fs is FileInfo)//如果是文件
                    {
                        if (((System.IO.FileInfo)(fs)).Length > 0 && !Path.GetFileName(fs.FullName).Equals("thumbs.db", StringComparison.OrdinalIgnoreCase))//如果文件大小为0则略过
                            infos.Add(new UploadFileInfos() { FilePath = fs.FullName.Replace(rootFolder + "\\", ""), FileModifyTime = fs.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss") });

                    }
                    if (fs is DirectoryInfo)//如果是文件夹
                    {
                        CurGetAllFiles(rootFolder, fs.FullName, ref infos);//递归
                    }
                }
            }
            catch
            {
                infos = new List<UploadFileInfos>();
            }
        }

        /// <summary>
        /// 读取文件返回字节流
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns></returns>
        private byte[] CurReadFile(string fileName)
        {
            FileStream pFileStream = null;
            byte[] pReadByte = new byte[0];
            try
            {
                pFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
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
        #endregion
    }

    public class UploadStateArg
    {
        public enum UploadState { Success, Uploading,Error }
        public UploadState State { get; set; }
        public string ProgressFile { get; set; }
        public string msg { get; set; }
    }
}
