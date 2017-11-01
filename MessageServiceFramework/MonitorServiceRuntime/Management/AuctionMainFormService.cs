using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TranstarAuction.Model.AuctionMain;
using TranstarAuction.Model;
using TranstarAuctionBIZ;
using TranstarAuction.Common;
using TranstarAuction.Service.Basic;
using TranstarAuction.Service.Runtime;
using TranstarAuction.Repository.Entitys;
using TranstarAuction.Repository.SqlMapDAL;
namespace TranstarAuction.Service
{
    public class AuctionMainFormService : ServiceBase
    {
        /// <summary>
        /// 获取主窗体标题
        /// </summary>
        /// <returns></returns>
        public AuctionCustomerInfoModel GetAuctionCustomerInfoModel(int TvuId)
        {

            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            return biz.getAuctionCustomerInfoModel(TvuId);
        }
        ///// <summary>
        ///// 获取主窗体关注车辆
        ///// </summary>
        ///// <returns></returns>
        //public List<AttentionCarModel> GetAuctionTitleDataList()
        //{
        //    AuctionHallBiz biz = new AuctionHallBiz();
        //    List<V_AuctionPublishCarSource_Test> Entitys = biz.GetAuctionHallDataList();
        //    var list = from item in Entitys
        //               select new AttentionCarModel()
        //               {
        //                   CarSourceId = item.CarSourceID,
        //                   PublishId = item.PublishId,
        //                   CarColor = item.CarBodyColor,
        //                   CarName = item.Car_Name,
        //                   CarYear = item.GetLicenseDate.ToString(),
        //                   CurrentAuctionMoney = Convert.ToInt32(item.HighestBidprice) == 0 ? Convert.ToInt32(item.StartPrice) : Convert.ToInt32(item.HighestBidprice),
        //                   CurrentAuctionPersonNums = item.BidCount,
        //                   Price = item.StartPrice,
        //                   StopTime = item.publishStopTime.ToString(),
        //                   SellerTvaId = item.TvaID,
        //                   SellerTvuId = item.PublishTvuId,
        //                   CarDisplacement = item.Exhaust.ToString(),
        //                   CityName = item.city_name,
        //                   ExpectTransferExpired = item.ExpectTransferExpired,
        //                   ExpectMoneyRecExpired = item.ExpectMoneyRecExpired,
        //                   Isrelocation = item.Isrelocation
        //               };
        //    return list.ToList();
        //}
        /// <summary>
        /// 获取主窗体关注车辆
        /// </summary>
        /// <returns></returns>
        public List<AttentionCarModel> GetAuctionTitleDataList(int TvaID, string dateTime)
        {
            List<AuctionHallDataModel> Entitys = GetAuctionFormDataList(TvaID, dateTime);
            var list = from item in Entitys
                       select new AttentionCarModel()
                       {
                           CarSourceId = item.CarSourceId,
                           PublishId = item.PublishId,
                           CarColor = item.CarColor,
                           CarName = item.CarType,
                           CarYear = item.Years,
                           CurrentAuctionMoney = Convert.ToDouble(item.CurrentPrices),
                           CurrentAuctionPersonNums = item.BidCount,
                           Price = Convert.ToDouble(item.StartPrices),
                           StopTime = item.StopTime,
                           SellerTvaId = item.SellerTvaId,
                           SellerTvuId = item.SellerTvuId,
                           CarDisplacement = item.CarDisplacement,
                           CityName = item.City,
                           ExpectTransferExpired = item.ExpectTransferExpired,
                           ExpectMoneyRecExpired = item.ExpectMoneyRecExpired,
                           Isrelocation = item.Isrelocation,
                           AttentionState = item.AttentionState,
                           BidStatus = item.BidStatus,
                           DataStatus = item.DataStatus,
                           OriginalPrice = item.CurrentPrices,
                           IsSetRebot = item.IsSetRebot,
                           IsBid = item.IsBid,
                           BrandId = item.BrandId,
                           ServerTime = DateTime.Now,
                           StartTime = item.StartTime,
                           CarDemand = item.CarDemand
                       };
            return list.ToList();
        }
        /// <summary>
        /// 获取主窗体数据
        /// </summary>
        /// <param name="TvaID"></param>
        /// <param name="dateTime"></param>
        /// <param name="carName"></param>
        /// <param name="TypeOrderbyStr"></param>
        /// <returns></returns>
        public List<AuctionHallDataModel> GetAuctionFormDataList(int TvaID, string dateTime)
        {
            PublishContext pc = new PublishContext();
            AuctionMainFormBiz amfb = new AuctionMainFormBiz();
            AuctionHallBiz biz = new AuctionHallBiz();
            List<AuctionHallDataModel> pidnewlist = new List<AuctionHallDataModel>();
            string key = string.Format("form_{0}", dateTime);
            string hallkey = string.Format("hall_{0}", dateTime);
            List<AuctionHallDataModel> pidlist = base.CurrentContext.Session.Get<List<AuctionHallDataModel>>(key);
            if (pidlist == null)
            {
                pidlist = new List<AuctionHallDataModel>();
                pidlist = biz.GetAuctionHallDataList(TvaID, dateTime, "", "", 1);
                base.CurrentContext.Session.Set<List<AuctionHallDataModel>>(key, pidlist);
                pidnewlist = pidlist;
            }
            else if (pidlist.Count >= 0)
            {
                //所有数据都更新成初始状态
                pidlist.RemoveAll(p => p.DataStatus == 2);
                pidlist.ForEach(p => p.DataStatus = 0);
                string key1 = "add";
                List<long> addList = base.CurrentContext.Session.Get<List<long>>(key1);
                string key2 = "cancel";
                List<long> cancelList = base.CurrentContext.Session.Get<List<long>>(key2);

                #region 更改关注相关
                //修改增加关注的状态
                if (addList != null)
                {
                    foreach (long l in addList.ToArray())
                    {
                        AuctionHallDataModel ahdm = pidlist.Where(p => p.PublishId == l).FirstOrDefault();
                        if (ahdm == null)
                        {
                            //if (base.CurrentContext.Session.Get<List<AuctionHallDataModel>>(hallkey) == null)
                            //{
                            //    if (base.CurrentContext.Session.Get<List<AuctionHallDataModel>>("hall_" + DateTime.Now.ToString("yyyy-MM-dd")) != null)
                            //    {
                            //        AuctionHallDataModel model = base.CurrentContext.Session.Get<List<AuctionHallDataModel>>("hall_" + DateTime.Now.ToString("yyyy-MM-dd")).Where(p => p.PublishId == l).FirstOrDefault();
                            //        if (model != null)
                            //        {
                            //            AuctionHallDataModel model1 = (AuctionHallDataModel)base.CurrentContext.Session.Get<List<AuctionHallDataModel>>("hall_" + DateTime.Now.ToString("yyyy-MM-dd")).Where(p => p.PublishId == l).FirstOrDefault().Clone();
                            //            model1.DataStatus = 3;
                            //            model1.AttentionState = "1";
                            //            pidlist.Add(model1);
                            //        }
                            //    }
                            //}
                            if (base.CurrentContext.Session.Get<List<AuctionHallDataModel>>(hallkey) != null)
                            {
                                AuctionHallDataModel model = base.CurrentContext.Session.Get<List<AuctionHallDataModel>>(hallkey).Where(p => p.PublishId == l).FirstOrDefault();
                                if (model != null)
                                {
                                    AuctionHallDataModel model1 = (AuctionHallDataModel)base.CurrentContext.Session.Get<List<AuctionHallDataModel>>(hallkey).Where(p => p.PublishId == l).FirstOrDefault().Clone();
                                    model1.DataStatus = 3;
                                    model1.AttentionState = "1";
                                    pidlist.Add(model1);
                                }
                            }
                        }
                    }
                }
                //修改取消关注的状态
                if (cancelList != null)
                {
                    foreach (long l in cancelList.ToArray())
                    {
                        AuctionHallDataModel ahdm = (AuctionHallDataModel)pidlist.Where(p => p.PublishId == l && p.AttentionState == "1").FirstOrDefault();
                        if (ahdm != null)
                        {
                            ahdm.AttentionState = "0";
                            ahdm.DataStatus = 2;
                        }
                    }
                }
                #endregion

                #region  增加竞价的model
                //if (base.CurrentContext.Session.Get<List<AuctionHallDataModel>>(hallkey) != null)
                //{
                //    List<AuctionHallDataModel> addModelList = base.CurrentContext.Session.Get<List<AuctionHallDataModel>>(hallkey).Where(p => p.BidStatus == "1" || p.BidStatus == "2").ToList();
                //    if (addModelList != null)
                //    {
                //        foreach (AuctionHallDataModel model in addModelList)
                //        {
                //            if (pidlist.Where(p => p.PublishId == model.PublishId).FirstOrDefault() == null)
                //            {
                //                AuctionHallDataModel newmodel =(AuctionHallDataModel) model.Clone();
                //                newmodel.DataStatus = 3;
                //                pidlist.Add(newmodel);
                //            }
                //        }
                //    }

                //}
                #endregion


                //判断状态是0,1,2,3的拍品
                List<AuctionHallDataModel> Sta1List = pidlist.Where(p => p.BidStatus == BidStatusType.确认条件 || p.BidStatus == BidStatusType.您是最高价 || p.BidStatus == BidStatusType.出价不是最高).ToList();
                if (Sta1List != null)
                {

                    foreach (AuctionHallDataModel ahdm in Sta1List)
                    {
                        //更改拍品状态
                        if (ahdm.AuctionStatus != biz.GetPublishTime(ahdm.PublishId, TvaID).AuctionStatus)
                        {
                            //更改数据源状态
                            ahdm.DataStatus = 1;
                            ahdm.AuctionStatus = biz.GetPublishTime(ahdm.PublishId, TvaID).AuctionStatus;
                        }
                        //判断机器人状态是否改变
                        if (ahdm.IsSetRebot != amfb.isSetRobotFromCache(TvaID, ahdm.PublishId))
                        {
                            //更改数据源状态
                            if (ahdm.DataStatus == 0)
                            {
                                ahdm.DataStatus = 1;
                            }
                            ahdm.IsSetRebot = amfb.isSetRobotFromCache(TvaID, ahdm.PublishId);
                        }
                        //判断关注是否发生变化
                        //if (ahdm.AttentionState != amfb.isSetAttention(TvaID, ahdm.PublishId).ToString())
                        //{
                        //    //更改数据源状态
                        //    ahdm.DataStatus = 1;
                        //    ahdm.AttentionState = amfb.isSetAttention(TvaID, ahdm.PublishId).ToString();
                        //}
                        //提前选择成交
                        if (Convert.ToInt32(ahdm.AuctionStatus) > 2)
                        {
                            //更改数据源状态
                            ahdm.DataStatus = 1;

                            pc.ahdm = ahdm;
                            pc.StartStatus();
                        }
                        //竞价时间结束
                        if (DateTime.Compare(Convert.ToDateTime(ahdm.StopTime), DateTime.Now) < 0)
                        {
                            //更改数据源状态
                            ahdm.DataStatus = 1;

                            pc.ahdm = ahdm;
                            pc.StartStatus();

                        }
                        //价格变化更改状态
                        AuctionPriceHistoryModel currModel = amfb.GetHighestBidRecord(ahdm.CarSourceId, ahdm.PublishId);
                        if (ahdm.CurrentPrices < currModel.AuctionPrice)
                        {

                            // 不是新增,删除的状态更改状态
                            if (ahdm.DataStatus == 0)
                            {
                                ahdm.DataStatus = 1;
                            }
                            //更改当前价
                            ahdm.CurrentPrices = currModel.AuctionPrice;
                            //更改结束时间
                            ahdm.StopTime = biz.GetPublishTime(ahdm.PublishId, TvaID).StopTime;
                            //更改拍品状态
                            ahdm.AuctionStatus = biz.GetPublishTime(ahdm.PublishId, TvaID).AuctionStatus;
                            pc.ahdm = ahdm;
                            pc.StartStatus();
                        }

                    }
                }
                //判断状态是3的拍品
                List<AuctionHallDataModel> Sta3List = pidlist.Where(p => p.BidStatus == BidStatusType.竞价结束).ToList();
                if (Sta3List != null)
                {
                    foreach (AuctionHallDataModel ahdm in Sta3List)
                    {
                        //更改拍品状态
                        if (ahdm.AuctionStatus != biz.GetPublishTime(ahdm.PublishId, TvaID).AuctionStatus)
                        {
                            //更改数据源状态
                            ahdm.DataStatus = 1;
                            ahdm.AuctionStatus = biz.GetPublishTime(ahdm.PublishId, TvaID).AuctionStatus;
                        }
                        pc.ahdm = ahdm;
                        pc.StartStatus();
                    }
                }
                pidnewlist = pidlist.Where(p => p.DataStatus > 0).ToList();

            }
            return pidnewlist;
        }
        ///// <summary>
        ///// 获取当前价
        ///// </summary>
        ///// <param name="carSourceID"></param>
        ///// <param name="pId"></param>
        ///// <returns></returns>
        //public AuctionPriceHistoryModel GetBidRecord(int carSourceID, long pId)
        //{
        //    AuctionMainFormBiz biz = new AuctionMainFormBiz();
        //    string key = string.Format("price_{0}", pId);
        //    AuctionPriceHistoryModel model = base.CurrentContext.Cache.Get<AuctionPriceHistoryModel>(key);
        //    if (model == null)
        //    {
        //        model = new AuctionPriceHistoryModel();
        //        model = biz.GetBidRecord(carSourceID, pId);
        //        base.CurrentContext.Cache.Insert<AuctionPriceHistoryModel>(key, model);
        //    }
        //    else
        //    {
        //        AuctionPriceHistoryModel aphm = biz.GetBidRecord(carSourceID, pId);
        //        if (model.AuctionPrice < aphm.AuctionPrice)
        //        {
        //            model = aphm;
        //            base.CurrentContext.Cache.Remove(key);
        //            base.CurrentContext.Cache.Insert<AuctionPriceHistoryModel>(key, aphm);
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    return model;
        //}
        /// <summary>
        /// 获取当前价
        /// </summary>
        /// <param name="carSourceID"></param>
        /// <param name="pId"></param>
        /// <returns></returns>
        public AuctionPriceHistoryModel GetHighestRecord(int carSourceID, long pId)
        {
            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            return biz.GetBidRecord(carSourceID, pId);
        }
        /// <summary>
        /// 获取竞价记录
        /// </summary>
        /// <param name="carSourceID"></param>
        /// <param name="pId"></param>
        /// <returns></returns>
        public List<AuctionPriceHistoryModel> GetBidRecordList(int carSourceID, long pId)
        {
            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            return biz.GetBidRecordList(carSourceID, pId);
        }
        /// <summary>
        /// 获取我的竞价记录
        /// </summary>
        /// <param name="carSourceID"></param>
        /// <param name="pId"></param>
        /// <param name="TvuID"></param>
        /// <returns></returns>
        public List<AuctionPriceHistoryModel> GetMyBidRecord(int carSourceID, long pId, int TvaID)
        {
            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            return biz.GetMyBidRecord(carSourceID, pId, TvaID);
        }
        /// <summary>
        /// 出价
        /// </summary>
        /// <param name="carSourceId">车源ID</param>
        /// <param name="publishId">拍品ID</param>
        /// <param name="sellerTvaId">卖家经销商ID</param>
        /// <param name="sellerTvuId">卖家用户ID</param>
        /// <param name="buyerId">买家用户ID</param>
        /// <param name="buyerVendorId">买家经销商ID</param>
        /// <param name="price">加价后价格万元</param>
        /// <param name="logisticsMode">物流方式</param>
        /// <param name="sysRouteId">路线ID</param>
        /// <param name="originalPrice">当前价万元</param>
        /// <param name="priceDegree">加价幅度元</param>
        /// <param name="isNew">true:最新路线和物流方式,false:取上一次竞价路线和物流方式</param>
        /// <returns>00-成功 01-抵押保证金余额不足 02-出价必须高于当前最高价 03-此次拍卖已经结束 04-出价已截止 05-出价已更新</returns>
        public string AuctionBid(AttentionCarModel acm)
        {
            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            if (!acm.IsNew)
            {
                AuctionEndTimeModel aem = biz.GetRouteWay(acm.BuyerVendorId, acm.PublishId);
                acm.LogisticsMode = Convert.ToInt32(aem.key);
                acm.SysRouteId = Convert.ToInt32(aem.value);
            }
            return biz.AuctionBid(acm.CarSourceId, acm.PublishId, acm.SellerTvaId, acm.SellerTvuId, acm.BuyerId, acm.BuyerVendorId, Convert.ToDecimal(acm.CurrentAuctionMoney), acm.LogisticsMode, acm.SysRouteId, Convert.ToDecimal(acm.OriginalPrice), Convert.ToDecimal(acm.PriceDegree));

        }
        /// <summary>
        /// 获取路线列表
        /// </summary>
        /// <returns></returns>
        public List<AuctionEndTimeModel> GetLgtRouteList()
        {
            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            return biz.GetLgtRouteList();
        }
        public string GetCityName(string sysRouterId)
        {
            AuctionEndTimeModel aem=GetLgtRouteList().Find(p => p.key == sysRouterId);
            string cityname = "";
            if (aem != null)
            {
                cityname = aem.value;
            }
            return cityname;
        }
        /// <summary>
        /// 获取各个结束点的数量
        /// </summary>
        /// <returns></returns>
        public List<AuctionEndTimeModel> GetEndTimeCount(int TvaID)
        {
            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            return biz.GetEndTimeCount(TvaID);
        }
        /// <summary>
        /// 获取所有结束点的数量
        /// </summary>
        /// <returns></returns>
        public List<AuctionEndTimeModel> GetAllEndTime(int TvaID, string carname)
        {
            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            return biz.GetEndTimeCount(TvaID, carname);
        }
        /// <summary>
        /// 获取车的数量
        /// </summary>
        /// <returns></returns>
        public int GetCarCount(int TvaID, string carname)
        {
            AuctionHallBiz biz = new AuctionHallBiz();
            return biz.GetAuctionHallDataList(TvaID, DateTime.Now.ToString("yyyy-MM-dd"), carname, "", 0).Count;
        }
        /// <summary>
        /// 增加机器人出价
        /// </summary> 
        /// <param name="bm"></param>
        /// <returns></returns>
        public string AddBidRobot(BidRebotModel bm)
        {

            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            if (!biz.HasMoneySetRebot(bm.PublishId, bm.MaxPrice, bm.BuyTvaId))
            {
                return "02";
            }
            if (biz.AddBidRobot(bm))
            {
                return "00";
            }
            else
            {
                return "01";
            }
        }
        /// <summary>
        /// 取消机器人出价
        /// </summary>
        /// <param name="TvaID"></param>
        /// <param name="PId"></param>
        public bool CancelBidRobot(int TvaID, long PId)
        {
            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            return biz.CancelBidRobot(TvaID, PId);
        }
        #region  获取机器人信息
        /// <summary>
        /// 获取机器人信息
        /// </summary>
        /// <param name="TvaID">经销商id</param>
        /// <param name="PId">拍品id</param>
        /// <returns></returns>
        public BidRebotModel GetBidRobot(int TvaID, long PId)
        {
            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            return biz.GetBidRobot(TvaID, PId);
        }
        #endregion
        /// <summary>
        /// 获取竞价的路线
        /// </summary>
        /// <param name="TvaID">经销商id</param>
        /// <param name="PId">拍品id</param>
        public AuctionEndTimeModel GetRouteWay(int TvaID, long PId)
        {
            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            return biz.GetRouteWay(TvaID, PId);
        }
        /// <summary>
        /// 是否设置机器人
        /// </summary>
        /// <param name="TvaID"></param>
        /// <param name="PId"></param>
        /// <returns></returns>
        public bool isSetRobot(int TvaID, long PId)
        {
            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            return biz.isSetRobot(TvaID, PId);
        }
        /// <summary>
        /// 获取车的基本信息
        /// </summary>
        /// <param name="carsourceId">车源id</param>
        /// <returns></returns>
        public AuctionCarModel GetCarSourceInfo(int carsourceId)
        {
            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            return biz.GetCarSourceInfo(carsourceId);
        }
        /// <summary>
        /// 获取拍品车源所有图片地址字符串的方法(new拍品详细页用)
        /// </summary>
        /// <param name="carSourceId"></param>
        /// <returns></returns>
        public IList<string> GetAllPictureUrlStrings(int carSourceId)
        {
            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            return biz.GetAllPictureUrlStrings(carSourceId);
        }
        /// <summary>
        /// 返回经销商信息
        /// </summary>
        /// <param name="CarSourceId"></param>
        /// <returns></returns>
        public VendorModel GetVendorInfo(int carSourceId, int TvaId)
        {
            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            return biz.GetVendorInfo(carSourceId, TvaId);
        }

        public override bool ProcessRequest(IServiceContext context)
        {
            context.SessionRequired = true;
            return base.ProcessRequest(context); //请保留此行，否则在具体的方法里面可能无法获取 CurrentContext 属性
        }

        #region 消息类
        /// <summary>
        /// 删除的指定的消息列表
        /// </summary>
        /// <param name="Ids">多个用逗号分隔</param>
        public void DelMessage(string Ids)
        {
            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            biz.DelMessage(Ids);
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="TvaID">经销商id</param>
        /// <param name="Ids">消息id,多个用,号分隔</param>
        public void UptMessage(int TvaID, string Ids)
        {
            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            biz.UptMessage(TvaID, Ids);
        }

        /// <summary>
        /// 获取消息列表,客户端需要使用全局连接
        /// </summary>
        /// <param name="Tvaid">经销商id</param>
        /// <param name="MsgID">消息id</param>
        /// <param name="flag">0:向上取1：向下取</param>
        /// <returns></returns>
        public List<NotifyMessageModel> GetMessageList(int Tvaid, long MsgID, int flag)
        {
            return new List<NotifyMessageModel>();
            //2012.8.14 因为怀疑数据库CPU问题，屏蔽以下代码，客户端不获取消息
            //AuctionMainFormBiz biz = new AuctionMainFormBiz();
            //List<NotifyMessageModel> list = biz.GetMessageList(Tvaid, MsgID, flag);
            //return list;

        }
        /// <summary>
        /// 获取火眼Id
        /// </summary>
        /// <param name="carID"></param>
        /// <returns></returns>
        public int GetHYCarID(int carID)
        {
            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            return biz.GetHYCarID(carID);
        }
        #endregion

        #region 缓存黑名单列表
        /// <summary>
        /// 黑名单列表
        /// </summary>
        /// <param name="tvaid">经销商ID</param>
        /// <returns></returns>
        public List<int> GetAllBlackTvas(int tvaid)
        {
            List<int[]> dic = GetAllBlackMan();

            var result = from p in dic
                         where p[0] == tvaid
                         select p[1];
            return result.ToList();
        }
        /// <summary>
        /// 缓存黑名单列表
        /// </summary>
        /// <returns></returns>
        private List<int[]> GetAllBlackMan()
        {
            return base.GlobalCache.Get<List<int[]>>("GetAllBlackMan",
                () =>
                {
                    AuctionHallBiz biz = new AuctionHallBiz();
                    return biz.GetAllBlackMan();
                },
                new System.Runtime.Caching.CacheItemPolicy()
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(60)
                }
                );
        }
        #endregion

        #region 是否采用物流方式
        /// <summary>
        /// 是否采用物流方式
        /// </summary>
        /// <param name="BrandId">品牌id</param>
        /// <returns></returns>
        public bool IsSysRounte(string CityName, int carsourceId)
        {
            bool IsSysRounte = false;
            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            IsSysRounte = biz.IsSysRounte(CityName, carsourceId);
            //if (city != "北京") IsSysRounte = false;
            return IsSysRounte;
        }
        #endregion

        #region 获取伙伴列表
        /// <summary>
        /// 获取伙伴列表
        /// </summary>
        /// <param name="tvaid"></param>
        /// <returns></returns>
        public List<MyFriendModel> GetFriendList(int tvaid)
        {

            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            return biz.GetFriendList(tvaid);

        }
        #endregion

        #region 插入拍品付款和物流信息
        /// <summary>
        /// 插入拍品付款和物流信息
        /// </summary>
        /// <param name="atcm">实体</param>
        /// <returns></returns>
        public bool AddPublishCondtition(AuctionTstConditionModel atcm)
        {
            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            bool result = true;
            try
            {
                //Web 可能已经确认过
                result = biz.AddPublishCondtition(atcm);
            }
            catch
            {
            }
            if (result == true)
            {
                string dateTime = DateTime.Now.ToString("yyyy-MM-dd");
                string key = string.Format("hall_{0}", dateTime);
                List<AuctionHallDataModel> pidlist = base.CurrentContext.Session.Get<List<AuctionHallDataModel>>(key);
                AuctionHallDataModel ahdm = pidlist.FirstOrDefault(p => p.PublishId == atcm.PublishId);
                if (ahdm != null)
                {
                    ahdm.BidStatus = BidStatusType.出价不是最高;
                    ahdm.DataStatus = 5;
                    System.Diagnostics.Debug.WriteLine("确认按钮OK");
                }

            }
            return result;
        }
        #endregion

        #region 获取拍品付款和物流信息列表
        /// <summary>
        /// 获取拍品付款和物流信息列表
        /// </summary>
        /// <param name="pid">拍品id</param>
        /// <param name="tvaid">经销商id</param>
        /// <returns></returns>
        public AuctionTstConditionModel GetPublishCondtition(long pid, int tvaid)
        {
            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            return biz.GetPublishCondtition(pid, tvaid);
        }
        #endregion

        #region 获取交易订单的拍品列表
        /// <summary>
        /// 获取交易订单的拍品列表
        /// </summary>
        /// <param name="tvaid">经销商id</param>
        /// <returns></returns>
        public List<BuyCarModel> GetBuyCarModelList(int tvaid)
        {
            //return null;
            string key = "CarList";
            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            List<BuyCarModel> bcmlist = base.CurrentContext.Session.Get<List<BuyCarModel>>(key);
            if (bcmlist == null)
            {
                bcmlist = new List<BuyCarModel>();
                bcmlist = biz.GetBuyCarModelList(tvaid);
                base.CurrentContext.Session.Set<List<BuyCarModel>>(key, bcmlist);
            }
            else if (bcmlist.Count >= 0)
            {
                bcmlist.ForEach(p => p.DataStatus = 0);
                List<BuyCarModel> middlelist = GetAllBuyCarModelList();
                foreach (BuyCarModel bcm in bcmlist.ToList())
                {
                    BuyCarModel buycarmodel = middlelist.Find(p => p.OrderId == bcm.OrderId);
                    if (buycarmodel != null)
                    {
                        if (bcm.OrderState != buycarmodel.OrderState || bcm.ArbState != buycarmodel.ArbState || bcm.TstResult != buycarmodel.TstResult)
                        {
                            bcmlist.RemoveAll(p => p.OrderId == bcm.OrderId);
                            buycarmodel.DataStatus = 1;
                            bcmlist.Add(buycarmodel);
                        }
                    }
                }
                List<BuyCarModel> entitylist = middlelist.FindAll(p => p.BuyTvaId == tvaid && p.TstResult == TstResult.交易中);
                List<BuyCarModel> tempList = bcmlist.FindAll(p => p.TstResult == TstResult.交易中);
                if (entitylist != null && entitylist.Count > tempList.Count)
                {

                    var idlist = (from c in entitylist select c.OrderId).Except(from d in tempList select d.OrderId);
                    List<int> ids = idlist.ToList();
                    foreach (int orderid in ids)
                    {
                        BuyCarModel bcm = new BuyCarModel();
                        bcm = middlelist.Find(p => p.OrderId == orderid);
                        bcm.DataStatus = 2;
                        bcmlist.Add(bcm);
                    }
                }
            }
            return bcmlist.FindAll(p => p.DataStatus != 0);
        }
        #endregion
        #region 收车
        /// <summary>
        /// 确认收车封装
        /// </summary>
        /// <param name="orderid">订单id</param>
        /// <param name="buytvuid">买家用户id</param>
        /// <returns></returns>
        public bool ConfirmTstOrder(int orderid, int buytvuid)
        {
            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            return biz.ConfirmTstOrder(orderid, buytvuid);
        }
        #endregion

        /// <summary>
        /// 获取物流费
        /// </summary>
        /// <param name="SysRouteID"></param>
        /// <param name="CarSourceID"></param>
        /// <param name="CarPrice"></param>
        /// <returns></returns>
        public decimal GetLgsPrice(int SysRouteID, int CarSourceID, decimal CarPrice)
        {
            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            decimal result = biz.GetLgsPrice(SysRouteID, CarSourceID, CarPrice);
            return result;
        }
        /// <summary>
        /// 获取快速出价和全部出价幅度列表
        /// </summary>
        /// <param name="reservationPrice"></param>
        /// <returns></returns>
        public AddPriceRangeModel GetAddPriceRangeModel(double reservationPrice)
        {

            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            return biz.GetAuctionDicAddPriceRangeModel(Convert.ToDecimal(reservationPrice));
        }
        /// <summary>
        /// 从缓存中获取所有的买到的车列表(交易中，或者今天完成)
        /// </summary>
        /// <returns></returns>
        private List<BuyCarModel> GetAllBuyCarModelList()
        {
            return base.GlobalCache.Get<List<BuyCarModel>>("Service_GetAllBuyCarModelList",
                () =>
                {
                    AuctionMainFormBiz biz = new AuctionMainFormBiz();
                    return biz.GetAllBuyCarModelList();
                },
                new System.Runtime.Caching.CacheItemPolicy()
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(1)
                }
                );
        }


        public DateTime GetServerDateTime()
        {
            return DateTime.Now;
        }
        public DateTime GetServerDateTime2()
        {
            System.Threading.Thread.Sleep(10000);
            return DateTime.Now;
        }

        /// <summary>
        /// 获取车源所在地城市编号
        /// </summary>
        /// <param name="carSourceID"></param>
        /// <returns></returns>
        private int GetLicenseCityId(Int32 carSourceID)
        {
            PCUtils util = new PCUtils();
            return (int)util.LicenseCityId(carSourceID);
        }

        /// <summary>
        /// 获取车源拍卖人指定的过户地址，供拍买人选择。如果只有一个，则不选择。
        /// </summary>
        /// <param name="carSourceId">车源ID</param>
        /// <param name="publishId">拍品ID</param>
        /// <returns>过户地址数组,可能为空</returns>
        public string[] AuctionTransferAddressName(int carSourceId,long publishId)
        {
            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            return biz.AuctionTransferAddressName(carSourceId, Convert.ToInt32( publishId));
        }

        /// <summary>
        /// 获取 拍品表过户地点ID
        /// </summary>
        /// <param name="publishId">拍品ID</param>
        /// <param name="carSourceId">车源ID</param>
        /// <param name="isTakeBySelf">是否上门自提</param>
        /// <param name="guoHuAddrName">过户地址名称</param>
        /// <param name="transferType">1本地过户 2外迁过户</param>
        /// <returns></returns>
        public int GetSysAddressId(long publishId, int carSourceId, bool isTakeBySelf, string guoHuAddrName, int transferType)
        {
            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            return biz.GetSysAddressId(Convert.ToInt32( publishId), carSourceId, isTakeBySelf, guoHuAddrName, transferType);
        }

    }
}
