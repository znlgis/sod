using PWMIS.Core.Extensions;
using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityTest
{
   /// <summary>
   /// 用来测试的本地SqlServer 数据库上下文类
   /// </summary>
    public class LocalDbContext : SqlServerDbContext
    {
        public LocalDbContext()
            : base("local")
        {

        }

        #region 父类抽象方法的实现

        protected override bool CheckAllTableExists()
        {
            //创建用户表
            CheckTableExists<UserEntity>();
            return true;
        }

        #endregion


        public List<IUser> AllUsers
        {
            get
            {
                return OQL.FromObject<IUser>().ToList(CurrentDataBase);
            }
        }

        public IUser GetUser(int id)
        {
            return OQL.FromObject<IUser>()
                       .Select()
                       .Where((cmp, e) => cmp.Property(e.UserID) == id)
                       .END
                    .ToObject(CurrentDataBase);
        }

        public EntityQuery<UserEntity> UserQuery
        {
            get 
            {
                return base.NewQuery<UserEntity>();
            }
        }
    }
}
