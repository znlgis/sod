using PWMIS.Core.Extensions;
using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Data;
using System.Collections.Generic;

namespace SOD.DataSync.Entitys
{
    // public class AduitWorkDbContext:DbContext
    class DemoDbContext:DbContext
    {
        public DemoDbContext(AdoHelper db) : base(db)
        {

        }

        protected override bool CheckAllTableExists()
        {
            //导入数据必须的
            base.CheckTableExists<DeletedPKIDEntity>();
            //以下是业务实体：
            base.CheckTableExists<TestEntity>();
            base.CheckTableExists<UserEntity>();

            return true;
        }

        public List<T> QueryAllList<T>() where T : EntityBase, new()
        {
            return OQL.From<T>().ToList(this.CurrentDataBase);
        }
    }
 }
