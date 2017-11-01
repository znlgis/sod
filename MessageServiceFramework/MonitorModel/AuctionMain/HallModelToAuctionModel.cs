using System;
using System.Collections.Generic;
using System.Text;

namespace TranstarAuction.Model.AuctionMain
{
   public  class HallModelToAuctionModel
    {
       public static IAttentionCarModel TransferModel(IAuctionHallDataModel hModel)
       {
           IAttentionCarModel model = new AttentionCarNotifyModel();
           model.AttentionState = hModel.AttentionState;
           model.AuctionState = hModel.AuctionStatus;
           model.BidStatus = hModel.BidStatus;
           model.CarColor = hModel.CarColor;
           model.CarDemand = hModel.CarDemand;
           model.CarDisplacement = hModel.CarDisplacement;
           model.CarName = hModel.CarType;
           model.CarSourceId = hModel.CarSourceId;
           model.CarYear = hModel.Years;
           model.CityName = hModel.City;
           model.CurrentAuctionMoney = hModel.CurrentPrices;
           model.Price = hModel.InitAuctionPrice;
           model.OriginalPrice = hModel.CurrentPrices;
           model.DataStatus = hModel.DataStatus;
           model.BrandId = hModel.BrandId;
           model.ExpectMoneyRecExpired = hModel.ExpectMoneyRecExpired;
           model.ExpectTransferExpired = hModel.ExpectTransferExpired;
           model.IsBid = hModel.IsBid;
           model.Isrelocation = hModel.Isrelocation;
           model.IsSetRebot = hModel.IsSetRebot;
           model.Unit = hModel.Unit;
           model.PublishId = hModel.PublishId;
           model.PublishStopTime = hModel.PublishStopTime;
           model.SellerTvaId = hModel.SellerTvaId;
           model.SellerTvuId = hModel.SellerTvuId;
           model.StartTime = hModel.StartTime;
           model.StopTime = hModel.StopTime;
           model.CurrentAuctionMoneyStr = model.CurrentAuctionMoney.ToString("0.00");
           model.IsFriend = hModel.IsFriend;
           model.IsSelfCar = hModel.IsSelfCar;
           model.ServerTime = hModel.ServerTime;
           model.IsOnLinePay = hModel.IsOnLinePay;
           model.IsBlackTraID = hModel.IsBlackTraID;
           model.CarSerialName = hModel.CarSerialName;
           model.IsAuction = hModel.IsAuction;
           model.KeepPrice = hModel.KeepPrice;
           model.OnlineOrOffline = hModel.OnlineOrOffline;
           model.IsAgentPay = hModel.IsAgentPay;
           model.IsAgentTransfer = hModel.IsAgentTransfer;
           return model;
       }
    }
}
