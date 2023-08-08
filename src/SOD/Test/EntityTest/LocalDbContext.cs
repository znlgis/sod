using System.Collections.Generic;
using PWMIS.Core.Extensions;
using PWMIS.DataMap.Entity;

namespace EntityTest
{
    /// <summary>
    ///     用来测试的本地SqlServer 数据库上下文类
    /// </summary>
    public class LocalDbContext : DbContext
    {
        public LocalDbContext()
            : base("local")
        {
            //local 是连接字符串名字
        }


        public List<IUser> AllUsers => OQL.FromObject<IUser>().ToList(CurrentDataBase);

        public EntityQuery<UserEntity> UserQuery => NewQuery<UserEntity>();

        #region 父类抽象方法的实现

        protected override bool CheckAllTableExists()
        {
            //可以使用 base.DbContextProvider 获取具体的提供程序，调用特定的方法
            //创建用户表
            CheckTableExists<UserEntity>();
            return true;
        }

        #endregion

        public IUser GetUser(int id)
        {
            return OQL.FromObject<IUser>()
                .Select()
                .Where((cmp, e) => cmp.Property(e.UserID) == id)
                .END
                .ToObject(CurrentDataBase);
        }
    }
}