using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Service.DTO
{
    /// <summary>
    /// 文件夹上传结果
    /// </summary>
    public class UploadResult
    {
        /// <summary>
        /// 上传是否全部成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 上传的文件数量
        /// </summary>
        public int FilesCount { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
    }
}
