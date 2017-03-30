using System;
using System.Collections.Generic;
using System.Text;

namespace PWMIS.Core
{
    class UniqueSequenceGUID
    {
         int lastSecond = 0;
         Dictionary<long, int> dictTong = new Dictionary<long, int>();
         static readonly DateTime baseDate = new DateTime(2017, 3, 1);

         
         public long NewID()
         {
             DateTime dt = DateTime.Now;
             long result = InnerNewSequenceGUID(dt);
             if (dt.Second == lastSecond)
             {
                 if (dictTong.ContainsKey(result))
                 {
                     //Console.WriteLine("repeat.{0}", result);
                     return NewID();
                 }
                 else
                 {
                     dictTong.Add(result, lastSecond);
                 }
             }
             else
             {
                 dictTong.Clear();
                 lastSecond = dt.Second;
                 //Console.WriteLine("clear dict");
             }
             return result;
         }

        /// <summary>
        /// 获取一个新的有序GUID整数
        /// </summary>
        /// <param name="dt">当前时间</param>
         /// <param name="haveMs">是否包含毫秒，生成更加有序的数字，但这会增加重复率</param>
        /// <returns></returns>
         protected internal static long InnerNewSequenceGUID(DateTime dt,bool haveMs)
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
             long msPart = (long)123 * 1000;//dt.Millisecond
             long dateTiePart = (datePart + timePart + msPart) * 10000;
             //获取GUID后8位数字,重复率会在万分之50一下
             //如果GUID不取余数，重复率在万分之一以下
             int guid = Math.Abs(Guid.NewGuid().GetHashCode());
             if (haveMs)
                 guid = guid % 10000000;
             //得到总数= 4（日期）+5（时间）+3（毫秒）+7(GUID)
             long seq = dateTiePart + guid;
             return seq;
         }
    }
}
