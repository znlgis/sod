using System;
using PWMIS.Common;
using PWMIS.DataMap.Entity;

namespace RoadTeam.Model.CS
{
    [Serializable]
    public class TbCsEvent : EntityBase
    {
        public TbCsEvent()
        {
            TableName = "TbCsEvent";
            EntityMap = EntityMapType.Table;
            //IdentityName = "标识字段名";
            IdentityName = "EventID";

            //PrimaryKeys.Add("主键字段名");
            PrimaryKeys.Add("EventID");
        }


        /// <summary>
        ///     事件ID
        /// </summary>
        public decimal EventID
        {
            get => getProperty<decimal>("EventID");
            set => setProperty("EventID", value);
        }

        /// <summary>
        ///     事件编号
        /// </summary>
        public string EventCode
        {
            get => getProperty<string>("EventCode");
            set => setProperty("EventCode", value, 50);
        }

        /// <summary>
        ///     发生位置
        /// </summary>
        public string EventPlace
        {
            get => getProperty<string>("EventPlace");
            set => setProperty("EventPlace", value, 50);
        }

        /// <summary>
        ///     发生时间
        /// </summary>
        public DateTime EventTime
        {
            get => getProperty<DateTime>("EventTime");
            set => setProperty("EventTime", value);
        }

        /// <summary>
        ///     事件报告者
        /// </summary>
        public string EventReportor
        {
            get => getProperty<string>("EventReportor");
            set => setProperty("EventReportor", value, 50);
        }

        /// <summary>
        ///     事件描述
        /// </summary>
        public string EventRemark
        {
            get => getProperty<string>("EventRemark");
            set => setProperty("EventRemark", value, 2147483647);
        }

        /// <summary>
        ///     事件当事人
        /// </summary>
        public string Eventor
        {
            get => getProperty<string>("Eventor");
            set => setProperty("Eventor", value, 50);
        }

        /// <summary>
        ///     损失情况
        /// </summary>
        public string EventLosses
        {
            get => getProperty<string>("EventLosses");
            set => setProperty("EventLosses", value, 100);
        }

        /// <summary>
        ///     0 未处理  1 已处理
        /// </summary>
        public bool EventStatus
        {
            get => getProperty<bool>("EventStatus");
            set => setProperty("EventStatus", value);
        }

        /// <summary>
        ///     处理情况
        /// </summary>
        public string EventProcessInfo
        {
            get => getProperty<string>("EventProcessInfo");
            set => setProperty("EventProcessInfo", value, 100);
        }

        /// <summary>
        ///     事件类型
        /// </summary>
        public short EventType
        {
            get => getProperty<short>("EventType");
            set => setProperty("EventType", value);
        }

        /// <summary>
        ///     事件等级
        /// </summary>
        public short EventGrade
        {
            get => getProperty<short>("EventGrade");
            set => setProperty("EventGrade", value);
        }

        /// <summary>
        ///     0 未审核 1已审核
        /// </summary>
        public bool EventCheck
        {
            get => getProperty<bool>("EventCheck");
            set => setProperty("EventCheck", value);
        }

        /// <summary>
        /// </summary>
        public string EventCheckor
        {
            get => getProperty<string>("EventCheckor");
            set => setProperty("EventCheckor", value, 50);
        }

        /// <summary>
        ///     审核时间
        /// </summary>
        public DateTime EventCheckTime
        {
            get => getProperty<DateTime>("EventCheckTime");
            set => setProperty("EventCheckTime", value);
        }

        /// <summary>
        ///     审核情况
        /// </summary>
        public string EventCheckInfo
        {
            get => getProperty<string>("EventCheckInfo");
            set => setProperty("EventCheckInfo", value, 2147483647);
        }


        protected override void SetFieldNames()
        {
            PropertyNames = new[]
            {
                "EventID", "EventCode", "EventPlace", "EventTime", "EventReportor", "EventRemark", "Eventor",
                "EventLosses", "EventStatus", "EventProcessInfo", "EventType", "EventGrade", "EventCheck",
                "EventCheckor", "EventCheckTime", "EventCheckInfo"
            };
        }
    }
}