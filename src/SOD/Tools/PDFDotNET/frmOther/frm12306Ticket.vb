Imports CefSharp

Public Class frm12306Ticket
    Dim ticketUrl As String = "TestHtml.html" ' "https://kyfw.12306.cn/otn/leftTicket/init"
    Dim WithEvents WebBrowser1 As CefSharp.WinForms.ChromiumWebBrowser


    Private Sub frm12306Ticket_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '有关 CefSharp的使用，请看 http://blog.csdn.net/gong_hui2000/article/details/48155547
        '                         http://www.codebye.com/cefsharp-help-5-javascript-handler.html
        If ticketUrl = "TestHtml.html" Then
            Dim path As String = System.IO.Path.Combine(System.Environment.CurrentDirectory, "TestHtml.html")
            Dim uri As New Uri(path)
            ticketUrl = uri.AbsoluteUri
        End If


        Me.WebBrowser1 = New CefSharp.WinForms.ChromiumWebBrowser(Me.ticketUrl)
        Me.WebBrowser1.RegisterJsObject("jsObj", New TicketNotify(), False)
        'WindowsFormsControlLibrary1

        Me.panBody.Controls.Add(Me.WebBrowser1)
        Me.WebBrowser1.Dock = DockStyle.Fill
        Timer1.Stop()

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'Dim js As String = "checkHaveTicket();"
        'Me.WebBrowser1.ExecuteScriptAsync(js)
        'Me.WebBrowser1.ExecuteScriptAsync("checkHaveTicket", "")
        Me.WebBrowser1.ExecuteScriptAsync("jsObj.ShowTest();")
    End Sub

    Private Sub frm12306Ticket_Activated(sender As Object, e As EventArgs) Handles MyBase.Activated

    End Sub


    Private Sub WebBrowser1_IsBrowserInitializedChanged(sender As Object, e As IsBrowserInitializedChangedEventArgs) Handles WebBrowser1.IsBrowserInitializedChanged
        If e.IsBrowserInitialized Then
            Dim js As String = "
 function checkHaveTicket()
{
 var div= document.getElementById('autosubmitcheckticketinfo');
 if(div)
 {
   alert('1');
   if(jsObj)
      alert('jsObj');
   else
      alert('not obj');
   jsObj.ShowTest();
   if(div.style.display == 'block' || div.style.display == '')
   {
      alert('有票了！');
   }
  }
}
"
            Me.WebBrowser1.ExecuteScriptAsync(js)

            Timer1.Start()
        End If
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
End Class

Public Class TicketNotify
    Public Sub MyNotify()
        MessageBox.Show("VB.Net")

    End Sub
End Class