using System;
using System.Collections.Generic;

using TranstarAuction.Model.AuctionMain;

namespace TranstarAuction.Presenters.ViewInterface
{
   public interface IRightConfirView
    {
        /// <summary>
        /// 是否显示物流运输
        /// </summary>
        /// <param name="BrandId"></param>
        /// <returns></returns>
        void IsSysRounte(bool result);

       /// <summary>
       /// 显示物流城市
       /// </summary>
       /// <param name="cityModelList"></param>
        void BindCity(List<AuctionEndTimeModel> cityModelList);
       /// <summary>
       /// 显示物流费
       /// </summary>
       /// <param name="price"></param>
       void BindLgsPrice(decimal price);
    }
}
