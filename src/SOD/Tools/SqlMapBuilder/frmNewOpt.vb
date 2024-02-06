Imports System.Xml
Imports PWMIS.DataMap.SqlMap
Imports System.Configuration

Public Class frmNewOpt
    Inherits System.Windows.Forms.Form

    Dim ArrListItem As New ArrayList
    Dim ArrUpdateServerType As New ArrayList '��¼���½ű����Ϳ죨��ӣ�ɾ����
    Public CurrConfigFile As String = String.Empty
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txtCmdsInterface As System.Windows.Forms.TextBox
    Dim sCmdsFlag As String

#Region " Windows ������������ɵĴ��� "

    Public Sub New()
        MyBase.New()

        '�õ����� Windows ���������������ġ�
        InitializeComponent()

        '�� InitializeComponent() ����֮������κγ�ʼ��

    End Sub

    '������д dispose ����������б�
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Windows ����������������
    Private components As System.ComponentModel.IContainer

    'ע��: ���¹����� Windows ����������������
    '����ʹ�� Windows ����������޸Ĵ˹��̡�
    '��Ҫʹ�ô���༭���޸�����
    Friend WithEvents tabNewFile As System.Windows.Forms.TabPage
    Friend WithEvents tabAddServerType As System.Windows.Forms.TabPage
    Friend WithEvents tabCmdClass As System.Windows.Forms.TabPage
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancle As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtFilePath As System.Windows.Forms.TextBox
    Friend WithEvents btnBrows As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtFileName As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lstSuportType As System.Windows.Forms.ListBox
    Friend WithEvents lstExistsType As System.Windows.Forms.ListBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lbCurrentFile As System.Windows.Forms.Label
    Friend WithEvents btnTypeAdd As System.Windows.Forms.Button
    Friend WithEvents txtServerVer As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents lstExistsCmdGroup As System.Windows.Forms.ListBox
    Friend WithEvents btnCmdsDel As System.Windows.Forms.Button
    Friend WithEvents btnCmdsAdd As System.Windows.Forms.Button
    Friend WithEvents btnCmdsEdit As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtCmdsName As System.Windows.Forms.TextBox
    Friend WithEvents txtCmdsClass As System.Windows.Forms.TextBox
    Friend WithEvents txtCmdsDesc As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents lblCurrDBType As System.Windows.Forms.Label
    Friend WithEvents TabConfiger As System.Windows.Forms.TabControl
    Friend WithEvents btnTypeRemove As System.Windows.Forms.Button
    Friend WithEvents tabDbConn As System.Windows.Forms.TabPage
    Friend WithEvents lblCurrDbType1 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtConnString As System.Windows.Forms.TextBox
    Friend WithEvents btnConnTest As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.TabConfiger = New System.Windows.Forms.TabControl
        Me.tabNewFile = New System.Windows.Forms.TabPage
        Me.txtFileName = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnBrows = New System.Windows.Forms.Button
        Me.txtFilePath = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.tabAddServerType = New System.Windows.Forms.TabPage
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtServerVer = New System.Windows.Forms.TextBox
        Me.btnTypeRemove = New System.Windows.Forms.Button
        Me.btnTypeAdd = New System.Windows.Forms.Button
        Me.lbCurrentFile = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.lstExistsType = New System.Windows.Forms.ListBox
        Me.lstSuportType = New System.Windows.Forms.ListBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.tabCmdClass = New System.Windows.Forms.TabPage
        Me.lblCurrDBType = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.txtCmdsDesc = New System.Windows.Forms.TextBox
        Me.txtCmdsClass = New System.Windows.Forms.TextBox
        Me.txtCmdsName = New System.Windows.Forms.TextBox
        Me.btnCmdsEdit = New System.Windows.Forms.Button
        Me.btnCmdsAdd = New System.Windows.Forms.Button
        Me.btnCmdsDel = New System.Windows.Forms.Button
        Me.lstExistsCmdGroup = New System.Windows.Forms.ListBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.tabDbConn = New System.Windows.Forms.TabPage
        Me.btnConnTest = New System.Windows.Forms.Button
        Me.txtConnString = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.lblCurrDbType1 = New System.Windows.Forms.Label
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancle = New System.Windows.Forms.Button
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog
        Me.Label14 = New System.Windows.Forms.Label
        Me.txtCmdsInterface = New System.Windows.Forms.TextBox
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
        Me.TabConfiger.Location = New System.Drawing.Point(10, 10)
        Me.TabConfiger.Name = "TabConfiger"
        Me.TabConfiger.SelectedIndex = 0
        Me.TabConfiger.Size = New System.Drawing.Size(376, 242)
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
        Me.tabNewFile.Location = New System.Drawing.Point(4, 22)
        Me.tabNewFile.Name = "tabNewFile"
        Me.tabNewFile.Size = New System.Drawing.Size(368, 216)
        Me.tabNewFile.TabIndex = 0
        Me.tabNewFile.Text = "�½��ļ�"
        '
        'txtFileName
        '
        Me.txtFileName.Location = New System.Drawing.Point(8, 128)
        Me.txtFileName.Name = "txtFileName"
        Me.txtFileName.Size = New System.Drawing.Size(136, 21)
        Me.txtFileName.TabIndex = 3
        Me.txtFileName.Text = "*.config"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(8, 104)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(152, 23)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "������Ҫ������ļ�����"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(8, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(168, 23)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "������Ҫ������ļ�·����"
        '
        'btnBrows
        '
        Me.btnBrows.Location = New System.Drawing.Point(279, 72)
        Me.btnBrows.Name = "btnBrows"
        Me.btnBrows.Size = New System.Drawing.Size(75, 23)
        Me.btnBrows.TabIndex = 2
        Me.btnBrows.Text = "(&B)���"
        '
        'txtFilePath
        '
        Me.txtFilePath.Location = New System.Drawing.Point(8, 72)
        Me.txtFilePath.Name = "txtFilePath"
        Me.txtFilePath.Size = New System.Drawing.Size(265, 21)
        Me.txtFilePath.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(312, 23)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "�½�һ��SqlMap�����ļ���"
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
        Me.tabAddServerType.Location = New System.Drawing.Point(4, 22)
        Me.tabAddServerType.Name = "tabAddServerType"
        Me.tabAddServerType.Size = New System.Drawing.Size(368, 216)
        Me.tabAddServerType.TabIndex = 1
        Me.tabAddServerType.Text = "���ݿ�����"
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(160, 186)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(48, 16)
        Me.Label7.TabIndex = 9
        Me.Label7.Text = "�汾��"
        '
        'txtServerVer
        '
        Me.txtServerVer.Location = New System.Drawing.Point(219, 178)
        Me.txtServerVer.Name = "txtServerVer"
        Me.txtServerVer.Size = New System.Drawing.Size(120, 21)
        Me.txtServerVer.TabIndex = 6
        '
        'btnTypeRemove
        '
        Me.btnTypeRemove.Location = New System.Drawing.Point(155, 146)
        Me.btnTypeRemove.Name = "btnTypeRemove"
        Me.btnTypeRemove.Size = New System.Drawing.Size(48, 23)
        Me.btnTypeRemove.TabIndex = 4
        Me.btnTypeRemove.Text = "<<"
        '
        'btnTypeAdd
        '
        Me.btnTypeAdd.Location = New System.Drawing.Point(155, 106)
        Me.btnTypeAdd.Name = "btnTypeAdd"
        Me.btnTypeAdd.Size = New System.Drawing.Size(48, 23)
        Me.btnTypeAdd.TabIndex = 2
        Me.btnTypeAdd.Text = ">>"
        '
        'lbCurrentFile
        '
        Me.lbCurrentFile.Location = New System.Drawing.Point(8, 32)
        Me.lbCurrentFile.Name = "lbCurrentFile"
        Me.lbCurrentFile.Size = New System.Drawing.Size(357, 42)
        Me.lbCurrentFile.TabIndex = 5
        Me.lbCurrentFile.Text = "��ǰ�ļ���"
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(219, 74)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(128, 23)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "���е����ݿ����ͣ�"
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(8, 74)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(120, 23)
        Me.Label5.TabIndex = 3
        Me.Label5.Text = "֧�ֵ����ݿ����ͣ�"
        '
        'lstExistsType
        '
        Me.lstExistsType.ItemHeight = 12
        Me.lstExistsType.Location = New System.Drawing.Point(219, 98)
        Me.lstExistsType.Name = "lstExistsType"
        Me.lstExistsType.Size = New System.Drawing.Size(120, 76)
        Me.lstExistsType.TabIndex = 5
        '
        'lstSuportType
        '
        Me.lstSuportType.ItemHeight = 12
        Me.lstSuportType.Location = New System.Drawing.Point(8, 98)
        Me.lstSuportType.Name = "lstSuportType"
        Me.lstSuportType.Size = New System.Drawing.Size(120, 100)
        Me.lstSuportType.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(8, 8)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(312, 23)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "���������ļ��ж���ĵ�ǰ��֧�ֵ����ݿ����͡�"
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
        Me.tabCmdClass.Location = New System.Drawing.Point(4, 22)
        Me.tabCmdClass.Name = "tabCmdClass"
        Me.tabCmdClass.Size = New System.Drawing.Size(368, 216)
        Me.tabCmdClass.TabIndex = 2
        Me.tabCmdClass.Text = "������"
        '
        'lblCurrDBType
        '
        Me.lblCurrDBType.Location = New System.Drawing.Point(8, 32)
        Me.lblCurrDBType.Name = "lblCurrDBType"
        Me.lblCurrDBType.Size = New System.Drawing.Size(357, 23)
        Me.lblCurrDBType.TabIndex = 8
        Me.lblCurrDBType.Text = "��ǰ���ݿ����ͣ�"
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
        Me.GroupBox1.Location = New System.Drawing.Point(199, 56)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(166, 148)
        Me.GroupBox1.TabIndex = 7
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "����������"
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(8, 114)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(32, 23)
        Me.Label12.TabIndex = 10
        Me.Label12.Text = "����"
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(8, 54)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(32, 23)
        Me.Label11.TabIndex = 9
        Me.Label11.Text = "����"
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(8, 24)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(32, 23)
        Me.Label10.TabIndex = 8
        Me.Label10.Text = "����"
        '
        'txtCmdsDesc
        '
        Me.txtCmdsDesc.Location = New System.Drawing.Point(45, 114)
        Me.txtCmdsDesc.Name = "txtCmdsDesc"
        Me.txtCmdsDesc.Size = New System.Drawing.Size(115, 21)
        Me.txtCmdsDesc.TabIndex = 9
        '
        'txtCmdsClass
        '
        Me.txtCmdsClass.Location = New System.Drawing.Point(45, 54)
        Me.txtCmdsClass.Name = "txtCmdsClass"
        Me.txtCmdsClass.Size = New System.Drawing.Size(115, 21)
        Me.txtCmdsClass.TabIndex = 8
        '
        'txtCmdsName
        '
        Me.txtCmdsName.Location = New System.Drawing.Point(45, 24)
        Me.txtCmdsName.Name = "txtCmdsName"
        Me.txtCmdsName.Size = New System.Drawing.Size(115, 21)
        Me.txtCmdsName.TabIndex = 7
        '
        'btnCmdsEdit
        '
        Me.btnCmdsEdit.Location = New System.Drawing.Point(145, 112)
        Me.btnCmdsEdit.Name = "btnCmdsEdit"
        Me.btnCmdsEdit.Size = New System.Drawing.Size(48, 23)
        Me.btnCmdsEdit.TabIndex = 5
        Me.btnCmdsEdit.Text = "�޸�"
        '
        'btnCmdsAdd
        '
        Me.btnCmdsAdd.Location = New System.Drawing.Point(145, 78)
        Me.btnCmdsAdd.Name = "btnCmdsAdd"
        Me.btnCmdsAdd.Size = New System.Drawing.Size(48, 23)
        Me.btnCmdsAdd.TabIndex = 4
        Me.btnCmdsAdd.Text = "���"
        '
        'btnCmdsDel
        '
        Me.btnCmdsDel.Location = New System.Drawing.Point(145, 142)
        Me.btnCmdsDel.Name = "btnCmdsDel"
        Me.btnCmdsDel.Size = New System.Drawing.Size(48, 23)
        Me.btnCmdsDel.TabIndex = 6
        Me.btnCmdsDel.Text = "ɾ��"
        '
        'lstExistsCmdGroup
        '
        Me.lstExistsCmdGroup.ItemHeight = 12
        Me.lstExistsCmdGroup.Location = New System.Drawing.Point(8, 80)
        Me.lstExistsCmdGroup.Name = "lstExistsCmdGroup"
        Me.lstExistsCmdGroup.Size = New System.Drawing.Size(131, 124)
        Me.lstExistsCmdGroup.TabIndex = 2
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(8, 56)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(100, 23)
        Me.Label9.TabIndex = 1
        Me.Label9.Text = "���е������飺"
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(8, 8)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(304, 23)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "���������飬����ӳ�䵽���ݷ��ʲ���ࡣ"
        '
        'tabDbConn
        '
        Me.tabDbConn.Controls.Add(Me.btnConnTest)
        Me.tabDbConn.Controls.Add(Me.txtConnString)
        Me.tabDbConn.Controls.Add(Me.Label13)
        Me.tabDbConn.Controls.Add(Me.lblCurrDbType1)
        Me.tabDbConn.Location = New System.Drawing.Point(4, 22)
        Me.tabDbConn.Name = "tabDbConn"
        Me.tabDbConn.Size = New System.Drawing.Size(368, 216)
        Me.tabDbConn.TabIndex = 3
        Me.tabDbConn.Text = "��������"
        '
        'btnConnTest
        '
        Me.btnConnTest.Location = New System.Drawing.Point(16, 104)
        Me.btnConnTest.Name = "btnConnTest"
        Me.btnConnTest.Size = New System.Drawing.Size(75, 23)
        Me.btnConnTest.TabIndex = 12
        Me.btnConnTest.Text = "��������"
        '
        'txtConnString
        '
        Me.txtConnString.Location = New System.Drawing.Point(16, 72)
        Me.txtConnString.Name = "txtConnString"
        Me.txtConnString.Size = New System.Drawing.Size(349, 21)
        Me.txtConnString.TabIndex = 11
        '
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(16, 48)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(120, 23)
        Me.Label13.TabIndex = 10
        Me.Label13.Text = "���ݿ������ַ�����"
        '
        'lblCurrDbType1
        '
        Me.lblCurrDbType1.Location = New System.Drawing.Point(16, 16)
        Me.lblCurrDbType1.Name = "lblCurrDbType1"
        Me.lblCurrDbType1.Size = New System.Drawing.Size(349, 23)
        Me.lblCurrDbType1.TabIndex = 9
        Me.lblCurrDbType1.Text = "��ǰ���ݿ����ͣ�"
        '
        'btnOK
        '
        Me.btnOK.Enabled = False
        Me.btnOK.Location = New System.Drawing.Point(184, 258)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 1
        Me.btnOK.Text = "(&Y)ȷ��"
        '
        'btnCancle
        '
        Me.btnCancle.Location = New System.Drawing.Point(272, 258)
        Me.btnCancle.Name = "btnCancle"
        Me.btnCancle.Size = New System.Drawing.Size(75, 23)
        Me.btnCancle.TabIndex = 2
        Me.btnCancle.Text = "(&C)ȡ��"
        '
        'Label14
        '
        Me.Label14.Location = New System.Drawing.Point(9, 84)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(32, 23)
        Me.Label14.TabIndex = 11
        Me.Label14.Text = "�ӿ�"
        '
        'txtCmdsInterface
        '
        Me.txtCmdsInterface.Location = New System.Drawing.Point(45, 84)
        Me.txtCmdsInterface.Name = "txtCmdsInterface"
        Me.txtCmdsInterface.Size = New System.Drawing.Size(115, 21)
        Me.txtCmdsInterface.TabIndex = 12
        '
        'frmNewOpt
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.ClientSize = New System.Drawing.Size(406, 291)
        Me.Controls.Add(Me.btnCancle)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.TabConfiger)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmNewOpt"
        Me.Text = "�����ļ�����"
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


    Private Sub btnCancle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancle.Click
        Me.Close()
    End Sub

    Private Sub btnBrows_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrows.Click
        Me.FolderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer
        Me.FolderBrowserDialog1.ShowDialog()
        txtFilePath.Text = Me.FolderBrowserDialog1.SelectedPath
        CheckCreateFile()
    End Sub

    Private Sub btnTypeAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTypeAdd.Click
        If btnTypeRemove.Enabled Then ArrUpdateServerType.Clear()
        AddServerType()
        sCmdsFlag = "AddServerType"
        btnOK.Enabled = True
        btnTypeRemove.Enabled = False

    End Sub

    Private Sub AddServerType()
        If lstSuportType.SelectedIndex <> -1 Then
            'lstSuportType.Items (0)
            Dim Item As ListItem
            Dim text As String = lstSuportType.SelectedItem
            For I As Integer = 0 To ArrListItem.Count - 1
                Item = CType(ArrListItem(I), ListItem)
                If Item.ItemText = text Then
                    MessageBox.Show("�Ѿ����ڽű����� " & text)
                    Exit Sub
                End If
            Next
            Item = New ListItem()
            Item.ItemText = text
            Item.ItemValue = ""

            ArrListItem.Add(Item)
            ArrUpdateServerType.Add(Item)

            lstExistsType.Items.Add(text)
            lstExistsType.SelectedIndex = -1
            lstSuportType.SelectedIndex = -1
            txtServerVer.Text = ""
            sCmdsFlag = "AddServerType"
        End If
    End Sub

    ''�����е��б���Ŀ���
    'Private Sub InitArrListItem()
    '    Dim Item As ListItem
    '    ArrListItem.Clear()
    '    Dim I As Integer
    '    For I = 0 To lstExistsType.Items.Count - 1
    '        ArrListItem.Add(lstExistsType.Items(I))
    '    Next
    '    '���³�ʼ���б���Ŀ
    '    lstExistsType.Items.Clear()
    '    For I = 0 To ArrListItem.Count - 1
    '        Item = CType(ArrListItem(I), ListItem)
    '        lstExistsType.Items.Add(Item.ItemText)
    '    Next
    'End Sub

    Private Sub btnTypeRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTypeRemove.Click
        If lstExistsType.SelectedIndex <> -1 Then
            If MsgBox("ȷ���������ļ���ȡ��������֧����" + vbCrLf + "���ѡ��ȷ������ô��ɾ������ʹ�����������еĽڵ㣡", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                Dim index As Integer = lstExistsType.SelectedIndex

                If btnTypeAdd.Enabled Then ArrUpdateServerType.Clear()
                ArrUpdateServerType.Add(ArrListItem(index)) '��¼��ɾ����������

                lstExistsType.Items.RemoveAt(index)
                ArrListItem.RemoveAt(index)
                txtServerVer.Text = ""
                sCmdsFlag = "DelServerType"
                btnTypeAdd.Enabled = False
                btnOK.Enabled = True
            End If
        End If
    End Sub

    Private Sub txtServerVer_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtServerVer.TextChanged
        Dim index As Integer = lstExistsType.SelectedIndex
        If index <> -1 Then
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

    Private Sub lstExistsType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstExistsType.SelectedIndexChanged
        If lstExistsType.SelectedIndex <> -1 Then
            Dim Item As ListItem
            Item = CType(ArrListItem(lstExistsType.SelectedIndex), ListItem)
            txtServerVer.Text = Item.ItemValue
            Dim strDbType As String = Item.ItemText

            CurrDataBaseType = [Enum].Parse(GetType(PWMIS.Common.DBMSType), strDbType)
            Me.lblCurrDBType.Text = "��ǰ���ݿ����ͣ�" & strDbType

        End If
    End Sub

    Private Sub lstExistsType_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstExistsType.DoubleClick
        If lstExistsType.SelectedIndex <> -1 Then
            Me.TabConfiger.SelectedTab = Me.tabCmdClass
            Dim strDbType As String = CType(ArrListItem(lstExistsType.SelectedIndex), ListItem).ItemText

            CurrDataBaseType = [Enum].Parse(GetType(PWMIS.Common.DBMSType), strDbType)
            Me.lblCurrDBType.Text = "��ǰ���ݿ����ͣ�" & strDbType
        End If
    End Sub


    Private Sub frmNewOpt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If CurrConfigFile <> "" Then TabConfiger.SelectedTab = tabAddServerType
        'InitArrListItem()
        lbCurrentFile.Text = "��ǰ�ļ���" & CurrConfigFile
        lstSuportType.DataSource = System.Enum.GetNames(GetType(PWMIS.Common.DBMSType))
    End Sub

    Private Sub TabConfiger_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabConfiger.SelectedIndexChanged
        Debug.WriteLine("TabConfiger_SelectedIndexChanged")
        Select Case TabConfiger.SelectedIndex
            Case 0

            Case 1
                GetExistsScriptType(Me.CurrConfigFile, ArrListItem, Me.lstExistsType)
                'InitArrListItem()
            Case 2
                If lstExistsType.SelectedIndex <> -1 Then
                    GetClassNameList(Me.CurrConfigFile, CType(ArrListItem(lstExistsType.SelectedIndex), ListItem).ItemText, Me.lstExistsCmdGroup)
                    ClearCmdsProperty()
                End If
            Case 3
                lblCurrDbType1.Text = "��ǰ���ݿ����ͣ�" & CurrDataBaseType.ToString()
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

    Private Sub lstExistsCmdGroup_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstExistsCmdGroup.SelectedIndexChanged
        If lstExistsCmdGroup.SelectedIndex = -1 Then Exit Sub
        If lstExistsType.SelectedIndex = -1 Then Exit Sub
        Dim node As XmlNode
        node = GetCommandClassNode(Me.CurrConfigFile, CType(ArrListItem(lstExistsType.SelectedIndex), ListItem).ItemText, lstExistsCmdGroup.SelectedItem)
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

    Private Sub btnCmdsAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCmdsAdd.Click
        If Me.txtCmdsName.Text = "" Then Exit Sub
        lstExistsCmdGroup.SelectedIndex = -1
        btnCmdsEdit.Enabled = False
        btnCmdsDel.Enabled = False
        btnOK.Enabled = True
        sCmdsFlag = "Added"
        '�ȴ�ȷ�Ϻ�����������
        MessageBox.Show("����Ҫ��ӵ���������Ϣ���밴[ȷ��]��ť��", "�����ļ�����", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    'ȷ������������
    Private Sub DoAddCommandGroup()
        If Me.txtCmdsName.Text = "" Then Exit Sub
        If lstExistsCmdGroup.Items.Contains(Me.txtCmdsName.Text) Then
            MsgBox("�����������Ѿ����ڣ��������������ƣ�", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        If lstExistsType.SelectedIndex = -1 Then
            MsgBox("����ѡ�����ݿ����ͣ�", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        If AddCommandClass(Me.CurrConfigFile, CType(ArrListItem(lstExistsType.SelectedIndex), ListItem).ItemText, txtCmdsName.Text, txtCmdsClass.Text, txtCmdsDesc.Text, txtCmdsInterface.Text) Then
            MsgBox("���ӳɹ���")
            lstExistsCmdGroup.Items.Add(Me.txtCmdsName.Text)
        End If
        btnCmdsEdit.Enabled = True
        btnCmdsDel.Enabled = True
    End Sub

    Private Sub DoEditCommandGroup()
        If Me.txtCmdsName.Text = "" Then Exit Sub
        If lstExistsType.SelectedIndex = -1 Then
            MsgBox("����ѡ�����ݿ����ͣ�", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        Dim OldName As String = lstExistsCmdGroup.SelectedItem
        If EditCommandClass(Me.CurrConfigFile, CType(ArrListItem(lstExistsType.SelectedIndex), ListItem).ItemText, _
                            OldName, Me.txtCmdsName.Text, Me.txtCmdsClass.Text, Me.txtCmdsDesc.Text, Me.txtCmdsInterface.Text) Then
            MsgBox("�޸ĳɹ���")
            lstExistsCmdGroup.Items.Remove(OldName)
            lstExistsCmdGroup.Items.Add(Me.txtCmdsName.Text)
        End If
        btnCmdsAdd.Enabled = True
        btnCmdsDel.Enabled = True

    End Sub

    Private Sub DoDelCommandGroup()
        If Me.txtCmdsName.Text = "" Then Exit Sub
        If DeleteCommandClass(Me.CurrConfigFile, CType(ArrListItem(lstExistsType.SelectedIndex), ListItem).ItemText, Me.txtCmdsName.Text) Then
            MsgBox("ɾ���ɹ���")
            lstExistsCmdGroup.Items.Remove(Me.txtCmdsName.Text)
            ClearCmdsProperty()
        End If
        btnCmdsAdd.Enabled = True
        btnCmdsEdit.Enabled = True
    End Sub

    Private Sub DoEditConntctionString()
        If SetConnectionConfig(Me.CurrConfigFile, CurrDataBaseType.ToString(), txtConnString.Text.Trim()) Then
            MessageBox.Show("�޸ĳɹ���")
            CurrDataBase = PWMIS.DataProvider.Adapter.MyDB.GetDBHelper(CurrDataBaseType, Me.txtConnString.Text.Trim())
        Else
            MessageBox.Show("�޸�ʧ�ܡ�")
        End If
    End Sub

    Private Sub DoCreateConfigFile()
        Dim filepath As String = Me.txtFilePath.Text
        If Not System.IO.Directory.Exists(filepath) Then
            MsgBox("Ŀ¼ " & filepath & " �����ڣ�", MsgBoxStyle.Critical, "�½������ļ�")
            Exit Sub
        End If
        If Not filepath.EndsWith("\") Then
            filepath &= "\"
        End If
        Dim FullFileName As String = filepath & Me.txtFileName.Text
        If CreateConfigFile(FullFileName) Then
            MsgBox("�����ļ� " & Me.txtFileName.Text & " �����ɹ���")
            CurrConfigFile = FullFileName
            lbCurrentFile.Text = "��ǰ�ļ���" & CurrConfigFile
        End If
    End Sub

    Private Sub DoAddServerType()
        Dim filepath As String = Me.CurrConfigFile
        Dim Item As ListItem
        For I As Integer = 0 To ArrUpdateServerType.Count - 1
            Item = CType(ArrUpdateServerType(I), ListItem)
            If Not AddScriptType(filepath, Item.ItemText, Item.ItemValue) Then
                MsgBox("������ݿ�����ʧ��", MsgBoxStyle.Critical)
                Exit Sub
            End If
        Next
        MsgBox("�����ɹ���", MsgBoxStyle.ApplicationModal, "������ݿ�����")
        btnTypeRemove.Enabled = True
    End Sub

    Private Sub DoDelServerType()
        If MsgBox("ִ�д˲������ݽ����ɻָ����Ƿ������", MsgBoxStyle.OkCancel, "����") = MsgBoxResult.Ok Then
            Dim filepath As String = Me.CurrConfigFile
            Dim Item As ListItem
            For I As Integer = 0 To ArrUpdateServerType.Count - 1
                Item = CType(ArrUpdateServerType(I), ListItem)
                If Not DeleteScriptType(filepath, Item.ItemText) Then
                    MsgBox("ɾ�����ݿ�����ʧ��", MsgBoxStyle.Critical)
                    Exit Sub
                End If
            Next
        End If
        MsgBox("�����ɹ���", MsgBoxStyle.ApplicationModal, "ɾ�����ݿ�����")
        btnTypeAdd.Enabled = True
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
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



    Private Sub btnCmdsEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCmdsEdit.Click
        If Me.txtCmdsName.Text = "" Or lstExistsCmdGroup.SelectedIndex = -1 Then Exit Sub
        btnCmdsAdd.Enabled = False
        btnCmdsDel.Enabled = False
        btnOK.Enabled = True
        sCmdsFlag = "Edit"
        '�ȴ�ȷ�Ϻ��޸�������
        MessageBox.Show("�޸���������Ϣ���밴[ȷ��]��ť��", "�����ļ�����", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnCmdsDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCmdsDel.Click
        If Me.txtCmdsName.Text = "" Or lstExistsCmdGroup.SelectedIndex = -1 Then Exit Sub
        btnCmdsAdd.Enabled = False
        btnCmdsEdit.Enabled = False
        btnOK.Enabled = True
        sCmdsFlag = "Delete"
        MessageBox.Show("ȷ�������밴[ȷ��]��ť��", "�����ļ�����", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub lstSuportType_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstSuportType.DoubleClick
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

    Private Sub txtFilePath_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFilePath.LostFocus
        CheckCreateFile()
    End Sub

    Private Sub txtFileName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFileName.TextChanged
        CheckCreateFile()
    End Sub

    Private Sub txtConnString_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtConnString.LostFocus
        'CurrDataBase.ConnectionString = txtConnString.Text.Trim()

    End Sub

    Private Sub btnConnTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConnTest.Click
        Try
            Dim adoHelper As PWMIS.DataProvider.Data.AdoHelper = PWMIS.DataProvider.Adapter.MyDB.GetDBHelper(CurrDataBaseType, txtConnString.Text.Trim())
            Dim conn As IDbConnection = adoHelper.GetConnection(txtConnString.Text.Trim())
            conn.Open()
            If conn.State = ConnectionState.Open Then
                conn.Close()
                MsgBox("���Գɹ������沢ʹ�ô��������ٴε���[ȷ��]��ť��", MsgBoxStyle.ApplicationModal, "��������")
                btnOK.Enabled = True
                sCmdsFlag = "EditConnection"
            End If
        Catch ex As Exception
            MsgBox("����ʧ�ܣ�" & ex.Message, MsgBoxStyle.Critical, "���Ӳ���")
        End Try
    End Sub










End Class
