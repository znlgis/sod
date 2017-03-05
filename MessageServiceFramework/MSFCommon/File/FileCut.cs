/*
 * ========================================================================
 * Copyright(c) 2008-2010北京高阳金信信息技术有限公司, All Rights Reserved.
 * ========================================================================
 *  大文件切割、合并操作类，比如邮件大附件的发送接收
 * 
 * 作者：邓太华     时间：2010-10-19
 * 版本：V1.0
 * 
 * 修改者：         时间：                
 * 修改说明：
 * ========================================================================
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PWMIS.EnterpriseFramework.Common
{
    /// <summary>
    /// 文件分割
    /// </summary>
    public class FileCut
    {
        /// <summary>
        /// 分卷文件头结构
        /// </summary>
        public struct CutedFileHead
        {
            public int Flag;
            public int Offset;
            public short FileNumber;
            public short FileCount;
            public string FileName;
            /// <summary>
            /// 合并分卷的时候，隐射的物理文件名
            /// </summary>
            public string MapFileName;
        }

        private const int SIZE = 1024;
        private const int CUT_FILE_FLAG = 101020;

        #region 属性定义
        /// <summary>
        /// 要切分的源文件
        /// </summary>
        public string SourceFile { get; set; }
        /// <summary>
        /// 操作的目标目录
        /// </summary>
        public string DescFolder { get; set; }
        /// <summary>
        /// 切分文件的大小，默认1M大小
        /// </summary>
        public int CutSize { get; set; }
        /// <summary>
        /// 操作中的错误信息，如果操作失败请即时检查该属性
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// 当前文件数量处理进度
        /// </summary>
        public int ProgresFileNumber { get; set; }
        /// <summary>
        /// 当前处理进度的文件大小
        /// </summary>
        public int ProgressFileSize { get; set; }
        /// <summary>
        /// 当前处理进度的文件已经处理的字节数
        /// </summary>
        public int ProgressFileRead { get; set; }
        /// <summary>
        /// 当前正在处理的文件名称
        /// </summary>
        public string ProgressFileName { get; set; }
        /// <summary>
        /// 进度是否处于工作中
        /// </summary>
        public bool ProgressWorking { get; private set; }
        /// <summary>
        /// 连接文件成功之后是否删除原始文件
        /// </summary>
        public bool DeleteFileArterLink = false;

        public string[] CutedFiles { get; private set; }
        #endregion

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FileCut()
        {
            this.CutSize = 1024 * 1024;
        }

        /// <summary>
        /// 使用要切分的文件名初始化本类
        /// </summary>
        /// <param name="sourceFile">要切分的文件名</param>
        public FileCut(string sourceFile)
            : this()
        {
            this.SourceFile = sourceFile;

        }

        #region 切分文件

        #region 重载方法
        /// <summary>
        /// 切分文件
        /// </summary>
        /// <returns>切分的结果文件列表，如果元素数量为0则表示切分失败</returns>
        public List<string> Cut()
        {
            return Cut(this.SourceFile, this.DescFolder, this.CutSize, 0, null);
        }
        /// <summary>
        /// 根据切分的文件数量自动切分文件
        /// </summary>
        /// <param name="count">切分的文件数量</param>
        /// <returns>切分的结果文件列表，如果元素数量为0则表示切分失败</returns>
        public List<string> Cut(int count)
        {
            if (string.IsNullOrEmpty(this.DescFolder))
                this.DescFolder = Path.GetDirectoryName(this.SourceFile);

            return Cut(this.SourceFile, this.DescFolder, 0, count, null);
        }
        #endregion

        /// <summary>
        /// 构建文件头字节信息数组
        /// </summary>
        /// <param name="cutFileNumber">文件序号，从1开始</param>
        /// <param name="cutFileCount">分卷文件的数量</param>
        /// <param name="sourceFileName">文件名</param>
        /// <returns>文件头字节</returns>
        private byte[] MakeFileHeadBytes(int cutFileNumber, int cutFileCount, string sourceFileName)
        {
            byte[] fileNameByte = System.Text.Encoding.Default.GetBytes(sourceFileName);
            int length = 2 * sizeof(int) + 2 * sizeof(short) + fileNameByte.Length;
            byte[] buffer = new byte[length];
            int index = 0;
            //写入标识
            BitConverter.GetBytes(CUT_FILE_FLAG).CopyTo(buffer, index);
            index += sizeof(int);
            //写入偏移量
            BitConverter.GetBytes(length).CopyTo(buffer, index);
            index += sizeof(int);
            //写入文件序号
            BitConverter.GetBytes((short)cutFileNumber).CopyTo(buffer, index);
            index += sizeof(short);
            //写入文件总数
            BitConverter.GetBytes((short)cutFileCount).CopyTo(buffer, index);
            index += sizeof(short);
            //写入文件名称
            fileNameByte.CopyTo(buffer, index);

            return buffer;
        }

        /// <summary>
        /// 获取分卷文件的文件头结构
        /// </summary>
        /// <param name="fs">文件流，调用此方法文件指针将会重置到文件起始位置</param>
        /// <returns></returns>
        public CutedFileHead GetCutedFileHead(FileStream fs)
        {
            fs.Seek(0, SeekOrigin.Begin);
            BinaryReader br = new BinaryReader(fs);
            CutedFileHead head = new CutedFileHead();
            head.Flag = br.ReadInt32();

            if (head.Flag == CUT_FILE_FLAG)
            {
                head.Offset = br.ReadInt32();
                head.FileNumber = br.ReadInt16();
                head.FileCount = br.ReadInt16();

                int strLength = head.Offset - 2 * sizeof(int) - 2 * sizeof(short);
                head.FileName = System.Text.Encoding.Default.GetString(br.ReadBytes(strLength));
            }
            else
            {
                head.FileName = "无效的分卷文件。";
            }
            return head;
        }

        /// <summary>
        /// 根据指定的分卷文件名，获取分卷头信息
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public CutedFileHead GetCutedFileHead(string fileName)
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                return GetCutedFileHead(fileStream);
            }
        }

        /// <summary>
        /// 切分文件。程序将在每个切分后的文件头上添加一定的附件信息指示源文件名和当前顺序。
        /// </summary>
        /// <remarks>
        ///分卷文件头结构定义：
        ///分卷文件标识 + 正文位置偏移量 + 分卷序号 + 分卷总数 + 源文件名
        ///每一部分按照顺序指定，其中，
        ///int 分卷文件标识 = 101020 ,取今天这段代码的日期 2010.10.20
        ///short 分卷序号     = 1,2,3...等文件顺序号
        ///short 分卷总数     = 2,3...等源文件分卷后的分卷文件数量
        ///string 源文件名  = 分卷前的原始文件名，带扩展名
        ///int 正文位置偏移量= sizeof(分卷文件标识)+sizeof(分卷序号)
        ///                    +sizeof(分卷总数)+byte[](源文件名).Length
        ///
        ///程序运行首先检查分卷文件标识，然后检查分卷序号是否连续，检查分卷的总数量是否正确，
        ///然后检查分卷文件名称是否一致，最后取得每个分卷文件的正文位置偏移量，开始读取文件。
        /// </remarks>
        /// <param name="sourceFile">源文件</param>
        /// <param name="descFolder">要存放文件的目录，如果为空，则使用源文件所在的目录</param>
        /// <param name="size">单个切分文件的大小，如果指定了此参数，将忽略count参数。</param>
        /// <param name="count">将文件切分成指定的个数，如果指定了size参数，将忽略此参数。</param>
        /// <param name="extName">切分后文件的扩展名，默认为 “part”</param>
        /// <returns>切分的结果文件列表，如果元素数量为0则表示切分失败</returns>
        public List<string> Cut(string sourceFile, string descFolder, int size, int count, string extName)
        {
            this.ProgressWorking = true ;
            this.ErrorMessage = "";
            if (!File.Exists(sourceFile))
            {
                this.ErrorMessage = "要切割的源文件不存在。" + sourceFile;
                return null;
            }
            if (size <= 0 && count <= 0)
            {
                this.ErrorMessage = "size参数和 count需要至少指定一个。";
                return null;
            }

            FileInfo fi = new FileInfo(sourceFile);
            if (fi.Length <= size)
            {
                this.ErrorMessage = "要切割的源文件比指定的切割文件长度小，不需要切割。";
                return null;
            }
            if (!Directory.Exists(descFolder))
                Directory.CreateDirectory(descFolder);
            if (!descFolder.EndsWith("\\"))
                descFolder += "\\";
            if (string.IsNullOrEmpty(extName))
            {
                extName = ".part";
            }
            else
            {
                if (extName.StartsWith("*."))
                    extName = extName.Substring(1);
                else if (!extName.StartsWith("."))
                    extName = "." + extName;
            }

            int fileLength = (int)fi.Length;

            if (size <= 0 && count > 0)
            {
                size = (fileLength / count) + 1;
            }
            if (count <= 0)
            {
                count = (fileLength / size) + 1;
            }

            string baseFileName = Path.GetFileName(sourceFile);
            string descFileName = descFolder + baseFileName;
            FileStream fsDesc = null;
            int doCount = 0;
            List<string> cutedFiels = new List<string>();
            this.ProgressFileSize = fileLength;

            try
            {
                using (FileStream fileStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
                {
                    int offset = 0;
                    int readCount = 0;
                    int currSize = 0;
                    do
                    {
                        byte[] dataArray = new byte[SIZE];
                        readCount = fileStream.Read(dataArray, 0, SIZE);
                        if (readCount == 0)
                            break;

                        offset += readCount;
                        this.ProgressFileRead = offset;
                        //模拟大文件，正式版本请注释下面一行代码
                        //System.Threading.Thread.Sleep(10);

                        //生成一个新的文件名并创建新的文件流
                        if (currSize == 0)
                        {
                            doCount++;//文件序号，从1开始
                            string currFileName = descFileName + doCount + extName;
                            this.ProgresFileNumber = doCount;
                            this.ProgressFileName = currFileName;

                            fsDesc = File.Create(currFileName);

                            byte[] fileHeadByte = MakeFileHeadBytes(doCount, count, baseFileName);
                            fsDesc.Write(fileHeadByte, 0, fileHeadByte.Length);

                            cutedFiels.Add(currFileName);
                        }

                        if (fsDesc != null)
                        {
                            fsDesc.Write(dataArray, 0, readCount);

                            currSize += readCount;
                            //达到一个切割文件的大小，关闭当前文件流
                            if (currSize >= size || offset >= fileLength)
                            {
                                fsDesc.Flush();
                                fsDesc.Close();
                                fsDesc.Dispose();
                                fsDesc = null;
                                currSize = 0;
                            }
                        }


                    } while (readCount == SIZE);
                    fileStream.Close();
                    fileStream.Dispose();
                }
                this.ProgressWorking = false;
                return cutedFiels;
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
               
            }
            finally
            {
                if (fsDesc != null)
                {
                    fsDesc.Close();
                    fsDesc.Dispose();
                    fsDesc = null;
                }
            }
            this.ProgressWorking = false;
            return null;
        }

        #endregion

        #region 链接文件

        #region 重载方法
        /// <summary>
        /// 链接文件
        /// </summary>
        /// <param name="sourceFolder">要链接的文件所在的目录</param>
        /// <returns>链接后的文件名，如果操作不成功返回空</returns>
        public string Link(string sourceFolder)
        {
            return Link(sourceFolder, "*.part", SearchOption.TopDirectoryOnly, this.DescFolder, null);
        }

        /// <summary>
        /// 链接文件
        /// </summary>
        /// <param name="cutedFiles">切分的文件名数组</param>
        /// <returns>链接后的文件名，如果操作不成功返回空</returns>
        public string Link(string[] cutedFiles)
        {
            return Link(cutedFiles, this.DescFolder, null);
        }
       
        /// <summary>
        /// 校验一组文件是否是有效的分组文件
        /// </summary>
        /// <param name="cutedFiles">要检查的源分卷文件数组</param>
        /// <param name="heads">符合目标的文件头数组</param>
        /// <returns></returns>
        public bool VerifyCutedFiles(string[] cutedFiles, ref CutedFileHead[] heads)//, out  List<string>  sortedFiles
        {
            //sortedFiles = new List<string>();
            this.ErrorMessage = "";
            if (cutedFiles.Length <= 1)
            {
                this.ErrorMessage = "分卷文件需要至少2个。";
                return false;
            }

            //检查所有文件头信息
            string checkMsg = "";
           
            List<CutedFileHead> headList = new List<CutedFileHead>();//本次所有有效的文件头列表

            for (int i = 0; i < cutedFiles.Length; i++)
            {
                CutedFileHead head = GetCutedFileHead(cutedFiles[i]);
                if (head.Flag != CUT_FILE_FLAG)
                {
                    checkMsg += "\r\n当前文件不是有效的分卷文件：" + cutedFiles[i];
                    //将无效文件删除
                    File.Delete(cutedFiles[i]);
                }
                else
                {
                    head.MapFileName = cutedFiles[i];
                    headList.Add(head);
                }
            }

            if (headList.Count <= 1)
            {
                //有效的分卷文件不够数量
                this.ErrorMessage = "有效的分卷文件至少需要2个；" + checkMsg;
                return false;
            }


            List<CutedFileHead> checkedHeads = new List<CutedFileHead>();//验证有效的文件文件列表
            var firstHeads = headList.FindAll(p => p.FileNumber == 1);//先找到第一个分卷文件，可能有多个（比如分次发送不同大小的文件）
            var foundOK = false;
            foreach (CutedFileHead item in firstHeads)
            {
                checkedHeads.Clear();
                checkedHeads.Add(item);
                int fileCount = item.FileCount;//当前分卷文件对应的同类文件个数
                for (int fileNumber = 2; fileNumber <= fileCount; fileNumber++)
                {
                    var currtHeads = headList.FindAll(p => p.FileNumber == fileNumber && p.FileName == item.FileName).ToArray();
                    if (currtHeads.Length > 0)
                    {
                        //找到当前顺序的文件
                        checkedHeads.Add(currtHeads[0]);
                    }
                    else
                    { 
                        //当前分卷文件不连续
                        break;
                    }
                }
                if (checkedHeads.Count == item.FileCount)
                { 
                    //寻找到有效的文件头列表
                    foundOK = true;
                    break;
                }
            }
            if (foundOK)
            {
                heads = checkedHeads.ToArray();
                return true;
            }
            else
            {
                this.ErrorMessage = "分卷文件不连续!";
                return false;
            }
        }
        /// <summary>
        /// 链接文件。
        /// </summary>
        /// <param name="cutedFiles">切分的文件名数组</param>
        /// <param name="descFolder">链接文件保存的目录，如果为空则采用第一个分卷文件所在的目录</param>
        /// <param name="descFileName">链接文件的名称，不带路径。如果为空，则采用链接文件中记录的原始文件名</param>
        /// <returns>链接后的文件名，如果操作不成功返回空</returns>
        public string Link(string[] cutedFiles, string descFolder, string descFileName)
        {
            this.ProgressWorking = true;
            this.CutedFiles = cutedFiles;
            CutedFileHead[] heads = new CutedFileHead[cutedFiles.Length];
            //List<string> sortedFiles = null;
            if (!VerifyCutedFiles(cutedFiles, ref heads))//, out sortedFiles
            {
                return "";
            }
            if (string.IsNullOrEmpty(descFolder))
            {
                descFolder = System.IO.Path.GetDirectoryName(cutedFiles[0]);
            }
            else
            {
                if (!Directory.Exists(descFolder))
                    Directory.CreateDirectory(descFolder);
            }
            string sourceFileName = string.IsNullOrEmpty(descFileName) ? heads[0].FileName : descFileName;
            FileStream fsDesc = null;// 

            try
            {
                fsDesc = File.Create(descFolder + "\\" + sourceFileName);
                //开始合并文件
                for (int i = 0; i < heads.Length ; i++)
                {
                    this.ProgressFileName = heads[i].MapFileName ;
                    this.ProgresFileNumber = i;

                    using (FileStream fsRead = new FileStream(heads[i].MapFileName, FileMode.Open, FileAccess.Read))
                    {
                        this.ProgressFileSize = (int)fsRead.Length;

                        //定位到文件的正文开头
                        fsRead.Seek(heads[i].Offset, SeekOrigin.Begin);

                        int offset = 0;
                        int readCount = 0;
                        do
                        {
                            byte[] dataArray = new byte[SIZE];
                            readCount = fsRead.Read(dataArray, 0, SIZE);
                            if (readCount == 0)
                                break;

                            //可以将 offset 作为进度条信息给外面使用,建议使用时钟定期查询
                            offset += readCount;
                            this.ProgressFileRead = offset;
                            //模拟大文件，正式版本请注释下面一行代码
                            //System.Threading.Thread.Sleep(10);
                            //将当前块数据写入目标文件
                            fsDesc.Write(dataArray, 0, readCount);
                        } while (readCount == SIZE);
                        fsRead.Close();
                        fsRead.Dispose();
                    }
                }//end for
                //连接成功后删除源文件
                if (this.DeleteFileArterLink)
                {
                    for (int i = 0; i < heads.Length ; i++)
                    {
                        File.Delete(heads[i].MapFileName);
                    }
                }

            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
               
            }
            finally 
            {
                if (fsDesc != null)
                {
                    fsDesc.Close();
                    fsDesc.Dispose();
                    fsDesc = null;
                }
            }
            this.ProgressWorking = false ;
            return sourceFileName;
        }
        #endregion

        /// <summary>
        /// 链接文件。
        /// </summary>
        /// <param name="sourceFolder">切分后的文件所在的目录</param>
        /// <param name="searchPattern">要搜索的文件的扩展名，例如 *.*,*.part</param>
        /// <param name="searchOption">是搜索当前目录，还是当前目录和它所有的子目录</param>
        /// <param name="descFolder">要存放链接后的文件所在的目录</param>
        /// <param name="descFileName">链接后的文件新的文件名，不要带路径</param>
        /// <returns>链接后的文件名，如果操作不成功返回空</returns>
        public string Link(string sourceFolder, string searchPattern, SearchOption searchOption, string descFolder, string descFileName)
        {
            if (!string.IsNullOrEmpty(sourceFolder))
            {
                DirectoryInfo di = new DirectoryInfo(sourceFolder);
                FileInfo[] files = di.GetFiles(searchPattern, searchOption);
                string[] cutedFiles = new string[files.Length];
                for (int i = 0; i < files.Length; i++)
                    cutedFiles[i] = files[i].FullName;

                return Link(cutedFiles, descFolder, descFileName);
            }
            else
            {
                this.ErrorMessage = "切分后的文件所在的目录不能为空。";
                return "";
            
            }
            
        }

        #endregion
    }
}
