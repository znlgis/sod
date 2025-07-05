
//-----------------------------------------------------------------
// SOD Framework (https://github.com/znlgis/sod)
// EntityCreateTool (Ver 1.1 Date:2023-5-1) Created SOD Entity File.
// Created Date: 2025-1-1 10:10:10
// Please do not modify this file.
//-----------------------------------------------------------------
using System;
using Sod.Pwmis.com.Interface;

namespace Sod.Pwmis.com.DTO
{
    /// <summary>
    /// 8轮巡策略 
    /// </summary>
    public class SodDemoDto : ISodDemo
    {
        public SodDemoDto()
        {
           
        }

        
        /// <summary>
        /// ID号 
        /// </summary>
        /// <remarks>
        /// 自增
        /// </remarks>

        public int ID
        {
            get ;
            set ;
        }


        /// <summary>
        /// 名称 
        /// </summary>
        public string Name
        {
            get ;
            set ;
        }


        /// <summary>
        /// 间隔 
        /// </summary>
        /// <remarks>
        /// 单位：分钟
        /// </remarks>

        public int Interval
        {
            get ;
            set ;
        }


        /// <summary>
        /// 记录时间 
        /// </summary>
        /// <remarks>
        /// 创建或者修改时间
        /// </remarks>

        public DateTime AtTime
        {
            get ;
            set ;
        }


       
    }
}
