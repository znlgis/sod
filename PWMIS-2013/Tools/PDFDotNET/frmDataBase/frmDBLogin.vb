Public Class frmDBLogin

    Dim oldHeight As Integer
    Dim shortHeight As Integer
    Dim DBEnginType As String() = {"SQLServer数据库服务", "SQLServer数据库文件", "SQLServer CE", "Oracle数据库服务", "Access", "其它数据库驱动程序"}
    Dim currDbmsType As PWMIS.Common.DBMSType

    ''' <summary>
    ''' 获取或者设置连接字符串
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ConnectionString() As String
        Get
            Return txtConnStr.Text
        End Get
        Set(ByVal value As String)
            txtConnStr.Text = value
        End Set
    End Property

    ''' <summary>
    ''' 获取数据提供程序，默认为空，将使用PDF.NET内置的提供程序
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Provider() As String
        Get
            If Me.currDbmsType <> PWMIS.Common.DBMSType.UNKNOWN Then
                Return txtProviderName.Text
            Else
                Return ""
            End If
        End Get
    End Property

    ''' <summary>
    ''' 获取服务器名称
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ServerName() As String
        Get
            Return Me.txtServerName.Text
        End Get
    End Property

    ''' <summary>
    ''' 当前选择的数据库类型
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property DBMSType() As PWMIS.Common.DBMSType
        Get
            'Dim _DbEnginType As PWMIS.Common.DBMSType
            'Select Case Me.cmbDbEngine.SelectedValue
            '    Case "SQLServer数据库服务", "SQLServer数据库文件"
            '        _DbEnginType = PWMIS.Common.DBMSType.SqlServer
            '    Case "SQLServer CE"
            '        _DbEnginType = PWMIS.Common.DBMSType.SqlServerCe
            '    Case "Oracle数据库服务"
            '        _DbEnginType = PWMIS.Common.DBMSType.Oracle
            '    Case "Access"
            '        _DbEnginType = PWMIS.Common.DBMSType.Access
            '    Case "MySQL"
            '        _DbEnginType = PWMIS.Common.DBMSType.MySql
            '    Case Else
            '        _DbEnginType = PWMIS.Common.DBMSType.UNKNOWN
            'End Select
            'Return _DbEnginType
            Return Me.currDbmsType
        End Get
    End Property

    Private Sub chkMoreInfo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMoreInfo.CheckedChanged
        Me.Height = IIf(chkMoreInfo.Checked, oldHeight, shortHeight)
        If chkMoreInfo.Checked Then
            Me.txtConnStr.Text = MakeConnectionString()
        End If
    End Sub


    Private Sub frmDBLogin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        oldHeight = Me.Height
        shortHeight = Me.btnOK.Location.Y + Me.btnOK.Height + 40

        Me.Height = shortHeight
        Me.cmbDbEngine.DataSource = Me.DBEnginType
        Me.btnFileBrowser.Visible = False
    End Sub

    Private Sub cmbDbEngine_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDbEngine.SelectedIndexChanged
        Me.txtLogName.Enabled = True
        Me.txtPwd.Enabled = True
        Me.btnProviderBrowser.Enabled = False

        Select Case Me.cmbDbEngine.SelectedValue
            Case "SQLServer数据库服务"
                Me.lblServerName.Text = "服务器地址："
                Me.lblLoginType.Text = "登录方式："
                Me.cmbLoginType.Enabled = True
                Me.cmbLoginType.DataSource = New String() {"Windows集成验证", "SQLServer 身份验证"}
                Me.btnFileBrowser.Visible = False
                Me.txtServerName.Text = ""
                Me.currDbmsType = PWMIS.Common.DBMSType.SqlServer
                Me.txtProviderName.Text = "PWMIS.DataProvider.Data.SqlServer,PWMIS.Core"
            Case "SQLServer数据库文件"
                Me.lblServerName.Text = "文件地址："
                Me.lblLoginType.Text = "登录方式："
                Me.btnFileBrowser.Visible = True
                Me.cmbLoginType.DataSource = New String() {"Windows集成验证", "SQLServer 身份验证"}
                Me.cmbLoginType.Enabled = True
                Me.txtServerName.Text = ""
                Me.OpenFileDialog1.Filter = "数据库文件*.mdf|*.mdf|所有文件|*.*"
                Me.currDbmsType = PWMIS.Common.DBMSType.SqlServer
                Me.txtProviderName.Text = "PWMIS.DataProvider.Data.SqlServer,PWMIS.Core"
            Case "SQLServer CE"
                Me.lblServerName.Text = "文件地址："
                Me.lblLoginType.Text = "登录方式："
                Me.btnFileBrowser.Visible = True
                Me.cmbLoginType.DataSource = New String() {"Windows集成验证"}
                Me.cmbLoginType.Enabled = True
                Me.txtPwd.Enabled = True
                Me.txtServerName.Text = ""
                Me.OpenFileDialog1.Filter = "数据库文件*.sf|*sdf|所有文件|*.*"
                Me.currDbmsType = PWMIS.Common.DBMSType.SqlServerCe
                Me.txtProviderName.Text = "PWMIS.DataProvider.Data.SqlServerCe,PWMIS.Core"
            Case "Access"
                Me.lblServerName.Text = "文件地址："
                Me.btnFileBrowser.Visible = True
                Me.lblLoginType.Text = "版本："
                Me.cmbLoginType.DataSource = New String() {"Access2000-2003", "Access2007"}
                Me.cmbLoginType.Enabled = True
                Me.txtServerName.Text = ""
                Me.OpenFileDialog1.Filter = "Access2000-2003|*.mdb|Access2007|*.accdb|所有文件|*.*"
                Me.currDbmsType = PWMIS.Common.DBMSType.Access
                Me.txtProviderName.Text = "PWMIS.DataProvider.Data.Access,PWMIS.Core"
            Case "Oracle数据库服务"
                Me.lblServerName.Text = "服务名："
                Me.lblLoginType.Text = "登录类型："
                Me.cmbLoginType.Enabled = True
                Me.cmbLoginType.DataSource = New String() {"Normal", "SysDBA"}
                Me.btnFileBrowser.Visible = False
                Me.txtServerName.Text = ""
                Me.currDbmsType = PWMIS.Common.DBMSType.Oracle
                Me.txtProviderName.Text = "PWMIS.DataProvider.Data.Oracle,PWMIS.Core"
            Case Else
                '其它数据库驱动程序
                'Me.lblServerName.Text = "数据提供程序："
                Me.btnFileBrowser.Visible = True
                Me.txtServerName.Text = ""
                Me.OpenFileDialog1.Filter = "AdoHelp提供程序|*.dll"
                Me.cmbLoginType.DataSource = Nothing
                Me.cmbLoginType.Enabled = False
                Me.txtLogName.Enabled = True
                Me.txtPwd.Enabled = True
                Me.txtProviderName.Text = ""
                Me.btnFileBrowser.Enabled = False
                Me.btnProviderBrowser.Enabled = True
        End Select

    End Sub

    Private Sub ChangeUIbyDbmsType(ByVal dbmsType As PWMIS.Common.DBMSType)
        Select Case dbmsType
            Case PWMIS.Common.DBMSType.MySql, PWMIS.Common.DBMSType.PostgreSQL
                Me.lblServerName.Text = "服务器地址："
                'Me.lblLoginType.Text = "登录类型："
                Me.cmbLoginType.Enabled = False
                'Me.cmbLoginType.DataSource = New String() {"Normal", "SysDBA"}
                Me.btnFileBrowser.Visible = False
                Me.txtServerName.Text = ""
            Case PWMIS.Common.DBMSType.SQLite
                Me.lblServerName.Text = "文件地址："
                Me.btnFileBrowser.Visible = True
                Me.cmbLoginType.Enabled = False
                Me.btnFileBrowser.Enabled = True
                Me.OpenFileDialog1.Filter = "SQLite数据库文件|*.*"
            Case Else
                Me.btnFileBrowser.Visible = True
                Me.txtServerName.Text = ""
                Me.OpenFileDialog1.Filter = "AdoHelp提供程序|*.dll"
                Me.cmbLoginType.DataSource = Nothing
                Me.cmbLoginType.Enabled = False
                Me.txtLogName.Enabled = True
                Me.txtPwd.Enabled = True
                Me.txtProviderName.Text = ""
                Me.btnFileBrowser.Enabled = False
                Me.btnProviderBrowser.Enabled = True

        End Select
    End Sub

    Private Sub btnFileBrowser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFileBrowser.Click
        Me.OpenFileDialog1.FileName = ""
        Me.OpenFileDialog1.ShowDialog()

        If Me.currDbmsType = PWMIS.Common.DBMSType.SQLite Then
            txtServerName.Text = Me.OpenFileDialog1.FileName
        ElseIf Me.cmbDbEngine.SelectedValue = "其它数据库驱动程序" And Me.OpenFileDialog1.FileName <> "" Then

        Else
            txtServerName.Text = Me.OpenFileDialog1.FileName
        End If

    End Sub

    Private Function MakeConnectionString() As String
        Dim connStr As String = ""
        Select Case Me.cmbDbEngine.SelectedValue
            Case "SQLServer数据库服务" ', "SQLServer数据库文件", "SQLServer CE"
                If Me.cmbLoginType.SelectedValue = "Windows集成验证" Then
                    connStr = "Data Source=" & txtServerName.Text & ";Integrated Security=True"
                Else
                    connStr = "server=" & txtServerName.Text & ";uid=" & txtLogName.Text & ";pwd=" & txtPwd.Text
                End If
            Case "SQLServer数据库文件"
                connStr = "Data Source=.\SQLEXPRESS;AttachDbFilename=" & txtServerName.Text & ";Connect Timeout=30;User Instance=True;"
                If Me.cmbLoginType.SelectedValue = "Windows集成验证" Then
                    connStr &= "Integrated Security=True"
                Else
                    connStr &= "uid=" & txtLogName.Text & ";pwd=" & txtPwd.Text
                End If
            Case "SQLServer CE"
                connStr = "Data Source=" & txtServerName.Text & ";Persist Security Info=True"
                If txtPwd.Enabled And txtPwd.Text <> "" Then
                    connStr &= ";Password=" & txtPwd.Text
                End If
            Case "Access"
                If Me.cmbLoginType.SelectedValue = "Access2000-2003" Then
                    connStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & Me.txtServerName.Text
                Else
                    connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Me.txtServerName.Text
                End If
                '
            Case "Oracle数据库服务"
                connStr = "Data Source=" & Me.txtServerName.Text & ";User Id=" & txtLogName.Text & ";Password=" & txtPwd.Text & ";Integrated Security=no;"
            Case "其它数据库驱动程序"
                If Me.currDbmsType = PWMIS.Common.DBMSType.MySql _
                Or Me.currDbmsType = PWMIS.Common.DBMSType.PostgreSQL Then
                    connStr = "server=" & txtServerName.Text & ";User Id=" & txtLogName.Text & ";password=" & txtPwd.Text
                ElseIf Me.currDbmsType = PWMIS.Common.DBMSType.SQLite Then
                    connStr = "Data Source=" & txtServerName.Text
                Else
                    connStr = Me.txtConnStr.Text
                End If


        End Select

        Return connStr
    End Function

    Private Sub cmbLoginType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbLoginType.SelectedIndexChanged
        If Me.cmbLoginType.SelectedValue = "Windows集成验证" Then
            Me.txtLogName.Enabled = False
            Me.txtPwd.Enabled = False
        Else
            Me.txtLogName.Enabled = True
            Me.txtPwd.Enabled = True
        End If
    End Sub

    Private Sub lnkRefesh_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkRefesh.LinkClicked
        Me.txtConnStr.Text = MakeConnectionString()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If txtServerName.Text.Trim() = "" Then
            MessageBox.Show(Me.lblServerName.Text & " 不能为空！")
        Else
            Me.txtConnStr.Text = MakeConnectionString()
            Me.Close()
        End If
        
    End Sub

    Private Sub btnTestConn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTestConn.Click

        Me.ConnectionString = MakeConnectionString()
        If Me.ConnectionString <> "" Then
            Try
                Dim db As PWMIS.DataProvider.Data.AdoHelper = PWMIS.DataProvider.Adapter.MyDB.GetDBHelperByProviderString(Me.Provider, Me.ConnectionString)
                Dim conn As IDbConnection = db.GetDbConnection()
                conn.Open()
                conn.Close()
                MessageBox.Show("测试成功！", "测试连接", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "测试连接", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

        End If
    End Sub

    Private Sub btnCacle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCacle.Click
        Me.ConnectionString = ""
        Me.Close()

    End Sub

    Private Sub btnProviderBrowser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProviderBrowser.Click
        Me.OpenFileDialog1.InitialDirectory = Application.StartupPath
        Me.OpenFileDialog1.FileName = ""
        Me.OpenFileDialog1.ShowDialog()

        If Me.cmbDbEngine.SelectedValue = "其它数据库驱动程序" And Me.OpenFileDialog1.FileName <> "" Then
            Dim helperName As String = ""
            Dim adoHelperType As Type = GetType(PWMIS.DataProvider.Data.AdoHelper)
            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.LoadFrom(Me.OpenFileDialog1.FileName)
            Dim find As Boolean = False
            txtServerName.Text = ""

            For Each t As Type In ass.GetTypes()
                Dim baseType As Type = t.BaseType
                If baseType IsNot Nothing Then
                    If baseType.FullName = "PWMIS.DataProvider.Data.AdoHelper" Then
                        helperName = t.FullName
                        If baseType.AssemblyQualifiedName = adoHelperType.AssemblyQualifiedName Then
                            find = True
                            Me.txtProviderName.Text = helperName & "," & System.IO.Path.GetFileNameWithoutExtension(Me.OpenFileDialog1.FileName)
                            If System.IO.Path.GetDirectoryName(Me.OpenFileDialog1.FileName) = Application.StartupPath Then
                                Dim helper As PWMIS.DataProvider.Data.AdoHelper = Activator.CreateInstance(t)
                                Me.currDbmsType = helper.CurrentDBMSType
                                ChangeUIbyDbmsType(Me.currDbmsType)

                            Else
                                MessageBox.Show("请将选定的程序集相关的其它附属程序集复制到当前程序的运行目录：" & Application.StartupPath)
                            End If
                            Exit For
                        Else
                            MessageBox.Show("选定的程序集中实现AdoHelper的子类 [" & helperName & "]所在的程序集与当前程序集(PWMIS.Core.dll)不匹配，请重新编译你选定的程序集。" & vbCrLf & baseType.AssemblyQualifiedName & vbCrLf & adoHelperType.AssemblyQualifiedName)
                            Exit Sub
                        End If

                    End If
                End If
            Next
            If Not find Then
                MessageBox.Show("在选定的程序集中未找到实现[PWMIS.DataProvider.Data.AdoHelper]的子类。", "数据提供程序", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        End If

    End Sub
End Class