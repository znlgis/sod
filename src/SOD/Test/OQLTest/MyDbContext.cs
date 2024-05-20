using PWMIS.Core.Extensions;

namespace OQLTest
{
    public class MyDbContext : DbContext
    {
        public MyDbContext() : base("conn1")
        {
        }

        protected override bool CheckAllTableExists()
        {
            CheckTableExists<v_userFavorites>();
            return true;
        }
    }
}
