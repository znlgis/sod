Public Class Form1
    Dim VerCode As String
    Private Sub 退出ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 退出ToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub menuAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuAbout.Click
        If VerCode = "R" Then
            Dim about As New AboutBox1()
            about.Show()
        Else
            Me.txtFileText.Text = "PDF.NET 快速（数据）开发框架，支持ORM和SQL-MAP功能，支持表现层、业务逻辑层、数据访问层的快速数据开发。" & vbCrLf & _
            "邓太华 2010.3"
            Me.lblFileName.Text = ""

        End If

    End Sub

    Private Sub menuSqlMapBuilder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSqlMapBuilder.Click
        RunProcessByConfig("SqlMapBuilderPath")
    End Sub

    Private Sub RunProcessByConfig(ByVal fileKey As String)
        System.Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory
        Dim fileName As String = Configuration.ConfigurationManager.AppSettings(fileKey)
        If System.IO.File.Exists(fileName) Then
            Try
                System.Diagnostics.Process.Start(fileName)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "打开文件", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try


        Else
            MessageBox.Show("指定的程序未找到，程序名称：" + fileName, "打开文件", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub


    Private Sub OpenConfigFile(ByVal fileKey As String)
        System.Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory
        Dim fileName As String = Configuration.ConfigurationManager.AppSettings(fileKey)
        fileName = fileName & ".config"
        If System.IO.File.Exists(fileName) Then
            'System.Diagnostics.Process.Start("notepad", fileName)
            Me.txtFileText.Text = System.IO.File.ReadAllText(fileName, System.Text.Encoding.Default)
            Me.lblOptTile.Text = "配置文件管理"
            Me.lblFileName.Text = fileName

        Else
            MessageBox.Show("指定的文件未找到，文件名称：" + fileName, "打开文件", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub menuSqlMapCodeMaker_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSqlMapCodeMaker.Click
        RunProcessByConfig("PDFCodeMakerPath")
    End Sub

    Private Sub menuEntityMaker_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuEntityMaker.Click
        RunProcessByConfig("EntityCodeMakerPath")
    End Sub

    Private Sub menuIDEConfig_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuIDEConfig.Click
        OpenConfigFile("PDFDotNETPath")
    End Sub

    Private Sub menuCodeMakerConfig_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuCodeMakerConfig.Click
        OpenConfigFile("PDFCodeMakerPath")
    End Sub

    Private Sub menuEntityMakerConfig_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuEntityMakerConfig.Click
        OpenConfigFile("EntityCodeMakerPath")
    End Sub


    Private Sub menuOpenSqlMapConfig_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuOpenSqlMapConfig.Click
        OpenMyFileDialog("配置文件|*.config|XML文件|*.xml")

    End Sub

    Private Sub menuOpenMakedCodeFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuOpenMakedCodeFile.Click
        OpenMyFileDialog("C#文件|*.cs|VB文件|*.vb|所有文件|*.*")

      
    End Sub

    Private Sub OpenMyFileDialog(ByVal filter As String)
        Me.OpenFileDialog1.FileName = ""
        Me.OpenFileDialog1.Filter = filter
        Me.OpenFileDialog1.ShowDialog()

        If Me.OpenFileDialog1.FileName <> "" Then
            'System.Diagnostics.Process.Start(Me.OpenFileDialog1.FileName)
            Me.txtFileText.Text = System.IO.File.ReadAllText(Me.OpenFileDialog1.FileName, System.Text.Encoding.Default)
            Me.lblOptTile.Text = "文件管理"
            Me.lblFileName.Text = Me.OpenFileDialog1.FileName

        End If
    End Sub

    Private Sub menuOnlineHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuOnlineHelp.Click
        If VerCode = "R" Then
            System.Diagnostics.Process.Start("http://www.pwmis.com/sqlmap")
        Else
            If Configuration.ConfigurationManager.AppSettings("OnLineHelp") <> "" Then
                System.Diagnostics.Process.Start(Configuration.ConfigurationManager.AppSettings("OnLineHelp"))
            End If
        End If

    End Sub

    Private Sub menuPDFNetHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuPDFNetHelp.Click
        System.Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory
        Dim fileName As String = "PdfNet.hlp"
        If System.IO.File.Exists(fileName) Then
            System.Diagnostics.Process.Start(fileName)

        Else
            MessageBox.Show("指定的文件未找到，文件名称：" + fileName, "打开文件", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        If Me.lblFileName.Text <> "请选择相应菜单" And Me.lblFileName.Text <> "" Then
            System.Diagnostics.Process.Start(Me.lblFileName.Text)
        End If

    End Sub


    Private Sub tsSqlMapper_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsSqlMapper.Click
        RunProcessByConfig("SqlMapBuilderPath")
    End Sub

    Private Sub tsDALCoder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsDALCoder.Click
        RunProcessByConfig("PDFCodeMakerPath")
    End Sub

    Private Sub tsEntityCoder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsEntityCoder.Click
        RunProcessByConfig("EntityCodeMakerPath")
    End Sub

    Private Sub Form1_SizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged
        Me.txtFileText.Width = Me.Width - 30
        Me.txtFileText.Height = Me.Height - 120
        Me.btnEdit.Left = Me.txtFileText.Width - 50

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        VerCode = Configuration.ConfigurationManager.AppSettings("VerCode") '等于Release版才可以
        Me.txtFileText.Text = My.Computer.FileSystem.ReadAllText("verinfo.txt", System.Text.Encoding.Default)
    End Sub

    Private Sub 版本信息ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 版本信息ToolStripMenuItem.Click
        Me.txtFileText.Text = My.Computer.FileSystem.ReadAllText("verinfo.txt", System.Text.Encoding.Default)

    End Sub

    Private Sub menuSysConfig_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSysConfig.Click

    End Sub
End Class
