using System.Collections.Generic;
using PWMIS.Core.Extensions;
using PWMIS.DataMap.Entity;
using WebApplication2.Model;

namespace WebApplication2.Repository
{
    public class ClassReunionRepository : DbContext
    {
        public ClassReunionRepository()
            : base("accessConn")
        {
        }

        public List<ContactInfo> AllContactInfo => OQL.FromObject<ContactInfo>().ToList(CurrentDataBase);

        public EntityQuery<ContactInfo> UserQuery => NewQuery<ContactInfo>();

        protected override bool CheckAllTableExists()
        {
            //可以使用 base.DbContextProvider 获取具体的提供程序，调用特定的方法
            //创建表
            CheckTableExists<ContactInfo>();
            return true;
        }

        public ContactInfo GetUser(int id)
        {
            return OQL.FromObject<ContactInfo>()
                .Select()
                .Where((cmp, e) => cmp.Property(e.CID) == id)
                .END
                .ToObject(CurrentDataBase);
        }
    }
}