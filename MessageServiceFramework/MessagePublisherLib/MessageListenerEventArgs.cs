using System;
using System.Collections.Generic;
using System.Text;

namespace MessagePublisher
{
    public class MessageListenerEventArgs : EventArgs
    {
        public MessageListener Listener { get; private set; }

        public string MessageText { get; set; }

        public MessageListenerEventArgs(MessageListener listener)
        {
            this.Listener = listener;
        }

        public MessageListenerEventArgs(MessageListener listener, string messageText)
        {
            this.Listener = listener;
            this.MessageText = messageText;
        }
    }

    public class MessageRequestEventArgs : MessageListenerEventArgs
    {
        public MessageRequestEventArgs(MessageListener listener)
            : base(listener)
        {
            this.MessageText = listener.FromMessage;
        }
        public MessageRequestEventArgs(MessageListener listener, string messageText)
            : base(listener, messageText)
        {
            this.MessageText = listener.FromMessage;
        }
        /// <summary>
        /// 消息的处理结果
        /// </summary>
        public string ResultText { get; set; }
    }
}
