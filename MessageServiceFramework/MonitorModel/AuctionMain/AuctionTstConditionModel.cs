using System;
using System.Collections.Generic;
using System.Text;

namespace TranstarAuction.Model.AuctionMain
{
    public class AuctionTstConditionModel
    {
        /// <summary>
        /// 买家用户ID
        /// </summary>
        public int BuyerTvuId
        {
            get;
            set;
        }
        /// <summary>
        /// 买家经销商ID
        /// </summary>
        public int BuyerTvaId
        {
            get;
            set;
        }
        /// <summary>
        /// 拍品ID
        /// </summary>
        public long PublishId
        {
            get;
            set;
        }
        /// <summary>
        /// 付款方式
        /// </summary>
        public int TstPayMode
        {
            get;
            set;
        }
        /// <summary>
        /// 提车方式
        /// </summary>
        public int TstPickUpMode
        {
            get;
            set;
        }
        /// <summary>
        /// 路线ID
        /// </summary>
        public int TstSysRouteId
        {
            get;
            set;
        }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get;
            set;
        }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName
        {
            get;
            set;
        }

        /// <summary>
        /// 拍品表过户地点ID
        /// </summary>
        public int SysAddressId
        {
            get;
            set;
        }

        /// <summary>
        /// 1优信拍代办过户 0不是优信拍代办过户
        /// </summary>
        public int TransferType
        {
            get;
            set;
        }
    }
}
