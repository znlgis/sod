//是否允许公开使用
#define NotPrivateUse

using System;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using MessagePublisher;
using MessagePublishService;
using PWMIS.EnterpriseFramework.Service.Basic;
using PWMIS.EnterpriseFramework.Service.Runtime;
using PWMIS.EnterpriseFramework.Service.Client.Model;
using PWMIS.EnterpriseFramework.Service.Group;
using System.Xml;

namespace PWMIS.EnterpriseFramework.Service.Host
{
    class Program
    {
#if(PrivateUse)
        private static readonly string UseDescrition = "当前版本仅供测试，如需要公开使用请购买许可协议";
#endif
        const string LogDirectory = "./Log/";
        private static object sync_obj = new object();
        private static DateTime CurrentDate = new DateTime(1900,1,1);
        private static TextWriter ConsoleOut;
        private static bool EnableConsoleOut = false;//是否允许输出转向
        /// <summary>
        /// 服务宿主地址
        /// </summary>
        public static ServiceHostInfo Host { get; private set; }
        /// <summary>
        /// 远程控制台监听器
        /// </summary>
        public static MessageListener RemoteConsoleListener;

        private static System.Threading.Timer CountTimer ;
        /// <summary>
        /// 监听器统计，如果服务挂机，则统计信息在1分钟后过期
        /// </summary>
        private static void ListenersCountTimer()
        {
            CountTimer = new System.Threading.Timer(new System.Threading.TimerCallback(o =>
            {
                DateTime dt = DateTime.Now;
                int[] arrCount = MessageCenter.Instance.CheckListeners();
                int currCount = arrCount[0];
                //将监听器数量写入全局缓存，供集群调度服务使用
                ICacheProvider cache = CacheProviderFactory.GetGlobalCacheProvider();
                string key = Program.Host.GetUri() + "_HostInfo";

                ServiceHostInfo serviceHostInfo = cache.Get<ServiceHostInfo>(key, () =>
                {
                    ServiceHostInfo info = new ServiceHostInfo();
                    info.RegServerDesc = Program.Host.RegServerDesc;
                    info.RegServerIP = Program.Host.RegServerIP;
                    info.RegServerPort = Program.Host.RegServerPort;
                    info.IsActive = Program.Host.IsActive;
                    info.ServerMappingIP = Program.Host.ServerMappingIP;
                    info.LogDirectory = Program.Host.LogDirectory;
                    info.ListenerCount = currCount;
                    info.ListenerMaxCount = currCount;
                    info.ListenerMaxDateTime = DateTime.Now;
                    info.ActiveConnectCount = arrCount[1];
                    return info;
                },
                new System.Runtime.Caching.CacheItemPolicy() { SlidingExpiration = new TimeSpan(0, 10, 0) }
                );

                bool changed = false;
                int maxCount = serviceHostInfo.ListenerMaxCount;

                if (currCount > maxCount)
                {
                    changed = true;
                    maxCount = currCount;
                    serviceHostInfo.ListenerCount = currCount;
                    serviceHostInfo.ListenerMaxCount = maxCount;
                    serviceHostInfo.ListenerMaxDateTime = DateTime.Now;
                }
                else if (currCount != serviceHostInfo.ListenerCount)
                {
                    changed = true;
                    serviceHostInfo.ListenerCount = currCount;
                }

                if (changed)
                {
                    serviceHostInfo.ActiveConnectCount = arrCount[1];

                    Host.ListenerCount = serviceHostInfo.ListenerCount;
                    Host.ListenerMaxCount = serviceHostInfo.ListenerMaxCount;
                    Host.ListenerMaxDateTime = serviceHostInfo.ListenerMaxDateTime;
                    Host.ActiveConnectCount = serviceHostInfo.ActiveConnectCount;

                    cache.Insert<ServiceHostInfo>(
                           key,
                           serviceHostInfo,
                           new System.Runtime.Caching.CacheItemPolicy() { SlidingExpiration = new TimeSpan(0, 1, 0) }
                           );
                }
                Console.WriteLine("=========监听器数量统计：当前{0}个,最大{1}个,用时{2} ms ============", currCount, maxCount, DateTime.Now.Subtract(dt).TotalMilliseconds);
            }), null, 1000, 10000);
        }

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            if (!System.IO.Directory.Exists(LogDirectory))
            {
                System.IO.Directory.CreateDirectory(LogDirectory);
            }
            Console.WriteLine("log ok.");
            /////////////////////////////////////////////////////////////////////////
#if(MONO)
            if (Environment.GetEnvironmentVariable("MONO_STRICT_MS_COMPLIANT") != "yes")
            {
                Environment.SetEnvironmentVariable("MONO_STRICT_MS_COMPLIANT", "yes");
                Console.WriteLine("设置环境变量“MONO_STRICT_MS_COMPLIANT”为Yes！");
            }
            else
            {
                Console.WriteLine("当前环境变量“MONO_STRICT_MS_COMPLIANT”值为Yes！");
            }
#endif

            ///////////////////////////////////////////////////////////////////////////
            //参数获取设置的服务地址，如果没有，则保留默认的 127.0.0.1:8888
            string ip = System.Configuration.ConfigurationManager.AppSettings["ServerIP"];// "127.0.0.1";
            //string ip = "127.0.0.1";
            IPAddress ipAddr;
            if (args.Length > 0 && IPAddress.TryParse(args[0], out ipAddr))
            {
                ip = ipAddr.ToString();
            }
            Console.WriteLine("ip config ok.");

            int port = int.Parse( System.Configuration.ConfigurationManager.AppSettings["ServerPort"]);// 8888;
            int tempPort;
            if (args.Length > 1 && int.TryParse(args[1], out tempPort))
            {
                port = tempPort;
            }
            if (args.Length > 2 && args[2].ToLower() == "outlog")
            {
                EnableConsoleOut = true;
            }

            Console.WriteLine("address config ok.");
            ////
           
            string uri1 = string.Format("net.tcp://{0}:{1}", ip, port-1);
            NetTcpBinding binding1 = new NetTcpBinding(SecurityMode.None);

            ServiceHost calculatorHost = new ServiceHost(typeof(CalculatorService));
            calculatorHost.AddServiceEndpoint(typeof(ICalculator), binding1, uri1);
            calculatorHost.Opened += delegate
            {
                Console.WriteLine("The Test Service(calculator) has begun to listen");
            };
            calculatorHost.Open();

            ////
            string uri = string.Format("net.tcp://{0}:{1}", ip, port);
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            //Console.WriteLine("binding init 1,ok.");

            binding.MaxBufferSize = int.MaxValue;
            //Console.WriteLine("binding init 2,ok.");

            binding.MaxReceivedMessageSize = int.MaxValue;
            //Console.WriteLine("binding init 3,ok.");
#if(MONO)
            XmlDictionaryReaderQuotas quo = new XmlDictionaryReaderQuotas();
            binding.ReaderQuotas = quo;
            Console.WriteLine("binding init 4_1,ok.");
#endif
            binding.ReaderQuotas.MaxArrayLength = 65536;
            //Console.WriteLine("binding init 4,ok.");

            binding.ReaderQuotas.MaxBytesPerRead = 10 * 1024 * 1024;
            binding.ReaderQuotas.MaxStringContentLength = 10 * 1024 * 1024; //65536;
           
            binding.ReceiveTimeout = TimeSpan.MaxValue;//设置连接自动断开的空闲时长；
            binding.MaxConnections = 100;
            binding.ListenBacklog = 200;
            //Console.WriteLine("binding init 5,ok.");
            binding.TransferMode = TransferMode.Buffered;
            //Console.WriteLine("binding init 6,ok.");
            //请参见 http://msdn.microsoft.com/zh-cn/library/ee767642 进行设置

            //Console.WriteLine("binding init ok.");
            ListAllBindingElements(binding);
           
            ServiceHost host = new ServiceHost(typeof(MessagePublishServiceImpl));
            Console.WriteLine("service config check all ok.");
            host.AddServiceEndpoint(typeof(IMessagePublishService), binding, uri);
            Console.WriteLine("=========PDF.NET.MSF (PWMIS Message Service) Ver {0} ==", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            Console.WriteLine("启动消息发布服务……接入地址：{0}", uri);
            Console.WriteLine();

            ChangeConsoleOut();
            if (EnableConsoleOut)
            {
                ListAllBindingElements(binding);
                Console.WriteLine("启动消息发布服务……接入地址：{0}", uri);
            }
            Console.WriteLine("检查服务节点... ...");

            ////////////////////向集群中写入节点 ///////////////////////////////////////////////////////////////
            //ServiceHostUri = uri;

            ServiceRegModel model = new ServiceRegModel();
            model.RegServerIP = ip;
            model.RegServerPort = port;
            model.RegServerDesc = string.Format("Server://{0}:{1}", ip, port);
            model.ServerMappingIP = System.Configuration.ConfigurationManager.AppSettings["ServerMappingIP"];

            Host = new ServiceHostInfo();
            Host.RegServerDesc = model.RegServerDesc;
            Host.RegServerIP = model.RegServerIP;
            Host.RegServerPort = model.RegServerPort;
            Host.IsActive = model.IsActive;
            Host.ServerMappingIP = model.ServerMappingIP;

            RegServiceContainer container = new RegServiceContainer();
            container.CurrentContext = new ServiceContext("");
            if (container.RegService(model))
                Console.WriteLine("======注册集群节点成功，服务将以集群模式运行==================");
            else
                Console.WriteLine("====== 未使用全局缓存，服务将以独立模式运行 ==================");

            ////////////////////////////////////////////////////////////////////////////////////////////////////////
            MessageCenter.Instance.ListenerAdded += new EventHandler<MessageListenerEventArgs>(Instance_ListenerAdded);
            MessageCenter.Instance.ListenerRemoved += new EventHandler<MessageListenerEventArgs>(Instance_ListenerRemoved);
            MessageCenter.Instance.NotifyError += new EventHandler<MessageNotifyErrorEventArgs>(Instance_NotifyError);
            MessageCenter.Instance.ListenerAcceptMessage += new EventHandler<MessageListenerEventArgs>(Instance_ListenerAcceptMessage);
            MessageCenter.Instance.ListenerEventMessage += new EventHandler<MessageListenerEventArgs>(Instance_ListenerEventMessage);
            MessageCenter.Instance.ListenerRequestMessage += new EventHandler<MessageRequestEventArgs>(Instance_ListenerRequestMessage);
            
#if(PrivateUse)
            if (ip.StartsWith("192.168.") || ip.StartsWith("127.0.0.1")) //测试，仅限于局域网使用
            {
                host.Open();

                Console.WriteLine("服务正在运行");
                EnterMessageInputMode();

                Console.WriteLine("正在关闭服务……");
                host.Close();

                Console.WriteLine("服务已关闭。");
            }
            else
            {
                Console.WriteLine("服务已关闭，{0}。", UseDescrition);
            }

#else
            host.Open();

            Console.WriteLine("服务正在运行");
            EnterMessageInputMode();

            Console.WriteLine("正在关闭服务……");
            host.Close();
            calculatorHost.Close();

            Console.WriteLine("服务已关闭。");

#endif

            host = null;
            Console.ReadLine();
        }

       

        /// <summary>
        /// 改变控制台的输出到日志文件中
        /// </summary>
        static void ChangeConsoleOut()
        {
            if (EnableConsoleOut)
            {
                if (CurrentDate != DateTime.Today)
                {
                    lock (sync_obj)
                    {
                        if (CurrentDate != DateTime.Today)
                        {
                            CurrentDate = DateTime.Today;
                            if (ConsoleOut != null)
                                ConsoleOut.Close();

                            string fileName = LogDirectory + CurrentDate.ToString("yyyy-MM-dd") + ".txt";
                            //备份日志文件
                            if (File.Exists(fileName))
                            {
                                string backFileName = LogDirectory + "back-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".txt";
                                File.Move(fileName, backFileName);
                            }

                            //StreamWriter standardOutput = new StreamWriter(Console.OpenStandardOutput(), Encoding.Default);
                            //standardOutput.AutoFlush = true;
                            //Console.SetOut(standardOutput);
                            Console.WriteLine("已经将控制台输出转向到文件：{0}", fileName);

                            ConsoleOut = File.CreateText(fileName);
                            
                            Console.SetOut(ConsoleOut);
                            ((StreamWriter)ConsoleOut).AutoFlush = true;
                            Console.WriteLine("----服务控制台 日志文件输出------");
                            ConsoleOut.Flush();
                        }
                    }
                }
                //每10秒输出一次缓冲区
                //if (DateTime.Now.Second % 10 == 0)
                //    ConsoleOut.Flush();
            }

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
                int count= MessageCenter.Instance.NotifyMessage(line);
                if (count == 0)
                    Console.WriteLine("没有客户订阅文本消息。");
                else
                    Console.WriteLine("[{0}]发送成功！订阅人数：{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),count);

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
            Console.WriteLine("[{0}]取消订阅-- From: {1}:{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), e.Listener.FromIP, e.Listener.FromPort);
        }

        static void Instance_ListenerAdded(object sender, MessageListenerEventArgs e)
        {
            Console.WriteLine("[{0}]订阅消息-- From: {1}:{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), e.Listener.FromIP, e.Listener.FromPort);
        }

        static void Instance_ListenerAcceptMessage(object sender, MessageListenerEventArgs e)
        {
            ChangeConsoleOut();

#if(PrivateUse)
            string ip= e.Listener.FromIP;
            if (ip.StartsWith("192.168.") || ip.StartsWith("127.0.0.1")) //测试，仅限于局域网使用
            {
                ip = string.Empty;
            }
            else
            {
                Console.WriteLine("错误，{0}", UseDescrition);
                return;
            }
#endif
            //下面整个处理过程应该放到一个动态实例对象的方法中,否则,多线程问题难以避免
            SubscriberInfo subInfo = new SubscriberInfo(e.Listener);
            MessageProcesser processer = new MessageProcesser(subInfo, e.Listener.FromMessage);
            processer.ServiceErrorEvent += new EventHandler<ServiceErrorEventArgs>(Processer_ServiceErrorEvent);
            //Console.WriteLine("process message begin.");
            try
            {
                processer.Process();
            }
            catch (Exception ex)
            {
                Processer_ServiceErrorEvent(processer, new ServiceErrorEventArgs(ex));
            }
            //Console.WriteLine("process message end.");
        }

        static void Instance_ListenerRequestMessage(object sender, MessageRequestEventArgs e)
        {
            MessageProcesser processer = new MessageProcesser();
            try
            {
                processer.Execute(e);
            }
            catch (Exception ex)
            {
                Processer_ServiceErrorEvent(processer, new ServiceErrorEventArgs(ex));
            }
        }

        public static void Processer_ServiceErrorEvent(object sender, ServiceErrorEventArgs e)
        {
            string text = string.Format("[{0}]处理服务的时候发生异常：{1}\r\n错误发生时的异常对象调用堆栈：\r\n{2}", 
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 
                e.ErrorMessageText,
                e.ErrorSource == null ? "NULL" : e.ErrorSource.ToString());
            ConsoleWriteSubText(text, 1000);
            WriteLogFile("ErrorLog.txt", text);
        }

        static void Instance_ListenerEventMessage(object sender, MessageListenerEventArgs e)
        {
            string text = string.Format("[{0}]监听器事件--From: {1}:{2}\r\n[{3}]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), e.Listener.FromIP, e.Listener.FromPort, e.MessageText);
            ConsoleWriteSubText(text,1000);
            WriteLogFile("ListenerEvent.txt", text);
        }

        static void ListAllBindingElements(Binding binding)
        {
            BindingElementCollection elements = binding.CreateBindingElements();
            for (int i = 0; i < elements.Count; i++)
            {
                Console.WriteLine("{0}. {1}", i + 1, elements[i].GetType().FullName);
            }
        }

        static void ConsoleWriteSubText(string text, int length)
        {
            Console.WriteLine(text.Length > length ? text.Substring(0, length)+" ...\" \r\n" : text);
        }

        /////////////////////
        static void TestService()
        {
            ServiceRequest request = new ServiceRequest();
            request.ServiceName = "User";
            request.MethodName = "Login";
            request.Parameters = new object[] { "aaa", "123" };

            Console.Write(CallService(request));
            Console.WriteLine();
            Console.WriteLine("---服务方法调用完成--");
        }


        static string CallService(ServiceRequest serviceRequest)
        {
            ServiceContext context = new ServiceContext(serviceRequest);
            serviceRequest = context.Request;
            context.ProcessService();
            return context.Response.AllText;
        }

        

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string errMsg = "程序发生未处理的异常：\r\n" + e.ExceptionObject.ToString();
            Console.WriteLine(errMsg);
            WriteLogFile("ErrorLog.txt",errMsg);
        }

        static void WriteLogFile(string fileName,string logMsg)
        {
            try
            {
                string text = string.Format("\r\n------------------------------\r\n{0}",  logMsg);
                System.IO.File.AppendAllText(LogDirectory + fileName, text);
            }
            catch
            { 
            
            }
        }
    }
}
