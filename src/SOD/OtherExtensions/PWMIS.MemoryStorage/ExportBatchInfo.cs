using System;
using PWMIS.DataMap.Entity;

namespace SOD.DataSync
{
    /// <summary>
    ///     数据导入批次信息
    /// </summary>
    public class ExportBatchInfo : EntityBase
    {
        public ExportBatchInfo()
        {
            TableName = "ExportBatchInfo";
            IdentityName = "ID";
            PrimaryKeys.Add("ID");
        }

        /// <summary>
        ///     主键
        /// </summary>
        public int ID
        {
            get => getProperty<int>("ID");
            set => setProperty("ID", value);
        }

        /// <summary>
        ///     导出的表名称
        /// </summary>
        public string ExportTableName
        {
            get => getProperty<string>("ExportTableName");
            set => setProperty("ExportTableName", value, 255);
        }

        /// <summary>
        ///     批次号
        /// </summary>
        public int BatchNumber
        {
            get => getProperty<int>("BatchNumber");
            set => setProperty("BatchNumber", value);
        }

        /// <summary>
        ///     导入本批次数据的数据包的文件路径
        /// </summary>
        public string PackagePath
        {
            get => getProperty<string>("PackagePath");
            set => setProperty("PackagePath", value, 255);
        }

        /// <summary>
        ///     上次导出时间
        /// </summary>
        public DateTime LastExportDate
        {
            get => getProperty<DateTime>("LastExportDate");
            set => setProperty("LastExportDate", value);
        }
    }
}