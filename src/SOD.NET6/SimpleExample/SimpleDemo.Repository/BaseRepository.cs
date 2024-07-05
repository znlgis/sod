using PWMIS.Core.Extensions;
using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Data;
using SimpleDemo.Interface;
using SimpleDemo.Interface.Infrastructure;
using SimpleDemo.Model;

namespace SimpleDemo.Repository
{
    public class BaseRepository<Tid, TParent, TEntity> : IRepository<TParent, Tid>
       where TParent : class, IIdentityProperty<Tid>
       where TEntity : EntityBase, TParent, new()
    {
        private DbContext _dbContext;

        public Guid ID { get; private set; }

        public IUowManager UnitOfWorkManager => (IUowManager)_dbContext;
        public DbContext DbContext => _dbContext;
        private string _tableName;
        public string QueryTableName
        {
            get
            {
                return _tableName;
            }
            set
            {
                _tableName = value;
            }
        }

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

        /// <summary>
        /// 构造OQL条件
        /// </summary>
        /// <param name="cmp"></param>
        /// <param name="e">实体类对象</param>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public OQLCompare GetOQLCompare(OQLCompare cmp, TEntity e, List<KeyValuePair<string, string>> keyValuePairs)
        {
            OQLCompare cmpResult = null;
            TEntity temp = new TEntity();
            var fieldsInfo = EntityFieldsCache.Item(temp.GetType());
            foreach (KeyValuePair<string, string> kv in keyValuePairs)
            {
                string fieldName = fieldsInfo.GetPropertyField(kv.Key);
                if (fieldName != null)
                {
                    string value = kv.Value;
                    if (kv.Value.StartsWith(">=")) //field1 >1
                    {
                        value = kv.Value.TrimStart('>', '=');
                    }
                    else if (kv.Value.StartsWith(">")) //field1 >=1
                    {
                        value = kv.Value.TrimStart('>');
                    }
                    else if (kv.Value.StartsWith("<="))  //field1 <1
                    {
                        value = kv.Value.TrimStart('<', '=');
                    }
                    else if (kv.Value.StartsWith("<"))  //field1 <=1
                    {
                        value = kv.Value.TrimStart('<');
                    }
                    else if (kv.Value.StartsWith("|"))  //field1 in (1,2,3) ?
                    {
                        value = kv.Value.TrimStart('|');
                    }
                    else if (kv.Value.StartsWith("="))
                    {
                        value = kv.Value.TrimStart('=');
                    }
                    else if (kv.Value.StartsWith("!="))
                    {
                        value = kv.Value.TrimStart('!', '=');
                    }

                    Type type = fieldsInfo.GetPropertyType(fieldName);
                    if (type.IsEnum)
                    {
                        temp[kv.Key] = Enum.Parse(type, value);
                    }
                    else if (type == typeof(DateTime))
                    {
                        //value format="yyyyMMddHHmmss"
                        DateTime date = CommonUtil.ParseDateTimeString(value);
                        temp[kv.Key] = date;
                    }
                    else
                    {
                        temp[kv.Key] = value;
                    }

                    if (kv.Value.StartsWith(">=")) //field1 >1
                    {
                        cmpResult = cmpResult & cmp.Comparer(e[kv.Key], ">=", temp[kv.Key]);
                    }
                    else if (kv.Value.StartsWith(">")) //field1 >=1
                    {
                        cmpResult = cmpResult & cmp.Comparer(e[kv.Key], ">", temp[kv.Key]);
                    }
                    else if (kv.Value.StartsWith("<="))  //field1 <1
                    {
                        cmpResult = cmpResult & cmp.Comparer(e[kv.Key], "<=", temp[kv.Key]);
                    }
                    else if (kv.Value.StartsWith("<"))  //field1 <=1
                    {
                        cmpResult = cmpResult & cmp.Comparer(e[kv.Key], "<", temp[kv.Key]);
                    }
                    else if (kv.Value.StartsWith("|"))  //field1 in (1,2,3)
                    {
                        cmpResult = cmpResult & cmp.Comparer(e[kv.Key], "in", temp[kv.Key]); //?
                    }
                    else if (kv.Value.StartsWith("IN[") && kv.Value.EndsWith("]"))  //field1 IN[1,2,3]
                    {
                        string tempStr = kv.Value.Substring(3, kv.Value.Length - 4);
                        if (type == typeof(string))
                        {
                            string[] cmpValue = tempStr.Split(',');
                            cmpResult = cmpResult & cmp.Comparer(e[kv.Key], "in", cmpValue);
                        }
                        else
                        {
                            string[] strValues = tempStr.Split(',');
                            List<int> cmpValues = new List<int>();
                            foreach (string item in strValues)
                            {
                                if (int.TryParse(item, out int i))
                                    cmpValues.Add(i);
                            }
                            if (cmpValues.Count > 0)
                            {
                                cmpResult = cmpResult & cmp.Comparer(e[kv.Key], "in", cmpValues);
                            }
                        }
                        //cmpResult = cmpResult & cmp.Comparer(e[kv.Key], "<", temp[kv.Key]);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(value))
                        {
                            if (kv.Value.StartsWith("="))
                                cmpResult = cmpResult & cmp.IsNull(e[kv.Key]);
                            else if (kv.Value.StartsWith("!="))
                                cmpResult = cmpResult & cmp.IsNotNull(e[kv.Key]);
                            else
                                cmpResult = cmpResult & cmp.IsNull(e[kv.Key]);
                        }
                        else if (value.StartsWith("%") || value.EndsWith("%"))
                        {
                            if (value.Contains('_')) //like 特殊字符
                                value = value.Replace("_", "\\_");
                            //可以直接支持 $
                            //if (value.Contains('$')) //like 特殊字符
                            //    value = value.Replace("$", "\\$");
                            //[] SqlServer 支持
                            //if (value.Contains('[')) //like 特殊字符
                            //    value = value.Replace("[", "[[]");
                            //if (value.Contains(']')) //like 特殊字符
                            //    value = value.Replace("[", "[]]");

                            if (kv.Value.StartsWith("="))
                                cmpResult = cmpResult & cmp.Comparer(e[kv.Key], "like", value);
                            else if (kv.Value.StartsWith("!="))
                                cmpResult = cmpResult & cmp.Comparer(e[kv.Key], "not like", value);
                            else
                                cmpResult = cmpResult & cmp.Comparer(e[kv.Key], "like", value);
                        }
                        else
                        {
                            if (kv.Value.StartsWith("="))
                                cmpResult = cmpResult & cmp.Property(e[kv.Key]) == temp[kv.Key];
                            if (kv.Value.StartsWith("!="))
                                cmpResult = cmpResult & cmp.Property(e[kv.Key]) != temp[kv.Key];
                            else
                                cmpResult = cmpResult & cmp.Property(e[kv.Key]) == temp[kv.Key];
                        }

                    }

                }
            }
            return cmpResult;
        }


        public TObj ConvertTo<TObj>(TParent data) where TObj : class, new()
        {
            return data.CopyTo<TObj>();
        }

        public int Delete(Tid id)
        {
            //TData必然是TEntity的接口，但data可能并不是实体类
            TEntity entity = new TEntity();
            entity.ID = id;
            return _dbContext.Remove(entity);
        }

        public IEnumerable<TParent> GetAll()
        {
            var list = _dbContext.QueryAllList<TEntity>();
            return list;
        }

        public TParent? GetById(Tid id)
        {
            TEntity entity = new TEntity();
            entity.ID = id;
            var q = OQL.From(entity)
                .Select()
                .Where(entity.ID)
                .END;
            TEntity dbEntity = _dbContext.QueryObject<TEntity>(q);
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

        public int Insert(TParent data)
        {
            var entity = data as TEntity;
            if (entity == null)
            {
                entity = new TEntity();
                entity.MapFrom(data, true);
                //使用 CopyTo 方法会设置实体类所有属性的修改状态，这会导致不正确的实体类映射表字段的默认值，
                //在表字段部分写入值的时候出现问题，故不建议这样做，应该使用实体类的 MapFrom 方法并设置修改属性。
                //entity = data.CopyTo<TEntity>();
            }
            int count = _dbContext.Add(entity);
            data.ID = entity.ID;//获取自增的ID值
            return count;
        }

        public int Update(TParent data)
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
            return _dbContext.Update(entity);
        }

        public PageResult<TParent> GetPageList(int pageSize, int pageNum, int total, List<KeyValuePair<string, string>> keyValuePairs, string[] orderStrings = null)
        {
            if (orderStrings == null)
                orderStrings = new string[] { "ID desc" };
            if (total <= 0) total = 0;
            var pageResult = new PageResult<TParent>();
            AdoHelper db = _dbContext.CurrentDataBase;
            db.CommandTimeOut = 60;

            if (keyValuePairs?.Count > 0)
            {
                OQLCompareFunc<TEntity> cmpFun = (cmp, e) => GetOQLCompare(cmp, e, keyValuePairs);
                TEntity entity = new TEntity();
                //分表
                if (!string.IsNullOrEmpty(this._tableName))
                {
                    entity.MapNewTableName(this._tableName);
                }
                var oql = OQL.From(entity)
                    .Select()
                    .Where(cmpFun)
                    .OrderBy(orderStrings)
                    .END;
                oql.Limit(pageSize, pageNum);
                oql.PageWithAllRecordCount = total;//为0将重新查询记录数
                pageResult.Items = EntityQuery<TEntity>.QueryList(oql, db);
                pageResult.Total = oql.PageWithAllRecordCount;
            }
            else
            {
                var goql = OQL.From<TEntity>()
                          .Select()
                          .OrderBy((o, e) => o.Desc(e["ID"]))
                          .Limit(pageSize, pageNum, total);
                pageResult.Items = goql.ToList(db);
                pageResult.Total = goql.AllCount;
            }
            return pageResult;
        }

    }
}
