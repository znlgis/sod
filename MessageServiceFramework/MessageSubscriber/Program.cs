using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace MessageSubscriber
{
    class Program
    {
        /// <summary>
        /// 客户端入口；
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string serviceUri = System.Configuration.ConfigurationManager.AppSettings["ServiceUri"];
            //string serviceUri= "net.tcp://127.0.0.1:8888"; //net.tcp://192.168.50.47:8888
            if (args.Length > 0)
            {
                serviceUri = args[0];
            }
            Console.WriteLine("[{0}]连接服务…… {1}", DateTime.Now.ToString("yy-MM-dd HH:mm:ss"), serviceUri);
            try
            {
                using (Subscriber sub = new Subscriber(serviceUri))
                {
                    sub.PublishingMessage += new EventHandler<MessageEventArgs>(sub_ReceivingMessage);
                    sub.Subscribe("","");
                    if (!sub.Closed)
                    {
                        Console.WriteLine("[{0}]连接成功！", DateTime.Now.ToString("yy-MM-dd HH:mm:ss"));
                        Console.WriteLine("输入 @exit 断开连接。");

                        string line;
                        do
                        {
                            Console.Write(">>");
                            line = Console.ReadLine();
                            if ("@exit" == line.Trim())
                            {
                                Console.WriteLine("[{0}]准备断开连接……", DateTime.Now.ToString("yy-MM-dd HH:mm:ss"));
                                break;
                            }
                            sub.SendMessage(line);
                            if (sub.Closed)
                            {
                                Console.WriteLine("[{0}]连接已经关闭！", DateTime.Now.ToString("yy-MM-dd HH:mm:ss"));
                                break;
                            }
                        } while (!sub.Closed);
                    }
                    else
                    {
                        Console.WriteLine("[{0}]连接已经关闭！", DateTime.Now.ToString("yy-MM-dd HH:mm:ss"));
                    }

                }//end using
            }
            catch (Exception ex)
            {
                Console.WriteLine("[{0}]发生错误！--{1}", DateTime.Now.ToString("yy-MM-dd HH:mm:ss"), ex.Message);
            }
            Console.WriteLine("[{0}]断开连接！", DateTime.Now.ToString("yy-MM-dd HH:mm:ss"));
            Console.ReadLine();
        }

        static void sub_ReceivingMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine("[{0}]收到消息：{1}", DateTime.Now.ToString("yy-MM-dd HH:mm:ss"), e.MessageText);
        }
    }
}
