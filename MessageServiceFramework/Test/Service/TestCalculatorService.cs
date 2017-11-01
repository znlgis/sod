using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PWMIS.EnterpriseFramework.Service.Runtime;


namespace ServiceSample
{
    public class TestCalculatorService : ServiceBase
    {
        
        public int Add(int a, int b)
        {
            Console.WriteLine("-----Session ID:{0}------", base.CurrentContext.Session.SessionID);
            //模拟服务器延时
            //System.Threading.Thread.Sleep(300);
            //测试会话和缓存
            //引用类型测试
            //object obj2 = base.CurrentContext.Session.SessionObject<object>("session_obj", "aaa");
            //string str = base.CurrentContext.Session.SessionObject<string>("session_str", "aaa");
            //值类型测试
            int sum = base.CurrentContext.Session.Get<int>("session_sum", 100);
            Console.WriteLine("sum01:{0}",sum);
            sum =sum+ a + b;
            Console.WriteLine("sum21:{0}", sum);
            base.CurrentContext.Session.Set<int>("session_sum", sum);
            Console.WriteLine("sum31:{0}", sum);
            return sum;
        }

        public int Sub(int a, int b)
        {
            throw new Exception("sub error.");
            //return a-b;
        }

        public void TestVoidMethod(string para)
        {
            base.CurrentContext.Session.Set<string>("VoidMethod", para);
        }

       
        public override bool ProcessRequest(IServiceContext context)
        {
            context.SessionRequired = true;
            context.SessionModel = SessionModel.RegisterData;
            return base.ProcessRequest(context); //请保留此行，否则在具体的方法里面可能无法获取 CurrentContext 属性
        }

       
    }
}
