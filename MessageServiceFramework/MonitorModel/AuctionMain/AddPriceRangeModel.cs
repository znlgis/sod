using System;
using System.Collections.Generic;
using System.Text;

namespace TranstarAuction.Model.AuctionMain
{
    public class AddPriceRangeModel
    {
        /// <summary>
        /// 快速出价范围
        /// </summary>
        public string QuickPriceRange
        {
            get;
            set;
        }

        /// <summary>
        /// 所有出价范围
        /// </summary>
        public string AllPriceRange
        {
            get;
            set;
        }
    }
}
