using System;
using System.Collections.Generic;
 
using TranstarAuction.Model;
using TranstarAuction.Model.AuctionMain;

namespace TranstarAuction.Presenters.ViewInterface
{
    public interface IAuctionHallView : IBaseView
    {
        /// <summary>
        /// 绑定拍卖大厅数据
        /// </summary>
        /// <param name="auctionHallDataModel">返回LIST</param>
        void ShowAuctionHallData(List<AuctionHallDataNotifyModel> auctionHallDataModel, string dateTime);
        /// <summary>
        /// 绑定搜索数据
        /// </summary>
        /// <param name="carSearchResultModel">返回LIST</param>
        void ShowCarSearchData(List<CarSearchResultModel> carSearchResultModel);
        /// <summary>
        /// 添加关注
        /// </summary>
        /// <param name="isGZ">关注是否成功</param>
        /// <param name="pID">拍品ID</param>
        void AddOrUptAtt(bool isGZ, long pID);
        /// <summary>
        /// 出价
        /// </summary>
        /// <param name="isResult"></param>
        void AuctionBid(string isResult);
        /// <summary>
        /// 回去汇总数据
        /// </summary>
        /// <param name="am"></param>
        void GetAllEndTime(int count);
    }
}
