<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEntityFromSQL
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtTableName = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtClassName = New System.Windows.Forms.TextBox
        Me.btnExamSQL = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtNamespace = New System.Windows.Forms.TextBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.CheckedSQL = New System.Windows.Forms.CheckBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtMapType = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.PanelUp = New System.Windows.Forms.Panel
        Me.PanelDown = New System.Windows.Forms.Panel
        Me.Label6 = New System.Windows.Forms.Label
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.txtSQL = New System.Windows.Forms.TextBox
        Me.GroupBox1.SuspendLayout()
        Me.PanelUp.SuspendLayout()
        Me.PanelDown.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(167, 12)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "要生成实体类的SQL查询语句："
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(20, 82)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(101, 12)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "将此查询命名为："
        '
        'txtTableName
        '
        Me.txtTableName.Location = New System.Drawing.Point(22, 97)
        Me.txtTableName.Name = "txtTableName"
        Me.txtTableName.Size = New System.Drawing.Size(163, 21)
        Me.txtTableName.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(20, 141)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(113, 12)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "映射的实体类名称："
        '
        'txtClassName
        '
        Me.txtClassName.Location = New System.Drawing.Point(22, 156)
        Me.txtClassName.Name = "txtClassName"
        Me.txtClassName.Size = New System.Drawing.Size(163, 21)
        Me.txtClassName.TabIndex = 5
        '
        'btnExamSQL
        '
        Me.btnExamSQL.Location = New System.Drawing.Point(373, 12)
        Me.btnExamSQL.Name = "btnExamSQL"
        Me.btnExamSQL.Size = New System.Drawing.Size(75, 23)
        Me.btnExamSQL.TabIndex = 6
        Me.btnExamSQL.Text = "验证SQL"
        Me.btnExamSQL.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(20, 207)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(65, 12)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "名称空间："
        '
        'txtNamespace
        '
        Me.txtNamespace.Location = New System.Drawing.Point(22, 222)
        Me.txtNamespace.Name = "txtNamespace"
        Me.txtNamespace.Size = New System.Drawing.Size(163, 21)
        Me.txtNamespace.TabIndex = 8
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(280, 40)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 12
        Me.btnOK.Text = "&Y 确定"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(373, 40)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 13
        Me.btnCancel.Text = "取消"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.CheckedSQL)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.txtMapType)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.txtTableName)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.txtClassName)
        Me.GroupBox1.Controls.Add(Me.txtNamespace)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Location = New System.Drawing.Point(454, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(194, 327)
        Me.GroupBox1.TabIndex = 14
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "操作属性"
        '
        'CheckedSQL
        '
        Me.CheckedSQL.AutoSize = True
        Me.CheckedSQL.Location = New System.Drawing.Point(22, 46)
        Me.CheckedSQL.Name = "CheckedSQL"
        Me.CheckedSQL.Size = New System.Drawing.Size(60, 16)
        Me.CheckedSQL.TabIndex = 12
        Me.CheckedSQL.Text = "已验证"
        Me.CheckedSQL.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(20, 31)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(107, 12)
        Me.Label7.TabIndex = 11
        Me.Label7.Text = "SQL查询的有效性："
        '
        'txtMapType
        '
        Me.txtMapType.Location = New System.Drawing.Point(22, 284)
        Me.txtMapType.Name = "txtMapType"
        Me.txtMapType.ReadOnly = True
        Me.txtMapType.Size = New System.Drawing.Size(163, 21)
        Me.txtMapType.TabIndex = 10
        Me.txtMapType.Text = "EntityMapType.SqlMap"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(20, 269)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(77, 12)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "实体类类型："
        '
        'PanelUp
        '
        Me.PanelUp.Controls.Add(Me.Label1)
        Me.PanelUp.Controls.Add(Me.btnExamSQL)
        Me.PanelUp.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelUp.Location = New System.Drawing.Point(0, 0)
        Me.PanelUp.Name = "PanelUp"
        Me.PanelUp.Size = New System.Drawing.Size(651, 42)
        Me.PanelUp.TabIndex = 15
        '
        'PanelDown
        '
        Me.PanelDown.Controls.Add(Me.Label6)
        Me.PanelDown.Controls.Add(Me.btnOK)
        Me.PanelDown.Controls.Add(Me.btnCancel)
        Me.PanelDown.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelDown.Location = New System.Drawing.Point(0, 378)
        Me.PanelDown.Name = "PanelDown"
        Me.PanelDown.Size = New System.Drawing.Size(651, 75)
        Me.PanelDown.TabIndex = 16
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(13, 7)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(503, 12)
        Me.Label6.TabIndex = 14
        Me.Label6.Text = "说明： 对于实体类类型是 Table 或者 View 的，最终要映射哪些列可以根据上面的SQL决定。"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.txtSQL, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.GroupBox1, 1, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 42)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(651, 336)
        Me.TableLayoutPanel1.TabIndex = 19
        '
        'txtSQL
        '
        Me.txtSQL.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtSQL.Location = New System.Drawing.Point(3, 3)
        Me.txtSQL.Multiline = True
        Me.txtSQL.Name = "txtSQL"
        Me.txtSQL.Size = New System.Drawing.Size(445, 330)
        Me.txtSQL.TabIndex = 1
        '
        'frmEntityFromSQL
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(651, 453)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.PanelDown)
        Me.Controls.Add(Me.PanelUp)
        Me.Name = "frmEntityFromSQL"
        Me.Text = "[SQL--实体类] 映射"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.PanelUp.ResumeLayout(False)
        Me.PanelUp.PerformLayout()
        Me.PanelDown.ResumeLayout(False)
        Me.PanelDown.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtTableName As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtClassName As System.Windows.Forms.TextBox
    Friend WithEvents btnExamSQL As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtNamespace As System.Windows.Forms.TextBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtMapType As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents CheckedSQL As System.Windows.Forms.CheckBox
    Friend WithEvents PanelUp As System.Windows.Forms.Panel
    Friend WithEvents PanelDown As System.Windows.Forms.Panel
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents txtSQL As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
End Class
