using System;
using System.Collections.Generic;
using System.Text;

namespace TranstarAuction.Model.AuctionMain
{
     public class PublishInfo
    {
         /// <summary>
         /// 拍品id
         /// </summary>
         public long PublishId
         {
             get;
             set;
         }
         /// <summary>
         /// 合作经销商id
         /// </summary>
         public int PartnerTvaId
         {
             get;
             set;
         }
         /// <summary>
         /// 时间点
         /// </summary>
         public int tim
         {
             get;
             set;
         }
    }
}
