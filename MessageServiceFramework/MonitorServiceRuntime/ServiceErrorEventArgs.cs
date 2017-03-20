using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TranstarAuction.Service.Runtime
{
    public class ServiceErrorEventArgs : EventArgs
    {
        public Exception ErrorSource { get;private set; }

        public string ErrorMessageText { get; private set; }

        public ServiceErrorEventArgs(string messageText)
        {
            this.ErrorMessageText = messageText;
        }

        public ServiceErrorEventArgs(Exception ex)
        {
            this.ErrorSource = ex;
            this.ErrorMessageText = ex.Message;
        }

        public ServiceErrorEventArgs(Exception ex, string messageText)
        {
            this.ErrorSource = ex;
            this.ErrorMessageText = messageText;
        }
    }
}
