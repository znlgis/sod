using System;
using System.Collections.Generic;
 
using TranstarAuction.Model.AuctionMain;
using System.ComponentModel;

namespace TranstarAuction.Presenters.ViewInterface
{
    public interface IMyBuyCarView
    {
        void PushDataList(List<IBuyCarModel> models);

        void BindReviveCar(bool result);
    }
}
