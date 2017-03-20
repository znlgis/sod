using System;
using System.Collections.Generic;
using System.Text;

namespace TranstarAuction.Model
{
    public class NotifyMessageModel
    {
        /// <summary>
        /// 消息id
        /// </summary>
        public long MsgID
        {
            get;
            set;
        }
        /// <summary>
        /// 拍品ID
        /// </summary>
        public long PublishId
        {
            get;
            set;
        }
        /// <summary>
        /// 消息类型(1：拍卖消息；2：交易消息；3：伙伴请求)
        /// </summary>
        public int MessageType
        {
            get;
            set;
        }
        /// <summary>
        /// 消息类型
        /// </summary>
        public byte MsgType
        {
            get;
            set;
        }
        /// <summary>
        /// 消息类型说明
        /// </summary>
        public string MsgSubject
        {
            get;
            set;
        }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string MsgContent
        {
            get;
            set;
        }
        /// <summary>
        /// 1：未读2:已读
        /// </summary>
        public int IsActive
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime
        {
            get;
            set;
        }
        /// <summary>
        /// 竞价结束时间
        /// </summary>
        public DateTime AuctionEndTime
        {
            get;
            set;
        }
    }
}
