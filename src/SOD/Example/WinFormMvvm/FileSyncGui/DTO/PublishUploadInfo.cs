using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Service.DTO
{
    /// <summary>
    /// 推送的上传信息
    /// </summary>
    public class PublishUploadInfo
    {
        public bool IsUploading { get; set; }
        public UploadStateArg StateArg { get; set; }
        public UploadResult Result { get; set; }
    }
}
