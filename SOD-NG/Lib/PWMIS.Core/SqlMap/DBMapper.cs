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
using System;
using System.Collections.Generic;
using System.Text;
using PWMIS.DataProvider.Data;
using PWMIS.DataProvider.Adapter;

namespace PWMIS.DataMap.SqlMap
{
    /// <summary>
    /// SQLMAP数据处理层基类
    /// </summary>
    /// <remarks></remarks>
    public abstract class DBMapper
    {
        #region "公共的数据库接口"
        AdoHelper  _DB;
        SqlMapper _Mapper;
        string _SqlMapFile;

        /// <summary>
        /// 初始化构造函数
        /// </summary>
        /// <remarks></remarks>
        public DBMapper()
        {
            _DB = MyDB.GetDBHelper();
            _Mapper = new SqlMapper();
            //_Mapper.CommandClassName = "EngineManager"
            _Mapper.DataBase = _DB;
        }

        /// <summary>
        /// 获取或设置当前使用的数据库操作对象
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public AdoHelper   CurrentDataBase
        {
            get { return _DB; }
            set
            {
                _DB = value;
                _Mapper.DataBase = _DB;
            }
        }

        /// <summary>
        /// 获取或设置SQL Map 配置文件地址(可以是一个外部配置文件或者嵌入程序集的配置文件)
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
            get { return _SqlMapFile; }
            set
            {
                _SqlMapFile = value;
                _Mapper.SqlMapFile = _SqlMapFile;
            }
        }

        /// <summary>
        /// 获取SQLMAP对象
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public SqlMapper Mapper
        {
            get { return _Mapper; }
        }

        #endregion

    }

}
