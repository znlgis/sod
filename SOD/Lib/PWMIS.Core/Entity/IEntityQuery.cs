using System;
namespace PWMIS.DataMap.Entity
{
    /// <summary>
    /// 实体类查询接口
    /// </summary>
    public interface IEntityQuery
    {
        PWMIS.DataProvider.Data.AdoHelper DefaultDataBase { get; set; }
        int Delete(EntityBase entity);
        int Delete(System.Collections.Generic.List<EntityBase> entityList);
        int Delete(EntityBase entity, PWMIS.DataProvider.Data.CommonDB db);
        int ExecuteOql(PWMIS.DataMap.Entity.OQL oql);
        bool ExistsEntity(EntityBase entity);
        bool FillEntity(EntityBase entity);
        //System.Collections.Generic.List<EntityBase> FillEntityList(PWMIS.DataMap.Entity.OQL oql, EntityBase entity);
        //string FillParameter(PWMIS.DataMap.Entity.OQL oql, EntityBase entity, out System.Data.IDataParameter[] paras);
        int Insert(System.Collections.Generic.List<EntityBase> entityList);
        int Insert(EntityBase entity);
        int Insert(EntityBase entity, PWMIS.DataProvider.Data.CommonDB db);
        int Save(PWMIS.DataProvider.Data.CommonDB db, params object[] fields);
        int Save(params object[] fields);
        int SaveAllChanges();
        int SaveAllChanges(PWMIS.DataProvider.Data.CommonDB db);
        int Update(System.Collections.Generic.List<EntityBase> entityList);
        int Update(EntityBase entity, PWMIS.DataProvider.Data.CommonDB db);
        int Update(EntityBase entity);
    }
}
