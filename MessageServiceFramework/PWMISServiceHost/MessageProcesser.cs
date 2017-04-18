using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MessagePublisher;
using PWMIS.EnterpriseFramework.Service.Basic;
using PWMIS.EnterpriseFramework.Service.Runtime;
using System.IO;

namespace PWMIS.EnterpriseFramework.Service.Host
{
    class MessageContext
    {
        /// <summary>
        /// 获取当前处理的消息
        /// </summary>
        public string Message { get; private set; }
        /// <summary>
        /// 获取当前处理的订阅者信息
        /// </summary>
        public SubscriberInfo SubscriberInfo { get; private set; }

        public MessageContext(SubscriberInfo subInfo, string message)
        {
            this.Message = message;
            this.SubscriberInfo = subInfo;
        }
    }
    /// <summary>
    /// 消息处理器(状态模式)
    /// </summary>
    class MessageProcesser
    {
        private MessageProcessBase currentProcess;
        private MessageContext context;

        public event EventHandler<ServiceErrorEventArgs> ServiceErrorEvent;

        public MessageProcesser() { }

        public MessageProcesser(SubscriberInfo subInfo, string message)
        {
            this.context = new MessageContext(subInfo, message);
            this.currentProcess = new ServiceMessageProcess();
            this.currentProcess.ServiceErrorEvent += new EventHandler<ServiceErrorEventArgs>(currentProcess_ServiceErrorEvent);
        }

        void currentProcess_ServiceErrorEvent(object sender, ServiceErrorEventArgs e)
        {
            if (this.ServiceErrorEvent != null)
                this.ServiceErrorEvent(sender, e);
        }


        /// <summary>
        /// 处理用户的订阅消息
        /// </summary>
        public void Process()
        {
            this.currentProcess.Work(this.context);

            #region 注释的代码
            //string message = this.message;
            //string identity = this.subscriberInfo.Identity;
            //if (ServiceRequest.IsServiceUrl(message))
            //{
            //    string processMesssage = string.Empty;
            //    processMesssage+= string.Format("[{0}]正在处理服务请求--From: {1}:{2},Identity:{3}\r\n>>[CMID:{4}]{5}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), subscriberInfo.FromIP, subscriberInfo.FromPort, identity, subscriberInfo.MessageID, this.message);

            //    int msgId = subscriberInfo.MessageID;//执行完服务方法后，MessageID 可能被另外一个线程改变
            //    //执行服务方法的时候，由服务方法指名是否需要维持会话状态
            //    ServiceContext context = new ServiceContext(message);
            //    context.ProcessService(subscriberInfo.SessionID);

            //    string result = context.Response.AllText;
            //    processMesssage += string.Format("[{0}]请求处理完毕--To: {1}:{2},Identity:{3}\r\n>>[SMID:{4}]消息长度：{5} 字节-------", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), subscriberInfo.FromIP, subscriberInfo.FromPort, identity, subscriberInfo.MessageID, result.Length.ToString("###,###"));
            //    MessageListener currLstn = MessageCenter.Instance.GetListener(this.subscriberInfo.FromIP, this.subscriberInfo.FromPort);
            //    if (context.Request.RequestModel == RequestModel.GetService)
            //    {
            //        MessageCenter.Instance.ResponseMessage(currLstn, msgId, result);
            //    }
            //    else
            //    {
            //        //订阅模式
            //        MessageCenter.Instance.NotifyOneMessage(currLstn, msgId, result);
            //        StartPublishWorker(context);
            //        processMesssage += string.Format("[{0}]当前监听器已经加入工作线程， {1}:{2},Identity:{3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), subscriberInfo.FromIP, subscriberInfo.FromPort, subscriberInfo.Identity);
            //    }
            //    Console.WriteLine(processMesssage);
            //}
            //else
            //{
            //    MessageListener currLstn = MessageCenter.Instance.GetListener(this.subscriberInfo.FromIP, this.subscriberInfo.FromPort);
            //    Console.WriteLine("[{0}]接收消息--From: {1}:{2},Identity:{3}\r\n>>[CMID:{4}]{5}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), currLstn.FromIP, currLstn.FromPort, identity, currLstn.MessageID, currLstn.FromMessage);
            //    if (message.StartsWith("[SROK]"))//客户端要求服务端确认
            //    {
            //        MessageCenter.Instance.NotifyOneMessage(currLstn, currLstn.MessageID, "收到！");
            //    }
            //    else
            //    {
            //        //如果Message的内容是某些命令，则执行响应的处理。
            //        if (message.StartsWith("[CMD]"))
            //        {
            //            //取消订阅的服务方法
            //            if (message.Substring("[CMD]".Length) == "DeSubscribeService")
            //            {
            //                SubscriberInfo subInfo = new SubscriberInfo(currLstn, currLstn.MessageID);
            //                SubscriberInfo objSubInfo;
            //                ServicePublisher publisher = PublisherFactory.Instance.Find(subInfo, out objSubInfo);
            //                if (publisher != null)
            //                    publisher.DeSubscribe(objSubInfo);
            //            }
            //        }

            //    }//end if
            //}//end if

            #endregion

        }//end sub

        /// <summary>
        /// 处理用户的请求消息
        /// </summary>
        /// <param name="e"></param>
        public void Execute(MessageRequestEventArgs e)
        {
            string message = e.MessageText;
            if (ServiceRequest.IsServiceUrl(message))
            {
                string identity = e.Listener.Identity;

                string processMesssage = string.Empty;
                DateTime beginTime = DateTime.Now;
                processMesssage = string.Format("[{0}]正在处理服务请求--From: {1}:{2},Identity:{3}\r\n>>[RMID:{4}]{5}", 
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), 
                    e.Listener.FromIP, e.Listener.FromPort, identity, e.Listener.MessageID, 
                    message.Length > 256 ? message.Substring(0, 256) : message);
                Console.WriteLine(processMesssage);

                int msgId = e.Listener.MessageID;
                //执行服务方法的时候，由服务方法指名是否需要维持会话状态
                ServiceContext context = new ServiceContext(message);
                context.ServiceErrorEvent += new EventHandler<ServiceErrorEventArgs>(currentProcess_ServiceErrorEvent);
                context.Request.ClientIP = e.Listener.FromIP;
                context.Request.ClientPort = e.Listener.FromPort;
                context.Request.ClientIdentity = identity;

                context.ProcessService(e.Listener.SessionID);

                string result = context.Response.AllText;
                bool noResult = context.NoResultRecord(result);
                DateTime endTime = DateTime.Now;
                processMesssage = string.Format("[{0}]请求处理完毕({1}ms)--To: {2}:{3},Identity:{4}\r\n>>[RMID:{5}]消息长度：{6} -------", 
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), 
                    endTime.Subtract(beginTime).TotalMilliseconds,
                    e.Listener.FromIP, e.Listener.FromPort, identity, e.Listener.MessageID, 
                    noResult ? "[Empty Result]" : result.Length.ToString("###,###") + "字节");
                Console.WriteLine(processMesssage);

                e.ResultText = result;
                //此处内容可能很大，不能全程输出
                if (context.Response.ResultType == typeof(byte[]))
                    Console.WriteLine("[byte Content]");
                else
                    Console.WriteLine("result:{0}", result.Length > 100 ? result.Substring(0, 100) + " ..." : result);
            }
            else
            {
                e.ResultText = "OK";
            }
        }
    }//end class

    /// <summary>
    /// 消息处理抽象类
    /// </summary>
    abstract class MessageProcessBase
    {
        private void SetState(MessageContext context)
        {
            this.Message = context.Message;
            this.SubscriberInfo = context.SubscriberInfo;
        }

        /// <summary>
        /// 获取当前处理的消息
        /// </summary>
        protected string Message { get; private set; }
        /// <summary>
        /// 获取当前处理的订阅者信息
        /// </summary>
        protected SubscriberInfo SubscriberInfo { get; private set; }

        /// <summary>
        /// 消息是否是当前类型
        /// </summary>
        /// <returns></returns>
        protected abstract bool IsCurrentType();
        /// <summary>
        /// 处理消息
        /// </summary>
        protected abstract void Process();
        /// <summary>
        /// 获取下一个消息处理对象实例
        /// </summary>
        /// <returns></returns>
        protected abstract MessageProcessBase GetNext();

        /// <summary>
        /// 判断当前消息是否属于自己处理的类型,如果是,执行消息处理,如果不是,调用下一个消息处理类
        /// </summary>
        public void Work(MessageContext context)
        {
            SetState(context);

            if (this.IsCurrentType())
            {
                Process();
            }
            else
            {
                MessageProcessBase nextProcess = GetNext();
                if (nextProcess != null)
                    nextProcess.Work(context);
            }
        }

        public event EventHandler<ServiceErrorEventArgs> ServiceErrorEvent;

        protected void OnServiceError(object sender, ServiceErrorEventArgs args)
        {
            if (this.ServiceErrorEvent != null)
                this.ServiceErrorEvent(sender, args);
        }

    }

    /// <summary>
    /// 处理服务消息
    /// </summary>
    class ServiceMessageProcess : MessageProcessBase
    {
        protected override bool IsCurrentType()
        {
            return ServiceRequest.IsServiceUrl(this.Message);
        }

        protected override void Process()
        {
            string message = this.Message;
            string identity = this.SubscriberInfo.Identity;

            string processMesssage = string.Empty;
            DateTime beginTime = DateTime.Now;
            processMesssage = string.Format("[{0}]正在处理服务请求--From: {1}:{2},Identity:{3}\r\n>>[PMID:{4}]{5}", 
                beginTime.ToString("yyyy-MM-dd HH:mm:ss.fff"), 
                this.SubscriberInfo.FromIP, this.SubscriberInfo.FromPort, identity, 
                this.SubscriberInfo.MessageID, 
                this.Message.Length > 256 ? this.Message.Substring(0, 256) : this.Message);
            Console.WriteLine(processMesssage);

            int msgId = this.SubscriberInfo.MessageID;//执行完服务方法后，MessageID 可能被另外一个线程改变
            //执行服务方法的时候，由服务方法指名是否需要维持会话状态
            ServiceContext context = new ServiceContext(message);
            context.Host = Program.Host;
            context.ServiceErrorEvent += new EventHandler<ServiceErrorEventArgs>(context_ServiceErrorEvent);
            context.Request.ClientIP = this.SubscriberInfo.FromIP;
            context.Request.ClientPort = this.SubscriberInfo.FromPort;
            context.Request.ClientIdentity = this.SubscriberInfo.Identity;
            context.GetMessageFun = strPara =>
            {
                MessageListener currLst = MessageCenter.Instance.GetListener(this.SubscriberInfo.FromIP, this.SubscriberInfo.FromPort);
                return currLst.CallBackFunction(msgId, strPara);
            };

            context.PreGetMessageFun = strPara =>
            {
                MessageListener currLst = MessageCenter.Instance.GetListener(this.SubscriberInfo.FromIP, this.SubscriberInfo.FromPort);
                return currLst.PreCallBackFunction(msgId, strPara);
            };

            string result = string.Empty;
            bool noResult = false;
            if (!context.HasError)
            {
                //Console.WriteLine("Process Service begin...");
                context.ProcessService(this.SubscriberInfo.SessionID);
                //Console.WriteLine("Process Service ok...");
                result = context.Response.AllText;
                noResult = context.NoResultRecord(result);
            }
            else
            {
                result = ServiceConst.CreateServiceErrorMessage(context.ErrorMessage);
            }

            DateTime endTime = DateTime.Now;
            processMesssage = string.Format("[{0}]请求处理完毕({1}ms)--To: {2}:{3},Identity:{4}\r\n>>[PMID:{5}]消息长度：{6} -------",
                   endTime.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                   endTime.Subtract(beginTime).TotalMilliseconds,
                   this.SubscriberInfo.FromIP, this.SubscriberInfo.FromPort, identity,
                   this.SubscriberInfo.MessageID, 
                   noResult ? "[Empty Result]" : result.Length.ToString("###,###") + "字节");
            Console.WriteLine(processMesssage);
            //此处内容可能很大，不能全程输出
            if (context.Response.ResultType == typeof(byte[]))
                Console.WriteLine("[byte Content]");
            else
                Console.WriteLine("result:{0}", result.Length > 100 ? result.Substring(0, 100) + " ..." : result);

            MessageListener currLstn = MessageCenter.Instance.GetListener(this.SubscriberInfo.FromIP, this.SubscriberInfo.FromPort);
            if (currLstn == null)
            {
                processMesssage = "Error:监听器未找到，已取消发送消息。请求源：" + this.SubscriberInfo.Message;
                Console.WriteLine(processMesssage);
                return;
            }
            if (context.Request.RequestModel == RequestModel.GetService)
            {
                //对于请求-响应模式，处理完服务以后，始终会回调客户端的方法（如果提供的话）
                if (MessageCenter.Instance.ResponseMessage(currLstn, msgId, result))
                    Console.WriteLine("Reponse Message OK.");
            }
            else
            {
                //订阅模式，仅在服务处理有结果的情况下，才给客户端发布数据。

                if (!noResult)
                {
                    if (MessageCenter.Instance.NotifyOneMessage(currLstn, msgId, result))
                        Console.WriteLine("Publish Message OK.");
                    else
                        Console.WriteLine("Publish no result.");
                }
                if (!context.HasError)
                {
                    StartPublishWorker(context);//把Host传递进去
                    processMesssage = string.Format("\r\n[{0}]当前监听器已经加入工作线程， {1}:{2},Identity:{3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), this.SubscriberInfo.FromIP, this.SubscriberInfo.FromPort, this.SubscriberInfo.Identity);
                    Console.WriteLine(processMesssage);
                }
            }
           
        }

        void context_ServiceErrorEvent(object sender, ServiceErrorEventArgs e)
        {
            base.OnServiceError(sender, e);
        }

        protected override MessageProcessBase GetNext()
        {
            return new TextMessageProcess();
        }

        /// <summary>
        /// 为当前监听器开启一个工作线程，不断处理请求，并将处理结果送给监听器
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="messageId"></param>
        /// <param name="context"></param>
        private void StartPublishWorker(ServiceContext context)
        {
            //ServiceRequest request = context.Request;
            //request.ClientIP = this.SubscriberInfo.FromIP;
            //request.ClientPort = this.SubscriberInfo.FromPort;
            //request.ClientIdentity = this.SubscriberInfo.Identity;
            this.SubscriberInfo.Request = context.Request;

            ServicePublisher publisher = PublisherFactory.Instance.GetPublisher(context);
            if (publisher.SubscriberInfoList.Count == 0)
            {
                publisher.PublisherErrorEvent += new EventHandler<ServiceErrorEventArgs>(publisher_PublisherErrorEvent);
            }
            publisher.Host = context.Host;
            publisher.ParallelExecute = context.ParallelExecute;
            publisher.BatchInterval = context.BatchInterval;

            publisher.SubscriberInfoList.Add(this.SubscriberInfo);//监听器可能存在端口复用的情况，因此需要及时释放SubscriberInfoList 的元素
            publisher.StartWork(context.Request.RequestModel== RequestModel.ServiceEvent);
        }

        void publisher_PublisherErrorEvent(object sender, ServiceErrorEventArgs e)
        {
            base.OnServiceError(sender, e);
        }

    }

    /// <summary>
    /// 处理带确认回复的文本消息
    /// </summary>
    class TextMessageProcess : MessageProcessBase
    {
        protected override bool IsCurrentType()
        {
            return this.Message.StartsWith("[SROK]");
        }

        protected override void Process()
        {
            MessageListener currLstn = MessageCenter.Instance.GetListener(this.SubscriberInfo.FromIP, this.SubscriberInfo.FromPort);
            Console.WriteLine("[{0}]接收消息--From: {1}:{2},Identity:{3}\r\n>>[CMID:{4}]{5}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), currLstn.FromIP, currLstn.FromPort, this.SubscriberInfo.Identity, currLstn.MessageID, currLstn.FromMessage);
            MessageCenter.Instance.NotifyOneMessage(currLstn, currLstn.MessageID, "收到！");
        }

        protected override MessageProcessBase GetNext()
        {
            return new CommandMessageProcess();
        }
    }

    /// <summary>
    /// 处理命令消息
    /// </summary>
    class CommandMessageProcess : MessageProcessBase
    {
        protected override bool IsCurrentType()
        {
            return this.Message.StartsWith("[CMD]");
        }

        protected override void Process()
        {
            if (this.Message.StartsWith("[CMD]"))
            {
                string cmdString = this.Message.Substring("[CMD]".Length);
                switch (cmdString)
                {
                    case "DeSubscribeService":
                        SubscriberInfo objSubInfo;
                        ServicePublisher publisher = PublisherFactory.Instance.Find(this.SubscriberInfo, out objSubInfo);
                        if (publisher != null)
                            publisher.DeSubscribe(objSubInfo);

                        break;
                    case "UpdateServiceHost":
                        string tempPath = System.IO.Path.GetTempPath();
                        DirectoryInfo info = new DirectoryInfo(tempPath);
                        string servicesTempFolder = Path.Combine(info.FullName, "_TartGetTempFolder_Services");

                        Console.WriteLine("接收到[服务升级]命令，准备调用批处理程序...");

                        System.Diagnostics.Process.Start(".\\UpdateService.bat", servicesTempFolder);
                        Console.WriteLine("批处理程序启动成功，主程序即将退出。");
                        Console.Out.Flush();
                        System.Environment.Exit(0);
                        break;
                    case "RemoteConsoleOutput"://远程控制台输出
                        MessageListener currLstn = MessageCenter.Instance.GetListener(this.SubscriberInfo.FromIP, this.SubscriberInfo.FromPort);
                        Program.RemoteConsoleListener = currLstn;
                        break;
                }
            }
            ////取消订阅的服务方法
            //if (this.Message.Substring("[CMD]".Length) == "DeSubscribeService")
            //{
            //    SubscriberInfo objSubInfo;
            //    ServicePublisher publisher = PublisherFactory.Instance.Find(this.SubscriberInfo, out objSubInfo);
            //    if (publisher != null)
            //        publisher.DeSubscribe(objSubInfo);
            //}
            ////UpdateServiceHost 升级服务器

        }

        protected override MessageProcessBase GetNext()
        {
            return null;
        }
    }

}//end namespace

