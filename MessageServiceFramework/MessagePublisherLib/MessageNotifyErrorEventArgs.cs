using System;
using System.Collections.Generic;
using System.Text;

namespace MessagePublisher
{
    public class MessageNotifyErrorEventArgs : EventArgs
    {
        public MessageListener Listener { get; private set; }

        public Exception Error { get; private set; }

        public MessageNotifyErrorEventArgs(MessageListener listener, Exception error)
        {
            this.Listener = listener;
            this.Error = error;
        }


    }
}
