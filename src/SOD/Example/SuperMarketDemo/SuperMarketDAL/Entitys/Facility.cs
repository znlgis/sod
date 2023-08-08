/*
 * PDF.NET 数据开发框架
 * http://www.pwmis.com/sqlmap
 */

using System;
using PWMIS.DataMap.Entity;

namespace SuperMarketDAL.Entitys
{
    /// <summary>
    ///     设备实体
    /// </summary>
    public class Facility : EntityBase
    {
        public Facility()
        {
            TableName = "设备表";
            PrimaryKeys.Add("编号"); //主键
            //IdentityName = "编号";//标识，自增
        }

        public string SerialNumber
        {
            get => getProperty<string>("编号");
            set => setProperty("编号", value);
        }

        public string FacilityName
        {
            get => getProperty<string>("设备名称");
            set => setProperty("设备名称", value);
        }

        public string Model
        {
            get => getProperty<string>("型号");
            set => setProperty("型号", value);
        }

        public DateTime PurchaseDate
        {
            get => getProperty<DateTime>("购置时间");
            set => setProperty("购置时间", value);
        }

        public string Memo
        {
            get => getProperty<string>("备注");
            set => setProperty("备注", value);
        }

        protected override void SetFieldNames()
        {
            PropertyNames = new[] { "编号", "设备名称", "型号", "购置时间", "备注" };
        }
    }
}