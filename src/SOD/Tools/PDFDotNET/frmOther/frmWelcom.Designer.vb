<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmWelcom
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()>
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

    '注意:  以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.panHead = New System.Windows.Forms.Panel()
        Me.btnNewTabWindow = New System.Windows.Forms.Button()
        Me.lnkNewWindow = New System.Windows.Forms.LinkLabel()
        Me.lnk12306 = New System.Windows.Forms.LinkLabel()
        Me.picGoHome = New System.Windows.Forms.PictureBox()
        Me.picRefersh = New System.Windows.Forms.PictureBox()
        Me.picGofawerd = New System.Windows.Forms.PictureBox()
        Me.picGoback = New System.Windows.Forms.PictureBox()
        Me.btnGo = New System.Windows.Forms.Button()
        Me.txtUrl = New System.Windows.Forms.TextBox()
        Me.panBody = New System.Windows.Forms.Panel()
        Me.cmbHotWebSite = New System.Windows.Forms.ComboBox()
        Me.panHead.SuspendLayout()
        CType(Me.picGoHome, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picRefersh, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picGofawerd, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picGoback, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'panHead
        '
        Me.panHead.BackColor = System.Drawing.Color.White
        Me.panHead.Controls.Add(Me.cmbHotWebSite)
        Me.panHead.Controls.Add(Me.btnNewTabWindow)
        Me.panHead.Controls.Add(Me.lnkNewWindow)
        Me.panHead.Controls.Add(Me.lnk12306)
        Me.panHead.Controls.Add(Me.picGoHome)
        Me.panHead.Controls.Add(Me.picRefersh)
        Me.panHead.Controls.Add(Me.picGofawerd)
        Me.panHead.Controls.Add(Me.picGoback)
        Me.panHead.Controls.Add(Me.btnGo)
        Me.panHead.Controls.Add(Me.txtUrl)
        Me.panHead.Dock = System.Windows.Forms.DockStyle.Top
        Me.panHead.Location = New System.Drawing.Point(0, 0)
        Me.panHead.Margin = New System.Windows.Forms.Padding(4)
        Me.panHead.Name = "panHead"
        Me.panHead.Size = New System.Drawing.Size(1475, 78)
        Me.panHead.TabIndex = 0
        '
        'btnNewTabWindow
        '
        Me.btnNewTabWindow.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnNewTabWindow.Font = New System.Drawing.Font("黑体", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.btnNewTabWindow.Location = New System.Drawing.Point(232, 12)
        Me.btnNewTabWindow.Name = "btnNewTabWindow"
        Me.btnNewTabWindow.Size = New System.Drawing.Size(50, 50)
        Me.btnNewTabWindow.TabIndex = 8
        Me.btnNewTabWindow.Text = "+"
        Me.btnNewTabWindow.UseVisualStyleBackColor = True
        '
        'lnkNewWindow
        '
        Me.lnkNewWindow.AutoSize = True
        Me.lnkNewWindow.Font = New System.Drawing.Font("宋体", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.lnkNewWindow.Location = New System.Drawing.Point(1064, 30)
        Me.lnkNewWindow.Name = "lnkNewWindow"
        Me.lnkNewWindow.Size = New System.Drawing.Size(94, 24)
        Me.lnkNewWindow.TabIndex = 7
        Me.lnkNewWindow.TabStop = True
        Me.lnkNewWindow.Text = "+新窗口"
        '
        'lnk12306
        '
        Me.lnk12306.AutoSize = True
        Me.lnk12306.Font = New System.Drawing.Font("宋体", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.lnk12306.Location = New System.Drawing.Point(1165, 30)
        Me.lnk12306.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lnk12306.Name = "lnk12306"
        Me.lnk12306.Size = New System.Drawing.Size(166, 24)
        Me.lnk12306.TabIndex = 6
        Me.lnk12306.TabStop = True
        Me.lnk12306.Text = "12306无声刷票"
        '
        'picGoHome
        '
        Me.picGoHome.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picGoHome.Image = Global.PDFDotNET.My.Resources.Resources.home
        Me.picGoHome.Location = New System.Drawing.Point(175, 10)
        Me.picGoHome.Margin = New System.Windows.Forms.Padding(4)
        Me.picGoHome.Name = "picGoHome"
        Me.picGoHome.Size = New System.Drawing.Size(55, 55)
        Me.picGoHome.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picGoHome.TabIndex = 5
        Me.picGoHome.TabStop = False
        '
        'picRefersh
        '
        Me.picRefersh.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picRefersh.Image = Global.PDFDotNET.My.Resources.Resources.refresh
        Me.picRefersh.Location = New System.Drawing.Point(119, 10)
        Me.picRefersh.Margin = New System.Windows.Forms.Padding(4)
        Me.picRefersh.Name = "picRefersh"
        Me.picRefersh.Size = New System.Drawing.Size(55, 55)
        Me.picRefersh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picRefersh.TabIndex = 4
        Me.picRefersh.TabStop = False
        '
        'picGofawerd
        '
        Me.picGofawerd.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picGofawerd.Image = Global.PDFDotNET.My.Resources.Resources.gofawert
        Me.picGofawerd.Location = New System.Drawing.Point(63, 10)
        Me.picGofawerd.Margin = New System.Windows.Forms.Padding(4)
        Me.picGofawerd.Name = "picGofawerd"
        Me.picGofawerd.Size = New System.Drawing.Size(55, 55)
        Me.picGofawerd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picGofawerd.TabIndex = 3
        Me.picGofawerd.TabStop = False
        '
        'picGoback
        '
        Me.picGoback.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picGoback.Image = Global.PDFDotNET.My.Resources.Resources.goback
        Me.picGoback.Location = New System.Drawing.Point(8, 10)
        Me.picGoback.Margin = New System.Windows.Forms.Padding(4)
        Me.picGoback.Name = "picGoback"
        Me.picGoback.Size = New System.Drawing.Size(55, 55)
        Me.picGoback.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picGoback.TabIndex = 2
        Me.picGoback.TabStop = False
        '
        'btnGo
        '
        Me.btnGo.BackgroundImage = Global.PDFDotNET.My.Resources.Resources.gopage
        Me.btnGo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.btnGo.FlatAppearance.BorderSize = 0
        Me.btnGo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGo.Font = New System.Drawing.Font("宋体", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.btnGo.ForeColor = System.Drawing.Color.Maroon
        Me.btnGo.Location = New System.Drawing.Point(1002, 4)
        Me.btnGo.Margin = New System.Windows.Forms.Padding(4)
        Me.btnGo.Name = "btnGo"
        Me.btnGo.Size = New System.Drawing.Size(55, 55)
        Me.btnGo.TabIndex = 1
        Me.btnGo.Text = "Go"
        Me.btnGo.UseVisualStyleBackColor = True
        '
        'txtUrl
        '
        Me.txtUrl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtUrl.Font = New System.Drawing.Font("宋体", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.txtUrl.ForeColor = System.Drawing.Color.Sienna
        Me.txtUrl.Location = New System.Drawing.Point(285, 19)
        Me.txtUrl.Margin = New System.Windows.Forms.Padding(4)
        Me.txtUrl.Name = "txtUrl"
        Me.txtUrl.Size = New System.Drawing.Size(709, 35)
        Me.txtUrl.TabIndex = 0
        Me.txtUrl.Text = "http://www.pwmis.com/sqlmap"
        '
        'panBody
        '
        Me.panBody.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panBody.Location = New System.Drawing.Point(0, 78)
        Me.panBody.Margin = New System.Windows.Forms.Padding(4)
        Me.panBody.Name = "panBody"
        Me.panBody.Size = New System.Drawing.Size(1475, 563)
        Me.panBody.TabIndex = 1
        '
        'cmbHotWebSite
        '
        Me.cmbHotWebSite.Font = New System.Drawing.Font("宋体", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.cmbHotWebSite.ForeColor = System.Drawing.Color.Sienna
        Me.cmbHotWebSite.FormattingEnabled = True
        Me.cmbHotWebSite.Location = New System.Drawing.Point(1338, 22)
        Me.cmbHotWebSite.Name = "cmbHotWebSite"
        Me.cmbHotWebSite.Size = New System.Drawing.Size(125, 32)
        Me.cmbHotWebSite.TabIndex = 9
        '
        'frmWelcom
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 18.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1475, 641)
        Me.Controls.Add(Me.panBody)
        Me.Controls.Add(Me.panHead)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmWelcom"
        Me.Text = "frmWelcom"
        Me.panHead.ResumeLayout(False)
        Me.panHead.PerformLayout()
        CType(Me.picGoHome, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picRefersh, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picGofawerd, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picGoback, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents panHead As Panel
    Friend WithEvents btnGo As Button
    Friend WithEvents txtUrl As TextBox
    Friend WithEvents panBody As Panel
    Friend WithEvents picGoback As PictureBox
    Friend WithEvents picGoHome As PictureBox
    Friend WithEvents picRefersh As PictureBox
    Friend WithEvents picGofawerd As PictureBox
    Friend WithEvents lnk12306 As LinkLabel
    Friend WithEvents lnkNewWindow As LinkLabel
    Friend WithEvents btnNewTabWindow As Button
    Friend WithEvents cmbHotWebSite As ComboBox
End Class
