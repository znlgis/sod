using SimpleDemo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDemo.Interface.Infrastructure
{
    public interface IServiceBase<TParent, TObj, Tid>
        where TParent : class, IIdentityProperty<Tid>
        where TObj : class, TParent, new()
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns>操作是否成功</returns>
        bool Insert(TParent data);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns>操作是否成功</returns>
        bool Update(TParent data);
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">要删除记录的标识值</param>
        /// <returns>操作是否成功</returns>
        bool Delete(Tid id);
        IEnumerable<TObj> GetAll();
        TObj? GetById(Tid id);

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageNum">页码，从1开始</param>
        /// <param name="total">记录总数，如果小于等于0则会查询记录总数</param>
        /// <param name="filter">记录过滤条件</param>
        /// <returns></returns>
        PageResult<TObj> GetPageList(int pageSize, int pageNum, int total, string filter);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="tids">要删除数据的ID数组</param>
        /// <returns>删除数据的条数</returns>
        int BatchDelete(Tid[] tids);
    }
}
