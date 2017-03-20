using System;
using System.Collections.Generic;
 
using TranstarAuction.Model;


namespace TranstarAuction.Presenters
{
    /// <summary>
    /// 全局数据 主持人
    /// </summary>
    public interface IGlobalPresenter : IPresenter
    {
        /// <summary>
        /// （从服务器）订阅拍卖大厅的适时数据。订阅的数据放到GlobalDataSource 属性中，并且需要发起DataSourceChange 事件
        /// </summary>
        /// <param name="trvid"></param>
        void SubscribeAutionHallData(int trvid);
        /// <summary>
        /// 全局数据源
        /// </summary>
        List<IAuctionHallDataModel> GlobalDataSource { get; set; }
        /// <summary>
        /// 数据改变事件
        /// </summary>
        event EventHandler DataSourceChange;
        /// <summary>
        /// 发送消息给全局主持人，由主持人决定如何进一步处理消息
        /// </summary>
        /// <param name="sender">发送人</param>
        /// <param name="message">消息内容</param>
        /// <param name="paraData">消息附带的数据</param>
        void SendMessage(object sender, string message, object paraData);
    }
}
