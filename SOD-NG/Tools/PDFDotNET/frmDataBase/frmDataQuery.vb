Public Class frmDataQuery
    Public CurrDataBase As PWMIS.DataProvider.Data.AdoHelper
    Public CurrDBName As String = ""
    Public CurrScriptFileName As String = ""
    Public IsGroupQuery As Boolean

    Private Const GroupQueryCfgFile As String = ".\Config\GroupQueryCfg.xml"
    ''' <summary>
    ''' 组查询的数据连接列表
    ''' </summary>
    ''' <remarks></remarks>
    Public DataConnections As New List(Of DataConnection)

    Dim blockSqlCount As Integer = 0

    Dim _currDBPath As String = ""
    ''' <summary>
    ''' 当前查询窗体使用的数据访问对象路径
    ''' </summary>
    ''' <remarks></remarks>
    Public Property CurrDBPath() As String
        Get
            Return _currDBPath
        End Get
        Set(ByVal value As String)
            Me.Text &= "@" & value
            _currDBPath = value
        End Set
    End Property
    Dim contentTextChange As Boolean

    ''' <summary>
    ''' 命令窗体
    ''' </summary>
    ''' <remarks></remarks>
    Public CommandForm As ICommand

    Public Sub New(ByVal dataBase As PWMIS.DataProvider.Data.AdoHelper)

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。

        CurrDataBase = dataBase
    End Sub

    ''' <summary>
    ''' 多数据源查询
    ''' </summary>
    ''' <param name="sql"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ExecuteGroupQuery(ByVal sql As String) As Integer
        Dim count As Integer = 0
        For Each dc As DataConnection In DataConnections
            If dc.Enabled Then
                If dc.CurrAdoHelper IsNot Nothing Then
                    dc.CurrAdoHelper.ConnectionString = dc.ConnectionStrng
                    count += BatchExecuteNoneQuery(dc.CurrAdoHelper, sql)
                    Me.txtExecuteMsg.Text &= vbCrLf & "执行成功，ConnectionString=" & dc.ConnectionStrng
                End If
            End If
        Next
        Return count
    End Function

    Private Function ExecuteNoneQuery(ByVal sql As String) As Integer
        Dim acceptCount As Integer
        If Me.chkGroupQuery.Checked Then
            Me.txtExecuteMsg.Text &= vbCrLf & "*********开始执行组查询（多数据源查询） ***********************"
            Me.txtExecuteMsg.Text &= vbCrLf & sql
            acceptCount = ExecuteGroupQuery(sql)
        Else
            acceptCount = BatchExecuteNoneQuery(CurrDataBase, sql)
        End If
        Me.txtExecuteMsg.Text &= vbCrLf & "批量查询执行成功，共" & Me.blockSqlCount & " 个语句块。"
        Return acceptCount
    End Function

    ''' <summary>
    ''' 批量执行数据库更新，将以脚本指定的分隔符分隔执行块，例如SQLServer 的Go语句（Go之后必须立刻回车，不能有其它字符）
    ''' </summary>
    ''' <param name="db"></param>
    ''' <param name="sql"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function BatchExecuteNoneQuery(ByVal db As PWMIS.DataProvider.Data.AdoHelper, ByVal sql As String) As Integer
        Dim count As Integer = 0
        blockSqlCount = 0
        If db.GetType() Is GetType(PWMIS.DataProvider.Data.SqlServer) Then
            'SQLServer 分隔处理
            sql = sql.Replace(vbCrLf, vbLf).Replace(vbLf, vbCrLf) & vbCrLf ' 防止最后一句没有加回车
            Dim spliteString = "GO" & vbCrLf
            sql = sql.Replace("go" & vbCrLf, spliteString).Replace("Go" & vbCrLf, spliteString)
            Dim sqlList As String() = sql.Split(New String() {spliteString}, StringSplitOptions.RemoveEmptyEntries)
            db.BeginTransaction()
            Try
                For Each sqlItem As String In sqlList
                    count += db.ExecuteNonQuery(sqlItem)
                    blockSqlCount += 1
                Next
                db.Commit()
            Catch ex As Exception
                db.Rollback()
                MessageBox.Show("执行T-SQL出错，已经终止操作并回滚事务，详细原因：" & ex.Message, "数据查询", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
           

        Else
            count = db.ExecuteNonQuery(sql)
        End If
        Return count
    End Function
    Private Sub ExecuteQuery(ByVal sql As String, ByVal isUpdate As Boolean, ByVal autoCheck As Boolean)
        Me.Cursor = Cursors.WaitCursor
        If sql <> "" Then
            Me.SplitContainer1.Panel2Collapsed = False
            Try
                Dim stopWatch As New Stopwatch
                Dim dsResult As DataSet = Nothing
                Dim acceptCount As Integer = 0
                Dim isUpdateSql As Boolean = False

                stopWatch.Start()
                If autoCheck Then
                    If sql.TrimStart(Nothing).ToUpper().StartsWith("SELECT") Then 'vbCr, vbLf, " "c
                        dsResult = CurrDataBase.ExecuteDataSet(sql)
                    Else
                        acceptCount = ExecuteNoneQuery(sql)
                        isUpdateSql = True
                    End If
                Else
                    If isUpdate Then
                        acceptCount = ExecuteNoneQuery(sql)
                        isUpdateSql = True
                    Else
                        dsResult = CurrDataBase.ExecuteDataSet(sql)
                    End If
                End If

                stopWatch.Stop()

                Me.txtExecuteMsg.Text &= vbCrLf & vbCrLf & DateTime.Now.ToString()
                Me.txtExecuteMsg.Text &= vbCrLf & "执行时间(ms)：" & stopWatch.ElapsedMilliseconds

                Me.TabPageGrid.Controls.Clear()

                If acceptCount > 0 Or isUpdateSql Then
                    Me.TabControl1.SelectedTab = Me.TabPageMsg
                    Me.txtExecuteMsg.Text &= vbCrLf & acceptCount & " 行数据受影响！"
                    Exit Try
                Else
                    If dsResult Is Nothing OrElse dsResult.Tables.Count = 0 Then
                        Exit Try '可能直接执行了更改查询语句
                    End If
                    Me.txtExecuteMsg.Text &= vbCrLf & dsResult.Tables(0).Rows.Count & " 行数据已获取。"
                    Me.TabControl1.SelectedTab = Me.TabPageGrid
                End If

                Dim grid1 As New DataGridView()
                grid1.Dock = DockStyle.Fill
                grid1.DataSource = dsResult.Tables(0)


                '创建多个网格控件，显示数据
                If dsResult.Tables.Count > 1 Then
                    '结果集大于1个，创建多个拆分器显示
                    Dim splite1 As New SplitContainer()
                    splite1.Dock = DockStyle.Fill
                    splite1.Orientation = Orientation.Horizontal

                    Me.TabPageGrid.Controls.Add(splite1)
                    splite1.Panel1.Controls.Add(grid1)

                    Dim fatherSplite As SplitContainer = splite1

                    For i As Integer = 1 To dsResult.Tables.Count - 1
                        Dim gridTemp As New DataGridView
                        Dim spliteTemp As SplitContainer = CreateNewSpliteContainer(fatherSplite, gridTemp)
                        gridTemp.DataSource = dsResult.Tables(i)
                        fatherSplite = spliteTemp
                        Me.txtExecuteMsg.Text &= vbCrLf & dsResult.Tables(i).Rows.Count & " 行数据已获取。"
                    Next
                Else
                    Me.TabPageGrid.Controls.Add(grid1)
                End If

            Catch ex As Exception
                MessageBox.Show(ex.Message, "数据查询", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

        End If
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub rtbQueryText_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles rtbQueryText.KeyDown
        'If e.KeyCode = Keys.F5 Then
        '    ' MessageBox.Show("F5 key down.")
        '    Dim sql As String = Me.rtbQueryText.SelectedText
        '    If sql = "" Then
        '        sql = Me.rtbQueryText.Text
        '    End If
        '    ExecuteQuery(sql, False, True)
        'End If

    End Sub

    Private Function CreateNewSpliteContainer(ByVal father As SplitContainer, ByVal gridView As DataGridView) As SplitContainer

        Dim spliteTemp As New SplitContainer()
        spliteTemp.Orientation = Orientation.Horizontal
        spliteTemp.Panel1.Controls.Add(gridView)
        gridView.Dock = DockStyle.Fill

        spliteTemp.Panel2Collapsed = True

        father.Panel2Collapsed = False
        father.Panel2.Controls.Clear()
        father.Panel2.Controls.Add(spliteTemp)
        spliteTemp.Dock = DockStyle.Fill

        Return spliteTemp
    End Function

    Private Sub frmDataQuery_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Me.dgvQueryData.Hide()
        ' Dim SplitContainer2 As New SplitContainer()
        If IsGroupQuery Then
            Me.TabControl1.SelectedTab = Me.TabPageGroupQuery
        Else
            Me.SplitContainer1.Panel2Collapsed = True
        End If

        'Me.rtbQueryText.Text = "--请输入（可从左侧数据连结拖动名称）或者选择SQL语句，然后按下F5执行。" & vbCrLf & Me.rtbQueryText.Text
        Me.rtbQueryText.Focus()
        Me.rtbQueryText.AllowDrop = True

        Dim source = From item As String In [Enum].GetNames(GetType(PWMIS.Common.DBMSType)) _
                                    Select Name = item, Value = [Enum].Parse(GetType(PWMIS.Common.DBMSType), item)

        Me.colDbmsType.DataSource = source.ToList()
        Me.colDbmsType.DisplayMember = "Name"
        Me.colDbmsType.ValueMember = "Value"

        'test
        'DataConnections.Add(New DataConnection() With {.Enabled = True, .DbType = PWMIS.Common.DBMSType.SqlServer, .ConnectionStrng = "aaaaa"})
        Me.dgvGroupQuery.AutoGenerateColumns = False
        Me.dgvGroupQuery.DataSource = DataConnections

        Me.btnLoadGQuery.Enabled = My.Computer.FileSystem.FileExists(GroupQueryCfgFile)
    End Sub

    Private Sub rtbQueryText_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) _
   Handles rtbQueryText.DragEnter
        If (e.Data.GetDataPresent(DataFormats.Text)) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub


    Private Sub rtbQueryText_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) _
   Handles rtbQueryText.DragDrop
        Dim i As Int16
        Dim s As String
        Dim s2 As String
        ' Get start position to drop the text.
        i = rtbQueryText.SelectionStart
        s = rtbQueryText.Text.Substring(i)
        s2 = rtbQueryText.Text.Substring(0, i)

        ' Drop the text on to the RichTextBox.
        rtbQueryText.Text = s2 + e.Data.GetData(DataFormats.Text).ToString() + s

        'Dim source As String = rtbQueryText.Text

        'Mid(source, i) = e.Data.GetData(DataFormats.Text).ToString()
        'rtbQueryText.Text = source

    End Sub

    ''' <summary>
    ''' 获取用户输入或者选择的SQL语句
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetUserSQL() As String
        Dim sql As String = Me.rtbQueryText.SelectedText
        If sql = "" Then
            sql = Me.rtbQueryText.Text
        End If
        Return sql
    End Function

    Private Sub frmDataQuery_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If MessageBox.Show("确认关闭当前查询窗口吗？", "查询管理", MessageBoxButtons.OKCancel) = Windows.Forms.DialogResult.Cancel Then
            e.Cancel = True
        End If
    End Sub

    Private Sub tsmItemQueryDataSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmItemQueryDataSet.Click
        ExecuteQuery(GetUserSQL, False, False)
    End Sub

    Private Sub tsmItemUpdateTable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmItemUpdateTable.Click
        ExecuteQuery(GetUserSQL, True, False)
    End Sub

    Private Sub tsmItemSaveSQLScript_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmItemSaveSQLScript.Click
        If Me.CurrScriptFileName = "" Then
            Me.SaveFileDialog1.Filter = "SQL查询文件|*.sql|所有文件|*.*"
            Me.SaveFileDialog1.ShowDialog()
            If Me.SaveFileDialog1.FileName <> "" Then
                Me.CurrScriptFileName = Me.SaveFileDialog1.FileName
                SaveFile()
            End If
        Else
            SaveFile()
        End If
        

    End Sub

    Private Sub SaveFile()
        Try
            My.Computer.FileSystem.WriteAllText(CurrScriptFileName, Me.rtbQueryText.Text, False)
            Me.contentTextChange = False
            Me.Text = Me.Text.Substring(1)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub tsmItemExecute_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmItemExecute.Click
        ExecuteQuery(GetUserSQL, False, True)
    End Sub

    Private Sub rtbQueryText_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rtbQueryText.TextChanged
        If Not Me.contentTextChange Then
            Me.contentTextChange = True
            Me.Text = "*" & Me.Text
        End If
    End Sub

    Private Sub tsmItemCreateEntity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmItemCreateEntity.Click
        Dim sqlForm As New frmEntityFromSQL
        sqlForm.MapSQL = GetUserSQL()
        sqlForm.ClassNamespace = Me.CurrDBName
        sqlForm.ShowDialog()
        If sqlForm.IsOK Then
            Dim window As New frmEntityCreate()
            window.CurrDbHelper = Me.CurrDataBase
            window.CurrDBName = Me.CurrDBName
            With sqlForm
                window.AddMapInfoItem(False, .TableName, "查询", .ClassName, "<默认>", .MapSQL)
            End With
            Me.CommandForm.OpenWindow(Me, window, "")
        End If
        
    End Sub

    Private Sub btnAddConn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddConn.Click
        DataConnections.Add(New DataConnection() With {.Enabled = False, .DbType = PWMIS.Common.DBMSType.SqlServer, .ConnectionStrng = "Server=<IP或名称>;DataBase=<数据库名称>;uid=sa;pwd=<数据库密码>"})
        BindGroupQueryGrid()

    End Sub

    Private Sub dgvGroupQuery_CellEndEdit(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvGroupQuery.CellEndEdit
        If e.ColumnIndex = 0 Then
            If dgvGroupQuery.Rows(e.RowIndex).Cells(0).Value IsNot Nothing Then
                If dgvGroupQuery.Rows(e.RowIndex).Cells(0).Value = True Then
                    '选择了当前连接
                    Dim dbType As String = dgvGroupQuery.Rows(e.RowIndex).Cells(1).Value
                    Dim connStr As String = dgvGroupQuery.Rows(e.RowIndex).Cells(2).Value
                    Dim db As PWMIS.DataProvider.Data.AdoHelper = TestConnection(dbType, connStr)
                    If db Is Nothing Then
                        dgvGroupQuery.Rows(e.RowIndex).Cells(0).Value = False
                        'MessageBox.Show("数据连接测试失败：" & connStr)
                    Else
                        Dim dc As DataConnection = dgvGroupQuery.Rows(e.RowIndex).DataBoundItem
                        dc.CurrAdoHelper = db
                    End If
                End If

            End If

        End If
    End Sub

    Private Function TestConnection(ByVal dbType As String, ByVal connStr As String) As PWMIS.DataProvider.Data.AdoHelper
        Try
            Dim DBMSType As PWMIS.Common.DBMSType = System.Enum.Parse(GetType(PWMIS.Common.DBMSType), dbType)
            Dim db As PWMIS.DataProvider.Data.AdoHelper = PWMIS.DataProvider.Adapter.MyDB.GetDBHelper(DBMSType, connStr)
            Dim conn As IDbConnection = db.GetConnection(connStr)
            conn.Open()
            conn.Close()
            'MessageBox.Show("测试成功！", "数据连接测试", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.lblGroupQueryMsg.Text = "数据连接 " & connStr & " 测试成功！"
            db.ConnectionString = connStr
            Return db
        Catch ex As Exception
            MessageBox.Show(ex.Message, "数据连接测试", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return Nothing
    End Function

    Private Sub tsmItemProperty_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmItemProperty.Click
        Me.TabControl1.SelectedTab = Me.TabPageMsg
        Me.SplitContainer1.Panel2Collapsed = False
        If Me.CurrDataBase Is Nothing Then
            Me.txtExecuteMsg.Text &= vbCrLf & "-------------当前查询窗口属性---------------" _
                 & vbCrLf & "脚本文件名：" & Me.CurrScriptFileName _
                 & vbCrLf & "是否组查询：" & Me.IsGroupQuery
        Else
            Me.txtExecuteMsg.Text &= vbCrLf & "-------------当前查询窗口属性---------------" _
                  & vbCrLf & "数据库名称：" & Me.CurrDBName _
                  & vbCrLf & "连接字符串：" & Me.CurrDataBase.ConnectionString _
                  & vbCrLf & "脚本文件名：" & Me.CurrScriptFileName _
                  & vbCrLf & "是否组查询：" & Me.IsGroupQuery
        End If
      
    End Sub

    Private Sub btnSaveGQuery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveGQuery.Click
        Dim type As Type = GetType(List(Of DataConnection))
        Dim xs As New System.Xml.Serialization.XmlSerializer(type)
        Dim sb As New System.Text.StringBuilder()
        Dim sw As New System.IO.StringWriter(sb)
        xs.Serialize(sw, DataConnections)
        Dim result As String = sb.ToString()

        My.Computer.FileSystem.WriteAllText(GroupQueryCfgFile, result, False)
        MessageBox.Show("保存成功！", "数据连接文件")
    End Sub

    Private Sub btnLoadGQuery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadGQuery.Click
        Dim type As Type = GetType(List(Of DataConnection))
        Dim xs As New System.Xml.Serialization.XmlSerializer(type)
        Dim reader As New System.IO.StringReader(My.Computer.FileSystem.ReadAllText(GroupQueryCfgFile))
        DataConnections = xs.Deserialize(reader)

        BindGroupQueryGrid()
    End Sub

    Private Sub BindGroupQueryGrid()
        Me.dgvGroupQuery.DataSource = Nothing
        Me.dgvGroupQuery.DataSource = DataConnections
    End Sub

    Private Sub btnDelConn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelConn.Click
        If Me.dgvGroupQuery.SelectedRows.Count > 0 Then
            Dim list As New List(Of DataConnection)
            For i As Integer = 0 To Me.dgvGroupQuery.SelectedRows.Count - 1
                Dim dc As DataConnection = Me.dgvGroupQuery.SelectedRows(i).DataBoundItem
                list.Add(dc)
            Next
            For Each item As DataConnection In list
                DataConnections.Remove(item)
            Next
            BindGroupQueryGrid()
        Else
            MessageBox.Show("请先选择要删除的行！")
        End If
    End Sub
End Class

