using System;
using System.Collections.Generic;
 
using TranstarAuction.Model.AuctionMain;
using TranstarAuction.Model;

namespace TranstarAuction.Presenters.ViewInterface
{
    public interface IAuctionForm : IBaseView
    {
        /// <summary>
        /// 绑定当前竞价记录
        /// </summary>
        /// <param name="auctionRecord"></param>
        void BindAuctionRecord(AuctionPriceHistoryModel auctionRecord);

        /// <summary>
        /// 绑定历史竞价记录
        /// </summary>
        /// <param name="auctionRecordHistory"></param>
        void BindAuctionRecordHistory(List<AuctionPriceHistoryModel> auctionRecordHistory);

        /// <summary>
        /// 竞价
        /// </summary>
        /// <param name="model"></param>
        void AuctionPrice(string result);

        /// <summary>
        /// 设置机器人
        /// </summary>
        /// <param name="result"></param>
        void  ReturnAddBidRobot(string result);

        /// <summary>
        /// 取消机器人设置
        /// </summary>
        /// <param name="?"></param>
        void CancelBidRobot(bool result);

        /// <summary>
        /// 上否设置机器人
        /// </summary>
        /// <param name="result"></param>
        void IsSetRobot(bool result);

        /// <summary>
        /// 绑定车基本信息
        /// </summary>
        /// <param name="carBaseModel"></param>
        void BindCarBaseInfo(AuctionCarModel carBaseModel);

        /// <summary>
        /// 绑定买卖家信息
        /// </summary>
        /// <param name="model"></param>
        void BindVendorInfo(VendorModel model);

        /// <summary>
        /// 得到火眼Id
        /// </summary>
        /// <param name="fireEyeCarId"></param>
        void GetFireEyeCarId(int fireEyeCarId);

        /// <summary>
        /// 关注或取消关注
        /// </summary>
        /// <param name="result"></param>
        void AddOrUptAtt(bool result);

        /// <summary>
        /// 获取机器人设置
        /// </summary>
        /// <param name="model"></param>
        void BindBidRobotInfo(BidRebotModel model);


        void IsSetRobotNotLoad(bool result);
        
       void BindPublishCondtition(bool result);

       void BindGetPublishCondtition(AuctionTstConditionModel model);

        /// <summary>
        /// 返回加价幅度数据
        /// </summary>
       void BindAddPriceRange(AddPriceRangeModel model);
    }
}
