<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSqlMapFile
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
        Me.TabControl1 = New MdiTabControl.TabControl
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Alignment = MdiTabControl.TabControl.TabAlignment.Bottom
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.MenuRenderer = Nothing
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.Size = New System.Drawing.Size(489, 313)
        Me.TabControl1.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
        Me.TabControl1.TabBorderEnhanceWeight = MdiTabControl.TabControl.Weight.Medium
        Me.TabControl1.TabCloseButtonImage = Nothing
        Me.TabControl1.TabCloseButtonImageDisabled = Nothing
        Me.TabControl1.TabCloseButtonImageHot = Nothing
        Me.TabControl1.TabIndex = 0
        Me.TabControl1.TabsDirection = MdiTabControl.TabControl.FlowDirection.LeftToRight
        '
        'frmSqlMapFile
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(489, 313)
        Me.Controls.Add(Me.TabControl1)
        Me.Name = "frmSqlMapFile"
        Me.Text = "frmSqlMapFile"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl1 As MdiTabControl.TabControl
End Class
