Imports System.Xml.Serialization
Imports PWMIS.Common
Imports PWMIS.DataProvider.Data

Module Module1
    Public CurrentDataBase As AdoHelper
End Module

''' <summary>
'''     数据连接类，用于系统数据连接配置管理
''' </summary>
''' <remarks></remarks>
<Serializable>
Public Class DataConnection
    Dim _CurrAdoHelper As AdoHelper

    ''' <summary>
    '''     当前数据库访问对象
    ''' </summary>
    ''' <remarks></remarks>
    <XmlIgnore>
    Public Property CurrAdoHelper As AdoHelper
        Get
            Return _CurrAdoHelper
        End Get
        Set
            _CurrAdoHelper = value
        End Set
    End Property

    Dim _IsSelected As Boolean

    ''' <summary>
    '''     是否有效
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Enabled As Boolean
        Get
            Return _IsSelected
        End Get
        Set
            _IsSelected = value
        End Set
    End Property

    Dim _DbType As DBMSType

    ''' <summary>
    '''     数据库类型
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DbType As DBMSType
        Get
            Return _DbType
        End Get
        Set
            _DbType = value
        End Set
    End Property

    Dim _ConnectionStrng As String

    ''' <summary>
    '''     连接字符串
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ConnectionStrng As String
        Get
            Return _ConnectionStrng
        End Get
        Set
            _ConnectionStrng = value
        End Set
    End Property

    Dim _Provider As String

    ''' <summary>
    '''     数据访问提供程序，格式：类全名称,程序集名称
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Provider As String
        Get
            Return _Provider
        End Get
        Set
            _Provider = value
        End Set
    End Property
End Class
