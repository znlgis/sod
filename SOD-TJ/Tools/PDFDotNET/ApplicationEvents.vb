Namespace My

    ' 以下事件可用于 MyApplication:
    ' 
    ' Startup: 应用程序启动时在创建启动窗体之前引发。
    ' Shutdown: 在关闭所有应用程序窗体后引发。如果应用程序异常终止，则不会引发此事件。
    ' UnhandledException: 在应用程序遇到未处理的异常时引发。
    ' StartupNextInstance: 在启动单实例应用程序且应用程序已处于活动状态时引发。
    ' NetworkAvailabilityChanged: 在连接或断开网络连接时引发。
    Partial Friend Class MyApplication
        Private Sub MyApplication_UnhandledException(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            MessageBox.Show(e.Exception.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '这里记录错误信息
            If MessageBox.Show("当前有错误未处理，需要关闭本应用程序吗？" & vbCrLf & "（如果错误不是很严重你可以不退出或者稍后退出本程序）", "系统错误", MessageBoxButtons.YesNo, MessageBoxIcon.Error) = DialogResult.No Then
                e.ExitApplication = False
            End If

        End Sub
    End Class

End Namespace

