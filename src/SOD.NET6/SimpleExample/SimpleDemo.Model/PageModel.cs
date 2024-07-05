using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDemo.Model
{
    public class PageResult<T> where T : class
    {
        public int Total { get; set; }
        public IEnumerable<T> Items { get; set; }

    }
}
