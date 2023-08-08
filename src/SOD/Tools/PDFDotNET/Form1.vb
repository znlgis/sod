Imports System.Configuration
Imports System.IO
Imports System.Text

Public Class Form1
    Dim VerCode As String

    Private Sub 退出ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 退出ToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub menuAbout_Click(sender As Object, e As EventArgs) Handles menuAbout.Click
        If VerCode = "R" Then
            Dim about As New AboutBox1()
            about.Show()
        Else
            Me.txtFileText.Text = "PDF.NET 快速（数据）开发框架，支持ORM和SQL-MAP功能，支持表现层、业务逻辑层、数据访问层的快速数据开发。" & vbCrLf &
                                  "邓太华 2010.3"
            Me.lblFileName.Text = ""

        End If
    End Sub

    Private Sub menuSqlMapBuilder_Click(sender As Object, e As EventArgs) Handles menuSqlMapBuilder.Click
        RunProcessByConfig("SqlMapBuilderPath")
    End Sub

    Private Sub RunProcessByConfig(fileKey As String)

        Dim fileName As String = ConfigurationManager.AppSettings(fileKey)
        If Not Path.IsPathRooted(fileName) Then
            fileName = Path.Combine(Application.StartupPath, fileName)
        End If
        If File.Exists(fileName) Then
            Try
                Environment.CurrentDirectory = Path.GetDirectoryName(fileName)
                Process.Start(fileName)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "打开文件", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try


        Else
            MessageBox.Show("指定的程序未找到，程序名称：" + fileName, "打开文件", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub


    Private Sub OpenConfigFile(fileKey As String)
        Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory
        Dim fileName As String = ConfigurationManager.AppSettings(fileKey)
        fileName = fileName & ".config"
        If Not Path.IsPathRooted(fileName) Then
            fileName = Path.Combine(Application.StartupPath, fileName)
        End If
        If File.Exists(fileName) Then
            'System.Diagnostics.Process.Start("notepad", fileName)
            Me.txtFileText.Text = File.ReadAllText(fileName, Encoding.Default)
            Me.lblOptTile.Text = "配置文件管理"
            Me.lblFileName.Text = fileName

        Else
            MessageBox.Show("指定的文件未找到，文件名称：" + fileName, "打开文件", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub menuSqlMapCodeMaker_Click(sender As Object, e As EventArgs) Handles menuSqlMapCodeMaker.Click
        RunProcessByConfig("PDFCodeMakerPath")
    End Sub

    Private Sub menuEntityMaker_Click(sender As Object, e As EventArgs) Handles menuEntityMaker.Click
        RunProcessByConfig("EntityCodeMakerPath")
    End Sub

    Private Sub menuIDEConfig_Click(sender As Object, e As EventArgs) Handles menuIDEConfig.Click
        OpenConfigFile("PDFDotNETPath")
    End Sub

    Private Sub menuCodeMakerConfig_Click(sender As Object, e As EventArgs) Handles menuCodeMakerConfig.Click
        OpenConfigFile("PDFCodeMakerPath")
    End Sub

    Private Sub menuEntityMakerConfig_Click(sender As Object, e As EventArgs) Handles menuEntityMakerConfig.Click
        OpenConfigFile("EntityCodeMakerPath")
    End Sub


    Private Sub menuOpenSqlMapConfig_Click(sender As Object, e As EventArgs) Handles menuOpenSqlMapConfig.Click
        OpenMyFileDialog("配置文件|*.config|XML文件|*.xml")
    End Sub

    Private Sub menuOpenMakedCodeFile_Click(sender As Object, e As EventArgs) Handles menuOpenMakedCodeFile.Click
        OpenMyFileDialog("C#文件|*.cs|VB文件|*.vb|所有文件|*.*")
    End Sub

    Private Sub OpenMyFileDialog(filter As String)
        Me.OpenFileDialog1.FileName = ""
        Me.OpenFileDialog1.Filter = filter
        Me.OpenFileDialog1.ShowDialog()

        If Me.OpenFileDialog1.FileName <> "" Then
            'System.Diagnostics.Process.Start(Me.OpenFileDialog1.FileName)
            Me.txtFileText.Text = File.ReadAllText(Me.OpenFileDialog1.FileName, Encoding.Default)
            Me.lblOptTile.Text = "文件管理"
            Me.lblFileName.Text = Me.OpenFileDialog1.FileName

        End If
    End Sub

    Private Sub menuOnlineHelp_Click(sender As Object, e As EventArgs) Handles menuOnlineHelp.Click
        If VerCode = "R" Then
            Process.Start("http://www.pwmis.com/sqlmap")
        Else
            If ConfigurationManager.AppSettings("OnLineHelp") <> "" Then
                Process.Start(ConfigurationManager.AppSettings("OnLineHelp"))
            End If
        End If
    End Sub

    Private Sub menuPDFNetHelp_Click(sender As Object, e As EventArgs) Handles menuPDFNetHelp.Click
        Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory
        Dim fileName = "PdfNet.hlp"
        If File.Exists(fileName) Then
            Process.Start(fileName)

        Else
            MessageBox.Show("指定的文件未找到，文件名称：" + fileName, "打开文件", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If Me.lblFileName.Text <> "请选择相应菜单" And Me.lblFileName.Text <> "" Then
            Process.Start(Me.lblFileName.Text)
        End If
    End Sub


    Private Sub tsSqlMapper_Click(sender As Object, e As EventArgs) Handles tsSqlMapper.Click
        RunProcessByConfig("SqlMapBuilderPath")
    End Sub

    Private Sub tsDALCoder_Click(sender As Object, e As EventArgs) Handles tsDALCoder.Click
        RunProcessByConfig("PDFCodeMakerPath")
    End Sub

    Private Sub tsEntityCoder_Click(sender As Object, e As EventArgs) Handles tsEntityCoder.Click
        RunProcessByConfig("EntityCodeMakerPath")
    End Sub

    Private Sub Form1_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        Me.txtFileText.Width = Me.Width - 30
        Me.txtFileText.Height = Me.Height - 120
        Me.btnEdit.Left = Me.txtFileText.Width - 50
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        VerCode = ConfigurationManager.AppSettings("VerCode") '等于Release版才可以
        Me.txtFileText.Text = My.Computer.FileSystem.ReadAllText("verinfo.txt", Encoding.Default)
    End Sub

    Private Sub 版本信息ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 版本信息ToolStripMenuItem.Click
        Me.txtFileText.Text = My.Computer.FileSystem.ReadAllText("verinfo.txt", Encoding.Default)
    End Sub

    Private Sub menuSysConfig_Click(sender As Object, e As EventArgs) Handles menuSysConfig.Click
    End Sub
End Class
