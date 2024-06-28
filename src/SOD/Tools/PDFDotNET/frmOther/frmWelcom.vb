Imports CefSharp
Imports CefSharp.WinForms

Public Class frmWelcom
    Dim WithEvents WebBrowser1 As CefSharp.WinForms.ChromiumWebBrowser
    Dim PageLoaded As Boolean
    ''' <summary>
    ''' 命令窗体
    ''' </summary>
    ''' <remarks></remarks>
    Public CommandForm As ICommand
    ''' <summary>
    ''' 父容器控件
    ''' </summary>
    Public ParentContainer As Control

    Public Property HomeUrl As String
        Get
            Return Me.txtUrl.Text
        End Get
        Set
            Me.txtUrl.Text = Value
        End Set
    End Property

    Private Sub frmWelcom_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        If Not CefSharp.Cef.IsInitialized Then
            Dim setting As CefSettings = New CefSettings()
            With setting
                .Locale = "zh-CN"
                .AcceptLanguageList = "zh-CN"
                .UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36"
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

            CefSharp.Cef.Initialize(setting)

        End If
        SendOperationStatusMessage("Web等待输入网址")

        Me.WebBrowser1 = New CefSharp.WinForms.ChromiumWebBrowser(Me.txtUrl.Text)
        Me.panBody.Controls.Add(Me.WebBrowser1)

        If Not ParentContainer Is Nothing Then
            Me.WebBrowser1.Size = New Drawing.Size(ParentContainer.Width, ParentContainer.Height - 50)
            Me.WebBrowser1.Dock = DockStyle.None
        Else
            btnNewTabWindow.Enabled = False
        End If

        If Me.txtUrl.Text = "" Then Me.txtUrl.Text = "http://" Else SendOperationStatusMessage("Web页正在加载...")
        LoadHotWebSiteInfo()
    End Sub

    Private Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        Me.WebBrowser1.Load(Me.txtUrl.Text)
        SendOperationStatusMessage("Web页正在加载...")
    End Sub

    Private Sub txtUrl_KeyDown(sender As Object, e As KeyEventArgs) Handles txtUrl.KeyDown
        If e.KeyCode = Keys.Enter Then
            Me.WebBrowser1.Load(Me.txtUrl.Text)
            SendOperationStatusMessage("Web页正在加载...")
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

    '独立窗口打开网页
    Private Sub lnkNewWindow_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkNewWindow.LinkClicked
        Dim window As New frmWelcom()
        window.CommandForm = Me.CommandForm
        window.HomeUrl = ""
        window.Text = "[SOD谷歌极简浏览器]"
        window.Show()
    End Sub

    Private Sub WebBrowser1_TitleChanged(sender As Object, e As TitleChangedEventArgs) Handles WebBrowser1.TitleChanged
        Me.Invoke(Sub()
                      Me.Text = e.Title + " @[SOD谷歌极简浏览器]"
                      PageLoaded = True
                  End Sub)
        SendOperationStatusMessage("Web页加载成功")
    End Sub

    Private Sub panBody_Resize(sender As Object, e As EventArgs) Handles panBody.Resize
        If PageLoaded Then
            If Not ParentContainer Is Nothing Then
                Me.WebBrowser1.Size = New Drawing.Size(ParentContainer.Width, ParentContainer.Height - 50)
                Me.WebBrowser1.Dock = DockStyle.None
            End If
        End If

    End Sub

    Private Sub btnNewTabWindow_Click(sender As Object, e As EventArgs) Handles btnNewTabWindow.Click
        If Not CommandForm Is Nothing Then
            Dim window As New frmWelcom()
            window.CommandForm = Me.CommandForm
            window.HomeUrl = ""
            window.Text = "[SOD谷歌极简浏览器]"
            window.ParentContainer = Me.ParentContainer
            CommandForm.OpenWindow(Me, window, "")
        End If
    End Sub

    Private Sub SendOperationStatusMessage(message As String)
        If Not CommandForm Is Nothing Then
            Dim dict As New Dictionary(Of String, Object)
            dict.Add("OpreationStatusMsg", message)
            CommandForm.Command("OpreationStatus", dict)
        End If
    End Sub

    Private Sub LoadHotWebSiteInfo()
        Dim hotCfg As String = System.Configuration.ConfigurationManager.AppSettings("HotWebSite")
        If Not String.IsNullOrEmpty(hotCfg) Then
            Dim siteList As List(Of KeyValuePair(Of String, String)) = New List(Of KeyValuePair(Of String, String))
            siteList.Add(New KeyValuePair(Of String, String)("常用网址", ""))

            Dim items As String() = hotCfg.Split(";")
            For Each s In items
                Dim site As String() = s.Split("|")
                If site.Length = 2 Then
                    siteList.Add(New KeyValuePair(Of String, String)(site(0), site(1)))
                End If
            Next
            cmbHotWebSite.DataSource = siteList
            cmbHotWebSite.DisplayMember = "Key"
            cmbHotWebSite.ValueMember = "Value"
        End If

    End Sub

    Private Sub cmbHotWebSite_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbHotWebSite.SelectedIndexChanged
        If cmbHotWebSite.SelectedIndex > 0 Then
            Dim url As String = cmbHotWebSite.SelectedValue
            Me.txtUrl.Text = url
            btnGo.PerformClick()
        End If
    End Sub

    Private Sub WebBrowser1_AddressChanged(sender As Object, e As AddressChangedEventArgs) Handles WebBrowser1.AddressChanged
        Dim myAction As Action(Of String) = New Action(Of String)(AddressOf AddressChange)
        Me.Invoke(myAction, e.Address)
    End Sub

    Private Sub AddressChange(address As String)
        Me.txtUrl.Text = address

    End Sub
End Class