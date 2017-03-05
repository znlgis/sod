using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MessagePublisher;
using PWMIS.EnterpriseFramework.Service.Basic;

namespace PWMIS.EnterpriseFramework.Service.Host
{
    /// <summary>
    /// 订阅者信息
    /// </summary>
    public class SubscriberInfo
    {
        public SubscriberInfo()
        { }

        public SubscriberInfo(MessageListener listener)
        {
            this.FromIP = listener.FromIP;
            this.FromPort = listener.FromPort;
            this.Identity = listener.Identity;
            this.MessageID = listener.MessageID;
            this.SessionID = listener.SessionID;
            this.Message = listener.FromMessage;
        }

        public SubscriberInfo(MessageListener listener, int messageId)
        {
            this.FromIP = listener.FromIP;
            this.FromPort = listener.FromPort;
            this.Identity = listener.Identity;
            this.MessageID = messageId;
            this.SessionID = listener.SessionID;
        }

        public string FromIP { get; set; }
        public int FromPort { get; set; }
        public string Identity { get; set; }
        public string SessionID { get; set; }
        public int MessageID { get; set; }
        public string Message { get; set; }
        public ServiceRequest Request { get; set; }
    }
}
