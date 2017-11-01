using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TranstarAuction.Model
{
    public class FileInfos
    {
        public string FilePath { get; set; }
        public string FileModifyTime { get; set; }
       
    }

    public enum UploadFileType 
    {
        ClientPacket,
        ServicePacket
    }
    public class UploadFileInfos : FileInfos
    {
        public bool IsUploadComplete { get; set; }
    }

    //public enum ListViewItemCategory { Directory,File}
    //public class ListViewInfo
    //{
    //    public ListViewItemCategory ItemCategory { get; set; }
    //    public string Name { get; set; }
    //    public DateTime ModifyTime { get; set; }
    //    public string Category { get; set; }
    //    public string Size { get; set; }

    //}
}
