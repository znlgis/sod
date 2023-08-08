/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap
 * ========================================================================
 * 该类的作用
 *
 * 作者：邓太华     时间：2008-10-12
 * 版本：V3.0
 *
 * 修改者：         时间：
 * 修改说明：
 * ========================================================================
 */

using System.Collections.Generic;
using System.Data;
using PWMIS.DataProvider.Adapter;
using PWMIS.DataProvider.Data;

namespace PWMIS.DataMap.SqlMap
{
    /// <summary>
    ///     SQLMAP数据处理层基类
    /// </summary>
    /// <remarks></remarks>
    public abstract class DBMapper
    {
        /// <summary>
        ///     映射到对象列表，要求对象的属性名根字段名大小写严格一致
        /// </summary>
        /// <typeparam name="T">对象类型，如一个DTO类</typeparam>
        /// <param name="reader">数据阅读器</param>
        /// <returns>对象列表</returns>
        public List<T> MapObjectList<T>(IDataReader reader) where T : class, new()
        {
            return AdoHelper.QueryList<T>(reader);
        }

        #region "公共的数据库接口"

        private AdoHelper _DB;
        private string _SqlMapFile;

        /// <summary>
        ///     初始化构造函数
        /// </summary>
        /// <remarks></remarks>
        public DBMapper()
        {
            _DB = MyDB.GetDBHelper();
            Mapper = new SqlMapper();
            //_Mapper.CommandClassName = "EngineManager"
            Mapper.DataBase = _DB;
        }

        /// <summary>
        ///     获取或设置当前使用的数据库操作对象
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public AdoHelper CurrentDataBase
        {
            get => _DB;
            set
            {
                _DB = value;
                Mapper.DataBase = _DB;
            }
        }

        /// <summary>
        ///     获取或设置SQL Map 配置文件地址(可以是一个外部配置文件或者嵌入程序集的配置文件)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string SqlMapFile
        {
            //If _SqlMapFile = "" Then
            //    _SqlMapFile = System.Configuration.ConfigurationSettings.AppSettings("SqlMapFile")
            //    If _SqlMapFile = "" Then
            //        Throw New ArgumentOutOfRangeException("SqlMapFile", "该属性没有在应用程序中设置值，请在应用程序配置文件中配置SqlMapFile项和值。 ")
            //    End If
            //End If
            get => _SqlMapFile;
            set
            {
                _SqlMapFile = value;
                Mapper.SqlMapFile = _SqlMapFile;
            }
        }

        /// <summary>
        ///     获取SQLMAP对象
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public SqlMapper Mapper { get; }

        #endregion
    }
}