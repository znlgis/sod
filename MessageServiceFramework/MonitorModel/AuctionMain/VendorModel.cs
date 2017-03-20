using System;
using System.Collections.Generic;
using System.Text;

namespace TranstarAuction.Model.AuctionMain
{
   public class VendorModel
    {
       /// <summary>
       /// 公司名称
       /// </summary>
       public string Company
       {
           get;
           set;
       }
       /// <summary>
       /// 联系人
       /// </summary>
       public string Linkman
       {
           get;
           set;
       }
       /// <summary>
       /// 公司类型
       /// </summary>
       public string CompanyTyp
       {
           get;
           set;
       }
       /// <summary>
       /// 公司地址
       /// </summary>
       public string Address
       {
           get;
           set;
       }
       /// <summary>
       /// 联系人电话
       /// </summary>
       public string LinkmanPhone
       {
           get;
           set;
       }
       /// <summary>
       /// 卖家总分
       /// </summary>
       public string SellerHonor
       {
           get;
           set;
       }
       /// <summary>
       /// 卖家好评
       /// </summary>
       public string SellerGoodRate
       {
           get;
           set;
       }
       /// <summary>
       /// 车况真实性
       /// </summary>
       public string SellerAtt1
       {
           get;
           set;
       }
       /// <summary>
       /// 信息完整性
       /// </summary>
       public string SellerAtt2
       {
           get;
           set;
       }
       /// <summary>
       /// 商家的态度
       /// </summary>
       public string SellerAtt3
       {
           get;
           set;
       }
       /// <summary>
       /// 买家总分 
       /// </summary>
       public string BuyerHonor
       {
           get;
           set;
       }
       /// <summary>
       /// 买家好评
       /// </summary>
       public string BuyerGoodRate
       {
           get;
           set;
       }
       /// <summary>
       /// 出价真实性
       /// </summary>
       public string BuyerAtt1
       {
           get;
           set;
       }
       /// <summary>
       /// 商谈的态度
       /// </summary>
       public string BuyerAtt2
       {
           get;
           set;
       }
       /// <summary>
       /// 付款及时性
       /// </summary>
       public string BuyerAtt3
       {
           get;
           set;
       }
       /// <summary>
       /// 是否显示
       /// </summary>
       public bool IsShow
       {
           get;
           set;
       }

    }
}
