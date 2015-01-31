<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDataBaseExpert
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
        Me.components = New System.ComponentModel.Container
        Dim TreeNode1 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("所有连接")
        Me.TreeView1 = New System.Windows.Forms.TreeView
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tsmItemNewGroup = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmItemEditGroup = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmItemDeleGroup = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.tsmItemNewConn = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmItemCloseConn = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmItemDeleConn = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.tsmiNewQuery = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiNewGroupQuery = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiCreateEntity = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiProperty = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.tsbNewConn = New System.Windows.Forms.ToolStripButton
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.tsmiExpTableData = New System.Windows.Forms.ToolStripMenuItem
        Me.CSV文件ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Excel文件ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiExpTableDataSQL = New System.Windows.Forms.ToolStripMenuItem
        Me.ContextMenuStrip1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TreeView1
        '
        Me.TreeView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TreeView1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.TreeView1.HideSelection = False
        Me.TreeView1.Location = New System.Drawing.Point(0, 28)
        Me.TreeView1.Name = "TreeView1"
        TreeNode1.Name = "root"
        TreeNode1.Text = "所有连接"
        Me.TreeView1.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode1})
        Me.TreeView1.Size = New System.Drawing.Size(292, 237)
        Me.TreeView1.TabIndex = 1
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmItemNewGroup, Me.tsmItemEditGroup, Me.tsmItemDeleGroup, Me.ToolStripSeparator3, Me.tsmItemNewConn, Me.tsmItemCloseConn, Me.tsmItemDeleConn, Me.ToolStripSeparator1, Me.tsmiNewQuery, Me.tsmiNewGroupQuery, Me.tsmiExpTableData, Me.tsmiCreateEntity, Me.ToolStripSeparator2, Me.ToolStripMenuItem1, Me.tsmiProperty})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(153, 308)
        '
        'tsmItemNewGroup
        '
        Me.tsmItemNewGroup.Name = "tsmItemNewGroup"
        Me.tsmItemNewGroup.Size = New System.Drawing.Size(152, 22)
        Me.tsmItemNewGroup.Text = "新键分组"
        '
        'tsmItemEditGroup
        '
        Me.tsmItemEditGroup.Name = "tsmItemEditGroup"
        Me.tsmItemEditGroup.Size = New System.Drawing.Size(152, 22)
        Me.tsmItemEditGroup.Text = "修改分组"
        '
        'tsmItemDeleGroup
        '
        Me.tsmItemDeleGroup.Name = "tsmItemDeleGroup"
        Me.tsmItemDeleGroup.Size = New System.Drawing.Size(152, 22)
        Me.tsmItemDeleGroup.Text = "删除分组"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(149, 6)
        '
        'tsmItemNewConn
        '
        Me.tsmItemNewConn.Name = "tsmItemNewConn"
        Me.tsmItemNewConn.Size = New System.Drawing.Size(152, 22)
        Me.tsmItemNewConn.Text = "新建连接"
        '
        'tsmItemCloseConn
        '
        Me.tsmItemCloseConn.Name = "tsmItemCloseConn"
        Me.tsmItemCloseConn.Size = New System.Drawing.Size(152, 22)
        Me.tsmItemCloseConn.Text = "关闭连接"
        '
        'tsmItemDeleConn
        '
        Me.tsmItemDeleConn.Name = "tsmItemDeleConn"
        Me.tsmItemDeleConn.Size = New System.Drawing.Size(152, 22)
        Me.tsmItemDeleConn.Text = "删除连接"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(149, 6)
        '
        'tsmiNewQuery
        '
        Me.tsmiNewQuery.Name = "tsmiNewQuery"
        Me.tsmiNewQuery.Size = New System.Drawing.Size(152, 22)
        Me.tsmiNewQuery.Text = "新建表查询"
        '
        'tsmiNewGroupQuery
        '
        Me.tsmiNewGroupQuery.Name = "tsmiNewGroupQuery"
        Me.tsmiNewGroupQuery.Size = New System.Drawing.Size(152, 22)
        Me.tsmiNewGroupQuery.Text = "多数据源查询"
        '
        'tsmiCreateEntity
        '
        Me.tsmiCreateEntity.Name = "tsmiCreateEntity"
        Me.tsmiCreateEntity.Size = New System.Drawing.Size(152, 22)
        Me.tsmiCreateEntity.Text = "生成实体类"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(149, 6)
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(152, 22)
        Me.ToolStripMenuItem1.Text = "刷新"
        '
        'tsmiProperty
        '
        Me.tsmiProperty.Name = "tsmiProperty"
        Me.tsmiProperty.Size = New System.Drawing.Size(152, 22)
        Me.tsmiProperty.Text = "属性"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbNewConn})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(292, 25)
        Me.ToolStrip1.TabIndex = 2
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbNewConn
        '
        Me.tsbNewConn.Image = Global.PDFDotNET.My.Resources.Resources.SqlQuery
        Me.tsbNewConn.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbNewConn.Name = "tsbNewConn"
        Me.tsbNewConn.Size = New System.Drawing.Size(76, 22)
        Me.tsbNewConn.Text = "新建连接"
        '
        'ImageList1
        '
        Me.ImageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.ImageList1.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        '
        'tsmiExpTableData
        '
        Me.tsmiExpTableData.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CSV文件ToolStripMenuItem, Me.Excel文件ToolStripMenuItem, Me.tsmiExpTableDataSQL})
        Me.tsmiExpTableData.Name = "tsmiExpTableData"
        Me.tsmiExpTableData.Size = New System.Drawing.Size(152, 22)
        Me.tsmiExpTableData.Text = "导出表数据"
        '
        'CSV文件ToolStripMenuItem
        '
        Me.CSV文件ToolStripMenuItem.Name = "CSV文件ToolStripMenuItem"
        Me.CSV文件ToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.CSV文件ToolStripMenuItem.Text = "CSV文件"
        '
        'Excel文件ToolStripMenuItem
        '
        Me.Excel文件ToolStripMenuItem.Name = "Excel文件ToolStripMenuItem"
        Me.Excel文件ToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.Excel文件ToolStripMenuItem.Text = "Excel文件"
        '
        'tsmiExpTableDataSQL
        '
        Me.tsmiExpTableDataSQL.Name = "tsmiExpTableDataSQL"
        Me.tsmiExpTableDataSQL.Size = New System.Drawing.Size(152, 22)
        Me.tsmiExpTableDataSQL.Text = "SQL数据文件"
        '
        'frmDataBaseExpert
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 265)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.TreeView1)
        Me.Name = "frmDataBaseExpert"
        Me.Text = "数据连结"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TreeView1 As System.Windows.Forms.TreeView
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbNewConn As System.Windows.Forms.ToolStripButton
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents tsmItemCloseConn As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiProperty As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiCreateEntity As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiNewQuery As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsmItemNewConn As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmItemDeleConn As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsmItemNewGroup As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmItemEditGroup As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmItemDeleGroup As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents tsmiNewGroupQuery As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiExpTableData As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CSV文件ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Excel文件ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiExpTableDataSQL As System.Windows.Forms.ToolStripMenuItem
End Class
