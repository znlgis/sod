Public Class frmExportData
    Public ExportTableName As String
    Public ExportTables As List(Of String)
    Public dbLeftChar As String = ""
    Public dbRightChar As String = ""
    Public CurrDataBase As PWMIS.DataProvider.Data.AdoHelper
    Public ExportTypeIndex As Integer = 2

    Dim isCancel As Boolean = False
    Dim isWorking As Boolean = False

    Private Sub btnExpData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExpData.Click
        If Me.cmbExpType.Text = "SQL数据文件" Then
            Me.btnCancel.Text = "取消"
            Try
                btnExpData.Enabled = False
                isWorking = True
                If Me.txtFileName.Text = "" Then
                    MessageBox.Show("请输入要保存的文件名或者路径")
                    Exit Try
                End If

                Dim path As String = System.IO.Path.GetDirectoryName(Me.txtFileName.Text)
                If Me.ckListTables.CheckedItems.Count > 1 Then
                    '批量导出
                    For Each table As String In Me.ckListTables.CheckedItems
                        Dim fileName As String = path & "\sqldata_" & table & ".sql"
                        ExportSQLData(table, fileName, Me.CurrDataBase, Me.dbLeftChar, Me.dbRightChar)
                    Next
                Else
                    If ExportTableName Is Nothing Or ExportTableName = "" Then
                        MessageBox.Show("请选择要导出的表！", "数据导出", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Else
                        ExportSQLData(ExportTableName, Me.txtFileName.Text, Me.CurrDataBase, Me.dbLeftChar, Me.dbRightChar)
                    End If

                End If


            Catch ex As Exception
                MessageBox.Show("导出数据失败，原因：" & ex.Message, "数据导出", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            btnExpData.Enabled = True
            isWorking = False
            If Me.isCancel Then
                Me.lblProcessMsg.Text = "操作已经取消"
            End If
            Me.btnCancel.Text = "关闭"
        Else
            MessageBox.Show("目前不支持导出该类型")
        End If
    End Sub

    Private Sub btnBrowFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowFile.Click
        If Me.cmbExpType.Text = "SQL数据文件" Then
            Me.SaveFileDialog1.Filter = "SQL查询文件|*.sql|所有文件|*.*"
            Me.SaveFileDialog1.ShowDialog()
            If Me.SaveFileDialog1.FileName <> "" Then
                Me.txtFileName.Text = Me.SaveFileDialog1.FileName
            End If

        Else
            MessageBox.Show("目前不支持导出该类型")
        End If

    End Sub

    Private Function ExportSQLData(ByVal tableName As String, ByVal fileName As String, ByVal db As PWMIS.DataProvider.Data.AdoHelper, ByVal dbLeftChar As String, ByVal dbRightChar As String) As Boolean
        'Dim tableName As String = Me.ExportTableName
        'Dim fileName As String = Me.txtFileName.Text
        My.Computer.FileSystem.WriteAllText(fileName, "--SQL 数据文件导出，" & DateTime.Now.ToString() & "---" & vbCrLf, False, System.Text.Encoding.Default)
        Dim Sql As String = "SELECT * FROM " & dbLeftChar & tableName & dbRightChar & vbCrLf

        Dim rowIndex As Long = 0
        Dim allCount As Long = db.ExecuteScalar("select count(*) from " & dbLeftChar & tableName & dbRightChar)

        Me.ProgressBar1.Value = 0
        If allCount = 0 Then
            MessageBox.Show("表" & tableName & "没有记录，不需要导出。", "数据导出", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.lblProcessMsg.Text = "操作已经取消"
            Return False
        End If
        If MessageBox.Show("表" & tableName & " 共有" & allCount & " 条记录，需要全部导出吗？", "数据导出", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
            Me.lblProcessMsg.Text = "操作已经取消"
            Return False
        End If

        Using reader As IDataReader = db.ExecuteDataReader(Sql)
            Dim scTable As DataTable = reader.GetSchemaTable()
            Dim scView As DataView = scTable.DefaultView
            scView.Sort = "ColumnOrdinal"
            Dim fieldIndex As New Dictionary(Of Integer, String) 'key=列的索引，Vlue=列的名称
            Dim fieldType As New Dictionary(Of Integer, Type) 'key=列的索引，Vlue=列的类型

            Dim tempSQL As String = "INSERT INTO " & dbLeftChar & tableName & dbRightChar & " ("
            For Each dv As DataRowView In scView
                tempSQL &= dbLeftChar & dv("ColumnName").ToString() & dbRightChar & ","
                fieldIndex.Add(dv("ColumnOrdinal"), dv("ColumnName"))
                fieldType.Add(dv("ColumnOrdinal"), dv("DataType"))

            Next

            tempSQL = tempSQL.TrimEnd(","c) & " ) VALUES( @@Values );" & vbCrLf

            Dim resultSQL As String
            Dim fieldCount As Integer = reader.FieldCount

            Dim writer As System.IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(fileName, True, System.Text.Encoding.Default)

            Do While reader.Read()
                Dim Values As New System.Text.StringBuilder
                For i As Integer = 0 To fieldCount - 1
                    If reader.IsDBNull(i) Then
                        Values.Append("NULL")
                    Else
                        Dim tempStr As String = reader(i).ToString()
                        If fieldType(i) Is GetType(String) OrElse fieldType(i) Is GetType(DateTime) OrElse fieldType(i) Is GetType(Boolean) Then
                            tempStr = "'" & tempStr.Replace("'", "''") & "'"
                        End If
                        Values.Append(tempStr)
                    End If
                    Values.Append(",")
                Next
                resultSQL = tempSQL.Replace("@@Values", Values.ToString().TrimEnd(","c))
                '采用流写入，应对大容量数据表输出文件
                writer.Write(resultSQL)

                '输出进度信息
                rowIndex += 1
                If rowIndex Mod 50 = 0 Then
                    Me.ProgressBar1.Value = rowIndex * 100 / allCount
                    Me.lblProcessMsg.Text = "正在导出表：" & tableName & "," & rowIndex & allCount
                    Application.DoEvents()
                End If
                If Me.isCancel Then Exit Do
            Loop
            reader.Close()
            writer.Flush()
            writer.Close()
        End Using

        Me.ProgressBar1.Value = 100
        Me.lblProcessMsg.Text = "完成导出表" & tableName
        Return True
    End Function

    Private Sub cmbExpType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbExpType.Click
        If Me.cmbExpType.Text = "SQL数据文件" Then
            Me.txtFileName.Text = ".\exp_sqldata_" & Me.ExportTableName & ".sql"

        Else
            MessageBox.Show("目前不支持导出" & Me.cmbExpType.Text & " 类型")
            Me.cmbExpType.SelectedIndex = 2
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Me.Close()
        Me.isCancel = True
        If Not isWorking Then
            Me.Close()
        End If
    End Sub

    Private Sub frmExportData_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.cmbExpType.SelectedIndex = Me.ExportTypeIndex
        '初始化选择的表
        If Me.ExportTables IsNot Nothing Then
            For Each Str As String In Me.ExportTables
                If Str = Me.ExportTableName Then
                    Me.ckListTables.Items.Add(Str, True)
                Else
                    Me.ckListTables.Items.Add(Str, False)
                End If
            Next
        End If

    End Sub


    Private Sub ckAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckAll.CheckedChanged
        For i As Integer = 0 To Me.ckListTables.Items.Count - 1
            Me.ckListTables.SetItemChecked(i, ckAll.Checked)
        Next

    End Sub




End Class