using LocalDB;
using PWMIS.Core;
using PWMIS.Core.Extensions;

namespace ConsoleTest
{
    internal class LocalDbContext : DbContext
    {
        public LocalDbContext() : base("local")
        {
        }

        protected override bool CheckAllTableExists()
        {
            InitializeTable<Table_User>("");
            CheckTableExists<MyCommandLogEntity>();
            return true;
        }
    }
}