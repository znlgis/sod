using System;
using System.Collections.Generic;
using System.Text;

namespace TranstarAuction.Model
{
    public enum BidStatusType : int
    {
        确认条件 = 0,
        您是最高价 = 1,
        出价不是最高 = 2,
        竞价结束 = 3,//竞价结束请等待
        交易中 = 4,//竞价成功
        竞价失败 = 5,//竞价失败
        流拍 = 6,//竞价失败
        未竞得=7
    }

    /// <summary>
    /// 可见性损伤
    /// </summary>
    public enum BrokenType : int
    {
        Bone = 1,
        Out,
        In
    }
    /// <summary>
    /// 订单显示状态
    /// </summary>
    public enum TstShow
    {
        待收车 = 1,
        待付款 = 2,
        争议上诉中 = 3,
        不显示 = 4,
        交易成功 = 5,
        交易失败 = 6
    }
    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderState
    {
        /// <summary>
        /// 1-待付款
        /// </summary>
        待付款 = 1,
        /// <summary>
        ///  4-待收车
        /// </summary>
        待收车 = 4,
        /// <summary>
        /// 8--已收车
        /// </summary>
        已收车 = 8,
        
    }
    /// <summary>
    /// 仲裁状态
    /// </summary>
    public enum ArbState
    {
        /// <summary>
        /// 初始化无意义
        /// </summary>
        None = 0,
        /// <summary>
        /// 未处理,仲裁中
        /// </summary>
        Nothandel = 1,
        /// <summary>
        /// 已处理
        /// </summary>
        Done = 2,
        /// <summary>
        /// 撤销
        /// </summary>
        Repal = 3
    }
    /// <summary>
    /// 订单结果
    /// </summary>
    public enum TstResult
    {
        交易中 = 1,
        交易成功 = 2,
        交易失败 = 3,
        线下处理 = 4
    }
    /// <summary>
    /// 付款方式
    /// </summary>
    public enum PaymentType
    {
        None = 0,
        在线付款 = 1,
        线下付款 = 2,
        代理付款 = 3
    }
    /// <summary>
    /// 提车方式
    /// </summary>
    public enum ReceiveType
    {
        None = 0,
        代理外迁 = 1,
        上门自提 = 2,

    }

    public enum CtiyList
    {
        请选择=0,
        成都=1,               
        沈阳=4,      
        潍坊=6,
        枣庄=7,
        郑州=8,
        上海=9
    }

    public enum SysRoutePrice
    {
        c2800=1,
        s990=4,
        w1050=6,
        z900=7,
        z990=8,
        s1800=9
    }

    /// <summary>
    /// （拍品）数据源时段
    /// </summary>
    public enum DataSourceTime
    {
        /// <summary>
        /// 11点结束
        /// </summary>
        End11 = 11,
        /// <summary>
        /// 14点（原来的其它整点）结束
        /// </summary>
        End14 = 14,
        /// <summary>
        /// 16点结束
        /// </summary>
        End16 = 16,
        
        /// <summary>
        /// 所有拍品
        /// </summary>
        EndAll = 24
    }
}
