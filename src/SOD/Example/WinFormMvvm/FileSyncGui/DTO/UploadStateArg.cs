using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Service.DTO
{
    /// <summary>
    /// 上传状态枚举
    /// </summary>
    public enum UploadState
    {
        /// <summary>
        /// 上传成功
        /// </summary>
        Success,
        /// <summary>
        /// 上传中
        /// </summary>
        Uploading,
        /// <summary>
        /// 错误
        /// </summary>
        Error
    }

    /// <summary>
    /// 上传状态参数
    /// </summary>
    public class UploadStateArg
    {
        /// <summary>
        /// 上传状态
        /// </summary>
        public UploadState State { get; set; }
        /// <summary>
        /// 上传的文件名
        /// </summary>
        public string ProgressFile { get; set; }
        /// <summary>
        /// 处理的消息，如果出错，这里是错误消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 处理进度（百分比）
        /// </summary>
        public int ProcessValue { get; set; }
        /// <summary>
        /// 总体处理进度（百分比）
        /// </summary>
        public int TotalProcessValue { get; set; }
    }
}
