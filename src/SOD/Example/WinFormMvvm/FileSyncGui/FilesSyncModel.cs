
using Common.Service.DTO;
using PWMIS.EnterpriseFramework.Service.Basic;
using PWMIS.EnterpriseFramework.Service.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSyncGui
{
    class FilesSyncModel
    {
        public event EventHandler<FileUploadEventArg> OnFileUploading;
        public event EventHandler<FilesUploadResultEventArg> OnUploadFinish;
        public event EventHandler<UploadErrorEventArg> OnUploadError;

        Proxy srvProxy = new Proxy();

        private bool _isWorking;
        /// <summary>
        /// 同步工作进行中
        /// </summary>
        public bool Working {
            get {
                if (srvProxy.ServiceSubscriber!=null && srvProxy.ServiceSubscriber.Closed)
                    _isWorking = false;
                return _isWorking;
            }
            set
            {
                _isWorking = value;
            }
        }

        public void Sync(string serverHost)
        {
            Working = true;
            ServiceRequest request = new ServiceRequest();
            request.ServiceName = "FileSyncService";
            request.MethodName = "StartUploadTask";
           
           
            srvProxy.ServiceBaseUri = string.Format("net.tcp://{0}", serverHost);
            srvProxy.ErrorMessage += SrvProxy_ErrorMessage;
            srvProxy.Subscribe<PublishUploadInfo>(request, PWMIS.EnterpriseFramework.Common.DataType.Json, converter => {
                if (converter.Succeed)
                {
                    PublishUploadInfo uploadInfo = converter.Result;
                    if (uploadInfo.IsUploading)
                    {
                        if (OnFileUploading != null)
                            OnFileUploading(this, new FileUploadEventArg(uploadInfo.StateArg));
                    }
                    else
                    {
                        if (OnUploadFinish != null)
                        {
                            OnUploadFinish(this, new FileSyncGui.FilesUploadResultEventArg(uploadInfo.Result));
                            Working = false;
                            //下面2行代码必须使用，这样才可以有效关闭连接
                            srvProxy.ErrorMessage -= SrvProxy_ErrorMessage;
                            srvProxy.Close();
                        }
                    }
                }
                else
                {
                    string err = converter.ErrorMessage;
                }
            }
            );
        }

        public List<KeyValuePair<string, string>> ServiceInfo(string serverHost)
        {
            ServiceRequest request = new ServiceRequest();
            request.ServiceName = "FileSyncService";
            request.MethodName = "ServiceInfo";

            Proxy srvProxy = new Proxy();
            srvProxy.ServiceBaseUri = string.Format("net.tcp://{0}", serverHost);
            srvProxy.ErrorMessage += SrvProxy_ErrorMessage;
            return srvProxy.RequestServiceAsync<List<KeyValuePair<string, string>>>(request).Result;
        }

        private void SrvProxy_ErrorMessage(object sender, MessageSubscriber.MessageEventArgs e)
        {
            Working = false;
            if (OnUploadError!=null)
                OnUploadError(sender, new FileSyncGui.UploadErrorEventArg(e.MessageText));
        }
    }
}
