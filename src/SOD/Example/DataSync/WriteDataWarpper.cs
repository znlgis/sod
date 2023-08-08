using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using PWMIS.Core.Extensions;
using PWMIS.DataMap.Entity;
using SOD.DataSync.Entitys;

namespace SOD.DataSync
{
    /// <summary>
    ///     数据写操作包装器工厂，系统如果需要同步的数据，在增删改之前必须使用本类进行包装。
    /// </summary>
    /// <example>
    ///     <![CDATA[
    /// DbContext dbContext =new LocalDbContext();
    /// using(IWriteDataWarpper warpper = WriteDataWarpperFactory.Create(dbContext))
    /// {
    ///     dbContext.Add<UserEntity>(user);
    ///     dbContext.Remove<RoleEntity)(role);
    /// }
    /// ]]>
    /// </example>
    public class WriteDataWarpperFactory
    {
        /// <summary>
        ///     创建一个导出数据使用的写操作包装器
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IWriteDataWarpper Create(DbContext context)
        {
            return new ExportDataWarpper(context);
        }
    }

    /// <summary>
    ///     数据写操作上下文，系统如果需要同步的数据，在增删改之前必须使用本类进行包装。
    /// </summary>
    public class ExportDataWarpper : IWriteDataWarpper
    {
        private DbContext OptDbContext;

        /// <summary>
        ///     以一个数据操作上下文对象初始化本类
        /// </summary>
        /// <param name="context"></param>
        protected internal ExportDataWarpper(DbContext context)
        {
            OptDbContext = context;
            OptDbContext.OnBeforeExecute += CurrentContext_OnBeforeExecute;
            OptDbContext.OnAfterExecute += CurrentContext_OnAfterExecute;
            OptDbContext.CheckTableExists<DeletedPKIDEntity>();
        }

        /// <summary>
        ///     释放资源，取消事件挂钩
        /// </summary>
        public void Dispose()
        {
            OptDbContext.OnBeforeExecute -= CurrentContext_OnBeforeExecute;
            OptDbContext.OnAfterExecute -= CurrentContext_OnAfterExecute;
            OptDbContext = null;
        }

        /// <summary>
        ///     快速插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public void QuickInsert<T>(List<T> list) where T : EntityBase, new()
        {
            if (list[0] is IExportTable)
                foreach (var entity in list)
                    ((IExportTable)entity).BatchNumber = 0;
            Task.Run(() =>
            {
                var db = OptDbContext.CurrentDataBase;
                var eq = new EntityQuery<T>(db);
                var count = eq.QuickInsert(list);
                Debug.WriteLine("ExportDataWarpper QuickInsert count=" + count);
            });
        }

        /// <summary>
        ///     快速删除数据，内部根据实体类的主键进行删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public void QuickDelete<T>(List<T> list) where T : EntityBase, new()
        {
            var idList = new List<object>();
            var entity0 = list[0];
            var tableName = entity0.GetTableName();
            var pkName = entity0.PrimaryKeys[0];


            foreach (var entity in list)
                idList.Add(entity[pkName]);

            var dteList = new List<DeletedPKIDEntity>();
            foreach (var entity in list)
            {
                var pkValue = entity[pkName];
                var idEntity = new DeletedPKIDEntity();
                if (pkValue is string)
                {
                    idEntity.TargetID = 0;
                    idEntity.TargetStringID = pkValue.ToString();
                }
                else
                {
                    idEntity.TargetID = Convert.ToInt64(pkValue);
                    idEntity.TargetStringID = "";
                }

                idEntity.TargetTableName = tableName;
                idEntity.DeletedTime = DateTime.Now;

                dteList.Add(idEntity);
            }

            var deleteQ = OQL.From(entity0)
                .Delete()
                .Where(cmp => cmp.Comparer(entity0[pkName], OQLCompare.CompareType.IN, idList.ToArray()))
                .END;

            var db = OptDbContext.CurrentDataBase;
            var eq = new EntityQuery<DeletedPKIDEntity>(db);
            db.BeginTransaction();
            try
            {
                var delCount = eq.ExecuteOql(deleteQ);
                var insCount = eq.QuickInsert(dteList);
                if (delCount != insCount)
                    throw new Exception("插入和删除的数据记录数不一样，删除数：" + delCount + ",插入数：" + insCount);
                db.Commit();
            }
            catch (Exception ex)
            {
                db.Rollback();
            }
        }

        private void CurrentContext_OnAfterExecute(object sender, EntityQueryExecuteEventArgs e)
        {
            if (e.ExecuteType == EntityQueryExecuteType.Delete)
            {
                //保存删除的ID信息 到指定的表
                var pkName = e.Entity.PrimaryKeys[0];
                var pkValue = e.Entity[pkName];


                var idEntity = new DeletedPKIDEntity();
                if (pkValue is string)
                {
                    idEntity.TargetID = 0;
                    idEntity.TargetStringID = pkValue.ToString();
                }
                else
                {
                    idEntity.TargetID = Convert.ToInt64(pkValue);
                    idEntity.TargetStringID = "";
                }

                idEntity.TargetTableName = e.Entity.GetTableName();
                idEntity.DeletedTime = DateTime.Now;

                OptDbContext.Add(idEntity);
                OptDbContext.CurrentDataBase.Commit();
            }
        }

        private void CurrentContext_OnBeforeExecute(object sender, EntityQueryExecuteEventArgs e)
        {
            var exportTable = e.Entity as IExportTable;
            if (exportTable != null) exportTable.BatchNumber = 0;
            if (e.ExecuteType == EntityQueryExecuteType.Delete) OptDbContext.CurrentDataBase.BeginTransaction();
        }
    }

    /// <summary>
    ///     通过拦截写数据库的事件来进行写操作扩展的包装器
    /// </summary>
    public class WriteDataEventWarpper : IWriteDataWarpper
    {
        private DbContext OptDbContext;

        protected internal WriteDataEventWarpper(DbContext context)
        {
            OptDbContext = context;
            OptDbContext.OnBeforeExecute += OptDbContext_OnBeforeExecute;
            OptDbContext.OnAfterExecute += OptDbContext_OnAfterExecute;
            OptDbContext.CheckTableExists<DeletedPKIDEntity>();
        }

        /// <summary>
        ///     释放资源，取消事件挂钩
        /// </summary>
        public void Dispose()
        {
            OptDbContext.OnBeforeExecute -= OptDbContext_OnBeforeExecute;
            OptDbContext.OnAfterExecute -= OptDbContext_OnAfterExecute;
            OptDbContext = null;
        }

        public void QuickInsert<T>(List<T> list) where T : EntityBase, new()
        {
            throw new NotImplementedException();
        }

        public void QuickDelete<T>(List<T> list) where T : EntityBase, new()
        {
            throw new NotImplementedException();
        }

        private void OptDbContext_OnAfterExecute(object sender, EntityQueryExecuteEventArgs e)
        {
            throw new NotSupportedException();
        }


        private void OptDbContext_OnBeforeExecute(object sender, EntityQueryExecuteEventArgs e)
        {
            throw new NotSupportedException();
        }
    }
}