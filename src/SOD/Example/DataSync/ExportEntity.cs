using System;
using System.IO;
using System.Linq;
using PWMIS.Core.Extensions;
using PWMIS.DataMap.Entity;
using PWMIS.MemoryStorage;
using SOD.DataSync.Entitys;

namespace SOD.DataSync
{
    /// <summary>
    ///     导出审计作业实体数据
    /// </summary>
    internal class SimpleExportEntitys
    {
        private const string C_Classification = "Classification";
        private const string C_BatchNumber = "BatchNumber";
        private readonly DbContext CurrDbContext;
        private readonly MemDB MemDB;


        public SimpleExportEntitys(MemDB mem, DbContext dbContext)
        {
            MemDB = mem;
            CurrDbContext = dbContext;
        }

        /// <summary>
        ///     数据分类标识，比如项目标识，产品标识
        /// </summary>
        public string ClassificationID { get; set; }

        /// <summary>
        ///     是否全部成功
        /// </summary>
        public bool AllSucceed { get; private set; }

        /// <summary>
        ///     （导出）有数据的表的数量
        /// </summary>
        public int HaveDataTableCount { get; private set; }

        /// <summary>
        ///     当前数据文件备份目录
        /// </summary>
        public string DataBackFolder { get; private set; }

        private void Exporter_OnSaved<T>(object sender, ExportEntityEventArgs<T> e) where T : EntityBase, new()
        {
            if (e.Succeed)
                Console.WriteLine("保存数据成功！");
            else
                Console.WriteLine("保存数据失败。");
        }

        private void Exporter_OnExported<T>(object sender, ExportEntityEventArgs<T> e) where T : EntityBase, new()
        {
            Console.WriteLine("------------------------------------------------------------");
            if (e.Succeed)
            {
                //处理上次没有导入的剩余数据============================
                //尝试加载本地数据,与导出的数据合并，以本次导出的数据优先
                var lastData = MemDB.LoadEntity<T>();
                if (lastData != null && lastData.Count > 0)
                {
                    var pkName = lastData[0].PrimaryKeys[0];
                    if (e.ExportedDataList.Count > 0)
                    {
                        var ids = e.ExportedDataList.Select(p => p[pkName]).ToArray();
                        var except = lastData.Where(p => !ids.Contains(p[pkName])).ToList();
                        e.ExportedDataList.AddRange(except);
                    }
                    else
                    {
                        e.ExportedDataList.AddRange(lastData);
                    }
                }

                if (e.ExportedDataList.Count > 0)
                {
                    if (e.EntityType == typeof(DeletedPKIDEntity))
                    {
                        //ID记录表，导出后删除当前批次数据库记录
                        var entity = new DeletedPKIDEntity();
                        var q = OQL.From(entity)
                            .Delete()
                            .Where(cmp => cmp.Comparer(entity.BatchNumber, "=", e.BatchNumber))
                            .END;
                        var count = EntityQuery<DeletedPKIDEntity>.ExecuteOql(q, CurrDbContext.CurrentDataBase);
                        Console.WriteLine("当前导出批次{0} 已经清除当前的ID删除表信息记录，条数：{1}", e.BatchNumber, count);
                    }

                    //已经提前更新了导出批次号,参见 FilterQuery 方法
                    HaveDataTableCount++;
                }

                Console.WriteLine("导出数据成功！\t 导出批次号：{0}\t 导出表名称：{1}\t 导出记录数：{2}",
                    e.BatchNumber,
                    e.ExportTable,
                    e.ExportedDataList.Count);
            }
            else
            {
                Console.WriteLine("导出数据失败，\r\n 导出批次号：{0}\r\n 导出表名称：{1}\r\n 出错原因：{2}",
                    e.BatchNumber,
                    e.ExportTable,
                    e.OperationExcepiton.Message);
                AllSucceed = false;
            }
        }

        /// <summary>
        ///     执行导出数据
        /// </summary>
        public void DoExportData()
        {
            AllSucceed = true;
            HaveDataTableCount = 0;

            InnerExportData<DeletedPKIDEntity>(); //首先导出ID删除记录
            //然后导出业务实体数据
            InnerExportData<TestEntity>();
            InnerExportData<UserEntity>();

            //导出后就备份内存数据文件，以便处理完成后删除
            BackUp();
            //
        }


        /// <summary>
        ///     导出后就备份内存数据文件
        /// </summary>
        private void BackUp()
        {
            var source = Path.Combine(MemDB.Path, "Data");
            var targetDir1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "SOD_EXP_DATABAK", string.IsNullOrEmpty(ClassificationID) ? "0" : ClassificationID);
            if (!Directory.Exists(targetDir1))
                Directory.CreateDirectory(targetDir1);
            var targetDir2 = Path.Combine(targetDir1, "Data-" + DateTime.Now.ToString("yyyyMMddHHmmss"));
            if (!Directory.Exists(targetDir2))
                Directory.CreateDirectory(targetDir2);
            DataBackFolder = targetDir2;

            var dinfo = new DirectoryInfo(source);
            //注，这里面传的是路径，并不是文件，所以不能保含带后缀的文件                
            foreach (var f in dinfo.GetFileSystemInfos())
            {
                var destName = Path.Combine(targetDir2, f.Name);
                if (f is FileInfo)
                    //如果是文件就复制       
                    File.Copy(f.FullName, destName, true); //true代表可以覆盖同名文件                     
            }
        }


        private void InnerExportData<T>() where T : EntityBase, new()
        {
            var exp = new ExportEntity<T>(MemDB, CurrDbContext);
            exp.OnExported += Exporter_OnExported;
            exp.OnSaved += Exporter_OnSaved;
            //只导出更新过批次号的记录，不再需要使用 System.Data.IsolationLevel.Serializable 隔离级别
            CurrDbContext.CurrentDataBase.BeginTransaction();
            try
            {
                exp.Export(FilterQuery, GetBatchNumber);
                if (AllSucceed)
                    CurrDbContext.CurrentDataBase.Commit();
                else
                    CurrDbContext.CurrentDataBase.Rollback();
            }
            catch (Exception ex)
            {
                Console.WriteLine("导出数据执行事务遇到错误：" + ex.Message);
                CurrDbContext.CurrentDataBase.Rollback();
            }
        }

        /// <summary>
        ///     生成批次号为空或者为0的查询，对应于本地新增或者更新过的数据
        /// </summary>
        /// <param name="batchNumber">批次号</param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private OQL FilterQuery(int batchNumber, EntityBase entity)
        {
            if (entity is IExportTable)
            {
                var tableInfoCache = EntityFieldsCache.Item(entity.GetType());
                var fieldList = tableInfoCache.PropertyNames;
                Console.WriteLine("===================导出表 [{0}] ==================", entity.GetTableName());

                var ExportableEntity = entity as IExportTable;
                ExportableEntity.BatchNumber = batchNumber;
                //将数据库导出标记为0或者为空的记录，更新为当前导出标记号
                //一定得按数据分类标识更新及导出
                OQL updateQ = null;
                if (fieldList.Contains(C_Classification))
                {
                    entity[C_Classification] = ClassificationID;
                    updateQ = OQL.From(entity)
                        .Update(ExportableEntity.BatchNumber)
                        .Where(cmp => cmp.EqualValue(entity[C_Classification]) &
                                      (cmp.Comparer(ExportableEntity.BatchNumber, "=", 0) |
                                       cmp.IsNull(ExportableEntity.BatchNumber))
                        )
                        .END;
                    var count = EntityQuery.ExecuteOql(updateQ, CurrDbContext.CurrentDataBase);
                    Console.WriteLine("（查询前）更新批次号 {0} 受影响的记录数 {1}", batchNumber, count);

                    var q = OQL.From(entity)
                        .Select()
                        .Where(cmp =>
                            cmp.EqualValue(entity[C_Classification]) & cmp.EqualValue(ExportableEntity.BatchNumber))
                        .END;
                    return q;
                }
                else
                {
                    updateQ = OQL.From(entity)
                        .Update(ExportableEntity.BatchNumber)
                        .Where(cmp =>
                            cmp.Comparer(ExportableEntity.BatchNumber, "=", 0) |
                            cmp.IsNull(ExportableEntity.BatchNumber))
                        .END;
                    var count = EntityQuery.ExecuteOql(updateQ, CurrDbContext.CurrentDataBase);
                    Console.WriteLine("（查询前）更新批次号 {0} 受影响的记录数 {1}", batchNumber, count);
                    var q = OQL.From(entity)
                        .Select()
                        .Where(cmp => cmp.EqualValue(ExportableEntity.BatchNumber))
                        .END;
                    return q;
                }
            }

            return null;
        }

        /// <summary>
        ///     从数据库获取下一个使用的批次号
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private int GetBatchNumber(EntityBase entity)
        {
            var batchNumber = 0;
            if (entity is IExportTable)
            {
                var tableInfoCache = EntityFieldsCache.Item(entity.GetType());
                var fieldList = tableInfoCache.PropertyNames;

                //查询当前表最大的批次号
                if (fieldList.Contains(C_Classification))
                {
                    entity[C_Classification] = ClassificationID;
                    var q = OQL.From(entity)
                        .Select().Max(((IExportTable)entity).BatchNumber, "")
                        .Where(entity[C_Classification])
                        .END;
                    var dbEntity = EntityQuery.QueryObject<IExportTable>(q, CurrDbContext.CurrentDataBase);
                    batchNumber = dbEntity.BatchNumber;
                }
                else
                {
                    var q = OQL.From(entity)
                        .Select().Max(((IExportTable)entity).BatchNumber, "")
                        .END;
                    var dbEntity = EntityQuery.QueryObject<IExportTable>(q, CurrDbContext.CurrentDataBase);
                    batchNumber = dbEntity.BatchNumber;
                }
            }

            batchNumber += 1;
            return batchNumber;
        }

        /// <summary>
        ///     导出成功后，更新实体表的导出批次号
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="batchNumber"></param>
        /// <returns></returns>
        /// <remarks>已经在导出前更新了批次号，此方法已经废弃</remarks>
        private int UpdateBatchNumber(EntityBase entity, int batchNumber)
        {
            if (entity is IExportTable)
            {
                OQL q = null;
                entity[C_BatchNumber] = batchNumber;
                try
                {
                    //如果AuditworkProjectID 属性不存在，那么此时赋值会出错 AuditworkProjectID
                    entity[C_Classification] = ClassificationID;
                    q = OQL.From(entity)
                        .Update(entity[C_BatchNumber])
                        .Where(cmp => cmp.EqualValue(entity[C_Classification]) &
                                      (cmp.Comparer(entity[C_BatchNumber], "=", 0) | cmp.IsNull(entity[C_BatchNumber]))
                        )
                        .END;
                }
                catch (Exception ex)
                {
                    q = OQL.From(entity)
                        .Update(entity[C_BatchNumber])
                        .Where(cmp => cmp.Comparer(entity[C_BatchNumber], "=", 0) | cmp.IsNull(entity[C_BatchNumber]))
                        .END;
                }

                var count = EntityQuery.ExecuteOql(q, CurrDbContext.CurrentDataBase);
                return count;
            }

            return 0;
        }
    }
}