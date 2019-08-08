using SOD.DataSync.Entitys;
using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Data;
using PWMIS.MemoryStorage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SOD.DataSync
{
    class TestUtils
    {
        static void TestWriteDataWarpper()
        {
            DemoDbContext remoteDbContext = new DemoDbContext(AdoHelper.CreateHelper("DemoDB"));
            //删除数据，必须通过DbContext 来处理，详细参见 解决方案说明.txt 文件的数据同步说明
            using (IWriteDataWarpper warpper = WriteDataWarpperFactory.Create(remoteDbContext))
            {
                UserEntity entity = new UserEntity();
                entity.UID = 100;
                remoteDbContext.Remove<UserEntity>(entity);
            }
        }


        static void TestMemDb()
        {
            MemDB db = MemDBEngin.GetDB();
            List<TestEntity> list = db.Get<TestEntity>();
            Console.WriteLine("加载数据 {0}条", list.Count);

            TestEntity[] entitys = new TestEntity[10000];
            for (int i = 0; i < 10000; i++)
            {
                entitys[i] = new TestEntity()
                {
                    ID = i,
                    Name = "Name" + i,
                    AtTime = DateTime.Now
                };
            }

            bool flag = db.SaveEntity<TestEntity>(entitys);
            if (flag)
                Console.WriteLine("保存数据成功！");
        }

        static void SaveEntity<T>(MemDB mem, T[] entitys) where T : EntityBase, new()
        {
            bool flag = mem.SaveEntity<T>(entitys);
            if (flag)
                Console.WriteLine("保存数据成功！");
        }

        static void ExportEntity<T>(MemDB mem, DemoDbContext dbContext) where T : EntityBase, new()
        {
            List<T> entityList = dbContext.QueryAllList<T>();
            SaveEntity<T>(mem, entityList.ToArray());
        }

        static void ExportEntityData(EntityBase entity, MemDB mem, DemoDbContext dbContext)
        {
            Type entityType = entity.GetType();
        }
    }
}
