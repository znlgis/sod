/*
 * 服务发布对象管理
 * 
 * 系统可以发布多个服务,每当客户端订阅一个服务的时候,首先检查"服务发布商"工厂中是否有此服务正在发布,
 * 如果没有,则创建一个,并放入发布工厂。对于相同的服务，只需发布一次，即可供多个客户端订阅。
 * 注：“相同的服务”，不仅仅取决于相同的服务类名称和服务方法名称，还包括相同的参数值。
 * 
 * 本功能采用“享元模式”设计。
 * 
 * 注意：以后需要修改程序，ServicePublisher应该记录所有的 ServiceContext 对象
 */ 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MessagePublisher;
using PWMIS.EnterpriseFramework.Service.Basic;
using PWMIS.EnterpriseFramework.Service.Runtime;
using PWMIS.EnterpriseFramework.Common.Encrypt;
using PWMIS.EnterpriseFramework.Service.Client.Model;
using System.Threading.Tasks;

namespace PWMIS.EnterpriseFramework.Service.Host
{
    abstract class ServicePublisher
    {
        private bool isRunning;
        private Thread thread;
        private int batchIndex = 0;//执行的批次顺序号

        AutoResetEvent resetEvent = new AutoResetEvent(false);
        /// <summary>
        /// 宿主信息
        /// </summary>
        public ServiceHostInfo Host { get; set; }
        /// <summary>
        /// 当前任务名称
        /// </summary>
        public string TaskName { get; private set; }
        /// <summary>
        /// 指定服务是否可以并发执行,默认为并行
        /// </summary>
        public bool ParallelExecute { get; set; }
        /// <summary>
        /// 每一批次的执行间隔时间，单位是毫秒，如果小于等于零，则不执行等待。默认为1秒
        /// </summary>
        public int BatchInterval { get; set; }
        /// <summary>
        /// 发布操作时候的错误事件
        /// </summary>
        public event EventHandler<ServiceErrorEventArgs> PublisherErrorEvent;
        /// <summary>
        /// 以一个任务名称初始化本类
        /// </summary>
        /// <param name="taskName"></param>
        public ServicePublisher(string taskName)
        {
            this.TaskName = taskName;
            this.BatchInterval = 1000;
        }

        protected string GetShortTaskName(int length)
        {
            string name = this.TaskName;
            return name.Length > length ? name.Substring(0, length)+"...(lenth:"+ name.Length+")" : name;
        }

        /// <summary>
        /// 设置等待事件的状态为终止，允许线程继续执行
        /// </summary>
        protected void SetPublishEvent()
        {
            resetEvent.Set();
            System.Threading.Thread.Sleep(0);
        }

        /// <summary>
        /// 订阅者信息列表
        /// </summary>
        public List<SubscriberInfo> SubscriberInfoList = new List<SubscriberInfo>();
        /// <summary>
        /// 开始服务发布工作，如工作线程未启动，则新启动线程，否则加入当前工作队列。
        /// </summary>
        public void StartWork(bool isEvent)
        {
            //当前线程属于消息订阅线程，不可阻塞
            if (!isRunning)
            {
                isRunning = true;
                if (isEvent)
                    thread = new Thread(new ThreadStart(DoEvent));
                else
                    thread = new Thread(new ThreadStart(DoWork));
                thread.Name = this.TaskName;
                thread.Start();
                Console.WriteLine(">>已经开启发布线程！");
            }
            else
            {
                Console.WriteLine(">>发布线程运行中... ...");
            }
        }

        /// <summary>
        /// 取消注册（但不关闭当前连接）
        /// </summary>
        /// <param name="subInfo"></param>
        public void DeSubscribe(SubscriberInfo subInfo)
        {
            if (SubscriberInfoList.Contains(subInfo))
                SubscriberInfoList.Remove(subInfo);
        }

        protected abstract void Publish(ref string workMessage);

        MessageListener[] GetListeners()
        {
            List<MessageListener> list = new List<MessageListener>();
            foreach (SubscriberInfo info in this.SubscriberInfoList.ToArray())
            {
                MessageListener currLst = MessageCenter.Instance.GetListener(info.FromIP, info.FromPort);
                if (currLst != null)
                    list.Add(currLst);
                else
                    this.SubscriberInfoList.Remove(info);
            }
            return list.ToArray();
        }

        /// <summary>
        /// 轮询工作模式
        /// </summary>
        void DoWork()
        {
            do
            {
                batchIndex++;
                if (BatchInterval > 0)
                    Thread.Sleep(BatchInterval);
                string workMessage = "\r\n----publisher do ------------------\r\n";
                string strTemp = "";

                int count = GetListeners().Length;
                if (count == 0)
                {
                    Console.WriteLine("\r\n[{0}]当前任务已经没有监听器，工作线程退出--Task Name: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), this.GetShortTaskName(255));
                    batchIndex = 0;
                    break;
                }
                else
                {
                    strTemp = string.Format("\r\n[{0}]当前工作线程有{1}个相关的监听器，Task Name：{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), count, this.GetShortTaskName(255));
                    if (DateTime.Now.Second % 30 == 0)
                        Console.Write(strTemp);//显示正在运行中
                }
                //将监听器数量写入全局缓存，供集群调度服务使用
                //这里获得的只是当前任务线程的监听器数量，由于可能有多个任务线程，所以不准。
                //ICacheProvider cache = CacheProviderFactory.GetGlobalCacheProvider();
                //cache.Insert<int>(Program.Host.GetUri()+ "_ListenerCount",count);


                #region 注释的代码
                //if (this.CurrentContext.SessionRequired)
                //{
                //    foreach (SubscriberInfo info in this.SubscriberInfoList.ToArray())
                //    {
                //        //根据每个会话来计算服务结果
                //        publishResult = CallService(info.SessionID);
                //        if (this.CurrentContext.NoResultRecord(publishResult))
                //            continue;
                //        //可能执行完服务后，监听器又断开了，因此需要再次获取
                //        MessageListener currLst = MessageCenter.Instance.GetListener(info.FromIP, info.FromPort);
                //        //workMessage += string.Format("\r\n线程名称：{0}", thread.Name);
                //        if (currLst != null)
                //        {
                //            MessageCenter.Instance.NotifyOneMessage(currLst, info.MessageID, publishResult);
                //            string text = string.Format("\r\n[{0}]Publish Required Session,ID: {1} \r\n>>[ID:{2}]消息长度：{3} 字节 ,消息内容摘要：\r\n{4}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), info.SessionID, info.MessageID, publishResult.Length.ToString("###,###"), publishResult);
                //            workMessage += text;
                //            System.Diagnostics.Debug.WriteLine(text);
                //        }
                //        else
                //        {
                //            string text = string.Format("\r\n[{0}]未找到监听器， Session,ID: {1} \r\n>>[ID:{2}]消息长度：{3} 字节 ,消息内容摘要：\r\n{4}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), info.SessionID, info.MessageID, publishResult.Length.ToString("###,###"), publishResult);
                //            workMessage += text;
                //            System.Diagnostics.Debug.WriteLine(text);
                //            this.SubscriberInfoList.Remove(info);
                //        }
                //    }
                //}
                //else
                //{
                //    publishResult = CallService();

                //    if (!this.CurrentContext.NoResultRecord(publishResult))
                //    {
                //        workMessage += string.Format("\r\n[{0}]Publish with No Required Session,begin...,Content is:\r\n{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), publishResult);
                //        foreach (SubscriberInfo info in this.SubscriberInfoList.ToArray())
                //        {
                //            //可能执行完服务后，监听器又断开了，因此需要再次获取
                //            MessageListener currLst = MessageCenter.Instance.GetListener(info.FromIP, info.FromPort);
                //            if (currLst != null)
                //            {
                //                count++;
                //                MessageCenter.Instance.NotifyOneMessage(currLst, info.MessageID, publishResult);
                //                string text = string.Format("\r\n[{0}]Publish To,SessionID: {1} \r\n>>[ID:{2}]消息长度：{3} 字节-------", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), info.SessionID, info.MessageID, publishResult.Length.ToString("###,###"));
                //                workMessage += text;
                //                System.Diagnostics.Debug.WriteLine(text);
                //            }
                //            else
                //            {
                //                string text = string.Format("\r\n[{0}]未找到监听器， Session,ID: {1} \r\n>>[ID:{2}]消息长度：{3} 字节 ,消息内容摘要：\r\n{4}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), info.SessionID, info.MessageID, publishResult.Length.ToString("###,###"), publishResult);
                //                workMessage += text;
                //                System.Diagnostics.Debug.WriteLine(text);
                //                this.SubscriberInfoList.Remove(info);
                //            }
                //        }
                //        workMessage += string.Format("\r\n[{0}]请求处理完毕--Publish Count: {1} -------", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), count);
                //    }//end if

                //}//end if
                #endregion

                this.Publish(ref workMessage);
                if (workMessage.Length > 0)
                {
                    Console.WriteLine(strTemp + "\r\n" + workMessage);
                    Console.WriteLine("\r\n-------------Thread Sleep {0}ms ---------------------", BatchInterval);
                }

            }
            while (true);
            isRunning = false;
        }

        /// <summary>
        /// 事件模式
        /// </summary>
        void DoEvent()
        {
            EventServicePublisher self = (EventServicePublisher)this;
            self.Ready();

            while (true)
            {
                //检查超期
                if (!self.CheckActiveLife())
                {
                    self.Close();
                    PublisherFactory.Instance.RemovePublisher(self.TaskName);
                    Console.WriteLine("\r\n[{0}]当前任务已检验到事件源对象为非活动状态，工作线程退出--Task Name: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), this.GetShortTaskName(255));
                    batchIndex = 0;
                    break;
                }
                string workMessage = "\r\n----publisher DoEvent------------------\r\n";

                int count = GetListeners().Length;
                if (count == 0)
                {
                    Console.WriteLine("[{0}]当前任务已经没有监听器，但事件源对象仍然活动，可接受再次订阅--Task Name: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), this.GetShortTaskName(255));
                }
                else
                {
                    Console.WriteLine("[{0}]当前工作线程有{1}个相关的监听器，Task Name：{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), count, this.GetShortTaskName(255));
                }
                //等待服务对象触发事件，等待30秒
                if (resetEvent.WaitOne(30 * 1000))
                {
                    this.Publish(ref workMessage);
                    if (workMessage.Length > 0)
                    {
                        Console.WriteLine( workMessage);
                    }
                    //推送完成，触发业务线程状态，允许业务线程继续
                    self.SetWorkEvent();
                }
                
            }
            isRunning = false;
        }

        protected string CallService(ServiceContext context)
        {
            context.BatchIndex = batchIndex;
            context.ServiceErrorEvent += new EventHandler<ServiceErrorEventArgs>(context_ServiceErrorEvent);
            context.ProcessService();
            context.ServiceErrorEvent -= new EventHandler<ServiceErrorEventArgs>(context_ServiceErrorEvent);
            return context.Response.AllText;
        }

        void context_ServiceErrorEvent(object sender, ServiceErrorEventArgs e)
        {
            if (this.PublisherErrorEvent != null)
                this.PublisherErrorEvent(this, e);
        }


        protected string CallService(string sessionId, ServiceContext context)
        {
            if (context.SessionRequired)
            {
                //不通过直接设置 ServiceContext对象的会话对象的方式，而是先设置会话标识，由Session属性来决定如何生成会话对象
                //context.Session = SessionContainer.Instance.GetSession(sessionId);
                context.SessionID = sessionId;
            }

            return CallService(context);
        }

        protected Func<string, string> GetMessageFun(SubscriberInfo info)
        {
            return strPara =>
            {
                MessageListener currLst = MessageCenter.Instance.GetListener(info.FromIP, info.FromPort);
                if (currLst != null)
                    return currLst.CallBackFunction(info.MessageID, strPara);
                else
                    return "";
            };
        }

        protected Func<string, string> PreGetMessageFun(SubscriberInfo info)
        {
            return strPara =>
            {
                MessageListener currLst = MessageCenter.Instance.GetListener(info.FromIP, info.FromPort);
                if (currLst != null)
                    return currLst.PreCallBackFunction(info.MessageID, strPara);
                else
                    return "";
            };
        }
    }

    /// <summary>
    /// 需要会话的服务发布商，这种情况会为“享元”中的不同的订阅者按照会话分别处理服务结果
    /// </summary>
    class SessionServicePublisher : ServicePublisher
    {
        public SessionServicePublisher(string taskName) : base(taskName) { }

        private void PublishItem(SubscriberInfo info, System.Diagnostics.Stopwatch sw, ref string tempMsg, ref int index)
        {
            string publishResult = null;
            ServiceContext context = new ServiceContext(info.Request);
            context.Host = this.Host;
            context.SessionRequired = true;
            context.GetMessageFun = base.GetMessageFun(info);
            context.PreGetMessageFun = base.PreGetMessageFun(info);
            //根据每个会话来计算服务结果
            publishResult = CallService(info.SessionID, context);
            tempMsg += string.Format("\r\nPub No.{0},have used {1}ms.\r\n", index++, sw.ElapsedMilliseconds);
            if (context.SendEmptyResult || (!context.SendEmptyResult && !context.NoResultRecord(publishResult)))
            {
                //可能执行完服务后，监听器又断开了，因此需要再次获取
                MessageListener currLst = MessageCenter.Instance.GetListener(info.FromIP, info.FromPort);

                if (currLst != null)
                {
                    MessageCenter.Instance.NotifyOneMessage(currLst, info.MessageID, publishResult);
                    string text = string.Format("\r\n[{0}]Publish Required Session,ID: {1} \r\n>>[ID:{2}]消息长度：{3} 字节 ,消息内容摘要：\r\n{4}",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        info.SessionID, info.MessageID, publishResult.Length.ToString("###,###"),
                        DataConverter.DeEncrypt8bitString(publishResult.Length > 256 ? publishResult.Substring(0, 256) : publishResult));
                    tempMsg += text;
                    //System.Diagnostics.Debug.WriteLine(text);
                }
                else
                {
                    string text = string.Format("\r\n[{0}]未找到监听器， Session,ID: {1} \r\n>>[ID:{2}]消息长度：{3} 字节 ,消息内容摘要：\r\n{4}",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        info.SessionID, info.MessageID, publishResult.Length.ToString("###,###"),
                        DataConverter.DeEncrypt8bitString(publishResult.Length > 256 ? publishResult.Substring(0, 256) : publishResult));
                    tempMsg += text;
                    //System.Diagnostics.Debug.WriteLine(text);
                    this.SubscriberInfoList.Remove(info);
                }
            }
        }

        protected override void Publish(ref string workMessage)
        {

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            int index = 0;
            string tempMsg = "";
            if (ParallelExecute)//是否启用并行执行
            {
                Parallel.ForEach(this.SubscriberInfoList.ToArray(), info =>
                {
                    PublishItem(info, sw, ref tempMsg, ref index);
                });
            }
            else
            {
                foreach (var info in this.SubscriberInfoList.ToArray())
                {
                    PublishItem(info, sw, ref tempMsg, ref index);
                }
            }

            sw.Stop();
            workMessage += tempMsg;
            workMessage += string.Format("\r\nPub All count={0},All have used {1}ms.\r\n", index, sw.ElapsedMilliseconds); ;
        }

    }

    /// <summary>
    /// 不需要会话的服务发布商，这种情况会为“享元”中的所有订阅者提供相同的服务结果
    /// </summary>
    class NoneSessionServicePublisher : ServicePublisher
    {
        public NoneSessionServicePublisher(string taskName) : base(taskName) { }

        protected override void Publish(ref string workMessage)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            workMessage += string.Format("\r\nPub2 CallService Begin:{0}.\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            int count = 0;
            Dictionary<string, string> dictResult = new Dictionary<string, string>();

            foreach (SubscriberInfo info in this.SubscriberInfoList.ToArray())
            {
                ServiceContext context = new ServiceContext(info.Request);
                context.Host = this.Host;
                //参数一样的话，仅计算一次
                string publishResult = null;
                string key = info.Request.ServiceUrl;
                if (dictResult.ContainsKey(key))
                {
                    publishResult = dictResult[key];
                }
                else
                {
                    context.GetMessageFun = base.GetMessageFun(info);
                    context.PreGetMessageFun = base.PreGetMessageFun(info);
                    publishResult = CallService(context);
                    dictResult.Add(key, publishResult);
                }

                //workMessage += string.Format("\r\n[{0}]Publish with No Required Session,begin...,Content is:\r\n{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), publishResult);

                if (!context.NoResultRecord(publishResult))
                {
                    //可能执行完服务后，监听器又断开了，因此需要再次获取
                    MessageListener currLst = MessageCenter.Instance.GetListener(info.FromIP, info.FromPort);
                    if (currLst != null)
                    {
                        count++;
                        MessageCenter.Instance.NotifyOneMessage(currLst, info.MessageID, publishResult);
                        string text = string.Format("\r\n[{0}]Pub2 To,SessionID: {1} \r\n>>[ID:{2}]消息长度：{3} 字节 ,消息内容摘要：\r\n{4}",
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            info.SessionID,
                            info.MessageID,
                            publishResult.Length.ToString("###,###"),
                            DataConverter.DeEncrypt8bitString(publishResult.Length > 256 ? publishResult.Substring(0, 256) : publishResult)
                            );
                        workMessage += text;
                        //System.Diagnostics.Debug.WriteLine(text);
                    }
                    else
                    {
                        string text = string.Format("\r\n[{0}]Pub2 未找到监听器， Session,ID: {1} \r\n>>[ID:{2}]消息长度：{3} 字节 ,消息内容摘要：\r\n{4}",
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), info.SessionID, info.MessageID, publishResult.Length.ToString("###,###"),
                            publishResult.Length > 256 ? publishResult.Substring(0, 256) : publishResult);
                        workMessage += text;
                        //System.Diagnostics.Debug.WriteLine(text);
                        this.SubscriberInfoList.Remove(info);
                    }
                    workMessage += string.Format("\r\n[{0}]请求处理完毕--Pub2 Count: {1} ,All usetime:{2}ms-------", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), count, sw.ElapsedMilliseconds);
                }
            }
            sw.Stop();
            workMessage += string.Format("\r\nPub2 CallService All Usetime:{0}ms.\r\n", sw.ElapsedMilliseconds);
        }
    }

    class EventServicePublisher : ServicePublisher
    {
        ServiceContext Context;
        string publishResult;
        DateTime lastPublishTime;
        bool published;
        AutoResetEvent workEvent = new AutoResetEvent(false);

        public EventServicePublisher(string taskName, IServiceContext context):base(taskName)
        {
            this.Context = (ServiceContext)context;
            this.Context.OnPublishDataEvent += Context_OnPublishDataEvent;
        }

        void Context_OnPublishDataEvent(object sender, ServiceEventArgs e)
        {
            //当前方法工作在业务工作线程
           
            Context.WriteResponse(e.EventData);
            this.publishResult = Context.Response.AllText;
            Context.Response.End();
           
            published = false;
            base.SetPublishEvent();
            //事件推送线程收到信号，开始工作
            //这里必须等待推送线程完成当前推送任务，业务工作线程进入等待状态
            workEvent.WaitOne();
        }

        /// <summary>
        /// 消息发布线程准备就绪，开启业务工作线程
        /// </summary>
        protected internal void Ready()
        {
            ServiceEventSource ses = this.Context.PublishEventSource;
            if (ses.EventWork != null)
            {
                Task.Factory.StartNew(ses.EventWork);
            }
        }

        /// <summary>
        /// 使业务工作线程继续
        /// </summary>
        protected internal void SetWorkEvent()
        {
            this.workEvent.Set();
        }

        /// <summary>
        /// 发布消息，注意当前方法工作在消息发布线程，不是业务线程
        /// </summary>
        /// <param name="workMessage"></param>
        protected override void Publish(ref string workMessage)
        {
            //避免监控线程超时，发布重复消息
            if (published)
                return;
            published = true;

            //在子线程推送消息
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            workMessage += string.Format("\r\nPub2 CallService Begin:{0}.\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            int count = 0;

            foreach (SubscriberInfo info in this.SubscriberInfoList.ToArray())
            {
                if (!Context.NoResultRecord(publishResult))
                {
                    //可能执行完服务后，监听器又断开了，因此需要再次获取
                    MessageListener currLst = MessageCenter.Instance.GetListener(info.FromIP, info.FromPort);
                    if (currLst != null)
                    {
                        count++;
                        MessageCenter.Instance.NotifyOneMessage(currLst, info.MessageID, publishResult);
                        string text = string.Format("\r\n[{0}]Pub2 To,SessionID: {1} \r\n>>[ID:{2}]消息长度：{3} 字节 ,消息内容摘要：\r\n{4}",
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            info.SessionID,
                            info.MessageID,
                            publishResult.Length.ToString("###,###"),
                            DataConverter.DeEncrypt8bitString(publishResult.Length > 256 ? publishResult.Substring(0, 256) : publishResult)
                            );
                        workMessage += text;
                        //真正发布给监听器才算一次发布
                        lastPublishTime = DateTime.Now;
                    }
                    else
                    {
                        string text = string.Format("\r\n[{0}]Pub2 未找到监听器， Session,ID: {1} \r\n>>[ID:{2}]消息长度：{3} 字节 ,消息内容摘要：\r\n{4}",
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), info.SessionID, info.MessageID, publishResult.Length.ToString("###,###"),
                            publishResult.Length > 256 ? publishResult.Substring(0, 256) : publishResult);
                        workMessage += text;
                        this.SubscriberInfoList.Remove(info);
                    }
                    workMessage += string.Format("\r\n[{0}]请求处理完毕--Pub2 Count: {1} ,All usetime:{2}ms-------", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), count, sw.ElapsedMilliseconds);
                }
            }
            sw.Stop();
            workMessage += string.Format("\r\nPub2 CallService All Usetime:{0}ms.\r\n", sw.ElapsedMilliseconds);
        }
        /// <summary>
        /// 检查事件源对象是否活动
        /// </summary>
        /// <returns></returns>
        public bool CheckActiveLife()
        {
            //lastPublishTime 为默认值，表示从未收到过服务发布的事件数据，此时应该认为事件源为活动状态
            return lastPublishTime==default(DateTime) ||
                (int)DateTime.Now.Subtract(lastPublishTime).TotalMinutes < Context.PublishEventSource.ActiveLife;
        }

        /// <summary>
        /// 关闭服务上下文对象，清理资源
        /// </summary>
        public void Close()
        {
            this.Context.Dispose();
            this.Context.OnPublishDataEvent -= Context_OnPublishDataEvent;
            this.Context = null;
        }
    }

    /// <summary>
    /// 服务发布商工厂
    /// </summary>
    class PublisherFactory
    {
        Dictionary<string, ServicePublisher> dict = new Dictionary<string, ServicePublisher>();

        /// <summary>
        /// 获取出版商，如果没有，则创建一个（享元模式）
        /// </summary>
        /// <param name="request">服务请求对象</param>
        /// <returns>出版商</returns>
        public ServicePublisher GetPublisher(IServiceContext context)
        {
            ServiceRequest request =context.Request;
            bool sessionRequired = context.SessionRequired;
            string key = "";
            if (request.RequestModel == RequestModel.ServiceEvent)
                key = request.ServiceUrl;
            else
                key = string.Format("Publish://{0}/{1}", request.ServiceName, request.MethodName);

            if (dict.ContainsKey(key))
            {
                return dict[key];
            }
            else
            {
                lock (_syncLock)
                {
                    if (dict.ContainsKey(key))
                    {
                        return dict[key];
                    }
                    else
                    {
                        ServicePublisher pub = null;
                        if (request.RequestModel == RequestModel.ServiceEvent)
                        {
                            pub = new EventServicePublisher(key,context);
                        }
                        else
                        {
                            if (sessionRequired)
                                pub = new SessionServicePublisher(key);
                            else
                                pub = new NoneSessionServicePublisher(key);
                        }

                        dict[key] = pub;
                        return pub;
                    }
                }
            }
        }

        public void RemovePublisher(string key)
        {
            dict.Remove(key);
        }

        /// <summary>
        /// 根据订阅者信息，查找它所在的发布者
        /// </summary>
        /// <param name="sourceSubInfo"></param>
        /// <returns></returns>
        public ServicePublisher Find(SubscriberInfo sourceSubInfo, out SubscriberInfo objSubInfo)
        {
            objSubInfo = null;

            foreach (string key in dict.Keys)
            {
                ServicePublisher item = dict[key];
                var tempSubinfo = item.SubscriberInfoList.ToArray().FirstOrDefault(p => p.FromIP == sourceSubInfo.FromIP
                    && p.FromPort == sourceSubInfo.FromPort
                    && p.MessageID == sourceSubInfo.MessageID);
                if (tempSubinfo != null)
                {
                    objSubInfo = tempSubinfo;
                    return item;
                }
            }
            return null;
        }

        #region PublisherFactory 的单例实现
        private static readonly object _syncLock = new object();//线程同步锁；
        private static PublisherFactory _instance;
        /// <summary>
        /// 返回 MessageCenter 的唯一实例；
        /// </summary>
        public static PublisherFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new PublisherFactory();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 保证单例的私有构造函数；
        /// </summary>
        private PublisherFactory() { }

        #endregion
    }


}
