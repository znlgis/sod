
//-----------------------------------------------------------------
// SOD Framework (https://github.com/znlgis/sod)
// EntityCreateTool (Ver 1.1 Date:2023-5-1) Created SOD Entity File.
// Created Date: 2025-1-1 10:10:10
// Please do not modify this file.
//-----------------------------------------------------------------
using System;
using PWMIS.DataMap.Entity;
using Sod.Pwmis.com.Interface;

namespace Sod.Pwmis.com.Entity
{
    /// <summary>
    /// SOD示例实体类 
    /// </summary>
    public class SodDemoEntity : EntityBase, ISodDemo
    {
        public SodDemoEntity()
        {
            TableName = "SodDemo";
            IdentityName = "ID"; 
            PrimaryKeys.Add("ID");
        }

        
        /// <summary>
        /// ID号 
        /// </summary>
        /// <remarks>
        /// 自增
        /// </remarks>

        public int ID
        {
            get { return getProperty<int>("ID"); }
            set { setProperty("ID", value); }
        }


        /// <summary>
        /// 名称  
        /// </summary>
        public string Name
        {
            get { return getProperty<string>("StrategyName"); }
            set { setProperty("StrategyName", value,50); }
        }


        /// <summary>
        /// 间隔 
        /// </summary>
        /// <remarks>
        /// 单位：分钟
        /// </remarks>

        public int Interval
        {
            get { return getProperty<int>("Interval"); }
            set { setProperty("Interval", value); }
        }


        /// <summary>
        /// 记录时间 
        /// </summary>
        /// <remarks>
        /// 创建或者修改时间
        /// </remarks>

        public DateTime AtTime
        {
            get { return getProperty<DateTime>("AtTime"); }
            set { setProperty("AtTime", value); }
        }


       
    }
}
