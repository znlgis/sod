
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
    /// 导出实体事件参数对象
    /// </summary>
    public class ExportEntityEventArgs : EventArgs
    {
        /// <summary>
        /// 导出的数据列表
        /// </summary>
        public System.Collections.IList ExportedDataList { get; private set; }
        /// <summary>
        /// 实体类类型
        /// </summary>
        public Type EntityType { get; private set; }
        /// <summary>
        /// 导出的实体表名称
        /// </summary>
        public string ExportTable { get; private set; }
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Succeed { get;protected internal set; }
        /// <summary>
        /// 操作异常时候的异常对象
        /// </summary>
        public Exception OperationExcepiton { get; protected internal set; }
        /// <summary>
        /// 当前表导出的批量号
        /// </summary>
        public int BatchNumber { get; protected internal set; }
        /// <summary>
        /// 是否撤销导出
        /// </summary>
        public bool Cancel { get; set; }

        public ExportEntityEventArgs(System.Collections.IList list, Type entityType, string table)
        {
            this.ExportedDataList = list;
            this.EntityType = entityType;
            this.ExportTable = table;
        }
    }

    /// <summary>
    /// 从关系数据库导出实体类到内存数据库
    /// </summary>
    public class ExportEntity
    {
        MemDB MemDB;
        DbContext CurrDbContext;

        /// <summary>
        /// 数据库表已经导出的时候
        /// </summary>
        public event EventHandler<ExportEntityEventArgs> OnExported;
        /// <summary>
        /// 导出的数据已经保存的时候
        /// </summary>
        public event EventHandler<ExportEntityEventArgs> OnSaved;

        /// <summary>
        /// 以一个内存数据库对象和数据上下文对象初始化本类
        /// </summary>
        /// <param name="mem">内存数据库对象</param>
        /// <param name="dbContext">数据上下文对象</param>
        public ExportEntity(MemDB mem, DbContext dbContext)
        {
            this.MemDB = mem;
            this.CurrDbContext = dbContext;
        }

        private void SaveEntity<T>(T[] entitys, ExportEntityEventArgs args) where T : EntityBase, new()
        {
            if (entitys.Length > 0)
            {
                args.Succeed = MemDB.SaveEntity<T>(entitys);
            }
            else
            {
                MemDB.DropEntity<T>();
                args.Succeed = true;
            }

            if (OnSaved != null)
                OnSaved(this, args);
        }

        /// <summary>
        /// 导出实体数据到内存数据库。如果当前实体操作失败，请检查导出事件的异常参数对象。
        /// </summary>
        /// <param name="funQ">获取导出数据的查询表达式委托方法，委托方法的参数为导出批次号；如果结果为空，导出实体全部数据</param>
        /// <typeparam name="T">实体类类型</typeparam>
        public void Export<T>(Func<int,T,OQL> funQ) where T : EntityBase, new()
        {
            Type entityType = typeof(T);
            try
            {
                //导出批次管理
                string exportTableName=EntityFieldsCache.Item(entityType).TableName;
                List<ExportBatchInfo> batchList= MemDB.Get<ExportBatchInfo>();
                ExportBatchInfo currBatch = batchList.FirstOrDefault(p => p.ExportTableName == exportTableName);
                if (currBatch == null)
                {
                    currBatch = new ExportBatchInfo();
                    currBatch.BatchNumber = 1;
                    currBatch.ExportTableName = exportTableName;
                    currBatch.LastExportDate = DateTime.Now;
                   // batchList.Add(currBatch);
                    MemDB.Add(currBatch);
                }
                else
                {
                    currBatch.BatchNumber += 1;
                    currBatch.LastExportDate = DateTime.Now;
                }
              
                MemDB.Save<ExportBatchInfo>();
                //导出数据
                OQL q = funQ(currBatch.BatchNumber, new T());
                List<T> entityList = q != null ? CurrDbContext.QueryList<T>(q) : CurrDbContext.QueryAllList<T>();
                ExportEntityEventArgs args = new ExportEntityEventArgs(entityList, entityType, exportTableName);
                args.Succeed = true;
                args.OperationExcepiton = null;
                args.BatchNumber = currBatch.BatchNumber;

                if (OnExported != null)
                    OnExported(this, args);
                if(!args.Cancel)
                    SaveEntity<T>(entityList.ToArray(), args);
            }
            catch (Exception ex)
            {
                ExportEntityEventArgs args = new ExportEntityEventArgs(null, entityType, EntityFieldsCache.Item(entityType).TableName);
                args.Succeed = false;
                args.OperationExcepiton = ex;

                if (OnExported != null)
                    OnExported(this, args);
            }
        }



        /*
        /// <summary>
        /// 根据实体类对象，导出实体数据
        /// </summary>
        /// <param name="entity">参照的实体类对象</param>
        public void ExportData(EntityBase entity)
        {
            Type entityType = entity.GetType();
            MethodInfo method  = GetType().GetMethod("Export", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo genericMethod= method.MakeGenericMethod(entityType);
            Action action = (Action)System.Delegate.CreateDelegate(typeof(Action), this, genericMethod);
            action();
        }
         * 
         */ 
    }
}
