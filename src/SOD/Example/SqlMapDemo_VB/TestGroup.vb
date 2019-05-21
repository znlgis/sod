'使用该程序前请先引用程序集：PWMIS.Core，并且下面定义的名称空间前缀不要使用ＰＷＭＩＳ，更多信息，请查看 http://www.pwmis.com/sqlmap 
' ========================================================================
' Copyright(c) 2008-2010 公司名称, All Rights Reserved.
' ========================================================================

Imports System.Collections.Generic
Imports PWMIS.DataMap.Entity
Imports PWMIS.DataMap.SqlMap
Imports PWMIS.Common

NameSpace SqlMapDemo.SqlMapDAL
''' <summary>
''' 文件名：TestSqlMapClass.vb
''' 类　名：TestSqlMapClass
''' 版　本：1.0
''' 创建时间：2018/12/9 22:13:34
''' 用途描述：SQL-MAP示例测试程序
''' 其它信息：该文件由 PDF.NET Code Maker 自动生成，修改前请先备份！
''' </summary>
Partial Public Class TestSqlMapClass
    Inherits DBMapper

    Sub New()
        Mapper.CommandClassName = "TestGroup"
        Mapper.EmbedAssemblySource="SqlMapDemo_VB,SqlMapDemo.SqlMap.config" 'SQL-MAP文件嵌入的程序集名称和资源名称，如果有多个SQL-MAP文件建议在此指明。
    End Sub


    ''' <summary>
    ''' 找出每一个系的最高分，并且按系编号，学生编号升序排列
    ''' </summary>
    ''' <returns></returns>
    Function QueryStudentSores( ) As DataSet
        With Mapper
            '获取命令信息
             Dim cmdInfo As CommandInfo = .GetCommandInfo("QueryStudentSores")
            '执行查询
            Return CurrentDataBase.ExecuteDataSet(CurrentDataBase.ConnectionString, cmdInfo.CommandType, cmdInfo.CommandText ,Nothing)
        End With
    End Function

    ''' <summary>
    ''' 查询所属系的学生信息
    ''' </summary>
    ''' <param name="DID"></param>
    ''' <returns></returns>
    Function GetStudent(ByVal DID As Int32  ) As DataSet
        With Mapper
            '获取命令信息
             Dim cmdInfo As CommandInfo = .GetCommandInfo("GetStudent")
            '参数赋值，推荐使用该种方式；
            cmdInfo.DataParameters(0).Value = DID
            '参数赋值，使用命名方式；
            'cmdInfo.SetParameterValue("@DID", DID)
            '执行查询
            Return CurrentDataBase.ExecuteDataSet(CurrentDataBase.ConnectionString, cmdInfo.CommandType, cmdInfo.CommandText , cmdInfo.DataParameters)
        End With
    End Function

    ''' <summary>
    ''' 查询所属系的学生成绩
    ''' </summary>
    ''' <param name="Category"></param>
    ''' <returns></returns>
    Function GetStudentScore(ByVal Category As String  ) As DataSet
        With Mapper
            '获取命令信息
             Dim cmdInfo As CommandInfo = .GetCommandInfo("GetStudentScore")
            '参数赋值，推荐使用该种方式；
            cmdInfo.DataParameters(0).Value = Category
            '参数赋值，使用命名方式；
            'cmdInfo.SetParameterValue("@Category", Category)
            '执行查询
            Return CurrentDataBase.ExecuteDataSet(CurrentDataBase.ConnectionString, cmdInfo.CommandType, cmdInfo.CommandText , cmdInfo.DataParameters)
        End With
    End Function

    ''' <summary>
    ''' 查询学生的成绩
    ''' </summary>
    ''' <param name="StuId"></param>
    ''' <returns></returns>
    Function GetStudentScore2(ByVal StuId As Int32  ) As List(Of StudentScore)
        With Mapper
            '获取命令信息
             Dim cmdInfo As CommandInfo = .GetCommandInfo("GetStudentScore2")
            '参数赋值，推荐使用该种方式；
            cmdInfo.DataParameters(0).Value = StuId
            '参数赋值，使用命名方式；
            'cmdInfo.SetParameterValue("@StuId", StuId)
            '执行查询
            Return MapObjectList(Of StudentScore)( CurrentDataBase.ExecuteReader(CurrentDataBase.ConnectionString, cmdInfo.CommandType, cmdInfo.CommandText , cmdInfo.DataParameters))
        End With
    End Function


End Class

End NameSpace 
