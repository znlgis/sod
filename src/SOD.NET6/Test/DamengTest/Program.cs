using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Data;

namespace DamengTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, SOD6!");
            //自增表测试，参考官方技术文档 https://eco.dameng.com/document/dm/zh-cn/faq/faq-sql-gramm.html
            //如果需要创建表，请取消下一个代码中的注释
            string createTable = @"
CREATE TABLE new_employees1
(
id_num int IDENTITY(1,1),
fname varchar (20),
minit char(1),
lname varchar(30)
);";
            var helper = AdoHelper.CreateHelper("local");
            //helper.ExecuteNonQuery(createTable);
            //查询插入的自增值必须跟插入语句在同一个连接会话中
            helper.OpenSession();
            string sql_insert = "insert into new_employees1(fname,minit,lname) values('test','d','test1');";
            helper.ExecuteNonQuery(sql_insert);
            string sql_lastid = "select @@IDENTITY";
            long lastInsertedId=(long)helper.ExecuteScalar(sql_lastid);
            Console.WriteLine("插入记录，自增标识列[ID]={0}", lastInsertedId);
            helper.CloseSession();
            //自增表测试完成

            SimpleEntity entity = new SimpleEntity();
            entity.Name = "Test_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            entity.AtTime = DateTime.Now;
            LocalDbContext ctx = new LocalDbContext();
            ctx.Add(entity);
            var localDb = (Dameng)ctx.CurrentDataBase;
            var cb = localDb.ConnectionStringBuilder;
            Console.WriteLine("Dameng(达梦) Add Entity Data OK! Database User ID={0},\r\n " +
                "Inserted Entity Identity Field value={1}", localDb.ConnectionUserID, entity.ID);
            //查询前10条数据
            var list= OQL.From<SimpleEntity>().Limit(10,1).ToList();
            //使用接口类型查询
            //SimpleEntity 映射的表名称和类型名称不一致，所以下面的查询需要指定表名称
            string tableName = entity.GetTableName();
            var list2 = OQL.FromObject<ISimple>(tableName)
                .Select()
                .OrderBy((o,entity)=>o.Asc(entity.ID)) //ISimple 动态创建的实体类没有指定主键信息，分页前需要指定排序字段
                .Limit(10, 1)
                .ToList();
            Console.WriteLine("Data record count={0}", list.Count);
            Console.Read();

        }
    }
}
