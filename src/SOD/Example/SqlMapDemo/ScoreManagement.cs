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
/// 文件名：ScoreManagement.cs
/// 类　名：ScoreManagement
/// 版　本：1.0
/// 创建时间：2018/12/13 16:46:16
/// 用途描述：分数管理
/// 其它信息：该文件由 PDF.NET Code Maker 自动生成，修改前请先备份！
/// </summary>
public partial class ScoreManagement
    : DBMapper 
{
	/// <summary>
	/// 默认构造函数
	/// </summary>
    public ScoreManagement()
    {
        Mapper.CommandClassName = "ScoreManagement";
        //CurrentDataBase.DataBaseType=DataBase.enumDataBaseType.SqlServer;
        Mapper.EmbedAssemblySource="SqlMapDemo,SqlMapDemo.SqlMap.config";//SQL-MAP文件嵌入的程序集名称和资源名称，如果有多个SQL-MAP文件建议在此指明。
    }


    /// <summary>
    /// 查询所有学生信息
    /// </summary>
    /// <returns></returns>
    public DataSet GetAllStudents( ) 
    { 
            //获取命令信息
            CommandInfo cmdInfo=Mapper.GetCommandInfo("GetAllStudents");
            //执行查询
            return CurrentDataBase.ExecuteDataSet(CurrentDataBase.ConnectionString, cmdInfo.CommandType, cmdInfo.CommandText ,null);
        //
    }//End Function

    /// <summary>
    /// 查询所属系的学生信息
    /// </summary>
    /// <param name="DID"></param>
    /// <returns></returns>
    public DataSet GetStudent(Int32 DID   ) 
    { 
            //获取命令信息
            CommandInfo cmdInfo=Mapper.GetCommandInfo("GetStudent");
            //参数赋值，推荐使用该种方式；
            cmdInfo.DataParameters[0].Value = DID;
            //参数赋值，使用命名方式；
            //cmdInfo.SetParameterValue("@DID", DID);
            //执行查询
            return CurrentDataBase.ExecuteDataSet(CurrentDataBase.ConnectionString, cmdInfo.CommandType, cmdInfo.CommandText , cmdInfo.DataParameters);
        //
    }//End Function

    /// <summary>
    /// 查询所属系的学生成绩
    /// </summary>
    /// <param name="Category"></param>
    /// <returns></returns>
    public DataSet GetStudentScore(String Category   ) 
    { 
            //获取命令信息
            CommandInfo cmdInfo=Mapper.GetCommandInfo("GetStudentScore");
            //参数赋值，推荐使用该种方式；
            cmdInfo.DataParameters[0].Value = Category;
            //参数赋值，使用命名方式；
            //cmdInfo.SetParameterValue("@Category", Category);
            //执行查询
            return CurrentDataBase.ExecuteDataSet(CurrentDataBase.ConnectionString, cmdInfo.CommandType, cmdInfo.CommandText , cmdInfo.DataParameters);
        //
    }//End Function

    /// <summary>
    /// 查询学生的成绩，映射结果到DTO列表
    /// </summary>
    /// <param name="StuId"></param>
    /// <returns></returns>
    public List<SqlMapDemo.StudentScore> GetStudentScore2(Int32 StuId   ) 
    { 
            //获取命令信息
            CommandInfo cmdInfo=Mapper.GetCommandInfo("GetStudentScore2");
            //参数赋值，推荐使用该种方式；
            cmdInfo.DataParameters[0].Value = StuId;
            //参数赋值，使用命名方式；
            //cmdInfo.SetParameterValue("@StuId", StuId);
            //执行查询
            return MapObjectList<SqlMapDemo.StudentScore>( CurrentDataBase.ExecuteReader(CurrentDataBase.ConnectionString, cmdInfo.CommandType, cmdInfo.CommandText , cmdInfo.DataParameters));
        //
    }//End Function

    /// <summary>
    /// 查询学生的成绩，映射结果到实体类列表
    /// </summary>
    /// <param name="StuId"></param>
    /// <returns></returns>
    public List<SqlMapDemo.ScoreEntity> GetStudentScoreEntitys(Int32 StuId   ) 
    { 
            //获取命令信息
            CommandInfo cmdInfo=Mapper.GetCommandInfo("GetStudentScoreEntitys");
            //参数赋值，推荐使用该种方式；
            cmdInfo.DataParameters[0].Value = StuId;
            //参数赋值，使用命名方式；
            //cmdInfo.SetParameterValue("@StuId", StuId);
            //执行查询
            return EntityQuery<SqlMapDemo.ScoreEntity>.QueryList( CurrentDataBase.ExecuteReader(CurrentDataBase.ConnectionString, cmdInfo.CommandType, cmdInfo.CommandText , cmdInfo.DataParameters));
        //
    }//End Function

    /// <summary>
    /// 增加学生
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="DeptId"></param>
    /// <returns></returns>
    public Int32 AddStudent(String Name  , Int32 DeptId   ) 
    { 
            //获取命令信息
            CommandInfo cmdInfo=Mapper.GetCommandInfo("InsertStudent");
            //参数赋值，推荐使用该种方式；
            cmdInfo.DataParameters[0].Value = Name;
            cmdInfo.DataParameters[1].Value = DeptId;
            //参数赋值，使用命名方式；
            //cmdInfo.SetParameterValue("@Name", Name);
            //cmdInfo.SetParameterValue("@DeptId", DeptId);
            //执行查询
            return CurrentDataBase.ExecuteNonQuery(CurrentDataBase.ConnectionString, cmdInfo.CommandType, cmdInfo.CommandText , cmdInfo.DataParameters);
        //
    }//End Function


}//End Class

}//End NameSpace 
