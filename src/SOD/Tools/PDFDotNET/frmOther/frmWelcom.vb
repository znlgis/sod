Imports CefSharp

Public Class frmWelcom
    Dim WebBrowser1 As CefSharp.WinForms.ChromiumWebBrowser
    ''' <summary>
    ''' 命令窗体
    ''' </summary>
    ''' <remarks></remarks>
    Public CommandForm As ICommand

    Private Sub frmWelcom_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim setting As CefSettings = New CefSettings()
        With setting
            .Locale = "zh-CN"
            .AcceptLanguageList = "zh-CN"
            .MultiThreadedMessageLoop = True
        End With
        Dim osVersion = Environment.OSVersion
        '//Disable GPU for Windows 7  ,8,8.1 
        If osVersion.Version.Major = 6 Then
            '// Disable GPU in WPF and Offscreen examples until #1634 has been resolved
            setting.CefCommandLineArgs.Add("disable-gpu", "1")
        End If

        CefSharp.Cef.Initialize(setting)

        Me.WebBrowser1 = New CefSharp.WinForms.ChromiumWebBrowser(Me.txtUrl.Text)
        Me.panBody.Controls.Add(Me.WebBrowser1)
        Me.WebBrowser1.Dock = DockStyle.Fill

    End Sub

    Private Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        Me.WebBrowser1.Load(Me.txtUrl.Text)
    End Sub

    Private Sub txtUrl_KeyDown(sender As Object, e As KeyEventArgs) Handles txtUrl.KeyDown
        If e.KeyCode = Keys.Enter Then
            Me.WebBrowser1.Load(Me.txtUrl.Text)
        End If
    End Sub

    Private Sub picGoback_Click(sender As Object, e As EventArgs) Handles picGoback.Click
        Me.WebBrowser1.Back()
        Me.txtUrl.Text = Me.WebBrowser1.Address
    End Sub

    Private Sub picGofawerd_Click(sender As Object, e As EventArgs) Handles picGofawerd.Click
        Me.WebBrowser1.Forward()
        Me.txtUrl.Text = Me.WebBrowser1.Address
    End Sub

    Private Sub picRefersh_Click(sender As Object, e As EventArgs) Handles picRefersh.Click
        Me.WebBrowser1.Reload()
    End Sub

    Private Sub picGoHome_Click(sender As Object, e As EventArgs) Handles picGoHome.Click
        Me.WebBrowser1.Load("http://www.pwmis.com/sqlmap")
        Me.txtUrl.Text = Me.WebBrowser1.Address
    End Sub

    Private Sub lnk12306_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnk12306.LinkClicked
        Dim window As New frm12306Ticket()
        window.CommandForm = Me.CommandForm
        Me.CommandForm.OpenWindow(Me, window, "")
    End Sub
End Class