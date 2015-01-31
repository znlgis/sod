<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEntityCreate
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.btnSQLtoEntity = New System.Windows.Forms.Button
        Me.dgMapInfo = New System.Windows.Forms.DataGridView
        Me.colSelect = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.colName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colType = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colEntityName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colFieldMap = New System.Windows.Forms.DataGridViewButtonColumn
        Me.colEntityFile = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colFirstLook = New System.Windows.Forms.DataGridViewLinkColumn
        Me.rbtnSelectAllTable = New System.Windows.Forms.RadioButton
        Me.rbtnSelectOneTable = New System.Windows.Forms.RadioButton
        Me.PropertyGrid1 = New System.Windows.Forms.PropertyGrid
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.txtMakeLog = New System.Windows.Forms.TextBox
        Me.btnSqlMapEntity = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.PrgBarMakeFile = New System.Windows.Forms.ProgressBar
        Me.btnMakeFile = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        CType(Me.dgMapInfo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnSQLtoEntity)
        Me.GroupBox1.Controls.Add(Me.dgMapInfo)
        Me.GroupBox1.Controls.Add(Me.rbtnSelectAllTable)
        Me.GroupBox1.Controls.Add(Me.rbtnSelectOneTable)
        Me.GroupBox1.Location = New System.Drawing.Point(28, 25)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(855, 389)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "选择生成实体类的方式"
        '
        'btnSQLtoEntity
        '
        Me.btnSQLtoEntity.Location = New System.Drawing.Point(240, 21)
        Me.btnSQLtoEntity.Name = "btnSQLtoEntity"
        Me.btnSQLtoEntity.Size = New System.Drawing.Size(124, 23)
        Me.btnSQLtoEntity.TabIndex = 3
        Me.btnSQLtoEntity.Text = "使用查询限定"
        Me.btnSQLtoEntity.UseVisualStyleBackColor = True
        '
        'dgMapInfo
        '
        Me.dgMapInfo.AllowUserToAddRows = False
        Me.dgMapInfo.AllowUserToOrderColumns = True
        Me.dgMapInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgMapInfo.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colSelect, Me.colName, Me.colType, Me.colEntityName, Me.colFieldMap, Me.colEntityFile, Me.colFirstLook})
        Me.dgMapInfo.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.dgMapInfo.Location = New System.Drawing.Point(25, 87)
        Me.dgMapInfo.Name = "dgMapInfo"
        Me.dgMapInfo.RowTemplate.Height = 23
        Me.dgMapInfo.Size = New System.Drawing.Size(781, 289)
        Me.dgMapInfo.TabIndex = 2
        Me.dgMapInfo.VirtualMode = True
        '
        'colSelect
        '
        Me.colSelect.DataPropertyName = "Selected"
        Me.colSelect.FalseValue = "False"
        Me.colSelect.HeaderText = "选择"
        Me.colSelect.Name = "colSelect"
        Me.colSelect.TrueValue = "True"
        Me.colSelect.Width = 50
        '
        'colName
        '
        Me.colName.DataPropertyName = "TableName"
        Me.colName.HeaderText = "名称"
        Me.colName.Name = "colName"
        Me.colName.Width = 150
        '
        'colType
        '
        Me.colType.DataPropertyName = "TableType"
        Me.colType.HeaderText = "类型"
        Me.colType.Name = "colType"
        Me.colType.Width = 60
        '
        'colEntityName
        '
        Me.colEntityName.DataPropertyName = "MapEntityName"
        Me.colEntityName.HeaderText = "类映射"
        Me.colEntityName.Name = "colEntityName"
        Me.colEntityName.Width = 150
        '
        'colFieldMap
        '
        Me.colFieldMap.DataPropertyName = "MapFieldName"
        Me.colFieldMap.HeaderText = "字段映射"
        Me.colFieldMap.Name = "colFieldMap"
        Me.colFieldMap.Text = "编辑"
        Me.colFieldMap.UseColumnTextForButtonValue = True
        Me.colFieldMap.Width = 60
        '
        'colEntityFile
        '
        Me.colEntityFile.DataPropertyName = "OutputFile"
        Me.colEntityFile.HeaderText = "输出文件"
        Me.colEntityFile.Name = "colEntityFile"
        Me.colEntityFile.Width = 150
        '
        'colFirstLook
        '
        Me.colFirstLook.HeaderText = "预览结果"
        Me.colFirstLook.Name = "colFirstLook"
        Me.colFirstLook.Text = "预览"
        Me.colFirstLook.UseColumnTextForLinkValue = True
        '
        'rbtnSelectAllTable
        '
        Me.rbtnSelectAllTable.AutoSize = True
        Me.rbtnSelectAllTable.Location = New System.Drawing.Point(25, 52)
        Me.rbtnSelectAllTable.Name = "rbtnSelectAllTable"
        Me.rbtnSelectAllTable.Size = New System.Drawing.Size(215, 16)
        Me.rbtnSelectAllTable.TabIndex = 1
        Me.rbtnSelectAllTable.TabStop = True
        Me.rbtnSelectAllTable.Text = "为当前""选择""的所有表生成实体类"
        Me.rbtnSelectAllTable.UseVisualStyleBackColor = True
        '
        'rbtnSelectOneTable
        '
        Me.rbtnSelectOneTable.AutoSize = True
        Me.rbtnSelectOneTable.Location = New System.Drawing.Point(25, 20)
        Me.rbtnSelectOneTable.Name = "rbtnSelectOneTable"
        Me.rbtnSelectOneTable.Size = New System.Drawing.Size(203, 16)
        Me.rbtnSelectOneTable.TabIndex = 0
        Me.rbtnSelectOneTable.TabStop = True
        Me.rbtnSelectOneTable.Text = "为当前选择行的一个表生成实体类"
        Me.rbtnSelectOneTable.UseVisualStyleBackColor = True
        '
        'PropertyGrid1
        '
        Me.PropertyGrid1.Location = New System.Drawing.Point(470, 20)
        Me.PropertyGrid1.Name = "PropertyGrid1"
        Me.PropertyGrid1.Size = New System.Drawing.Size(249, 212)
        Me.PropertyGrid1.TabIndex = 2
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtMakeLog)
        Me.GroupBox2.Controls.Add(Me.PropertyGrid1)
        Me.GroupBox2.Controls.Add(Me.btnSqlMapEntity)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.PrgBarMakeFile)
        Me.GroupBox2.Controls.Add(Me.btnMakeFile)
        Me.GroupBox2.Location = New System.Drawing.Point(28, 420)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(855, 238)
        Me.GroupBox2.TabIndex = 3
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "生成实体类"
        '
        'txtMakeLog
        '
        Me.txtMakeLog.Location = New System.Drawing.Point(25, 90)
        Me.txtMakeLog.Multiline = True
        Me.txtMakeLog.Name = "txtMakeLog"
        Me.txtMakeLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtMakeLog.Size = New System.Drawing.Size(418, 124)
        Me.txtMakeLog.TabIndex = 5
        '
        'btnSqlMapEntity
        '
        Me.btnSqlMapEntity.Font = New System.Drawing.Font("宋体", 12.0!)
        Me.btnSqlMapEntity.Location = New System.Drawing.Point(341, 20)
        Me.btnSqlMapEntity.Name = "btnSqlMapEntity"
        Me.btnSqlMapEntity.Size = New System.Drawing.Size(102, 24)
        Me.btnSqlMapEntity.TabIndex = 4
        Me.btnSqlMapEntity.Text = "高级"
        Me.btnSqlMapEntity.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(23, 72)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 12)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "生成进度"
        '
        'PrgBarMakeFile
        '
        Me.PrgBarMakeFile.Location = New System.Drawing.Point(99, 61)
        Me.PrgBarMakeFile.Name = "PrgBarMakeFile"
        Me.PrgBarMakeFile.Size = New System.Drawing.Size(344, 23)
        Me.PrgBarMakeFile.TabIndex = 2
        '
        'btnMakeFile
        '
        Me.btnMakeFile.Font = New System.Drawing.Font("宋体", 12.0!)
        Me.btnMakeFile.Location = New System.Drawing.Point(99, 20)
        Me.btnMakeFile.Name = "btnMakeFile"
        Me.btnMakeFile.Size = New System.Drawing.Size(102, 24)
        Me.btnMakeFile.TabIndex = 1
        Me.btnMakeFile.Text = "&M 生成"
        Me.btnMakeFile.UseVisualStyleBackColor = True
        '
        'frmEntityCreate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(916, 668)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "frmEntityCreate"
        Me.Text = "实体类生成器"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.dgMapInfo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rbtnSelectOneTable As System.Windows.Forms.RadioButton
    Friend WithEvents dgMapInfo As System.Windows.Forms.DataGridView
    Friend WithEvents rbtnSelectAllTable As System.Windows.Forms.RadioButton
    Friend WithEvents btnSQLtoEntity As System.Windows.Forms.Button
    Friend WithEvents PropertyGrid1 As System.Windows.Forms.PropertyGrid
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PrgBarMakeFile As System.Windows.Forms.ProgressBar
    Friend WithEvents btnMakeFile As System.Windows.Forms.Button
    Friend WithEvents btnSqlMapEntity As System.Windows.Forms.Button
    Friend WithEvents txtMakeLog As System.Windows.Forms.TextBox
    Friend WithEvents colSelect As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents colName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colType As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colEntityName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colFieldMap As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents colEntityFile As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colFirstLook As System.Windows.Forms.DataGridViewLinkColumn
End Class
