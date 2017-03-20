using System;
using System.Collections.Generic;
using System.Text;

namespace TranstarAuction.Model
{
    public class SearchConditionModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID
        {
            get;
            set;
        }
        /// <summary>
        /// 拍卖时间
        /// </summary>
        public DateTime AuctionHallDate
        {
            get;
            set;
        }
        /// <summary>
        /// 城市
        /// </summary>
        public string City
        {
            get;
            set;
        }
        /// <summary>
        /// 车品牌
        /// </summary>
        public string CarBrand
        {
            get;
            set;
        }
        /// <summary>
        /// 车型
        /// </summary>
        public string CarType
        {
            get;
            set;
        }
        /// <summary>
        /// 车款式
        /// </summary>
        public string CarStyle
        {
            get;
            set;
        }
        /// <summary>
        /// 车颜色
        /// </summary>
        public string CarColor
        {
            get;
            set;
        }
        /// <summary>
        /// 里程数
        /// </summary>
        public string Mileage
        {
            get;
            set;
        }
        /// <summary>
        /// 年限
        /// </summary>
        public string Years
        {
            get;
            set;
        }
        /// <summary>
        /// 当前价(万)
        /// </summary>
        public string CurrentPrices
        {
            get;
            set;
        }
        /// <summary>
        /// 排序状态(0：升序，1：降序)
        /// </summary>
        public int SortStatus
        {
            get;
            set;
        }
    }
}
