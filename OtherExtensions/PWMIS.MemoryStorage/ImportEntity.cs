
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

    public class ImportEntityEventArgs : EventArgs
    {
        public System.Collections.IList DataList { get; private set; }

        public Type EntityType { get; private set; }

        public string ImportTable { get; private set; }
        /// <summary>
        /// 是否撤销导入
        /// </summary>
        public bool Cancel { get; set; }

        public ImportEntityEventArgs(System.Collections.IList list, Type entityType, string table)
        {
            this.DataList = list;
            this.EntityType = entityType;
            this.ImportTable = table;
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
        }

        /// <summary>
        /// 导入数据到关系数据库
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <returns>导入的数据数量</returns>
        public int Import<T>() where T : EntityBase, new()
        {
            List<T> list = this.MemDB.Get<T>();
            if (list.Count > 0)
            {
                EntityBase entity = (EntityBase)list[0];
                ImportEntityEventArgs args=  new ImportEntityEventArgs(list, typeof(T), entity.GetTableName());
                if (BeforeImport != null)
                {
                    BeforeImport(this,args);
                    if (args.Cancel)
                        return -1;
                }
                int count = this.CurrDbContext.AddList(list);
                //Console.WriteLine("导入数据 {0}成功！数量：{1}", typeof(T).Name, count);
                if (AfterImport != null)
                {
                    args.Cancel = false;
                    AfterImport(this,args);
                }
                return count;
            }
            else
                return 0;
        }

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
    }
}
