<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.文件ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.打开ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.menuOpenSqlMapConfig = New System.Windows.Forms.ToolStripMenuItem
        Me.menuOpenMakedCodeFile = New System.Windows.Forms.ToolStripMenuItem
        Me.退出ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
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
        Me.版本信息ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.lblOptTile = New System.Windows.Forms.Label
        Me.lblFileName = New System.Windows.Forms.Label
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.tsSqlMapper = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.tsDALCoder = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.tsEntityCoder = New System.Windows.Forms.ToolStripButton
        Me.btnEdit = New System.Windows.Forms.Button
        Me.txtFileText = New System.Windows.Forms.RichTextBox
        Me.MenuStrip1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.文件ToolStripMenuItem, Me.T工具ToolStripMenuItem, Me.配置ToolStripMenuItem, Me.H帮助ToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(940, 25)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        '文件ToolStripMenuItem
        '
        Me.文件ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.打开ToolStripMenuItem, Me.退出ToolStripMenuItem})
        Me.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem"
        Me.文件ToolStripMenuItem.Size = New System.Drawing.Size(54, 21)
        Me.文件ToolStripMenuItem.Text = "&F 文件"
        '
        '打开ToolStripMenuItem
        '
        Me.打开ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menuOpenSqlMapConfig, Me.menuOpenMakedCodeFile})
        Me.打开ToolStripMenuItem.Name = "打开ToolStripMenuItem"
        Me.打开ToolStripMenuItem.Size = New System.Drawing.Size(114, 22)
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
        '退出ToolStripMenuItem
        '
        Me.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem"
        Me.退出ToolStripMenuItem.Size = New System.Drawing.Size(114, 22)
        Me.退出ToolStripMenuItem.Text = "&X 退出"
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
        Me.H帮助ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menuAbout, Me.menuPDFNetHelp, Me.menuOnlineHelp, Me.版本信息ToolStripMenuItem})
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
        '版本信息ToolStripMenuItem
        '
        Me.版本信息ToolStripMenuItem.Name = "版本信息ToolStripMenuItem"
        Me.版本信息ToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.版本信息ToolStripMenuItem.Text = "版本信息"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'lblOptTile
        '
        Me.lblOptTile.AutoSize = True
        Me.lblOptTile.Location = New System.Drawing.Point(12, 57)
        Me.lblOptTile.Name = "lblOptTile"
        Me.lblOptTile.Size = New System.Drawing.Size(29, 12)
        Me.lblOptTile.TabIndex = 2
        Me.lblOptTile.Text = "就绪"
        '
        'lblFileName
        '
        Me.lblFileName.AutoSize = True
        Me.lblFileName.Location = New System.Drawing.Point(125, 57)
        Me.lblFileName.Name = "lblFileName"
        Me.lblFileName.Size = New System.Drawing.Size(89, 12)
        Me.lblFileName.TabIndex = 3
        Me.lblFileName.Text = "请选择相应菜单"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsSqlMapper, Me.ToolStripSeparator1, Me.tsDALCoder, Me.ToolStripSeparator2, Me.tsEntityCoder})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 25)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(940, 25)
        Me.ToolStrip1.TabIndex = 5
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
        'btnEdit
        '
        Me.btnEdit.BackgroundImage = Global.PDFDotNET.My.Resources.Resources.TextTemplate
        Me.btnEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnEdit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnEdit.Location = New System.Drawing.Point(866, 46)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(62, 36)
        Me.btnEdit.TabIndex = 4
        Me.btnEdit.Text = "   编辑"
        Me.btnEdit.UseVisualStyleBackColor = False
        '
        'txtFileText
        '
        Me.txtFileText.BackColor = System.Drawing.Color.Azure
        Me.txtFileText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFileText.ForeColor = System.Drawing.Color.Blue
        Me.txtFileText.Location = New System.Drawing.Point(12, 88)
        Me.txtFileText.Name = "txtFileText"
        Me.txtFileText.ReadOnly = True
        Me.txtFileText.Size = New System.Drawing.Size(916, 441)
        Me.txtFileText.TabIndex = 6
        Me.txtFileText.Text = "PDF.NET(Ver 4.0)集成管理界面"
        Me.txtFileText.WordWrap = False
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(940, 541)
        Me.Controls.Add(Me.txtFileText)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.btnEdit)
        Me.Controls.Add(Me.lblFileName)
        Me.Controls.Add(Me.lblOptTile)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.Text = "PDF.NET (Ver4.0） 集成管理界面"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents 文件ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 打开ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 退出ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents T工具ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuSqlMapBuilder As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuSqlMapCodeMaker As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuEntityMaker As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 配置ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuIDEConfig As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuCodeMakerConfig As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuEntityMakerConfig As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents H帮助ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuAbout As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuPDFNetHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuOnlineHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuOpenSqlMapConfig As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuOpenMakedCodeFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents menuSysConfig As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblOptTile As System.Windows.Forms.Label
    Friend WithEvents lblFileName As System.Windows.Forms.Label
    Friend WithEvents btnEdit As System.Windows.Forms.Button
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsSqlMapper As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsDALCoder As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsEntityCoder As System.Windows.Forms.ToolStripButton
    Friend WithEvents txtFileText As System.Windows.Forms.RichTextBox
    Friend WithEvents 版本信息ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
