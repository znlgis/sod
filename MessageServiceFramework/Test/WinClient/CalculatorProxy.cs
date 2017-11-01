using PWMIS.EnterpriseFramework.Service.Basic;
using PWMIS.EnterpriseFramework.Service.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinClient
{
    class CalculatorProxy
    {
        Proxy serviceProxy = new Proxy();
        const string SERVICE_NAME = "Calculator";
        public CalculatorProxy(string baseUri)
        {
            serviceProxy.ErrorMessage += new EventHandler<MessageSubscriber.MessageEventArgs>(sessionProxy_ErrorMessage);
            serviceProxy.ServiceBaseUri = baseUri;
        }

        private ServiceRequest CreateRequest(string methodName,object[] paras)
        {
            ServiceRequest request = new ServiceRequest();
            request.ServiceName = SERVICE_NAME;
            request.MethodName = methodName;
            request.Parameters = paras;
            return request;
        }

        public Task<int> Add(int a, int b)
        {
            ServiceRequest request = CreateRequest("Add", new object[] { a, b });
            return serviceProxy.RequestServiceAsync<int>(request);
        }

        public  Task<int> Sub(int a, int b)
        {
            ServiceRequest request = CreateRequest("Sub", new object[] { a, b });
            return serviceProxy.RequestServiceAsync<int>(request);
        }

        void sessionProxy_ErrorMessage(object sender, MessageSubscriber.MessageEventArgs e)
        {
           
        }
    }
}
