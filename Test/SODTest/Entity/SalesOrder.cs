using System;
using PWMIS.Common;
using PWMIS.DataMap.Entity;

namespace SODTest
{
    /// <summary>
    /// 订单
    /// </summary>
    [Serializable]
    public partial class SalesOrder : EntityBase
    {
        public SalesOrder()
        {
            TableName = "tb_SalesOrder";
            EntityMap = EntityMapType.Table;
            PrimaryKeys.Add("iBillID");
        }

        protected override void SetFieldNames()
        {
            PropertyNames = new string[] { "iBillID", "vcBillNo", "dtBillDate", "vcMemo", "iCustomerID", "CreationDate", "CreatedBy", "LastUpdateDate", "LastUpdatedBy", "FlagApp", "AppDate", "AppUser", "iOrderTypeID", "iState", "vcDesign", "vcDesignPhone", "numDayRequest", "numDay", "dtDelivery", "IsPicture", "vcBuy", "vcBuyPhone", "vcBuyAddress", "vcImgFile" };
        }

        #region Model
        /// <summary>
        /// 主键
        /// </summary>
        public int iBillID
        {
            get { return getProperty<int>("iBillID"); }
            set { setProperty("iBillID", value); }
        }
        /// <summary>
        /// 单据号码
        /// </summary>
        public String vcBillNo
        {
            get { return getProperty<String>("vcBillNo"); }
            set { setProperty("vcBillNo", value); }
        }
        /// <summary>
        /// 单据日期
        /// </summary>
        public DateTime dtBillDate
        {
            get { return getProperty<DateTime>("dtBillDate"); }
            set { setProperty("dtBillDate", value); }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public String vcMemo
        {
            get { return getProperty<String>("vcMemo"); }
            set { setProperty("vcMemo", value); }
        }
        /// <summary>
        /// 客户
        /// </summary>
        public int iCustomerID
        {
            get { return getProperty<int>("iCustomerID"); }
            set { setProperty("iCustomerID", value); }
        }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreationDate
        {
            get { return getProperty<DateTime>("CreationDate"); }
            set { setProperty("CreationDate", value); }
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public String CreatedBy
        {
            get { return getProperty<String>("CreatedBy"); }
            set { setProperty("CreatedBy", value); }
        }
        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime LastUpdateDate
        {
            get { return getProperty<DateTime>("LastUpdateDate"); }
            set { setProperty("LastUpdateDate", value); }
        }
        /// <summary>
        /// 更新人
        /// </summary>
        public String LastUpdatedBy
        {
            get { return getProperty<String>("LastUpdatedBy"); }
            set { setProperty("LastUpdatedBy", value); }
        }
        /// <summary>
        /// 确定标记
        /// </summary>
        public String FlagApp
        {
            get { return getProperty<String>("FlagApp"); }
            set { setProperty("FlagApp", value); }
        }
        /// <summary>
        /// 确定日期
        /// </summary>
        public DateTime AppDate
        {
            get { return getProperty<DateTime>("AppDate"); }
            set { setProperty("AppDate", value); }
        }
        /// <summary>
        /// 确定人
        /// </summary>
        public String AppUser
        {
            get { return getProperty<String>("AppUser"); }
            set { setProperty("AppUser", value); }
        }
        /// <summary>
        /// 订单类型
        /// </summary>
        public String iOrderTypeID
        {
            get { return getProperty<String>("iOrderTypeID"); }
            set { setProperty("iOrderTypeID", value); }
        }
        /// <summary>
        /// 单据状态
        /// </summary>
        public int iState
        {
            get { return getProperty<int>("iState"); }
            set { setProperty("iState", value); }
        }
        /// <summary>
        /// 设计师
        /// </summary>
        public String vcDesign
        {
            get { return getProperty<String>("vcDesign"); }
            set { setProperty("vcDesign", value); }
        }
        /// <summary>
        /// 设计师电话
        /// </summary>
        public String vcDesignPhone
        {
            get { return getProperty<String>("vcDesignPhone"); }
            set { setProperty("vcDesignPhone", value); }
        }
        /// <summary>
        /// 参考交付周期
        /// </summary>
        public int numDayRequest
        {
            get { return getProperty<int>("numDayRequest"); }
            set { setProperty("numDayRequest", value); }
        }
        /// <summary>
        /// 交付周期
        /// </summary>
        public int numDay
        {
            get { return getProperty<int>("numDay"); }
            set { setProperty("numDay", value); }
        }
        /// <summary>
        /// 参考交付日期
        /// </summary>
        public DateTime dtDelivery
        {
            get { return getProperty<DateTime>("dtDelivery"); }
            set { setProperty("dtDelivery", value); }
        }
        /// <summary>
        /// 有无图纸
        /// </summary>
        public int IsPicture
        {
            get { return getProperty<int>("IsPicture"); }
            set { setProperty("IsPicture", value); }
        }
        /// <summary>
        /// 客户信息
        /// </summary>
        public String vcBuy
        {
            get { return getProperty<String>("vcBuy"); }
            set { setProperty("vcBuy", value); }
        }
        /// <summary>
        /// 客户电话
        /// </summary>
        public String vcBuyPhone
        {
            get { return getProperty<String>("vcBuyPhone"); }
            set { setProperty("vcBuyPhone", value); }
        }
        /// <summary>
        /// 客户地址
        /// </summary>
        public String vcBuyAddress
        {
            get { return getProperty<String>("vcBuyAddress"); }
            set { setProperty("vcBuyAddress", value); }
        }
        /// <summary>
        /// 图纸地址
        /// </summary>
        public String vcImgFile
        {
            get { return getProperty<String>("vcImgFile"); }
            set { setProperty("vcImgFile", value); }
        }
        #endregion Model

        /// <summary>
        /// 经销商名称
        /// </summary>
        public string CustomerName
        {
            get;
            set;
        }
    }
}
