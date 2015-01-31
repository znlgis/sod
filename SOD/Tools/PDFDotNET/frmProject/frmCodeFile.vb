Public Class frmCodeFile
    Public FileName As String
    ''' <summary>
    ''' 当前操作的内容文本
    ''' </summary>
    ''' <remarks></remarks>
    Public ContentText As String
    Private contentChange As Boolean

    Private Sub OpenConfigFile(ByVal fileName As String)
        'System.Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory
        'Dim fileName As String = Configuration.ConfigurationManager.AppSettings(fileKey)
        'fileName = fileName & ".config"
        If System.IO.File.Exists(fileName) Then
            'System.Diagnostics.Process.Start("notepad", fileName)
            Me.txtFileText.Text = System.IO.File.ReadAllText(fileName, System.Text.Encoding.Default)

            Me.Text = "文件：" + fileName

        Else
            MessageBox.Show("指定的文件未找到，文件名称：" + fileName, "打开文件", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub frmCodeFile_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '如果指定了文件名，则打开文件，否则用指定的文本初始化控件内容
        If Me.FileName IsNot Nothing OrElse Me.FileName <> "" Then
            OpenConfigFile(Me.FileName)
        Else
            Me.txtFileText.Text = Me.ContentText
        End If

    End Sub

    Private Sub 关闭ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 关闭ToolStripMenuItem.Click
       
        Me.Close()
    End Sub

    Private Function ConfirmSaveFile() As Integer
        If Not Me.txtFileText.ReadOnly And Me.contentChange Then
            Select Case MessageBox.Show("文件已经修改，需要保存吗？", "编辑文件", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                Case Windows.Forms.DialogResult.Yes
                    SaveFile()
                    Return 1
                Case Windows.Forms.DialogResult.Cancel
                    Return -1
            End Select

        End If
        Return 0
    End Function

    Private Sub SaveFile()
        Try
            Dim saveFileName As String = Me.FileName
            If Me.FileName Is Nothing OrElse Me.FileName = "" Then
                saveFileName = InputBox("请输入要保存的文件名", "保存文件")
                If saveFileName = "" Then
                    MessageBox.Show("文件名为空，不能保存", "保存文件")
                    Return
                End If

            End If
            My.Computer.FileSystem.WriteAllText(saveFileName, Me.txtFileText.Text, False, System.Text.Encoding.Default)
            Me.contentChange = False
            Me.Text = Me.Text.TrimStart("*"c)

            Me.FileName = saveFileName
        Catch ex As Exception
            MessageBox.Show(ex.Message)

        End Try
    End Sub

    Private Sub 保存ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 保存ToolStripMenuItem.Click
        SaveFile()
        Me.txtFileText.ReadOnly = True
    End Sub

    Private Sub SetMenuEnabled()
        ContextMenuStrip1.Items(2).Enabled = Me.contentChange
        ContextMenuStrip1.Items(3).Enabled = Not Me.txtFileText.ReadOnly
        ContextMenuStrip1.Items(4).Enabled = Not Me.txtFileText.ReadOnly
        ContextMenuStrip1.Items(5).Enabled = Not Me.txtFileText.ReadOnly
        ContextMenuStrip1.Items(6).Enabled = Not Me.txtFileText.ReadOnly
        ContextMenuStrip1.Items(7).Enabled = Not Me.txtFileText.ReadOnly
        ContextMenuStrip1.Items(8).Enabled = Not Me.txtFileText.ReadOnly
    End Sub

    Private Sub ContextMenuStrip1_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening
        SetMenuEnabled()
    End Sub

    Private Sub 编辑ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 编辑ToolStripMenuItem.Click
        If 编辑ToolStripMenuItem.Text = "编辑" Then
            Me.txtFileText.ReadOnly = False
            编辑ToolStripMenuItem.Text = "保护"
        Else
            Me.txtFileText.ReadOnly = True
            编辑ToolStripMenuItem.Text = "编辑"
        End If
        SetMenuEnabled()
    End Sub

    Private Sub 启用外部程序编辑ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 启用外部程序编辑ToolStripMenuItem.Click

        '
        '
        '
        Me.txtFileText.ReadOnly = True
    End Sub

    Private Sub menu_Paste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menu_Paste.Click
        ' Determine if there is any text in the Clipboard to paste into the text box.
        If Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) = True Then
            ' Determine if any text is selected in the text box.
            If txtFileText.SelectionLength > 0 Then
                ' Ask user if they want to paste over currently selected text.
                If MessageBox.Show("Do you want to paste over current selection?", _
                    "Cut Example", MessageBoxButtons.YesNo) = DialogResult.No Then
                    ' Move selection to the point after the current selection and paste.
                    txtFileText.SelectionStart = txtFileText.SelectionStart + _
                        txtFileText.SelectionLength
                End If
            End If
            ' Paste current text in Clipboard into text box.
            txtFileText.Paste()
            SetContentChange()
        End If

    End Sub

    Private Sub menu_Copy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menu_Copy.Click
        ' Ensure that text is selected in the text box.   
        If txtFileText.SelectionLength > 0 Then
            ' Copy the selected text to the Clipboard.
            txtFileText.Copy()
            SetContentChange()
        End If

    End Sub

    Private Sub menu_Cut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menu_Cut.Click
        ' Ensure that text is currently selected in the text box.   
        If txtFileText.SelectedText <> "" Then
            ' Cut the selected text in the control and paste it into the Clipboard.
            txtFileText.Cut()
            SetContentChange()
        End If

    End Sub

    Private Sub menu_Undo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menu_Undo.Click
        ' Determine if last operation can be undone in text box.   
        If txtFileText.CanUndo = True Then
            ' Undo the last operation.
            txtFileText.Undo()
            ' Clear the undo buffer to prevent last action from being redone.
            ' txtFileText.ClearUndo()
            SetContentChange()
        End If

    End Sub

    Private Sub txtFileText_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtFileText.KeyPress
        'If Not Char.IsControl(e.KeyChar) Then
        '    SetContentChange()

        'End If
    End Sub

    Private Sub frmCodeFile_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If ConfirmSaveFile() = -1 Then
            e.Cancel = True
        End If
    End Sub

    Private Sub SetContentChange()
        If Not Me.contentChange Then
            Me.contentChange = True
            Me.Text = "*" & Me.Text
        End If
    End Sub

    Private Sub txtFileText_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFileText.TextChanged
        SetContentChange()
    End Sub
End Class