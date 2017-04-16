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
        IServiceContext context;

        public event EventHandler Alarming;

        public AlarmClockService()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += timer_Elapsed;
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (e.SignalTime >= this.AlarmTime)
            {
                if (Alarming != null)
                    Alarming(this, new EventArgs());

                context.PublishData(e.SignalTime);
            }
        }

       
        public ServiceEventSource SetAlarmTime(DateTime targetTime)
        {
            this.AlarmTime = targetTime;
            timer.Start();
            return new ServiceEventSource(timer);
        }


        public void CompleteRequest(IServiceContext context)
        {

        }

        public bool ProcessRequest(IServiceContext context)
        {
            this.context = context;
            return true;
        }

        public bool IsUnSubscribe
        {
            get { return false; }
        }
    }
}
