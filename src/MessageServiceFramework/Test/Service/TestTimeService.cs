using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PWMIS.EnterpriseFramework.Service.Runtime;
using Model;
using System.Threading.Tasks;

namespace ServiceSample
{
    public class TestTimeService:IService
    {
        TimeCount tc;
        IServiceContext currContext = null;
        public TestTimeService()
        {
            tc = new TimeCount();
        }

        public TimeCount ServerTime()
        {
           
            tc.Execute();
            return tc;
        }

        public ServiceEventSource ParallelTime(int timeCount)
        {
            return new ServiceEventSource(this, 1, () => {
                PublishParallelData(timeCount);
                Console.WriteLine("All Publish OK.");
                this.currContext.PublishData(new DateTime(2018,1,1));
                this.currContext.PublishEventSource.DeActive();
            });

        }

        private void PublishParallelData(int timeCount)
        {
            Task[] tasks = new Task[timeCount];
            for (int i = 0; i < timeCount; i++)
            {
                tasks[i] =Task.Factory.StartNew(obj => {
                    int index = (int)obj;
                    DateTime dt = DateTime.Now;
                    Console.WriteLine(">>>>>>>>> NO.{0} begin publish data:{1},Thread ID:{2}",index, dt, System.Threading.Thread.CurrentThread.ManagedThreadId);
                    this.currContext.PublishData(dt);
                    Console.WriteLine("<<<<<<<<< NO.{0} end   publish data:{1},Thread ID:{2}", index, dt, System.Threading.Thread.CurrentThread.ManagedThreadId);
                    Console.WriteLine();
                },i);
            }
            Task.WaitAll(tasks, -1);
            throw new Exception("test error...............");
        }

        public void CompleteRequest(IServiceContext context)
        {
            
        }

        public bool ProcessRequest(IServiceContext context)
        {
            this.currContext = context;
            return true;
        }


        public bool IsUnSubscribe
        {
            get { return false; }
        }
    }
}
