using System;
using System.Collections.Generic;
 
using System.ComponentModel;
using TranstarAuction.Model;
using TranstarAuction.Presenters.Presenter;
using TranstarAuction.Model.AuctionMain;

namespace TranstarAuction.Presenters.ViewInterface
{
    public interface IMyAttentionCarView
    {
        /// <summary>
        /// 获取全局主持人
        /// </summary>
         IGlobalPresenter GlobalPresenter { get;}

         //BindingList<IAttentionCarModel> DataSource11 { get; set; }
         //BindingList<IAttentionCarModel> DataSource16 { get; set; }
         //BindingList<IAttentionCarModel> DataSourceOther { get; set; }

        /// <summary>
        /// 更新网格关联的数据源
        /// </summary>
        /// <param name="Type">数据源的时段</param>
        /// <param name="newData">新数据列表</param>
         void UpdateDataSource(DataSourceTime Type, List<IAttentionCarModel> newData);

         /// <summary>
        /// 实现取消关注接口
        /// </summary>
        /// <param name="result"></param>
         void AddOrUptAtt(bool result);

         /// <summary>
        /// 实现竞价接口
        /// </summary>
        /// <param name="result"></param>
         void AuctionPrice(string result);
        /// <summary>
        /// 在更新数据源的时候，处理数据中有数据改变的情况
        /// </summary>
         /// <param name="model">当前更改的数据</param>
         void DataModifyAction(IAttentionCarModel model);
        /// <summary>
        /// 更新面板上的日期文字
        /// </summary>
        /// <param name="day"></param>
         void ChangeDataDay(string day);

       
    }
}
