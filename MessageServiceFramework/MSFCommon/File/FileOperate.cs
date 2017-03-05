using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TranstarAuction.Common
{
    //public class FileInfos
    //{
    //    public string FilPath { get; set; }
    //    public string FileModifyTime { get; set; }
    //}


    //public class FileOperate
    //{
    //    /// <summary>
    //    /// 遍历服务器指定目录的所有文件，获取文件路径和修改时间返回
    //    /// </summary>
    //    /// <param name="rootFolder">根目录</param>
    //    /// <param name="targetFolder">每次遍历的目标文件夹</param>
    //    /// <param name="infos">存储的文件信息</param>
    //    public void CurGetAllFiles(string rootFolder, string targetFolder, ref List<FileInfos> infos)//定义静态的函数
    //    {
    //        try
    //        {
    //            DirectoryInfo di = new DirectoryInfo(targetFolder);//把文件夹定义为DirectoryInfo形式
    //            FileSystemInfo[] fsinfo = di.GetFileSystemInfos();//获取此文件夹中的文件和文件夹的信息，返回一FileSystemInfo格式的数组
    //            foreach (FileSystemInfo fs in fsinfo)//遍历数组
    //            {
    //                if (fs is FileInfo)//如果是文件
    //                {
    //                    infos.Add(new FileInfos() { FilPath = fs.FullName.Replace(rootFolder, ""), FileModifyTime = fs.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss") });
    //                }
    //                if (fs is DirectoryInfo)//如果是文件夹
    //                {
    //                    CurGetAllFiles(rootFolder, fs.FullName, ref infos);//递归
    //                }
    //            }
    //        }
    //        catch
    //        {

    //        }
    //    }

    //    /// <summary>
    //    /// 用于存储符合条件的文件
    //    /// </summary>
    //    public List<FileInfos> CurListInfos = null;

    //    /// <summary>
    //    ///
    //    /// </summary>
    //    /// <param name="tarGetFolder">要搜索的文件夹</param>
    //    /// <param name="tarGetFileName">要搜索的文件名</param>
    //    /// <param name="ModifyTime">要搜索的文件的最后修改时间</param>
    //    public void CurSearchFiles(string tarGetFileName, string ModifyTime)
    //    {
    //        string ClientPath = Environment.CurrentDirectory;
    //        if (File.Exists(Path.Combine(ClientPath, tarGetFileName)))
    //        {
    //            FileInfo fi = new FileInfo(Path.Combine(ClientPath, tarGetFileName));
    //            if (fi.LastWriteTime < DateTime.Parse(ModifyTime))
    //            {
    //                if (CurListInfos == null)
    //                {
    //                    CurListInfos = new List<FileInfos>();
    //                }
    //                CurListInfos.Add(new FileInfos() { FilPath = tarGetFileName, FileModifyTime = ModifyTime });
    //            }

    //        }
    //        else
    //        {
    //            if (CurListInfos == null)
    //            {
    //                CurListInfos = new List<FileInfos>();
    //            }
    //            CurListInfos.Add(new FileInfos() { FilPath = tarGetFileName, FileModifyTime = ModifyTime });
    //        }
    //    }

    //    public List<string> CurListInfo = null;
    //    public void CurSearchFile(string tarGetFileName, string ModifyTime)
    //    {
    //        string ClientPath = Environment.CurrentDirectory;
    //        if (File.Exists(Path.Combine(ClientPath, tarGetFileName)))
    //        {
    //            FileInfo fi = new FileInfo(Path.Combine(ClientPath, tarGetFileName));
    //            if (fi.LastWriteTime < DateTime.Parse(ModifyTime))
    //            {
    //                if (CurListInfo == null)
    //                {
    //                    CurListInfo = new List<string>();
    //                }
    //                CurListInfo.Add(tarGetFileName);
    //            }

    //        }
    //        else
    //        {
    //            if (CurListInfo == null)
    //            {
    //                CurListInfo = new List<string>();
    //            }
    //            CurListInfo.Add(tarGetFileName);
    //        }
    //    }

    //    /// <summary>
    //    /// 读取文件
    //    /// </summary>
    //    /// <param name="fileName"></param>
    //    /// <returns></returns>
    //    public byte[] CurReadFile(string fileName)
    //    {

    //        FileStream pFileStream = null;
    //        byte[] pReadByte = new byte[0];
    //        try
    //        {
    //            pFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
    //            BinaryReader r = new BinaryReader(pFileStream);
    //            r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开
    //            pReadByte = r.ReadBytes((int)r.BaseStream.Length);
    //            return pReadByte;
    //        }
    //        catch
    //        {
    //            return pReadByte;
    //        }
    //        finally
    //        {
    //            if (pFileStream != null)
    //                pFileStream.Close();
    //        }
    //    }

    //    /// <summary>
    //    /// 写文件
    //    /// </summary>
    //    /// <param name="pReadByte"></param>
    //    /// <param name="fileName"></param>
    //    public void CurWriteFile(byte[] pReadByte, string fileName)
    //    {
    //        FileStream pFileStream = null;
    //        try
    //        {
    //            if (File.Exists(fileName))
    //            {
    //                File.Delete(fileName);
    //            }
    //            pFileStream = new FileStream(fileName, FileMode.Create);
    //            pFileStream.Write(pReadByte, 0, pReadByte.Length);
    //        }
    //        catch
    //        {
    //        }

    //        finally
    //        {
    //            if (pFileStream != null)
    //                pFileStream.Close();
    //        }
    //    }
    //}
}
