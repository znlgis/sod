using System;
using System.Collections.Generic;
using System.Text;

namespace TranstarAuction.Model.AuctionMain
{
    /// <summary>
    /// 要延迟显示数据改变状态的实体对象（剩余60秒）
    /// </summary>
    public class DataChangeEntitys
    {
        public ShowDelayEntity NewEntity { get; private set; }
        public ShowDelayEntity OldEntity { get; private set; }
       

        public DataChangeEntitys(ShowDelayEntity newEntity, ShowDelayEntity OldEntity)
        {
            this.NewEntity = newEntity;
            this.OldEntity = OldEntity;
        }
        
    }
}
