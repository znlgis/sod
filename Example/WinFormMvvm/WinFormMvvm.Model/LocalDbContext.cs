using PWMIS.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormMvvm.Model
{
    class LocalDbContext : DbContext
    {
        public LocalDbContext()
            : base("default")
        {
            //local 是连接字符串名字 
        }

        protected override bool CheckAllTableExists()
        {
            //创建用户表 
            CheckTableExists<UserEntity>();
            return true;
        }
    }

}
