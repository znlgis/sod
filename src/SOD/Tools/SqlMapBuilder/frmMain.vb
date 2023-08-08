Imports System.ComponentModel
Imports System.Globalization
Imports System.IO
Imports System.Xml
Imports PWMIS.Common
Imports PWMIS.DataMap.SqlMap
Imports PWMIS.DataProvider.Adapter

Public Class Form1
    Inherits Form

    Private ReadOnly ArrSqlScript As New Hashtable
    Private IsCmdType As Boolean
    Private AddRowInex As Integer
    Private EditRowIndex As Integer
    Private LastSelectedCmdName As String
    Private LastRowIndex As Integer '上一次选择的行索引
    Private XPath As String
    Private Mapper As SqlMapper
    Friend WithEvents DataGridTableStyle1 As DataGridTableStyle
    Friend WithEvents DataGridTextBoxColumn1 As DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn2 As DataGridTextBoxColumn
    Friend WithEvents cmdSave As Button
    Private IsLoadCmdType As Boolean
    Friend WithEvents DataGridTextBoxColumn3 As DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn4 As DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn5 As DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn6 As DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn7 As DataGridTextBoxColumn
    Friend WithEvents lblHelp As Label '是否正在装载数据
    Private SqlMapScriptChanged As Boolean '脚本是否手工改变过。

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
    Friend WithEvents txtFileName As TextBox
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents btnFileOpen As Button
    Friend WithEvents cmbScriptType As ComboBox
    Friend WithEvents lstClassName As ListBox
    Friend WithEvents dgCommandInfo As DataGrid
    Friend WithEvents txtCommandText As TextBox
    Friend WithEvents DataSet1 As DataSet
    Friend WithEvents dtCommandClass As DataTable
    Friend WithEvents CommandName As DataColumn
    Friend WithEvents CommandType As DataColumn
    Friend WithEvents QueryType As DataColumn
    Friend WithEvents Method As DataColumn
    Friend WithEvents btnParseXml As Button
    Friend WithEvents Description As DataColumn
    Friend WithEvents cmbCmdType As ComboBox
    Friend WithEvents btnConfig As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtSQL As TextBox
    Friend WithEvents btnParamBuilder As Button
    Friend WithEvents btnSQLTest As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents ResultClass As DataColumn
    Friend WithEvents cmbResultClass As ComboBox
    Friend WithEvents ResultMap As DataColumn

    <DebuggerStepThrough>
    Private Sub InitializeComponent()
        Me.txtFileName = New TextBox
        Me.OpenFileDialog1 = New OpenFileDialog
        Me.btnFileOpen = New Button
        Me.cmbScriptType = New ComboBox
        Me.lstClassName = New ListBox
        Me.dgCommandInfo = New DataGrid
        Me.DataSet1 = New DataSet
        Me.dtCommandClass = New DataTable
        Me.CommandName = New DataColumn
        Me.CommandType = New DataColumn
        Me.QueryType = New DataColumn
        Me.Method = New DataColumn
        Me.Description = New DataColumn
        Me.ResultClass = New DataColumn
        Me.ResultMap = New DataColumn
        Me.DataGridTableStyle1 = New DataGridTableStyle
        Me.DataGridTextBoxColumn1 = New DataGridTextBoxColumn
        Me.DataGridTextBoxColumn2 = New DataGridTextBoxColumn
        Me.DataGridTextBoxColumn3 = New DataGridTextBoxColumn
        Me.DataGridTextBoxColumn4 = New DataGridTextBoxColumn
        Me.DataGridTextBoxColumn5 = New DataGridTextBoxColumn
        Me.DataGridTextBoxColumn6 = New DataGridTextBoxColumn
        Me.DataGridTextBoxColumn7 = New DataGridTextBoxColumn
        Me.txtCommandText = New TextBox
        Me.btnParseXml = New Button
        Me.cmbCmdType = New ComboBox
        Me.btnConfig = New Button
        Me.Label1 = New Label
        Me.Label2 = New Label
        Me.txtSQL = New TextBox
        Me.btnParamBuilder = New Button
        Me.btnSQLTest = New Button
        Me.Label3 = New Label
        Me.cmbResultClass = New ComboBox
        Me.cmdSave = New Button
        Me.lblHelp = New Label
        CType(Me.dgCommandInfo, ISupportInitialize).BeginInit()
        CType(Me.DataSet1, ISupportInitialize).BeginInit()
        CType(Me.dtCommandClass, ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtFileName
        '
        Me.txtFileName.Location = New Drawing.Point(211, 8)
        Me.txtFileName.Name = "txtFileName"
        Me.txtFileName.Size = New Size(383, 21)
        Me.txtFileName.TabIndex = 0
        '
        'btnFileOpen
        '
        Me.btnFileOpen.Location = New Drawing.Point(600, 6)
        Me.btnFileOpen.Name = "btnFileOpen"
        Me.btnFileOpen.Size = New Size(88, 23)
        Me.btnFileOpen.TabIndex = 1
        Me.btnFileOpen.Text = "(&B)打开文件"
        '
        'cmbScriptType
        '
        Me.cmbScriptType.Location = New Drawing.Point(24, 40)
        Me.cmbScriptType.Name = "cmbScriptType"
        Me.cmbScriptType.Size = New Size(168, 20)
        Me.cmbScriptType.TabIndex = 2
        '
        'lstClassName
        '
        Me.lstClassName.ItemHeight = 12
        Me.lstClassName.Location = New Drawing.Point(24, 72)
        Me.lstClassName.Name = "lstClassName"
        Me.lstClassName.Size = New Size(168, 172)
        Me.lstClassName.TabIndex = 3
        '
        'dgCommandInfo
        '
        Me.dgCommandInfo.AllowSorting = False
        Me.dgCommandInfo.DataMember = "dtClass"
        Me.dgCommandInfo.DataSource = Me.DataSet1
        Me.dgCommandInfo.HeaderForeColor = SystemColors.ControlText
        Me.dgCommandInfo.Location = New Drawing.Point(211, 40)
        Me.dgCommandInfo.Name = "dgCommandInfo"
        Me.dgCommandInfo.Size = New Size(771, 203)
        Me.dgCommandInfo.TabIndex = 4
        Me.dgCommandInfo.TableStyles.AddRange(New DataGridTableStyle() {Me.DataGridTableStyle1})
        '
        'DataSet1
        '
        Me.DataSet1.DataSetName = "NewDataSet"
        Me.DataSet1.Locale = New CultureInfo("zh-CN")
        Me.DataSet1.Tables.AddRange(New DataTable() {Me.dtCommandClass})
        '
        'dtCommandClass
        '
        Me.dtCommandClass.Columns.AddRange(
            New DataColumn() _
                                              {Me.CommandName, Me.CommandType, Me.QueryType, Me.Method, Me.Description,
                                               Me.ResultClass, Me.ResultMap})
        Me.dtCommandClass.Constraints.AddRange(
            New Constraint() _
                                                  {New UniqueConstraint("Constraint1", New String() {"命令名"}, True)})
        Me.dtCommandClass.PrimaryKey = New DataColumn() {Me.CommandName}
        Me.dtCommandClass.TableName = "dtClass"
        '
        'CommandName
        '
        Me.CommandName.AllowDBNull = False
        Me.CommandName.Caption = "命令名"
        Me.CommandName.ColumnName = "命令名"
        '
        'CommandType
        '
        Me.CommandType.Caption = "命令类型"
        Me.CommandType.ColumnName = "命令类型"
        '
        'QueryType
        '
        Me.QueryType.Caption = "查询类型"
        Me.QueryType.ColumnName = "查询类型"
        '
        'Method
        '
        Me.Method.Caption = "类成员"
        Me.Method.ColumnName = "类成员"
        '
        'Description
        '
        Me.Description.Caption = "描述"
        Me.Description.ColumnName = "描述"
        Me.Description.MaxLength = 100
        '
        'ResultClass
        '
        Me.ResultClass.Caption = "结果类型"
        Me.ResultClass.ColumnName = "结果类型"
        '
        'ResultMap
        '
        Me.ResultMap.Caption = "结果映射"
        Me.ResultMap.ColumnName = "结果映射"
        '
        'DataGridTableStyle1
        '
        Me.DataGridTableStyle1.DataGrid = Me.dgCommandInfo
        Me.DataGridTableStyle1.GridColumnStyles.AddRange(
            New DataGridColumnStyle() _
                                                            {Me.DataGridTextBoxColumn1, Me.DataGridTextBoxColumn2,
                                                             Me.DataGridTextBoxColumn3, Me.DataGridTextBoxColumn4,
                                                             Me.DataGridTextBoxColumn5, Me.DataGridTextBoxColumn6,
                                                             Me.DataGridTextBoxColumn7})
        Me.DataGridTableStyle1.HeaderForeColor = SystemColors.ControlText
        Me.DataGridTableStyle1.MappingName = "dtClass"
        '
        'DataGridTextBoxColumn1
        '
        Me.DataGridTextBoxColumn1.Format = ""
        Me.DataGridTextBoxColumn1.FormatInfo = Nothing
        Me.DataGridTextBoxColumn1.HeaderText = "命令名"
        Me.DataGridTextBoxColumn1.MappingName = "命令名"
        Me.DataGridTextBoxColumn1.Width = 120
        '
        'DataGridTextBoxColumn2
        '
        Me.DataGridTextBoxColumn2.Format = ""
        Me.DataGridTextBoxColumn2.FormatInfo = Nothing
        Me.DataGridTextBoxColumn2.HeaderText = "命令类型"
        Me.DataGridTextBoxColumn2.MappingName = "命令类型"
        Me.DataGridTextBoxColumn2.Width = 80
        '
        'DataGridTextBoxColumn3
        '
        Me.DataGridTextBoxColumn3.Format = ""
        Me.DataGridTextBoxColumn3.FormatInfo = Nothing
        Me.DataGridTextBoxColumn3.HeaderText = "查询类型"
        Me.DataGridTextBoxColumn3.MappingName = "查询类型"
        Me.DataGridTextBoxColumn3.Width = 80
        '
        'DataGridTextBoxColumn4
        '
        Me.DataGridTextBoxColumn4.Format = ""
        Me.DataGridTextBoxColumn4.FormatInfo = Nothing
        Me.DataGridTextBoxColumn4.HeaderText = "类成员"
        Me.DataGridTextBoxColumn4.MappingName = "类成员"
        Me.DataGridTextBoxColumn4.Width = 75
        '
        'DataGridTextBoxColumn5
        '
        Me.DataGridTextBoxColumn5.Format = ""
        Me.DataGridTextBoxColumn5.FormatInfo = Nothing
        Me.DataGridTextBoxColumn5.HeaderText = "描述"
        Me.DataGridTextBoxColumn5.MappingName = "描述"
        Me.DataGridTextBoxColumn5.Width = 200
        '
        'DataGridTextBoxColumn6
        '
        Me.DataGridTextBoxColumn6.Format = ""
        Me.DataGridTextBoxColumn6.FormatInfo = Nothing
        Me.DataGridTextBoxColumn6.HeaderText = "结果类型"
        Me.DataGridTextBoxColumn6.MappingName = "结果类型"
        Me.DataGridTextBoxColumn6.Width = 75
        '
        'DataGridTextBoxColumn7
        '
        Me.DataGridTextBoxColumn7.Format = ""
        Me.DataGridTextBoxColumn7.FormatInfo = Nothing
        Me.DataGridTextBoxColumn7.HeaderText = "结果映射"
        Me.DataGridTextBoxColumn7.MappingName = "结果映射"
        Me.DataGridTextBoxColumn7.Width = 75
        '
        'txtCommandText
        '
        Me.txtCommandText.Location = New Drawing.Point(24, 508)
        Me.txtCommandText.Multiline = True
        Me.txtCommandText.Name = "txtCommandText"
        Me.txtCommandText.ScrollBars = ScrollBars.Both
        Me.txtCommandText.Size = New Size(958, 175)
        Me.txtCommandText.TabIndex = 5
        Me.txtCommandText.Text = "CommandText"
        Me.txtCommandText.WordWrap = False
        '
        'btnParseXml
        '
        Me.btnParseXml.Location = New Drawing.Point(706, 6)
        Me.btnParseXml.Name = "btnParseXml"
        Me.btnParseXml.Size = New Size(88, 23)
        Me.btnParseXml.TabIndex = 6
        Me.btnParseXml.Text = "(&R)重新加载"
        '
        'cmbCmdType
        '
        Me.cmbCmdType.Location = New Drawing.Point(608, 283)
        Me.cmbCmdType.Name = "cmbCmdType"
        Me.cmbCmdType.Size = New Size(80, 20)
        Me.cmbCmdType.TabIndex = 7
        Me.cmbCmdType.Text = "Text"
        '
        'btnConfig
        '
        Me.btnConfig.Location = New Drawing.Point(891, 6)
        Me.btnConfig.Name = "btnConfig"
        Me.btnConfig.Size = New Size(88, 23)
        Me.btnConfig.TabIndex = 8
        Me.btnConfig.Text = "(&M)配置管理"
        '
        'Label1
        '
        Me.Label1.Location = New Drawing.Point(24, 487)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New Size(168, 16)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "SqlMap 脚本:"
        '
        'Label2
        '
        Me.Label2.Location = New Drawing.Point(24, 289)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New Size(100, 16)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "SQL 语句:"
        '
        'txtSQL
        '
        Me.txtSQL.Location = New Drawing.Point(24, 311)
        Me.txtSQL.Multiline = True
        Me.txtSQL.Name = "txtSQL"
        Me.txtSQL.ScrollBars = ScrollBars.Both
        Me.txtSQL.Size = New Size(958, 173)
        Me.txtSQL.TabIndex = 11
        Me.txtSQL.Text = "SQL"
        Me.txtSQL.WordWrap = False
        '
        'btnParamBuilder
        '
        Me.btnParamBuilder.Location = New Drawing.Point(211, 279)
        Me.btnParamBuilder.Name = "btnParamBuilder"
        Me.btnParamBuilder.Size = New Size(98, 26)
        Me.btnParamBuilder.TabIndex = 14
        Me.btnParamBuilder.Text = "(&P)参数生成器"
        '
        'btnSQLTest
        '
        Me.btnSQLTest.Location = New Drawing.Point(360, 279)
        Me.btnSQLTest.Name = "btnSQLTest"
        Me.btnSQLTest.Size = New Size(96, 26)
        Me.btnSQLTest.TabIndex = 15
        Me.btnSQLTest.Text = "(&T)查询测试"
        '
        'Label3
        '
        Me.Label3.Location = New Drawing.Point(24, 8)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New Size(142, 23)
        Me.Label3.TabIndex = 16
        Me.Label3.Text = "Sql Map 配置文件："
        '
        'cmbResultClass
        '
        Me.cmbResultClass.Location = New Drawing.Point(514, 283)
        Me.cmbResultClass.Name = "cmbResultClass"
        Me.cmbResultClass.Size = New Size(80, 20)
        Me.cmbResultClass.TabIndex = 17
        Me.cmbResultClass.Text = "ResultClass"
        '
        'cmdSave
        '
        Me.cmdSave.Location = New Drawing.Point(810, 6)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New Size(75, 23)
        Me.cmdSave.TabIndex = 18
        Me.cmdSave.Text = "(&S)保存"
        Me.cmdSave.UseVisualStyleBackColor = True
        '
        'lblHelp
        '
        Me.lblHelp.ForeColor = Color.DarkBlue
        Me.lblHelp.Location = New Drawing.Point(212, 246)
        Me.lblHelp.Name = "lblHelp"
        Me.lblHelp.Size = New Size(770, 26)
        Me.lblHelp.TabIndex = 19
        Me.lblHelp.Text = "请打开一个配置文件！"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New Size(6, 14)
        Me.ClientSize = New Size(994, 695)
        Me.Controls.Add(Me.lblHelp)
        Me.Controls.Add(Me.cmdSave)
        Me.Controls.Add(Me.cmbResultClass)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.btnSQLTest)
        Me.Controls.Add(Me.btnParamBuilder)
        Me.Controls.Add(Me.txtSQL)
        Me.Controls.Add(Me.txtCommandText)
        Me.Controls.Add(Me.txtFileName)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnConfig)
        Me.Controls.Add(Me.cmbCmdType)
        Me.Controls.Add(Me.btnParseXml)
        Me.Controls.Add(Me.dgCommandInfo)
        Me.Controls.Add(Me.lstClassName)
        Me.Controls.Add(Me.cmbScriptType)
        Me.Controls.Add(Me.btnFileOpen)
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.Text = "SqlMap Builder Ver 3.1.3"
        CType(Me.dgCommandInfo, ISupportInitialize).EndInit()
        CType(Me.DataSet1, ISupportInitialize).EndInit()
        CType(Me.dtCommandClass, ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

#End Region

    Private Sub btnFileOpen_Click(sender As Object, e As EventArgs) Handles btnFileOpen.Click
        Me.OpenFileDialog1.ShowDialog()
        Me.txtFileName.Text = Me.OpenFileDialog1.FileName
        ConfirmSave()
        GetExistsScriptType(Me.txtFileName.Text, Me.cmbScriptType)
        cmbScriptType.Text = "选择脚本类型"
        Me.lblHelp.Text = "请先选择脚本类型。"
    End Sub

    Private Sub btnParseXml_Click(sender As Object, e As EventArgs) Handles btnParseXml.Click
        'Dim doc As XmlDocument = New XmlDocument
        'Dim _ErrDescription As String
        'Me.cmbScriptType.Items.Clear()
        'Try
        '    doc.Load(Me.txtFileName.Text) '可以考虑采用XML文件流的方式加快读取

        '    Dim SqlScripts As XmlNodeList
        '    Dim root As XmlElement = doc.DocumentElement
        '    SqlScripts = root.SelectNodes("/SqlMap/Script")
        '    If Not SqlScripts Is Nothing Then
        '        For Each node As XmlNode In SqlScripts
        '            If Not node.Attributes("Type") Is Nothing Then
        '                Me.cmbScriptType.Items.Add(node.Attributes("Type").Value)
        '            End If
        '        Next
        '    End If
        'Catch ex As Exception
        '    _ErrDescription = ex.Message
        'End Try
        ConfirmSave()
        GetExistsScriptType(Me.txtFileName.Text, Me.cmbScriptType)
        cmbScriptType.Text = "选择脚本类型"
    End Sub

    Private Sub cmbScriptType_SelectedIndexChanged(sender As Object, e As EventArgs) _
        Handles cmbScriptType.SelectedIndexChanged
        Me.lblHelp.Text = "选SQL-MAP择脚本对应的数据库类型。可以在SQL-MAP配置文件中指定不同的数据库类型，从而在程序运行时适配合适的数据驱动程序。"
        'Dim doc As XmlDocument = New XmlDocument
        'Dim _ErrDescription As String
        'Me.lstClassName.Items.Clear()
        'Try
        '    doc.Load(Me.txtFileName.Text) '可以考虑采用XML文件流的方式加快读取

        '    Dim SqlScripts As XmlNode
        '    Dim root As XmlElement = doc.DocumentElement
        '    SqlScripts = root.SelectSingleNode("/SqlMap/Script[@Type='" & CStr(cmbScriptType.SelectedItem) & "']")
        '    If Not SqlScripts Is Nothing AndAlso SqlScripts.HasChildNodes Then
        '        For Each node As XmlNode In SqlScripts.ChildNodes
        '            If node.NodeType = XmlNodeType.Element AndAlso Not node.Attributes("Name") Is Nothing Then
        '                Me.lstClassName.Items.Add(node.Attributes("Name").Value)
        '            End If
        '        Next
        '    End If
        'Catch ex As Exception
        '    _ErrDescription = ex.Message
        'End Try
        Dim strScriptType As String = CStr(cmbScriptType.SelectedItem)
        'CurrDataBase..DataBaseType = [Enum].Parse(GetType(DataBase.enumDataBaseType), strScriptType)
        GetClassNameList(Me.txtFileName.Text, strScriptType, Me.lstClassName)
        Mapper.DataBase = MyDB.GetDBHelper() 'DataBase.CreateDataBase(CurrDataBase.DataBaseType)
    End Sub

    '确认是否保存
    Private Sub ConfirmSave()
        If DataSet1.HasChanges() Then 'Or SqlScriptChanged
            Select Case MsgBox("你已经做了修改，需要保存吗？", MsgBoxStyle.YesNoCancel, "操作提示")
                Case MsgBoxResult.Cancel
                    Debug.WriteLine("用户已经取消该操作")
                    Exit Sub
                Case MsgBoxResult.Yes
                    Debug.WriteLine("保存修改结果")
                    AddNode()
                Case MsgBoxResult.No
                    Debug.WriteLine("放弃修改继续操作")
                    DataSet1.Reset()
            End Select
            DataSet1.AcceptChanges()
        End If
    End Sub

    Private Sub lstClassName_SelectedIndexChanged(sender As Object, e As EventArgs) _
        Handles lstClassName.SelectedIndexChanged
        Me.lblHelp.Text = "选择命令组。命令组是一组SQL-MAP脚本的集合，映射为.NET的一个数据访问层类。"
        ConfirmSave()
        Me.dgCommandInfo.ReadOnly = False
        Dim doc As XmlDocument = New XmlDocument
        Dim _ErrDescription As String
        AddRowInex = - 1
        EditRowIndex = - 1
        Me.dtCommandClass.Rows.Clear()
        Me.cmbCmdType.Visible = False
        Me.cmbResultClass.Visible = False
        Me.txtCommandText.Text = ""
        ArrSqlScript.Clear() '清空SQL-MAP脚本数组
        Try
            doc.Load(Me.txtFileName.Text) '可以考虑采用XML文件流的方式加快读取

            Dim SqlScripts As XmlNode
            Dim root As XmlElement = doc.DocumentElement
            Dim ClassName As String = CStr(Me.lstClassName.SelectedItem)
            XPath = "/SqlMap/Script[@Type='" & CStr(cmbScriptType.SelectedItem) & "']/CommandClass[@Name='" & ClassName &
                    "']"
            SqlScripts = root.SelectSingleNode(XPath)
            If Not SqlScripts Is Nothing AndAlso SqlScripts.HasChildNodes Then
                For Each node As XmlNode In SqlScripts.ChildNodes
                    If node.NodeType = XmlNodeType.Element AndAlso Not node.Attributes("CommandName") Is Nothing Then
                        'Me.lstClassName.Items.Add(node.Attributes("CommandName").Value)
                        Dim CommandName As String = node.Attributes("CommandName").Value
                        Dim dr As DataRow = Me.dtCommandClass.NewRow()
                        dr(0) = CommandName
                        dr(1) = node.Attributes("CommandType").Value
                        dr(2) = node.Name
                        If Not node.Attributes("Method") Is Nothing Then
                            dr(3) = node.Attributes("Method").Value
                        Else
                            dr(3) = ""
                        End If
                        If Not node.Attributes("Description") Is Nothing Then
                            dr(4) = node.Attributes("Description").Value
                        Else
                            dr(4) = ""
                        End If
                        If Not node.Attributes("ResultClass") Is Nothing Then
                            dr(5) = node.Attributes("ResultClass").Value
                        End If
                        If Not node.Attributes("ResultMap") Is Nothing Then
                            dr(6) = node.Attributes("ResultMap").Value
                        End If
                        Me.dtCommandClass.Rows.Add(dr)
                        ArrSqlScript.Add(CommandName, node.InnerText)

                    End If
                Next
            End If
            Me.dtCommandClass.AcceptChanges()
            Me.dgCommandInfo.CaptionText = XPath
        Catch ex As Exception
            _ErrDescription = ex.Message
            MsgBox(_ErrDescription, MsgBoxStyle.Critical)
        End Try
    End Sub


    Private Sub dgCommandInfo_Click(sender As Object, e As EventArgs) Handles dgCommandInfo.Click
        Try
            'AddRowInex = -1
            Me.lblHelp.Text = "选择一条SQL-MAP脚本命令。移动[当前单元格]，可以编辑当前信息。如果需要增加新的命令，请在空白行输入信息。"
            SelectCommandName()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub SelectCommandName()

        If _
            ArrSqlScript.Count > 0 AndAlso dtCommandClass.Rows.Count > 0 AndAlso
            dgCommandInfo.CurrentRowIndex() < ArrSqlScript.Count Then
            Dim Key As Object _
            '= dtCommandClass.Rows(dgCommandInfo.CurrentRowIndex).RowState=DataRowState.Modified .Item(0, DataRowVersion.Original)  '  dgCommandInfo.Item(dgCommandInfo.CurrentRowIndex, 0)
            If dtCommandClass.Rows(dgCommandInfo.CurrentRowIndex).RowState = DataRowState.Modified Then
                Key = dtCommandClass.Rows(dgCommandInfo.CurrentRowIndex).Item(0, DataRowVersion.Original)
            Else
                Key = dgCommandInfo.Item(dgCommandInfo.CurrentRowIndex, 0)
            End If
            If ArrSqlScript.ContainsKey(Key) AndAlso Not ArrSqlScript(Key) Is Nothing Then
                Me.txtCommandText.Text = CStr(ArrSqlScript(Key)).Trim()
            Else
                Me.txtCommandText.Text = "NULL"
            End If
        End If
        cmbCmdType.Visible = False
        cmbResultClass.Visible = False
    End Sub

    Private Sub dgCommandInfo_CurrentCellChanged(sender As Object, e As EventArgs) _
        Handles dgCommandInfo.CurrentCellChanged
        Select Case dgCommandInfo.CurrentCell.ColumnNumber
            Case 0
                Me.lblHelp.Text = "SQL-MAP脚本命令名。在同一个命令组中必须唯一，对应于SQL-MAP数据访问类中的一个方法名称。"
            Case 1
                Me.lblHelp.Text = "SQL语句的命令类型(CommandType)。"
            Case 2
                Me.lblHelp.Text = "SQL-MAP脚本命令类型，它指示代码生成器生成合适的CRUD操作方法。"
            Case 3
                Me.lblHelp.Text = "如果指定了类成员名称，那么将使用这个名称代替命令名，作为SQL-MAP数据访问类中的一个方法名称。"
            Case 4
                Me.lblHelp.Text = "对当前SQL-MAP脚本命令的描述，建议你为每个命令写上相信的描述信息，便于理解和维护。"
            Case 5
                Me.lblHelp.Text = "结果类型，指示查询的结果类型是值类型，数据集，还是实体类或者实体类集合。"
            Case 6
                Me.lblHelp.Text = "结果映射，如果你指定了结果类型是实体类或实体类集合，那么需要指定具体的结果映射的目标实体类类型。"

        End Select

        If Me.LastRowIndex <> dgCommandInfo.CurrentRowIndex Then
            Me.LastRowIndex = dgCommandInfo.CurrentRowIndex
            SelectCommandName()
        End If
        If EditRowIndex <> - 1 And Me.dtCommandClass.Rows.Count >= EditRowIndex Then
            '编辑信息
            Dim NewEditedName As String = dgCommandInfo.Item(EditRowIndex, 0)
            Debug.WriteLine("edit row " & EditRowIndex.ToString & ":" & NewEditedName)
            Debug.WriteLine("LastSelectedCmdName :" & LastSelectedCmdName)
            '更改键名
            If Not ArrSqlScript.ContainsKey(NewEditedName) Then
                ArrSqlScript.Add(NewEditedName, ArrSqlScript(LastSelectedCmdName))
            End If
            EditRowIndex = - 1

        End If

        Dim tempIndex As Integer = - 1 '刚被添加的行
        If AddRowInex = dgCommandInfo.CurrentRowIndex Then
            tempIndex = AddRowInex
        Else
            tempIndex = dgCommandInfo.CurrentRowIndex
        End If
        On Error GoTo ex
        If _
            AddRowInex <> - 1 And tempIndex <> - 1 And EditRowIndex = - 1 And AddRowInex > dtCommandClass.Rows.Count And
            AddRowInex = dgCommandInfo.CurrentRowIndex AndAlso Not dgCommandInfo.Item(tempIndex, 0) Is Nothing AndAlso
            Not dgCommandInfo.Item(tempIndex, 0) Is DBNull.Value Then
            '添加新行
            '默认值
            dgCommandInfo.Item(tempIndex, 1) = "Text"
            dgCommandInfo.Item(tempIndex, 2) = "Select"
            '保存新键
            Dim NewAddName As String = dgCommandInfo.Item(tempIndex, 0)
            If Not ArrSqlScript.ContainsKey(NewAddName) Then
                ArrSqlScript.Add(NewAddName, Me.txtCommandText.Text)
                AddRowInex = - 1
                Debug.WriteLine("new key added:" & NewAddName)
            End If

        End If

        If dgCommandInfo.CurrentRowIndex >= ArrSqlScript.Count Then
            '可能处于增加记录状态
            AddRowInex = dgCommandInfo.CurrentRowIndex
            Me.txtCommandText.Text = ""
            Debug.WriteLine("New  Count=" & Me.dtCommandClass.Rows.Count)
        End If

        Select Case dgCommandInfo.CurrentCell.ColumnNumber
            Case 0
                If AddRowInex <> - 1 And tempIndex <> - 1 Then
                    If Not dgCommandInfo.Item(dgCommandInfo.CurrentCell) Is DBNull.Value Then
                        EditRowIndex = dgCommandInfo.CurrentCell.RowNumber
                        LastSelectedCmdName = dgCommandInfo.Item(dgCommandInfo.CurrentCell)
                    End If
                    SelectCommandName()
                    dgCommandInfo.ReadOnly = False
                End If

            Case 1
                If Not IsCmdType Then
                    IsCmdType = True
                    LoadCmdType(IsCmdType) '装载命令类型
                End If
                ShowCmbCmdType()
            Case 2
                If IsCmdType Then
                    IsCmdType = False
                    LoadCmdType(IsCmdType) '装载查询类型
                End If
                ShowCmbCmdType()
            Case 5
                If dgCommandInfo.Item(dgCommandInfo.CurrentRowIndex, 2) = "Select" Then
                    ShowDGCombox(Me.cmbResultClass)
                End If
            Case Else
                Me.cmbCmdType.Visible = False
                Me.cmbResultClass.Visible = False
                dgCommandInfo.ReadOnly = False
        End Select
        ex:
        If Err.Number <> 0 Then
            MsgBox(Err.Description, MsgBoxStyle.Critical, "操作错误")
            Err.Clear()
            AddRowInex = - 1
            EditRowIndex = - 1
        End If
    End Sub

    Private Sub ShowCmbCmdType()
        ShowDGCombox(cmbCmdType)
    End Sub

    Private Sub ShowDGCombox(myCmb As ComboBox)
        myCmb.SelectedItem = dgCommandInfo.Item(dgCommandInfo.CurrentCell)
        myCmb.Width = dgCommandInfo.GetCurrentCellBounds.Width
        myCmb.Left = dgCommandInfo.Left + dgCommandInfo.GetCurrentCellBounds.Left
        myCmb.Top = dgCommandInfo.Top + dgCommandInfo.GetCurrentCellBounds.Top
        myCmb.Text = IIf(dgCommandInfo.Item(dgCommandInfo.CurrentCell) Is DBNull.Value, "",
                         dgCommandInfo.Item(dgCommandInfo.CurrentCell))
        myCmb.Visible = True
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        IsCmdType = True
        AddRowInex = - 1
        EditRowIndex = - 1
        cmbCmdType.Visible = False
        cmbResultClass.Visible = False
        cmbResultClass.DataSource = [Enum].GetNames(GetType(enumResultClass))
        LoadCmdType(IsCmdType)
        dgCommandInfo.ReadOnly = True
        Mapper = New SqlMapper
    End Sub

    Private Sub LoadCmdType(TypeFlag As Boolean)
        'cmbCmdType.Items.Clear()
        cmbCmdType.DataSource = Nothing
        cmbCmdType.SelectedIndex = - 1
        IsLoadCmdType = True
        If TypeFlag Then
            'cmbCmdType.Items.Add(System.Data.CommandType.StoredProcedure.ToString())
            'cmbCmdType.Items.Add(System.Data.CommandType.TableDirect.ToString)
            'cmbCmdType.Items.Add(System.Data.CommandType.Text.ToString)
            cmbCmdType.DataSource = [Enum].GetNames(GetType(CommandType))
        Else
            'cmbCmdType.Items.Add("Select")
            'cmbCmdType.Items.Add("Update")
            'cmbCmdType.Items.Add("Delete")
            'cmbCmdType.Items.Add("Create")
            cmbCmdType.DataSource = [Enum].GetNames(GetType(enumQueryType))
        End If
    End Sub

    Private Sub cmbCmdType_TextChanged(sender As Object, e As EventArgs) Handles cmbCmdType.TextChanged
        If IsLoadCmdType Then
            IsLoadCmdType = False
            Exit Sub
        End If
        If dgCommandInfo.CurrentRowIndex = dgCommandInfo.CurrentCell.RowNumber Then _
' dgCommandInfo.CurrentRowIndex <> -1 
            If cmbCmdType.SelectedIndex <> - 1 Then
                Dim strText As String = cmbCmdType.Text
                For I As Integer = 0 To cmbCmdType.Items.Count - 1
                    If strText = cmbCmdType.Items(I) Then
                        '此处可能出错
                        dgCommandInfo.Item(dgCommandInfo.CurrentCell) = strText
                        Exit For
                    End If
                Next

            End If
        End If
    End Sub


    Private Sub dgCommandInfo_Scroll(sender As Object, e As EventArgs) Handles dgCommandInfo.Scroll
        Me.cmbCmdType.Visible = False
        Me.cmbResultClass.Visible = False
    End Sub

    Private Sub txtCommandText_TextChanged(sender As Object, e As EventArgs) Handles txtCommandText.TextChanged
        On Error Resume Next
        If Not Mapper Is Nothing Then
            'Dim cmdInfo As CommandInfo = Mapper.GetScriptInfo(txtCommandText.Text)
            txtSQL.Text = Mapper.GetScriptInfo(txtCommandText.Text) 'cmdInfo.CommandText
            SqlMapScriptChanged = True
        End If
    End Sub

    Private Sub AddNode()
        Dim doc As XmlDocument = New XmlDocument
        Dim _ErrDescription As String
        Try
            doc.Load(Me.txtFileName.Text) '可以考虑采用XML文件流的方式加快读取

            Dim SqlScripts As XmlNode
            Dim root As XmlElement = doc.DocumentElement
            SqlScripts = root.SelectSingleNode(XPath)
            If Not SqlScripts Is Nothing Then

                UpdateSubNode(doc, SqlScripts, DataRowState.Added)
                UpdateSubNode(doc, SqlScripts, DataRowState.Deleted)
                UpdateSubNode(doc, SqlScripts, DataRowState.Modified)

                doc.Save(Me.txtFileName.Text)
            End If
        Catch ex As Exception
            _ErrDescription = ex.Message
            MsgBox(_ErrDescription, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub UpdateSubNode(doc As XmlDocument, SqlScripts As XmlNode, rowState As DataRowState)
        Dim myTable As DataTable
        Dim node As XmlElement
        myTable = Me.dtCommandClass.GetChanges(rowState)
        If Not myTable Is Nothing Then
            For Each dr As DataRow In myTable.Rows
                Select Case rowState
                    Case DataRowState.Added
                        AddSubNode(doc, SqlScripts, dr)
                    Case DataRowState.Deleted
                        node = SqlScripts.SelectSingleNode("./*[@CommandName='" & dr(0, DataRowVersion.Original) & "']")
                        SqlScripts.RemoveChild(node)
                    Case DataRowState.Modified
                        node = SqlScripts.SelectSingleNode("./*[@CommandName='" & dr(0, DataRowVersion.Original) & "']")
                        SqlScripts.RemoveChild(node)
                        EditSubNode(doc, SqlScripts, dr)
                End Select
            Next
        End If
    End Sub

    Private Sub AddSubNode(doc As XmlDocument, SqlScripts As XmlNode, dr As DataRow)
        Dim node As XmlElement = GetUpdateSubNode(doc, dr)
        Dim CDATASqlText As XmlCDataSection = doc.CreateCDataSection(ArrSqlScript(dr(0)))
        node.AppendChild(CDATASqlText)
        SqlScripts.AppendChild(node)
    End Sub

    Private Sub EditSubNode(doc As XmlDocument, SqlScripts As XmlNode, dr As DataRow)
        Dim node As XmlElement = GetUpdateSubNode(doc, dr)
        Dim CDATASqlText As XmlCDataSection = doc.CreateCDataSection(ArrSqlScript(dr(0, DataRowVersion.Original)))
        node.AppendChild(CDATASqlText)
        SqlScripts.AppendChild(node)
    End Sub

    Private Function GetUpdateSubNode(doc As XmlDocument, dr As DataRow) As XmlElement
        Dim node As XmlElement
        If dr.IsNull(0) Then Throw New Exception("列 [" & dr.Table.Columns(0).ColumnName() & "] 不能为空！")
        If dr.IsNull(1) Then Throw New Exception("列 [" & dr.Table.Columns(1).ColumnName() & "] 不能为空！")

        node = doc.CreateElement(dr(2))
        node.SetAttribute("CommandName", dr(0))
        node.SetAttribute("CommandType", dr(1))
        If Not dr.IsNull(3) Then node.SetAttribute("Method", dr(3))
        If Not dr.IsNull(4) Then node.SetAttribute("Description", dr(4))
        If Not dr.IsNull(5) AndAlso dr(2) = "Select" Then
            '处理结果类型
            node.SetAttribute("ResultClass", dr(5))
            If dr(5) = enumResultClass.EntityObject.ToString() Or dr(5) = enumResultClass.EntityList.ToString Then
                If dr(6) Is DBNull.Value Then
                    MessageBox.Show(dr(0).ToString() & " 需要指定映射具体的实体类类型！")
                Else
                    node.SetAttribute("ResultMap", dr(6))
                End If

            End If
        End If
        Return node
    End Function


    Private Sub dtCommandClass_RowDeleted(sender As Object, e As DataRowChangeEventArgs) _
        Handles dtCommandClass.RowDeleted
        EditRowIndex = - 1
    End Sub

    Private Sub btnConfig_Click(sender As Object, e As EventArgs) Handles btnConfig.Click
        ConfirmSave()
        Dim FileName As String = Me.txtFileName.Text
        Dim frmNewOpt1 As New frmNewOpt
        If File.Exists(FileName) Then
            frmNewOpt1.CurrConfigFile = FileName
        End If
        frmNewOpt1.Show()
    End Sub


    Private Sub txtCommandText_LostFocus(sender As Object, e As EventArgs) Handles txtCommandText.LostFocus
        '记录修改
        Dim index As Integer = dgCommandInfo.CurrentRowIndex
        If index <> - 1 Then
            Dim Key As Object = dgCommandInfo.Item(index, 0)
            If ArrSqlScript.ContainsKey(Key) Then
                ArrSqlScript(Key) = txtCommandText.Text
                'SqlScriptChanged = True
                EditRowIndex = index
                dtCommandClass.Rows(index).BeginEdit()
                dtCommandClass.Rows(index).EndEdit()
            End If
        End If
    End Sub

    Private Sub btnParamBuilder_Click(sender As Object, e As EventArgs) Handles btnParamBuilder.Click
        If Me.txtSQL.Text = "" Then
            MsgBox("请先选择网格中的一条记录（如果已经选择请输入SQL语句）", MsgBoxStyle.Exclamation, "参数生成器")
            Return
        End If
        Dim frmParasBuilder As New frmParamsBuilder
        'txtCommandText.Text = "正在解析参数，请稍候。。。"
        If dgCommandInfo.CurrentRowIndex <> - 1 Then

            frmParasBuilder.DataBaseType = [Enum].Parse(GetType(DBMSType), Me.cmbScriptType.SelectedItem.ToString()) _
            'DataBase.enumDataBaseType.DB2
            frmParasBuilder.CommandType = dgCommandInfo.Item(dgCommandInfo.CurrentRowIndex, 1)
            frmParasBuilder.QueryType = dgCommandInfo.Item(dgCommandInfo.CurrentRowIndex, 2)
            frmParasBuilder.SqlMapScript = txtCommandText.Text
            frmParasBuilder.SqlText = Me.txtSQL.Text
            frmParasBuilder.ShowDialog()
            If frmParasBuilder.SqlMapScript <> "" Then
                txtCommandText.Text = frmParasBuilder.SqlMapScript()
                txtCommandText.Focus()
            End If
        Else
            MsgBox("请先加载配置文件并选择一行！")
        End If
    End Sub

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles MyBase.Closing
        ConfirmSave()
    End Sub

    Private Sub cmbResultClass_TextChanged(sender As Object, e As EventArgs) Handles cmbResultClass.TextChanged
        If dgCommandInfo.CurrentRowIndex = dgCommandInfo.CurrentCell.RowNumber Then _
' dgCommandInfo.CurrentRowIndex <> -1 
            If cmbResultClass.SelectedIndex <> - 1 Then
                Dim strText As String = cmbResultClass.Text
                For I As Integer = 0 To cmbResultClass.Items.Count - 1
                    If strText = cmbResultClass.Items(I) Then
                        dgCommandInfo.Item(dgCommandInfo.CurrentCell) = strText
                        Exit For
                    End If
                Next

            End If
        End If
    End Sub

    Private Sub btnSQLTest_Click(sender As Object, e As EventArgs) Handles btnSQLTest.Click
        'MsgBox("该版本暂时不支持此功能！")
        Dim frmParasBuilder As New frmParamsBuilder
        'txtCommandText.Text = "正在解析参数，请稍候。。。"
        frmParasBuilder.CommandType = "Text"
        frmParasBuilder.QueryType = "Select"
        frmParasBuilder.SqlMapScript = txtCommandText.Text
        frmParasBuilder.SqlText = Me.txtSQL.Text
        frmParasBuilder.ShowDialog()
    End Sub

    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        ConfirmSave()
    End Sub

    Private Sub txtCommandText_Leave(sender As Object, e As EventArgs) Handles txtCommandText.Leave
        If SqlMapScriptChanged Then
            If ArrSqlScript.Count = 0 AndAlso dtCommandClass.Rows.Count = 1 AndAlso dgCommandInfo.CurrentRowIndex() = 0 _
                Then
                Dim Key As Object = dgCommandInfo.Item(dgCommandInfo.CurrentRowIndex, 0)
                ArrSqlScript(Key) = Me.txtCommandText.Text
            End If

            If _
                ArrSqlScript.Count > 0 AndAlso dtCommandClass.Rows.Count > 0 AndAlso
                dgCommandInfo.CurrentRowIndex() <= ArrSqlScript.Count Then
                Dim Key As Object _
                '= dtCommandClass.Rows(dgCommandInfo.CurrentRowIndex).RowState=DataRowState.Modified .Item(0, DataRowVersion.Original)  '  dgCommandInfo.Item(dgCommandInfo.CurrentRowIndex, 0)
                If dtCommandClass.Rows(dgCommandInfo.CurrentRowIndex).RowState = DataRowState.Modified Then
                    Key = dtCommandClass.Rows(dgCommandInfo.CurrentRowIndex).Item(0, DataRowVersion.Original)
                Else
                    Key = dgCommandInfo.Item(dgCommandInfo.CurrentRowIndex, 0)
                End If
                If ArrSqlScript.ContainsKey(Key) AndAlso Not ArrSqlScript(Key) Is Nothing Then
                    ArrSqlScript(Key) = Me.txtCommandText.Text
                    dgCommandInfo.Item(dgCommandInfo.CurrentRowIndex, 4) += ""
                Else
                    ArrSqlScript(Key) = Me.txtCommandText.Text
                End If

                SqlMapScriptChanged = False
            End If
        End If
    End Sub

    Private Sub txtSQL_Enter(sender As Object, e As EventArgs) Handles txtSQL.Enter
        Me.lblHelp.Text = "请在此输入你准备查询的SQL 语句，可以使用 @参数名 的参数查询方式，然后单击 [参数生成器]，进行参数信息设置，最后确定，就会自动生成SQL-MAP脚本。"
    End Sub

    Private Sub txtCommandText_Enter(sender As Object, e As EventArgs) Handles txtCommandText.Enter
        Me.lblHelp.Text = "SQL-MAP脚本编辑框，你可以在这里直接输入SQL-MAP脚本，但是建议你先使用[参数生成器]并且执行[查询测试]，由程序为你生成正确的 SQL-MAP脚本。"
    End Sub

    Private Sub txtSQL_Leave(sender As Object, e As EventArgs) Handles txtSQL.Leave
        If Me.txtCommandText.Text.Trim() = "" Then
            ' MessageBox.Show("请输入SQL-MAP脚本！（可以单击【参数生成器】自动生成）", "操作提示")
            Me.lblHelp.Text = "请输入SQL-MAP脚本！（可以单击【参数生成器】自动生成）"
        End If
    End Sub
End Class
