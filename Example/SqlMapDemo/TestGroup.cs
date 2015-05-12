//使用该程序前请先引用程序集：PWMIS.Core，并且下面定义的名称空间前缀不要使用ＰＷＭＩＳ，更多信息，请查看 http://www.pwmis.com/sqlmap 
// ========================================================================
// Copyright(c) 2008-2010 公司名称, All Rights Reserved.
// ========================================================================
using System;
using System.Data;
using System.Collections.Generic;
using PWMIS.DataMap.SqlMap;
using PWMIS.DataMap.Entity;
using PWMIS.Common;

namespace SqlMapDemo.SqlMapDAL
{
/// <summary>
/// 文件名：TestSqlMapClass.cs
/// 类　名：TestSqlMapClass
/// 版　本：1.0
/// 创建时间：2015/5/12 17:16:32
/// 用途描述：SQL-MAP示例测试程序
/// 其它信息：该文件由 PDF.NET Code Maker 自动生成，修改前请先备份！
/// </summary>
public partial class TestSqlMapClass
    : DBMapper 
{
	/// <summary>
	/// 默认构造函数
	/// </summary>
    public TestSqlMapClass()
    {
        Mapper.CommandClassName = "TestGroup";
        //CurrentDataBase.DataBaseType=DataBase.enumDataBaseType.SqlServer;
        Mapper.EmbedAssemblySource="SqlMapDemo,SqlMapDemo.SqlMap.config";//SQL-MAP文件嵌入的程序集名称和资源名称，如果有多个SQL-MAP文件建议在此指明。
    }


    /// <summary>
    /// 找出每一个系的最高分，并且按系编号，学生编号升序排列
    /// </summary>
    /// <returns></returns>
    public DataSet QueryStudentSores( ) 
    { 
            //获取命令信息
            CommandInfo cmdInfo=Mapper.GetCommandInfo("QueryStudentSores");
            //执行查询
            return CurrentDataBase.ExecuteDataSet(CurrentDataBase.ConnectionString, cmdInfo.CommandType, cmdInfo.CommandText ,null);
        //
    }//End Function


}//End Class

}//End NameSpace 
