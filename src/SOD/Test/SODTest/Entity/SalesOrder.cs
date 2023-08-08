using System;
using PWMIS.Common;
using PWMIS.DataMap.Entity;

namespace SODTest
{
    /// <summary>
    ///     订单
    /// </summary>
    [Serializable]
    public class SalesOrder : EntityBase
    {
        public SalesOrder()
        {
            TableName = "tb_SalesOrder";
            EntityMap = EntityMapType.Table;
            PrimaryKeys.Add("iBillID");
        }

        /// <summary>
        ///     经销商名称
        /// </summary>
        public string CustomerName { get; set; }

        protected override void SetFieldNames()
        {
            PropertyNames = new[]
            {
                "iBillID", "vcBillNo", "dtBillDate", "vcMemo", "iCustomerID", "CreationDate", "CreatedBy",
                "LastUpdateDate", "LastUpdatedBy", "FlagApp", "AppDate", "AppUser", "iOrderTypeID", "iState",
                "vcDesign", "vcDesignPhone", "numDayRequest", "numDay", "dtDelivery", "IsPicture", "vcBuy",
                "vcBuyPhone", "vcBuyAddress", "vcImgFile"
            };
        }

        #region Model

        /// <summary>
        ///     主键
        /// </summary>
        public int iBillID
        {
            get => getProperty<int>("iBillID");
            set => setProperty("iBillID", value);
        }

        /// <summary>
        ///     单据号码
        /// </summary>
        public string vcBillNo
        {
            get => getProperty<string>("vcBillNo");
            set => setProperty("vcBillNo", value);
        }

        /// <summary>
        ///     单据日期
        /// </summary>
        public DateTime dtBillDate
        {
            get => getProperty<DateTime>("dtBillDate");
            set => setProperty("dtBillDate", value);
        }

        /// <summary>
        ///     备注
        /// </summary>
        public string vcMemo
        {
            get => getProperty<string>("vcMemo");
            set => setProperty("vcMemo", value);
        }

        /// <summary>
        ///     客户
        /// </summary>
        public int iCustomerID
        {
            get => getProperty<int>("iCustomerID");
            set => setProperty("iCustomerID", value);
        }

        /// <summary>
        ///     创建日期
        /// </summary>
        public DateTime CreationDate
        {
            get => getProperty<DateTime>("CreationDate");
            set => setProperty("CreationDate", value);
        }

        /// <summary>
        ///     创建人
        /// </summary>
        public string CreatedBy
        {
            get => getProperty<string>("CreatedBy");
            set => setProperty("CreatedBy", value);
        }

        /// <summary>
        ///     更新日期
        /// </summary>
        public DateTime LastUpdateDate
        {
            get => getProperty<DateTime>("LastUpdateDate");
            set => setProperty("LastUpdateDate", value);
        }

        /// <summary>
        ///     更新人
        /// </summary>
        public string LastUpdatedBy
        {
            get => getProperty<string>("LastUpdatedBy");
            set => setProperty("LastUpdatedBy", value);
        }

        /// <summary>
        ///     确定标记
        /// </summary>
        public string FlagApp
        {
            get => getProperty<string>("FlagApp");
            set => setProperty("FlagApp", value);
        }

        /// <summary>
        ///     确定日期
        /// </summary>
        public DateTime AppDate
        {
            get => getProperty<DateTime>("AppDate");
            set => setProperty("AppDate", value);
        }

        /// <summary>
        ///     确定人
        /// </summary>
        public string AppUser
        {
            get => getProperty<string>("AppUser");
            set => setProperty("AppUser", value);
        }

        /// <summary>
        ///     订单类型
        /// </summary>
        public string iOrderTypeID
        {
            get => getProperty<string>("iOrderTypeID");
            set => setProperty("iOrderTypeID", value);
        }

        /// <summary>
        ///     单据状态
        /// </summary>
        public int iState
        {
            get => getProperty<int>("iState");
            set => setProperty("iState", value);
        }

        /// <summary>
        ///     设计师
        /// </summary>
        public string vcDesign
        {
            get => getProperty<string>("vcDesign");
            set => setProperty("vcDesign", value);
        }

        /// <summary>
        ///     设计师电话
        /// </summary>
        public string vcDesignPhone
        {
            get => getProperty<string>("vcDesignPhone");
            set => setProperty("vcDesignPhone", value);
        }

        /// <summary>
        ///     参考交付周期
        /// </summary>
        public int numDayRequest
        {
            get => getProperty<int>("numDayRequest");
            set => setProperty("numDayRequest", value);
        }

        /// <summary>
        ///     交付周期
        /// </summary>
        public int numDay
        {
            get => getProperty<int>("numDay");
            set => setProperty("numDay", value);
        }

        /// <summary>
        ///     参考交付日期
        /// </summary>
        public DateTime dtDelivery
        {
            get => getProperty<DateTime>("dtDelivery");
            set => setProperty("dtDelivery", value);
        }

        /// <summary>
        ///     有无图纸
        /// </summary>
        public int IsPicture
        {
            get => getProperty<int>("IsPicture");
            set => setProperty("IsPicture", value);
        }

        /// <summary>
        ///     客户信息
        /// </summary>
        public string vcBuy
        {
            get => getProperty<string>("vcBuy");
            set => setProperty("vcBuy", value);
        }

        /// <summary>
        ///     客户电话
        /// </summary>
        public string vcBuyPhone
        {
            get => getProperty<string>("vcBuyPhone");
            set => setProperty("vcBuyPhone", value);
        }

        /// <summary>
        ///     客户地址
        /// </summary>
        public string vcBuyAddress
        {
            get => getProperty<string>("vcBuyAddress");
            set => setProperty("vcBuyAddress", value);
        }

        /// <summary>
        ///     图纸地址
        /// </summary>
        public string vcImgFile
        {
            get => getProperty<string>("vcImgFile");
            set => setProperty("vcImgFile", value);
        }

        #endregion Model
    }
}