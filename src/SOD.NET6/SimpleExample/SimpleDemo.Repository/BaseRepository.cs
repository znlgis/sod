using PWMIS.Core.Extensions;
using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Data;
using SimpleDemo.Interface;
using SimpleDemo.Interface.Infrastructure;
using SimpleDemo.Interface.IRepositories;
using SimpleDemo.Model;

namespace SimpleDemo.Repository
{
    public class BaseRepository<Tid, TParent, TEntity> : IRepository<TParent, Tid>
       where TParent : class, IIdentityProperty<Tid>
       where TEntity : EntityBase, TParent, new()
    {
        private DbContext _dbContext;
        private string _tableName = string.Empty;
        private IUowManager? _uowManager;

        public Guid ID { get; private set; }
        public IUowManager UnitOfWorkManager
        {
            get
            {
                if (_uowManager == null)
                {
                    var uowm = _dbContext as IUowManager;
                    if (uowm == null)
                    {
                        uowm = new SimpleUowManager(_dbContext);
                    }
                    _uowManager = uowm;
                }
                return _uowManager;
            }
        }
        public DbContext DbContext => _dbContext;
        public string QueryTableName => _tableName;


        public BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            ID = Guid.NewGuid();
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

        #region 内部快捷查询方法
        /// <summary>
        /// 创建一个新的默认的实体查询对象
        /// </summary>
        /// <returns></returns>
        protected EntityQuery<TEntity> CreateDefaultQuery()
        {
            return new EntityQuery<TEntity>(this.DbContext.CurrentDataBase);
        }
        /// <summary>
        /// 创建一个新的指定类型的实体查询对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected EntityQuery<T> CreateQuery<T>(T? entity = null) where T : EntityBase, new()
        {
            var query = new EntityQuery<T>(this.DbContext.CurrentDataBase);
            if (entity != null)
                query.CurrentEntity = entity;
            return query;
        }
        /// <summary>
        /// 创建一个单表或者多表查询的结果映射对象（查询结果实体容器）
        /// </summary>
        /// <param name="q">OQL查询对象</param>
        /// <returns>实体容器</returns>
        protected EntityContainer CreateQueryMapper(OQL q)
        {
            return new EntityContainer(q, this.DbContext.CurrentDataBase);
        }

        /// <summary>
        /// 将OQL查询结果映射到一个新的类型列表数据中
        /// </summary>
        /// <typeparam name="T">列表数据类型</typeparam>
        /// <param name="q">OQL查询表达式</param>
        /// <param name="action">对结果进行映射的方法</param>
        /// <returns></returns>
        protected IList<T> MapResultToList<T>(OQL q, Action<T> action) where T : class, new()
        {
            var container = this.CreateQueryMapper(q);
            var result = container.MapToList<T>(() => {
                T item = new T();
                action(item);
                return item;
            });
            return result;
        }

        /// <summary>
        /// 执行OQL查询
        /// </summary>
        /// <param name="q">OQL查询对象</param>
        /// <returns>操作受影响的记录行数</returns>
        protected int ExecuteQuery(OQL q)
        {
            return EntityQuery.ExecuteOql(q, this.DbContext.CurrentDataBase);
        }

        /// <summary>
        /// 执行GOQL查询以获取结果列表
        /// </summary>
        /// <param name="q">GOQL查询对象</param>
        /// <returns>实体对象列表</returns>
        protected List<T> QueryList<T>(GOQL<T> goql) where T : EntityBase, new()
        {
            return goql.ToList(this.DbContext.CurrentDataBase);
        }

        /// <summary>
        /// 使用一个OQL泛型委托参数进行查询获取列表数据
        /// </summary>
        /// <typeparam name="T">要查询的实体类类型</typeparam>
        /// <param name="goqlFun">OQL泛型委托方法</param>
        /// <returns>类型为T的列表结果</returns>
        protected List<T> QueryList<T>(Action<GOQL<T>> goqlFun) where T : EntityBase, new()
        {
            var goql = OQL.From<T>();
            goqlFun(goql);
            return goql.ToList(this.DbContext.CurrentDataBase);
        }

        /// <summary>
        /// 执行GOQL查询并将结果映射到POCO类型
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <typeparam name="TObj">POCO类型</typeparam>
        /// <param name="goql">GOQL查询对象</param>
        /// <returns>POCO类型列表</returns>
        protected List<TObj> QueryPocoList<T, TObj>(GOQL<T> goql)
            where T : EntityBase, new()
            where TObj : class, new()
        {
            //如果目标对象的属性与源对象的属性类型不一致，下面的方法会失败，例如数据库字段是long类型，但DTO的属性是int类型。
            //故采用ConvertToList 进行类型相容性转换，下面一行代码注释。
            //return goql.MapToPOCOList<TObj>(this.DbContext.CurrentDataBase);
            var list = goql.ToList(this.DbContext.CurrentDataBase);
            return ConvertToList<T, TObj>(list);
        }

        /// <summary>
        /// 执行OQL查询以获取结果列表
        /// </summary>
        /// <param name="q">OQL查询对象</param>
        /// <returns>实体对象列表</returns>
        protected List<T> QueryList<T>(OQL q) where T : EntityBase, new()
        {
            return EntityQuery<T>.QueryList(q, this.DbContext.CurrentDataBase);
        }

        /// <summary>
        /// 使用OQL表达式进行查询
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// var list= this.QueryList<UserEntity>(u=>OQL.From(u).Select(u.ID,u.FirstName).OrderBy(u.id).End))
        /// ]]>
        /// </example>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="fun">返回OQL表达式的方法</param>
        /// <returns></returns>
        protected List<T> QueryList<T>(Func<T, OQL> fun) where T : EntityBase, new()
        {
            T entity = new T();
            var q = fun(entity);
            return EntityQuery<T>.QueryList(q, this.DbContext.CurrentDataBase);
        }

        /// <summary>
        /// 使用对实体类初始化后的OQL表达式进行查询
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// var list=this.QueryList<UserEntity>(
        ///   u=>{u.FirstName="zhangsan"},
        ///   u=>OQL.From(u).Select().Where(u.FirstName).End
        ///   );
        /// ]]>
        /// </example>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="actionInit">对实体类初始化的方法</param>
        /// <param name="funOql">返回OQL表达式的方法</param>
        /// <returns></returns>
        protected List<T> QueryList<T>(Action<T> actionInit, Func<T, OQL> funOql) where T : EntityBase, new()
        {
            T entity = new T();
            actionInit(entity);
            var q = funOql(entity);
            return EntityQuery<T>.QueryList(q, this.DbContext.CurrentDataBase);
        }

        protected TObj EntityToPOCO<T, TObj>(T entity)
            where T : EntityBase, new()
            where TObj : class, new()
        {
            try
            {
                TObj obj = new TObj();
                entity.MapToPOCO(obj);//如果属性字段值是long类型，属性是int类型，MapToPOCO 方法会失败。
                return obj;
            }
            catch
            {
                TObj obj = entity.CopyTo<TObj>();
                return obj;
            }
        }

        /// <summary>
        /// 执行GOQL查询一个对象数据并将结果数据映射到一个POCO对象中
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <typeparam name="TObj">POCO对象类型</typeparam>
        /// <param name="goql">GOQL表达式</param>
        /// <returns></returns>
        protected TObj? QueryPocoObject<T, TObj>(GOQL<T> goql)
            where T : EntityBase, new()
            where TObj : class, new()
        {
            var entity = goql.ToObject(this.DbContext.CurrentDataBase);
            if (entity != null)
            {
                TObj obj = EntityToPOCO<T, TObj>(entity);
                return obj;
            }
            return null;
        }

        /// <summary>
        /// 执行GOQL查询一个对象
        /// </summary>
        /// <param name="goql"></param>
        /// <returns></returns>
        protected TParent? QueryObject(GOQL<TEntity> goql)
        {
            var entity = goql.ToObject(this.DbContext.CurrentDataBase);
            return entity;
        }

        protected T QueryObject<T>(OQL q) where T : EntityBase, new()
        {
            return EntityQuery<T>.QueryObject(q, this.DbContext.CurrentDataBase);
        }
        #endregion

        /// <summary>
        /// 构造OQL条件
        /// </summary>
        /// <param name="cmp"></param>
        /// <param name="e">实体类对象</param>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public OQLCompare? GetOQLCompare(OQLCompare cmp, TEntity e, List<KeyValuePair<string, string>> keyValuePairs)
        {
            OQLCompare? cmpResult = null;
            TEntity temp = new TEntity();
            var fieldsInfo = EntityFieldsCache.Item(temp.GetType());
            foreach (KeyValuePair<string, string> kv in keyValuePairs)
            {
                string fieldName = fieldsInfo.GetPropertyField(kv.Key);
                bool isInCondition = false;
                bool isNotInCondition = false;
                if (fieldName != null)
                {
                    string value = kv.Value;
                    if (kv.Value.StartsWith(">=")) //field1 >=1
                    {
                        value = kv.Value.TrimStart('>', '=');
                    }
                    else if (kv.Value.StartsWith(">")) //field1 >1
                    {
                        value = kv.Value.TrimStart('>');
                    }
                    else if (kv.Value.StartsWith("<="))  //field1 <=1
                    {
                        value = kv.Value.TrimStart('<', '=');
                    }
                    else if (kv.Value.StartsWith("<"))  //field1 <1
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
                    else if (kv.Value.StartsWith("|") || (kv.Value.StartsWith("IN[") && kv.Value.EndsWith("]")))
                    {
                        isInCondition = true; //IN查询，此处不处理
                        e[kv.Key] = "0";
                    }
                    else if (kv.Value.StartsWith("!|") || (kv.Value.StartsWith("NOT IN[") && kv.Value.EndsWith("]")))
                    {
                        isNotInCondition = true; //IN查询，此处不处理
                        e[kv.Key] = "0";
                    }
                    else
                    {
                        try
                        {
                            temp[kv.Key] = value;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Warning at:temp[kv.Key] = value,kv.Key={0},value={1} .Error Message:", kv.Key, value, ex.Message);
                        }
                    }

                    if (kv.Value.StartsWith(">=")) //field1 >=1
                    {
                        cmpResult = cmpResult & cmp.Comparer(e[kv.Key], ">=", temp[kv.Key]);
                    }
                    else if (kv.Value.StartsWith(">")) //field1 >1
                    {
                        cmpResult = cmpResult & cmp.Comparer(e[kv.Key], ">", temp[kv.Key]);
                    }
                    else if (kv.Value.StartsWith("<="))  //field1 <=1
                    {
                        cmpResult = cmpResult & cmp.Comparer(e[kv.Key], "<=", temp[kv.Key]);
                    }
                    else if (kv.Value.StartsWith("<"))  //field1 <1
                    {
                        cmpResult = cmpResult & cmp.Comparer(e[kv.Key], "<", temp[kv.Key]);
                    }
                    else if (isInCondition || isNotInCondition)
                    {
                        string tempStr = "";
                        string strInOpt = "IN";
                        if (isInCondition)
                        {
                            tempStr = kv.Value.StartsWith('|') ?
                                      kv.Value.TrimStart('|') :                  //fieldName$|1 2 3
                                      kv.Value.Substring(3, kv.Value.Length - 4);//fieldName$IN[1 2 3]
                        }
                        else
                        {
                            strInOpt = "NOT IN";
                            tempStr = kv.Value.StartsWith("!|") ?
                                      kv.Value.TrimStart('!', '|') :              //fieldName$!|1 2 3
                                      kv.Value.Substring(7, kv.Value.Length - 8); //fieldName$NOT IN[1 2 3]
                        }
                        //value ="1 2 3";
                        if (type == typeof(string))
                        {
                            string[] cmpValue = tempStr.Split(' ');
                            cmpResult = cmpResult & cmp.Comparer(e[kv.Key], strInOpt, cmpValue);
                        }
                        else if (type == typeof(int))
                        {
                            string[] strValues = tempStr.Split(' ');
                            List<int> cmpValues = new List<int>();
                            foreach (string item in strValues)
                            {
                                if (int.TryParse(item, out int i))
                                    cmpValues.Add(i);
                            }
                            if (cmpValues.Count > 0)
                            {
                                int[] arr = cmpValues.ToArray();
                                cmpResult = cmpResult & cmp.Comparer((int)e[kv.Key], strInOpt, arr);
                            }
                        }
                        else if (type == typeof(long))
                        {
                            string[] strValues = tempStr.Split(' ');
                            List<long> cmpValues = new List<long>();
                            foreach (string item in strValues)
                            {
                                if (long.TryParse(item, out long i))
                                    cmpValues.Add(i);
                            }
                            if (cmpValues.Count > 0)
                            {
                                long[] arr = cmpValues.ToArray();
                                cmpResult = cmpResult & cmp.Comparer((long)e[kv.Key], strInOpt, arr);
                            }
                        }
                        else
                        {
                            string[] strValues = tempStr.Split(' ');
                            List<double> cmpValues = new List<double>();
                            foreach (string item in strValues)
                            {
                                if (double.TryParse(item, out double i))
                                    cmpValues.Add(i);
                            }
                            if (cmpValues.Count > 0)
                            {
                                double[] arr = cmpValues.ToArray();
                                cmpResult = cmpResult & cmp.Comparer((double)e[kv.Key], strInOpt, arr);
                            }
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(value)) //空值
                        {
                            if (kv.Value.StartsWith("="))
                                cmpResult = cmpResult & cmp.IsNull(e[kv.Key]);
                            else if (kv.Value.StartsWith("!="))
                                cmpResult = cmpResult & cmp.IsNotNull(e[kv.Key]);
                            else
                                cmpResult = cmpResult & cmp.IsNull(e[kv.Key]);
                        }
                        else if (value == "''") //空字符串，不是空值
                        {
                            temp[kv.Key] = "";
                            if (kv.Value.StartsWith("="))
                                cmpResult = cmpResult & cmp.Property(e[kv.Key]) == temp[kv.Key];
                            if (kv.Value.StartsWith("!="))
                                cmpResult = cmpResult & cmp.Property(e[kv.Key]) != temp[kv.Key];
                            else
                                cmpResult = cmpResult & cmp.Property(e[kv.Key]) == temp[kv.Key];
                        }
                        else if (value == "null") //过滤空值和空字符串
                        {
                            e[kv.Key] = "";
                            if (kv.Value.StartsWith("="))
                                cmpResult = cmpResult & (cmp.IsNull(e[kv.Key]) | cmp.EqualValue(e[kv.Key]));
                            else if (kv.Value.StartsWith("!="))
                                cmpResult = cmpResult & cmp.IsNotNull(e[kv.Key]) & cmp.Property(e[kv.Key]) != "";
                            else
                                cmpResult = cmpResult & (cmp.IsNull(e[kv.Key]) | cmp.EqualValue(e[kv.Key]));
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

        private void SetDecodingForGetter(object data)
        {
            if (data != null && data is IDecodingObject)
            {
                ((IDecodingObject)data).DecodingInGetter = true;
            }
        }

        public TObj ConvertTo<TObj>(TParent data) where TObj : class, new()
        {
            SetDecodingForGetter(data);
            return data.CopyTo<TObj>();
        }

        /// <summary>
        /// 将实体类列表数据转换成Dto列表数据，要求实体类和Dto有共同的接口
        /// </summary>
        /// <typeparam name="TSource">实体类类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public List<TTarget> ConvertToList<TSource, TTarget>(List<TSource> source)
            where TTarget : class, new()
            where TSource : EntityBase, new()
        {
            return source.ConvertAll<TTarget>(entity => {
                if (entity is IDecodingObject)
                {
                    ((IDecodingObject)entity).DecodingInGetter = true;
                    return entity.CopyTo<TTarget>();
                }
                else
                {
                    TTarget target = EntityToPOCO<TSource, TTarget>(entity);
                    return target;
                }
            });
        }

        public int Delete(Tid id)
        {
            TEntity entity = new TEntity();
            entity.ID = id;
            return _dbContext.Remove(entity);
        }

        public IEnumerable<TParent> GetAll()
        {
            var list = _dbContext.QueryAllList<TEntity>();
            //查询所有数据
            //if (ServiceConst.QueryAllRecordCount > 0)
            //{
            //    if (list.Count > ServiceConst.QueryAllRecordCount)
            //    {
            //        var result = list.Take(ServiceConst.QueryAllRecordCount);
            //        foreach (var data in result)
            //        {
            //            SetDecodingForGetter(data);
            //        }
            //        return result;
            //    }
            //}
            foreach (var data in list)
            {
                SetDecodingForGetter(data);
            }
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
            SetDecodingForGetter(dbEntity);
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

        /// <summary>
        /// 根据属性名和属性值获取数据
        /// </summary>
        /// <param name="propertyName">TParent对象的属性名字</param>
        /// <param name="propertyValue">TParent对象的属性值</param>
        /// <returns></returns>
        public TParent? GetByPropertyValue(string propertyName, object? propertyValue)
        {
            TEntity entity = new TEntity();
            entity[propertyName] = propertyValue;

            var q = OQL.From(entity)
                    .Select()
                    .Where(entity[propertyName])
                    .END;
            TEntity dbEntity = _dbContext.QueryObject<TEntity>(q);
            return dbEntity;
        }

        private TEntity PrepairEntity(TParent data)
        {
            var entity = data as TEntity;
            if (entity == null)
            {
                entity = new TEntity();
                if (entity is IDecodingObject)
                {
                    var entity2 = data.CopyTo<TEntity>();//调用属性方法，对属性值进行编码
                    SetPropertyDefaultValue(entity);
                    entity.MapFrom(entity2, true);
                }
                else
                {
                    SetPropertyDefaultValue(entity);
                    entity.MapFrom(data, true);//不调用属性方法，直接获取内部属性值
                }
                //使用 CopyTo 方法会设置实体类所有属性的修改状态，这会导致不正确的实体类映射表字段的默认值，
                //在表字段部分写入值的时候出现问题，故不建议这样做，应该使用实体类的 MapFrom 方法并设置修改属性。
                //entity = data.CopyTo<TEntity>();
            }
            return entity;
        }

        /// <summary>
        /// 插入数据。(使用本方法需要注意remarks说明)
        /// </summary>
        /// <remarks>
        /// 注意如果data是DTO之类简单对象，对象成员属性的值是类型默认值可能不会插入到数据库中，在这种情况下，
        /// 可以重写 SetPropertyDefaultValue 方法并在此方法中对需要插入的实体类属性设置默认值，或者使用CompareAndUpdate 方法替代。
        /// </remarks>
        /// <param name="data">实体类或者DTO等数据对象</param>
        /// <returns>操作受影响的行数</returns>
        public int Insert(TParent data)
        {
            var entity = PrepairEntity(data);
            int count = _dbContext.Add(entity);
            data.ID = entity.ID;//获取自增的ID值
            return count;
        }

        /// <summary>
        /// 更新数据。(使用本方法需要注意remarks说明)
        /// </summary>
        /// <remarks>
        /// 注意如果data是DTO之类简单对象，对象成员属性的值是类型默认值可能不会更新到数据库中，在这种情况下，
        /// 可以重写 SetPropertyDefaultValue 方法并在此方法中对需要更新的实体类属性设置默认值，或者使用CompareAndUpdate 方法替代。
        /// </remarks>
        /// <param name="data">实体类或者DTO等数据对象</param>
        /// <returns>操作受影响的行数</returns>
        public int Update(TParent data)
        {
            var entity = PrepairEntity(data);
            return _dbContext.Update(entity);
        }

        /// <summary>
        /// 比较和更新。在更新数据之前先从数据库查询指定主键的数据进行比较，如果不同再更新，如果没有则不更新。
        /// </summary>
        /// <typeparam name="TObj">POCO类型</typeparam>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <typeparam name="Tpk">主键类型</typeparam>
        /// <param name="obj">要更新的数据对象</param>
        /// <param name="beforUpdate">实体类更新前要进行的处理</param>
        /// <returns>返回查询到的记录数量</returns>
        public int CompareAndUpdate<TObj, T, Tpk>(TObj obj, Tpk id, Action<T>? beforUpdate = null)
            where T : EntityBase, IIdentityProperty<Tpk>, new()
            where TObj : IIdentityProperty<Tpk>, new()
        {
            T entity = new T();
            entity.ID = id;
            var entityQuery = this.CreateQuery<T>();
            if (entityQuery.FillEntity(entity))
            {
                entity.ID = id;//ID属性的内部类型可能变成了long类型，这里改回来。
                int changeCount = entity.MapFrom(obj, true);
                if (changeCount > 0)
                {
                    if (beforUpdate != null)
                        beforUpdate(entity);
                    return entityQuery.Update(entity);
                }
                return 1;
            }
            else
            {
                return 0;
            }
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

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="tids">要删除数据的ID数组</param>
        /// <returns>删除数据的条数</returns>
        public int BatchDelete(Tid[] tids)
        {
            TEntity entity = new TEntity();
            OQL q = OQL.From(entity)
                .Delete()
                .Where(cmp => cmp.Comparer(entity.ID, "in", tids))
                .END;
            return EntityQuery.ExecuteOql(q, _dbContext.CurrentDataBase);
        }

        public TParent NewEntity()
        {
            return new TEntity();
        }

        /// <summary>
        /// 设置实体对象属性的默认值。该方法仅在实体类数据插入数据库前调用。设置属性的 defaultValue能够修改字段属性的修改状态，以便将默认值更新到数据库中。
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void SetPropertyDefaultValue(TEntity entity)
        {
            //当属性值为类型默认值的时候，默认情况下该属性的修改状态为未修改。
            //如果需要将属性的类型默认值保存到数据库，请在子类重写本方法，例如：entity.XID=0;
            //如需要设置所有属性为已修改状态，请在子类调用下面一行代码。
            //entity.ResetChanges(true);
        }
    }
}
