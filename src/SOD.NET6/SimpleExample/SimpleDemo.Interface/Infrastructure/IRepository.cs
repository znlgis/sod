using SimpleDemo.Model;

namespace SimpleDemo.Interface.Infrastructure
{
    

    public interface IRepository 
    {
        /// <summary>
        /// 当前仓储对象实例的标识
        /// </summary>
        Guid ID { get; }
        IUowManager UnitOfWorkManager { get; }
    }

    public interface IRepository<TParent, Tid> : IRepository where TParent : class, IIdentityProperty<Tid>
    {
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        IEnumerable<TParent> GetAll();
        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TParent? GetById(Tid id);
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns>操作受影响的记录数</returns>
        int Insert(TParent data);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns>操作受影响的记录数</returns>
        int Update(TParent data);
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">操作受影响的记录数</param>
        /// <returns></returns>
        int Delete(Tid id);

        /// <summary>
        /// 将数据转换成指定的类型
        /// </summary>
        /// <typeparam name="TObj"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        TObj ConvertTo<TObj>(TParent data) where TObj : class, new();

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageNum">页码，从1开始</param>
        /// <param name="total">记录总数，如果小于等于0则会查询记录总数</param>
        /// <param name="keyValuePairs">记录过滤条件</param>
        /// <returns></returns>
        public PageResult<TParent> GetPageList(int pageSize, int pageNum, int total, List<KeyValuePair<string, string>> keyValuePairs, string[] orderStrings = null);

    }
}
