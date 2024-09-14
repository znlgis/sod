using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Data;

namespace KingbaseTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, SOD6!");
            Test1();

            SimpleEntity entity = new SimpleEntity();
            entity.Name = "Test_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            entity.AtTime = DateTime.Now;
            LocalDbContext ctx = new LocalDbContext();
            ctx.Add(entity);

            Console.WriteLine("insert ok");
        }

        static void Test1()
        {
            SimpleEntity entity = new SimpleEntity();
            SimpleEntity entity2 = new SimpleEntity();

            var helper = AdoHelper.CreateHelper("local2");
            //Kingbase 架构集合名字:
            /*
             * 不区分大小写：
 "METADATACOLLECTIONS" 
 "RESTRICTIONS" 
 "DATASOURCEINFORMATION"
 "DATATYPES"
 "RESERVEDWORDS" 
 "DATABASES" 
 "SCHEMATA" 
 "TABLES" 
 "COLUMNS" 
 "VIEWS" 
 "USERS" 
 "INDEXES" 
 "INDEXCOLUMNS" 
 "CONSTRAINTS"
 "PRIMARYKEY" 
 "UNIQUEKEYS" 
 "FOREIGNKEYS" 
 "CONSTRAINTCOLUMNS" 
             * 
             */
            var schema0 = helper.GetSchema("TABLES", null);

            String[] columnRestrictions = new String[4];
            columnRestrictions[1] = "public";
            columnRestrictions[2] = "alarms_202404";

            var schema = helper.GetSchema("Columns", columnRestrictions);
            var schema2 = helper.GetSchema("DATASOURCEINFORMATION", null);


            string sqlCount = @"SELECT   COUNT( ""ID"") AS  ""ID"" 
FROM ""Arm_2405""  
     WHERE    ""AtTime"" >= :P0 AND  ""AtTime"" <= :P1 ";
            DateTime dt0 = new DateTime(2024, 5, 23, 9, 18, 54);
            DateTime dt1 = new DateTime(2024, 5, 24, 9, 18, 54);
            //对于Kingbase 的DateTime类型,timestamp类型，需要将参数转换成字符串进行 SELECT 查询，
            //timestamp with time zone,timestamp without time zone 不需要。
            //var p0 = helper.GetParameter("P0", dt0.ToString("yyyy-MM-dd HH:mm:ss"));
            //var p1= helper.GetParameter("P1",dt1.ToString("yyyy-MM-dd HH:mm:ss"));
            var p0 = helper.GetParameter("P0", dt0);
            var p1 = helper.GetParameter("P1", dt1);

            System.Data.IDataParameter[] paras = helper.CreateParameters(p0, p1);

            long count = (long)helper.ExecuteScalar(sqlCount, System.Data.CommandType.Text, paras);

            string sqlSelect = @"select * from ""Arm_2405"" where ""szAlarmID"" = :P0 ";
            var p2 = helper.GetParameter("P0", "3399509722548600002.20240523091854121");
            System.Data.IDataParameter[] paras2 = helper.CreateParameters(p2);
            var ds123 = helper.ExecuteDataSet(sqlSelect, System.Data.CommandType.Text, paras2);
            //2024-5-23 9:18:54
            string sqlSelect3 = @"select * from ""Arm_2405"" where ""AtTime"" = '2024-05-23 09:18:54' ";
            var ds1111 = helper.ExecuteDataSet(sqlSelect3);

            DateTime dt3 = new DateTime(2024, 5, 23, 9, 18, 54);

            string sqlSelect2 = @"select * from ""Arm_2405"" where ""AtTime"" = :P0 ";
            var p3 = helper.GetParameter("P0", dt3) as Kdbndp.KdbndpParameter;
            //p3.KdbndpDbType = KdbndpTypes.KdbndpDbType.Timestamp;
            System.Data.IDataParameter[] paras3 = helper.CreateParameters(p3);
            var ds111 = helper.ExecuteDataSet(sqlSelect2, System.Data.CommandType.Text, paras3);


            helper.BeginTransaction();
            string sql_insert = "insert into devicesetstatus([EquipmentID]) values('123434567890');";
            helper.ExecuteNonQuery(sql_insert);
            string sql = "SELECT nextval('devicesetstatus_id_seq')";
            var ds = helper.ExecuteScalar(sql);
            string sql2 = "SELECT currval('devicesetstatus_id_seq') ;";
            var ds2 = helper.ExecuteScalar(sql2);
            var ds3 = helper.ExecuteScalar(sql2);
            helper.Commit();


            entity.Name = "Test_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            entity.AtTime = DateTime.Now;
            LocalDbContext ctx = new LocalDbContext();
            ctx.Add(entity);

            //示例：下面方法将批量生成当前库下类似下面的表的列修改语句：
            //ALTER TABLE "SimpleTable3" MODIFY COLUMN "AtTime" timestamp WITHOUT time ZONE  
            string[] fieldNames = new string[] { "AtTime" };
            string ddlsql = ctx.GetModifyColumnSql(fieldNames, typeof(DateTime), "timestamp without time zone");

            //如果数据库表上的列类型是 timestamp ，参数化查询select数据会报错:
            //KingbaseException: 22P02: invalid input syntax for type double precision: "2024-05-24 14:06:05"
            DateTime dt4 = new DateTime(2024, 5, 24, 14, 6, 5);
            string sqlSelect4 = @"select * from SimpleTable1 where AtTime = :P0 ";
            var p4 = helper.GetParameter("P0", dt4) as Kdbndp.KdbndpParameter;
            p4.KdbndpDbType = KdbndpTypes.KdbndpDbType.Timestamp;
            p4.DataTypeName = "timestamp";
            System.Data.IDataParameter[] paras4 = helper.CreateParameters(p4);
            var ds222 = helper.ExecuteDataSet(sqlSelect4, System.Data.CommandType.Text, paras4);

            //修改字段时间类型：
            //ALTER TABLE "SimpleTable3" MODIFY COLUMN "AtTime" timestamp WITHOUT time ZONE  

            var localDb = (Kingbase)ctx.CurrentDataBase;
            var cb = localDb.ConnectionStringBuilder;
            Console.WriteLine("Kingbase Add Entity Data OK! Database User ID={0},\r\n " +
                "Inserted Entity Identity Field value={1}", localDb.ConnectionUserID, entity.ID);

            //查询前10条数据
            var list = OQL.From<SimpleEntity>().Limit(10, 1).ToList();
            Console.WriteLine("Query Data count={0}", list.Count);
            Console.Read();
        }
    }
}
