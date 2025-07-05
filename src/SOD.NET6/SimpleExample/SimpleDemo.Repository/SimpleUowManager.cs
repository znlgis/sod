using PWMIS.Core.Extensions;
using SimpleDemo.Interface.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDemo.Repository
{

    public class SimpleUowManager : IUowManager
    {
        DbContext _dbContext;
        Guid _id;
        public SimpleUowManager(DbContext dbContext)
        {
            _dbContext = dbContext;
            _id = Guid.NewGuid();
        }
        public Guid ID => _id;

        /// <summary>
        /// 是否是分布式事务。结果为否。
        /// </summary>
        public bool IsGlobalTrans
        {
            get { return false; }
            set { }
        }
        public void BeginTransaction(string? transName = "")
        {
            Console.WriteLine("SimpleUowManager 开始事务。。。。" + transName);
            _dbContext.CurrentDataBase.BeginTransaction();
        }

        public void BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public bool Commit()
        {
            Console.WriteLine("SimpleUowManager 提交事务。。。。");
            try
            {
                _dbContext.CurrentDataBase.Commit();
                Console.WriteLine("SimpleUowManager 提交事务成功！");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("事务提交异常，错误信息：" + ex.Message);
            }
            return false;
        }

        public void Rollback()
        {
            Console.WriteLine("SimpleUowManager 回滚事务");
            _dbContext.CurrentDataBase.Rollback();
        }

        void IUowManager.Commit()
        {
            this.Commit();
        }
    }

}
