Public Class frmSqlMapFile

    Private Sub frmSqlMapFile_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.TabControl1.TabPages.Add(New frmSqlMapFileEdit())
        Me.TabControl1.TabPages.Add(New frmSqlMapDesion())
    End Sub
End Class