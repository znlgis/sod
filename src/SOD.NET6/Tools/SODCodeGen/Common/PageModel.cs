using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sod.Pwmis.com.Common
{
    public class PageResult<T> where T : class
    {
        public int Total { get; set; }
        public IEnumerable<T> Items { get; set; }

    }
}
