<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPropertys
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
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.propCatagory = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.propName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.propValue = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.lblPropDesc = New System.Windows.Forms.Label
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.propCatagory, Me.propName, Me.propValue})
        Me.DataGridView1.Location = New System.Drawing.Point(22, 46)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowTemplate.Height = 23
        Me.DataGridView1.Size = New System.Drawing.Size(547, 343)
        Me.DataGridView1.TabIndex = 0
        '
        'propCatagory
        '
        Me.propCatagory.HeaderText = "分类"
        Me.propCatagory.Name = "propCatagory"
        Me.propCatagory.ReadOnly = True
        '
        'propName
        '
        Me.propName.HeaderText = "属性名"
        Me.propName.Name = "propName"
        Me.propName.ReadOnly = True
        '
        'propValue
        '
        Me.propValue.HeaderText = "属性值"
        Me.propValue.Name = "propValue"
        '
        'lblPropDesc
        '
        Me.lblPropDesc.AutoSize = True
        Me.lblPropDesc.Location = New System.Drawing.Point(39, 407)
        Me.lblPropDesc.Name = "lblPropDesc"
        Me.lblPropDesc.Size = New System.Drawing.Size(65, 12)
        Me.lblPropDesc.TabIndex = 1
        Me.lblPropDesc.Text = "属性说明："
        '
        'frmPropertys
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(668, 466)
        Me.Controls.Add(Me.lblPropDesc)
        Me.Controls.Add(Me.DataGridView1)
        Me.Name = "frmPropertys"
        Me.Text = "属性窗口"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents propCatagory As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents propName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents propValue As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lblPropDesc As System.Windows.Forms.Label
End Class
