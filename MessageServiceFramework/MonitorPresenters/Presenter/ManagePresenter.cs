using System;
using System.Collections.Generic;
 

using System.IO;
using PWMIS.EnterpriseFramework.Service.Basic;
using PWMIS.EnterpriseFramework.Common;
using PWMIS.EnterpriseFramework.Service.Client.Model;

namespace TranstarAuction.Presenters.Presenter
{
    public class ManagePresenter : PresenterBase
    {
        public void Close()
        {
            base.ServiceProxy.Close();
        }

        public void RequestManageConnect(Action<bool> action, string identityname)
        {
            ServiceRequest request = new ServiceRequest();
            request.ServiceName = "ManageService";
            request.MethodName = "Connect";
            request.Parameters = new object[] { identityname };

            base.ServiceProxy.RequestService<bool>(request, DataType.Text, action);
        }

        public void RequestManagePermission(Action<bool> action, string password)
        {
            ServiceRequest request = new ServiceRequest();
            request.ServiceName = "ManageService";
            request.MethodName = "IsManager";
            request.Parameters = new object[] { password };

            base.ServiceProxy.RequestService<bool>(request, DataType.Text, action);
        }

        public void RequestChangePassword(Action<bool> action, string oldpwd, string newpwd)
        {
            ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "ManageService";
            request.MethodName = "ChangePassword";
            request.Parameters = new object[] { oldpwd, newpwd };

            base.ServiceProxy.RequestService<bool>(request, DataType.Text, action);
        }

        public void RequestLogFileText(Action<string> action, string fileName,int beginRow, int endRow)
        {
            ServiceRequest request = new ServiceRequest();
            request.ServiceName = "ManageService";
            request.MethodName = "GetLogFileText";
            request.Parameters = new object[] { fileName,beginRow, endRow };

            base.ServiceProxy.RequestService<string>(request,  DataType.Text, action);
        }

        public void DownloadLogFile(Action<byte[]> action, string fileName)
        {
            ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "ManageService";
            request.MethodName = "CurReadFile";
            request.Parameters = new object[] { fileName };
            var converter=  base.ServiceProxy.GetServiceMessage<byte[]>(request,  DataType.Binary);
            if (converter.Succeed)
            {
                action(converter.Result);
            }
        }

        public void DownloadLargeFile(string serverFileName,string localFileName,Action<double> writing, Action<long> writeComplex)
        {
            ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "ManageService";
            request.MethodName = "ReadLargeFile";
            request.Parameters = new object[] { serverFileName };

            if (File.Exists(localFileName))
                File.Delete(localFileName);
            FileStream fs= File.Create(localFileName);
            long writedLen = 0;
            long allLen = 0;

            base.ServiceProxy.RequestService<long, byte[], int, long, bool>(request.ServiceUrl,  DataType.Text,
                position =>
                {
                    fs.Close();
                    //length==0 为文件有异常
                    writeComplex(position);
                },
                fileBytes =>
                {
                    int len = fileBytes.Length;
                    fs.Write(fileBytes, 0, len);
                    writedLen += len;
                    double process = (double)writedLen * 100 / allLen;
                    writing(process);
                    return fileBytes.Length;
                },
                fileLength =>
                {
                    allLen = fileLength;
                    return true;
                }
             );
            
        }

        public void RequestLogFileList(Action<List<ServiceFileInfo>> action)
        {
            ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "ManageService";
            request.MethodName = "GetAllLogFiles";
            request.Parameters = new object[] {  };
            base.ServiceProxy.RequestService<List<ServiceFileInfo>>(request,  DataType.Json, action);
        }

        /// <summary>
        /// 异步请求获取日志文件的行数
        /// </summary>
        /// <param name="action"></param>
        /// <param name="fileName"></param>
        public void RequestLogFileRowsCount(Action<int> action, string fileName)
        {
            ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "ManageService";
            request.MethodName = "GetLogFileRowsCount";
            request.Parameters = new object[] { fileName };
            base.ServiceProxy.RequestService<int>(request,  DataType.Json, action);
        }

         /// <summary>
        /// 获取当前系统目录下的文件信息(子目录以dir 表示)
        /// </summary>
        /// <param name="directory">相对目录</param>
        /// <returns>文件信息</returns>
        public List<ServiceFileInfo> GetDirectoryFiles(string directory)
        {
            ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "ManageService";
            request.MethodName = "GetDirectoryFiles";
            request.Parameters = new object[] { directory };

            var converter= base.ServiceProxy.GetServiceMessage<List<ServiceFileInfo>>(request, DataType.Json);
            return converter.Result;
        }
    }
}
