#region << 版 本 注 释 >>
/*
 * ========================================================================
 * Copyright(c) 2011-2012 BitAuto.com, All Rights Reserved.
 * ========================================================================
 *  
 * 【版本更新服务类】
 *  
 * 作者：[周燕龙]   时间：2011-12-27
 * 文件名：FileUpdateService
 * 版本：V1.0.0
 * 
 * 修改者：[周燕龙]   时间：2012-01-05        
 * 修改说明：添加相关注释
 * ========================================================================
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TranstarAuction.Service.Runtime;
using System.IO;
using TranstarAuction.Model;


namespace TranstarAuction.Service
{
    public class FileUpdateService : ServiceBase
    {
        #region 变量

        private string _workWfolder = string.Empty;
        private string _folder = string.Empty;

        #endregion     

        public FileUpdateService()
        {
            _workWfolder = System.Configuration.ConfigurationManager.AppSettings["UpdatFileFolder"];
            if (string.IsNullOrEmpty(_workWfolder)) _workWfolder = "WorkFolder";    
            _folder = Path.Combine(Environment.CurrentDirectory, _workWfolder); 
        }

        /// <summary>
        /// 获取指定目录下所有文件信息：文件名、最后写入时间
        /// </summary>
        /// <param name="rootFolder">工作根目录</param>
        /// <param name="targetFolder">要遍历的目录</param>
        /// <param name="infos">文件信息</param>
        public void CurGetAllFiles(string rootFolder, string targetFolder, ref List<FileInfos> infos)
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
                            infos.Add(new FileInfos() { FilePath = fs.FullName.Replace(rootFolder + "\\", ""), FileModifyTime = fs.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss") });

                    }
                    if (fs is DirectoryInfo)//如果是文件夹
                    {
                        CurGetAllFiles(rootFolder, fs.FullName, ref infos);//递归
                    }
                }
            }
            catch
            {
                infos = new List<FileInfos>();
            }
        }  

        /// <summary>
        /// 遍历服务器指定目录的所有文件，获取文件路径和修改时间返回
        /// </summary>
        /// <param name="rootFolder">根目录</param>
        /// <param name="targetFolder">每次遍历的目标文件夹</param>
        /// <param name="infos">存储的文件信息</param>
        public List<FileInfos>  CurGetAllFiles()//定义静态的函数
        {
            string rootFolder = string.Empty;
            string targetFolder = string.Empty;

            //string folder = Path.Combine(Environment.CurrentDirectory, _workWfolder + "\\");
            if (!Directory.Exists(_folder))
            {
                Directory.CreateDirectory(_folder);
            }

            rootFolder = targetFolder = _folder;
            List<FileInfos> infos = new List<FileInfos>();
           
            CurGetAllFiles(rootFolder, targetFolder, ref infos);
            return infos;
        }  

        /// <summary>
        /// 读取文件返回字节流
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns></returns>
        public byte[] CurReadFile(string fileName)
        {

            fileName = Path.Combine(_folder,fileName);
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
    }
}
