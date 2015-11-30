Public Class frmWelcom

    Private Sub frmWelcom_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.WebBrowser1.Url = New Uri("http://pwmis.codeplex.com/")

    End Sub
End Class