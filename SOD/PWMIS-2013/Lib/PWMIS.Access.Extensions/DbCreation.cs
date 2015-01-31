using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using PWMIS.DataProvider.Data;
using PWMIS.DataMap.Entity;
using ADOX;
using System.Configuration;

namespace PWMIS.AccessExtensions
{
    /// <summary>
    /// Access 数据库访问帮助类
    /// </summary>
    public static class AccessUility
    {
        /// <summary>
        /// 指定文件名，创建Access数据库文件，适用于32位系统的Access
        /// </summary>
        /// <param name="filePath">数据库文件路径</param>
        public static void CreateDataBase(string filePath)
        {
            ADOX.Catalog catalog = new Catalog();
            catalog.Create("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Jet OLEDB:Engine Type=5");
        }

        /// <summary>
        /// 根据连接字符串，创建数据库文件，适用于OLEDB4.0 之后的Access数据库版本或者64位系统
        /// </summary>
        /// <param name="connString"></param>
        public static void CreateDataBaseByConnString(string connString)
        {
            ADOX.Catalog catalog = new Catalog();
            catalog.Create(connString);
        }

        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="access"></param>
        /// <param name="entity">实体类</param>
        public static void CreateTable(Access access, EntityBase entity)
        {
            EntityCommand ecmd = new EntityCommand(entity, access);
            string sql = ecmd.CreateTableCommand;
            access.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 配置一个指定的连接名称，并指定Access数据库文件的名字，适用于32位系统
        /// </summary>
        /// <param name="connectionName">连接名称</param>
        /// <param name="dbFilePath">数据库文件路径</param>
        public static void ConfigConnectionSettings(string connectionName, string dbFilePath)
        {
            //Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //if (cfa.ConnectionStrings.ConnectionStrings[connectionName] == null)
            //{
            //    var connSetting = new ConnectionStringSettings(connectionName, "Provider=Microsoft.Jet.Oledb.4.0;Data Source=" + dbFilePath, "Access");
            //    cfa.ConnectionStrings.ConnectionStrings.Add(connSetting);
            //    cfa.Save();
            //    ConfigurationManager.RefreshSection("connectionStrings");
            //}

            ConfigConnectionSettings2(connectionName, "Provider=Microsoft.Jet.Oledb.4.0;Data Source=" + dbFilePath);
        }

        /// <summary>
        /// 配置一个指定的连接名称，并指定Access数据库文件的名字，适用于64位系统
        /// </summary>
        /// <param name="connectionName"></param>
        /// <param name="connectionString"></param>
        public static void ConfigConnectionSettings2(string connectionName,string connectionString)
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (cfa.ConnectionStrings.ConnectionStrings[connectionName] == null)
            {
                var connSetting = new ConnectionStringSettings(connectionName, connectionString, "Access");
                cfa.ConnectionStrings.ConnectionStrings.Add(connSetting);
                cfa.Save();
                ConfigurationManager.RefreshSection("connectionStrings");
            }
        }

        /*
         * public static void CreateTable_Test(Catalog catalog)
        {
            ADOX.Table item = new ADOX.Table();
            item.Name = "Test";
            ADOX.Column column = new ADOX.Column();
            column.ParentCatalog = catalog;
            column.Name = "ID";
            column.Type = ADOX.DataTypeEnum.adInteger;
            column.DefinedSize = 9;
            column.Properties["AutoIncrement"].Value = true;
            item.Columns.Append(column, ADOX.DataTypeEnum.adInteger, 9);
            item.Keys.Append("IDKey", KeyTypeEnum.adKeyPrimary, column, null, null);
            item.Columns.Append("PrjOnKey", ADOX.DataTypeEnum.adVarWChar, 50);
            item.Columns.Append("MeasTime", ADOX.DataTypeEnum.adDate, 0);
            catalog.Tables.Append(item);
        }
         */
    }
}
