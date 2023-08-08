using System.Collections.Generic;
using PWMIS.Core.Extensions;
using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Data;

namespace SOD.DataSync.Entitys
{
    // public class AduitWorkDbContext:DbContext
    internal class DemoDbContext : DbContext
    {
        public DemoDbContext(AdoHelper db) : base(db)
        {
        }

        protected override bool CheckAllTableExists()
        {
            //导入数据必须的
            CheckTableExists<DeletedPKIDEntity>();
            //以下是业务实体：
            CheckTableExists<TestEntity>();
            CheckTableExists<UserEntity>();

            return true;
        }

        public List<T> QueryAllList<T>() where T : EntityBase, new()
        {
            return OQL.From<T>().ToList(CurrentDataBase);
        }
    }
}