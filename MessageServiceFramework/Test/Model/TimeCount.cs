using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class TimeCount
    {
        private  int allCount=0;

        public DateTime Now { get; set; }
        public int Count { get; set; }


        public void Execute()
        {
            this.Now = DateTime.Now;
            this.Count = ++allCount;
        }

    }
}
