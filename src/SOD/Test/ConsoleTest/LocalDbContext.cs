using LocalDB;
using PWMIS.Core;
using PWMIS.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleTest
{
    class LocalDbContext:DbContext
    {
        public LocalDbContext() : base("local")
        {

        }

        protected override bool CheckAllTableExists()
        {
            base.InitializeTable<Table_User>("");
            base.CheckTableExists<MyCommandLogEntity>();
            return true;
        }
    }
}
