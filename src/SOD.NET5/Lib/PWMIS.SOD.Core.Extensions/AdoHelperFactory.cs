using System;
using PWMIS.DataProvider.Adapter;
using PWMIS.DataProvider.Data;

namespace PWMIS.Core.Extensions
{
    public class AdoHelperFactory
    {
        /// <summary>
        ///     从应用程序配置文件的数据连接配置信息中，根据连接名称信息构造AdoHelper实例对象，
        ///     如果providerName配置为[SqlServer/Oracle/Odbc/SQLite]之一且大小写一致，
        ///     那么将直接构造AdoHelper实例对象而不使用任何反射过程。
        /// </summary>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public static AdoHelper DirectCreateHelper(string connectionName)
        {
            var connSetting = MyDB.GetDBConnectionSettingInfo(connectionName);

            var providerName = connSetting.Item1;
            var connectionString = connSetting.Item2;
            var providerInfo = providerName.Split(',');
            var helperAssembly = "";
            string helperType;

            if (providerInfo.Length == 1)
            {
                switch (providerName)
                {
                    case "SqlServer":
                    case "":
                        AdoHelper helper = new SqlServer();
                        helper.ConnectionString = connectionString;
                        return helper;
                    case "Access":
                    case "OleDb":
                        helperAssembly = "PWMIS.SOD.Core.AccessExtensions";
                        break;
                    case "Oracle":
                        AdoHelper helper2 = new Oracle();
                        helper2.ConnectionString = connectionString;
                        return helper2;
                    case "Odbc":
                        AdoHelper helper3 = new Odbc();
                        helper3.ConnectionString = connectionString;
                        return helper3;
                    case "SQLite":
                        AdoHelper helper4 = new SQLite();
                        helper4.ConnectionString = connectionString;
                        return helper4;
                }

                if (helperAssembly == "")
                    throw new Exception(
                        "数据访问提供程序名错误。如果是简便名称（没有逗号分隔），必须是[SqlServer/Access/Oledb/Oracle/Odbc/SQLite]之一，且大小写一致。 ");

                helperType = "PWMIS.DataProvider.Data." + providerName;
            }
            else
            {
                helperAssembly = providerInfo[1].Trim();
                helperType = providerInfo[0].Trim();
            }

            return MyDB.GetDBHelper(helperAssembly, helperType, connectionString);
        }
    }
}