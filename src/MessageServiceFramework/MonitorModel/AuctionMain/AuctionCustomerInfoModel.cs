using System;
using System.Collections.Generic;
using System.Text;

namespace TranstarAuction.Model.AuctionMain
{
   public class AuctionCustomerInfoModel
    {
       /// <summary>
       /// 用户公司名称
       /// </summary>
       public string CompanyName
       {
           get;
           set;
       }

       /// <summary>
       /// 用户名称
       /// </summary>
       public string UserName
       {
           get;
           set;
       }

       /// <summary>
       /// 用户当前保证金
       /// </summary>
       public int CurrentNearestMoney
       {
           get;
           set;
       }

    }
}
