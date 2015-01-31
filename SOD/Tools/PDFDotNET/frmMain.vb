Public Class frmMain
    Implements ICommand

    Dim tpDataBaseExpert As MdiTabControl.TabPage
    Dim tpSqlMapExpert As MdiTabControl.TabPage
    Dim tpPdfNetSlnExpert As MdiTabControl.TabPage

    Dim VerCode As String

    Private Sub menuOpenMakedCodeFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuOpenMakedCodeFile.Click
        Me.OpenFileDialog1.Filter = "C#文件|*.cs|VB文件|*.vb|所有文件|*.*"
        Me.OpenFileDialog1.ShowDialog()
        If Me.OpenFileDialog1.FileName <> "" Then
            Dim frmCF As New frmCodeFile
            frmCF.FileName = Me.OpenFileDialog1.FileName
            Me.TabControlMain.TabPages.Add(frmCF)
        End If

    End Sub

    Private Sub TabControlMain_GetTabRegion(ByVal sender As System.Object, ByVal e As MdiTabControl.TabControl.GetTabRegionEventArgs) Handles TabControlMain.GetTabRegion
        e.Points(1) = New Point(e.TabHeight - 2, 2)
        e.Points(2) = New Point(e.TabHeight + 2, 0)
    End Sub

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tpDataBaseExpert = Me.TabControlLeft.TabPages.Add(New frmDataBaseExpert(Me)) 'index=2
        tpSqlMapExpert = Me.TabControlLeft.TabPages.Add(New frmSqlMapExpert()) 'index=1
        tpPdfNetSlnExpert = Me.TabControlLeft.TabPages.Add(New frmPdfNetSlnExpert()) 'index=0

        VerCode = Configuration.ConfigurationManager.AppSettings("VerCode") '等于Release版才可以

        TestWriteCfg()
        TestReadCfg()
    End Sub

    Private Sub TestWriteCfg()
        Dim cfgItemList As New List(Of GenerateConfigItem)
        cfgItemList.Add(New GenerateConfigItem("E:\vs2008_project\PFT团队项目\FTS\FTS.DAL\SQLMap.config", "E:\vs2008_project\PFT团队项目\FTS\temp", "TestWebAppDAL"))
        cfgItemList.Add(New GenerateConfigItem("E:\\SQLMap.config", "E:\temp", "TestDAL"))

        Dim ser As New System.Xml.Serialization.XmlSerializer(cfgItemList.GetType())
        Dim writer As System.IO.TextWriter = System.IO.File.CreateText(".\GenerateConfig.xml")
        ser.Serialize(writer, cfgItemList)
        writer.Flush()
        writer.Close()


    End Sub


    Private Sub TestReadCfg()
        Dim cfgItemList As List(Of GenerateConfigItem) = Nothing
       
        Dim ser As New System.Xml.Serialization.XmlSerializer(GetType(List(Of GenerateConfigItem)))
        Dim reader As System.IO.TextReader = FileIO.FileSystem.OpenTextFileReader(".\GenerateConfig.xml")

        cfgItemList = ser.Deserialize(reader)


    End Sub

    Private Sub TabControlLeft_TabPaintBorder(ByVal sender As System.Object, ByVal e As MdiTabControl.TabControl.TabPaintEventArgs) Handles TabControlLeft.TabPaintBorder
        If Not e.Selected Then
            e.Handled = True
            e.Graphics.DrawLine(Pens.Azure, e.ClipRectangle.Width - 1, 2, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 3)
        End If
    End Sub

    Private Sub menuOpenSqlMapConfig_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuOpenSqlMapConfig.Click
        Dim sqlmapForm As New frmSqlMapFile
        Me.TabControlMain.TabPages.Add(sqlmapForm)

    End Sub

    Private Sub menuItemEditCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemEditCopy.Click
        ' MessageBox.Show("Ctrol C")
        SendKeys.Send("^C")

    End Sub

    Private Sub menuItemVConnExpert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemVConnExpert.Click
        'Me.TabControlLeft.TabPages(2).Select()
        tpDataBaseExpert.Select()
    End Sub

    Private Sub menuSqlMapExpert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSqlMapExpert.Click
        tpSqlMapExpert.Select()

    End Sub

    Private Sub menuItemPdfNetSln_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemPdfNetSln.Click
        tpPdfNetSlnExpert.Select()

    End Sub

    Public Function Command(ByVal commandName As String, ByVal parameters As Dictionary(Of String, Object)) As Boolean Implements ICommand.Command
        If commandName = "ShowPropertyForm" Then
            Me.TabControlMain.TabPages.Add(New frmPropertys())

        End If
    End Function

    Function OpenWindow(ByVal sender As Object, ByVal objectWindow As System.Windows.Forms.Form, ByVal openStyle As String) As Boolean Implements ICommand.OpenWindow
        Me.TabControlMain.TabPages.Add(objectWindow)
    End Function

    Private Sub tsSqlMapper_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsSqlMapper.Click
        RunProcessByConfig("SqlMapBuilderPath")
    End Sub

    Private Sub tsDALCoder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsDALCoder.Click
        RunProcessByConfig("PDFCodeMakerPath")
    End Sub

    Private Sub tsEntityCoder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsEntityCoder.Click
        'RunProcessByConfig("EntityCodeMakerPath")
        tpDataBaseExpert.Select()
        Dim objForm As frmDataBaseExpert = tpDataBaseExpert.Form
        If Not objForm.CreateEntity() Then
            MessageBox.Show("请选择一个或多个数据表或者视图，然后在右键菜单中选择生成实体类。", "实体类生成器", MessageBoxButtons.OK, MessageBoxIcon.Information)

        End If
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

    Private Sub 查询窗口ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 查询窗口ToolStripMenuItem.Click
        If Module1.CurrentDataBase IsNot Nothing Then
            Me.TabControlMain.TabPages.Add(New frmDataQuery(Module1.CurrentDataBase))
        Else
            MessageBox.Show("请先打开一个数据库连接！")

        End If

    End Sub

    Private Sub menuItemEditCut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemEditCut.Click
        SendKeys.Send("^X")
    End Sub

    Private Sub menuItemEditPast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemEditPast.Click
        SendKeys.Send("^V")
    End Sub

    Private Sub menuItemUnDo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemUnDo.Click
        SendKeys.Send("^Z")
    End Sub

    Private Sub tsBtnRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsBtnRun.Click
        SendKeys.Send("{F5}")
        SendKeys.Flush()
        Application.DoEvents()
    End Sub

    Private Sub tsmItemSaveFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmItemSaveFile.Click
        SendKeys.Send("^S")
        '无效？
    End Sub

    Private Sub menuAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuAbout.Click
        If VerCode = "R" Then
            Dim about As New AboutBox1()
            about.Show()
        Else
            Dim msg As String = "PDF.NET 快速（数据）开发框架，支持ORM和SQL-MAP功能，支持表现层、业务逻辑层、数据访问层的快速数据开发。" & vbCrLf & _
            "邓太华 2010.3"
            MessageBox.Show(msg, "PDF.NET 数据开发框架")

        End If
    End Sub

    Private Sub menuPDFNetHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuPDFNetHelp.Click
        System.Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory
        Dim fileName As String = "pdf.net.chm"
        If System.IO.File.Exists(fileName) Then
            System.Diagnostics.Process.Start(fileName)

        Else
            MessageBox.Show("指定的文件未找到，文件名称：" + fileName, "打开文件", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    Private Sub menuSqlMapBuilder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSqlMapBuilder.Click
        RunProcessByConfig("SqlMapBuilderPath")
    End Sub

    Private Sub menuSqlMapCodeMaker_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSqlMapCodeMaker.Click
        RunProcessByConfig("PDFCodeMakerPath")
    End Sub

    Private Sub menuEntityMaker_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuEntityMaker.Click
        tpDataBaseExpert.Select()
        Dim objForm As frmDataBaseExpert = tpDataBaseExpert.Form
        If Not objForm.CreateEntity() Then
            MessageBox.Show("请选择一个或多个数据表或者视图，然后在右键菜单中选择生成实体类。", "实体类生成器", MessageBoxButtons.OK, MessageBoxIcon.Information)

        End If
    End Sub

    Private Sub menuCodeMakerConfig_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuCodeMakerConfig.Click
        OpenConfigFile("PDFCodeMakerPath")
    End Sub

    Private Sub OpenConfigFile(ByVal fileKey As String)
        System.Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory
        Dim fileName As String = Configuration.ConfigurationManager.AppSettings(fileKey)
        fileName = fileName & ".config"
        If System.IO.File.Exists(fileName) Then
            'System.Diagnostics.Process.Start("notepad", fileName)
            'Me.txtFileText.Text = System.IO.File.ReadAllText(fileName, System.Text.Encoding.Default)
            'Me.lblOptTile.Text = "配置文件管理"
            'Me.lblFileName.Text = fileName

            Dim frmCF As New frmCodeFile
            frmCF.FileName = fileName
            Me.TabControlMain.TabPages.Add(frmCF)

        Else
            MessageBox.Show("指定的文件未找到，文件名称：" + fileName, "打开文件", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub menuIDEConfig_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuIDEConfig.Click
        OpenConfigFile("PDFDotNETPath")
    End Sub
End Class