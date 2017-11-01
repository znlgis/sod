using System;
using System.Collections.Generic;
 
using TranstarAuction.Model.AuctionMain;

namespace TranstarAuction.Presenters.ViewInterface
{
    public interface ICityForm
    {
        void GetCityData(List<AuctionEndTimeModel> cityList);
    }
}
