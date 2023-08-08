Imports System.ComponentModel
Imports System.IO
Imports System.Xml
Imports PWMIS.Common
Imports PWMIS.DataProvider.Adapter
Imports PWMIS.DataProvider.Data

Public Class frmNewOpt
    Inherits Form

    Dim ArrListItem As New ArrayList
    Dim ReadOnly ArrUpdateServerType As New ArrayList '记录更新脚本类型快（添加，删除）
    Public CurrConfigFile As String = String.Empty
    Friend WithEvents Label14 As Label
    Friend WithEvents txtCmdsInterface As TextBox
    Dim sCmdsFlag As String

#Region " Windows 窗体设计器生成的代码 "

    Public Sub New()
        MyBase.New()

        '该调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        '在 InitializeComponent() 调用之后添加任何初始化
    End Sub

    '窗体重写 dispose 以清理组件列表。
    Protected Overloads Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Windows 窗体设计器所必需的
    Private components As IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改此过程。
    '不要使用代码编辑器修改它。
    Friend WithEvents tabNewFile As TabPage
    Friend WithEvents tabAddServerType As TabPage
    Friend WithEvents tabCmdClass As TabPage
    Friend WithEvents btnOK As Button
    Friend WithEvents btnCancle As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents txtFilePath As TextBox
    Friend WithEvents btnBrows As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents txtFileName As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents lstSuportType As ListBox
    Friend WithEvents lstExistsType As ListBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents lbCurrentFile As Label
    Friend WithEvents btnTypeAdd As Button
    Friend WithEvents txtServerVer As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents lstExistsCmdGroup As ListBox
    Friend WithEvents btnCmdsDel As Button
    Friend WithEvents btnCmdsAdd As Button
    Friend WithEvents btnCmdsEdit As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label10 As Label
    Friend WithEvents txtCmdsName As TextBox
    Friend WithEvents txtCmdsClass As TextBox
    Friend WithEvents txtCmdsDesc As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents lblCurrDBType As Label
    Friend WithEvents TabConfiger As TabControl
    Friend WithEvents btnTypeRemove As Button
    Friend WithEvents tabDbConn As TabPage
    Friend WithEvents lblCurrDbType1 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents txtConnString As TextBox
    Friend WithEvents btnConnTest As Button

    <DebuggerStepThrough>
    Private Sub InitializeComponent()
        Me.TabConfiger = New TabControl
        Me.tabNewFile = New TabPage
        Me.txtFileName = New TextBox
        Me.Label3 = New Label
        Me.Label2 = New Label
        Me.btnBrows = New Button
        Me.txtFilePath = New TextBox
        Me.Label1 = New Label
        Me.tabAddServerType = New TabPage
        Me.Label7 = New Label
        Me.txtServerVer = New TextBox
        Me.btnTypeRemove = New Button
        Me.btnTypeAdd = New Button
        Me.lbCurrentFile = New Label
        Me.Label6 = New Label
        Me.Label5 = New Label
        Me.lstExistsType = New ListBox
        Me.lstSuportType = New ListBox
        Me.Label4 = New Label
        Me.tabCmdClass = New TabPage
        Me.lblCurrDBType = New Label
        Me.GroupBox1 = New GroupBox
        Me.Label12 = New Label
        Me.Label11 = New Label
        Me.Label10 = New Label
        Me.txtCmdsDesc = New TextBox
        Me.txtCmdsClass = New TextBox
        Me.txtCmdsName = New TextBox
        Me.btnCmdsEdit = New Button
        Me.btnCmdsAdd = New Button
        Me.btnCmdsDel = New Button
        Me.lstExistsCmdGroup = New ListBox
        Me.Label9 = New Label
        Me.Label8 = New Label
        Me.tabDbConn = New TabPage
        Me.btnConnTest = New Button
        Me.txtConnString = New TextBox
        Me.Label13 = New Label
        Me.lblCurrDbType1 = New Label
        Me.btnOK = New Button
        Me.btnCancle = New Button
        Me.FolderBrowserDialog1 = New FolderBrowserDialog
        Me.Label14 = New Label
        Me.txtCmdsInterface = New TextBox
        Me.TabConfiger.SuspendLayout()
        Me.tabNewFile.SuspendLayout()
        Me.tabAddServerType.SuspendLayout()
        Me.tabCmdClass.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.tabDbConn.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabConfiger
        '
        Me.TabConfiger.Controls.Add(Me.tabNewFile)
        Me.TabConfiger.Controls.Add(Me.tabAddServerType)
        Me.TabConfiger.Controls.Add(Me.tabCmdClass)
        Me.TabConfiger.Controls.Add(Me.tabDbConn)
        Me.TabConfiger.Location = New Drawing.Point(10, 10)
        Me.TabConfiger.Name = "TabConfiger"
        Me.TabConfiger.SelectedIndex = 0
        Me.TabConfiger.Size = New Size(376, 242)
        Me.TabConfiger.TabIndex = 0
        '
        'tabNewFile
        '
        Me.tabNewFile.Controls.Add(Me.txtFileName)
        Me.tabNewFile.Controls.Add(Me.Label3)
        Me.tabNewFile.Controls.Add(Me.Label2)
        Me.tabNewFile.Controls.Add(Me.btnBrows)
        Me.tabNewFile.Controls.Add(Me.txtFilePath)
        Me.tabNewFile.Controls.Add(Me.Label1)
        Me.tabNewFile.Location = New Drawing.Point(4, 22)
        Me.tabNewFile.Name = "tabNewFile"
        Me.tabNewFile.Size = New Size(368, 216)
        Me.tabNewFile.TabIndex = 0
        Me.tabNewFile.Text = "新建文件"
        '
        'txtFileName
        '
        Me.txtFileName.Location = New Drawing.Point(8, 128)
        Me.txtFileName.Name = "txtFileName"
        Me.txtFileName.Size = New Size(136, 21)
        Me.txtFileName.TabIndex = 3
        Me.txtFileName.Text = "*.config"
        '
        'Label3
        '
        Me.Label3.Location = New Drawing.Point(8, 104)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New Size(152, 23)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "请输入要保存的文件名："
        '
        'Label2
        '
        Me.Label2.Location = New Drawing.Point(8, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New Size(168, 23)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "请输入要保存的文件路径："
        '
        'btnBrows
        '
        Me.btnBrows.Location = New Drawing.Point(279, 72)
        Me.btnBrows.Name = "btnBrows"
        Me.btnBrows.Size = New Size(75, 23)
        Me.btnBrows.TabIndex = 2
        Me.btnBrows.Text = "(&B)浏览"
        '
        'txtFilePath
        '
        Me.txtFilePath.Location = New Drawing.Point(8, 72)
        Me.txtFilePath.Name = "txtFilePath"
        Me.txtFilePath.Size = New Size(265, 21)
        Me.txtFilePath.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.Location = New Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New Size(312, 23)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "新建一个SqlMap配置文件。"
        '
        'tabAddServerType
        '
        Me.tabAddServerType.Controls.Add(Me.Label7)
        Me.tabAddServerType.Controls.Add(Me.txtServerVer)
        Me.tabAddServerType.Controls.Add(Me.btnTypeRemove)
        Me.tabAddServerType.Controls.Add(Me.btnTypeAdd)
        Me.tabAddServerType.Controls.Add(Me.lbCurrentFile)
        Me.tabAddServerType.Controls.Add(Me.Label6)
        Me.tabAddServerType.Controls.Add(Me.Label5)
        Me.tabAddServerType.Controls.Add(Me.lstExistsType)
        Me.tabAddServerType.Controls.Add(Me.lstSuportType)
        Me.tabAddServerType.Controls.Add(Me.Label4)
        Me.tabAddServerType.Location = New Drawing.Point(4, 22)
        Me.tabAddServerType.Name = "tabAddServerType"
        Me.tabAddServerType.Size = New Size(368, 216)
        Me.tabAddServerType.TabIndex = 1
        Me.tabAddServerType.Text = "数据库类型"
        '
        'Label7
        '
        Me.Label7.Location = New Drawing.Point(160, 186)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New Size(48, 16)
        Me.Label7.TabIndex = 9
        Me.Label7.Text = "版本："
        '
        'txtServerVer
        '
        Me.txtServerVer.Location = New Drawing.Point(219, 178)
        Me.txtServerVer.Name = "txtServerVer"
        Me.txtServerVer.Size = New Size(120, 21)
        Me.txtServerVer.TabIndex = 6
        '
        'btnTypeRemove
        '
        Me.btnTypeRemove.Location = New Drawing.Point(155, 146)
        Me.btnTypeRemove.Name = "btnTypeRemove"
        Me.btnTypeRemove.Size = New Size(48, 23)
        Me.btnTypeRemove.TabIndex = 4
        Me.btnTypeRemove.Text = "<<"
        '
        'btnTypeAdd
        '
        Me.btnTypeAdd.Location = New Drawing.Point(155, 106)
        Me.btnTypeAdd.Name = "btnTypeAdd"
        Me.btnTypeAdd.Size = New Size(48, 23)
        Me.btnTypeAdd.TabIndex = 2
        Me.btnTypeAdd.Text = ">>"
        '
        'lbCurrentFile
        '
        Me.lbCurrentFile.Location = New Drawing.Point(8, 32)
        Me.lbCurrentFile.Name = "lbCurrentFile"
        Me.lbCurrentFile.Size = New Size(357, 42)
        Me.lbCurrentFile.TabIndex = 5
        Me.lbCurrentFile.Text = "当前文件："
        '
        'Label6
        '
        Me.Label6.Location = New Drawing.Point(219, 74)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New Size(128, 23)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "已有的数据库类型："
        '
        'Label5
        '
        Me.Label5.Location = New Drawing.Point(8, 74)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New Size(120, 23)
        Me.Label5.TabIndex = 3
        Me.Label5.Text = "支持的数据库类型："
        '
        'lstExistsType
        '
        Me.lstExistsType.ItemHeight = 12
        Me.lstExistsType.Location = New Drawing.Point(219, 98)
        Me.lstExistsType.Name = "lstExistsType"
        Me.lstExistsType.Size = New Size(120, 76)
        Me.lstExistsType.TabIndex = 5
        '
        'lstSuportType
        '
        Me.lstSuportType.ItemHeight = 12
        Me.lstSuportType.Location = New Drawing.Point(8, 98)
        Me.lstSuportType.Name = "lstSuportType"
        Me.lstSuportType.Size = New Size(120, 100)
        Me.lstSuportType.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.Location = New Drawing.Point(8, 8)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New Size(312, 23)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "管理配置文件中定义的当前所支持的数据库类型。"
        '
        'tabCmdClass
        '
        Me.tabCmdClass.Controls.Add(Me.lblCurrDBType)
        Me.tabCmdClass.Controls.Add(Me.GroupBox1)
        Me.tabCmdClass.Controls.Add(Me.btnCmdsEdit)
        Me.tabCmdClass.Controls.Add(Me.btnCmdsAdd)
        Me.tabCmdClass.Controls.Add(Me.btnCmdsDel)
        Me.tabCmdClass.Controls.Add(Me.lstExistsCmdGroup)
        Me.tabCmdClass.Controls.Add(Me.Label9)
        Me.tabCmdClass.Controls.Add(Me.Label8)
        Me.tabCmdClass.Location = New Drawing.Point(4, 22)
        Me.tabCmdClass.Name = "tabCmdClass"
        Me.tabCmdClass.Size = New Size(368, 216)
        Me.tabCmdClass.TabIndex = 2
        Me.tabCmdClass.Text = "命令组"
        '
        'lblCurrDBType
        '
        Me.lblCurrDBType.Location = New Drawing.Point(8, 32)
        Me.lblCurrDBType.Name = "lblCurrDBType"
        Me.lblCurrDBType.Size = New Size(357, 23)
        Me.lblCurrDBType.TabIndex = 8
        Me.lblCurrDBType.Text = "当前数据库类型："
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtCmdsInterface)
        Me.GroupBox1.Controls.Add(Me.Label14)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.txtCmdsDesc)
        Me.GroupBox1.Controls.Add(Me.txtCmdsClass)
        Me.GroupBox1.Controls.Add(Me.txtCmdsName)
        Me.GroupBox1.Location = New Drawing.Point(199, 56)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New Size(166, 148)
        Me.GroupBox1.TabIndex = 7
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "命令组属性"
        '
        'Label12
        '
        Me.Label12.Location = New Drawing.Point(8, 114)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New Size(32, 23)
        Me.Label12.TabIndex = 10
        Me.Label12.Text = "描述"
        '
        'Label11
        '
        Me.Label11.Location = New Drawing.Point(8, 54)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New Size(32, 23)
        Me.Label11.TabIndex = 9
        Me.Label11.Text = "类名"
        '
        'Label10
        '
        Me.Label10.Location = New Drawing.Point(8, 24)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New Size(32, 23)
        Me.Label10.TabIndex = 8
        Me.Label10.Text = "名称"
        '
        'txtCmdsDesc
        '
        Me.txtCmdsDesc.Location = New Drawing.Point(45, 114)
        Me.txtCmdsDesc.Name = "txtCmdsDesc"
        Me.txtCmdsDesc.Size = New Size(115, 21)
        Me.txtCmdsDesc.TabIndex = 9
        '
        'txtCmdsClass
        '
        Me.txtCmdsClass.Location = New Drawing.Point(45, 54)
        Me.txtCmdsClass.Name = "txtCmdsClass"
        Me.txtCmdsClass.Size = New Size(115, 21)
        Me.txtCmdsClass.TabIndex = 8
        '
        'txtCmdsName
        '
        Me.txtCmdsName.Location = New Drawing.Point(45, 24)
        Me.txtCmdsName.Name = "txtCmdsName"
        Me.txtCmdsName.Size = New Size(115, 21)
        Me.txtCmdsName.TabIndex = 7
        '
        'btnCmdsEdit
        '
        Me.btnCmdsEdit.Location = New Drawing.Point(145, 112)
        Me.btnCmdsEdit.Name = "btnCmdsEdit"
        Me.btnCmdsEdit.Size = New Size(48, 23)
        Me.btnCmdsEdit.TabIndex = 5
        Me.btnCmdsEdit.Text = "修改"
        '
        'btnCmdsAdd
        '
        Me.btnCmdsAdd.Location = New Drawing.Point(145, 78)
        Me.btnCmdsAdd.Name = "btnCmdsAdd"
        Me.btnCmdsAdd.Size = New Size(48, 23)
        Me.btnCmdsAdd.TabIndex = 4
        Me.btnCmdsAdd.Text = "添加"
        '
        'btnCmdsDel
        '
        Me.btnCmdsDel.Location = New Drawing.Point(145, 142)
        Me.btnCmdsDel.Name = "btnCmdsDel"
        Me.btnCmdsDel.Size = New Size(48, 23)
        Me.btnCmdsDel.TabIndex = 6
        Me.btnCmdsDel.Text = "删除"
        '
        'lstExistsCmdGroup
        '
        Me.lstExistsCmdGroup.ItemHeight = 12
        Me.lstExistsCmdGroup.Location = New Drawing.Point(8, 80)
        Me.lstExistsCmdGroup.Name = "lstExistsCmdGroup"
        Me.lstExistsCmdGroup.Size = New Size(131, 124)
        Me.lstExistsCmdGroup.TabIndex = 2
        '
        'Label9
        '
        Me.Label9.Location = New Drawing.Point(8, 56)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New Size(100, 23)
        Me.Label9.TabIndex = 1
        Me.Label9.Text = "已有的命令组："
        '
        'Label8
        '
        Me.Label8.Location = New Drawing.Point(8, 8)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New Size(304, 23)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "管理命令组，定义映射到数据访问层的类。"
        '
        'tabDbConn
        '
        Me.tabDbConn.Controls.Add(Me.btnConnTest)
        Me.tabDbConn.Controls.Add(Me.txtConnString)
        Me.tabDbConn.Controls.Add(Me.Label13)
        Me.tabDbConn.Controls.Add(Me.lblCurrDbType1)
        Me.tabDbConn.Location = New Drawing.Point(4, 22)
        Me.tabDbConn.Name = "tabDbConn"
        Me.tabDbConn.Size = New Size(368, 216)
        Me.tabDbConn.TabIndex = 3
        Me.tabDbConn.Text = "数据连接"
        '
        'btnConnTest
        '
        Me.btnConnTest.Location = New Drawing.Point(16, 104)
        Me.btnConnTest.Name = "btnConnTest"
        Me.btnConnTest.Size = New Size(75, 23)
        Me.btnConnTest.TabIndex = 12
        Me.btnConnTest.Text = "测试连接"
        '
        'txtConnString
        '
        Me.txtConnString.Location = New Drawing.Point(16, 72)
        Me.txtConnString.Name = "txtConnString"
        Me.txtConnString.Size = New Size(349, 21)
        Me.txtConnString.TabIndex = 11
        '
        'Label13
        '
        Me.Label13.Location = New Drawing.Point(16, 48)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New Size(120, 23)
        Me.Label13.TabIndex = 10
        Me.Label13.Text = "数据库连接字符串："
        '
        'lblCurrDbType1
        '
        Me.lblCurrDbType1.Location = New Drawing.Point(16, 16)
        Me.lblCurrDbType1.Name = "lblCurrDbType1"
        Me.lblCurrDbType1.Size = New Size(349, 23)
        Me.lblCurrDbType1.TabIndex = 9
        Me.lblCurrDbType1.Text = "当前数据库类型："
        '
        'btnOK
        '
        Me.btnOK.Enabled = False
        Me.btnOK.Location = New Drawing.Point(184, 258)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New Size(75, 23)
        Me.btnOK.TabIndex = 1
        Me.btnOK.Text = "(&Y)确定"
        '
        'btnCancle
        '
        Me.btnCancle.Location = New Drawing.Point(272, 258)
        Me.btnCancle.Name = "btnCancle"
        Me.btnCancle.Size = New Size(75, 23)
        Me.btnCancle.TabIndex = 2
        Me.btnCancle.Text = "(&C)取消"
        '
        'Label14
        '
        Me.Label14.Location = New Drawing.Point(9, 84)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New Size(32, 23)
        Me.Label14.TabIndex = 11
        Me.Label14.Text = "接口"
        '
        'txtCmdsInterface
        '
        Me.txtCmdsInterface.Location = New Drawing.Point(45, 84)
        Me.txtCmdsInterface.Name = "txtCmdsInterface"
        Me.txtCmdsInterface.Size = New Size(115, 21)
        Me.txtCmdsInterface.TabIndex = 12
        '
        'frmNewOpt
        '
        Me.AutoScaleBaseSize = New Size(6, 14)
        Me.ClientSize = New Size(406, 291)
        Me.Controls.Add(Me.btnCancle)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.TabConfiger)
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmNewOpt"
        Me.Text = "配置文件管理"
        Me.TabConfiger.ResumeLayout(False)
        Me.tabNewFile.ResumeLayout(False)
        Me.tabNewFile.PerformLayout()
        Me.tabAddServerType.ResumeLayout(False)
        Me.tabAddServerType.PerformLayout()
        Me.tabCmdClass.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.tabDbConn.ResumeLayout(False)
        Me.tabDbConn.PerformLayout()
        Me.ResumeLayout(False)
    End Sub

#End Region


    Private Sub btnCancle_Click(sender As Object, e As EventArgs) Handles btnCancle.Click
        Me.Close()
    End Sub

    Private Sub btnBrows_Click(sender As Object, e As EventArgs) Handles btnBrows.Click
        Me.FolderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer
        Me.FolderBrowserDialog1.ShowDialog()
        txtFilePath.Text = Me.FolderBrowserDialog1.SelectedPath
        CheckCreateFile()
    End Sub

    Private Sub btnTypeAdd_Click(sender As Object, e As EventArgs) Handles btnTypeAdd.Click
        If btnTypeRemove.Enabled Then ArrUpdateServerType.Clear()
        AddServerType()
        sCmdsFlag = "AddServerType"
        btnOK.Enabled = True
        btnTypeRemove.Enabled = False
    End Sub

    Private Sub AddServerType()
        If lstSuportType.SelectedIndex <> - 1 Then
            'lstSuportType.Items (0)
            Dim Item As ListItem
            Dim text As String = lstSuportType.SelectedItem
            For I As Integer = 0 To ArrListItem.Count - 1
                Item = CType(ArrListItem(I), ListItem)
                If Item.ItemText = text Then
                    MessageBox.Show("已经存在脚本类型 " & text)
                    Exit Sub
                End If
            Next
            Item = New ListItem()
            Item.ItemText = text
            Item.ItemValue = ""

            ArrListItem.Add(Item)
            ArrUpdateServerType.Add(Item)

            lstExistsType.Items.Add(text)
            lstExistsType.SelectedIndex = - 1
            lstSuportType.SelectedIndex = - 1
            txtServerVer.Text = ""
            sCmdsFlag = "AddServerType"
        End If
    End Sub

    ''从已有的列表项目添加
    'Private Sub InitArrListItem()
    '    Dim Item As ListItem
    '    ArrListItem.Clear()
    '    Dim I As Integer
    '    For I = 0 To lstExistsType.Items.Count - 1
    '        ArrListItem.Add(lstExistsType.Items(I))
    '    Next
    '    '重新初始化列表项目
    '    lstExistsType.Items.Clear()
    '    For I = 0 To ArrListItem.Count - 1
    '        Item = CType(ArrListItem(I), ListItem)
    '        lstExistsType.Items.Add(Item.ItemText)
    '    Next
    'End Sub

    Private Sub btnTypeRemove_Click(sender As Object, e As EventArgs) Handles btnTypeRemove.Click
        If lstExistsType.SelectedIndex <> - 1 Then
            If _
                MsgBox("确定在配置文件中取消该类型支持吗？" + vbCrLf + "如果选择确定，那么将删除此项和此项下面的所有的节点！", MsgBoxStyle.OkCancel) =
                MsgBoxResult.Ok Then
                Dim index As Integer = lstExistsType.SelectedIndex

                If btnTypeAdd.Enabled Then ArrUpdateServerType.Clear()
                ArrUpdateServerType.Add(ArrListItem(index)) '记录被删除掉的数据

                lstExistsType.Items.RemoveAt(index)
                ArrListItem.RemoveAt(index)
                txtServerVer.Text = ""
                sCmdsFlag = "DelServerType"
                btnTypeAdd.Enabled = False
                btnOK.Enabled = True
            End If
        End If
    End Sub

    Private Sub txtServerVer_TextChanged(sender As Object, e As EventArgs) Handles txtServerVer.TextChanged
        Dim index As Integer = lstExistsType.SelectedIndex
        If index <> - 1 Then
            Dim Item As ListItem
            Item = CType(ArrListItem(index), ListItem)
            Item.ItemValue = txtServerVer.Text
            ArrListItem(index) = Item
            Dim selectedText As String = Item.ItemText

            For I As Integer = 0 To ArrUpdateServerType.Count - 1
                Item = CType(ArrUpdateServerType(I), ListItem)
                If Item.ItemText = selectedText Then
                    Item.ItemValue = txtServerVer.Text
                    ArrUpdateServerType(I) = Item
                    btnOK.Enabled = True
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Sub lstExistsType_SelectedIndexChanged(sender As Object, e As EventArgs) _
        Handles lstExistsType.SelectedIndexChanged
        If lstExistsType.SelectedIndex <> - 1 Then
            Dim Item As ListItem
            Item = CType(ArrListItem(lstExistsType.SelectedIndex), ListItem)
            txtServerVer.Text = Item.ItemValue
            Dim strDbType As String = Item.ItemText

            CurrDataBaseType = [Enum].Parse(GetType(DBMSType), strDbType)
            Me.lblCurrDBType.Text = "当前数据库类型：" & strDbType

        End If
    End Sub

    Private Sub lstExistsType_DoubleClick(sender As Object, e As EventArgs) Handles lstExistsType.DoubleClick
        If lstExistsType.SelectedIndex <> - 1 Then
            Me.TabConfiger.SelectedTab = Me.tabCmdClass
            Dim strDbType As String = CType(ArrListItem(lstExistsType.SelectedIndex), ListItem).ItemText

            CurrDataBaseType = [Enum].Parse(GetType(DBMSType), strDbType)
            Me.lblCurrDBType.Text = "当前数据库类型：" & strDbType
        End If
    End Sub


    Private Sub frmNewOpt_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If CurrConfigFile <> "" Then TabConfiger.SelectedTab = tabAddServerType
        'InitArrListItem()
        lbCurrentFile.Text = "当前文件：" & CurrConfigFile
        lstSuportType.DataSource = [Enum].GetNames(GetType(DBMSType))
    End Sub

    Private Sub TabConfiger_SelectedIndexChanged(sender As Object, e As EventArgs) _
        Handles TabConfiger.SelectedIndexChanged
        Debug.WriteLine("TabConfiger_SelectedIndexChanged")
        Select Case TabConfiger.SelectedIndex
            Case 0

            Case 1
                GetExistsScriptType(Me.CurrConfigFile, ArrListItem, Me.lstExistsType)
                'InitArrListItem()
            Case 2
                If lstExistsType.SelectedIndex <> - 1 Then
                    GetClassNameList(Me.CurrConfigFile,
                                     CType(ArrListItem(lstExistsType.SelectedIndex), ListItem).ItemText,
                                     Me.lstExistsCmdGroup)
                    ClearCmdsProperty()
                End If
            Case 3
                lblCurrDbType1.Text = "当前数据库类型：" & CurrDataBaseType.ToString()
                'Select Case CurrDataBaseType
                '    Case PWMIS.Common.DBMSType.Access
                '        CurrDataBase.ConnectionString = ConfigurationSettings.AppSettings("OleDbConnectionString")
                '        'Case PWMIS.Common.DBMSType..Odbc
                '        '    CurrDataBase.ConnectionString = ConfigurationSettings.AppSettings("OdbcConnectionString")
                '        'Case PWMIS.Common.DBMSType..OleDb
                '        '    CurrDataBase.ConnectionString = ConfigurationSettings.AppSettings("OleDbConnectionString")
                '    Case PWMIS.Common.DBMSType.Oracle
                '        CurrDataBase.ConnectionString = ConfigurationSettings.AppSettings("OracleConnectionString")
                '    Case PWMIS.Common.DBMSType.SqlServer
                '        CurrDataBase.ConnectionString = ConfigurationSettings.AppSettings("SqlServerConnectionString")
                '    Case Else
                '        CurrDataBase.ConnectionString = ""
                'End Select
                'CurrDataBase=Nothing?
                'CurrDataBase.ConnectionString = GetConnectionConfig(Me.CurrConfigFile, CurrDataBaseType.ToString())

                txtConnString.Text = GetConnectionConfig(Me.CurrConfigFile, CurrDataBaseType.ToString())
        End Select
    End Sub

    Private Sub lstExistsCmdGroup_SelectedIndexChanged(sender As Object, e As EventArgs) _
        Handles lstExistsCmdGroup.SelectedIndexChanged
        If lstExistsCmdGroup.SelectedIndex = - 1 Then Exit Sub
        If lstExistsType.SelectedIndex = - 1 Then Exit Sub
        Dim node As XmlNode
        node = GetCommandClassNode(Me.CurrConfigFile, CType(ArrListItem(lstExistsType.SelectedIndex), ListItem).ItemText,
                                   lstExistsCmdGroup.SelectedItem)
        txtCmdsName.Text = node.Attributes("Name").Value
        If Not node.Attributes("Class") Is Nothing Then
            txtCmdsClass.Text = node.Attributes("Class").Value
        Else
            txtCmdsClass.Text = ""
        End If
        If Not node.Attributes("Description") Is Nothing Then
            txtCmdsDesc.Text = node.Attributes("Description").Value
        Else
            txtCmdsDesc.Text = ""
        End If
        If Not node.Attributes("Interface") Is Nothing Then
            txtCmdsInterface.Text = node.Attributes("Interface").Value
        Else
            txtCmdsInterface.Text = ""
        End If
    End Sub

    Private Sub btnCmdsAdd_Click(sender As Object, e As EventArgs) Handles btnCmdsAdd.Click
        If Me.txtCmdsName.Text = "" Then Exit Sub
        lstExistsCmdGroup.SelectedIndex = - 1
        btnCmdsEdit.Enabled = False
        btnCmdsDel.Enabled = False
        btnOK.Enabled = True
        sCmdsFlag = "Added"
        '等待确认后增加命令组
        MessageBox.Show("输入要添加的命令组信息后请按[确定]按钮！", "配置文件管理", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    '确认增加命令组
    Private Sub DoAddCommandGroup()
        If Me.txtCmdsName.Text = "" Then Exit Sub
        If lstExistsCmdGroup.Items.Contains(Me.txtCmdsName.Text) Then
            MsgBox("命令组名称已经存在，请用其他的名称！", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        If lstExistsType.SelectedIndex = - 1 Then
            MsgBox("请先选择数据库类型！", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        If _
            AddCommandClass(Me.CurrConfigFile, CType(ArrListItem(lstExistsType.SelectedIndex), ListItem).ItemText,
                            txtCmdsName.Text, txtCmdsClass.Text, txtCmdsDesc.Text, txtCmdsInterface.Text) Then
            MsgBox("增加成功！")
            lstExistsCmdGroup.Items.Add(Me.txtCmdsName.Text)
        End If
        btnCmdsEdit.Enabled = True
        btnCmdsDel.Enabled = True
    End Sub

    Private Sub DoEditCommandGroup()
        If Me.txtCmdsName.Text = "" Then Exit Sub
        If lstExistsType.SelectedIndex = - 1 Then
            MsgBox("请先选择数据库类型！", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        Dim OldName As String = lstExistsCmdGroup.SelectedItem
        If EditCommandClass(Me.CurrConfigFile, CType(ArrListItem(lstExistsType.SelectedIndex), ListItem).ItemText,
                            OldName, Me.txtCmdsName.Text, Me.txtCmdsClass.Text, Me.txtCmdsDesc.Text,
                            Me.txtCmdsInterface.Text) Then
            MsgBox("修改成功！")
            lstExistsCmdGroup.Items.Remove(OldName)
            lstExistsCmdGroup.Items.Add(Me.txtCmdsName.Text)
        End If
        btnCmdsAdd.Enabled = True
        btnCmdsDel.Enabled = True
    End Sub

    Private Sub DoDelCommandGroup()
        If Me.txtCmdsName.Text = "" Then Exit Sub
        If _
            DeleteCommandClass(Me.CurrConfigFile, CType(ArrListItem(lstExistsType.SelectedIndex), ListItem).ItemText,
                               Me.txtCmdsName.Text) Then
            MsgBox("删除成功！")
            lstExistsCmdGroup.Items.Remove(Me.txtCmdsName.Text)
            ClearCmdsProperty()
        End If
        btnCmdsAdd.Enabled = True
        btnCmdsEdit.Enabled = True
    End Sub

    Private Sub DoEditConntctionString()
        If SetConnectionConfig(Me.CurrConfigFile, CurrDataBaseType.ToString(), txtConnString.Text.Trim()) Then
            MessageBox.Show("修改成功！")
            CurrDataBase = MyDB.GetDBHelper(CurrDataBaseType, Me.txtConnString.Text.Trim())
        Else
            MessageBox.Show("修改失败。")
        End If
    End Sub

    Private Sub DoCreateConfigFile()
        Dim filepath As String = Me.txtFilePath.Text
        If Not Directory.Exists(filepath) Then
            MsgBox("目录 " & filepath & " 不存在！", MsgBoxStyle.Critical, "新建配置文件")
            Exit Sub
        End If
        If Not filepath.EndsWith("\") Then
            filepath &= "\"
        End If
        Dim FullFileName As String = filepath & Me.txtFileName.Text
        If CreateConfigFile(FullFileName) Then
            MsgBox("配置文件 " & Me.txtFileName.Text & " 创建成功！")
            CurrConfigFile = FullFileName
            lbCurrentFile.Text = "当前文件：" & CurrConfigFile
        End If
    End Sub

    Private Sub DoAddServerType()
        Dim filepath As String = Me.CurrConfigFile
        Dim Item As ListItem
        For I As Integer = 0 To ArrUpdateServerType.Count - 1
            Item = CType(ArrUpdateServerType(I), ListItem)
            If Not AddScriptType(filepath, Item.ItemText, Item.ItemValue) Then
                MsgBox("添加数据库类型失败", MsgBoxStyle.Critical)
                Exit Sub
            End If
        Next
        MsgBox("操作成功！", MsgBoxStyle.ApplicationModal, "添加数据库类型")
        btnTypeRemove.Enabled = True
    End Sub

    Private Sub DoDelServerType()
        If MsgBox("执行此操作数据将不可恢复，是否继续？", MsgBoxStyle.OkCancel, "警告") = MsgBoxResult.Ok Then
            Dim filepath As String = Me.CurrConfigFile
            Dim Item As ListItem
            For I As Integer = 0 To ArrUpdateServerType.Count - 1
                Item = CType(ArrUpdateServerType(I), ListItem)
                If Not DeleteScriptType(filepath, Item.ItemText) Then
                    MsgBox("删除数据库类型失败", MsgBoxStyle.Critical)
                    Exit Sub
                End If
            Next
        End If
        MsgBox("操作成功！", MsgBoxStyle.ApplicationModal, "删除数据库类型")
        btnTypeAdd.Enabled = True
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        Select Case sCmdsFlag
            Case "Added"
                DoAddCommandGroup()
            Case "Edit"
                DoEditCommandGroup()
            Case "Delete"
                DoDelCommandGroup()
            Case "CreateFile"
                DoCreateConfigFile()
            Case "AddServerType"
                DoAddServerType()
            Case "DelServerType"
                DoDelServerType()
            Case "EditConnection"
                DoEditConntctionString()
            Case Else
                Me.Close()
        End Select
    End Sub


    Private Sub btnCmdsEdit_Click(sender As Object, e As EventArgs) Handles btnCmdsEdit.Click
        If Me.txtCmdsName.Text = "" Or lstExistsCmdGroup.SelectedIndex = - 1 Then Exit Sub
        btnCmdsAdd.Enabled = False
        btnCmdsDel.Enabled = False
        btnOK.Enabled = True
        sCmdsFlag = "Edit"
        '等待确认后修改命令组
        MessageBox.Show("修改命令组信息后请按[确定]按钮！", "配置文件管理", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnCmdsDel_Click(sender As Object, e As EventArgs) Handles btnCmdsDel.Click
        If Me.txtCmdsName.Text = "" Or lstExistsCmdGroup.SelectedIndex = - 1 Then Exit Sub
        btnCmdsAdd.Enabled = False
        btnCmdsEdit.Enabled = False
        btnOK.Enabled = True
        sCmdsFlag = "Delete"
        MessageBox.Show("确定操作请按[确定]按钮！", "配置文件管理", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub lstSuportType_DoubleClick(sender As Object, e As EventArgs) Handles lstSuportType.DoubleClick
        btnOK.Enabled = True
        AddServerType()
    End Sub

    Private Sub ClearCmdsProperty()
        Me.txtCmdsName.Text = ""
        Me.txtCmdsClass.Text = ""
        Me.txtCmdsDesc.Text = ""
    End Sub

    Private Sub CheckCreateFile()
        If txtFilePath.Text.Trim <> "" And txtFileName.Text <> "" AndAlso txtFileName.Text.ToLower <> "*.config" Then
            btnOK.Enabled = True
            sCmdsFlag = "CreateFile"
        End If
    End Sub

    Private Sub txtFilePath_LostFocus(sender As Object, e As EventArgs) Handles txtFilePath.LostFocus
        CheckCreateFile()
    End Sub

    Private Sub txtFileName_TextChanged(sender As Object, e As EventArgs) Handles txtFileName.TextChanged
        CheckCreateFile()
    End Sub

    Private Sub txtConnString_LostFocus(sender As Object, e As EventArgs) Handles txtConnString.LostFocus
        'CurrDataBase.ConnectionString = txtConnString.Text.Trim()
    End Sub

    Private Sub btnConnTest_Click(sender As Object, e As EventArgs) Handles btnConnTest.Click
        Try
            Dim adoHelper As AdoHelper = MyDB.GetDBHelper(CurrDataBaseType, txtConnString.Text.Trim())
            Dim conn As IDbConnection = adoHelper.GetConnection(txtConnString.Text.Trim())
            conn.Open()
            If conn.State = ConnectionState.Open Then
                conn.Close()
                MsgBox("测试成功！保存并使用此连接请再次单击[确定]按钮。", MsgBoxStyle.ApplicationModal, "数据连接")
                btnOK.Enabled = True
                sCmdsFlag = "EditConnection"
            End If
        Catch ex As Exception
            MsgBox("连接失败！" & ex.Message, MsgBoxStyle.Critical, "连接测试")
        End Try
    End Sub
End Class
