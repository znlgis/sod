using System;
using System.Collections.Generic;
using System.Text;

namespace TranstarAuction.Model.AuctionMain
{
   public  class AuctionTitleModel
    {
       /// <summary>
       /// 车名称
       /// </summary>
       public string CarName
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
       /// 公里数
       /// </summary>
       public int CarKilometre
       {
           get;
           set;
       }

       /// <summary>
       /// 车年限
       /// </summary>
       public string CarYear
       {
           get;
           set;
       }
    }
}
