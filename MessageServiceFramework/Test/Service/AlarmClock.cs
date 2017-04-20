using PWMIS.EnterpriseFramework.Service.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceSample
{
    public class AlarmClockService : IService
    {
        System.Timers.Timer timer;
        DateTime AlarmTime;
        IServiceContext CurrentContext;
        int publishCount;

        public event EventHandler Alarming;

        public AlarmClockService()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 10000;
            timer.Elapsed += timer_Elapsed;
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (e.SignalTime >= this.AlarmTime)
            {
                if (Alarming != null)
                    Alarming(this, new EventArgs());

                CurrentContext.PublishData(DateTime.Now); //e.SignalTime
                publishCount++;
                Console.WriteLine("AlarmClockService Publish Count:{0}",publishCount);
            }
            if (publishCount > 10)
            {
                timer.Stop();
                Console.WriteLine("AlarmClockService Timer Stoped. ");
            }
        }

       
        public ServiceEventSource SetAlarmTime(DateTime targetTime)
        {
            return new ServiceEventSource(timer, 2, () => {
                //要初始化执行的代码或者方法
                publishCount = 0;
                this.AlarmTime = targetTime;
                timer.Start();
                //如果上面的代码是一个执行时间比较长的方法，但又不知道何时执行完成，
                //并且不想等待超时回收服务对象，而是在执行完成后立即回收服务对象，可以调用下面的代码：
                //CurrentContext.PublishEventSource.DeActive();
                //注意：调用DeActive 方法后将会停止事件推送，所以请注意此方法调用的时机。

                //下面代码仅做测试，查看服务事件源对象的活动生命周期
                int life = CurrentContext.PublishEventSource.ActiveLife;

                //如果上面执行的是一个执行时间比较长的方法，并且有返回值，想将返回值也推送给订阅端，可以再次执行CurrentContext.PublishData
                CurrentContext.PublishData(DateTime.Now);
            });
        }


        public void CompleteRequest(IServiceContext context)
        {

        }

        public bool ProcessRequest(IServiceContext context)
        {
            this.CurrentContext = context;
            return true;
        }

        public bool IsUnSubscribe
        {
            get { return false; }
        }
    }
}
