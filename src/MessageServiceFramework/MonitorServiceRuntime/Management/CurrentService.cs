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
namespace TranstarAuction.Service
{
    
    public class CurrentService:ServiceBase
    {
        /// <summary>
        /// 获取当前价
        /// </summary>
        /// <param name="carSourceID"></param>
        /// <param name="pId"></param>
        /// <returns></returns>
        public AuctionPriceHistoryModel GetBidRecord(int carSourceID, long pId)
        {
            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            string key = string.Format("price_{0}", pId);
            AuctionPriceHistoryModel model = base.CurrentContext.Cache.Get<AuctionPriceHistoryModel>(key);
            if (model == null)
            {
                model = new AuctionPriceHistoryModel();
                model = biz.GetBidRecord(carSourceID, pId);
                base.CurrentContext.Cache.Insert<AuctionPriceHistoryModel>(key, model);
            }
            else
            {
                AuctionPriceHistoryModel aphm = biz.GetBidRecord(carSourceID, pId);
                if (model.AuctionPrice < aphm.AuctionPrice)
                {
                    model = aphm;
                    base.CurrentContext.Cache.Remove(key);
                    base.CurrentContext.Cache.Insert<AuctionPriceHistoryModel>(key, aphm);
                }
                else
                {
                    return null;
                }
            }
            return model;
        }
        /// <summary>
        /// 获取当前价
        /// </summary>
        /// <param name="carSourceID"></param>
        /// <param name="pId"></param>
        /// <returns></returns>
        public List<AuctionPriceHistoryModel> GetBidRecordList(long pId)
        {
            AuctionMainFormBiz biz = new AuctionMainFormBiz();
            //先从本地缓存获取出价列表
            string key = string.Format("price_{0}", pId+"list");
            List<AuctionPriceHistoryModel> modellist = base.CurrentContext.Cache.Get<List<AuctionPriceHistoryModel>>(key);

            if (modellist == null || modellist.Count==0)
            {
                modellist = biz.GetBidRecordList(0, pId);
                if(modellist.Count>0)
                    base.CurrentContext.Cache.Insert<List<AuctionPriceHistoryModel>>(key, modellist);
                return modellist;//第一次，直接全部返回
            }
            else
            {
                //比较出新的数据
                if (modellist.Count > 0)
                {
                    double maxPrice = modellist.Max(p => p.AuctionPrice);
                    var newlist = biz.GetBidRecordList(0, pId);
                    if (newlist!=null && newlist.Count > 0)
                    {
                        var resultList = newlist.Where(p => p.AuctionPrice > maxPrice).ToList();
                        if (resultList.Count > 0)
                        {
                            resultList.Sort((a, b) => a.AuctionPrice.CompareTo(b.AuctionPrice));
                            // 有新数据，更新缓存 
                            base.CurrentContext.Cache.Remove(key);
                            base.CurrentContext.Cache.Insert<List<AuctionPriceHistoryModel>>(key, resultList);
                        }
                        return resultList;
                    }
                    return null;
                }
            }
            return null;
        }
        public override bool ProcessRequest(IServiceContext context)
        {
            context.SessionRequired = false;
            return base.ProcessRequest(context); //请保留此行，否则在具体的方法里面可能无法获取 CurrentContext 属性
        }
    }
}
