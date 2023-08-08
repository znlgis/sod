using System;
using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Data;
using PWMIS.MemoryStorage;
using SOD.DataSync.Entitys;

namespace SOD.DataSync
{
    internal class TestUtils
    {
        private static void TestWriteDataWarpper()
        {
            var remoteDbContext = new DemoDbContext(AdoHelper.CreateHelper("DemoDB"));
            //删除数据，必须通过DbContext 来处理，详细参见 解决方案说明.txt 文件的数据同步说明
            using (var warpper = WriteDataWarpperFactory.Create(remoteDbContext))
            {
                var entity = new UserEntity();
                entity.UID = 100;
                remoteDbContext.Remove(entity);
            }
        }


        private static void TestMemDb()
        {
            var db = MemDBEngin.GetDB();
            var list = db.Get<TestEntity>();
            Console.WriteLine("加载数据 {0}条", list.Count);

            var entitys = new TestEntity[10000];
            for (var i = 0; i < 10000; i++)
                entitys[i] = new TestEntity
                {
                    ID = i,
                    Name = "Name" + i,
                    AtTime = DateTime.Now
                };

            var flag = db.SaveEntity(entitys);
            if (flag)
                Console.WriteLine("保存数据成功！");
        }

        private static void SaveEntity<T>(MemDB mem, T[] entitys) where T : EntityBase, new()
        {
            var flag = mem.SaveEntity(entitys);
            if (flag)
                Console.WriteLine("保存数据成功！");
        }

        private static void ExportEntity<T>(MemDB mem, DemoDbContext dbContext) where T : EntityBase, new()
        {
            var entityList = dbContext.QueryAllList<T>();
            SaveEntity(mem, entityList.ToArray());
        }

        private static void ExportEntityData(EntityBase entity, MemDB mem, DemoDbContext dbContext)
        {
            var entityType = entity.GetType();
        }
    }
}