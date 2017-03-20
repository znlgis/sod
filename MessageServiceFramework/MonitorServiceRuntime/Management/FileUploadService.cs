using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TranstarAuction.Service.Runtime;

using System.Runtime.InteropServices;
using TranstarAuction.Model;

namespace TranstarAuction.Service
{
    public class FileUploadService : ServiceBase
    {
        /// <summary>
        /// 客户端升级补丁下载到此临时目录
        /// </summary>
        public string PacketsTempFolder { get; set; }

        /// <summary>
        /// 服务端程序下载到此临时目录
        /// </summary>
        public string ServicesTempFolder { get; set; }

        /// <summary>
        /// 客户端补丁目录
        /// </summary>
        public string WorkFolder { get; set; }

        public FileUploadService()
        {
            string tempPath = System.IO.Path.GetTempPath();// System.Environment.GetEnvironmentVariable("TEMP");Win7下，目录为：C:\Users\zhouyanlong\AppData\Local\Temp\
            DirectoryInfo info = new DirectoryInfo(tempPath);
            PacketsTempFolder = Path.Combine(info.FullName, "_TartGetTempFolder_Packets");
            ServicesTempFolder = Path.Combine(info.FullName, "_TartGetTempFolder_Services");

            var workWfolder = System.Configuration.ConfigurationManager.AppSettings["UpdatFileFolder"];
            if (string.IsNullOrEmpty(workWfolder)) workWfolder = "WorkFolder";
            WorkFolder = Path.Combine(Environment.CurrentDirectory, workWfolder);
        }
        //#region 获取文件类型信息
        ////在shell32.dll导入函数SHGetFileInfo
        //[DllImport("shell32.dll", EntryPoint = "SHGetFileInfo")]
        //public static extern int GetFileInfo(string pszPath, int dwFileAttributes,
        //    ref FileInfomation psfi, int cbFileInfo, int uFlags);

        ////定义SHFILEINFO结构(名字随便起，这里用FileInfomation)
        //[StructLayout(LayoutKind.Sequential)]
        //public struct FileInfomation
        //{
        //    public IntPtr hIcon;
        //    public int iIcon;
        //    public int dwAttributes;

        //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        //    public string szDisplayName;

        //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        //    public string szTypeName;
        //}

        ////定义文件属性标识
        //public enum FileAttributeFlags : int
        //{
        //    FILE_ATTRIBUTE_READONLY = 0x00000001,
        //    FILE_ATTRIBUTE_HIDDEN = 0x00000002,
        //    FILE_ATTRIBUTE_SYSTEM = 0x00000004,
        //    FILE_ATTRIBUTE_DIRECTORY = 0x00000010,
        //    FILE_ATTRIBUTE_ARCHIVE = 0x00000020,
        //    FILE_ATTRIBUTE_DEVICE = 0x00000040,
        //    FILE_ATTRIBUTE_NORMAL = 0x00000080,
        //    FILE_ATTRIBUTE_TEMPORARY = 0x00000100,
        //    FILE_ATTRIBUTE_SPARSE_FILE = 0x00000200,
        //    FILE_ATTRIBUTE_REPARSE_POINT = 0x00000400,
        //    FILE_ATTRIBUTE_COMPRESSED = 0x00000800,
        //    FILE_ATTRIBUTE_OFFLINE = 0x00001000,
        //    FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x00002000,
        //    FILE_ATTRIBUTE_ENCRYPTED = 0x00004000
        //}

        ////定义获取资源标识
        //public enum GetFileInfoFlags : int
        //{
        //    SHGFI_ICON = 0x000000100,     // get icon
        //    SHGFI_DISPLAYNAME = 0x000000200,     // get display name
        //    SHGFI_TYPENAME = 0x000000400,     // get type name
        //    SHGFI_ATTRIBUTES = 0x000000800,     // get attributes
        //    SHGFI_ICONLOCATION = 0x000001000,     // get icon location
        //    SHGFI_EXETYPE = 0x000002000,     // return exe type
        //    SHGFI_SYSICONINDEX = 0x000004000,     // get system icon index
        //    SHGFI_LINKOVERLAY = 0x000008000,     // put a link overlay on icon
        //    SHGFI_SELECTED = 0x000010000,     // show icon in selected state
        //    SHGFI_ATTR_SPECIFIED = 0x000020000,     // get only specified attributes
        //    SHGFI_LARGEICON = 0x000000000,     // get large icon
        //    SHGFI_SMALLICON = 0x000000001,     // get small icon
        //    SHGFI_OPENICON = 0x000000002,     // get open icon
        //    SHGFI_SHELLICONSIZE = 0x000000004,     // get shell size icon
        //    SHGFI_PIDL = 0x000000008,     // pszPath is a pidl
        //    SHGFI_USEFILEATTRIBUTES = 0x000000010,     // use passed dwFileAttribute
        //    SHGFI_ADDOVERLAYS = 0x000000020,     // apply the appropriate overlays
        //    SHGFI_OVERLAYINDEX = 0x000000040      // Get the index of the overlay
        //}


        //private string GetTypeName(string fileName)
        //{
        //    FileInfomation fileInfo = new FileInfomation();  //初始化FileInfomation结构

        //    //调用GetFileInfo函数，最后一个参数说明获取的是文件类型(SHGFI_TYPENAME)
        //    int res = GetFileInfo(fileName, (int)FileAttributeFlags.FILE_ATTRIBUTE_NORMAL,
        //        ref fileInfo, Marshal.SizeOf(fileInfo), (int)GetFileInfoFlags.SHGFI_TYPENAME);

        //    return fileInfo.szTypeName;
        //}
        //#endregion

        //public List<ListViewInfo> GetDirectoryInfos(UploadFileType uftype)
        //{
        //    List<ListViewInfo> list = null;
        //    switch (uftype)
        //    {
        //        case UploadFileType.ClientPacket:
        //            DirectoryInfo directoryinfo = new DirectoryInfo(WorkFolder);
        //            var filesysteminfos = directoryinfo.GetFileSystemInfos();
        //            list = filesysteminfos.Select(o =>
        //                {
        //                    var info = new ListViewInfo();
        //                    var type = o as DirectoryInfo;
        //                    if (type != null)
        //                    {
        //                        info.ItemCategory = ListViewItemCategory.Directory;
        //                        info.Category = "文件夹";
        //                        info.Name = type.Name;
        //                        info.ModifyTime = type.LastWriteTime;
        //                    }
        //                    else
        //                    {
        //                        var file = o as FileInfo;
        //                        info.ItemCategory = ListViewItemCategory.File;
        //                        info.Category = GetTypeName(file.FullName);
        //                        info.Name = file.Name;
        //                        info.ModifyTime = file.LastWriteTime;
        //                        info.Size = file.Length + "字节";
        //                    }
        //                    return info;
        //                }).ToList();

        //            break;
        //        case UploadFileType.ServicePacket:
        //            break;
        //    }
        //    return list;
        //}

        /// <summary>
        /// 上传文件（通过回调客户端的方式）
        /// </summary>
        /// <param name="list">文件列表</param>
        public bool GetUploadFilesFromClient(List<UploadFileInfos> list, UploadFileType uftype)
        {
            if (list != null && list.Count > 0)
            {
                var tempdir = GetTempDir(uftype);
                if (Directory.Exists(tempdir))
                    Directory.Delete(tempdir, true);
                else
                    Directory.CreateDirectory(tempdir);

                try
                {
                    foreach (var info in list)
                    {
                        var pathfile = Path.Combine(tempdir, info.FilePath);
                        var data = GetUploadFile(info);
                        if (data.Length == 0)
                        {//如果有长度为零的文件表示客户读取文件失败，终止本次补丁发布操作
                            return false;
                        }
                        CurWriteFile(data, pathfile);

                        DateTime date;
                        if (DateTime.TryParseExact(info.FileModifyTime, "yyyy-MM-dd HH:mm:ss", new System.Globalization.CultureInfo("zh-CN"), System.Globalization.DateTimeStyles.None, out date))
                        {
                            System.IO.File.SetLastWriteTime(pathfile, date);
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        public bool UpdatePackets()
        {
            try
            {
                CopyDir(PacketsTempFolder, WorkFolder);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string GetTempDir(UploadFileType uftype)
        {//临时目录
            string tempdir = string.Empty;
            switch (uftype)
            {
                case UploadFileType.ClientPacket:
                    tempdir = PacketsTempFolder;
                    break;
                case UploadFileType.ServicePacket:
                    tempdir = ServicesTempFolder;
                    break;
            }
            return tempdir;
        }

        private byte[] GetUploadFile(UploadFileInfos fileinfo)
        {//回调客户端
            return base.CurrentContext.CallBackFunction<UploadFileInfos, byte[]>(fileinfo);
        }

        private void CopyDir(string srcPath, string aimPath)
        {//复制目录
            try
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加之
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                    aimPath += Path.DirectorySeparatorChar;
                // 判断目标目录是否存在如果不存在则新建之
                if (!Directory.Exists(aimPath)) Directory.CreateDirectory(aimPath);
                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
                // string[] fileList = Directory.GetFiles(srcPath);
                string[] fileList = Directory.GetFileSystemEntries(srcPath);
                // 遍历所有的文件和目录
                foreach (string file in fileList)
                {
                    // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                    if (Directory.Exists(file))
                        CopyDir(file, aimPath + Path.GetFileName(file));
                    // 否则直接Copy文件
                    else
                        File.Copy(file, aimPath + Path.GetFileName(file), true);
                }
            }
            catch (Exception e)
            {
                ;
            }
        }

        /// <summary>
        /// 将服务器端获取到的字节流写入文件
        /// </summary>
        /// <param name="pReadByte">流</param>
        /// <param name="fileName">文件名</param>
        private void CurWriteFile(byte[] pReadByte, string fileName)
        {//写入文件
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
    }
}
