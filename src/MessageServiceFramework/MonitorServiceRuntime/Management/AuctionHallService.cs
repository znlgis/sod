using System;
using System.Collections.Generic;
using System.Linq;
using TranstarAuction.Model;
using TranstarAuction.Model.AuctionMain;
using TranstarAuction.Service.Runtime;
using TranstarAuctionBIZ;
using System.Threading.Tasks;
namespace TranstarAuction.Service
{
    public class AuctionHallService : ServiceBase
    {
        //用于并行计算的线程锁对象
        private object sync_lock_obj1 = new object();

        /// <summary>
        /// 获取大厅数据
        /// </summary>
        /// <param name="TvaID"></param>
        /// <param name="dateTime"></param>
        /// <param name="carName"></param>
        /// <param name="TypeOrderbyStr"></param>
        /// <returns></returns>
        public List<AuctionHallDataModel> GetAuctionHallDataList(int TvaID, string dateTime, string carName, string TypeOrderbyStr)
        {
            //this.CurrentContext.ParallelExecute = false;

            AuctionMainFormBiz amfb = new AuctionMainFormBiz();
            AuctionHallBiz biz = new AuctionHallBiz();
            PublishContext pc = new PublishContext();
            List<AuctionHallDataModel> pidnewlist = new List<AuctionHallDataModel>();
            //大厅的各个整点的缓存key
            string key = string.Format("hall_{0}", dateTime);
            //bool firstRequest = false;//是否首次访问

            List<AuctionHallDataModel> sessionDataList = base.CurrentContext.Session.Get<List<AuctionHallDataModel>>(key);
            //注意，这里TotalList 只能是本地缓存，不能是全局缓存
            List<AuctionHallDataModel> TotalList = base.CurrentContext.Cache.Get<List<AuctionHallDataModel>>("totalPublishList", () =>
                {
                    lock (sync_lock_obj1)
                    {
                        List<AuctionHallDataModel> result = base.CurrentContext.Cache.Get<List<AuctionHallDataModel>>("totalPublishList");
                        if (result == null)
                            result = biz.GetNewAuctionHallDataList();
                        return result;
                    }
                },
                new System.Runtime.Caching.CacheItemPolicy()
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(24 - DateTime.Now.Hour)
                });
            if (sessionDataList == null)
            {
                sessionDataList = new List<AuctionHallDataModel>();
                //sessionDataList = biz.GetAuctionHallDataList(TvaID, dateTime, "", TypeOrderbyStr, 0);
                List<long> showPublishList = biz.ShowPublishlist(TvaID);
                List<long> attentionPL = biz.AttPublishlist(TvaID);
                foreach (AuctionHallDataModel model in TotalList)
                {
                    if (model.IsShow == false) //有的拍品可能为IsShow == false
                    {
                        if (showPublishList.Find(p => p == model.PublishId) == 0)
                            continue;
                    }
                    AuctionHallDataModel entity = (AuctionHallDataModel)model.Clone();
                    if (attentionPL.Find(p => p == model.PublishId) != 0)
                        entity.AttentionState = "1";
                    entity.BuyerVendorId = TvaID;
                    pc.ahdm = entity;
                    pc.SetStatus(new AuctionState());
                    pc.StartStatus();
                    sessionDataList.Add(entity);

                }
                base.CurrentContext.Session.Set<List<AuctionHallDataModel>>(key, sessionDataList);
                pidnewlist = sessionDataList;

                SetTodayDateTimeType(pidnewlist);
            }
            else
            {
                //将合适的数据都更新成初始状态
                sessionDataList.ForEach(
                    p =>
                    {
                        if (p.DataStatus == 5)
                        {
                            p.DataStatus = 1;
                        }
                        else
                        {
                            if (p.DataStatus == 2) //上次标记是删除状态的，这次标记为已经删除
                            {
                                p.DataStatus = 4;
                            }
                            else
                            {
                                if (p.DataStatus != 4) //以下程序的处理都要排除
                                    p.DataStatus = 0;
                            }
                        }
                    }
                    );
                #region 获取所有拍品列表

                List<AuctionHallDataModel> modelList = GetAllPublishDataListByMinute();//得出是否有新的拍品
                #endregion
                if (modelList != null) //&& modelList.Count > TotalList.Count(p=>p.BidStatus<BidStatusType.竞价结束)
                {
                    ////Except 得出是否有新的拍品ID
                    //var idlist = (from c in modelList select c.PublishId).Except(from d in TotalList select d.PublishId);
                    //List<long> ids = idlist.ToList();
                    Parallel.ForEach(modelList, item =>
                    {
                        //AuctionHallDataModel model = biz.GetPublishInfo(pid, TvaID);
                        //if (model != null && model.PublishId > 0)
                        //{
                        //    model.DataStatus = 3;
                        //    sessionDataList.Add(model);
                        //}
                        List<long> showPublishList = biz.ShowPublishlist(TvaID);
                        List<long> attentionPL = biz.AttPublishlist(TvaID);
                        AuctionHallDataModel model = item;// modelList.FirstOrDefault(p => p.PublishId == pid);
                        bool falg = true;
                        if (model.IsShow == false)
                        {
                            if (showPublishList.Find(p => p == model.PublishId) == 0)
                                falg = false;

                        }
                        //base.CurrentContext.BatchIndex
                        if (falg == true)
                        {
                            if (sessionDataList.Find(p => p.PublishId == model.PublishId) == null)
                            {
                                AuctionHallDataModel entity = (AuctionHallDataModel)model.Clone();
                                if (attentionPL.Find(p => p == model.PublishId) != 0)
                                    entity.AttentionState = "1";
                                entity.BuyerVendorId = TvaID;
                                entity.DataStatus = 3;
                                pc.ahdm = entity;
                                pc.SetStatus(new AuctionState());
                                pc.StartStatus();
                                sessionDataList.Add(entity);
                                //TotalList.Add(model);
                            }
                        }
                    });
                }

                //增加web的关注
                OptCurrentSessionAtt(TvaID);
                string key1 = "add";
                List<long> addList = base.CurrentContext.Session.Get<List<long>>(key1);
                string key2 = "cancel";
                List<long> cancelList = base.CurrentContext.Session.Get<List<long>>(key2);
                #region 更改关注相关
                //修改增加关注的状态
                if (addList != null && addList.Count > 0)
                {
                    foreach (long l in addList.ToArray())
                    {
                        AuctionHallDataModel ahdm = sessionDataList.Where(p => p.PublishId == l && p.DataStatus != 4 && p.AttentionState == "0").FirstOrDefault();
                        if (ahdm != null)
                        {
                            ahdm.AttentionState = "1";
                            ahdm.DataStatus = 1;
                        }
                    }
                }
                //修改取消关注的状态
                if (cancelList != null && cancelList.Count > 0)
                {
                    foreach (long l in cancelList.ToArray())
                    {
                        AuctionHallDataModel ahdm = sessionDataList.Where(p => p.PublishId == l && p.DataStatus != 4 && p.AttentionState == "1").FirstOrDefault();
                        if (ahdm != null)
                        {
                            ahdm.AttentionState = "0";
                            ahdm.DataStatus = 1;
                        }
                    }
                }
                #endregion
                //一次取出所有的缓存拍品
                string[] keys = new string[sessionDataList.Count];
                int count = 0;
                foreach (AuctionHallDataModel model in sessionDataList)
                {

                    keys[count] = "publishinfo_" + model.PublishId.ToString();
                    count++;

                }
                List<AuctionHallDataModel> compareModel = biz.GetPublishList(keys);//来自优信拍缓存中的拍品信息
                //判断状态是0,1,2的拍品
                List<AuctionHallDataModel> Sta1List = sessionDataList.Where(p => p.DataStatus != 4 && (p.BidStatus == BidStatusType.确认条件 || p.BidStatus == BidStatusType.您是最高价 || p.BidStatus == BidStatusType.出价不是最高)).ToList();
                if (Sta1List != null)
                {
                    //<AuctionHallDataModel>
                    Parallel.ForEach(Sta1List, ahdm =>
                    {
                        //AuctionHallDataModel tempAHD = biz.GetPublishTime(ahdm.PublishId, TvaID);
                        AuctionHallDataModel tempAHD = compareModel.Find(p => p.PublishId == ahdm.PublishId);
                        //更新拍品起拍价
                        if (double.Parse(tempAHD.StartPrices) != double.Parse(ahdm.StartPrices))
                        {
                            ahdm.DataStatus = 1;
                            ahdm.StartPrices = tempAHD.StartPrices;//StartPrices 原始起拍价？
                            ahdm.InitAuctionPrice = double.Parse(tempAHD.StartPrices);
                            ahdm.CurrentPrices = double.Parse(tempAHD.StartPrices);
                            //更改缓存起拍价
                            AuctionHallDataModel tempHC = TotalList.Find(p => p.PublishId == ahdm.PublishId);
                            if (tempHC != null)
                            {
                                tempHC.StartPrices = tempAHD.StartPrices;
                                tempHC.InitAuctionPrice = double.Parse(tempAHD.StartPrices);
                                tempHC.CurrentPrices = double.Parse(tempAHD.StartPrices);
                            }

                        }
                        //更改拍品状态
                        if (ahdm.AuctionStatus != tempAHD.AuctionStatus)
                        {
                            //更改数据源状态
                            ahdm.DataStatus = 1;
                            ahdm.AuctionStatus = tempAHD.AuctionStatus;

                            //更改缓存状态
                            AuctionHallDataModel tempHC = TotalList.Find(p => p.PublishId == ahdm.PublishId);
                            if (tempHC != null)
                            {
                                tempHC.AuctionStatus = tempAHD.AuctionStatus;
                            }
                        }
                        ////判断机器人状态是否改变
                        //bool falg = amfb.isSetRobotFromCache(TvaID, ahdm.PublishId);
                        //if (ahdm.IsSetRebot != falg)
                        //{
                        //    //更改数据源状态
                        //    ahdm.DataStatus = 1;
                        //    ahdm.IsSetRebot = falg;
                        //}

                        //提前选择成交
                        if (Convert.ToInt32(ahdm.AuctionStatus) > 2)
                        {
                            //更改数据源状态
                            ahdm.DataStatus = 1;

                            pc.ahdm = ahdm;
                            pc.SetStatus(new TstingState());
                            pc.StartStatus();
                        }
                        //竞价时间结束
                        if (DateTime.Compare(Convert.ToDateTime(ahdm.StopTime).AddSeconds(3), DateTime.Now) < 0)
                        {
                            //更改数据源状态
                            ahdm.DataStatus = 1;

                            pc.ahdm = ahdm;
                            pc.SetStatus(new AuctionState());
                            pc.StartStatus();

                        }
                        //价格变化更改状态
                        //AuctionPriceHistoryModel currModel = amfb.GetHighestBidRecord(ahdm.CarSourceId, ahdm.PublishId);
                        if (ahdm.CurrentPrices < tempAHD.CurrentPrices)
                        {
                            //更改数据源状态
                            if (ahdm.DataStatus == 0)
                            {
                                ahdm.DataStatus = 1;
                            }
                            //更改当前价
                            ahdm.CurrentPrices = tempAHD.CurrentPrices;
                            //更改结束时间
                            ahdm.StopTime = tempAHD.StopTime;
                            pc.ahdm = ahdm;
                            pc.SetStatus(new AuctionState());
                            pc.StartStatus();

                            //更改缓存价格和时间
                            AuctionHallDataModel tempHC=TotalList.Find(p => p.PublishId == ahdm.PublishId);
                            if (tempHC != null)
                            {
                                tempHC.StopTime = tempAHD.StopTime;
                                tempHC.CurrentPrices = tempAHD.CurrentPrices;
                            }
                        }
                        //更新竞价结束状态
                        if (tempAHD.BidStatus == BidStatusType.竞价结束)
                        {
                            //更改数据源状态
                            ahdm.DataStatus = 1;
                            ahdm.BidStatus = BidStatusType.竞价结束;
                            pc.ahdm = ahdm;
                            pc.SetStatus(new AuctionState());
                            pc.StartStatus();
                        }

                    });
                }
                //判断状态是3的拍品
                List<AuctionHallDataModel> Sta3List = sessionDataList.Where(p => p.DataStatus != 4 && p.BidStatus == BidStatusType.竞价结束).ToList();
                if (Sta3List != null)
                {
                    Parallel.ForEach(Sta3List, ahdm =>
                    {
                        //更改拍品状态
                        //AuctionHallDataModel tempAHD = biz.GetPublishTime(ahdm.PublishId, TvaID);
                        AuctionHallDataModel tempAHD = compareModel.Find(p => p.PublishId == ahdm.PublishId);
                        if (ahdm.AuctionStatus != tempAHD.AuctionStatus)
                        {
                            //更改数据源状态
                            ahdm.DataStatus = 1;
                            ahdm.AuctionStatus = tempAHD.AuctionStatus;

                            //更改缓存状态,如果使用全局缓存，一定要重新插入缓存
                            AuctionHallDataModel tempHC = TotalList.Find(p => p.PublishId == ahdm.PublishId);
                            if (tempHC != null)
                            {
                                tempHC.AuctionStatus = tempAHD.AuctionStatus;
                            }
                        }

                        pc.ahdm = ahdm;
                        pc.SetStatus(new PublishStoppedState());
                        pc.StartStatus();
                    });

                }
                if (!string.IsNullOrEmpty(carName))
                {
                    pidnewlist = sessionDataList.Where(p => p.CarType.IndexOf(carName) > 0 && p.DataStatus != 4).ToList();
                }
                else
                {
                    pidnewlist = sessionDataList.Where(p =>
                                                            (p.DataStatus == 1 || p.DataStatus == 3) && p.DataStatus != 4
                                                      ).ToList();
                }

                //将新加的车放入会话变量pidlist
                //foreach (var item in pidnewlist)
                //{
                //    var obj = sessionDataList.Find(p => p.PublishId == item.PublishId);
                //    if (obj != null)
                //        sessionDataList.Remove(obj);
                //    sessionDataList.Add(item);
                //}
            }//end if

            #region 处理黑名单
            List<int[]> exp = GetAllTvas(TvaID);//取出所有的黑名单，[0]=买家，[1]=卖家

            pidnewlist.ForEach(delegate(AuctionHallDataModel model)
            {
                model.ServerTime = DateTime.Now;
                model.IsBlackTraID = false;
                foreach (var item in exp)
                {
                    if (item[1] == model.SellerTvaId)
                    {
                        model.IsBlackTraID = true;
                        break;
                    }
                }

            });
            #endregion

            #region 处理是否可参拍
            if (biz.CanAuctionConfig(TvaID))
            {
                pidnewlist.ForEach(p =>
                {
                    p.IsAuction = true;
                });
            }
            else
            {
                List<MyFriendModel> friendList = GetFriendList(TvaID);
                if (friendList == null || friendList.Count == 0)
                {
                    pidnewlist.ForEach(p =>
                    {
                        p.IsAuction = false;
                    });
                }
                else
                {
                    pidnewlist.ForEach(p =>
                    {
                        //当前卖家经销商ID等于伙伴经销商ID
                        if (friendList.Exists(i => i.SellerAId == p.SellerTvaId))
                            p.IsAuction = true;
                        else
                            p.IsAuction = false;
                    });
                }
            }
            #endregion

            //2012.4.6 晚 暂不切换
            //到了整点，首先让客户端删除竞价结束的记录
            //if (DateTime.Now.Hour == 11 || DateTime.Now.Hour == 14 || DateTime.Now.Hour == 16)
            //{
            //    foreach (var item in sessionDataList)
            //    {
            //        if (item.DataStatus!=4
            //            && (item.Type == DataSourceTime.End11 || item.Type == DataSourceTime.End14 || item.Type == DataSourceTime.End16) 
            //            && this.IsPriceTimeOver(item))
            //        {
            //            item.DataStatus = 2;
            //            //if (!pidnewlist.Contains(item))
            //            //    pidnewlist.Add(item);
            //        }
            //    }
            //}

            //如果今日16点的车，所有车均出价结束，表示前台只需要明日相应时段的数据了。
            //2012.4.5 最新需求变更：统一到17点才切换
            //if (DateTime.Now.Hour >= 17)
            //{
            //    //是否切换过数据 
            //    bool changedTime = base.CurrentContext.Session.Get<bool>("_ChangedTime");
            //    if (!changedTime)
            //    {
            //        #region 原有代码，注释
            //        //判断是否需要切换
            //        //int count = 0;
            //        //foreach (var item in sessionDataList)
            //        //{
            //        //    if (item.Type == DataSourceTime.End16) //16点的
            //        //    {
            //        //        if (!this.IsPriceTimeOver(item))
            //        //        {
            //        //            count++;
            //        //        }
            //        //    }
            //        //}
            //        //if (count == 0)
            //        //{
            //        //    //此时没有16点还在拍的车了，切换到明日的车
            //        //    //如果客户端一直连线，需要删除已经头一天的车
            //        //    int delCount=sessionDataList.RemoveAll(p => DateTime.Now.Subtract( p.PriceEndTime).Hours>24);

            //        //    //需要将今日11点，14点，16点整点的车的数据删除
            //        //    sessionDataList.ForEach(p => {
            //        //        if (p.Type != DataSourceTime.EndAll)
            //        //            p.DataStatus = 2;
            //        //    });

            //        //    foreach (var entity in sessionDataList)
            //        //    {
            //        //        if (entity.DataStatus == 2 || entity.DataStatus == 4)
            //        //            continue;

            //        //        if (entity.PriceEndTime.Minute == 0 && entity.PriceEndTime.Second == 0 && entity.PriceEndTime.Day != DateTime.Now.Day)
            //        //        {
            //        //            switch (entity.PriceEndTime.Hour)
            //        //            {
            //        //                case 11: entity.Type = DataSourceTime.End11; break;
            //        //                case 14: entity.Type = DataSourceTime.End14; break;
            //        //                case 16: entity.Type = DataSourceTime.End16; break;
            //        //                default: entity.Type = DataSourceTime.EndAll; break;
            //        //            }
            //        //            entity.DataStatus = 1;
            //        //        }
            //        //        else
            //        //        {
            //        //            if (this.IsPriceTimeOver(entity))
            //        //                entity.DataStatus = 2;
            //        //            else
            //        //                entity.Type = DataSourceTime.EndAll;
            //        //        }

            //        //    }

            //        //    //首次切换，需要推送所有的其它数据到前台
            //        //    pidnewlist = sessionDataList;
            //        //    //完成第一次切换
            //        //    base.CurrentContext.Session.Set<bool>("_ChangedTime", true);
            //        //}
            //        #endregion

            //    }
            //    else
            //    {
            //        //已经切换过，仅需处理新数据
            //        foreach (var entity in pidnewlist)
            //        {
            //            if (entity.DataStatus == 2 || entity.DataStatus == 4)
            //                continue;

            //            if (entity.PriceEndTime.Minute == 0 && entity.PriceEndTime.Second == 0 && entity.PriceEndTime.Day != DateTime.Now.Day)
            //            {
            //                switch (entity.PriceEndTime.Hour)
            //                {
            //                    case 11: entity.Type = DataSourceTime.End11; break;
            //                    case 14: entity.Type = DataSourceTime.End14; break;
            //                    case 16: entity.Type = DataSourceTime.End16; break;
            //                    default: entity.Type = DataSourceTime.EndAll; break;
            //                }
            //            }
            //            else
            //            {
            //                if (this.IsPriceTimeOver(entity))
            //                    entity.DataStatus = 2;
            //                else
            //                    entity.Type = DataSourceTime.EndAll;
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    //今日16点以前的时候，明日结束和非整点的拍品，都算作其它时段的拍品
            SetTodayDateTimeType(pidnewlist);
            //}

            return pidnewlist;
        }

        /// <summary>
        /// 出价是否已经结束
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private bool IsPriceTimeOver(AuctionHallDataModel model)
        {
            return DateTime.Parse(model.StopTime) < DateTime.Now;
        }

        /// <summary>
        /// 设置今日的数据源时段
        /// </summary>
        /// <param name="pidnewlist"></param>
        private void SetTodayDateTimeType(List<AuctionHallDataModel> pidnewlist)
        {
            foreach (var entity in pidnewlist)
            {
                if (entity.PriceEndTime.Minute == 0 && entity.PriceEndTime.Second == 0 && entity.PriceEndTime.Day == DateTime.Now.Day)
                {
                    switch (entity.PriceEndTime.Hour)
                    {
                        case 11: entity.Type = DataSourceTime.End11; break;
                        case 14: entity.Type = DataSourceTime.End14; break;
                        case 16: entity.Type = DataSourceTime.End16; break;
                        default: entity.Type = DataSourceTime.EndAll; break;
                    }
                }
                else
                {
                    entity.Type = DataSourceTime.EndAll;
                }
            }
        }

        /// <summary>
        /// 匹配车型
        /// </summary>
        /// <param name="carName"></param>
        /// <returns></returns>
        public List<CarSearchResultModel> GetCarList(string carName)
        {
            AuctionHallBiz biz = new AuctionHallBiz();
            return biz.GetCarList(carName);

        }
        /// <summary>
        /// 处理当前的会话状态
        /// </summary>
        /// <param name="TvaID"></param>
        /// <param name="pID"></param>
        private void OptCurrentSessionAtt(int TvaID)
        {
            List<AttentionInfo> attlist = GetAttList().Where(p => p.TvaID == TvaID).ToList();
            if (attlist != null)
            {
                foreach (AttentionInfo ai in attlist)
                {
                    OptAttentionList(ai.AttentionSta, ai.PublishID);
                }
            }
        }
        /// <summary>
        /// 增加关注
        /// </summary>
        /// <param name="TvaID"></param>
        /// <param name="TvuID"></param>
        /// <param name="carsourceID"></param>
        /// <param name="pID"></param>
        /// <param name="sta"></param>
        /// <param name="sr"></param>
        public bool AddOrUptAtt(int TvaID, int TvuID, int carsourceID, long pID, int sta, int sr)
        {
            AuctionHallBiz biz = new AuctionHallBiz();
            //关注日志
            string msg = sta == 1 ? "设置关注" : "取消关注";
            //放到下面，biz.AddOrUptAtt可能不会返回真?
            if (biz.AddOrUptAtt(TvaID, TvuID, carsourceID, pID, sta, sr))
            {
                List<AttentionInfo> ailist = base.GlobalCache.Get<List<AttentionInfo>>("Service_GetAttList");
                if (ailist != null)
                {
                    ailist.RemoveAll(p => p.PublishID == pID);
                }
                OptAttentionList(sta, pID);

                //string dateTime = DateTime.Now.ToString("yyyy-MM-dd");
                //string key = string.Format("hall_{0}", dateTime);
                //List<AuctionHallDataModel> pidlist = base.CurrentContext.Session.Get<List<AuctionHallDataModel>>(key);
                //AuctionHallDataModel ahdm = pidlist.FirstOrDefault(p => p.PublishId == pID);
                //if (ahdm != null)
                //{
                //    ahdm.DataStatus = 5;
                //    ahdm.AttentionState = sta.ToString();
                //}
                biz.SaveAttentionLog(TvuID, msg + " 成功", pID);
                return true;

            }
            else
            {
                biz.SaveAttentionLog(TvuID, msg + " 不成功", pID);
                return false;
            }
        }
        /// <summary>
        /// 根据状态处理会话状态列表
        /// </summary>
        /// <param name="sta"></param>
        /// <param name="pID"></param>
        private void OptAttentionList(int sta, long pID)
        {
            List<long> addlist = base.CurrentContext.Session.Get<List<long>>("add");
            List<long> cacellist = base.CurrentContext.Session.Get<List<long>>("cancel");
            //状态是取消的时候，再取消缓存里加一条，再在增加的缓存里取一条
            if (sta == 0)
            {
                if (cacellist == null)
                {
                    cacellist = new List<long>();
                    cacellist.Add(pID);
                    base.CurrentContext.Session.Set<List<long>>("cancel", cacellist);
                }
                else
                {
                    //防止重复增加，先删除再增加
                    cacellist.RemoveAll(p => p == pID);
                    cacellist.Add(pID);
                }
                if (addlist != null)
                {
                    addlist.RemoveAll(p => p == pID);
                }
            }
            //状态是增加的时候，再增加缓存里加一条，再在取消的缓存里取一条
            if (sta == 1)
            {
                if (addlist == null)
                {
                    addlist = new List<long>();
                    addlist.Add(pID);
                    base.CurrentContext.Session.Set<List<long>>("add", addlist);
                }
                else
                {
                    //防止重复增加，先删除再增加
                    addlist.RemoveAll(p => p == pID);
                    addlist.Add(pID);
                }
                if (cacellist != null)
                {
                    cacellist.RemoveAll(p => p == pID);
                }
            }
        }
        /// <summary>
        /// 从缓存中获取所有的拍品列表
        /// </summary>
        /// <returns></returns>
        private List<PublishInfo> GetAllPublishIdList()
        {
            int count = base.CurrentContext.Session.Get<int>("PublishCount");

            List<PublishInfo> publishList = base.CurrentContext.Cache.Get<List<PublishInfo>>("Service_GetAllPublishIdList",
               () =>
               {
                   AuctionHallBiz biz = new AuctionHallBiz();
                   return biz.GetAllPublishIdList();
               },
               new System.Runtime.Caching.CacheItemPolicy()
               {
                   AbsoluteExpiration = DateTime.Now.AddMinutes(1)
               }
               );
            if (count != publishList.Count)
            {
                count = publishList.Count;
                base.CurrentContext.Session.Set<int>("PublishCount", count);
                return publishList;
            }
            return null;
        }

        private static object sync_PublishListCount = new object();

        /// <summary>
        /// 从缓存中获取所有的拍品列表(监测发现该方法经常出现数据库服务器无法打开连接，故修改原有实现，使用锁变量)
        /// </summary>
        /// <returns></returns>
        private List<AuctionHallDataModel> GetAllPublishDataListByMinute()
        {
            //依赖于 PublishListCount 来比较有没有新纪录，可能会出问题，比如实际上剩余符合要求的拍品数量（期间有更改过的）恰巧跟此数量一致
            //int count = base.CurrentContext.Session.Get<int>("PublishListCount");
            List<AuctionHallDataModel> publishList = base.CurrentContext.Cache.Get<List<AuctionHallDataModel>>("Service_GetAllPublishIdListByMinute",
               () =>
               {
                   lock (sync_PublishListCount)
                   {
                       List<AuctionHallDataModel> result = base.CurrentContext.Cache.Get<List<AuctionHallDataModel>>("Service_GetAllPublishIdListByMinute");
                       if (result == null)
                       {
                           AuctionHallBiz biz = new AuctionHallBiz();
                           result= biz.GetNewAuctionHallDataList().FindAll(p=>p.BidStatus<BidStatusType.竞价结束);
                           //此时 result 可能为空集合
                       }
                       if (result.Count() > 0)
                       {
                           long[] arrIDs = result.Select(p => p.PublishId).ToArray();
                           long[] oldArrIDs = base.CurrentContext.Cache.Get<long[]>("PublishIDArrayMinute", () => { return new long[] { }; });
                           var exceptIDs = arrIDs.Except(oldArrIDs).ToArray();//本次新增加的拍品ID
                           if (exceptIDs.Length > 0)
                           {
                               //新查询出来的跟会话中的有差异
                               base.CurrentContext.Cache.Remove("PublishIDArrayMinute");
                               base.CurrentContext.Cache.Insert<long[]>("PublishIDArrayMinute", arrIDs);
                               var newList = from item in result
                                             where exceptIDs.Contains(item.PublishId)
                                             select item;

                               return newList.ToList();
                           }
                       }
                       return new List<AuctionHallDataModel>();//没有新拍品
                   }
                   
               },
               new System.Runtime.Caching.CacheItemPolicy()
               {
                   AbsoluteExpiration = DateTime.Now.AddMinutes(1)
               }
               );

            return publishList;
            //if (count != publishList.Count)
            //{
            //    count = publishList.Count;
            //    base.CurrentContext.Session.Set<int>("PublishListCount", count);
            //    return publishList;
            //}
            //return null;
        }
        /// <summary>
        ///  从缓存中获取所有的拍品列表(xin)
        /// </summary>
        /// <param name="tvaid"></param>
        /// <returns></returns>
        private List<PublishInfo> GetAllPublishIdList2(int tvaid)
        {
            return base.CurrentContext.Cache.Get<List<PublishInfo>>("Service_GetAllPublishIdList",
                () =>
                {
                    AuctionHallBiz biz = new AuctionHallBiz();
                    List<long> list = biz.GetVendorPartnerPublishID(tvaid);
                    List<PublishInfo> result = new List<PublishInfo>();
                    foreach (long item in list)
                        result.Add(new PublishInfo() { PublishId = item });
                    return result;
                },
                new System.Runtime.Caching.CacheItemPolicy()
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(1)
                }
                );
        }
        /// <summary>
        /// 从缓存中获取所有的关注列表
        /// </summary>
        /// <returns></returns>
        private List<AttentionInfo> GetAttList()
        {
            return base.GlobalCache.Get<List<AttentionInfo>>("Service_GetAttList",
                () =>
                {
                    AuctionHallBiz biz = new AuctionHallBiz();
                    return biz.GetAttList();
                },
                new System.Runtime.Caching.CacheItemPolicy()
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(1)
                }
                );
        }
        #region 缓存黑名单列表
        private List<int[]> GetAllTvas(int tvaid)
        {
            List<int[]> dic = GetAllBlackMan();
            List<int[]> result = dic.Where(p => p[0] == tvaid).ToList();
            return result;
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
                    AbsoluteExpiration = DateTime.Now.AddMinutes(10) //正式环境，改成1小时1次。
                }
                );
        }
        #endregion

        /// <summary>
        /// 获取缓存的伙伴列表
        /// </summary>
        /// <param name="tvaid"></param>
        /// <returns></returns>
        private List<MyFriendModel> GetFriendList(int tvaid)
        {
            return base.GlobalCache.Get<List<MyFriendModel>>(
                "Friend_List",
                () =>
                {
                    AuctionMainFormBiz biz = new AuctionMainFormBiz();
                    return biz.GetFriendList(tvaid);
                },
                new System.Runtime.Caching.CacheItemPolicy()
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(10)
                }
            );


        }
        /// <summary>
        /// 获取所有保证金大于0的用户列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetUserList()
        {
            AuctionHallBiz biz = new AuctionHallBiz();
            return biz.GetUserList();
        }

        public override bool ProcessRequest(IServiceContext context)
        {
            context.SessionRequired = true;
            return base.ProcessRequest(context); //请保留此行，否则在具体的方法里面可能无法获取 CurrentContext 属性
        }
    }
}
