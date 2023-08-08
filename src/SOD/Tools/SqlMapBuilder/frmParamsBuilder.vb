Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Globalization
Imports PWMIS.Common
Imports PWMIS.DataMap.SqlMap
Imports PWMIS.DataProvider.Adapter

Public Class frmParamsBuilder
    Inherits Form

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
    Friend WithEvents Label1 As Label
    Friend WithEvents txtSQL As TextBox
    Friend WithEvents btnParse As Button
    Friend WithEvents DataGrid1 As DataGrid
    Friend WithEvents btnOK As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents DataSet1 As DataSet
    Friend WithEvents dtParams As DataTable
    Friend WithEvents ParamName As DataColumn
    Friend WithEvents PID As DataColumn
    Friend WithEvents DataType As DataColumn
    Friend WithEvents FieldType As DataColumn
    Friend WithEvents FieldSize As DataColumn
    Friend WithEvents ParaDirect As DataColumn
    Friend WithEvents cmbDataType As ComboBox
    Friend WithEvents cmbDBType As ComboBox
    Friend WithEvents cmbParaDirect As ComboBox
    Friend WithEvents btnQeuryTest As Button
    Friend WithEvents paraTestValue As DataColumn
    Friend WithEvents DataGrid2 As DataGrid

    <DebuggerStepThrough>
    Private Sub InitializeComponent()
        Me.Label1 = New Label
        Me.txtSQL = New TextBox
        Me.btnParse = New Button
        Me.DataGrid1 = New DataGrid
        Me.DataSet1 = New DataSet
        Me.dtParams = New DataTable
        Me.PID = New DataColumn
        Me.ParamName = New DataColumn
        Me.DataType = New DataColumn
        Me.FieldType = New DataColumn
        Me.FieldSize = New DataColumn
        Me.ParaDirect = New DataColumn
        Me.paraTestValue = New DataColumn
        Me.btnOK = New Button
        Me.btnCancel = New Button
        Me.cmbDataType = New ComboBox
        Me.cmbDBType = New ComboBox
        Me.cmbParaDirect = New ComboBox
        Me.btnQeuryTest = New Button
        Me.DataGrid2 = New DataGrid
        CType(Me.DataGrid1, ISupportInitialize).BeginInit()
        CType(Me.DataSet1, ISupportInitialize).BeginInit()
        CType(Me.dtParams, ISupportInitialize).BeginInit()
        CType(Me.DataGrid2, ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Location = New Drawing.Point(16, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New Size(100, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "查询语句："
        '
        'txtSQL
        '
        Me.txtSQL.AcceptsReturn = True
        Me.txtSQL.Location = New Drawing.Point(16, 40)
        Me.txtSQL.Multiline = True
        Me.txtSQL.Name = "txtSQL"
        Me.txtSQL.ScrollBars = ScrollBars.Both
        Me.txtSQL.Size = New Size(715, 142)
        Me.txtSQL.TabIndex = 12
        Me.txtSQL.Text = "SQL"
        Me.txtSQL.WordWrap = False
        '
        'btnParse
        '
        Me.btnParse.Location = New Drawing.Point(88, 8)
        Me.btnParse.Name = "btnParse"
        Me.btnParse.Size = New Size(86, 25)
        Me.btnParse.TabIndex = 13
        Me.btnParse.Text = "(I)参数分析"
        '
        'DataGrid1
        '
        Me.DataGrid1.CaptionText = "参数信息表"
        Me.DataGrid1.DataMember = "tbParams"
        Me.DataGrid1.DataSource = Me.DataSet1
        Me.DataGrid1.HeaderForeColor = SystemColors.ControlText
        Me.DataGrid1.Location = New Drawing.Point(16, 188)
        Me.DataGrid1.Name = "DataGrid1"
        Me.DataGrid1.Size = New Size(715, 168)
        Me.DataGrid1.TabIndex = 16
        '
        'DataSet1
        '
        Me.DataSet1.DataSetName = "NewDataSet"
        Me.DataSet1.Locale = New CultureInfo("zh-CN")
        Me.DataSet1.Tables.AddRange(New DataTable() {Me.dtParams})
        '
        'dtParams
        '
        Me.dtParams.Columns.AddRange(
            New DataColumn() _
                                        {Me.PID, Me.ParamName, Me.DataType, Me.FieldType, Me.FieldSize, Me.ParaDirect,
                                         Me.paraTestValue})
        Me.dtParams.Constraints.AddRange(
            New Constraint() _
                                            {New UniqueConstraint("Constraint1", New String() {"编号"}, False)})
        Me.dtParams.TableName = "tbParams"
        '
        'PID
        '
        Me.PID.AllowDBNull = False
        Me.PID.AutoIncrement = True
        Me.PID.AutoIncrementSeed = CType(1, Long)
        Me.PID.Caption = "编号"
        Me.PID.ColumnName = "编号"
        Me.PID.DataType = GetType(Integer)
        Me.PID.ReadOnly = True
        '
        'ParamName
        '
        Me.ParamName.Caption = "参数名"
        Me.ParamName.ColumnName = "参数名"
        Me.ParamName.MaxLength = 255
        '
        'DataType
        '
        Me.DataType.Caption = "数据类型"
        Me.DataType.ColumnName = "数据类型"
        '
        'FieldType
        '
        Me.FieldType.Caption = "字段类型"
        Me.FieldType.ColumnName = "字段类型"
        '
        'FieldSize
        '
        Me.FieldSize.Caption = "字段长度"
        Me.FieldSize.ColumnName = "字段长度"
        Me.FieldSize.DataType = GetType(Integer)
        '
        'ParaDirect
        '
        Me.ParaDirect.Caption = "参数类型"
        Me.ParaDirect.ColumnName = "参数类型"
        '
        'paraTestValue
        '
        Me.paraTestValue.Caption = "参数测试值"
        Me.paraTestValue.ColumnName = "参数测试值"
        '
        'btnOK
        '
        Me.btnOK.Location = New Drawing.Point(553, 9)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New Size(75, 23)
        Me.btnOK.TabIndex = 17
        Me.btnOK.Text = "(&Y)确定"
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = DialogResult.Cancel
        Me.btnCancel.Location = New Drawing.Point(656, 9)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New Size(75, 23)
        Me.btnCancel.TabIndex = 18
        Me.btnCancel.Text = "(&C)取消"
        '
        'cmbDataType
        '
        Me.cmbDataType.Location = New Drawing.Point(208, 233)
        Me.cmbDataType.Name = "cmbDataType"
        Me.cmbDataType.Size = New Size(66, 20)
        Me.cmbDataType.TabIndex = 19
        Me.cmbDataType.Text = "DataType"
        '
        'cmbDBType
        '
        Me.cmbDBType.Location = New Drawing.Point(280, 233)
        Me.cmbDBType.Name = "cmbDBType"
        Me.cmbDBType.Size = New Size(75, 20)
        Me.cmbDBType.TabIndex = 20
        Me.cmbDBType.Text = "DBType"
        '
        'cmbParaDirect
        '
        Me.cmbParaDirect.Location = New Drawing.Point(424, 233)
        Me.cmbParaDirect.Name = "cmbParaDirect"
        Me.cmbParaDirect.Size = New Size(80, 20)
        Me.cmbParaDirect.TabIndex = 21
        Me.cmbParaDirect.Text = "Direction"
        '
        'btnQeuryTest
        '
        Me.btnQeuryTest.Location = New Drawing.Point(208, 8)
        Me.btnQeuryTest.Name = "btnQeuryTest"
        Me.btnQeuryTest.Size = New Size(92, 25)
        Me.btnQeuryTest.TabIndex = 22
        Me.btnQeuryTest.Text = "(&Q)查询测试"
        '
        'DataGrid2
        '
        Me.DataGrid2.CaptionText = "查询结果"
        Me.DataGrid2.DataMember = ""
        Me.DataGrid2.HeaderForeColor = SystemColors.ControlText
        Me.DataGrid2.Location = New Drawing.Point(16, 363)
        Me.DataGrid2.Name = "DataGrid2"
        Me.DataGrid2.Size = New Size(715, 176)
        Me.DataGrid2.TabIndex = 23
        '
        'frmParamsBuilder
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleBaseSize = New Size(6, 14)
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New Size(746, 551)
        Me.Controls.Add(Me.DataGrid2)
        Me.Controls.Add(Me.btnQeuryTest)
        Me.Controls.Add(Me.cmbParaDirect)
        Me.Controls.Add(Me.cmbDBType)
        Me.Controls.Add(Me.cmbDataType)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.DataGrid1)
        Me.Controls.Add(Me.btnParse)
        Me.Controls.Add(Me.txtSQL)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "frmParamsBuilder"
        Me.Text = "参数生成器"
        CType(Me.DataGrid1, ISupportInitialize).EndInit()
        CType(Me.DataSet1, ISupportInitialize).EndInit()
        CType(Me.dtParams, ISupportInitialize).EndInit()
        CType(Me.DataGrid2, ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

#End Region

    Private _SqlMapScript As String
    Private _CommandType As String
    Private _QueryType As String

    ''' <summary>
    '''     数据库类型
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DataBaseType As DBMSType


    Public Property SqlMapScript As String
        Set
            _SqlMapScript = Value
            '进行参数初始化
            Dim sqlMap As New SqlMapper
            Dim connStr As String = ""
            sqlMap.DataBase = MyDB.GetDBHelper(Me.DataBaseType, connStr)
            'sqlMap.DataBase.DataBaseType = Me.DataBaseType

            sqlMap.GetScriptInfo(_SqlMapScript)
            Dim ParasScript As List(Of String) = sqlMap.ParamsScript
            For I As Integer = 0 To ParasScript.Count - 1
                Dim tempArr() As String = Split(ParasScript(I), ":")
                tempArr(0) = "@" + tempArr(0)
                If tempArr.Length > 1 Then
                    Dim tempArr2() As String = tempArr(1).Split(",")
                    Select Case tempArr2.Length
                        Case 1
                            AddParameterData(tempArr(0), tempArr2(0))
                        Case 2
                            AddParameterData(tempArr(0), tempArr2(0), tempArr2(1))
                        Case 3
                            AddParameterData(tempArr(0), tempArr2(0), tempArr2(1), Int32.Parse(tempArr2(2)))
                        Case 4
                            AddParameterData(tempArr(0), tempArr2(0), tempArr2(1), Int32.Parse(tempArr2(2)), tempArr2(3))
                    End Select
                Else
                    AddParameterData(tempArr(0))
                End If
            Next
        End Set
        Get
            Return _SqlMapScript
        End Get
    End Property

    Public Property SqlText As String
        Get
            Return Me.txtSQL.Text
        End Get
        Set
            Me.txtSQL.Text = Value
        End Set
    End Property

    Public Property CommandType As String
        Get
            Return _CommandType
        End Get
        Set
            _CommandType = Value
        End Set
    End Property

    Public Property QueryType As String
        Get
            Return _QueryType
        End Get
        Set
            _QueryType = Value
        End Set
    End Property


    Private Sub frmParamsBuilder_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadSelectedType()

        'AddParameterData("@aaa", "String")
        'AddParameterData("@bbb", "Int32", "Vachar", 32)
    End Sub

    Private Sub LoadSelectedType()

        Dim ArrTemp() As String = [Enum].GetNames(GetType(TypeCode))
        ReDim Preserve ArrTemp(ArrTemp.Length)
        ArrTemp(ArrTemp.Length - 1) = "<自动>"
        Me.cmbDataType.DataSource = ArrTemp
        Me.cmbDataType.Visible = False

        ArrTemp = [Enum].GetNames(GetType(DbType))
        ReDim Preserve ArrTemp(ArrTemp.Length)
        ArrTemp(ArrTemp.Length - 1) = "<自动>"
        Me.cmbDBType.DataSource = ArrTemp
        Me.cmbDBType.Visible = False
        ArrTemp = [Enum].GetNames(GetType(ParameterDirection))
        ReDim Preserve ArrTemp(ArrTemp.Length)
        ArrTemp(ArrTemp.Length - 1) = "<自动>"
        Me.cmbParaDirect.DataSource = ArrTemp
        Me.cmbParaDirect.Visible = False
    End Sub

    Sub AddParameterData(sParamName As String, Optional ByVal sDataType As String = "",
                         Optional ByVal sFieldType As String = "", Optional ByVal iSize As Integer = 0,
                         Optional ByVal sParamDirect As String = "")
        '除sParamName　外，空　表示未定义
        If sParamName = "" Then Exit Sub
        Dim dr As DataRow = Me.dtParams.NewRow()
        dr(1) = sParamName
        If sDataType <> "" Then dr(2) = sDataType
        If sFieldType <> "" Then dr(3) = sFieldType
        If iSize <> 0 Then dr(4) = iSize
        If sParamDirect <> "" Then dr(5) = sParamDirect
        Me.dtParams.Rows.Add(dr)
    End Sub

    Private Sub DataGrid1_CurrentCellChanged(sender As Object, e As EventArgs) Handles DataGrid1.CurrentCellChanged
        Select Case DataGrid1.CurrentCell.ColumnNumber
            Case 2
                ShowSelectedComb(Me.cmbDataType)
                Me.cmbDBType.Visible = False
                Me.cmbParaDirect.Visible = False
            Case 3
                ShowSelectedComb(Me.cmbDBType)
                Me.cmbDataType.Visible = False
                Me.cmbParaDirect.Visible = False
            Case 5
                ShowSelectedComb(Me.cmbParaDirect)
                Me.cmbDataType.Visible = False
                Me.cmbDBType.Visible = False
            Case Else
                Me.cmbDataType.Visible = False
                Me.cmbDBType.Visible = False
                Me.cmbParaDirect.Visible = False
        End Select
    End Sub

    Private Sub ShowSelectedComb(myCmbCtr As ComboBox)
        myCmbCtr.Width = DataGrid1.GetCurrentCellBounds.Width
        myCmbCtr.Left = DataGrid1.Left + DataGrid1.GetCurrentCellBounds.Left
        myCmbCtr.Top = DataGrid1.Top + DataGrid1.GetCurrentCellBounds.Top
        myCmbCtr.Text = IIf(DataGrid1.Item(DataGrid1.CurrentCell) Is DBNull.Value, "",
                            DataGrid1.Item(DataGrid1.CurrentCell))
        myCmbCtr.Visible = True
    End Sub

    Private Sub DataGrid1_Scroll(sender As Object, e As EventArgs) Handles DataGrid1.Scroll
        Me.cmbDataType.Visible = False
        Me.cmbDBType.Visible = False
        Me.cmbParaDirect.Visible = False
    End Sub

    Private Sub cmbDataType_TextChanged(sender As Object, e As EventArgs) Handles cmbDataType.TextChanged
        If DataGrid1.CurrentRowIndex <> - 1 And DataGrid1.CurrentCell.ColumnNumber = 2 Then
            DataGrid1.Item(DataGrid1.CurrentCell) = cmbDataType.SelectedItem
        End If
    End Sub

    Private Sub cmbDBType_TextChanged(sender As Object, e As EventArgs) Handles cmbDBType.TextChanged
        If DataGrid1.CurrentRowIndex <> - 1 And DataGrid1.CurrentCell.ColumnNumber = 3 Then
            DataGrid1.Item(DataGrid1.CurrentCell) = cmbDBType.SelectedItem
        End If
    End Sub

    Private Sub cmbParaDirect_TextChanged(sender As Object, e As EventArgs) Handles cmbParaDirect.TextChanged
        If DataGrid1.CurrentRowIndex <> - 1 And DataGrid1.CurrentCell.ColumnNumber = 5 Then
            DataGrid1.Item(DataGrid1.CurrentCell) = cmbParaDirect.SelectedItem
        End If
    End Sub

    Private Sub btnParse_Click(sender As Object, e As EventArgs) Handles btnParse.Click
        Dim SqlParaNames As ArrayList = GetSqlParameterNames(txtSQL.Text)
        Dim ParamExists As Boolean
        '参数不存在方可添加
        For I As Integer = 0 To SqlParaNames.Count - 1
            ParamExists = False
            For Each dr As DataRow In Me.dtParams.Rows
                If dr(1) = SqlParaNames(I) Then
                    ParamExists = True
                    Exit For
                End If
            Next
            If Not ParamExists Then AddParameterData(SqlParaNames(I))
        Next
    End Sub

    Private Function GetSqlParameterNames(SQL As String) As ArrayList
        'Dim SQL As String = txtSQL.Text
        Dim ArrTemp As New ArrayList
        Dim strTemp As String
        Dim S, M, L, N, K, At As Integer
        Dim Length As Integer = SQL.Length

        S = 1
        Do
            L = InStr(S, SQL, "@", CompareMethod.Text)
            If L = 0 Then Exit Do
            M = InStr(L + 1, SQL, ",", CompareMethod.Text)
            N = InStr(L + 1, SQL, " ", CompareMethod.Text)
            K = InStr(L + 1, SQL, ")", CompareMethod.Text)

            If M < N And M <> 0 Then
                At = M
            ElseIf N <> 0 Then
                At = N
            Else
                At = K
            End If
            If At = 0 Then
                strTemp = Mid(SQL, L)
                ArrTemp.Add(strTemp)
                Exit Do
            Else
                strTemp = Mid(SQL, L, At - L)
                ArrTemp.Add(strTemp)
            End If
            S = At + 1
        Loop Until S > Length
        Return ArrTemp
    End Function

    Private Function GetSqlMapScript(SQL As String) As String
        'Dim SQL As String = Me.txtSQL.Text
        Dim Paraname As String
        Dim ParamMapString As String
        For I As Integer = 0 To dtParams.Rows.Count - 1
            With dtParams.Rows(I)
                Paraname = .Item(1)
                If Paraname <> "" Then
                    ParamMapString = Mid(Paraname, 2)
                    If Not .Item(2) Is DBNull.Value AndAlso .Item(2) <> "<自动>" Then
                        ParamMapString &= ":" & .Item(2)
                        If Not .Item(3) Is DBNull.Value AndAlso .Item(3) <> "<自动>" Then
                            ParamMapString &= "," & .Item(3)
                            If Not .Item(4) Is DBNull.Value AndAlso .Item(4) <> 0 Then
                                ParamMapString &= "," & .Item(4)
                                If Not .Item(5) Is DBNull.Value AndAlso .Item(5) <> "<自动>" Then
                                    ParamMapString &= "," & .Item(5)
                                End If
                            End If
                        End If
                    End If
                    ParamMapString = "#" & ParamMapString & "#"
                    SQL = Replace(SQL, Paraname, ParamMapString)
                End If
            End With
        Next
        Return SQL
    End Function

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        _SqlMapScript = GetSqlMapScript(Me.txtSQL.Text)
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub DataGrid1_Click(sender As Object, e As EventArgs) Handles DataGrid1.Click
        Me.cmbDataType.Visible = False
        Me.cmbDBType.Visible = False
        Me.cmbParaDirect.Visible = False
    End Sub

    Private Sub btnQeuryTest_Click(sender As Object, e As EventArgs) Handles btnQeuryTest.Click
        If CurrDataBase Is Nothing Then
            MsgBox("请先设置数据库连接（在主界面单击配置按钮）", MsgBoxStyle.Exclamation, "查询测试")
            Exit Sub
        End If
        Dim queryType As enumQueryType = [Enum].Parse(GetType(enumQueryType), Me.QueryType)
        Dim commandType As CommandType = [Enum].Parse(GetType(CommandType), Me.CommandType)
        _SqlMapScript = GetSqlMapScript(Me.txtSQL.Text)

        Dim sqlMap As New SqlMapper
        sqlMap.DataBase = CurrDataBase

        Dim cmdInfo As CommandInfo = sqlMap.GetCommandInfoBySqlMapScript(_SqlMapScript)
        Dim sql As String = cmdInfo.CommandText '  sqlMap.GetScriptInfo(_SqlMapScript) '必须先分析脚本信息
        Dim params() As IDataParameter = cmdInfo.DataParameters ' sqlMap.GetParameters(_SqlMapScript)


        For I As Integer = 0 To params.Length - 1
            Dim strValue As Object = dtParams.Rows(I)(6)
            If strValue Is DBNull.Value Then
                MsgBox("参数[" & params(I).ParameterName & "] 未赋值！", MsgBoxStyle.Critical, "查询测试")
                Exit Sub
            End If
            params(I).Value = strValue
        Next


        For Each RP As String In sqlMap.ParamsReplaceable
            For Each dr As DataRow In dtParams.Rows
                If CStr(dr(1)) = "@%" & RP & "%" Then
                    If dr(6) Is DBNull.Value Then
                        MsgBox("替换参数 " & dr(1) & "未赋值！", MsgBoxStyle.Critical, "查询测试")
                        Exit Sub
                    End If
                    '执行参数替换
                    ' sqlMap.SetParameterValue(RP, dr(6), enumParamType.ReplacedText)
                    cmdInfo.SetParameterValue(RP, dr(6), enumParamType.ReplacedText)
                    Exit For
                End If
            Next
        Next

        Dim commandText As String = cmdInfo.CommandText  'sqlMap.CommandText
        DataGrid2.DataSource = Nothing
        Try
            Select Case queryType
                Case enumQueryType.Select
                    Dim ds As DataSet = CurrDataBase.ExecuteDataSet(CurrDataBase.ConnectionString, commandType,
                                                                    commandText, params)
                    DataGrid2.DataSource = ds.Tables(0)
                    MsgBox("查询数据集成功！")
                    If ds.Tables.Count > 1 Then
                        For J As Integer = 1 To ds.Tables.Count - 1
                            If MsgBox("查看下一个数据集吗？", MsgBoxStyle.YesNo, "查看数据集") = MsgBoxResult.Yes Then
                                DataGrid2.DataSource = ds.Tables(J)
                            End If
                        Next
                    End If
                Case enumQueryType.Delete, enumQueryType.Update
                    Dim count As Integer = CurrDataBase.ExecuteNonQuery(CurrDataBase.ConnectionString, commandType,
                                                                        commandText, params)
                    If count > 0 Then
                        MsgBox("命令执行成功，本次执行受影响的行数：" + CStr(count))
                    End If
                Case Else
                    MsgBox("不支持类型的查询！")
            End Select
        Catch ex As Exception
            MsgBox("查询失败：" + ex.Message + vbCrLf + "请先用参数分析检查是否还有替换参数。", MsgBoxStyle.Critical, "查询测试")
        End Try
    End Sub
End Class
