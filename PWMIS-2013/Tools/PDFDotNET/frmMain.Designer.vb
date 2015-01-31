<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Dim ToolStripSystemRenderer5 As System.Windows.Forms.ToolStripSystemRenderer = New System.Windows.Forms.ToolStripSystemRenderer
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.文件ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem
        Me.解决方案文件ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SQLMAP文件ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.查询窗口ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.打开ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.menuOpenSqlMapConfig = New System.Windows.Forms.ToolStripMenuItem
        Me.menuOpenMakedCodeFile = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmItemSaveFile = New System.Windows.Forms.ToolStripMenuItem
        Me.退出ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.menuItemUnDo = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.menuItemEditCut = New System.Windows.Forms.ToolStripMenuItem
        Me.menuItemEditCopy = New System.Windows.Forms.ToolStripMenuItem
        Me.menuItemEditPast = New System.Windows.Forms.ToolStripMenuItem
        Me.menuItemEditDelete = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem
        Me.menuItemVConnExpert = New System.Windows.Forms.ToolStripMenuItem
        Me.menuSqlMapExpert = New System.Windows.Forms.ToolStripMenuItem
        Me.menuItemPdfNetSln = New System.Windows.Forms.ToolStripMenuItem
        Me.T工具ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.menuSqlMapBuilder = New System.Windows.Forms.ToolStripMenuItem
        Me.menuSqlMapCodeMaker = New System.Windows.Forms.ToolStripMenuItem
        Me.menuEntityMaker = New System.Windows.Forms.ToolStripMenuItem
        Me.配置ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.menuIDEConfig = New System.Windows.Forms.ToolStripMenuItem
        Me.menuCodeMakerConfig = New System.Windows.Forms.ToolStripMenuItem
        Me.menuEntityMakerConfig = New System.Windows.Forms.ToolStripMenuItem
        Me.menuSysConfig = New System.Windows.Forms.ToolStripMenuItem
        Me.H帮助ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.menuAbout = New System.Windows.Forms.ToolStripMenuItem
        Me.menuPDFNetHelp = New System.Windows.Forms.ToolStripMenuItem
        Me.menuOnlineHelp = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.tsSqlMapper = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.tsDALCoder = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.tsEntityCoder = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me.tsBtnRun = New System.Windows.Forms.ToolStripButton
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.TabControlLeft = New MdiTabControl.TabControl
        Me.TabControlMain = New MdiTabControl.TabControl
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.MenuStrip1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.文件ToolStripMenuItem, Me.ToolStripMenuItem1, Me.ToolStripMenuItem2, Me.T工具ToolStripMenuItem, Me.配置ToolStripMenuItem, Me.H帮助ToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(693, 25)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        '文件ToolStripMenuItem
        '
        Me.文件ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem3, Me.打开ToolStripMenuItem, Me.tsmItemSaveFile, Me.退出ToolStripMenuItem})
        Me.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem"
        Me.文件ToolStripMenuItem.Size = New System.Drawing.Size(54, 21)
        Me.文件ToolStripMenuItem.Text = "&F 文件"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.解决方案文件ToolStripMenuItem, Me.SQLMAP文件ToolStripMenuItem, Me.查询窗口ToolStripMenuItem})
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(168, 22)
        Me.ToolStripMenuItem3.Text = "&N 新建"
        '
        '解决方案文件ToolStripMenuItem
        '
        Me.解决方案文件ToolStripMenuItem.Name = "解决方案文件ToolStripMenuItem"
        Me.解决方案文件ToolStripMenuItem.Size = New System.Drawing.Size(155, 22)
        Me.解决方案文件ToolStripMenuItem.Text = "解决方案文件"
        '
        'SQLMAP文件ToolStripMenuItem
        '
        Me.SQLMAP文件ToolStripMenuItem.Name = "SQLMAP文件ToolStripMenuItem"
        Me.SQLMAP文件ToolStripMenuItem.Size = New System.Drawing.Size(155, 22)
        Me.SQLMAP文件ToolStripMenuItem.Text = "SQL-MAP文件"
        '
        '查询窗口ToolStripMenuItem
        '
        Me.查询窗口ToolStripMenuItem.Name = "查询窗口ToolStripMenuItem"
        Me.查询窗口ToolStripMenuItem.Size = New System.Drawing.Size(155, 22)
        Me.查询窗口ToolStripMenuItem.Text = "查询窗口"
        '
        '打开ToolStripMenuItem
        '
        Me.打开ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menuOpenSqlMapConfig, Me.menuOpenMakedCodeFile})
        Me.打开ToolStripMenuItem.Name = "打开ToolStripMenuItem"
        Me.打开ToolStripMenuItem.Size = New System.Drawing.Size(168, 22)
        Me.打开ToolStripMenuItem.Text = "&O 打开"
        '
        'menuOpenSqlMapConfig
        '
        Me.menuOpenSqlMapConfig.Name = "menuOpenSqlMapConfig"
        Me.menuOpenSqlMapConfig.Size = New System.Drawing.Size(187, 22)
        Me.menuOpenSqlMapConfig.Text = "SQL-MAP脚本文件"
        '
        'menuOpenMakedCodeFile
        '
        Me.menuOpenMakedCodeFile.Name = "menuOpenMakedCodeFile"
        Me.menuOpenMakedCodeFile.Size = New System.Drawing.Size(187, 22)
        Me.menuOpenMakedCodeFile.Text = "生成的.NET代码文件"
        '
        'tsmItemSaveFile
        '
        Me.tsmItemSaveFile.Name = "tsmItemSaveFile"
        Me.tsmItemSaveFile.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.tsmItemSaveFile.Size = New System.Drawing.Size(168, 22)
        Me.tsmItemSaveFile.Text = "保存文件"
        '
        '退出ToolStripMenuItem
        '
        Me.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem"
        Me.退出ToolStripMenuItem.Size = New System.Drawing.Size(168, 22)
        Me.退出ToolStripMenuItem.Text = "&X 退出"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menuItemUnDo, Me.ToolStripSeparator3, Me.menuItemEditCut, Me.menuItemEditCopy, Me.menuItemEditPast, Me.menuItemEditDelete})
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(55, 21)
        Me.ToolStripMenuItem1.Text = "&E 编辑"
        '
        'menuItemUnDo
        '
        Me.menuItemUnDo.Name = "menuItemUnDo"
        Me.menuItemUnDo.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Z), System.Windows.Forms.Keys)
        Me.menuItemUnDo.Size = New System.Drawing.Size(145, 22)
        Me.menuItemUnDo.Text = "撤销"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(142, 6)
        '
        'menuItemEditCut
        '
        Me.menuItemEditCut.Name = "menuItemEditCut"
        Me.menuItemEditCut.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.menuItemEditCut.Size = New System.Drawing.Size(145, 22)
        Me.menuItemEditCut.Text = "剪切"
        '
        'menuItemEditCopy
        '
        Me.menuItemEditCopy.Name = "menuItemEditCopy"
        Me.menuItemEditCopy.ShortcutKeyDisplayString = "Ctrl+C"
        Me.menuItemEditCopy.Size = New System.Drawing.Size(145, 22)
        Me.menuItemEditCopy.Text = "复制"
        '
        'menuItemEditPast
        '
        Me.menuItemEditPast.Name = "menuItemEditPast"
        Me.menuItemEditPast.ShortcutKeyDisplayString = "Ctrl+V"
        Me.menuItemEditPast.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.menuItemEditPast.Size = New System.Drawing.Size(145, 22)
        Me.menuItemEditPast.Text = "粘贴"
        '
        'menuItemEditDelete
        '
        Me.menuItemEditDelete.Name = "menuItemEditDelete"
        Me.menuItemEditDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete
        Me.menuItemEditDelete.Size = New System.Drawing.Size(145, 22)
        Me.menuItemEditDelete.Text = "删除"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menuItemVConnExpert, Me.menuSqlMapExpert, Me.menuItemPdfNetSln})
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(56, 21)
        Me.ToolStripMenuItem2.Text = "&V 视图"
        '
        'menuItemVConnExpert
        '
        Me.menuItemVConnExpert.Name = "menuItemVConnExpert"
        Me.menuItemVConnExpert.Size = New System.Drawing.Size(191, 22)
        Me.menuItemVConnExpert.Text = "数据库资源管理器"
        '
        'menuSqlMapExpert
        '
        Me.menuSqlMapExpert.Name = "menuSqlMapExpert"
        Me.menuSqlMapExpert.Size = New System.Drawing.Size(191, 22)
        Me.menuSqlMapExpert.Text = "SQL-MAP资源管理器"
        '
        'menuItemPdfNetSln
        '
        Me.menuItemPdfNetSln.Name = "menuItemPdfNetSln"
        Me.menuItemPdfNetSln.Size = New System.Drawing.Size(191, 22)
        Me.menuItemPdfNetSln.Text = "PDF.NET解决方案"
        '
        'T工具ToolStripMenuItem
        '
        Me.T工具ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menuSqlMapBuilder, Me.menuSqlMapCodeMaker, Me.menuEntityMaker})
        Me.T工具ToolStripMenuItem.Name = "T工具ToolStripMenuItem"
        Me.T工具ToolStripMenuItem.Size = New System.Drawing.Size(55, 21)
        Me.T工具ToolStripMenuItem.Text = "&T 工具"
        '
        'menuSqlMapBuilder
        '
        Me.menuSqlMapBuilder.Name = "menuSqlMapBuilder"
        Me.menuSqlMapBuilder.Size = New System.Drawing.Size(231, 22)
        Me.menuSqlMapBuilder.Text = "&M SQL-MAP配置文件管理器"
        '
        'menuSqlMapCodeMaker
        '
        Me.menuSqlMapCodeMaker.Name = "menuSqlMapCodeMaker"
        Me.menuSqlMapCodeMaker.Size = New System.Drawing.Size(231, 22)
        Me.menuSqlMapCodeMaker.Text = "&C SQL-MAP代码生成器"
        '
        'menuEntityMaker
        '
        Me.menuEntityMaker.Name = "menuEntityMaker"
        Me.menuEntityMaker.Size = New System.Drawing.Size(231, 22)
        Me.menuEntityMaker.Text = "&E PDF.NET实体类生成器"
        '
        '配置ToolStripMenuItem
        '
        Me.配置ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menuIDEConfig, Me.menuCodeMakerConfig, Me.menuEntityMakerConfig, Me.menuSysConfig})
        Me.配置ToolStripMenuItem.Name = "配置ToolStripMenuItem"
        Me.配置ToolStripMenuItem.Size = New System.Drawing.Size(57, 21)
        Me.配置ToolStripMenuItem.Text = "&G 配置"
        '
        'menuIDEConfig
        '
        Me.menuIDEConfig.Name = "menuIDEConfig"
        Me.menuIDEConfig.Size = New System.Drawing.Size(215, 22)
        Me.menuIDEConfig.Text = "集成管理配置"
        '
        'menuCodeMakerConfig
        '
        Me.menuCodeMakerConfig.Name = "menuCodeMakerConfig"
        Me.menuCodeMakerConfig.Size = New System.Drawing.Size(215, 22)
        Me.menuCodeMakerConfig.Text = "SQL-MAP代码生成器配置"
        '
        'menuEntityMakerConfig
        '
        Me.menuEntityMakerConfig.Name = "menuEntityMakerConfig"
        Me.menuEntityMakerConfig.Size = New System.Drawing.Size(215, 22)
        Me.menuEntityMakerConfig.Text = "实体类生成器配置"
        '
        'menuSysConfig
        '
        Me.menuSysConfig.Name = "menuSysConfig"
        Me.menuSysConfig.Size = New System.Drawing.Size(215, 22)
        Me.menuSysConfig.Text = "系统配置"
        '
        'H帮助ToolStripMenuItem
        '
        Me.H帮助ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menuAbout, Me.menuPDFNetHelp, Me.menuOnlineHelp})
        Me.H帮助ToolStripMenuItem.Name = "H帮助ToolStripMenuItem"
        Me.H帮助ToolStripMenuItem.Size = New System.Drawing.Size(57, 21)
        Me.H帮助ToolStripMenuItem.Text = "&H 帮助"
        '
        'menuAbout
        '
        Me.menuAbout.Name = "menuAbout"
        Me.menuAbout.Size = New System.Drawing.Size(149, 22)
        Me.menuAbout.Text = "关于"
        '
        'menuPDFNetHelp
        '
        Me.menuPDFNetHelp.Name = "menuPDFNetHelp"
        Me.menuPDFNetHelp.Size = New System.Drawing.Size(149, 22)
        Me.menuPDFNetHelp.Text = "PDF.NET帮助"
        '
        'menuOnlineHelp
        '
        Me.menuOnlineHelp.Name = "menuOnlineHelp"
        Me.menuOnlineHelp.Size = New System.Drawing.Size(149, 22)
        Me.menuOnlineHelp.Text = "在线支持"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsSqlMapper, Me.ToolStripSeparator1, Me.tsDALCoder, Me.ToolStripSeparator2, Me.tsEntityCoder, Me.ToolStripSeparator4, Me.tsBtnRun})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 25)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(693, 25)
        Me.ToolStrip1.TabIndex = 6
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsSqlMapper
        '
        Me.tsSqlMapper.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsSqlMapper.Image = Global.PDFDotNET.My.Resources.Resources.sql
        Me.tsSqlMapper.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsSqlMapper.Name = "tsSqlMapper"
        Me.tsSqlMapper.Size = New System.Drawing.Size(23, 22)
        Me.tsSqlMapper.Text = "SQL-MAP配置文件管理"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'tsDALCoder
        '
        Me.tsDALCoder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsDALCoder.Image = Global.PDFDotNET.My.Resources.Resources.SqlQuery
        Me.tsDALCoder.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsDALCoder.Name = "tsDALCoder"
        Me.tsDALCoder.Size = New System.Drawing.Size(23, 22)
        Me.tsDALCoder.Text = "SQL-MAP代码生成器"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'tsEntityCoder
        '
        Me.tsEntityCoder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsEntityCoder.Image = Global.PDFDotNET.My.Resources.Resources.field
        Me.tsEntityCoder.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsEntityCoder.Name = "tsEntityCoder"
        Me.tsEntityCoder.Size = New System.Drawing.Size(23, 22)
        Me.tsEntityCoder.Text = "实体类生成器"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 25)
        '
        'tsBtnRun
        '
        Me.tsBtnRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsBtnRun.Image = CType(resources.GetObject("tsBtnRun.Image"), System.Drawing.Image)
        Me.tsBtnRun.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsBtnRun.Name = "tsBtnRun"
        Me.tsBtnRun.Size = New System.Drawing.Size(23, 22)
        Me.tsBtnRun.Text = "运行"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BackColor = System.Drawing.SystemColors.ButtonShadow
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 50)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.TabControlLeft)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.TabControlMain)
        Me.SplitContainer1.Size = New System.Drawing.Size(693, 423)
        Me.SplitContainer1.SplitterDistance = 275
        Me.SplitContainer1.TabIndex = 7
        '
        'TabControlLeft
        '
        Me.TabControlLeft.Alignment = MdiTabControl.TabControl.TabAlignment.Top
        Me.TabControlLeft.AllowTabReorder = False
        Me.TabControlLeft.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlLeft.DropButtonVisible = False
        Me.TabControlLeft.FontBoldOnSelect = False
        Me.TabControlLeft.Location = New System.Drawing.Point(0, 0)
        Me.TabControlLeft.MenuRenderer = Nothing
        Me.TabControlLeft.Name = "TabControlLeft"
        Me.TabControlLeft.Size = New System.Drawing.Size(275, 423)
        Me.TabControlLeft.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
        Me.TabControlLeft.TabBorderEnhanceWeight = MdiTabControl.TabControl.Weight.Medium
        Me.TabControlLeft.TabCloseButtonImage = Nothing
        Me.TabControlLeft.TabCloseButtonImageDisabled = Nothing
        Me.TabControlLeft.TabCloseButtonImageHot = Nothing
        Me.TabControlLeft.TabCloseButtonVisible = False
        Me.TabControlLeft.TabHeight = 18
        Me.TabControlLeft.TabIconSize = New System.Drawing.Size(12, 12)
        Me.TabControlLeft.TabIndex = 0
        Me.TabControlLeft.TabMaximumWidth = 100
        Me.TabControlLeft.TabMinimumWidth = 50
        Me.TabControlLeft.TabOffset = -1
        Me.TabControlLeft.TabPadLeft = 2
        Me.TabControlLeft.TabPadRight = 3
        Me.TabControlLeft.TabsDirection = MdiTabControl.TabControl.FlowDirection.LeftToRight
        '
        'TabControlMain
        '
        Me.TabControlMain.Alignment = MdiTabControl.TabControl.TabAlignment.Top
        Me.TabControlMain.BackHighColor = System.Drawing.Color.Transparent
        Me.TabControlMain.BackLowColor = System.Drawing.Color.Transparent
        Me.TabControlMain.CloseButtonVisible = True
        Me.TabControlMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlMain.Location = New System.Drawing.Point(0, 0)
        Me.TabControlMain.MenuRenderer = ToolStripSystemRenderer5
        Me.TabControlMain.Name = "TabControlMain"
        Me.TabControlMain.Size = New System.Drawing.Size(414, 423)
        Me.TabControlMain.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed
        Me.TabControlMain.TabBorderEnhanced = True
        Me.TabControlMain.TabBorderEnhanceWeight = MdiTabControl.TabControl.Weight.Soft
        Me.TabControlMain.TabCloseButtonImage = Nothing
        Me.TabControlMain.TabCloseButtonImageDisabled = Nothing
        Me.TabControlMain.TabCloseButtonImageHot = Nothing
        Me.TabControlMain.TabCloseButtonSize = New System.Drawing.Size(14, 14)
        Me.TabControlMain.TabCloseButtonVisible = False
        Me.TabControlMain.TabHeight = 18
        Me.TabControlMain.TabIndex = 6
        Me.TabControlMain.TabOffset = -8
        Me.TabControlMain.TabPadLeft = 20
        Me.TabControlMain.TabsDirection = MdiTabControl.TabControl.FlowDirection.LeftToRight
        Me.TabControlMain.TabTop = 1
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(693, 473)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Name = "frmMain"
        Me.Text = "PDF.NET 数据开发框架 -- 集成开发环境"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents 文件ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 打开ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuOpenSqlMapConfig As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuOpenMakedCodeFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 退出ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents T工具ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuSqlMapBuilder As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuSqlMapCodeMaker As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuEntityMaker As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 配置ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuIDEConfig As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuCodeMakerConfig As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuEntityMakerConfig As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuSysConfig As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents H帮助ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuAbout As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuPDFNetHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuOnlineHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsSqlMapper As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsDALCoder As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsEntityCoder As System.Windows.Forms.ToolStripButton
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents TabControlMain As MdiTabControl.TabControl
    Friend WithEvents TabControlLeft As MdiTabControl.TabControl
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemVConnExpert As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuSqlMapExpert As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemPdfNetSln As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemEditCut As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemEditCopy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemEditPast As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemEditDelete As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 解决方案文件ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SQLMAP文件ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 查询窗口ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemUnDo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsBtnRun As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsmItemSaveFile As System.Windows.Forms.ToolStripMenuItem
End Class
