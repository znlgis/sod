<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmExportData
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
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnExpData = New System.Windows.Forms.Button
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar
        Me.lblProcessMsg = New System.Windows.Forms.Label
        Me.cmbExpType = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnBrowFile = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtFileName = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.ckListTables = New System.Windows.Forms.CheckedListBox
        Me.ckAll = New System.Windows.Forms.CheckBox
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnCancel)
        Me.GroupBox1.Controls.Add(Me.btnExpData)
        Me.GroupBox1.Controls.Add(Me.ProgressBar1)
        Me.GroupBox1.Controls.Add(Me.lblProcessMsg)
        Me.GroupBox1.Controls.Add(Me.cmbExpType)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.btnBrowFile)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.txtFileName)
        Me.GroupBox1.Location = New System.Drawing.Point(266, 43)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(394, 342)
        Me.GroupBox1.TabIndex = 9
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "导出选项"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(216, 297)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 17
        Me.btnCancel.Text = "取消"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnExpData
        '
        Me.btnExpData.Location = New System.Drawing.Point(107, 297)
        Me.btnExpData.Name = "btnExpData"
        Me.btnExpData.Size = New System.Drawing.Size(75, 23)
        Me.btnExpData.TabIndex = 16
        Me.btnExpData.Text = "导出"
        Me.btnExpData.UseVisualStyleBackColor = True
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(30, 236)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(330, 23)
        Me.ProgressBar1.TabIndex = 15
        '
        'lblProcessMsg
        '
        Me.lblProcessMsg.AutoSize = True
        Me.lblProcessMsg.Location = New System.Drawing.Point(162, 210)
        Me.lblProcessMsg.Name = "lblProcessMsg"
        Me.lblProcessMsg.Size = New System.Drawing.Size(53, 12)
        Me.lblProcessMsg.TabIndex = 14
        Me.lblProcessMsg.Text = "导出进度"
        '
        'cmbExpType
        '
        Me.cmbExpType.FormattingEnabled = True
        Me.cmbExpType.Items.AddRange(New Object() {"CSV", "Excel", "SQL数据文件"})
        Me.cmbExpType.Location = New System.Drawing.Point(148, 45)
        Me.cmbExpType.Name = "cmbExpType"
        Me.cmbExpType.Size = New System.Drawing.Size(121, 20)
        Me.cmbExpType.TabIndex = 13
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(28, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(101, 12)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "导出的文件类型："
        '
        'btnBrowFile
        '
        Me.btnBrowFile.Location = New System.Drawing.Point(285, 109)
        Me.btnBrowFile.Name = "btnBrowFile"
        Me.btnBrowFile.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowFile.TabIndex = 11
        Me.btnBrowFile.Text = "浏览"
        Me.btnBrowFile.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(28, 93)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(209, 12)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "导出的数据要保存的文件名称或路径："
        '
        'txtFileName
        '
        Me.txtFileName.Location = New System.Drawing.Point(30, 111)
        Me.txtFileName.Name = "txtFileName"
        Me.txtFileName.Size = New System.Drawing.Size(239, 21)
        Me.txtFileName.TabIndex = 9
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(23, 43)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(101, 12)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "选择要操作的表："
        '
        'ckListTables
        '
        Me.ckListTables.FormattingEnabled = True
        Me.ckListTables.Location = New System.Drawing.Point(25, 61)
        Me.ckListTables.Name = "ckListTables"
        Me.ckListTables.Size = New System.Drawing.Size(217, 324)
        Me.ckListTables.TabIndex = 11
        '
        'ckAll
        '
        Me.ckAll.AutoSize = True
        Me.ckAll.Location = New System.Drawing.Point(164, 39)
        Me.ckAll.Name = "ckAll"
        Me.ckAll.Size = New System.Drawing.Size(72, 16)
        Me.ckAll.TabIndex = 12
        Me.ckAll.Text = "全部选择"
        Me.ckAll.UseVisualStyleBackColor = True
        '
        'frmExportData
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(697, 403)
        Me.Controls.Add(Me.ckAll)
        Me.Controls.Add(Me.ckListTables)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmExportData"
        Me.Text = "导出数据"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnExpData As System.Windows.Forms.Button
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents lblProcessMsg As System.Windows.Forms.Label
    Friend WithEvents cmbExpType As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnBrowFile As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtFileName As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ckListTables As System.Windows.Forms.CheckedListBox
    Friend WithEvents ckAll As System.Windows.Forms.CheckBox
End Class
