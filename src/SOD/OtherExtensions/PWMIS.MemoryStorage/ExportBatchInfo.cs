using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PWMIS.MemoryStorage
{
    /// <summary>
    /// 数据导入批次信息
    /// </summary>
    public class ExportBatchInfo:EntityBase
    {
        public ExportBatchInfo()
        {
            TableName = "ExportBatchInfoV2";
            IdentityName = "ID";
            PrimaryKeys.Add("ID");
        }

        /// <summary>
        /// 主键
        /// </summary>
        public int ID
        {
            get { return getProperty<int>("ID"); }
            set { setProperty("ID", value); }
        }

        /// <summary>
        /// 导出的表名称
        /// </summary>
        public string ExportTableName
        {
            get { return getProperty<string>("ExportTableName"); }
            set { setProperty("ExportTableName", value, 255); }
        }

        /// <summary>
        /// 批次号
        /// </summary>
        public int BatchNumber
        {
            get { return getProperty<int>("BatchNumber"); }
            set { setProperty("BatchNumber", value); }
        }

        /// <summary>
        /// 导入本批次数据的数据包的文件路径
        /// </summary>
        public string PackagePath
        {
            get { return getProperty<string>("PackagePath"); }
            set { setProperty("PackagePath", value, 255); }
        }

        /// <summary>
        /// 上次导出时间
        /// </summary>
        public DateTime LastExportDate
        {
            get { return getProperty<DateTime>("LastExportDate"); }
            set { setProperty("LastExportDate", value); }
        }
    }
}
