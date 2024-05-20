using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Data;

namespace KingbaseTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, SOD6!");
            SimpleEntity entity = new SimpleEntity();
            entity.Name = "Test_"+DateTime.Now.ToString("yyyyMMddHHmmss");
            entity.AtTime = DateTime.Now;
            LocalDbContext ctx = new LocalDbContext();
            ctx.Add(entity);
            var localDb = (Kingbase)ctx.CurrentDataBase;
            var cb = localDb.ConnectionStringBuilder;
            Console.WriteLine("Kingbase Add Entity Data OK! Database User ID={0},\r\n " +
                "Inserted Entity Identity Field value={1}", localDb.ConnectionUserID,entity.ID);

            //查询前10条数据
            var list = OQL.From<SimpleEntity>().Limit(10, 1).ToList();
            Console.WriteLine("Query Data count={0}", list.Count);
            Console.Read();
        }
    }
}
