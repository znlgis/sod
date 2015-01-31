

using System;
using PWMIS.Common;
using PWMIS.DataMap.Entity;

namespace RoadTeam.Model.CS
{
    [Serializable()]
    public partial class TbCsEvent : EntityBase
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


        protected override void SetFieldNames()
        {
            PropertyNames = new string[] { "EventID", "EventCode", "EventPlace", "EventTime", "EventReportor", "EventRemark", "Eventor", "EventLosses", "EventStatus", "EventProcessInfo", "EventType", "EventGrade", "EventCheck", "EventCheckor", "EventCheckTime", "EventCheckInfo" };
        }



        /// <summary>
        /// 事件ID
        /// </summary>
        public System.Decimal EventID
        {
            get { return getProperty<System.Decimal>("EventID"); }
            set { setProperty("EventID", value); }
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public System.String EventCode
        {
            get { return getProperty<System.String>("EventCode"); }
            set { setProperty("EventCode", value, 50); }
        }

        /// <summary>
        /// 发生位置
        /// </summary>
        public System.String EventPlace
        {
            get { return getProperty<System.String>("EventPlace"); }
            set { setProperty("EventPlace", value, 50); }
        }

        /// <summary>
        /// 发生时间
        /// </summary>
        public System.DateTime EventTime
        {
            get { return getProperty<System.DateTime>("EventTime"); }
            set { setProperty("EventTime", value); }
        }

        /// <summary>
        /// 事件报告者
        /// </summary>
        public System.String EventReportor
        {
            get { return getProperty<System.String>("EventReportor"); }
            set { setProperty("EventReportor", value, 50); }
        }

        /// <summary>
        /// 事件描述
        /// </summary>
        public System.String EventRemark
        {
            get { return getProperty<System.String>("EventRemark"); }
            set { setProperty("EventRemark", value, 2147483647); }
        }

        /// <summary>
        /// 事件当事人
        /// </summary>
        public System.String Eventor
        {
            get { return getProperty<System.String>("Eventor"); }
            set { setProperty("Eventor", value, 50); }
        }

        /// <summary>
        /// 损失情况
        /// </summary>
        public System.String EventLosses
        {
            get { return getProperty<System.String>("EventLosses"); }
            set { setProperty("EventLosses", value, 100); }
        }

        /// <summary>
        /// 0 未处理  1 已处理
        /// </summary>
        public System.Boolean EventStatus
        {
            get { return getProperty<System.Boolean>("EventStatus"); }
            set { setProperty("EventStatus", value); }
        }

        /// <summary>
        /// 处理情况
        /// </summary>
        public System.String EventProcessInfo
        {
            get { return getProperty<System.String>("EventProcessInfo"); }
            set { setProperty("EventProcessInfo", value, 100); }
        }

        /// <summary>
        /// 事件类型
        /// </summary>
        public System.Int16 EventType
        {
            get { return getProperty<System.Int16>("EventType"); }
            set { setProperty("EventType", value); }
        }

        /// <summary>
        /// 事件等级
        /// </summary>
        public System.Int16 EventGrade
        {
            get { return getProperty<System.Int16>("EventGrade"); }
            set { setProperty("EventGrade", value); }
        }

        /// <summary>
        /// 0 未审核 1已审核
        /// </summary>
        public System.Boolean EventCheck
        {
            get { return getProperty<System.Boolean>("EventCheck"); }
            set { setProperty("EventCheck", value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String EventCheckor
        {
            get { return getProperty<System.String>("EventCheckor"); }
            set { setProperty("EventCheckor", value, 50); }
        }

        /// <summary>
        /// 审核时间
        /// </summary>
        public System.DateTime EventCheckTime
        {
            get { return getProperty<System.DateTime>("EventCheckTime"); }
            set { setProperty("EventCheckTime", value); }
        }

        /// <summary>
        /// 审核情况
        /// </summary>
        public System.String EventCheckInfo
        {
            get { return getProperty<System.String>("EventCheckInfo"); }
            set { setProperty("EventCheckInfo", value, 2147483647); }
        }


    }
}
