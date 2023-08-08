Public Class frmSqlMapFile
    Private Sub frmSqlMapFile_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.TabControl1.TabPages.Add(New frmSqlMapFileEdit())
        Me.TabControl1.TabPages.Add(New frmSqlMapDesion())
    End Sub
End Class