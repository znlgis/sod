using System;
using System.Collections.Generic;
 
using MessageSubscriber;

using TranstarAuction.Model;
using System.IO;
using PWMIS.EnterpriseFramework.Service.Client;

namespace TranstarAuction.Presenters
{
    /// <summary>
    /// UI主持人基础类
    /// </summary>
    public class PresenterBase : IPresenter
    {
        /// <summary>
        /// 服务代理
        /// </summary>
        public Proxy ServiceProxy { get; private set; }

        /// <summary>
        /// 当前系统的用户
        /// </summary>
        public static UserModel CurrentUser { get; protected set; }

        static string _ServiceUri = string.Empty;
        /// <summary>
        /// 服务的基础地址
        /// </summary>
        public static string ServiceUri
        {
            get
            {
                if (string.IsNullOrEmpty(_ServiceUri))
                    _ServiceUri = System.Configuration.ConfigurationManager.AppSettings["ServiceUri"];
                return _ServiceUri;
            }
            set
            {
                _ServiceUri = value;
            }
        }

        public PresenterBase()
        {
            ServiceProxy = new Proxy();
            ServiceProxy.ServiceBaseUri = PresenterBase.ServiceUri;//System.Configuration.ConfigurationManager.AppSettings["ServiceUri"];
            ServiceProxy.ErrorMessage += new EventHandler<MessageEventArgs>(ServiceProxy_ErrorMessage);
        }

        private  void ServiceProxy_ErrorMessage(object sender, MessageEventArgs e)
        {
            OnServiceProxyError(e.MessageText);
        }

        /// <summary>
        /// 服务代理访问服务出现错误的时候的自定义处理，例如显示错误信息等，需要继承的类处理。
        /// </summary>
        /// <param name="errorMessage"></param>
        protected virtual void OnServiceProxyError(string errorMessage)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "优信拍");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string fileName = Path.Combine(path, "ErrorLog.txt");

            WriteLogFile(fileName, "ServiceProxyError：\r\n" + errorMessage);
        }

        void WriteLogFile(string fileName, string logMsg)
        {
            try
            {
                string text = string.Format("\r\n[{0}] {1}\r\n", DateTime.Now.ToString(), logMsg);
                System.IO.File.AppendAllText(fileName, text);
            }
            catch
            {

            }
        }
    }
}
