''' <summary>
''' 代码生成配置项
''' </summary>
''' <remarks></remarks>
Public Class GenerateConfigItem

    Public Sub New()

    End Sub

    Public Sub New(ByVal sqlMapConfigFile As String, ByVal outPutPath As String, ByVal rootNameSpace As String)

        Me.SqlMapConfigFile = sqlMapConfigFile
        Me.OutPutPath = outPutPath
        Me.RootNameSpace = rootNameSpace

    End Sub

    Private _SqlMapConfigFile As String
    Public Property SqlMapConfigFile() As String
        Get
            Return _SqlMapConfigFile
        End Get
        Set(ByVal value As String)
            _SqlMapConfigFile = value
        End Set
    End Property

    Private _OutPutPath As String
    Public Property OutPutPath() As String
        Get
            Return _OutPutPath
        End Get
        Set(ByVal value As String)
            _OutPutPath = value
        End Set
    End Property

    Private _RootNameSpace As String
    Public Property RootNameSpace() As String
        Get
            Return _RootNameSpace
        End Get
        Set(ByVal value As String)
            _RootNameSpace = value
        End Set
    End Property

End Class
