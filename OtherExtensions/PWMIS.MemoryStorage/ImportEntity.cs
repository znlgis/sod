
using PWMIS.Core.Extensions;
using PWMIS.DataMap.Entity;
using PWMIS.MemoryStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PWMIS.MemoryStorage
{

    /// <summary>
    /// 导入数据的方式
    /// </summary>
    public enum ImportMode
    { 
        /// <summary>
        /// 追加，用于数据不会重复的情况
        /// </summary>
        Append,
        /// <summary>
        /// 清除和插入，在数据插入前先清除表的全部数据，用于不考虑差异数据的高效数据导入
        /// </summary>
        TruncateAndInsert,
        /// <summary>
        /// 更新，要更新的数据必须有时间戳或者版本号之类的可以标识数据新旧的字段，数据会逐行更新
        /// </summary>
        Update,
        /// <summary>
        /// 合并，如果数据在目标表存在则读出然后合并不同的数据，不会理会数据的新旧标识；如果目标数据不存在，则插入此数据
        /// </summary>
        Merge,
        /// <summary>
        /// 用户自定义的其它数据导入处理方式
        /// </summary>
        UserDefined
    }

    public enum ImportResultFlag
    {
        /// <summary>
        /// 没有导入批次信息，不能导入
        /// </summary>
        NoBatchInfo,
        /// <summary>
        /// 数据是旧的，不需要导入，可能已经导入过
        /// </summary>
        IsOldData,
        /// <summary>
        /// 用户取消的导入
        /// </summary>
        UserCanceled,
        /// <summary>
        /// 成功
        /// </summary>
        Succeed
    }

    /// <summary>
    /// 数据导入结果信息
    /// </summary>
    public class ImportResult
    {
        /// <summary>
        /// 导入的记录数量
        /// </summary>
        public int ImportCount { get; protected internal set; }
        /// <summary>
        /// 导入的表名称
        /// </summary>
        public string ImportTable { get; protected internal set; }
        /// <summary>
        /// 导入的批次号
        /// </summary>
        public int BatchNumber { get; protected internal set; }
        /// <summary>
        /// 是否取消了导入
        /// </summary>
        public bool IsCancel { get; protected internal set; }
        /// <summary>
        /// 导入结果枚举
        /// </summary>
        public ImportResultFlag Flag { get; protected internal set; }
    }

    /// <summary>
    /// 实体数据导入参数类
    /// </summary>
    public class ImportEntityEventArgs : EventArgs
    {
        /// <summary>
        /// 要导入的列表数据
        /// </summary>
        public System.Collections.IList DataList { get; private set; }
        /// <summary>
        /// 实体类类型
        /// </summary>
        public Type EntityType { get; private set; }
        /// <summary>
        /// 导入的实体表名称
        /// </summary>
        public string ImportTable { get; private set; }
        /// <summary>
        /// 导入的批次号
        /// </summary>
        public int BatchNumber { get; private set; }
        /// <summary>
        /// 是否撤销导入
        /// </summary>
        public bool Cancel { get; set; }
        /// <summary>
        /// 使用一个数据集合和其它信息初始化本类
        /// </summary>
        /// <param name="list"></param>
        /// <param name="entityType"></param>
        /// <param name="table"></param>
        /// <param name="batchNumber"></param>
        public ImportEntityEventArgs(System.Collections.IList list, Type entityType, string table,int batchNumber)
        {
            this.DataList = list;
            this.EntityType = entityType;
            this.ImportTable = table;
            this.BatchNumber = batchNumber;
        }
    }

    /// <summary>
    /// 从内存数据库导入数据到关系数据库
    /// </summary>
    public class ImportEntity
    {
        MemDB MemDB;
        DbContext CurrDbContext;

        /// <summary>
        /// 导入前事件
        /// </summary>
        public event EventHandler<ImportEntityEventArgs> BeforeImport;
        /// <summary>
        /// 导入后事件
        /// </summary>
        public event EventHandler<ImportEntityEventArgs> AfterImport;

        /// <summary>
        /// 以一个内存数据库对象和数据上下文对象初始化本类
        /// </summary>
        /// <param name="mem">内存数据库对象</param>
        /// <param name="dbContext">数据上下文对象</param>
        public ImportEntity(MemDB mem, DbContext dbContext)
        {
            this.MemDB = mem;
            this.CurrDbContext = dbContext;
            dbContext.CheckTableExists<ExportBatchInfo>();
        }

        /// <summary>
        /// 导入数据到关系数据库
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="mode">导入模式</param>
        /// <param name="isNew">导入模式为更新模式的时候，进行实体类数据新旧比较的自定义方法，第一个参数为源实体，第二个参数为数据库的目标实体，返回源是否比目标新</param>
        /// <returns>导入的数据数量</returns>
        public ImportResult Import<T>(ImportMode mode, Func<T, T, bool> isNew) where T : EntityBase, new()
        {
            Type entityType = typeof(T);
            string importTableName = EntityFieldsCache.Item(entityType).TableName;
            ImportResult result = new ImportResult();
            result.ImportTable = importTableName;
            result.IsCancel = true;

            //导出批次管理
            List<ExportBatchInfo> batchList = MemDB.Get<ExportBatchInfo>();
            ExportBatchInfo currBatch = batchList.FirstOrDefault(p => p.ExportTableName == importTableName);
            if (currBatch == null)
            {
                result.Flag = ImportResultFlag.NoBatchInfo;
                return result;//没有导入批次信息，不能导入
            }
            //只有比数据库的导入批次数据新，才可以导入
            OQL q = OQL.From(currBatch)
                .Select()
                .Where(currBatch.ExportTableName)
                .END;
            ExportBatchInfo dbBatch=  this.CurrDbContext.QueryObject<ExportBatchInfo>(q);
            if (dbBatch == null)
            {
                currBatch.SetDefaultChanges();
                this.CurrDbContext.Add<ExportBatchInfo>(currBatch);
                result.BatchNumber = currBatch.BatchNumber;
            }
            else
            {
                result.BatchNumber = currBatch.BatchNumber;
                if (currBatch.BatchNumber <= dbBatch.BatchNumber)
                {
                    result.Flag = ImportResultFlag.IsOldData;
                    return result;//没有新数据需要导入
                }
                currBatch.ID = dbBatch.ID;
            }

            //导入数据
            int count = 0;// 
            List<T> list = this.MemDB.Get<T>();
            if (list.Count > 0)
            {
                ImportEntityEventArgs args = new ImportEntityEventArgs(list, entityType, importTableName, currBatch.BatchNumber);
                if (BeforeImport != null)
                {
                    BeforeImport(this,args);
                    if (args.Cancel)
                    {
                        result.Flag = ImportResultFlag.UserCanceled;
                        return result;
                    }
                }
               
                if (mode == ImportMode.Append)
                {
                    list.ForEach(item => {
                        item.SetDefaultChanges();
                    });
                    count = this.CurrDbContext.AddList(list);
                }
                else if (mode == ImportMode.TruncateAndInsert)
                {
                    string sql = "TRUNCATE TABLE [" + importTableName + "]";
                    this.CurrDbContext.CurrentDataBase.ExecuteNonQuery(sql);
                    list.ForEach(item =>
                    {
                        item.SetDefaultChanges();
                    });
                    count = this.CurrDbContext.AddList(list);
                }
                else if (mode == ImportMode.Update)
                {
                    if (isNew == null)
                        throw new ArgumentNullException("当 ImportMode 为Update 模式的时候，参数 isNew 不能为空。");
                    foreach (T item in list)
                    {
                        T dbEntity = (T)item.Clone();
                        EntityQuery eq = new EntityQuery(this.CurrDbContext.CurrentDataBase);
                        if (eq.FillEntity(dbEntity))
                        {
                            if (isNew(item, dbEntity))
                            {
                                item.SetDefaultChanges();//设置了更改状态，才可以更新到数据库
                                count += eq.Update(item);
                            }
                        }
                    }
                }
                else if (mode == ImportMode.Merge)
                {
                    foreach (T item in list)
                    {
                        T dbEntity = (T)item.Clone();
                        EntityQuery eq = new EntityQuery(this.CurrDbContext.CurrentDataBase);
                        if (eq.FillEntity(dbEntity))
                        {
                            int changedCount = dbEntity.MapFrom(item, true);
                            if (changedCount > 0)
                            {
                               count+= eq.Update(dbEntity);
                            }
                        }
                        else
                        {
                            //没有Fill成功实体，说明数据库没有此数据，则添加数据到数据库
                            item.SetDefaultChanges();
                            count+= eq.Insert(item);
                        }
                    }
                }
                else
                {
                    //自定义的处理方式，请在 BeforeImport 事件自行处理
                }
                
                if (AfterImport != null)
                {
                    args.Cancel = false;
                    AfterImport(this,args);
                }
              
            }//end if
            //更新导入批次信息
            currBatch.BatchNumber = result.BatchNumber;
            currBatch.LastExportDate = DateTime.Now;
            this.CurrDbContext.Update<ExportBatchInfo>(currBatch);

            result.IsCancel = false;
            result.Flag = ImportResultFlag.Succeed;
            result.ImportCount = count;
            return result;
        }

        /*
         * 过时的代码，仅做反射示例
         * 
        /// <summary>
        /// 根据实体类对象，导入内存数据到数据库
        /// </summary>
        /// <param name="entity">参照的实体类对象</param>
        /// <returns>导入的数据数量</returns>
        public int ImportData(EntityBase entity)
        {
            Type entityType = entity.GetType();
            MethodInfo method = GetType().GetMethod("Import", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo genericMethod = method.MakeGenericMethod(entityType);
            Func<int> fun = (Func<int>)System.Delegate.CreateDelegate(typeof(Func<int>), this, genericMethod);

          
            return fun();
        }

        /// <summary>
        /// 从内存数据库导入全部数据到关系数据库
        /// </summary>
        public void ImportAllData()
        {
            foreach (EntityBase entity in CurrDbContext.ResolveAllEntitys())
            {
                ImportData(entity);
            }
        }
         * 
         */ 
    }
}
