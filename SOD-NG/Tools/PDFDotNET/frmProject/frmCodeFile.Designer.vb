<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCodeFile
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
        Me.txtFileText = New System.Windows.Forms.RichTextBox
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.编辑ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.启用外部程序编辑ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.保存ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.关闭ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.menu_Cut = New System.Windows.Forms.ToolStripMenuItem
        Me.menu_Copy = New System.Windows.Forms.ToolStripMenuItem
        Me.menu_Paste = New System.Windows.Forms.ToolStripMenuItem
        Me.menu_Undo = New System.Windows.Forms.ToolStripMenuItem
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtFileText
        '
        Me.txtFileText.BackColor = System.Drawing.Color.Azure
        Me.txtFileText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFileText.ContextMenuStrip = Me.ContextMenuStrip1
        Me.txtFileText.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtFileText.ForeColor = System.Drawing.Color.Blue
        Me.txtFileText.Location = New System.Drawing.Point(0, 0)
        Me.txtFileText.Name = "txtFileText"
        Me.txtFileText.ReadOnly = True
        Me.txtFileText.Size = New System.Drawing.Size(792, 565)
        Me.txtFileText.TabIndex = 7
        Me.txtFileText.Text = "PDF.NET(Ver 3.2)集成管理界面"
        Me.txtFileText.WordWrap = False
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.编辑ToolStripMenuItem, Me.启用外部程序编辑ToolStripMenuItem, Me.保存ToolStripMenuItem, Me.关闭ToolStripMenuItem, Me.ToolStripSeparator1, Me.menu_Cut, Me.menu_Copy, Me.menu_Paste, Me.menu_Undo})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(171, 186)
        '
        '编辑ToolStripMenuItem
        '
        Me.编辑ToolStripMenuItem.Name = "编辑ToolStripMenuItem"
        Me.编辑ToolStripMenuItem.Size = New System.Drawing.Size(170, 22)
        Me.编辑ToolStripMenuItem.Text = "编辑"
        '
        '启用外部程序编辑ToolStripMenuItem
        '
        Me.启用外部程序编辑ToolStripMenuItem.Name = "启用外部程序编辑ToolStripMenuItem"
        Me.启用外部程序编辑ToolStripMenuItem.Size = New System.Drawing.Size(170, 22)
        Me.启用外部程序编辑ToolStripMenuItem.Text = "启用外部程序编辑"
        '
        '保存ToolStripMenuItem
        '
        Me.保存ToolStripMenuItem.Name = "保存ToolStripMenuItem"
        Me.保存ToolStripMenuItem.Size = New System.Drawing.Size(170, 22)
        Me.保存ToolStripMenuItem.Text = "保存"
        '
        '关闭ToolStripMenuItem
        '
        Me.关闭ToolStripMenuItem.Name = "关闭ToolStripMenuItem"
        Me.关闭ToolStripMenuItem.Size = New System.Drawing.Size(170, 22)
        Me.关闭ToolStripMenuItem.Text = "关闭"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(167, 6)
        '
        'menu_Cut
        '
        Me.menu_Cut.Name = "menu_Cut"
        Me.menu_Cut.ShortcutKeyDisplayString = "Ctrl+X"
        Me.menu_Cut.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.menu_Cut.Size = New System.Drawing.Size(170, 22)
        Me.menu_Cut.Text = "剪切"
        '
        'menu_Copy
        '
        Me.menu_Copy.Name = "menu_Copy"
        Me.menu_Copy.ShortcutKeyDisplayString = "Ctrl+C"
        Me.menu_Copy.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.menu_Copy.Size = New System.Drawing.Size(170, 22)
        Me.menu_Copy.Text = "复制"
        '
        'menu_Paste
        '
        Me.menu_Paste.Name = "menu_Paste"
        Me.menu_Paste.ShortcutKeyDisplayString = "Ctrl+V"
        Me.menu_Paste.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.menu_Paste.Size = New System.Drawing.Size(170, 22)
        Me.menu_Paste.Text = "粘贴"
        '
        'menu_Undo
        '
        Me.menu_Undo.Name = "menu_Undo"
        Me.menu_Undo.ShortcutKeyDisplayString = "Ctrl+Z"
        Me.menu_Undo.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Z), System.Windows.Forms.Keys)
        Me.menu_Undo.Size = New System.Drawing.Size(170, 22)
        Me.menu_Undo.Text = "撤销"
        '
        'frmCodeFile
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(792, 565)
        Me.Controls.Add(Me.txtFileText)
        Me.Name = "frmCodeFile"
        Me.Text = "文件"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtFileText As System.Windows.Forms.RichTextBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents 编辑ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 启用外部程序编辑ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 保存ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 关闭ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menu_Paste As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menu_Copy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents menu_Cut As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menu_Undo As System.Windows.Forms.ToolStripMenuItem
End Class
