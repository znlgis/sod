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
        private SubscriberInfo()
        { }

        public SubscriberInfo(MessageListener listener)
        {
            //this.FromIP = listener.FromIP;
            //this.FromPort = listener.FromPort;
            //this.Identity = listener.Identity;
            this.MessageID = listener.MessageID;
            //this.SessionID = listener.SessionID;
            //this.Message = listener.FromMessage;
            this._innerListener = listener;
        }

        public SubscriberInfo(MessageListener listener, int messageId)
        {
            //this.FromIP = listener.FromIP;
            //this.FromPort = listener.FromPort;
            //this.Identity = listener.Identity;
            this.MessageID = messageId;
            //this.SessionID = listener.SessionID;
            this._innerListener = listener;
        }

        public string FromIP {
            get { return _innerListener.FromIP; }
        }
        public int FromPort {
            get { return _innerListener.FromPort; }
        }
        public string Identity {
            get { return _innerListener.Identity; }
        }
        public string SessionID {
            get { return _innerListener.SessionID; }
        }
        public int MessageID { get; set; }
        public string Message {
            get { return _innerListener.FromMessage; }
        }
        public ServiceRequest Request { get; set; }

        public MessageListener _innerListener { get; private set; }

    }
}
