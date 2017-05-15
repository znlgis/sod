
using PWMIS.Core.Extensions;
using PWMIS.DataMap.Entity;
using PWMIS.MemoryStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    public class ExportEntityEventArgs : EventArgs
    {
        /// <summary>
        /// 导出的数据列表
        /// </summary>
        public System.Collections.IList ExportedDataList { get; private set; }

        public Type EntityType { get; private set; }

        public string ExportTable { get; private set; }
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
    class ExportEntity
    {
        MemDB MemDB;
        DbContext CurrDbContext;

        /// <summary>
        /// 导出前事件，此时没有数据
        /// </summary>
        public event EventHandler<ExportEntityEventArgs> BeforeImport;
        /// <summary>
        /// 导出后事件
        /// </summary>
        public event EventHandler<ExportEntityEventArgs> AfterImport;

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

        private void SaveEntity<T>(T[] entitys) where T : EntityBase, new()
        {
            bool flag = MemDB.SaveEntity<T>(entitys);
            if (flag)
                Console.WriteLine("导出数据 {0}成功！数量：{1}",typeof(T).Name,entitys.Length);
        }

        /// <summary>
        /// 导出实体数据
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        public void Export<T>() where T : EntityBase, new()
        {

            List<T> entityList = CurrDbContext.QueryAllList<T>();
            SaveEntity<T>( entityList.ToArray());
        }

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
    }
}
