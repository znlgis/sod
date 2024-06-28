using PWMIS.Core.Extensions;
using SimpleDemo.Entity;
using SimpleDemo.Interface.Infrastructure;

namespace SimpleDemo.Repository
{
    public class SimpleDbContext : DbContext, IUowManager
    {
        public SimpleDbContext():base("SimpleDB")
        {
            ID = Guid.NewGuid(); 
        }

        public Guid ID { get; private set; }

        public void BeginTransaction()
        {
            Console.WriteLine("开始事务。。。。");
            base.CurrentDataBase.BeginTransaction();
        }

        public void Commit()
        {
            Console.WriteLine("提交事务");
            base.CurrentDataBase.Commit();
        }

        public void Rollback()
        {
            Console.WriteLine("回滚事务");
            base.CurrentDataBase.Rollback();
        }

        protected override bool CheckAllTableExists()
        {
            //CheckTableExists<EquipmentEntity>();
            InitializeTable<EquipmentEntity>("CREATE UNIQUE INDEX [idx_equid] On [{0}] ([EquipmentID]);");
            return true;
        }
    }
}
