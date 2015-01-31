Public Class frmEntityFromSQL
    Dim _isOK As Boolean = False

    '查询将会保存在EntitySqlMap.config文件中，文件配置节有<命名空间><查询名字><SQL>
    Public Property MapSQL() As String
        Get
            Return Me.txtSQL.Text
        End Get
        Set(ByVal value As String)
            Me.txtSQL.Text = value.Replace(vbCrLf, vbLf).Replace(vbLf, vbCrLf)
        End Set
    End Property

    Public Property TableName() As String
        Get
            Return Me.txtTableName.Text
        End Get
        Set(ByVal value As String)
            Me.txtTableName.Text = value
        End Set
    End Property

    Public Property IsCheckedSQL() As Boolean
        Get
            Return Me.CheckedSQL.Checked
        End Get
        Set(ByVal value As Boolean)
            Me.CheckedSQL.Checked = value
        End Set
    End Property

    Public Property ClassName() As String
        Get
            Return Me.txtClassName.Text
        End Get
        Set(ByVal value As String)
            Me.txtClassName.Text = value
        End Set
    End Property

    Public Property ClassNamespace() As String
        Get
            Return Me.txtNamespace.Text
        End Get
        Set(ByVal value As String)
            Me.txtNamespace.Text = value
        End Set
    End Property

    Public Property EntityMapTypeString() As String
        Get
            Return txtMapType.Text
        End Get
        Set(ByVal value As String)
            txtMapType.Text = value
        End Set
    End Property

    ''' <summary>
    ''' 是否确定根据查询生成实体类
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsOK() As Boolean
        Get
            Return Me._isOK
        End Get
        Protected Set(ByVal value As Boolean)
            Me._isOK = value
        End Set
    End Property

    Private Sub frmEntityFromSQL_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


    End Sub



    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.IsOK = False
        If Me.CheckedSQL.Checked Then
            If Me.txtTableName.Text.Trim = "" Then
                MsgBox("查询的名称不能为空！", MsgBoxStyle.Exclamation, "SQL-实体类映射")
                Me.txtTableName.Focus()
                Return
            End If
            If Not Me.CheckedSQL.Checked Then
                MsgBox("SQL语句未验证！", MsgBoxStyle.Exclamation, "SQL-实体类映射")
                Return
            End If
            If Me.txtClassName.Text.Trim = "" Then
                MsgBox("实体类名称不能为空！", MsgBoxStyle.Exclamation, "SQL-实体类映射")
                Me.txtClassName.Focus()
                Return
            End If
            If Me.txtNamespace.Text.Trim = "" Then
                MsgBox("名称空间不能为空！", MsgBoxStyle.Exclamation, "SQL-实体类映射")
                Me.txtNamespace.Focus()
                Return
            End If

            Me.Close()
            Me.IsOK = True
        Else
            MsgBox("SQL 查询还未验证！", MsgBoxStyle.Exclamation, "SQL-实体类映射")

        End If


    End Sub
End Class