using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Service.DTO
{
    public class UploadFileInfos
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 文件修改时间
        /// </summary>
        public DateTime FileModifyTime { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 是否上传完成
        /// </summary>
        public bool IsUploadComplete { get; set; }
        /// <summary>
        /// 要上传的文件流中的偏移量
        /// </summary>
        public long Offset { get; set; }
        /// <summary>
        /// 要上传的文件流长度
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// 当前文件在整体上传文件列表中的索引号
        /// </summary>
        public int UploadIndex { get; set; }
    }
}
