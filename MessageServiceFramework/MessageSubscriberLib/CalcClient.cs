using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using MessagePublishService;

namespace MessageSubscriber
{
    /// <summary>
    /// 简易计算器客户端，仅供测试WCF通信
    /// </summary>
    public class CalcClient
    {
        private string address;

        public CalcClient(string serviceAddress)
        {
            this.address = serviceAddress;
        }
        public CalcClient(string host,int port)
        {
            address = string.Format("net.tcp://{0}:{1}", host, port);
        }
        public double GetAddResult(double a, double b)
        {
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            ChannelFactory<ICalculator> channelFactory2 = new ChannelFactory<ICalculator>(binding, new EndpointAddress(address));
            ICalculator calculator = channelFactory2.CreateChannel();
            Console.WriteLine("尝试开始调用单工 Add 方法");
            try
            {
                return calculator.Add(a, b);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An Exception is thrown!\n\t:Type:{0}\n\tMessage:{1}", ex.GetType(), ex.Message);
            }
            finally
            {
                try
                {
                    channelFactory2.Close();
                }
                catch
                { 
                
                }
            }
            return 0;
        }

    }
}
