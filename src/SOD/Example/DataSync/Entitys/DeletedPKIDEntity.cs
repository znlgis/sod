using System;
using PWMIS.DataMap.Entity;

namespace SOD.DataSync.Entitys
{
    /// <summary>
    ///     数据表删除数据对应的ID记录信息
    /// </summary>
    public class DeletedPKIDEntity : EntityBase, IExportTable
    {
        public DeletedPKIDEntity()
        {
            TableName = "DeletedTableIDs";
            PrimaryKeys.Add("ID");
            IdentityName = "ID";
        }

        /// <summary>
        ///     自增标识
        /// </summary>
        public int ID
        {
            get => getProperty<int>("ID");
            set => setProperty("ID", value);
        }

        /// <summary>
        ///     所在表名称
        /// </summary>
        public string TargetTableName
        {
            get => getProperty<string>("TargetTableName");
            set => setProperty("TargetTableName", value, 250);
        }

        /// <summary>
        ///     所在ID
        /// </summary>
        public long TargetID
        {
            get => getProperty<long>("TargetID");
            set => setProperty("TargetID", value);
        }

        /// <summary>
        ///     所在的字符串类型ID
        /// </summary>
        public string TargetStringID
        {
            get => getProperty<string>("TargetStringID");
            set => setProperty("TargetStringID", value, 128);
        }

        /// <summary>
        ///     删除的时间
        /// </summary>
        public DateTime DeletedTime
        {
            get => getProperty<DateTime>("DeletedTime");
            set => setProperty("DeletedTime", value);
        }

        /// <summary>
        ///     批次号，非持久化属性
        /// </summary>
        public int BatchNumber
        {
            get => getProperty<int>("BatchNumber");
            set => setProperty("BatchNumber", value);
        }
    }
}