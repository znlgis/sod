using System;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using PWMIS.DataProvider.Data;

namespace PWMIS.Core
{
    internal class UniqueSequenceGUID
    {
        //机器标识，3位整数
        private static readonly int MachineID;
        private static int SeqNum;
        private static int signal; //0可用，1不可用。


        private static readonly DateTime baseDate = new(2017, 3, 1);

        static UniqueSequenceGUID()
        {
            //先从配置文件获取，如果获取不到，再从IP地址随机生成，如果失败，再直接生成3位随机数
            var strMachineId = ConfigurationManager.AppSettings["SOD_MachineID"];
            if (!string.IsNullOrEmpty(strMachineId) && int.TryParse(strMachineId, out MachineID))
            {
                if (MachineID >= 1000 || MachineID < 100)
                    MachineID = GetMachineRandom();
            }
            else
            {
                MachineID = GetMachineRandom();
            }

            CommandLog.Instance.LogWriter.WriteLog("初始化分布式机器ID：" + MachineID);
        }

        private static int GetMachineRandom()
        {
            var result = 100;
            try
            {
                var host = Dns.GetHostName();
                //个别机器因为DNS设置问题，GetHostEntry 会发生 “不知道这样的主机”的错误
                foreach (var ip in Dns.GetHostEntry(host).AddressList)
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        var bytes = ip.GetAddressBytes();
                        var intIp = (bytes[1] << 16) | (bytes[2] << 8) | bytes[3];
                        result = new Random(intIp).Next(100, 999);
                        break;
                    }
            }
            catch (Exception ex)
            {
                result = new Random().Next(100, 999);
                CommandLog.Instance.LogWriter.WriteLog("获取分布式机器ID失败，原因：" + ex.Message + "。将采用随机ID");
            }

            return result;
        }

        public long NewID()
        {
            return InnerNewSequenceGUID(DateTime.Now, true);
        }

        public static int GetCurrentMachineID()
        {
            return MachineID;
        }

        /// <summary>
        ///     获取一个新的有序GUID整数
        /// </summary>
        /// <param name="dt">当前时间</param>
        /// <param name="haveMs">是否包含毫秒，如果不包含，将使用3位随机数代替</param>
        /// <returns></returns>
        protected internal static long InnerNewSequenceGUID(DateTime dt, bool haveMs)
        {
            //线程安全的自增并且不超过最大值10000
            var countNum = Interlocked.Increment(ref SeqNum);
            if (countNum >= 10000)
            {
                while (Interlocked.Exchange(ref signal, 1) != 0) //加自旋锁
                {
                    //黑魔法
                }

                //进入临界区
                if (SeqNum >= 10000)
                {
                    SeqNum = 0;
                    //达到1万个数后，延迟10毫秒，重新取系统时间，避免重复
                    Thread.Sleep(10);
                    dt = DateTime.Now;
                }

                countNum = Interlocked.Increment(ref SeqNum);
                //离开临界区
                Interlocked.Exchange(ref signal, 0); //释放锁
            }

            //日期以 2017.3.1日为基准，计算当前日期距离基准日期相差的天数，可以使用20年。
            //日期部分使用4位数字表示
            var days = (int)dt.Subtract(baseDate).TotalDays;
            //时间部分表示一天中所有的秒数，最大为 86400秒,共5位
            //日期时间总位数= 4（日期）+5（时间）+3（毫秒）=12
            var times = dt.Second + dt.Minute * 60 + dt.Hour * 3600;
            //long 类型最大值 9223 3720 3685 4775 807
            //可用随机位数= 19-12=7
            var datePart = ((long)days + 1000) * 1000 * 1000 * 1000 * 100;
            var timePart = (long)times * 1000 * 1000;
            long msPart = 0;
            if (haveMs)
                msPart = dt.Millisecond;
            else
                msPart = new Random().Next(100, 1000);
            var dateTiePart = (datePart + timePart + msPart * 1000) * 10000;

            var mid = MachineID * 10000;
            //得到总数= 4（日期）+5（时间）+3（毫秒）+7(GUID)
            var seq = dateTiePart + mid;

            return seq + countNum;
            ;
        }
    }
}