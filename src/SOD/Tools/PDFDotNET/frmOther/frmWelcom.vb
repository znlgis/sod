Imports CefSharp

Public Class frmWelcom
    Dim WebBrowser1 As CefSharp.WinForms.ChromiumWebBrowser


    Private Sub frmWelcom_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim settion As CefSettings = New CefSettings()
        With settion
            .Locale = "zh-CN"
            .AcceptLanguageList = "zh-CN"
            .MultiThreadedMessageLoop = True
        End With

        CefSharp.Cef.Initialize(settion)

        Me.WebBrowser1 = New CefSharp.WinForms.ChromiumWebBrowser("http://pwmis.codeplex.com/")
        Me.Controls.Add(Me.WebBrowser1)
        Me.WebBrowser1.Dock = DockStyle.Fill

    End Sub
End Class