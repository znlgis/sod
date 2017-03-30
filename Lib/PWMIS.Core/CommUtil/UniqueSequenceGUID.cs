using System;
using System.Collections.Generic;
using System.Text;

namespace PWMIS.Core
{
    class UniqueSequenceGUID
    {
         int lastSecond = 0;
         Dictionary<long, int> dictTong = new Dictionary<long, int>();

         
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

         protected internal static long InnerNewSequenceGUID(DateTime dt)
         {
             int num = dt.Second
                   + dt.Minute * 100
                   + dt.Hour * 100 * 100
                   + dt.Day * 100 * 100 * 100;
             if (dt.Month < 10) //月份大于等于10，会导致结果为负数
                 num += dt.Month * 100 * 100 * 100 * 100;

             long sn = (long)num * 1000 + dt.Millisecond;
             long seq = sn * 10000000 + Math.Abs(Guid.NewGuid().GetHashCode());
             return seq;
         }
    }
}
