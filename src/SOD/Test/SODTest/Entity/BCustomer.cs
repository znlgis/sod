using System;
using PWMIS.Common;
using PWMIS.DataMap.Entity;

namespace SODTest
{
    /// <summary>
    /// </summary>
    [Serializable]
    public class BCustomer : EntityBase
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
            PropertyNames = new[]
            {
                "ISID", "vcCode", "vcName", "iAreaID", "iCityID", "vcEasyCode", "vcLinkMan", "vcLinkPhone", "vcLinkFax",
                "vcLinkEmail", "vcLinkAddress", "InUse", "iParentID", "numDiscount", "CanSetSample"
            };
        }

        #region Model

        /// <summary>
        /// </summary>
        public int ISID
        {
            get => getProperty<int>("ISID");
            set => setProperty("ISID", value);
        }

        /// <summary>
        ///     公司編碼
        /// </summary>
        public string vcCode
        {
            get => getProperty<string>("vcCode");
            set => setProperty("vcCode", value);
        }

        /// <summary>
        ///     公司名稱
        /// </summary>
        public string vcName
        {
            get => getProperty<string>("vcName");
            set => setProperty("vcName", value);
        }

        /// <summary>
        ///     区域tb_BAreaID
        /// </summary>
        public int iAreaID
        {
            get => getProperty<int>("iAreaID");
            set => setProperty("iAreaID", value);
        }

        /// <summary>
        /// </summary>
        public int iCityID
        {
            get => getProperty<int>("iCityID");
            set => setProperty("iCityID", value);
        }

        /// <summary>
        /// </summary>
        public string vcEasyCode
        {
            get => getProperty<string>("vcEasyCode");
            set => setProperty("vcEasyCode", value);
        }

        /// <summary>
        ///     联系人
        /// </summary>
        public string vcLinkMan
        {
            get => getProperty<string>("vcLinkMan");
            set => setProperty("vcLinkMan", value);
        }

        /// <summary>
        ///     电话
        /// </summary>
        public string vcLinkPhone
        {
            get => getProperty<string>("vcLinkPhone");
            set => setProperty("vcLinkPhone", value);
        }

        /// <summary>
        ///     传真
        /// </summary>
        public string vcLinkFax
        {
            get => getProperty<string>("vcLinkFax");
            set => setProperty("vcLinkFax", value);
        }

        /// <summary>
        ///     Email
        /// </summary>
        public string vcLinkEmail
        {
            get => getProperty<string>("vcLinkEmail");
            set => setProperty("vcLinkEmail", value);
        }

        /// <summary>
        ///     地址
        /// </summary>
        public string vcLinkAddress
        {
            get => getProperty<string>("vcLinkAddress");
            set => setProperty("vcLinkAddress", value);
        }

        /// <summary>
        /// </summary>
        public string InUse
        {
            get => getProperty<string>("InUse");
            set => setProperty("InUse", value);
        }

        /// <summary>
        ///     上级代理商
        /// </summary>
        public int iParentID
        {
            get => getProperty<int>("iParentID");
            set => setProperty("iParentID", value);
        }

        /// <summary>
        ///     折扣
        /// </summary>
        public decimal numDiscount
        {
            get => getProperty<decimal>("numDiscount");
            set => setProperty("numDiscount", value);
        }

        /// <summary>
        ///     是否可以下样单(0否1是)
        /// </summary>
        public int CanSetSample
        {
            get => getProperty<int>("CanSetSample");
            set => setProperty("CanSetSample", value);
        }

        #endregion Model
    }
}