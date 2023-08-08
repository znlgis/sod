using PWMIS.Core.Extensions;

namespace SODTest
{
    public class SimpleDbContext : DbContext
    {
        public SimpleDbContext() : base("local2")
        {
        }

        protected override bool CheckAllTableExists()
        {
            CheckTableExists<UserEntity>();
            CheckTableExists<SimpleOrderEntity>();
            //创建表以后立即创建索引
            InitializeTable<SimpleOrderItemEntity>("CREATE INDEX [Idx_OrderID] On [{0}] ([OrderID])");
            return true;
        }
    }
}