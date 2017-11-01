using System;
using System.Collections.Generic;
 
using TranstarAuction.Model.AuctionMain;

namespace TranstarAuction.Presenters.ViewInterface
{
    public interface IMyAttentionCarUcView : IBaseView
    {
        /// <summary>
        /// 绑定结束时间实体列表
        /// </summary>
        /// <param name="EndTimeModelList"></param>
        void BindEndTimeData(List<AuctionEndTimeModel> EndTimeModelList);

        /// <summary>
        /// 得到结束时间内关注车列表
        /// </summary>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        void BindAttentionCar11(List<IAttentionCarModel> AttentionCarModelList);


        /// <summary>
        /// 得到结束时间内关注车列表
        /// </summary>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        void BindAttentionCar16(List<IAttentionCarModel> AttentionCarModelList);

        /// <summary>
        /// 得到结束时间内关注车列表
        /// </summary>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        void BindAttentionCarOther(List<IAttentionCarModel> AttentionCarModelList);

        /// <summary>
        /// 得到结束时间车数量
        /// </summary>
        /// <param name="endTimeCarCountModelList"></param>
        void GetEndTimeCarCount(List<AuctionEndTimeModel> endTimeCarCountModelList);


        /// <summary>
        /// 竞价
        /// </summary>
        /// <param name="model"></param>
        void AuctionPrice(string result);

        /// <summary>
        /// 关注或取消关注
        /// </summary>
        /// <param name="result"></param>
        void AddOrUptAtt(bool result);

        string CurrentPanelName { get; }
    }
}
