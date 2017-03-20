using System;
using System.Collections.Generic;
using System.Text;

using IBMP.AOP;
using PWMIS.EnterpriseFramework.Common;

namespace TranstarAuction.Model.AuctionMain
{
    public class MyFriendModel:BaseObject
    {
        /// <summary>
        /// 卖家用户ID
        /// </summary>
        public int SellerUId
        {
            get;
            set;
        }
        /// <summary>
        /// 卖家经销商ID
        /// </summary>
        public int SellerAId
        {
            get;
            set;
        }
        /// <summary>
        /// 所在城市
        /// </summary>
        public string City
        {
            get;
            set;
        }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName
        {
            get;
            set;
        }
        /// <summary>
        /// 卖家姓名
        /// </summary>
        public string SellerName
        {
            get;
            set;
        }
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }

    public class MyFriendNotifyModel:NotifyBase,IMyFriendModel
    {
        int _SellerUId;
        public int SellerUId
        {
            get { return _SellerUId; }
            set{_SellerUId=value;OnPropertyChanged("SellerUId");}
        }
        int _SellerAId;
        public int SellerAId
        {
            get{return _SellerAId;}
            set { _SellerAId = value; OnPropertyChanged("SellerAId"); }
        }
        string _City;
        public string City
        {
            get { return _City; }
            set { _City = value; OnPropertyChanged("City"); }
        }
        string _CompanyName;
        public string CompanyName
        {
            get { return _CompanyName; }
            set { _CompanyName = value; OnPropertyChanged("CompanyName"); }
        }
        string _SellerName;
        public string SellerName
        {
            get { return _SellerName; }
            set { _SellerName = value; OnPropertyChanged("SellerName"); }
        }
    }
}
