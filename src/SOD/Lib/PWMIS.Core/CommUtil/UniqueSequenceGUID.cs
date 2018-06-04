using System;
using System.Collections.Generic;
using System.Text;

namespace PWMIS.Core
{
    class UniqueSequenceGUID
    {
        //机器标识，3位整数
        static int MachineID;
        static int SeqNum;

        static readonly DateTime baseDate = new DateTime(2017, 3, 1);

        static UniqueSequenceGUID()
        {
            //先从配置文件获取，如果获取不到，再从IP地址随机生成，如果失败，再直接生成3位随机数
            string strMachineId = System.Configuration.ConfigurationManager.AppSettings["SOD_MachineID"];
            if (!string.IsNullOrEmpty(strMachineId) && int.TryParse(strMachineId, out MachineID))
            {
                if(MachineID>=1000 || MachineID<100)
                    MachineID = GetMachineRandom();
            }
            else
            {
                MachineID = GetMachineRandom();
            }
            PWMIS.DataProvider.Data.CommandLog.Instance.LogWriter.WriteLog("初始化分布式机器ID：" + MachineID);
        }

        private static int GetMachineRandom()
        {
            int result = 100;
            try
            {
                string host = System.Net.Dns.GetHostName();
                //个别机器因为DNS设置问题，GetHostEntry 会发生 “不知道这样的主机”的错误
                foreach (System.Net.IPAddress ip in System.Net.Dns.GetHostEntry(host).AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        var bytes = ip.GetAddressBytes();
                        int intIp = (int)bytes[1] << 16 | (int)bytes[2] << 8 | (int)bytes[3];
                        result = new Random(intIp).Next(100, 999);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                result = new Random().Next(100, 999);
                PWMIS.DataProvider.Data.CommandLog.Instance.LogWriter.WriteLog("获取分布式机器ID失败，原因："+ex.Message+"。将采用随机ID");
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
        /// 获取一个新的有序GUID整数
        /// </summary>
        /// <param name="dt">当前时间</param>
        /// <param name="haveMs">是否包含毫秒，如果不包含，将使用3位随机数代替</param>
        /// <returns></returns>
        protected internal static long InnerNewSequenceGUID(DateTime dt, bool haveMs)
        {
            //日期以 2017.3.1日为基准，计算当前日期距离基准日期相差的天数，可以使用20年。
            //日期部分使用4位数字表示
            int days = (int)dt.Subtract(baseDate).TotalDays;
            //时间部分表示一天中所有的秒数，最大为 86400秒,共5位
            //日期时间总位数= 4（日期）+5（时间）+3（毫秒）=12
            int times = dt.Second + dt.Minute * 60 + dt.Hour * 3600;
            //long 类型最大值 9223 3720 3685 4775 807
            //可用随机位数= 19-12=7
            long datePart = ((long)days + 1000) * 1000 * 1000 * 1000 * 100;
            long timePart = (long)times * 1000 * 1000;
            long msPart = 0;
            if (haveMs)
            {
                msPart = (long)dt.Millisecond ;
            }
            else

            {
                msPart = new Random().Next(100, 1000);
            }
            long dateTiePart = (datePart + timePart + msPart*1000) * 10000;

            int mid = MachineID * 10000;
            //得到总数= 4（日期）+5（时间）+3（毫秒）+7(GUID)
            long seq = dateTiePart + mid;

            //线程安全的自增并且不超过最大值10000
            int startValue = System.Threading.Interlocked.Increment(ref SeqNum);
            while (startValue >= 10000)
            {
                SeqNum = 0;
                startValue = 0;
                //可能此时别的线程再次更改了 SeqNum
                while (startValue != SeqNum)
                {
                    startValue = System.Threading.Interlocked.Increment(ref SeqNum);
                }
            }

            seq = seq + startValue;
            return seq;
        }
    }
}
