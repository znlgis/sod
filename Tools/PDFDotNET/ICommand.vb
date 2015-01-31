Public Interface ICommand
    ''' <summary>
    ''' 执行命令
    ''' </summary>
    ''' <param name="commandName">命令名称</param>
    ''' <param name="parameters">命令参数</param>
    ''' <returns>命令执行是否成功</returns>
    ''' <remarks></remarks>
    Function Command(ByVal commandName As String, ByVal parameters As Dictionary(Of String, Object)) As Boolean
    ''' <summary>
    ''' 打开指定的窗口实例
    ''' </summary>
    ''' <param name="sender">发送请求的对象</param>
    ''' <param name="objectWindow">指定的窗口实例</param>
    ''' <param name="openStyle">打开窗口的方式</param>
    ''' <returns>是否成功</returns>
    ''' <remarks></remarks>
    Function OpenWindow(ByVal sender As Object, ByVal objectWindow As System.Windows.Forms.Form, ByVal openStyle As String) As Boolean
End Interface
