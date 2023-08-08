﻿using System;
using PWMIS.DataMap.Entity;

namespace WebApplication2.Model
{
    /// <summary>
    ///     联系信息
    /// </summary>
    public class ContactInfo : EntityBase
    {
        public ContactInfo()
        {
            TableName = "联系信息";
            IdentityName = "编号";
            PrimaryKeys.Add("编号");
        }

        public int CID
        {
            get => getProperty<int>("编号");
            set => setProperty("编号", value);
        }

        /// <summary>
        ///     身份证号，可用于登录密码
        /// </summary>
        public string PersonID
        {
            get => getProperty<string>("身份证号");
            set => setProperty("身份证号", value, 20);
        }

        /// <summary>
        ///     姓名，可用于登录名
        /// </summary>
        public string Name
        {
            get => getProperty<string>("姓名");
            set => setProperty("姓名", value, 10);
        }

        public int HomeMemberCount
        {
            get => getProperty<int>("家属人数");
            set => setProperty("家属人数", value);
        }

        public string ContactPhone
        {
            get => getProperty<string>("联系电话");
            set => setProperty("联系电话", value, 20);
        }

        public bool NeedRoom
        {
            get => getProperty<bool>("是否预定房间");
            set => setProperty("是否预定房间", value);
        }

        public string ComeFrom
        {
            get => getProperty<string>("出发地");
            set => setProperty("出发地", value, 50);
        }

        public string ClassNum
        {
            get => getProperty<string>("原班级");
            set => setProperty("原班级", value, 10);
        }

        public string OtherInfo
        {
            get => getProperty<string>("其它信息");
            set => setProperty("其它信息", value, 200);
        }

        public DateTime AtTime
        {
            get => getProperty<DateTime>("填写时间");
            set => setProperty("填写时间", value);
        }

        protected override void SetFieldNames()
        {
            base.PropertyNames = new[]
            {
                "编号", "身份证号", "姓名", "家属人数", "联系电话", "是否预定房间",
                "出发地", "原班级", "其它信息", "填写时间"
            };
        }
    }
}