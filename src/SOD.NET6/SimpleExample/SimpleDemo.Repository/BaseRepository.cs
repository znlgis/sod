using PWMIS.Core.Extensions;
using PWMIS.DataMap.Entity;
using SimpleDemo.Interface;
using SimpleDemo.Interface.Infrastructure;

namespace SimpleDemo.Repository
{
    public class BaseRepository<Tid, TParent, TEntity> : IRepository<TParent, Tid>
        where TParent : IIdentityProperty<Tid>
        where TEntity : EntityBase, TParent, new()
    {
        private  DbContext _dbContext;

        public Guid ID { get; private set; }

        public IUowManager UnitOfWorkManager =>  (IUowManager)_dbContext;
        public DbContext DbContext => _dbContext;

        public BaseRepository(IUowManager dbContext)
        {
            var context = dbContext as DbContext;
            if (context != null)
            {
                _dbContext = context;
            }
            else
            {
                _dbContext = new SimpleDbContext();
            }
            ID = Guid.NewGuid();
        }

        public void Delete(TParent data)
        {
            //TData必然是TEntity的接口，但data可能并不是实体类
            var entity = data as TEntity; 
            if (entity == null)
            { 
                entity = data.CopyTo<TEntity>();
            }
            _dbContext.Remove(entity);
        }

        public IEnumerable<TParent> GetAll()
        {
            var list= _dbContext.QueryAllList<TEntity>();
            //var result = list.ConvertAll<TParent>(entity => entity);
            return list;
        }

        public TParent? GetById(Tid id)
        {
            TEntity entity = new TEntity();
            entity.ID = id;
            var q2 = OQL.FromObject<TEntity>()
                .Select()
                .Where((cmp, e) => cmp.EqualValue(entity.ID))
                .END;
            TEntity dbEntity= q2.ToObject(_dbContext.CurrentDataBase);
            return dbEntity;
            
            /*
            //假设id 是主键的值
            string pkName = entity.PrimaryKeys.First();
            entity[pkName] = id;  //等价于 entity.ID=id;
            //方式一：OQL
            var q = OQL.From(entity)
                .Select()
                .Where(entity[pkName])
                .END;
            return _dbContext.QueryObject<TEntity>(q);
            */

            /*
            //方式二：GOQL
            //  不需要先执行 entity[pkName] = id;
            var q2= OQL.FromObject<TEntity>()
                .Select()
                .Where((cmp, e) => cmp.Property(entity[pkName])==id)
                .END;
            return q2.ToObject(_dbContext.CurrentDataBase);
            */

            /*
            //方式三：EntytyQuery<T>
            EntityQuery<TEntity> query = new EntityQuery<TEntity>(_dbContext.CurrentDataBase);
            query.FillEntity(entity);
            return entity;
            */
        }

        public void Insert(TParent data)
        {
            var entity = data as TEntity;
            if (entity == null)
            {
                entity = new TEntity();
                entity.MapFrom(data,true);
                //使用 CopyTo 方法会设置实体类所有属性的修改状态，这会导致不正确的实体类映射表字段的默认值，
                //在表字段部分写入值的时候出现问题，故不建议这样做，应该使用实体类的 MapFrom 方法并设置修改属性。
                //entity = data.CopyTo<TEntity>();
            }
            _dbContext.Add(entity);
        }

        public void Update(TParent data)
        {
            var entity = data as TEntity;
            if (entity == null)
            {
                entity = new TEntity();
                entity.MapFrom(data, true);
                //使用 CopyTo 方法会设置实体类所有属性的修改状态，这会导致不正确的实体类映射表字段的默认值，
                //在表字段部分更新的时候出现问题，故不建议这样做，应该使用实体类的 MapFrom 方法并设置修改属性。
                //entity = data.CopyTo<TEntity>();
            }
            _dbContext.Update(entity);
        }

    }
}
