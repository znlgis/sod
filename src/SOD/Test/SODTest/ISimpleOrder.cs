using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SODTest
{
    public interface ISimpleOrder
    {
        long OrderID { get; set; }
        string OrderName { get; set; }
        int UserID { get; set; }
        DateTime OrderDate { get; set; }
        double OrderPrice { get; set; }
        ISimpleOrderItem[] OrderItems { get; }
    }

    public interface ISimpleOrderItem
    { 
         string GoodsID { get; set; }
         string GoodsName { get; set; }
         int Number { get; set; }
         double UnitPrice { get; set; }
    }

    public class SimpleOrder : ISimpleOrder
    {
        public long OrderID { get; set; }
        public string OrderName { get; set; }
        public int UserID { get; set; }
        public DateTime OrderDate { get; set; }
        public double OrderPrice { get; set; }
        public ISimpleOrderItem[] OrderItems { get; set; }
    }

    public class SimpleOrderItem: ISimpleOrderItem
    {
        public string GoodsID { get; set; }
        public string GoodsName { get; set; }
        public int Number { get; set; }
        public double UnitPrice { get; set; }
    }

    public class SimpleOrderEntity :EntityBase, ISimpleOrder
    {
        public SimpleOrderEntity()
        {
            TableName = "TbOrders";
            PrimaryKeys.Add(nameof(OrderID));
        }

        public long OrderID
        {
            get { return getProperty<long>(nameof(OrderID)); }
            set { setProperty(nameof(OrderID), value); } 
        }
        public string OrderName
        {
            get { return getProperty<string>(nameof(OrderName)); }
            set { setProperty(nameof(OrderName), value, 100); } //长度 100
        }
        public int UserID
        {
            get { return getProperty<int>(nameof(UserID)); }
            set { setProperty(nameof(UserID), value); } 
        }
        public DateTime OrderDate
        {
            get { return getProperty<DateTime>(nameof(OrderDate)); }
            set { setProperty(nameof(OrderDate), value); } 
        }

        public double OrderPrice
        {
            get { return getProperty<double>(nameof(OrderPrice)); }
            set { setProperty(nameof(OrderPrice), value); }
        }

        public ISimpleOrderItem[] OrderItems 
        {
            get {
                return this.OrderItemEntities.ToArray();
            }
        }

        public List<SimpleOrderItemEntity> OrderItemEntities { get; set; }
        
    }

    public class SimpleOrderItemEntity : EntityBase, ISimpleOrderItem
    {
        public SimpleOrderItemEntity()
        {
            TableName = "TbOrderItems";
            IdentityName = nameof(ID);
            PrimaryKeys.Add(nameof(ID));
            SetForeignKey<SimpleOrderEntity>(nameof(OrderID));
        }

        public long ID
        {
            get { return getProperty<long>(nameof(ID)); }
            set { setProperty(nameof(ID), value); }
        }

        public long OrderID
        {
            get { return getProperty<long>(nameof(OrderID)); }
            set { setProperty(nameof(OrderID), value); }
        }

        public string GoodsID
        {
            get { return getProperty<string>(nameof(GoodsID)); }
            set { setProperty(nameof(GoodsID), value, 100); } //长度 100
        }
        public string GoodsName
        {
            get { return getProperty<string>(nameof(GoodsName)); }
            set { setProperty(nameof(GoodsName), value, 100); } //长度 100
        }
        public int Number
        {
            get { return getProperty<int>(nameof(Number)); }
            set { setProperty(nameof(Number), value); } 
        }
        public double UnitPrice
        {
            get { return getProperty<double>(nameof(UnitPrice)); }
            set { setProperty(nameof(UnitPrice), value); } 
        }
    }

}
