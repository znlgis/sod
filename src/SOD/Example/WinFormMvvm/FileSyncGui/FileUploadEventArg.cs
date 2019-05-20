using Common.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSyncGui
{
    class FileUploadEventArg:EventArgs
    {
        public UploadStateArg StateArg { get; private set; }
        public FileUploadEventArg(UploadStateArg arg)
        {
            this.StateArg = arg;
        }
    }

    class FilesUploadResultEventArg : EventArgs
    {
        public UploadResult Result { get; private set; }
        public FilesUploadResultEventArg(UploadResult result)
        {
            this.Result = result;
        }
    }

    class UploadErrorEventArg : EventArgs
    {
        public string ErrorMessage { get; private set; }
        public UploadErrorEventArg(string errMsg)
        {
            this.ErrorMessage = errMsg;
        }
    }
}
