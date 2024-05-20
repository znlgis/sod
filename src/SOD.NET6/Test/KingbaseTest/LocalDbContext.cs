using PWMIS.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingbaseTest
{
    class LocalDbContext : DbContext
    {
        public LocalDbContext() : base("local")
        {

        }

        protected override bool CheckAllTableExists()
        {
            //base.InitializeTable<Table_User>("");
            base.CheckTableExists<SimpleEntity>();
            return true;
        }
    }
}
