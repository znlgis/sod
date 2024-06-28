<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDBLogin
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnProviderBrowser = New System.Windows.Forms.Button()
        Me.txtProviderName = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnFileBrowser = New System.Windows.Forms.Button()
        Me.cmbLoginType = New System.Windows.Forms.ComboBox()
        Me.lblLoginType = New System.Windows.Forms.Label()
        Me.txtPwd = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtLogName = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtServerName = New System.Windows.Forms.TextBox()
        Me.lblServerName = New System.Windows.Forms.Label()
        Me.cmbDbEngine = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCacle = New System.Windows.Forms.Button()
        Me.chkMoreInfo = New System.Windows.Forms.CheckBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lnkRefesh = New System.Windows.Forms.LinkLabel()
        Me.btnSaveConn = New System.Windows.Forms.Button()
        Me.btnTestConn = New System.Windows.Forms.Button()
        Me.txtConnStr = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.btnProviderBrowser)
        Me.Panel1.Controls.Add(Me.txtProviderName)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.btnFileBrowser)
        Me.Panel1.Controls.Add(Me.cmbLoginType)
        Me.Panel1.Controls.Add(Me.lblLoginType)
        Me.Panel1.Controls.Add(Me.txtPwd)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.txtLogName)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.txtServerName)
        Me.Panel1.Controls.Add(Me.lblServerName)
        Me.Panel1.Controls.Add(Me.cmbDbEngine)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Panel1.Location = New System.Drawing.Point(193, 15)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(549, 246)
        Me.Panel1.TabIndex = 8
        '
        'btnProviderBrowser
        '
        Me.btnProviderBrowser.Enabled = False
        Me.btnProviderBrowser.Location = New System.Drawing.Point(465, 50)
        Me.btnProviderBrowser.Margin = New System.Windows.Forms.Padding(4)
        Me.btnProviderBrowser.Name = "btnProviderBrowser"
        Me.btnProviderBrowser.Size = New System.Drawing.Size(61, 29)
        Me.btnProviderBrowser.TabIndex = 21
        Me.btnProviderBrowser.Text = "浏览"
        Me.btnProviderBrowser.UseVisualStyleBackColor = True
        '
        'txtProviderName
        '
        Me.txtProviderName.Location = New System.Drawing.Point(147, 52)
        Me.txtProviderName.Margin = New System.Windows.Forms.Padding(4)
        Me.txtProviderName.Name = "txtProviderName"
        Me.txtProviderName.ReadOnly = True
        Me.txtProviderName.Size = New System.Drawing.Size(304, 25)
        Me.txtProviderName.TabIndex = 20
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(20, 65)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(112, 15)
        Me.Label2.TabIndex = 19
        Me.Label2.Text = "数据提供程序："
        '
        'btnFileBrowser
        '
        Me.btnFileBrowser.Location = New System.Drawing.Point(465, 88)
        Me.btnFileBrowser.Margin = New System.Windows.Forms.Padding(4)
        Me.btnFileBrowser.Name = "btnFileBrowser"
        Me.btnFileBrowser.Size = New System.Drawing.Size(61, 29)
        Me.btnFileBrowser.TabIndex = 18
        Me.btnFileBrowser.Text = "浏览"
        Me.btnFileBrowser.UseVisualStyleBackColor = True
        '
        'cmbLoginType
        '
        Me.cmbLoginType.FormattingEnabled = True
        Me.cmbLoginType.Location = New System.Drawing.Point(147, 130)
        Me.cmbLoginType.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbLoginType.Name = "cmbLoginType"
        Me.cmbLoginType.Size = New System.Drawing.Size(235, 23)
        Me.cmbLoginType.TabIndex = 17
        '
        'lblLoginType
        '
        Me.lblLoginType.Location = New System.Drawing.Point(52, 139)
        Me.lblLoginType.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblLoginType.Name = "lblLoginType"
        Me.lblLoginType.Size = New System.Drawing.Size(87, 15)
        Me.lblLoginType.TabIndex = 16
        Me.lblLoginType.Text = "登录方式："
        Me.lblLoginType.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtPwd
        '
        Me.txtPwd.Location = New System.Drawing.Point(147, 202)
        Me.txtPwd.Margin = New System.Windows.Forms.Padding(4)
        Me.txtPwd.Name = "txtPwd"
        Me.txtPwd.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPwd.Size = New System.Drawing.Size(235, 25)
        Me.txtPwd.TabIndex = 15
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(52, 210)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(82, 15)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "登录密码："
        '
        'txtLogName
        '
        Me.txtLogName.Location = New System.Drawing.Point(147, 166)
        Me.txtLogName.Margin = New System.Windows.Forms.Padding(4)
        Me.txtLogName.Name = "txtLogName"
        Me.txtLogName.Size = New System.Drawing.Size(235, 25)
        Me.txtLogName.TabIndex = 13
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(52, 175)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(82, 15)
        Me.Label3.TabIndex = 12
        Me.Label3.Text = "登录名称："
        '
        'txtServerName
        '
        Me.txtServerName.Location = New System.Drawing.Point(147, 90)
        Me.txtServerName.Margin = New System.Windows.Forms.Padding(4)
        Me.txtServerName.Name = "txtServerName"
        Me.txtServerName.Size = New System.Drawing.Size(304, 25)
        Me.txtServerName.TabIndex = 11
        '
        'lblServerName
        '
        Me.lblServerName.Location = New System.Drawing.Point(4, 94)
        Me.lblServerName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblServerName.Name = "lblServerName"
        Me.lblServerName.Size = New System.Drawing.Size(135, 26)
        Me.lblServerName.TabIndex = 10
        Me.lblServerName.Text = "服务器名称："
        Me.lblServerName.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbDbEngine
        '
        Me.cmbDbEngine.FormattingEnabled = True
        Me.cmbDbEngine.Location = New System.Drawing.Point(147, 20)
        Me.cmbDbEngine.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbDbEngine.Name = "cmbDbEngine"
        Me.cmbDbEngine.Size = New System.Drawing.Size(304, 23)
        Me.cmbDbEngine.TabIndex = 9
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(36, 30)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(97, 15)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "服务器类型："
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(260, 282)
        Me.btnOK.Margin = New System.Windows.Forms.Padding(4)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(100, 29)
        Me.btnOK.TabIndex = 9
        Me.btnOK.Text = "确定"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCacle
        '
        Me.btnCacle.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCacle.Location = New System.Drawing.Point(407, 282)
        Me.btnCacle.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCacle.Name = "btnCacle"
        Me.btnCacle.Size = New System.Drawing.Size(100, 29)
        Me.btnCacle.TabIndex = 10
        Me.btnCacle.Text = "取消"
        Me.btnCacle.UseVisualStyleBackColor = True
        '
        'chkMoreInfo
        '
        Me.chkMoreInfo.AutoSize = True
        Me.chkMoreInfo.Location = New System.Drawing.Point(580, 291)
        Me.chkMoreInfo.Margin = New System.Windows.Forms.Padding(4)
        Me.chkMoreInfo.Name = "chkMoreInfo"
        Me.chkMoreInfo.Size = New System.Drawing.Size(89, 19)
        Me.chkMoreInfo.TabIndex = 11
        Me.chkMoreInfo.Text = "高级选项"
        Me.chkMoreInfo.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.SystemColors.Control
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.lnkRefesh)
        Me.Panel2.Controls.Add(Me.btnSaveConn)
        Me.Panel2.Controls.Add(Me.btnTestConn)
        Me.Panel2.Controls.Add(Me.txtConnStr)
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Location = New System.Drawing.Point(3, 330)
        Me.Panel2.Margin = New System.Windows.Forms.Padding(4)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(739, 124)
        Me.Panel2.TabIndex = 12
        '
        'lnkRefesh
        '
        Me.lnkRefesh.AutoSize = True
        Me.lnkRefesh.Location = New System.Drawing.Point(143, 19)
        Me.lnkRefesh.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lnkRefesh.Name = "lnkRefesh"
        Me.lnkRefesh.Size = New System.Drawing.Size(37, 15)
        Me.lnkRefesh.TabIndex = 4
        Me.lnkRefesh.TabStop = True
        Me.lnkRefesh.Text = "刷新"
        '
        'btnSaveConn
        '
        Me.btnSaveConn.Location = New System.Drawing.Point(409, 90)
        Me.btnSaveConn.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSaveConn.Name = "btnSaveConn"
        Me.btnSaveConn.Size = New System.Drawing.Size(100, 29)
        Me.btnSaveConn.TabIndex = 3
        Me.btnSaveConn.Text = "保存连接"
        Me.btnSaveConn.UseVisualStyleBackColor = True
        '
        'btnTestConn
        '
        Me.btnTestConn.Location = New System.Drawing.Point(263, 90)
        Me.btnTestConn.Margin = New System.Windows.Forms.Padding(4)
        Me.btnTestConn.Name = "btnTestConn"
        Me.btnTestConn.Size = New System.Drawing.Size(100, 29)
        Me.btnTestConn.TabIndex = 2
        Me.btnTestConn.Text = "测试连接"
        Me.btnTestConn.UseVisualStyleBackColor = True
        '
        'txtConnStr
        '
        Me.txtConnStr.Location = New System.Drawing.Point(35, 38)
        Me.txtConnStr.Margin = New System.Windows.Forms.Padding(4)
        Me.txtConnStr.Name = "txtConnStr"
        Me.txtConnStr.Size = New System.Drawing.Size(613, 25)
        Me.txtConnStr.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(35, 19)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(97, 15)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "连接字符串："
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.PDFDotNET.My.Resources.Resources.数据导出
        Me.PictureBox1.Location = New System.Drawing.Point(-4, -2)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(136, 335)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 13
        Me.PictureBox1.TabStop = False
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'frmDBLogin
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCacle
        Me.ClientSize = New System.Drawing.Size(759, 461)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.chkMoreInfo)
        Me.Controls.Add(Me.btnCacle)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.Name = "frmDBLogin"
        Me.Text = "连接到服务器"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents txtPwd As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtLogName As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtServerName As System.Windows.Forms.TextBox
    Friend WithEvents lblServerName As System.Windows.Forms.Label
    Friend WithEvents cmbDbEngine As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCacle As System.Windows.Forms.Button
    Friend WithEvents chkMoreInfo As System.Windows.Forms.CheckBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnSaveConn As System.Windows.Forms.Button
    Friend WithEvents btnTestConn As System.Windows.Forms.Button
    Friend WithEvents txtConnStr As System.Windows.Forms.TextBox
    Friend WithEvents cmbLoginType As System.Windows.Forms.ComboBox
    Friend WithEvents lblLoginType As System.Windows.Forms.Label
    Friend WithEvents btnFileBrowser As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents lnkRefesh As System.Windows.Forms.LinkLabel
    Friend WithEvents txtProviderName As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnProviderBrowser As System.Windows.Forms.Button
End Class
