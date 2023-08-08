using System.Collections.Generic;
using PWMIS.DataProvider.Data;

namespace PWMIS.DataMap.Entity
{
    /// <summary>
    ///     实体类查询接口
    /// </summary>
    public interface IEntityQuery
    {
        AdoHelper DefaultDataBase { get; set; }
        int Delete(EntityBase entity);
        int Delete(List<EntityBase> entityList);
        int Delete(EntityBase entity, CommonDB db);
        int ExecuteOql(OQL oql);
        bool ExistsEntity(EntityBase entity);

        bool FillEntity(EntityBase entity);

        //System.Collections.Generic.List<EntityBase> FillEntityList(PWMIS.DataMap.Entity.OQL oql, EntityBase entity);
        //string FillParameter(PWMIS.DataMap.Entity.OQL oql, EntityBase entity, out System.Data.IDataParameter[] paras);
        int Insert(List<EntityBase> entityList);
        int Insert(EntityBase entity);
        int Insert(EntityBase entity, CommonDB db);
        int Save(CommonDB db, params object[] fields);
        int Save(params object[] fields);
        int SaveAllChanges();
        int SaveAllChanges(CommonDB db);
        int Update(List<EntityBase> entityList);
        int Update(EntityBase entity, CommonDB db);
        int Update(EntityBase entity);
    }
}