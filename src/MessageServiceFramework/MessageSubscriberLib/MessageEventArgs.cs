using System;
using System.Collections.Generic;
using System.Text;

namespace MessageSubscriber
{
    public  class MessageEventArgs : EventArgs
    {
        public string MessageText { get; set; }

        public MessageEventArgs() { 
        
        }

        public MessageEventArgs(string msgText)
        {
            this.MessageText = msgText;
        }
    }
}
