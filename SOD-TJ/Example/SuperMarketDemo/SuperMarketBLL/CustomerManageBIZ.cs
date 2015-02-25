/*
 * PDF.NET 数据开发框架
 * http://www.pwmis.com/sqlmap
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperMarketDAL.Entitys;
using PWMIS.DataMap.Entity;
using SuperMarketModel;
using PWMIS.Core.Extensions;

namespace SuperMarketBLL
{
    public class CustomerManageBIZ
    {
        public bool AddContactInfo(CustomerContactInfo info)
        {
            return EntityQuery<CustomerContactInfo>.Instance.Insert(info)>0;
        }

        public bool UpdateContactInfo(CustomerContactInfo info)
        {
            return EntityQuery<CustomerContactInfo>.Instance.Update (info) > 0;
        }

        public bool RemoveContactInfo(CustomerContactInfo info)
        {
            return EntityQuery<CustomerContactInfo>.Instance.Delete (info) > 0;
        }

        /// <summary>
        /// 获取一个顾客信息，如果没有则返回空对象
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public CustomerContactInfo GetCustomerContactInfo(string customerID)
        {
            CustomerContactInfo info = new CustomerContactInfo() { CustomerID = customerID };
            if (EntityQuery<CustomerContactInfo>.Fill(info))
                return info;
            else
                return null;
        }

        /// <summary>
        /// 获取一个已经注册的客户，如果获取不成功，则返回空
        /// </summary>
        /// <param name="customerID">客户号</param>
        /// <returns></returns>
        public Customer GetRegistedCustomer(string customerID)
        {
            CustomerContactInfo info = GetCustomerContactInfo(customerID);
            if (info != null)
            {
                return new Customer() {  CustomerID =info.CustomerID, CustomerName =info.CustomerName };
            }
            return null;
        }

        /// <summary>
        /// 获取联系人信息记录数量
        /// </summary>
        /// <returns></returns>
        public int GetContactInfoCount()
        {
            CustomerContactInfo info = new CustomerContactInfo();
            OQL q = OQL.From(info)
                .Select()
                .Count(info.CustomerID, "tempField").END;
            CustomerContactInfo infoCount = EntityQuery<CustomerContactInfo>.QueryObject(q);
            return Convert.ToInt32(infoCount.PropertyList("tempField"));
        }

        /// <summary>
        /// 获取指定页的联系人信息
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="allCount"></param>
        /// <returns></returns>
        public List<CustomerContactInfo> GetContactInfoList(int pageSize, int pageNumber, int allCount)
        {
            CustomerContactInfo info = new CustomerContactInfo();
            OQL q = new OQL(info);
            q.Select().OrderBy(info.CustomerName, "asc");
            q.Limit(pageSize, pageNumber);
            q.PageWithAllRecordCount = allCount;

            //return EntityQuery<CustomerContactInfo>.QueryList(q);
            return q.ToList<CustomerContactInfo>();
        }
    }
}
