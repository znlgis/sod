using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace PWMIS.EnterpriseFramework.Service.Basic
{
    /// <summary>
    /// 请求模式
    /// </summary>
    public enum RequestModel
    {
        /// <summary>
        /// 服务请求模式，默认
        /// </summary>
        GetService,
        /// <summary>
        /// 消息发布模式
        /// </summary>
        Publish,
        /// <summary>
        /// 服务事件模式
        /// </summary>
        ServiceEvent
    }

    /// <summary>
    /// 服务调用请求
    /// </summary>
    public class ServiceRequest
    {
        private string _serviceUrl;
        private static string serviceUrlFlag = "Service://";
        private static string publishUrlFlag = "Publish://";

        public ServiceRequest()
        {
            this.RequestModel = Basic.RequestModel.GetService;
        }
        /// <summary>
        /// 获取或设置服务请求的模式
        /// </summary>
        public RequestModel RequestModel { get; set; }

        /// <summary>
        /// 请求的服务名
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 请求的服务中的方法名
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// 服务方法要调用的参数，只跟参数类型和参数顺序相关
        /// </summary>
        public object[] Parameters { get; set; }
        /// <summary>
        /// 获取方法的参数数组，内部使用，用于处理复杂参数对象
        /// </summary>
        public ServiceMethodParameter[] MethodParameters { get; private set; }

        /// <summary>
        /// 获取或者设置当前服务请求的地址
        /// </summary>
        public string ServiceUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_serviceUrl))
                    _serviceUrl = this.CreateServiceUrl();
                return _serviceUrl;
            }
            set
            {
                _serviceUrl = "";
                string[] args;
                RequestModel requestModel;
                if (IsServiceUrl(value, out args, out requestModel))
                {
                    this.ServiceName = args[0];
                    this.MethodName = args[1];
                    this.MethodParameters = ParameterParse.GetParameters(args[2]);
                    this.RequestModel = requestModel;
                    _serviceUrl = value;
                }
            }
        }

        /// <summary>
        /// 重新初始化服务地址，比如对请求对象重新设置参数后，应该调用此方法。
        /// </summary>
        public void ResetServiceUrl()
        {
            _serviceUrl = "";
        }

        /// <summary>
        /// 用于指示订阅数据的时候，服务方法的结果类型
        /// </summary>
        public Type ResultType { get; set; }

        /// <summary>
        /// 与此请求相关的客户端IP地址，此属性仅供服务器设置
        /// </summary>
        public string ClientIP { get; set; }
        /// <summary>
        /// 与此请求相关的客户端通信端口，此属性仅供服务器设置
        /// </summary>
        public int ClientPort { get; set; }
        /// <summary>
        /// 客户端的唯一标识，此属性仅供服务器设置
        /// </summary>
        public string ClientIdentity { get; set; }


        /// <summary>
        /// 根据当前请求对象，创建服务地址
        /// </summary>
        /// <returns></returns>
        public string CreateServiceUrl()
        {
            string paraString = ParameterParse.GetParameterString(this.Parameters);
            string[] arr = new string[] { this.ServiceName, this.MethodName, paraString };
            string strTemp = this.RequestModel == Basic.RequestModel.GetService ? serviceUrlFlag : publishUrlFlag;
            //暂时不在请求的时候由客户端指名是否需要会话，而由服务方法决定是否需要
            //string strFlag = this.SessionRequired ? "Session" : "NoSession";
            //return "["+strTemp+"]"+ strTemp + string.Join("/", arr);
            return strTemp + string.Join("/", arr);
        }

        /// <summary>
        /// 输入的是否是服务请求地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsServiceUrl(string input)
        {
            string[] args;
            RequestModel requestModel;
            return IsServiceUrl(input, out args, out requestModel);
        }

        private static bool IsServiceUrl(string input, out string[] args, out RequestModel requestModel)
        {
            requestModel = RequestModel.GetService;
            if (!string.IsNullOrEmpty(input))
            {
                string strTemp = input.StartsWith(serviceUrlFlag) ? serviceUrlFlag
                    : input.StartsWith(publishUrlFlag) ? publishUrlFlag
                    : string.Empty;

                if (!string.IsNullOrEmpty(strTemp))
                {
                    args = input.Substring(strTemp.Length).Split('/');
                    if (args.Length > 2)
                    {
                        if (strTemp == publishUrlFlag)
                            requestModel = RequestModel.Publish;
                        return true;
                    }
                }
            }
            args = null;
            return false;
        }
    }
}
