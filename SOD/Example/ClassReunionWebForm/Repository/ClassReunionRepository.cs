using PWMIS.Core.Extensions;
using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebApplication2.Model;

namespace WebApplication2.Repository
{
    public class ClassReunionRepository : DbContext
    {
        public ClassReunionRepository()
            : base("accessConn") 
        {
        
        }

        protected override bool CheckAllTableExists()
        {
            //可以使用 base.DbContextProvider 获取具体的提供程序，调用特定的方法
            //创建表
            CheckTableExists<ContactInfo>();
            return true;
        }

        public List<ContactInfo> AllContactInfo
        {
            get
            {
                return OQL.FromObject<ContactInfo>().ToList(CurrentDataBase);
            }
        }

        public ContactInfo GetUser(int id)
        {
            return OQL.FromObject<ContactInfo>()
                       .Select()
                       .Where((cmp, e) => cmp.Property(e.CID) == id)
                       .END
                    .ToObject(CurrentDataBase);
        }

        public EntityQuery<ContactInfo> UserQuery
        {
            get
            {
                return base.NewQuery<ContactInfo>();
            }
        }
    }
}