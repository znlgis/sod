using System;
using System.Collections.Generic;
using System.Text;

namespace TranstarAuction.Model.AuctionMain
{
    public class AuctionCarModel
    {
        /// <summary>
        /// 车编号
        /// </summary>
        public int CarId
        {
            get;
            set;
        }

        /// <summary>
        /// 发布编号
        /// </summary>
        public int PubId
        {
            get;
            set;
        }

        /// <summary>
        /// 车名称
        /// </summary>
        public string CarName
        {
            get;
            set;
        }

        /// <summary>
        /// 车品牌
        /// </summary>
        public string CarBrand
        {
            get;
            set;
        }

        /// <summary>
        /// 车年限
        /// </summary>
        public string CarYear
        {
            get;
            set;
        }

        /// <summary>
        /// 车排量
        /// </summary>
        public string CarDisplacement
        {
            get;
            set;
        }

        /// <summary>
        /// 车版本如豪华型
        /// </summary>
        public string CarVersion
        {
            get;
            set;
        }
        /// <summary>
        /// 是否一手车
        /// </summary>
        public string liIsFirstHand
        {
            get;
            set;
        }
        /// <summary>
        /// 原购车价
        /// </summary>
        public string liCashNum
        {
            get;
            set;
        }
        /// <summary>
        /// 发动机号是否变更
        /// </summary>
        public string ltrIsChangeEngineNum
        {
            get;
            set;
        }
        /// <summary>
        /// 发动机号
        /// </summary>
        public string ltrIsChangeEngineNumNew
        {
            get;
            set;
        }
        /// <summary>
        /// 车架号拓号
        /// </summary>
        public string ltrIsFrameNum
        {
            get;
            set;
        }
        /// <summary>
        /// 登记日期
        /// </summary>
        public string RegisterDate
        {
            get;
            set;
        }

        /// <summary>
        /// 变速箱类型
        /// </summary>
        public string GearBoxType
        {
            get;
            set;
        }

        /// <summary>
        /// 车类型
        /// </summary>
        public string CarType
        {
            get;
            set;
        }

        /// <summary>
        /// 行驶公里数
        /// </summary>
        public int CarKilometre
        {
            get;
            set;
        }

        /// <summary>
        /// 年审有效期
        /// </summary>
        public string AnnualSurverValidDate
        {
            get;
            set;
        }

        /// <summary>
        /// 购置税
        /// </summary>
        public string PurchaseTax
        {
            get;
            set;
        }

        /// <summary>
        /// 强制险到期
        /// </summary>
        public string ForceInsureValidDate
        {
            get;
            set;
        }

        /// <summary>
        /// 商业险到期
        /// </summary>
        public string BussinessInsureValidDate
        {
            get;
            set;
        }

        /// <summary>
        /// 车辆颜色
        /// </summary>
        public string CarColor
        {
            get;
            set;
        }

        /// <summary>
        /// 环保标准
        /// </summary>
        public string EnvironmentalStandard
        {
            get;
            set;
        }


        /// <summary>
        /// 车牌所在地
        /// </summary>
        public string PlateNumberInCity
        {
            get;
            set;
        }

        /// <summary>
        /// 出厂日期
        /// </summary>
        public string LeaveFactoryDate
        {
            get;
            set;
        }

        /// <summary>
        /// 保养记录
        /// </summary>
        public string RepairRecord
        {
            get;
            set;
        }

        /// <summary>
        /// 车船税到期
        /// </summary>
        public string CarBoatTaxValidDate
        {
            get;
            set;
        }

        /// <summary>
        /// VinCode
        /// </summary>
        public string VinCode
        {
            get;
            set;
        }

        /// <summary>
        /// 补充信息
        /// </summary>
        public string ReplenishInfo
        {
            get;
            set;
        }
        /// <summary>
        /// 配置信息
        /// </summary>
        public string ltrCarConfig
        {
            get;
            set;
        }

        /// <summary>
        /// 车牌
        /// </summary>
        public string PlateNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 起动机/转向助力状态
        /// </summary>
        public string Starter
        {
            get;
            set;
        }
        /// <summary>
        /// 起动机/转向助描述
        /// </summary>
        public string StarterDesc
        {
            get;
            set;
        }
        /// <summary>
        ///发动机状态
        /// </summary>
        public string NewEngine
        {
            get;
            set;
        }
        /// <summary>
        /// 发动机描述
        /// </summary>
        public string NewEngineDesc
        {
            get;
            set;
        }
        /// <summary>
        /// 变速器状态
        /// </summary>
        public string NewTransmission
        {
            get;
            set;
        }
        /// <summary>
        /// 变速器描述
        /// </summary>
        public string NewTransmissionDesc
        {
            get;
            set;
        }
        /// <summary>
        /// 避震器状态
        /// </summary>
        public string ShockAbsorber
        {
            get;
            set;
        }
        /// <summary>
        /// 避震器描述
        /// </summary>
        public string ShockAbsorberDesc
        {
            get;
            set;
        }
        /// <summary>
        /// 底盘 / 行驶状态
        /// </summary>
        public string Chassis
        {
            get;
            set;
        }
        /// <summary>
        ///底盘 / 行驶描述
        /// </summary>
        public string ChassisDesc
        {
            get;
            set;
        }
        /// <summary>
        /// 制动器状态
        /// </summary>
        public string Brake
        {
            get;
            set;
        }
        /// <summary>
        /// 制动器描述
        /// </summary>
        public string BrakeDesc
        {
            get;
            set;
        }
        /// <summary>
        /// 排气系统状态
        /// </summary>
        public string ExhaustSystem
        {
            get;
            set;
        }
        /// <summary>
        /// 排气系统描述
        /// </summary>
        public string ExhaustSystemDesc
        {
            get;
            set;
        }
        /// <summary>
        /// 电器系统状态
        /// </summary>
        public string ElectricalSystem
        {
            get;
            set;
        }
        /// <summary>
        /// 电器系统描述
        /// </summary>
        public string ElectricalSystemDesc
        {
            get;
            set;
        }
        /// <summary>
        /// 补充说明描述
        /// </summary>
        public string SupplementInfo
        {
            get;
            set;
        }
        /// <summary>
        ///旧的 发动机/变速箱状态
        /// </summary>
        public int PaintDesc
        {
            get;
            set;
        }
        /// <summary>
        ///旧的 发动机/变速箱描述
        /// </summary>
        public string PaintRemark
        {
            get;
            set;
        }
        /// <summary>
        /// 舒适性配置状态
        /// </summary>
        public int SurfaceDesc
        {
            get;
            set;
        }
        /// <summary>
        /// 舒适性配置描述
        /// </summary>
        public string SurfaceRemark
        {
            get;
            set;
        }
        /// <summary>
        /// 安全性配置状态
        /// </summary>
        public int SeriousAccidentDesc
        {
            get;
            set;
        }
        /// <summary>
        /// 安全性配置描述
        /// </summary>
        public string SeriousAccidentRemark
        {
            get;
            set;
        }
        /// <summary>
        /// 行驶系统状态
        /// </summary>
        public int EngineDesc
        {
            get;
            set;
        }
        /// <summary>
        /// 行驶系统描述
        /// </summary>
        public string EngineRemark
        {
            get;
            set;
        }
        /// <summary>
        /// 随车附件配置状态
        /// </summary>
        public int TransmissionDesc
        {
            get;
            set;
        }
        /// <summary>
        /// 随车附件配置描述
        /// </summary>
        public string TransmissionRemark
        {
            get;
            set;
        }
        /// <summary>
        /// 其他异常描述
        /// </summary>
        public string OtherFileRemark
        {
            get;
            set;
        }
       
        /// <summary>
        /// 上次过户日期(非一手车)
        /// </summary>
        public string LastTimeGH
        {
            get;
            set;
        }
        /// <summary>
        /// 商业险(有无)
        /// </summary>
        public bool BusinessInsure
        {
            get;
            set;
        }
        /// <summary>
        /// 商业险金额
        /// </summary>
        public string BusinessInsurePrice
        {
            get;
            set;
        }
    }
}
