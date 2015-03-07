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
using System.Data;

namespace PWMIS.DataForms.Adapter
{
	/// <summary>
	/// 智能窗体命令对象，使用该对前必须确保对应的数据表有主建和插入时候的自增列
	/// </summary>
	public class IBCommand
	{
		string _insertCmd=string.Empty ;
		string _updateCmd=string.Empty ;
		string _tableName=string.Empty ;
		string _selectCmd=string.Empty ;
		string _deleteCmd=string.Empty ;
        string _guidpk = string.Empty;
		int _id=0;

		/// <summary>
		/// 默认构造函数
		/// </summary>
		public IBCommand()
		{
		
		}

		/// <summary>
		/// 指定一个数据表初始化该类
		/// </summary>
		/// <param name="tableName"></param>
		public IBCommand(string tableName)
		{
			_tableName=tableName;
		}

		/// <summary>
		/// 插入数据命令
		/// </summary>
		public string InsertCommand
		{
			get{return _insertCmd ;}
			set{_insertCmd =value; }
		}

		/// <summary>
		/// 更新数据命令
		/// </summary>
		public string UpdateCommand
		{
			get{return _updateCmd ;}
			set{_updateCmd =value;}
		}

		/// <summary>
		/// 选择数据命令
		/// </summary>
		public string SelectCommand
		{
			get{return _selectCmd ;}
			set{_selectCmd =value;}
		}

		/// <summary>
		/// 删除数据命令
		/// </summary>
		public string DeleteCommand
		{
			get{return _deleteCmd ;}
			set{_deleteCmd =value;}
		}

		/// <summary>
		/// 表名称
		/// </summary>
		public string TableName
		{
			get{return _tableName ;}
			set{_tableName =value;}
		}

		/// <summary>
		/// 插入标识，用于数据库的自增列，等于0表示还未插入，大鱼0表示已经插入过数据自增标识值，等于-2表示非数字类型的主键。
		/// </summary>
		public int InsertedID
		{
			get{return _id;}
			set{_id=value;}
		}

       
        /// <summary>
        /// GUID 主键名称
        /// </summary>
        public string GuidPrimaryKey
        {
            get { return _guidpk; }
            set { _guidpk = value; }
        }

        /// <summary>
        /// 对应的查询参数数组，用于更新和删除
        /// </summary>
        public IDataParameter[] Parameters { get; set; }
        /// <summary>
        /// 用于插入数据的参数数组
        /// </summary>
        public IDataParameter[] InsertParameters { get; set; }

	}
}
