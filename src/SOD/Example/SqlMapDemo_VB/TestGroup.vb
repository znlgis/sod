'ʹ�øó���ǰ�������ó��򼯣�PWMIS.Core���������涨������ƿռ�ǰ׺��Ҫʹ�ãУףͣɣӣ�������Ϣ����鿴 http://www.pwmis.com/sqlmap 
' ========================================================================
' Copyright(c) 2008-2010 ��˾����, All Rights Reserved.
' ========================================================================

Imports System.Collections.Generic
Imports PWMIS.DataMap.Entity
Imports PWMIS.DataMap.SqlMap
Imports PWMIS.Common

NameSpace SqlMapDemo.SqlMapDAL
''' <summary>
''' �ļ�����TestSqlMapClass.vb
''' �ࡡ����TestSqlMapClass
''' �桡����1.0
''' ����ʱ�䣺2018/12/9 22:13:34
''' ��;������SQL-MAPʾ�����Գ���
''' ������Ϣ�����ļ��� PDF.NET Code Maker �Զ����ɣ��޸�ǰ���ȱ��ݣ�
''' </summary>
Partial Public Class TestSqlMapClass
    Inherits DBMapper

    Sub New()
        Mapper.CommandClassName = "TestGroup"
        Mapper.EmbedAssemblySource="SqlMapDemo_VB,SqlMapDemo.SqlMap.config" 'SQL-MAP�ļ�Ƕ��ĳ������ƺ���Դ���ƣ�����ж��SQL-MAP�ļ������ڴ�ָ����
    End Sub


    ''' <summary>
    ''' �ҳ�ÿһ��ϵ����߷֣����Ұ�ϵ��ţ�ѧ�������������
    ''' </summary>
    ''' <returns></returns>
    Function QueryStudentSores( ) As DataSet
        With Mapper
            '��ȡ������Ϣ
             Dim cmdInfo As CommandInfo = .GetCommandInfo("QueryStudentSores")
            'ִ�в�ѯ
            Return CurrentDataBase.ExecuteDataSet(CurrentDataBase.ConnectionString, cmdInfo.CommandType, cmdInfo.CommandText ,Nothing)
        End With
    End Function

    ''' <summary>
    ''' ��ѯ����ϵ��ѧ����Ϣ
    ''' </summary>
    ''' <param name="DID"></param>
    ''' <returns></returns>
    Function GetStudent(ByVal DID As Int32  ) As DataSet
        With Mapper
            '��ȡ������Ϣ
             Dim cmdInfo As CommandInfo = .GetCommandInfo("GetStudent")
            '������ֵ���Ƽ�ʹ�ø��ַ�ʽ��
            cmdInfo.DataParameters(0).Value = DID
            '������ֵ��ʹ��������ʽ��
            'cmdInfo.SetParameterValue("@DID", DID)
            'ִ�в�ѯ
            Return CurrentDataBase.ExecuteDataSet(CurrentDataBase.ConnectionString, cmdInfo.CommandType, cmdInfo.CommandText , cmdInfo.DataParameters)
        End With
    End Function

    ''' <summary>
    ''' ��ѯ����ϵ��ѧ���ɼ�
    ''' </summary>
    ''' <param name="Category"></param>
    ''' <returns></returns>
    Function GetStudentScore(ByVal Category As String  ) As DataSet
        With Mapper
            '��ȡ������Ϣ
             Dim cmdInfo As CommandInfo = .GetCommandInfo("GetStudentScore")
            '������ֵ���Ƽ�ʹ�ø��ַ�ʽ��
            cmdInfo.DataParameters(0).Value = Category
            '������ֵ��ʹ��������ʽ��
            'cmdInfo.SetParameterValue("@Category", Category)
            'ִ�в�ѯ
            Return CurrentDataBase.ExecuteDataSet(CurrentDataBase.ConnectionString, cmdInfo.CommandType, cmdInfo.CommandText , cmdInfo.DataParameters)
        End With
    End Function

    ''' <summary>
    ''' ��ѯѧ���ĳɼ�
    ''' </summary>
    ''' <param name="StuId"></param>
    ''' <returns></returns>
    Function GetStudentScore2(ByVal StuId As Int32  ) As List(Of StudentScore)
        With Mapper
            '��ȡ������Ϣ
             Dim cmdInfo As CommandInfo = .GetCommandInfo("GetStudentScore2")
            '������ֵ���Ƽ�ʹ�ø��ַ�ʽ��
            cmdInfo.DataParameters(0).Value = StuId
            '������ֵ��ʹ��������ʽ��
            'cmdInfo.SetParameterValue("@StuId", StuId)
            'ִ�в�ѯ
            Return MapObjectList(Of StudentScore)( CurrentDataBase.ExecuteReader(CurrentDataBase.ConnectionString, cmdInfo.CommandType, cmdInfo.CommandText , cmdInfo.DataParameters))
        End With
    End Function


End Class

End NameSpace 
