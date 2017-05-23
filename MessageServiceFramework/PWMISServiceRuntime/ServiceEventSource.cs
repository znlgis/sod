using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PWMIS.EnterpriseFramework.Service.Runtime
{
    /// <summary>
    /// 服务事件源对象，如果距离末次发布数据的时间超过指定时间，源对象被视为未活动（发布数据）状态,源对象会被回收；
    /// 如果源对象从未发布过数据，被视为活动状态
    /// </summary>
    public class ServiceEventSource
    {
        const int MAX_TIME = 60 * 24;
        /// <summary>
        /// 引发事件的源对象
        /// </summary>
        public object Source { get; private set; }
        /// <summary>
        /// 活动时间，单位：分钟，默认20分钟，最大不可以超过24小时
        /// </summary>
        public int ActiveLife { get; private set; }
        /// <summary>
        /// 当前所在的服务上下文
        /// </summary>
        public IServiceContext CurrentContext { get; protected internal set; }
        /// <summary>
        /// 以默认的事件源对象存活周期初始化本对象
        /// </summary>
        /// <param name="source">引发事件的源对象</param>
        public ServiceEventSource(object source)
        {
            this.Source = source;
            this.ActiveLife = 20;
        }

        /// <summary>
        /// 事件源以指定的存活周期初始化本对象
        /// </summary>
        /// <param name="source">引发事件的源对象</param>
        /// <param name="time">活动时间，单位分钟</param>
        public ServiceEventSource(object source,int time)
        {
            this.Source = source;
            if (time > MAX_TIME)
                this.ActiveLife = MAX_TIME;
            else
                this.ActiveLife = time;
        }

        /// <summary>
        /// 事件源以指定的存活周期初始化本对象，并指定引发事件的工作方法
        /// </summary>
        /// <param name="source">引发事件的源对象</param>
        /// <param name="time">活动时间，单位分钟</param>
        /// <param name="work">在服务中会引发事件的工作</param>
        public ServiceEventSource(object source, int time, Action work)
            : this(source, time)
        {
            this.EventWork = work;
        }

        /// <summary>
        /// 获取在服务中会引发事件的工作
        /// </summary>
        public Action EventWork { get; private set; }

        /// <summary>
        /// 设置事件源为非活动状态
        /// </summary>
        public void DeActive()
        {
            this.ActiveLife = 0;
            //发布一次空数据，促使尽快结束发布线程
            this.CurrentContext.PublishData(null);
        }
    }
}
