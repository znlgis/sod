Imports CefSharp
Imports CefSharp.WinForms

Public Class Form2
    Dim WithEvents WebBrowser1 As ChromiumWebBrowser

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Cef.IsInitialized Then
            Dim setting = New CefSettings()
            With setting
                .Locale = "zh-CN"
                .AcceptLanguageList = "zh-CN"
                .UserAgent =
                    "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36"
                .MultiThreadedMessageLoop = True
                .SetOffScreenRenderingBestPerformanceArgs()
            End With
            Dim osVersion = Environment.OSVersion
            '//Disable GPU for Windows 7  ,8,8.1 
            If osVersion.Version.Major = 6 Then
                '// Disable GPU in WPF and Offscreen examples until #1634 has been resolved
                'setting.CefCommandLineArgs.Add("disable-gpu", "1")
                'SetOffScreenRenderingBestPerformanceArgs 已经调用此方法来设置相同的功能
            End If

            Cef.Initialize(setting)

        End If

        Me.WebBrowser1 = New ChromiumWebBrowser("http://www.pwmis.com/sqlmap")
        Me.PanelBody.Controls.Add(Me.WebBrowser1)
        Me.WebBrowser1.Dock = DockStyle.Fill
    End Sub

    Private Sub WebBrowser1_TitleChanged(sender As Object, e As TitleChangedEventArgs) Handles WebBrowser1.TitleChanged
        Me.Invoke(Sub()
            Me.Text = e.Title
        End Sub)
    End Sub
End Class