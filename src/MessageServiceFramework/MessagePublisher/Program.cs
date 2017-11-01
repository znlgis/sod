using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using MessagePublishService;
using System.Net;

namespace MessagePublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            //参数获取设置的服务地址，如果没有，则保留默认的 127.0.0.1:8888
            string ip = "127.0.0.1";
            IPAddress ipAddr;
            if (args.Length > 0 && IPAddress.TryParse(args[0], out ipAddr))
            {
                ip = ipAddr.ToString();
            }
            int port = 8888;
            int tempPort;
            if (args.Length > 1 && int.TryParse(args[1], out tempPort))
            {
                port = tempPort;
            }
            string uri = string.Format("net.tcp://{0}:{1}", ip, port);
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.Transport);
            binding.MaxBufferSize = int.MaxValue;
            binding.ReceiveTimeout = TimeSpan.MaxValue;//设置连接自动断开的空闲时长；
            ServiceHost host = new ServiceHost(typeof(MessagePublishServiceImpl));
            host.AddServiceEndpoint(typeof(IMessagePublishService), binding, uri);

            Console.WriteLine("启动消息发布服务……接入地址：{0}", uri);

            MessageCenter.Instance.ListenerAdded += new EventHandler<MessageListenerEventArgs>(Instance_ListenerAdded);
            MessageCenter.Instance.ListenerRemoved += new EventHandler<MessageListenerEventArgs>(Instance_ListenerRemoved);
            MessageCenter.Instance.NotifyError += new EventHandler<MessageNotifyErrorEventArgs>(Instance_NotifyError);
            MessageCenter.Instance.ListenerAcceptMessage += new EventHandler<MessageListenerEventArgs>(Instance_ListenerAcceptMessage);
            MessageCenter.Instance.ListenerEventMessage += new EventHandler<MessageListenerEventArgs>(Instance_ListenerEventMessage);
            host.Open();

            Console.WriteLine("服务正在运行");
            EnterMessageInputMode();

            Console.WriteLine("正在关闭服务……");
            host.Close();

            Console.WriteLine("服务已关闭。");
            host = null;
            Console.ReadLine();
        }

       

      

        /// <summary>
        /// 消息发送模式；
        /// </summary>
        static void EnterMessageInputMode()
        {
            Console.WriteLine("请输入要发送的消息，按回车键发送。输入 @exit 回车结束操作并关闭服务。");
            string line;
            do
            {
                Console.Write(">>");
                line = Console.ReadLine();
                if ("@exit" == line.Trim())
                {
                    break;
                }
                if (string.Empty == line.Trim())
                {
                    Console.WriteLine("[{0}]不能发送空消息！", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    continue;
                }
                MessageCenter.Instance.NotifyMessage(line);
                Console.WriteLine("[{0}]发送成功！", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            } while (true);
        }

        static void Instance_NotifyError(object sender, MessageNotifyErrorEventArgs e)
        {
            Console.WriteLine("[{0}]消息发送失败！--IP:{1}; Port:{2}; Error:{3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), e.Listener.FromIP, e.Listener.FromPort, e.Error.Message);
            Console.WriteLine("移除无效监听器……");
            MessageCenter.Instance.RemoveListener(e.Listener);
        }

        static void Instance_ListenerRemoved(object sender, MessageListenerEventArgs e)
        {
            Console.WriteLine("[{0}]取消订阅-- From: {1}:{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), e.Listener.FromIP, e.Listener.FromPort);
        }

        static void Instance_ListenerAdded(object sender, MessageListenerEventArgs e)
        {
            Console.WriteLine("[{0}]订阅消息--From: {1}:{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), e.Listener.FromIP, e.Listener.FromPort);
        }

        static void Instance_ListenerAcceptMessage(object sender, MessageListenerEventArgs e)
        {
            Console.WriteLine("[{0}]接收消息--From: {1}:{2},Identity:{3}\r\n>>{4}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), e.Listener.FromIP,e.Listener.FromPort,  e.Listener.GetIdentity(),e.Listener.FromMessage);
            e.Listener.Notify(0,"收到！");
        }

        static void Instance_ListenerEventMessage(object sender, MessageListenerEventArgs e)
        {
            Console.WriteLine("[{0}]监听器事件--From: {1}:{2}\r\n[{3}]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), e.Listener.FromIP, e.Listener.FromPort, e.MessageText);
        }
    }
}
