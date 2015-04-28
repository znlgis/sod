using System;
using PWMIS.Common;
using PWMIS.DataMap.Entity;

namespace SODTest
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public partial class BCustomer : EntityBase
    {
        public BCustomer()
        {
            TableName = "tb_BCustomer";
            EntityMap = EntityMapType.Table;
            //IdentityName = "ISID";
            PrimaryKeys.Add("ISID");
        }

        protected override void SetFieldNames()
        {
            PropertyNames = new string[] { "ISID", "vcCode", "vcName", "iAreaID", "iCityID", "vcEasyCode", "vcLinkMan", "vcLinkPhone", "vcLinkFax", "vcLinkEmail", "vcLinkAddress", "InUse", "iParentID", "numDiscount", "CanSetSample" };
        }

        #region Model
        /// <summary>
        /// 
        /// </summary>
        public int ISID
        {
            get { return getProperty<int>("ISID"); }
            set { setProperty("ISID", value); }
        }
        /// <summary>
        /// 公司編碼
        /// </summary>
        public String vcCode
        {
            get { return getProperty<String>("vcCode"); }
            set { setProperty("vcCode", value); }
        }
        /// <summary>
        /// 公司名稱
        /// </summary>
        public String vcName
        {
            get { return getProperty<String>("vcName"); }
            set { setProperty("vcName", value); }
        }
        /// <summary>
        /// 区域tb_BAreaID
        /// </summary>
        public int iAreaID
        {
            get { return getProperty<int>("iAreaID"); }
            set { setProperty("iAreaID", value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public int iCityID
        {
            get { return getProperty<int>("iCityID"); }
            set { setProperty("iCityID", value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public String vcEasyCode
        {
            get { return getProperty<String>("vcEasyCode"); }
            set { setProperty("vcEasyCode", value); }
        }
        /// <summary>
        /// 联系人
        /// </summary>
        public String vcLinkMan
        {
            get { return getProperty<String>("vcLinkMan"); }
            set { setProperty("vcLinkMan", value); }
        }
        /// <summary>
        /// 电话
        /// </summary>
        public String vcLinkPhone
        {
            get { return getProperty<String>("vcLinkPhone"); }
            set { setProperty("vcLinkPhone", value); }
        }
        /// <summary>
        /// 传真
        /// </summary>
        public String vcLinkFax
        {
            get { return getProperty<String>("vcLinkFax"); }
            set { setProperty("vcLinkFax", value); }
        }
        /// <summary>
        /// Email
        /// </summary>
        public String vcLinkEmail
        {
            get { return getProperty<String>("vcLinkEmail"); }
            set { setProperty("vcLinkEmail", value); }
        }
        /// <summary>
        /// 地址
        /// </summary>
        public String vcLinkAddress
        {
            get { return getProperty<String>("vcLinkAddress"); }
            set { setProperty("vcLinkAddress", value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public String InUse
        {
            get { return getProperty<String>("InUse"); }
            set { setProperty("InUse", value); }
        }
        /// <summary>
        /// 上级代理商
        /// </summary>
        public int iParentID
        {
            get { return getProperty<int>("iParentID"); }
            set { setProperty("iParentID", value); }
        }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal numDiscount
        {
            get { return getProperty<decimal>("numDiscount"); }
            set { setProperty("numDiscount", value); }
        }
        /// <summary>
        /// 是否可以下样单(0否1是)
        /// </summary>
        public int CanSetSample
        {
            get { return getProperty<int>("CanSetSample"); }
            set { setProperty("CanSetSample", value); }
        }
        #endregion Model
    }
}
