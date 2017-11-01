using System;
using System.Collections.Generic;
using System.Text;
using MessageSubscriber;


namespace PWMIS.EnterpriseFramework.Service.Client
{

    class SubscriberUseInfo
    {
        public Subscriber Connector { get; set; }
        public string ServiceUri { get; set; }
        public DateTime ActiveTime { get; set; }
        public bool IsUsing { get; set; }
    }
    /// <summary>
    /// 服务连接池
    /// </summary>
    class PublishServicePool
    {
        List<SubscriberUseInfo> ChannelList = new List<SubscriberUseInfo>();
        System.Threading.Timer timer = null;

        private PublishServicePool()
        {
            timer = new System.Threading.Timer(new System.Threading.TimerCallback(o => {
                CheckPoolConnect();
            }), null, 60000, 30000);
        }
        /// <summary>
        /// 从池中获取一个服务通道
        /// </summary>
        /// <param name="serviceUri">服务的基地址</param>
        /// <returns>服务通道对象（消息订阅者对象）</returns>
        public Subscriber GetServiceChannel(string serviceUri)
        {
            lock (_syncLock)
            {
                SubscriberUseInfo result = ChannelList.Find(p => p.ServiceUri == serviceUri && !p.IsUsing);
                if (result == null)
                {
                    result = new SubscriberUseInfo();
                    result.Connector = new Subscriber(serviceUri);
                    ChannelList.Add(result);
                    System.Diagnostics.Debug.WriteLine("ChannelList count:" + ChannelList.Count);
                }
                result.ServiceUri = serviceUri;
                result.ActiveTime = DateTime.Now;
                result.IsUsing = true;
                return result.Connector;
            }
        }

        /// <summary>
        /// 如果订阅者在连接池中，则放回池中
        /// </summary>
        /// <param name="channel"></param>
        public void BackPool(Subscriber channel)
        {
            if (channel == null)
                return;

            SubscriberUseInfo result = null;
            lock (_syncLock)
            {
                result = ChannelList.Find(p => p.Connector == channel);
            }
            if (result != null)
            {
                result.IsUsing = false;
            }
        }

        /// <summary>
        /// 检查连接池，清除无效的连接
        /// </summary>
        private void CheckPoolConnect()
        {
            SubscriberUseInfo[] tempArray = ChannelList.ToArray();
            foreach (SubscriberUseInfo item in tempArray)
            {
                if (item.Connector != null)
                    item.Connector.Close(1);
                lock (_syncLock)
                {
                    if (item.Connector == null || (!item.IsUsing && DateTime.Now.Subtract(item.ActiveTime).TotalSeconds > 30))
                    {
                        ChannelList.Remove(item);
                    }
                }
            }
        }

         #region PublisherFactory 的单例实现
        private static readonly object _syncLock = new object();//线程同步锁；
        private static PublishServicePool _instance;
        /// <summary>
        /// 返回 MessageCenter 的唯一实例；
        /// </summary>
        public static PublishServicePool Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new PublishServicePool();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion
    }
}

