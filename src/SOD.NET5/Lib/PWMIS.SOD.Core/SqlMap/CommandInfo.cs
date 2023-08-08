﻿using System;
using System.Data;
using PWMIS.Common;

namespace PWMIS.DataMap.SqlMap
{
    /// <summary>
    ///     命令信息，支持处理分页SQL语句
    /// </summary>
    public class CommandInfo
    {
        private string _sqlPageText;

        private string _SqlText;

        /// <summary>
        ///     设置记录总数，只有该值大于零，才会启用分页功能。
        /// </summary>
        public int AllCount;

        /// <summary>
        ///     获取或者设置数据库类型
        /// </summary>
        public DBMSType DataBaseType;

        /// <summary>
        ///     获取或者分页的当前页码，默认为1
        /// </summary>
        public int PageNumber = 1;

        /// <summary>
        ///     获取或者设置分页大小，默认为10
        /// </summary>
        public int PageSize = 10;

        protected internal string ParaChar;

        /// <summary>
        ///     使用指定的数据库类型初始化本类
        /// </summary>
        /// <param name="dbType"></param>
        public CommandInfo(DBMSType dbType)
        {
            DataBaseType = dbType;
        }

        /// <summary>
        ///     获取命令类型
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CommandType CommandType { get; protected internal set; }

        /// <summary>
        ///     获取可以执行的SQL命令文本
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string CommandText
        {
            get
            {
                if (AllCount > 0)
                {
                    if (_sqlPageText == null)
                        _sqlPageText = SQLPage.MakeSQLStringByPage(DataBaseType, _SqlText, "", PageSize, PageNumber,
                            AllCount);
                    return _sqlPageText;
                }

                return _SqlText;
            }
            protected internal set
            {
                _SqlText = value;
                _sqlPageText = null;
            }
        }

        /// <summary>
        ///     获取参数数组
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public IDataParameter[] DataParameters { get; protected internal set; }

        /// <summary>
        ///     设置分页信息，pageInfo[ 0]=记录数量，pageInfo[ 1]=页码，pageInfo[ 2]=页大小
        /// </summary>
        /// <param name="pageInfo">分页信息数组</param>
        public void SetPageInfo(int[] pageInfo)
        {
            switch (pageInfo.Length)
            {
                case 1:
                    AllCount = pageInfo[0];
                    break;
                case 2:
                    AllCount = pageInfo[0];
                    PageNumber = pageInfo[1];
                    break;
                case 3:
                    AllCount = pageInfo[0];
                    PageNumber = pageInfo[1];
                    PageSize = pageInfo[2];
                    break;
            }
        }

        /// <summary>
        ///     给指定的参数赋值
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <remarks></remarks>
        public void SetParameterValue(string paramName, object paramValue)
        {
            for (var I = 0; I <= DataParameters.Length - 1; I++)
                if (DataParameters[I].ParameterName == paramName)
                {
                    DataParameters[I].Value = paramValue;
                    return;
                }

            throw new Exception("没有找到指定的参数名：" + paramName);
        }

        /// <summary>
        ///     指定参数替换类型的参数赋值方法,paramName 不带Me.ParaChar 或者 "#"限定符
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="paramValue">参数值</param>
        /// <param name="paramType">参数类型</param>
        /// <remarks></remarks>
        public void SetParameterValue(string paramName, string paramValue, enumParamType paramType)
        {
            if (paramType == enumParamType.DataParameter)
            {
                SetParameterValue(paramName, paramValue);
            }
            else
            {
                if ((_SqlText == null) | (_SqlText == string.Empty)) throw new Exception("无法设置替换参数，可能命令没有初始化．");
                _SqlText = _SqlText.Replace(ParaChar + "%" + paramName + "%", paramValue);
            }
        }
    }
}