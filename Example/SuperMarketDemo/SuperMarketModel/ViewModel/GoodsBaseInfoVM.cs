using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarketModel.ViewModel
{
    public class GoodsBaseInfoVM
    {
        public string SerialNumber
        {
            get;
            set;
        }

        public string GoodsName
        {
            get;
            set;
        }

        public string Manufacturer
        {
            get;
            set;
        }

        public int CanUserMonth
        {
            get;
            set;
        }
    }
}
