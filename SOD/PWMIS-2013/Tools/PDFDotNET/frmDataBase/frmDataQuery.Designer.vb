<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDataQuery
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
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.rtbQueryText = New System.Windows.Forms.RichTextBox
        Me.cmsExecuteQuery = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tsmItemExecute = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmItemQueryDataSet = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmItemUpdateTable = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.tsmItemCreateEntity = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmItemSaveSQLScript = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.tsmItemProperty = New System.Windows.Forms.ToolStripMenuItem
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPageGrid = New System.Windows.Forms.TabPage
        Me.TabPageMsg = New System.Windows.Forms.TabPage
        Me.txtExecuteMsg = New System.Windows.Forms.TextBox
        Me.TabPageGroupQuery = New System.Windows.Forms.TabPage
        Me.btnDelConn = New System.Windows.Forms.Button
        Me.btnLoadGQuery = New System.Windows.Forms.Button
        Me.btnSaveGQuery = New System.Windows.Forms.Button
        Me.btnAddConn = New System.Windows.Forms.Button
        Me.chkGroupQuery = New System.Windows.Forms.CheckBox
        Me.dgvGroupQuery = New System.Windows.Forms.DataGridView
        Me.colSelected = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.colDbmsType = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.colConnStr = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.lblGroupQueryMsg = New System.Windows.Forms.Label
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.cmsExecuteQuery.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPageMsg.SuspendLayout()
        Me.TabPageGroupQuery.SuspendLayout()
        CType(Me.dgvGroupQuery, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.rtbQueryText)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.TabControl1)
        Me.SplitContainer1.Size = New System.Drawing.Size(750, 604)
        Me.SplitContainer1.SplitterDistance = 267
        Me.SplitContainer1.TabIndex = 0
        '
        'rtbQueryText
        '
        Me.rtbQueryText.ContextMenuStrip = Me.cmsExecuteQuery
        Me.rtbQueryText.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtbQueryText.Font = New System.Drawing.Font("新宋体", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.rtbQueryText.Location = New System.Drawing.Point(0, 0)
        Me.rtbQueryText.Name = "rtbQueryText"
        Me.rtbQueryText.Size = New System.Drawing.Size(750, 267)
        Me.rtbQueryText.TabIndex = 0
        Me.rtbQueryText.Text = ""
        '
        'cmsExecuteQuery
        '
        Me.cmsExecuteQuery.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmItemExecute, Me.tsmItemQueryDataSet, Me.tsmItemUpdateTable, Me.ToolStripSeparator1, Me.tsmItemCreateEntity, Me.tsmItemSaveSQLScript, Me.ToolStripSeparator2, Me.tsmItemProperty})
        Me.cmsExecuteQuery.Name = "cmsExecuteQuery"
        Me.cmsExecuteQuery.Size = New System.Drawing.Size(192, 148)
        Me.cmsExecuteQuery.Text = "执行查询"
        '
        'tsmItemExecute
        '
        Me.tsmItemExecute.Name = "tsmItemExecute"
        Me.tsmItemExecute.ShortcutKeyDisplayString = ""
        Me.tsmItemExecute.ShortcutKeys = System.Windows.Forms.Keys.F5
        Me.tsmItemExecute.Size = New System.Drawing.Size(191, 22)
        Me.tsmItemExecute.Text = "执行"
        Me.tsmItemExecute.ToolTipText = "执行当前查询"
        '
        'tsmItemQueryDataSet
        '
        Me.tsmItemQueryDataSet.Name = "tsmItemQueryDataSet"
        Me.tsmItemQueryDataSet.Size = New System.Drawing.Size(191, 22)
        Me.tsmItemQueryDataSet.Text = "获取结果集"
        '
        'tsmItemUpdateTable
        '
        Me.tsmItemUpdateTable.Name = "tsmItemUpdateTable"
        Me.tsmItemUpdateTable.Size = New System.Drawing.Size(191, 22)
        Me.tsmItemUpdateTable.Text = "更新表数据"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(188, 6)
        '
        'tsmItemCreateEntity
        '
        Me.tsmItemCreateEntity.Name = "tsmItemCreateEntity"
        Me.tsmItemCreateEntity.Size = New System.Drawing.Size(191, 22)
        Me.tsmItemCreateEntity.Text = "生成实体类"
        '
        'tsmItemSaveSQLScript
        '
        Me.tsmItemSaveSQLScript.Name = "tsmItemSaveSQLScript"
        Me.tsmItemSaveSQLScript.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.tsmItemSaveSQLScript.Size = New System.Drawing.Size(191, 22)
        Me.tsmItemSaveSQLScript.Text = "保存SQL查询"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(188, 6)
        '
        'tsmItemProperty
        '
        Me.tsmItemProperty.Name = "tsmItemProperty"
        Me.tsmItemProperty.Size = New System.Drawing.Size(191, 22)
        Me.tsmItemProperty.Text = "查看属性"
        '
        'TabControl1
        '
        Me.TabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom
        Me.TabControl1.Controls.Add(Me.TabPageGrid)
        Me.TabControl1.Controls.Add(Me.TabPageMsg)
        Me.TabControl1.Controls.Add(Me.TabPageGroupQuery)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Multiline = True
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(750, 333)
        Me.TabControl1.TabIndex = 0
        '
        'TabPageGrid
        '
        Me.TabPageGrid.Location = New System.Drawing.Point(4, 4)
        Me.TabPageGrid.Name = "TabPageGrid"
        Me.TabPageGrid.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageGrid.Size = New System.Drawing.Size(742, 307)
        Me.TabPageGrid.TabIndex = 0
        Me.TabPageGrid.Text = "网格"
        Me.TabPageGrid.UseVisualStyleBackColor = True
        '
        'TabPageMsg
        '
        Me.TabPageMsg.Controls.Add(Me.txtExecuteMsg)
        Me.TabPageMsg.Location = New System.Drawing.Point(4, 4)
        Me.TabPageMsg.Name = "TabPageMsg"
        Me.TabPageMsg.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageMsg.Size = New System.Drawing.Size(742, 307)
        Me.TabPageMsg.TabIndex = 1
        Me.TabPageMsg.Text = "消息"
        Me.TabPageMsg.UseVisualStyleBackColor = True
        '
        'txtExecuteMsg
        '
        Me.txtExecuteMsg.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtExecuteMsg.Location = New System.Drawing.Point(3, 3)
        Me.txtExecuteMsg.Multiline = True
        Me.txtExecuteMsg.Name = "txtExecuteMsg"
        Me.txtExecuteMsg.ReadOnly = True
        Me.txtExecuteMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtExecuteMsg.Size = New System.Drawing.Size(736, 301)
        Me.txtExecuteMsg.TabIndex = 0
        '
        'TabPageGroupQuery
        '
        Me.TabPageGroupQuery.Controls.Add(Me.btnDelConn)
        Me.TabPageGroupQuery.Controls.Add(Me.btnLoadGQuery)
        Me.TabPageGroupQuery.Controls.Add(Me.btnSaveGQuery)
        Me.TabPageGroupQuery.Controls.Add(Me.btnAddConn)
        Me.TabPageGroupQuery.Controls.Add(Me.chkGroupQuery)
        Me.TabPageGroupQuery.Controls.Add(Me.dgvGroupQuery)
        Me.TabPageGroupQuery.Controls.Add(Me.lblGroupQueryMsg)
        Me.TabPageGroupQuery.Location = New System.Drawing.Point(4, 4)
        Me.TabPageGroupQuery.Name = "TabPageGroupQuery"
        Me.TabPageGroupQuery.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageGroupQuery.Size = New System.Drawing.Size(742, 307)
        Me.TabPageGroupQuery.TabIndex = 2
        Me.TabPageGroupQuery.Text = "组查询"
        Me.TabPageGroupQuery.UseVisualStyleBackColor = True
        '
        'btnDelConn
        '
        Me.btnDelConn.Location = New System.Drawing.Point(244, 275)
        Me.btnDelConn.Name = "btnDelConn"
        Me.btnDelConn.Size = New System.Drawing.Size(75, 23)
        Me.btnDelConn.TabIndex = 9
        Me.btnDelConn.Text = "删除连接"
        Me.btnDelConn.UseVisualStyleBackColor = True
        '
        'btnLoadGQuery
        '
        Me.btnLoadGQuery.Location = New System.Drawing.Point(612, 275)
        Me.btnLoadGQuery.Name = "btnLoadGQuery"
        Me.btnLoadGQuery.Size = New System.Drawing.Size(92, 23)
        Me.btnLoadGQuery.TabIndex = 8
        Me.btnLoadGQuery.Text = "加载连接文件"
        Me.btnLoadGQuery.UseVisualStyleBackColor = True
        '
        'btnSaveGQuery
        '
        Me.btnSaveGQuery.Location = New System.Drawing.Point(497, 275)
        Me.btnSaveGQuery.Name = "btnSaveGQuery"
        Me.btnSaveGQuery.Size = New System.Drawing.Size(99, 23)
        Me.btnSaveGQuery.TabIndex = 7
        Me.btnSaveGQuery.Text = "保存为连接文件"
        Me.btnSaveGQuery.UseVisualStyleBackColor = True
        '
        'btnAddConn
        '
        Me.btnAddConn.Location = New System.Drawing.Point(149, 275)
        Me.btnAddConn.Name = "btnAddConn"
        Me.btnAddConn.Size = New System.Drawing.Size(75, 23)
        Me.btnAddConn.TabIndex = 6
        Me.btnAddConn.Text = "添加连接"
        Me.btnAddConn.UseVisualStyleBackColor = True
        '
        'chkGroupQuery
        '
        Me.chkGroupQuery.AutoSize = True
        Me.chkGroupQuery.Location = New System.Drawing.Point(41, 279)
        Me.chkGroupQuery.Name = "chkGroupQuery"
        Me.chkGroupQuery.Size = New System.Drawing.Size(84, 16)
        Me.chkGroupQuery.TabIndex = 5
        Me.chkGroupQuery.Text = "启用组查询"
        Me.chkGroupQuery.UseVisualStyleBackColor = True
        '
        'dgvGroupQuery
        '
        Me.dgvGroupQuery.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvGroupQuery.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colSelected, Me.colDbmsType, Me.colConnStr})
        Me.dgvGroupQuery.Location = New System.Drawing.Point(41, 44)
        Me.dgvGroupQuery.Name = "dgvGroupQuery"
        Me.dgvGroupQuery.RowTemplate.Height = 23
        Me.dgvGroupQuery.Size = New System.Drawing.Size(663, 229)
        Me.dgvGroupQuery.TabIndex = 4
        Me.dgvGroupQuery.VirtualMode = True
        '
        'colSelected
        '
        Me.colSelected.DataPropertyName = "Enabled"
        Me.colSelected.FalseValue = "false"
        Me.colSelected.HeaderText = "选择"
        Me.colSelected.Name = "colSelected"
        Me.colSelected.TrueValue = "true"
        Me.colSelected.Width = 50
        '
        'colDbmsType
        '
        Me.colDbmsType.DataPropertyName = "DbType"
        Me.colDbmsType.HeaderText = "数据库类型"
        Me.colDbmsType.Name = "colDbmsType"
        '
        'colConnStr
        '
        Me.colConnStr.DataPropertyName = "ConnectionStrng"
        Me.colConnStr.HeaderText = "连接字符串"
        Me.colConnStr.Name = "colConnStr"
        Me.colConnStr.Width = 500
        '
        'lblGroupQueryMsg
        '
        Me.lblGroupQueryMsg.AutoSize = True
        Me.lblGroupQueryMsg.Location = New System.Drawing.Point(39, 11)
        Me.lblGroupQueryMsg.Name = "lblGroupQueryMsg"
        Me.lblGroupQueryMsg.Size = New System.Drawing.Size(653, 12)
        Me.lblGroupQueryMsg.TabIndex = 3
        Me.lblGroupQueryMsg.Text = "如果启用组查询，那么下表中所有选中的数据库连接都将执行同一个查询语句，这要求对应的数据源都有相同的数据架构。"
        '
        'frmDataQuery
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(750, 604)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "frmDataQuery"
        Me.Text = "查询窗口"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.cmsExecuteQuery.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPageMsg.ResumeLayout(False)
        Me.TabPageMsg.PerformLayout()
        Me.TabPageGroupQuery.ResumeLayout(False)
        Me.TabPageGroupQuery.PerformLayout()
        CType(Me.dgvGroupQuery, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents rtbQueryText As System.Windows.Forms.RichTextBox
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPageGrid As System.Windows.Forms.TabPage
    Friend WithEvents TabPageMsg As System.Windows.Forms.TabPage
    Friend WithEvents txtExecuteMsg As System.Windows.Forms.TextBox
    Friend WithEvents cmsExecuteQuery As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents tsmItemQueryDataSet As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmItemUpdateTable As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsmItemCreateEntity As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmItemSaveSQLScript As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents tsmItemExecute As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TabPageGroupQuery As System.Windows.Forms.TabPage
    Friend WithEvents chkGroupQuery As System.Windows.Forms.CheckBox
    Friend WithEvents dgvGroupQuery As System.Windows.Forms.DataGridView
    Friend WithEvents lblGroupQueryMsg As System.Windows.Forms.Label
    Friend WithEvents colSelected As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents colDbmsType As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents colConnStr As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents btnAddConn As System.Windows.Forms.Button
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsmItemProperty As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnSaveGQuery As System.Windows.Forms.Button
    Friend WithEvents btnLoadGQuery As System.Windows.Forms.Button
    Friend WithEvents btnDelConn As System.Windows.Forms.Button
End Class
