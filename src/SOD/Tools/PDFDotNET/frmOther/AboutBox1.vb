Imports System.IO

Public NotInheritable Class AboutBox1
    Private Sub AboutBox1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' 设置此窗体的标题。
        Dim ApplicationTitle As String
        If My.Application.Info.Title <> "" Then
            ApplicationTitle = My.Application.Info.Title
        Else
            ApplicationTitle = Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If
        Me.Text = String.Format("关于 {0}", ApplicationTitle)
        ' 初始化“关于”对话框显示的所有文字。
        ' TODO: 在项目的“应用程序”窗格中自定义此应用程序的程序集信息 
        '    属性对话框(在“项目”菜单下)。
        Me.LabelProductName.Text = My.Application.Info.ProductName
        Me.LabelVersion.Text = String.Format("版本 {0}", My.Application.Info.Version.ToString)
        Me.LabelCopyright.Text = My.Application.Info.Copyright
        Me.LabelCompanyName.Text = My.Application.Info.CompanyName
        Me.TextBoxDescription.Text = My.Application.Info.Description
    End Sub

    Private Sub OKButton_Click(sender As Object, e As EventArgs) Handles OKButton.Click
        Me.Close()
    End Sub
End Class
