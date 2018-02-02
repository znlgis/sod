<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm12306Ticket
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
        Me.components = New System.ComponentModel.Container()
        Me.panel1 = New System.Windows.Forms.Panel()
        Me.label1 = New System.Windows.Forms.Label()
        Me.ckbToStation = New System.Windows.Forms.CheckBox()
        Me.ckbFromStation = New System.Windows.Forms.CheckBox()
        Me.txtToStation = New System.Windows.Forms.TextBox()
        Me.txtFromStation = New System.Windows.Forms.TextBox()
        Me.label2 = New System.Windows.Forms.Label()
        Me.numericUpDown1 = New System.Windows.Forms.NumericUpDown()
        Me.linkMsg = New System.Windows.Forms.LinkLabel()
        Me.lblMsg = New System.Windows.Forms.Label()
        Me.btnOpenUrl2 = New System.Windows.Forms.Button()
        Me.btnOpenUrl = New System.Windows.Forms.Button()
        Me.txtUrl = New System.Windows.Forms.TextBox()
        Me.panBody = New System.Windows.Forms.Panel()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.panel1.SuspendLayout()
        CType(Me.numericUpDown1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'panel1
        '
        Me.panel1.Controls.Add(Me.label1)
        Me.panel1.Controls.Add(Me.ckbToStation)
        Me.panel1.Controls.Add(Me.ckbFromStation)
        Me.panel1.Controls.Add(Me.txtToStation)
        Me.panel1.Controls.Add(Me.txtFromStation)
        Me.panel1.Controls.Add(Me.label2)
        Me.panel1.Controls.Add(Me.numericUpDown1)
        Me.panel1.Controls.Add(Me.linkMsg)
        Me.panel1.Controls.Add(Me.lblMsg)
        Me.panel1.Controls.Add(Me.btnOpenUrl2)
        Me.panel1.Controls.Add(Me.btnOpenUrl)
        Me.panel1.Controls.Add(Me.txtUrl)
        Me.panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.panel1.Location = New System.Drawing.Point(0, 0)
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New System.Drawing.Size(738, 113)
        Me.panel1.TabIndex = 2
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(17, 51)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(419, 12)
        Me.label1.TabIndex = 13
        Me.label1.Text = "填写并启用备用站点，增加成功率，比如北京站可以填写备用站点为 北京西。"
        '
        'ckbToStation
        '
        Me.ckbToStation.AutoSize = True
        Me.ckbToStation.Location = New System.Drawing.Point(667, 47)
        Me.ckbToStation.Name = "ckbToStation"
        Me.ckbToStation.Size = New System.Drawing.Size(84, 16)
        Me.ckbToStation.TabIndex = 12
        Me.ckbToStation.Text = "备用目的地"
        Me.ckbToStation.UseVisualStyleBackColor = True
        '
        'ckbFromStation
        '
        Me.ckbFromStation.AutoSize = True
        Me.ckbFromStation.Location = New System.Drawing.Point(471, 47)
        Me.ckbFromStation.Name = "ckbFromStation"
        Me.ckbFromStation.Size = New System.Drawing.Size(84, 16)
        Me.ckbFromStation.TabIndex = 11
        Me.ckbFromStation.Text = "备用出发地"
        Me.ckbFromStation.UseVisualStyleBackColor = True
        '
        'txtToStation
        '
        Me.txtToStation.Location = New System.Drawing.Point(757, 42)
        Me.txtToStation.Name = "txtToStation"
        Me.txtToStation.Size = New System.Drawing.Size(100, 21)
        Me.txtToStation.TabIndex = 10
        '
        'txtFromStation
        '
        Me.txtFromStation.Location = New System.Drawing.Point(561, 42)
        Me.txtFromStation.Name = "txtFromStation"
        Me.txtFromStation.Size = New System.Drawing.Size(100, 21)
        Me.txtFromStation.TabIndex = 9
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(572, 19)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(89, 12)
        Me.label2.TabIndex = 6
        Me.label2.Text = "刷新间隔（秒）"
        '
        'numericUpDown1
        '
        Me.numericUpDown1.Location = New System.Drawing.Point(667, 10)
        Me.numericUpDown1.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.numericUpDown1.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numericUpDown1.Name = "numericUpDown1"
        Me.numericUpDown1.Size = New System.Drawing.Size(37, 21)
        Me.numericUpDown1.TabIndex = 5
        Me.numericUpDown1.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'linkMsg
        '
        Me.linkMsg.AutoSize = True
        Me.linkMsg.Location = New System.Drawing.Point(767, 19)
        Me.linkMsg.Name = "linkMsg"
        Me.linkMsg.Size = New System.Drawing.Size(53, 12)
        Me.linkMsg.TabIndex = 4
        Me.linkMsg.TabStop = True
        Me.linkMsg.Text = "使用说明"
        '
        'lblMsg
        '
        Me.lblMsg.AutoSize = True
        Me.lblMsg.Location = New System.Drawing.Point(17, 78)
        Me.lblMsg.Name = "lblMsg"
        Me.lblMsg.Size = New System.Drawing.Size(803, 24)
        Me.lblMsg.TabIndex = 3
        Me.lblMsg.Text = "12306自助购票无声弹窗程序，可以开启本程序后进行其它工作，甚至在电脑关闭声音的情况下，及时通知有票信息。单击【开始刷票】按钮开始操作。" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "请在购票网页中登录并" &
    "输入你的购票信息，开启自动提交功能。"
        '
        'btnOpenUrl2
        '
        Me.btnOpenUrl2.Location = New System.Drawing.Point(471, 8)
        Me.btnOpenUrl2.Name = "btnOpenUrl2"
        Me.btnOpenUrl2.Size = New System.Drawing.Size(75, 23)
        Me.btnOpenUrl2.TabIndex = 2
        Me.btnOpenUrl2.Text = "开始刷票"
        Me.btnOpenUrl2.UseVisualStyleBackColor = True
        '
        'btnOpenUrl
        '
        Me.btnOpenUrl.Location = New System.Drawing.Point(369, 8)
        Me.btnOpenUrl.Name = "btnOpenUrl"
        Me.btnOpenUrl.Size = New System.Drawing.Size(75, 23)
        Me.btnOpenUrl.TabIndex = 1
        Me.btnOpenUrl.Text = "打开测试页"
        Me.btnOpenUrl.UseVisualStyleBackColor = True
        '
        'txtUrl
        '
        Me.txtUrl.Location = New System.Drawing.Point(19, 10)
        Me.txtUrl.Name = "txtUrl"
        Me.txtUrl.Size = New System.Drawing.Size(327, 21)
        Me.txtUrl.TabIndex = 0
        Me.txtUrl.Text = "TestHtml.html"
        '
        'panBody
        '
        Me.panBody.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panBody.Location = New System.Drawing.Point(0, 113)
        Me.panBody.Name = "panBody"
        Me.panBody.Size = New System.Drawing.Size(738, 407)
        Me.panBody.TabIndex = 3
        '
        'Timer1
        '
        Me.Timer1.Interval = 10000
        '
        'frm12306Ticket
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(738, 520)
        Me.Controls.Add(Me.panBody)
        Me.Controls.Add(Me.panel1)
        Me.Name = "frm12306Ticket"
        Me.Text = "frm12306Ticket"
        Me.panel1.ResumeLayout(False)
        Me.panel1.PerformLayout()
        CType(Me.numericUpDown1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Private WithEvents panel1 As Panel
    Private WithEvents label1 As Label
    Private WithEvents ckbToStation As CheckBox
    Private WithEvents ckbFromStation As CheckBox
    Private WithEvents txtToStation As TextBox
    Private WithEvents txtFromStation As TextBox
    Private WithEvents label2 As Label
    Private WithEvents numericUpDown1 As NumericUpDown
    Private WithEvents linkMsg As LinkLabel
    Private WithEvents lblMsg As Label
    Private WithEvents btnOpenUrl2 As Button
    Private WithEvents btnOpenUrl As Button
    Private WithEvents txtUrl As TextBox
    Friend WithEvents panBody As Panel
    Friend WithEvents Timer1 As Timer
End Class
