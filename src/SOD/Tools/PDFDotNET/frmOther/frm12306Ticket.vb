Imports CefSharp

Public Class frm12306Ticket
    Dim ticketUrl As String = "TestHtml.html" ' "https://kyfw.12306.cn/otn/leftTicket/init"
    Dim WithEvents WebBrowser1 As CefSharp.WinForms.ChromiumWebBrowser
    ''' <summary>
    ''' 命令窗体
    ''' </summary>
    ''' <remarks></remarks>
    Public CommandForm As ICommand

    Public Lookup As Boolean
    Public FoundTickt As Boolean

    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        '有关 CefSharp的使用，请看 http://blog.csdn.net/gong_hui2000/article/details/48155547
        '                         http://www.codebye.com/cefsharp-help-5-javascript-handler.html
        If ticketUrl = "TestHtml.html" Then
            Dim path As String = System.IO.Path.Combine(System.Environment.CurrentDirectory, "TestHtml.html")
            Dim uri As New Uri(path)
            ticketUrl = uri.AbsoluteUri
        End If


        Me.WebBrowser1 = New CefSharp.WinForms.ChromiumWebBrowser(Me.ticketUrl)
        Me.WebBrowser1.RegisterJsObject("jsObj", New TicketNotify(Me), Nothing)
        'WindowsFormsControlLibrary1

        Me.panBody.Controls.Add(Me.WebBrowser1)
        Me.WebBrowser1.Dock = DockStyle.Fill

    End Sub

    Public Sub StartNotify()
        Me.Lookup = True
    End Sub

    Public Sub StopNotify()
        Me.Lookup = False
    End Sub

    Public Function StartOrStopNotify() As Boolean
        Me.Lookup = Not Me.Lookup
        If Not Me.Lookup Then Me.FoundTickt = False
        Return Me.Lookup
    End Function
    Private Sub frm12306Ticket_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        StopNotify()

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim msg As String = DateTime.Now.ToLongTimeString()
        If Me.FoundTickt Then
            If DateTime.Now.Second Mod 10 = 0 Then
                Me.Notify()
                Me.WebBrowser1.ExecuteScriptAsync("showHaveTicket", "")
            End If
        ElseIf Me.Lookup Then
            '下面两行代码效果一样
            Me.WebBrowser1.ExecuteScriptAsync("checkHaveTicket", "")
            'Me.WebBrowser1.GetMainFrame().ExecuteJavaScriptAsync("checkHaveTicket();")
            msg = "监控中 " + msg
        Else
            lblMsg.Text = "监控已经停止。"
        End If

        Me.lblTime.Text = msg
    End Sub

    Private Sub frm12306Ticket_Activated(sender As Object, e As EventArgs) Handles MyBase.Activated

    End Sub

    Private Sub WebBrowser1_FrameLoadEnd(sender As Object, e As FrameLoadEndEventArgs) Handles WebBrowser1.FrameLoadEnd
        Dim js As String =
<string>
       var divAlert=true;
       function checkHaveTicket() {
            var div = document.getElementById('autosubmitcheckticketinfo');
            if (div) {
                if (div.style.display == 'block' || div.style.display == '') {
                    //txtName.value = '有票了!!!';
                    jsObj.myNotify();
                }
                else
                {
                    jsObj.endNotify();
                }
            }
        }

       function showHaveTicket() {
            var div = document.getElementById('autosubmitcheckticketinfo');
            if (div) {
                if (div.style.display == 'block' || div.style.display == '') {
                    if(divAlert)
                       div.style.borderBottom='5px solid #FF0000';
                    else
                        div.style.borderBottom='none';
                    divAlert= !divAlert;
                }
            }
        }

</string>
        '下面两行代码效果一样
        'Me.WebBrowser1.GetMainFrame().ExecuteJavaScriptAsync(js)
        Me.WebBrowser1.ExecuteScriptAsync(js)
    End Sub


    Private Sub WebBrowser1_IsBrowserInitializedChanged(sender As Object, e As IsBrowserInitializedChangedEventArgs) Handles WebBrowser1.IsBrowserInitializedChanged
        If e.IsBrowserInitialized Then
            '不可以在这里注册JS代码，新版CefSharp 找不到
            Me.Timer1.Start()
        End If
    End Sub

    Public Sub Notify()
        Dim dict As New Dictionary(Of String, Object)
        dict.Add("sender", Me)
        Me.CommandForm.Command("TopForm", dict)
        Dim myAction As Action = Sub()
                                     'Me.Timer1.Stop()
                                     'Me.Timer1.Interval = 10000
                                     'Me.Timer1.Start()
                                     Me.TopMost = True
                                     Me.Activate()
                                     'MessageBox.Show("有票了，请在浏览器中完成操作！\r\n 如果你返回修改了订票规则（比如修改席别），请单击下【继续刷票】按钮！")
                                     lblMsg.Text = "有票了，请在网页上继续操作！"
                                     Dim player As New System.Media.SoundPlayer
                                     Dim alertWav As String = "Alarm01.wav"
                                     player.SoundLocation = alertWav
                                     player.Load()
                                     player.Play()

                                 End Sub
        Me.Invoke(myAction)

    End Sub

    'Sub leftTicket()
    '    Dim aaa = Me.WebBrowser1.getf
    '    Dim div = WebBrowser1.GetElementById("autosubmitcheckticketinfo")
    '    If div <> Nothing Then

    '        '//修改刷新时间
    '        If DateTime.Now.Second Mod 2 = 0 Then '//每2秒切换一次

    '            If Not isChanged Then

    '                isChanged = !isChanged

    '                If changeSearchTime Then

    '                    Dim win = (mshtml.IHTMLWindow2)webBrowser1.Document.Window.DomWindow
    '                        win.execScript("autoSearchTime = " + autoSearchTime + " ;", "javascript")
    '                    changeSearchTime = False
    '                End If
    '                If secFromStation! = "" Then

    '                    Dim currFromText As String = isOldFromStation ? oldFromStation : secFromStation
    '                    isOldFromStation = !isOldFromStation
    '                    SetHtmlTextBoxValue("fromStationText", currFromText)
    '                End If
    '                If secToStation <> "" Then

    '                    Dim currToText As String = isOldToStation ? oldToStation : secToStation
    '                    isOldToStation = !isOldToStation
    '                    SetHtmlTextBoxValue("toStationText", currToText)
    '                End If
    '            End If
    '        Else
    '            isChanged = False
    '        End If


    '        mshtml.IHTMLElement currDiv = (mshtml.IHTMLElement)div.DomElement
    '            If currDiv.style.display = "block" Or String.IsNullOrEmpty(currDiv.style.display) Then

    '            Timer1.Stop()
    '            '//最小化问题
    '            If this.WindowState = FormWindowState.Minimized Then

    '                this.WindowState = FormWindowState.Maximized
    '                currDiv.style.left = "400px"
    '            End If

    '            this.TopMost = True
    '            MessageBox.Show("有票了，请在浏览器中完成操作！\r\n 如果你返回修改了订票规则（比如修改席别），请单击下【继续刷票】按钮！")
    '            lblMsg.Text = "弹窗监视器已经停止，请单击【继续刷票】按钮开启！"
    '            this.TopMost = False
    '        End If
    '    End If
    'End Sub

    Private Sub btnOpenUrl_Click(sender As Object, e As EventArgs) Handles btnOpenUrl.Click
        Dim path As String = System.IO.Path.Combine(System.Environment.CurrentDirectory, "TestHtml.html")
        Dim uri As New Uri(path)
        ticketUrl = uri.AbsoluteUri
        Me.txtUrl.Text = "TestHtml.html"
        Me.WebBrowser1.Load(ticketUrl)
    End Sub

    Private Sub btnOpenUrl2_Click(sender As Object, e As EventArgs) Handles btnOpenUrl2.Click
        If btnOpenUrl2.Text = "停止刷票" Then
            StopNotify()
            btnOpenUrl2.Text = "开始刷票"
        Else
            ticketUrl = "https://kyfw.12306.cn/otn/leftTicket/init"
            Me.txtUrl.Text = ticketUrl
            Me.WebBrowser1.Load(ticketUrl)
            StartNotify()
            lblMsg.Text = "请登陆12306后，开启网页的【订票助手】功能，设定好条件后，勾选【开启自动查询】，最后点击【查询】按钮。本程序监控到有票的时候，会及时弹窗通知。"
            btnOpenUrl2.Text = "停止刷票"
        End If
     
    End Sub

    Private Sub btnTestJS_Click(sender As Object, e As EventArgs) Handles btnTestJS.Click
        Me.WebBrowser1.ExecuteScriptAsync("alert(typeof jsObj )")
    End Sub
End Class

Public Class TicketNotify
    Dim owerForm As Form

    Public Sub New(ByVal owner As Form)
        Me.owerForm = owner
    End Sub
    Public Sub TestNotify()

        Dim target As frm12306Ticket = Me.owerForm
        If target.StartOrStopNotify() Then
            MessageBox.Show("开始【测试】弹窗和音乐通知！（再次按下停止）", "VB.NET -》浏览器脚本通知", MessageBoxButtons.OK)
        Else
            MessageBox.Show("停止【测试】弹窗和音乐通知。", "VB.NET -》浏览器脚本通知", MessageBoxButtons.OK)
        End If
    End Sub
    Public Sub MyNotify()
        Dim target As frm12306Ticket = Me.owerForm
        target.FoundTickt = True
        ' target.Notify()
    End Sub
    Public Sub EndNotify()
        Dim target As frm12306Ticket = Me.owerForm
        target.FoundTickt = False
        ' target.Notify()
    End Sub
End Class