using Sod.Pwmis.com.Entity;
using Sod.Pwmis.com.Interface;
using Sod.Pwmis.com.Interface.Infrastructure;
using Sod.Pwmis.com.Repository.Entities;
using PWMIS.Core.Extensions;
using PWMIS.DataMap.Entity;
using System;

namespace Sod.Pwmis.com.Repository
{
    public class NetWorkDbContext : UowDbContext
    {
        public NetWorkDbContext(IGMT? gmtServer=null):base("NetWorkDb",gmtServer)
        {
             
        }

        protected override bool CheckAllTableExists()
        {
            InitializeTable<DbVersion>("INSERT INTO [{0}]([Version],[BizName],[UpdateTime]) VALUES(5,'网管数据库','2024-07-19')");
            CheckTableExists<SodDemoEntity>();
            UpdateDatabase();
            return true;
        }

        private void UpdateDatabase()
        {
            //ignore :是否忽略更新表结构语句执行的错误，如果不忽略更新数据库失败将抛出异常终止进程继续执行。
#if DEBUG
            bool ignore =false;//ignore=false 适用于开发过程中修改表结构，生产环境请设置为 true
#else
            bool ignore=true;
#endif
            Console.WriteLine();
            Console.WriteLine("** Update Database Ignore Error:{0} **",ignore);
            //UpdateDatabaseSql(1, "ALTER TABLE [NetworkNode] ADD ([Status] int,[Remark] varchar(500))", ignore);
            //UpdateDatabaseSql(3, "ALTER TABLE [Equipment] ADD ([NodeID] int)", ignore);
            //UpdateDatabaseSql(4, "ALTER TABLE [GroupingRelation] ADD ([RelationType] int)", ignore);
            //UpdateDatabaseSql(5, "ALTER TABLE [Equipment] ADD ([InGroupCount] int)", ignore);
            //UpdateDatabaseSql(6, "ALTER TABLE [Equipment] ADD ([szUacCode] varchar(32),[bLocal] int)", ignore);
            //UpdateDatabaseSql(7, "ALTER TABLE [HostAssetRecord] ADD ([DeviceType] varchar(32))", ignore);

        }

        /// <summary>
        /// 执行SQL更新数据库
        /// </summary>
        /// <param name="version">数据版本号</param>
        /// <param name="sql"></param>
        /// <param name="ignore">是否忽略执行更新时候的错误</param>
        private void UpdateDatabaseSql(int version, string sql, bool ignore = true)
        {
            DbVersion dbVer = OQL.From<DbVersion>().ToObject(this.CurrentDataBase);
            if (dbVer.Version < version)
            {
                try
                {
                    Console.WriteLine();
                    Console.WriteLine("Current Database Version {0},Need update to Version {1} ... Please wait!",
                        dbVer.Version, version);
                    this.CurrentDataBase.ExecuteNonQuery(sql);
                    this.CurrentDataBase.Logger.WriteLog("Successfully executed the Database Update Sql:" + sql,
                        "CMDbContext.UpdateDatabaseSql");
                }
                catch (Exception ex)
                {
                    if (!ignore)
                        throw;
                    else
                        Console.WriteLine("Error on executed the Database Update Sql:{0},Error Message:{1}", sql, ex.Message);
                    return;
                }

                dbVer.Version = version;
                dbVer.UpdateTime = DateTime.Now;
                OQL q = OQL.From(dbVer)
                    .Update(dbVer.Version, dbVer.UpdateTime)                     //SOD 安全更新：
                    .Where(cmp => cmp.Comparer(dbVer.Version, "<", version))     //SOD更新实体类要求实体类有主键，
                    .END;                                                        //如无主键，OQL要求必须有Where条件
                int count= EntityQuery<DbVersion>.ExecuteOql(q, this.CurrentDataBase);
                if(count > 0)
                {
                    this.CurrentDataBase.Logger.WriteLog("Successfully Update the Database version:" + version,
                  "CMDbContext.UpdateDatabaseSql");
                }
            }
        }
        /// <summary>
        /// 获取数据库的数据版本
        /// </summary>
        /// <returns></returns>
        public int GetDataVersion()
        {
            DbVersion dbVer = OQL.FromObject<DbVersion>().ToObject(this.CurrentDataBase);
            return dbVer.Version;
        }

    }
}
