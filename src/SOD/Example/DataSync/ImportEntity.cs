using System;
using System.Collections.Generic;
using System.Linq;
using PWMIS.Core.Extensions;
using PWMIS.DataMap.Entity;
using PWMIS.MemoryStorage;
using SOD.DataSync.Entitys;

namespace SOD.DataSync
{
    /// <summary>
    ///     导入数据
    /// </summary>
    internal class SimpleImportEntitys
    {
        private const string C_Classification = "Classification";
        private const string C_BatchNumber = "BatchNumber";
        private readonly DbContext CurrDbContext;
        private readonly MemDB MemDB;

        public SimpleImportEntitys(MemDB mem, DbContext dbContext)
        {
            MemDB = mem;
            CurrDbContext = dbContext;
        }

        /// <summary>
        ///     数据分类标识
        /// </summary>
        public string Classification { get; set; }

        private void Importer_BeforeImport<T>(object sender, ImportEntityEventArgs<T> e) where T : EntityBase, new()
        {
            //导入前数据检查
            if (e.DataList.Count > 0)
            {
                //检查是否需要导入指定数据分类的数据
                if (string.IsNullOrEmpty(Classification))
                {
                    var list = e.DataList as List<EntityBase>;
                    var entity = list[0];
                    var fieldList = entity.PropertyNames;
                    //如果目标表字段包含该数据分类字段
                    if (fieldList.Contains(C_Classification))
                    {
                        var count = list.Count(p => (string)p[C_Classification] != Classification);
                        if (count > 0)
                        {
                            e.Cancel = true;
                            Console.WriteLine("当前表{0}有{1}条记录不符合指定数据分类（分类标识={2}）值的数据，本次导入取消。",
                                e.ImportTable, count, Classification);
                        }
                    }
                }

                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine("BeforeImport：\t 导入批次号：{0}\t 导入表名称：{1}\t 导入记录数：{2}，导入模式：{3}",
                    e.BatchNumber,
                    e.ImportTable,
                    e.DataList.Count, e.Mode
                );
            }
        }

        private void Importer_AfterImport<T>(object sender, ImportEntityEventArgs<T> e) where T : EntityBase, new()
        {
            if (e.DataList.Count > 0)
                Console.WriteLine("AfterImport：\t 导入批次号：{0}\t 导入表名称：{1}\t 导入记录数：{2}",
                    e.BatchNumber,
                    e.ImportTable,
                    e.DataList.Count
                );
        }

        private ImportResult InnerImport<T>(ImportMode model, Func<T, T, bool> isNew) where T : EntityBase, new()
        {
            var imp = new ImportEntity<T>(MemDB, CurrDbContext);
            imp.AfterImport += Importer_AfterImport;
            imp.BeforeImport += Importer_BeforeImport;
            return imp.Import(model, isNew);
        }

        /// <summary>
        ///     导入数据
        /// </summary>
        public void DoImportData()
        {
            //首先导入其它业务表            
            ShowImportResult(InnerImport<TestEntity>(ImportMode.Update, (s, t) => s.AtTime > t.AtTime));
            ShowImportResult(InnerImport<UserEntity>(ImportMode.Merge, null));

            //由于要处理上一次未导入成功的情况，新的数据文件包含了上次的数据，所以要在其它表导入成功后再删除表需要删除的记录。
            var result = InnerImport<DeletedPKIDEntity>(ImportMode.TruncateAndInsert, null);
            if (result.Flag == ImportResultFlag.Succeed) ExecuteDeleteData();
            ShowImportResult(result);
        }

        /// <summary>
        ///     根据ID删除表，删除目标表的数据
        /// </summary>
        private void ExecuteDeleteData()
        {
            var idList = MemDB.Get<DeletedPKIDEntity>();
            var tableNames = idList.Select(s => s.TargetTableName).Distinct().ToArray();
            var entitys = CurrDbContext.ResolveAllEntitys();

            foreach (var name in tableNames)
            {
                var targetEntity = entitys.FirstOrDefault(p => p.GetTableName() == name);
                if (targetEntity == null)
                {
                    Console.WriteLine("表名称{0} 不属于实体类型集合中的任意一个元素", name);
                    continue;
                }

                var pkName = targetEntity.PrimaryKeys[0];
                var targetIDs = idList.Where(p => p.TargetTableName == name && p.TargetID > 0).Select(s => s.TargetID)
                    .ToArray();
                Console.WriteLine("表{0} 可能有{1}条数据需要删除..", name, targetIDs.Length);
                OQL q = null;
                var DATA_BLOCK = 500; //必须限定in查询的数量，否则出错
                if (targetIDs.Length > 0)
                {
                    targetEntity[pkName] = 0;

                    if (targetIDs.Length > DATA_BLOCK)
                    {
                        var arrIds = new long[DATA_BLOCK];
                        var offset = 0;
                        while (offset < targetIDs.Length)
                        {
                            var length = offset + DATA_BLOCK < targetIDs.Length
                                ? DATA_BLOCK
                                : targetIDs.Length - offset;
                            if (length < DATA_BLOCK)
                                //处理结尾的数据
                                arrIds = new long[length];
                            Array.Copy(targetIDs, offset, arrIds, 0, length);
                            //构造和执行查询
                            q = OQL.From(targetEntity)
                                .Delete()
                                .Where(cmp => cmp.Comparer(Convert.ToInt64(targetEntity[pkName]), "in", arrIds))
                                .END;
                            var cur_count = EntityQuery.ExecuteOql(q, CurrDbContext.CurrentDataBase);
                            Console.WriteLine("已经删除表{0} {1}条数据！", name, cur_count);

                            offset += DATA_BLOCK;
                        }
                    }
                    else
                    {
                        //构造和执行查询
                        q = OQL.From(targetEntity)
                            .Delete()
                            .Where(cmp => cmp.Comparer(Convert.ToInt64(targetEntity[pkName]), "in", targetIDs))
                            .END;
                        var cur_count = EntityQuery.ExecuteOql(q, CurrDbContext.CurrentDataBase);
                        Console.WriteLine("已经删除表{0} {1}条数据！", name, cur_count);
                    }
                }
                else
                {
                    //字符串ID处理
                    var targetStringIDs = idList.Where(p => p.TargetTableName == name).Select(s => s.TargetStringID)
                        .ToArray();
                    targetEntity[pkName] = "0";
                    //
                    if (targetStringIDs.Length > DATA_BLOCK)
                    {
                        var arrIds = new string[DATA_BLOCK];
                        var offset = 0;
                        while (offset < targetStringIDs.Length)
                        {
                            var length = offset + DATA_BLOCK < targetStringIDs.Length
                                ? DATA_BLOCK
                                : targetStringIDs.Length - offset;
                            if (length < DATA_BLOCK)
                                //处理结尾的数据
                                arrIds = new string[length];
                            Array.Copy(targetStringIDs, offset, arrIds, 0, length);
                            //构造和执行查询
                            q = OQL.From(targetEntity)
                                .Delete()
                                .Where(cmp => cmp.Comparer((string)targetEntity[pkName], "in", arrIds))
                                .END;
                            var cur_count = EntityQuery.ExecuteOql(q, CurrDbContext.CurrentDataBase);
                            Console.WriteLine("已经删除表{0} {1}条数据！", name, cur_count);

                            offset += DATA_BLOCK;
                        }
                    }
                    else
                    {
                        //构造和执行查询
                        q = OQL.From(targetEntity)
                            .Delete()
                            .Where(cmp => cmp.Comparer((string)targetEntity[pkName], "in", targetStringIDs))
                            .END;
                        var cur_count = EntityQuery.ExecuteOql(q, CurrDbContext.CurrentDataBase);
                        Console.WriteLine("已经删除表{0} {1}条数据！", name, cur_count);
                    }
                }
            }
        }

        private void ShowImportResult(ImportResult result)
        {
            Console.WriteLine("导入数据完成：\t 导入表名称：{0}\t 导入批次号：{1}\t 是否取消导入：{2}\r\n 导入记录数：{3}\t 用时（秒）：{4}\t 错误消息：{5}",
                result.ImportTable,
                result.BatchNumber,
                result.IsCancel,
                result.ImportCount,
                result.Duration,
                result.ErrorMessage);
            Console.Write("导入结果：");
            switch (result.Flag)
            {
                case ImportResultFlag.NoBatchInfo:
                    Console.WriteLine("没有批次信息，不能导入");
                    break;
                case ImportResultFlag.IsOldData:
                    Console.WriteLine("没有新数据，可能是重复导入");
                    break;
                case ImportResultFlag.UserCanceled:
                    Console.WriteLine("导入已被用户取消");
                    break;
                case ImportResultFlag.Succeed:
                    Console.WriteLine("导入成功");
                    break;
                case ImportResultFlag.Error:
                    Console.WriteLine("导入失败");
                    break;
            }

            Console.WriteLine("=======================================================");
        }
    }
}