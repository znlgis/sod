Imports System.Configuration
Imports System.IO
Imports System.Xml.Serialization
Imports MdiTabControl
Imports Microsoft.VisualBasic.FileIO

Public Class frmMain
    Implements ICommand

    Dim tpDataBaseExpert As TabPage
    Dim tpSqlMapExpert As TabPage
    Dim tpPdfNetSlnExpert As TabPage

    Dim VerCode As String

    Private Sub menuOpenMakedCodeFile_Click(sender As Object, e As EventArgs) Handles menuOpenMakedCodeFile.Click
        Me.OpenFileDialog1.Filter = "C#文件|*.cs|VB文件|*.vb|所有文件|*.*"
        Me.OpenFileDialog1.ShowDialog()
        If Me.OpenFileDialog1.FileName <> "" Then
            Dim frmCF As New frmCodeFile
            frmCF.FileName = Me.OpenFileDialog1.FileName
            Me.TabControlMain.TabPages.Add(frmCF)
        End If
    End Sub

    Private Sub TabControlMain_GetTabRegion(sender As Object, e As TabControl.GetTabRegionEventArgs) _
        Handles TabControlMain.GetTabRegion
        e.Points(1) = New Point(e.TabHeight - 2, 2)
        e.Points(2) = New Point(e.TabHeight + 2, 0)
    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        tpDataBaseExpert = Me.TabControlLeft.TabPages.Add(New frmDataBaseExpert(Me)) 'index=2
        tpSqlMapExpert = Me.TabControlLeft.TabPages.Add(New frmSqlMapExpert()) 'index=1
        tpPdfNetSlnExpert = Me.TabControlLeft.TabPages.Add(New frmPdfNetSlnExpert()) 'index=0

        VerCode = ConfigurationManager.AppSettings("VerCode") '等于Release版才可以

        TestWriteCfg()
        TestReadCfg()

        Dim welcomForm As New frmWelcom
        welcomForm.CommandForm = Me
        welcomForm.ParentContainer = Me.TabControlMain
        Me.TabControlMain.SetControlsSizeLocation()
        Me.TabControlMain.TabPages.Add(welcomForm)
    End Sub

    Private Sub TestWriteCfg()
        Dim cfgItemList As New List(Of GenerateConfigItem)
        cfgItemList.Add(New GenerateConfigItem("E:\vs2008_project\PFT团队项目\FTS\FTS.DAL\SQLMap.config",
                                               "E:\vs2008_project\PFT团队项目\FTS\temp", "TestWebAppDAL"))
        cfgItemList.Add(New GenerateConfigItem("E:\\SQLMap.config", "E:\temp", "TestDAL"))

        Dim ser As New XmlSerializer(cfgItemList.GetType())
        Dim writer As TextWriter = File.CreateText(".\GenerateConfig.xml")
        ser.Serialize(writer, cfgItemList)
        writer.Flush()
        writer.Close()
    End Sub


    Private Sub TestReadCfg()
        Dim cfgItemList As List(Of GenerateConfigItem) = Nothing

        Dim ser As New XmlSerializer(GetType(List(Of GenerateConfigItem)))
        Dim reader As TextReader = FileSystem.OpenTextFileReader(".\GenerateConfig.xml")

        cfgItemList = ser.Deserialize(reader)
    End Sub

    Private Sub TabControlLeft_TabPaintBorder(sender As Object, e As TabControl.TabPaintEventArgs) _
        Handles TabControlLeft.TabPaintBorder
        If Not e.Selected Then
            e.Handled = True
            e.Graphics.DrawLine(Pens.Azure, e.ClipRectangle.Width - 1, 2, e.ClipRectangle.Width - 1,
                                e.ClipRectangle.Height - 3)
        End If
    End Sub

    Private Sub menuOpenSqlMapConfig_Click(sender As Object, e As EventArgs) Handles menuOpenSqlMapConfig.Click
        Dim sqlmapForm As New frmSqlMapFile
        Me.TabControlMain.TabPages.Add(sqlmapForm)
    End Sub

    Private Sub menuItemEditCopy_Click(sender As Object, e As EventArgs) Handles menuItemEditCopy.Click
        ' MessageBox.Show("Ctrol C")
        SendKeys.Send("^C")
    End Sub

    Private Sub menuItemVConnExpert_Click(sender As Object, e As EventArgs) Handles menuItemVConnExpert.Click
        'Me.TabControlLeft.TabPages(2).Select()
        tpDataBaseExpert.Select()
    End Sub

    Private Sub menuSqlMapExpert_Click(sender As Object, e As EventArgs) Handles menuSqlMapExpert.Click
        tpSqlMapExpert.Select()
    End Sub

    Private Sub menuItemPdfNetSln_Click(sender As Object, e As EventArgs) Handles menuItemPdfNetSln.Click
        tpPdfNetSlnExpert.Select()
    End Sub

    Public Function Command(commandName As String, parameters As Dictionary(Of String, Object)) As Boolean _
        Implements ICommand.Command
        If commandName = "ShowPropertyForm" Then
            Me.TabControlMain.TabPages.Add(New frmPropertys())
        ElseIf commandName = "TopForm" Then
            Dim myAction As Action = Sub()
                Me.WindowState = FormWindowState.Maximized
                Dim form As Form = parameters("sender")
                Me.TabControlMain.TabPages(form).Select()
                form.Select()
            End Sub
            Me.Invoke(myAction)
        ElseIf commandName = "OpreationStatus" Then
            Dim myAction As Action = Sub()
                Me.WindowState = FormWindowState.Maximized
                Dim message As String = parameters("OpreationStatusMsg")
                OpreationStatusMsg.Text = message
            End Sub
            Me.Invoke(myAction)
        End If
    End Function

    Function OpenWindow(sender As Object, objectWindow As Form, openStyle As String) As Boolean _
        Implements ICommand.OpenWindow
        Dim page As TabPage = Me.TabControlMain.TabPages.Add(objectWindow)
        AddHandler page.Click, AddressOf TabControlMainPage_Click
        '重新调整Tab窗体大小，否则在工具条容器内显示不完全
        ResizeTabControlMainWindow(objectWindow)
    End Function

    Private Sub TabControlMainPage_Click(sender As Object, e As EventArgs)
        Dim page As TabPage = sender
        Dim objectWindow As Form = page.Form
        ResizeTabControlMainWindow(objectWindow)
    End Sub

    Private Sub ResizeTabControlMainWindow(objectWindow As Form)
        objectWindow.Dock = DockStyle.None
        Dim cSize As Size = SplitContainer1.Panel2.Size
        objectWindow.Size = New Size(cSize.Width - 3, cSize.Height - 23)
    End Sub

    Private Sub tsSqlMapper_Click(sender As Object, e As EventArgs) Handles tsSqlMapper.Click
        RunProcessByConfig("SqlMapBuilderPath")
    End Sub

    Private Sub tsDALCoder_Click(sender As Object, e As EventArgs) Handles tsDALCoder.Click
        RunProcessByConfig("PDFCodeMakerPath")
    End Sub

    Private Sub tsEntityCoder_Click(sender As Object, e As EventArgs) Handles tsEntityCoder.Click
        'RunProcessByConfig("EntityCodeMakerPath")
        tpDataBaseExpert.Select()
        Dim objForm As frmDataBaseExpert = tpDataBaseExpert.Form
        If Not objForm.CreateEntity() Then
            MessageBox.Show("请选择一个或多个数据表或者视图，然后在右键菜单中选择生成实体类。", "实体类生成器", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)

        End If
    End Sub

    Private Sub RunProcessByConfig(fileKey As String)

        Dim fileName As String = ConfigurationManager.AppSettings(fileKey)
        If Not Path.IsPathRooted(fileName) Then
            fileName = Path.Combine(Application.StartupPath, fileName)
        End If
        If File.Exists(fileName) Then
            Try
                Environment.CurrentDirectory = Path.GetDirectoryName(fileName)
                Dim currFileName As String = Path.GetFileName(fileName)
                Process.Start(currFileName)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "打开文件", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try


        Else
            MessageBox.Show("指定的程序未找到，程序名称：" + fileName, "打开文件", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub 查询窗口ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 查询窗口ToolStripMenuItem.Click
        If CurrentDataBase IsNot Nothing Then
            Me.TabControlMain.TabPages.Add(New frmDataQuery(CurrentDataBase))
        Else
            MessageBox.Show("请先打开一个数据库连接！")

        End If
    End Sub

    Private Sub menuItemEditCut_Click(sender As Object, e As EventArgs) Handles menuItemEditCut.Click
        SendKeys.Send("^X")
    End Sub

    Private Sub menuItemEditPast_Click(sender As Object, e As EventArgs) Handles menuItemEditPast.Click
        SendKeys.Send("^V")
    End Sub

    Private Sub menuItemUnDo_Click(sender As Object, e As EventArgs) Handles menuItemUnDo.Click
        SendKeys.Send("^Z")
    End Sub

    Private Sub tsBtnRun_Click(sender As Object, e As EventArgs) Handles tsBtnRun.Click
        SendKeys.Send("{F5}")
        SendKeys.Flush()
        Application.DoEvents()
    End Sub

    Private Sub tsmItemSaveFile_Click(sender As Object, e As EventArgs) Handles tsmItemSaveFile.Click
        SendKeys.Send("^S")
        '无效？
    End Sub

    Private Sub menuAbout_Click(sender As Object, e As EventArgs) Handles menuAbout.Click
        If VerCode = "R" Then
            Dim about As New AboutBox1()
            about.Show()
        Else
            Dim msg As String = "PDF.NET 快速（数据）开发框架，支持ORM和SQL-MAP功能，支持表现层、业务逻辑层、数据访问层的快速数据开发。" & vbCrLf &
                                "邓太华 2010.3"
            MessageBox.Show(msg, "PDF.NET 数据开发框架")

        End If
    End Sub

    Private Sub menuPDFNetHelp_Click(sender As Object, e As EventArgs) Handles menuPDFNetHelp.Click
        Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory
        Dim fileName = "pdf.net.chm"
        If File.Exists(fileName) Then
            Process.Start(fileName)

        Else
            MessageBox.Show("指定的文件未找到，文件名称：" + fileName, "打开文件", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    Private Sub menuSqlMapBuilder_Click(sender As Object, e As EventArgs) Handles menuSqlMapBuilder.Click
        RunProcessByConfig("SqlMapBuilderPath")
    End Sub

    Private Sub menuSqlMapCodeMaker_Click(sender As Object, e As EventArgs) Handles menuSqlMapCodeMaker.Click
        RunProcessByConfig("PDFCodeMakerPath")
    End Sub

    Private Sub menuEntityMaker_Click(sender As Object, e As EventArgs) Handles menuEntityMaker.Click
        tpDataBaseExpert.Select()
        Dim objForm As frmDataBaseExpert = tpDataBaseExpert.Form
        If Not objForm.CreateEntity() Then
            MessageBox.Show("请选择一个或多个数据表或者视图，然后在右键菜单中选择生成实体类。", "实体类生成器", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)

        End If
    End Sub

    Private Sub menuCodeMakerConfig_Click(sender As Object, e As EventArgs) Handles menuCodeMakerConfig.Click
        OpenConfigFile("PDFCodeMakerPath")
    End Sub

    Private Sub OpenConfigFile(fileKey As String)
        Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory
        Dim fileName As String = ConfigurationManager.AppSettings(fileKey)
        fileName = fileName & ".config"
        If File.Exists(fileName) Then
            'System.Diagnostics.Process.Start("notepad", fileName)
            'Me.txtFileText.Text = System.IO.File.ReadAllText(fileName, System.Text.Encoding.Default)
            'Me.lblOptTile.Text = "配置文件管理"
            'Me.lblFileName.Text = fileName

            Dim frmCF As New frmCodeFile
            frmCF.FileName = fileName
            frmCF.Text = fileName

            Me.TabControlMain.TabPages.Add(frmCF)

        Else
            MessageBox.Show("指定的文件未找到，文件名称：" + fileName, "打开文件", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub menuIDEConfig_Click(sender As Object, e As EventArgs) Handles menuIDEConfig.Click
        OpenConfigFile("PDFDotNETPath")
    End Sub

    Private Sub menuSODBrowser_Click(sender As Object, e As EventArgs) Handles menuSODBrowser.Click
        Dim welcomForm As New frmWelcom
        welcomForm.CommandForm = Me
        Me.TabControlMain.TabPages.Add(welcomForm)
    End Sub
End Class