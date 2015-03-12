Imports PWMIS.DataProvider.Adapter
Imports PWMIS.DataProvider.Data


Public Class frmDataBaseExpert

    Dim _currDataBase As AdoHelper = Nothing
    Dim CurrDBName As String
    Dim oldSqlConnStr As String
    Dim needRefresh As Boolean
    Dim CurrDbType As String
    Dim dbLeftChar As String = "["
    Dim dbRightChar As String = "]" '其它数据库必须处理
    Dim DBNodePath As Dictionary(Of String, AdoHelper) = New Dictionary(Of String, AdoHelper)
    Dim OldConnStrNodePath As Dictionary(Of String, String) = New Dictionary(Of String, String)
    Dim dbNodePathKey As String = ""

    Private Property CurrDataBase() As AdoHelper
        Get
            '从当前树选择的路径来获取指定的数据访问对象
            If Me.TreeView1.SelectedNode IsNot Nothing Then
                Dim selectPath As String = Me.TreeView1.SelectedNode.FullPath
                For Each key As String In DBNodePath.Keys
                    If InStr(selectPath, key) > 0 Then
                        _currDataBase = DBNodePath(key)
                        oldSqlConnStr = OldConnStrNodePath(key)
                        dbNodePathKey = key
                        Exit For
                    End If
                Next
            End If
            Module1.CurrentDataBase = _currDataBase
            Return _currDataBase
        End Get
        Set(ByVal value As AdoHelper)
            _currDataBase = value
        End Set
    End Property

    ''' <summary>
    ''' 命令窗体
    ''' </summary>
    ''' <remarks></remarks>
    Public CommandForm As ICommand

    Public Sub New()

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。

    End Sub

    Public Sub New(ByVal commandForm As ICommand)
        InitializeComponent()
        Me.CommandForm = commandForm


    End Sub


    Private Sub initTreeView()
        Me.TreeView1.Nodes(0).Nodes.Clear()

        Dim doc As New Xml.XmlDocument()
        doc.Load("Config\DataConnectionCfg.xml")
        Dim groups As Xml.XmlNodeList = doc.GetElementsByTagName("Group")
        For Each group As Xml.XmlNode In groups
            Dim groupName As String = group.Attributes("Name").Value

            Dim groupNode As TreeNode = Me.TreeView1.Nodes(0).Nodes.Add(groupName)
            groupNode.ImageKey = "ConnStrSection"
            groupNode.SelectedImageKey = "ConnStrSection"

            For Each conn As Xml.XmlNode In group
                If conn.Name = "Connection" Then
                    Dim connNode As New TreeNode()
                    connNode.SelectedImageKey = "SQLHost"
                    connNode.ImageKey = "SQLHost"
                    connNode.Name = conn.Attributes("DbType").Value
                    connNode.Text = conn.Attributes("DbType").Value & ":" & conn.Attributes("Name").Value
                    connNode.ToolTipText = conn.Attributes("ConnectionString").Value
                    If conn.Attributes("Provider") Is Nothing OrElse conn.Attributes("Provider").Value = "" Then
                        connNode.Tag = conn.Attributes("DbType").Value
                    Else
                        connNode.Tag = conn.Attributes("Provider").Value '以逗号区分是否是Provider
                    End If


                    connNode.Nodes.Add(New TreeNode())

                    groupNode.Nodes.Add(connNode)

                End If
            Next

        Next
    End Sub

    Private Sub frmDataBaseExpert_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.ImageList1.Images.Add("CfgNode", Image.FromFile(".\images\ConfigurationNode.bmp"))
        Me.ImageList1.Images.Add("ConnStrSection", Image.FromFile(".\images\ConnectionStringsSectionNode.bmp"))
        Me.ImageList1.Images.Add("ConnStrSettings", Image.FromFile(".\images\ConnectionStringSettingsNode.bmp"))
        Me.ImageList1.Images.Add("DBSection", Image.FromFile(".\images\DatabaseSectionNode.bmp"))
        Me.ImageList1.Images.Add("SQLHost", Image.FromFile(".\images\SQLHost.bmp"))
        Me.ImageList1.Images.Add("ProviderMap", Image.FromFile(".\images\ProviderMappingNode.bmp"))
        Me.ImageList1.Images.Add("Tables", Image.FromFile(".\images\customizing.bmp"))
        Me.ImageList1.Images.Add("Table", Image.FromFile(".\images\show_columns.bmp"))
        Me.ImageList1.Images.Add("Functions", Image.FromFile(".\images\AppSettingsNode.bmp"))
        Me.ImageList1.Images.Add("Function", Image.FromFile(".\images\AppSettingNode.bmp"))
        Me.ImageList1.Images.Add("Element", Image.FromFile(".\images\OraclePackageElementNode.bmp"))

        Me.TreeView1.ImageList = Me.ImageList1
        'Me.TreeView1.ImageKey = "CfgNode"

        Me.TreeView1.Nodes(0).ImageKey = "ConnStrSection"
        Me.TreeView1.Nodes(0).SelectedImageKey = "ConnStrSection"
        initTreeView()

    End Sub

    Private Sub tsmiProperty_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiProperty.Click
        Dim paras As New Dictionary(Of String, Object)
        paras.Add("key1", "aaa")
        paras.Add("key2", "bbb")

        CommandForm.Command("ShowPropertyForm", paras)

    End Sub

    Private Sub TreeView1_AfterExpand(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterExpand
        Me.Cursor = Cursors.WaitCursor
        TreeView1.SelectedNode = e.Node

        Select Case e.Node.Level
            Case 2
                If e.Node.FirstNode.Text <> Nothing And Not needRefresh Then Exit Select
                '数据库节点
                Dim connStr As String = e.Node.ToolTipText
                Dim temp As String = e.Node.Tag
                Dim conn As AdoHelper = Nothing

                If temp = "" Then Exit Select

                Dim at As Integer = InStr(temp, ",")

                If at > 1 Then
                    Dim assemblyType As String = Microsoft.VisualBasic.Left(temp, at - 1)
                    Dim assemblyFile As String = Mid(temp, at + 1)
                    conn = MyDB.GetDBHelper(assemblyFile, assemblyType, connStr)
                Else
                    conn = MyDB.GetDBHelper(temp, connStr)
                End If
                Me.CurrDataBase = conn

                Me.CurrDbType = e.Node.Name
                '将当前数据库对象加入到路径缓存中
                Me.DBNodePath(e.Node.FullPath) = conn
                Me.OldConnStrNodePath(e.Node.FullPath) = connStr

                If Me.CurrDbType = "Oracle" Then
                    dbLeftChar = """"
                    dbRightChar = """"
                ElseIf Me.CurrDbType = "OleDb" Or Me.CurrDbType = "Odbc" Then
                    dbLeftChar = ""
                    dbRightChar = ""
                Else
                    dbLeftChar = "["
                    dbRightChar = "]"
                End If

                'oldSqlConnStr = connStr

                Try
                    Dim dtSchema As DataTable = conn.GetSchema("Databases", Nothing)
                    Dim dabaseView As DataView = dtSchema.DefaultView
                    If dtSchema.Columns.Contains("dbid") Then
                        dabaseView.Sort = "dbid"
                    End If

                    e.Node.Nodes.Clear()
                    For Each item As DataRowView In dabaseView
                        Dim node As New TreeNode(item("database_name")) 'item(0)
                        node.ImageKey = "DBSection"
                        node.SelectedImageKey = "ProviderMap"

                        node.Name = item("database_name")
                        node.ToolTipText = "创建时间:" & item(2) 'MySQL 不是创建时间
                        e.Node.Nodes.Add(node) '添加数据库名称节点

                        node.Nodes.Add(New TreeNode("表"))
                        node.Nodes.Add(New TreeNode("视图"))
                        node.Nodes.Add(New TreeNode("存储过程"))
                        node.Nodes.Add(New TreeNode("函数"))

                        node.Nodes(0).SelectedImageKey = "Tables"
                        node.Nodes(0).ImageKey = "Tables"
                        node.Nodes(1).SelectedImageKey = "Tables"
                        node.Nodes(1).ImageKey = "Tables"
                        node.Nodes(2).SelectedImageKey = "Functions"
                        node.Nodes(2).ImageKey = "Functions"
                        node.Nodes(3).SelectedImageKey = "Functions"
                        node.Nodes(3).ImageKey = "Functions"
                    Next
                Catch ex As Exception
                    '可能有些数据库不支持 Databases 元数据
                    e.Node.Nodes.Clear()
                    If Me.CurrDbType = "Oracle" Then
                        Dim node As New TreeNode("数据库")
                        node.Name = "main"
                        e.Node.Nodes.Add(node) '添加数据库名称节点
                        node.Nodes.Add(New TreeNode("表"))
                        node.Nodes.Add(New TreeNode("视图"))

                        node.Nodes(0).SelectedImageKey = "Tables"
                        node.Nodes(0).ImageKey = "Tables"
                        node.Nodes(1).SelectedImageKey = "Views"
                        node.Nodes(1).ImageKey = "Views"
                   ElseIf Me.CurrDbType = "SQLite" Or Me.CurrDbType = "Access" Then
                        Dim node As New TreeNode("数据库")
                        node.Name = "main"
                        e.Node.Nodes.Add(node) '添加数据库名称节点
                        node.Nodes.Add(New TreeNode("表"))
                        node.Nodes.Add(New TreeNode("视图"))

                        node.Nodes(0).SelectedImageKey = "Tables"
                        node.Nodes(0).ImageKey = "Tables"
                        node.Nodes(1).SelectedImageKey = "Tables"
                        node.Nodes(1).ImageKey = "Tables"
                    ElseIf Me.CurrDbType = "SQLServerCe" Then
                        Dim node As New TreeNode("数据库")
                        node.Name = ""
                        e.Node.Nodes.Add(node) '添加数据库名称节点
                        node.Nodes.Add(New TreeNode("表"))
                        node.Nodes(0).SelectedImageKey = "Tables"
                        node.Nodes(0).ImageKey = "Tables"

                    End If
                End Try


                needRefresh = False
            Case 3
                '显示所有表
                'If e.Node.FirstNode.Text <> Nothing Then Exit Select
                Dim node As TreeNode = e.Node
                Me.CurrDBName = node.Name

                If Me.CurrDataBase.CurrentDBMSType = PWMIS.Common.DBMSType.SqlServer _
                Or Me.CurrDataBase.CurrentDBMSType = PWMIS.Common.DBMSType.MySql _
                Or Me.CurrDataBase.CurrentDBMSType = PWMIS.Common.DBMSType.PostgreSQL Then
                    Me.CurrDataBase.ConnectionString = oldSqlConnStr & ";DataBase=" & node.Name '直接使用 oldSqlConnStr 可能会出错
                End If
                Dim nodeTable As TreeNode = node.Nodes(0)
                If nodeTable.Nodes.Count > 0 And Not needRefresh Then Exit Select '不再重复构建树
                Dim dbName As String = node.Name
                Dim tableFilter As String = "BASE TABLE"

                If Me.CurrDbType = "SQLite" Then tableFilter = "table" '
                If Me.CurrDbType = "SQLServerCe" Then
                    dbName = Nothing
                    tableFilter = "TABLE"
                End If
                If Me.CurrDbType = "Access" Then
                    dbName = Nothing
                    tableFilter = "TABLE"
                End If
                Dim dtSchema As DataTable '= Me.CurrDataBase.GetSchema("Tables", New String() {dbName, Nothing, Nothing, tableFilter})
                If Me.CurrDataBase.CurrentDBMSType = PWMIS.Common.DBMSType.MySql Then
                    dtSchema = Me.CurrDataBase.GetSchema("Tables", New String() {Nothing, dbName, Nothing, tableFilter})
                ElseIf Me.CurrDataBase.CurrentDBMSType = PWMIS.Common.DBMSType.Oracle Then
                    dtSchema = Me.CurrDataBase.GetSchema("Tables", Nothing)
                Else
                    dtSchema = Me.CurrDataBase.GetSchema("Tables", New String() {dbName, Nothing, Nothing, tableFilter})
                End If
                Dim tableView As DataView = dtSchema.DefaultView

                nodeTable.Nodes.Clear()

                If Me.CurrDataBase.CurrentDBMSType = PWMIS.Common.DBMSType.Oracle Then
                    Dim owner As String = Me.CurrDataBase.ConnectionUserID
                    Dim dvCurr As New DataView(dtSchema, "OWNER='" & owner & "'", "table_name", DataViewRowState.CurrentRows)
                    For Each item As DataRowView In dvCurr
                        Dim childNode As New TreeNode(item(1))
                        childNode.Name = item(1)
                        childNode.SelectedImageKey = "Table"
                        childNode.ImageKey = "Table"

                        Dim nodeColumn As New TreeNode("列")
                        nodeColumn.Name = "表列"
                        childNode.Nodes.Add(nodeColumn)
                        childNode.Nodes.Add(New TreeNode("索引"))
                        childNode.Nodes.Add(New TreeNode("触发器"))

                        nodeTable.Nodes.Add(childNode)

                    Next

                    ''下面的代码列出的表没法正确获取到列信息且表名重复，暂时屏蔽
                    'Dim dvOther As New DataView(dtSchema, "OWNER='" & owner & "'", "owner,table_name", DataViewRowState.CurrentRows)
                    'For Each item As DataRowView In dvOther
                    '    Dim childNode As New TreeNode(item(0) & "." & item(1))
                    '    childNode.Name = item(0) & Chr(34) & "." & Chr(34) & item(1)
                    '    childNode.SelectedImageKey = "Table"
                    '    childNode.ImageKey = "Table"

                    '    Dim nodeColumn As New TreeNode("列")
                    '    nodeColumn.Name = "表列"
                    '    childNode.Nodes.Add(nodeColumn)
                    '    childNode.Nodes.Add(New TreeNode("索引"))
                    '    childNode.Nodes.Add(New TreeNode("触发器"))

                    '    nodeTable.Nodes.Add(childNode)

                    'Next

                Else
                    tableView.Sort = "table_schema,TABLE_NAME"
                    For Each item As DataRowView In tableView
                        Dim childNode As New TreeNode(item(1) & "." & item(2))
                        childNode.Name = item(2)
                        childNode.SelectedImageKey = "Table"
                        childNode.ImageKey = "Table"

                        Dim nodeColumn As New TreeNode("列")
                        nodeColumn.Name = "表列"
                        childNode.Nodes.Add(nodeColumn)
                        childNode.Nodes.Add(New TreeNode("索引"))
                        childNode.Nodes.Add(New TreeNode("触发器"))

                        nodeTable.Nodes.Add(childNode)

                    Next

                End If


                
                If Me.CurrDbType = "SQLServerCe" Then Exit Select 'SQLCE 不支持视图、存储过程

                '显示所有视图
                Dim nodeView As TreeNode = node.Nodes(1)
                dtSchema = Me.CurrDataBase.GetSchema("Views", Nothing) ' , New String() {node.Name, Nothing, Nothing}
                tableView = dtSchema.DefaultView

                nodeView.Nodes.Clear()
                If Me.CurrDataBase.CurrentDBMSType = PWMIS.Common.DBMSType.Oracle Then
                    'owner view_name text_length text type_text_length type_text oid_text_length oid_text view_type_owner view_type 
                    'tableView.Sort = "owner,view_name"

                    Dim owner As String = Me.CurrDataBase.ConnectionUserID
                    Dim dvCurr As New DataView(dtSchema, "OWNER='" & owner & "'", "owner,view_name", DataViewRowState.CurrentRows)

                    For Each item As DataRowView In dvCurr
                        Dim childNode As New TreeNode(item(0) & "." & item(1))
                        childNode.Name = item(1)
                        childNode.SelectedImageKey = "Table"
                        childNode.ImageKey = "Table"

                        Dim nodeColumn As New TreeNode("列")
                        nodeColumn.Name = "视图列"


                        Dim nodeColumnS As New TreeNode("源表列")
                        nodeColumnS.Name = "原始视图列"

                        childNode.Nodes.Add(nodeColumn)
                        childNode.Nodes.Add(nodeColumnS)
                        nodeView.Nodes.Add(childNode)

                    Next

                Else
                    tableView.Sort = "table_schema,table_name"
                    For Each item As DataRowView In tableView
                        Dim childNode As New TreeNode(item(1) & "." & item(2))
                        childNode.Name = item(2)
                        childNode.SelectedImageKey = "Table"
                        childNode.ImageKey = "Table"

                        Dim nodeColumn As New TreeNode("列")
                        nodeColumn.Name = "视图列"


                        Dim nodeColumnS As New TreeNode("源表列")
                        nodeColumnS.Name = "原始视图列"

                        childNode.Nodes.Add(nodeColumn)
                        childNode.Nodes.Add(nodeColumnS)
                        nodeView.Nodes.Add(childNode)

                    Next
                End If


                

                If node.Nodes.Count < 3 Then Exit Select '有些数据库没有存储过程
                'PostgreSQL 没有单独的存储过程，统一成为函数
                If Me.CurrDataBase.CurrentDBMSType <> PWMIS.Common.DBMSType.PostgreSQL Then
                    '显示所有存储过程
                    Dim nodeProc As TreeNode = node.Nodes(2)
                    dtSchema = Me.CurrDataBase.GetSchema("Procedures", New String() {node.Name, Nothing, Nothing}) ' , New String() {node.Name, Nothing, Nothing}
                    tableView = dtSchema.DefaultView
                    tableView.RowFilter = "ROUTINE_TYPE='PROCEDURE'"
                    tableView.Sort = "ROUTINE_NAME"

                    nodeProc.Nodes.Clear()
                    For Each item As DataRowView In tableView
                        Dim childNode As New TreeNode(item("ROUTINE_SCHEMA") & "." & item("ROUTINE_NAME"))
                        childNode.Name = item("ROUTINE_NAME")
                        childNode.SelectedImageKey = "Function"
                        childNode.ImageKey = "Function"

                        Dim temp As New TreeNode("参数") With {.Name = "存储过程参数"}
                        childNode.Nodes.Add(temp)
                        nodeProc.Nodes.Add(childNode)

                    Next

                    '显示所有函数
                    Dim nodeFun As TreeNode = node.Nodes(3)
                    tableView = dtSchema.DefaultView
                    tableView.RowFilter = "ROUTINE_TYPE='FUNCTION'"
                    tableView.Sort = "ROUTINE_NAME"

                    nodeFun.Nodes.Clear()
                    For Each item As DataRowView In tableView
                        Dim childNode As New TreeNode(item("ROUTINE_SCHEMA") & "." & item("ROUTINE_NAME"))
                        childNode.Name = item("ROUTINE_NAME")
                        Dim temp As New TreeNode("参数") With {.Name = "函数参数"}
                        childNode.Nodes.Add(temp)
                        childNode.Nodes.Add(New TreeNode("返回结果"))
                        childNode.SelectedImageKey = "Function"
                        childNode.ImageKey = "Function"

                        nodeFun.Nodes.Add(childNode)

                    Next

                Else
                    Dim nodeProc As TreeNode = node.Nodes(2)
                    dtSchema = Me.CurrDataBase.GetSchema("Procedures", Nothing) ' New String() {node.Name, Nothing, Nothing}

                    Dim nodeFun As TreeNode = node.Nodes(3)
                    tableView = dtSchema.DefaultView
                    'tableView.RowFilter = "ROUTINE_TYPE='FUNCTION'"
                    tableView.Sort = "ROUTINE_NAME"

                    nodeFun.Nodes.Clear()
                    For Each item As DataRowView In tableView
                        Dim childNode As New TreeNode(item("ROUTINE_NAME"))
                        childNode.Name = item("ROUTINE_NAME")
                        Dim temp As New TreeNode("参数") With {.Name = "函数参数"}
                        childNode.Nodes.Add(temp)
                        childNode.Nodes.Add(New TreeNode("返回结果"))
                        childNode.SelectedImageKey = "Function"
                        childNode.ImageKey = "Function"

                        nodeFun.Nodes.Add(childNode)

                    Next

                End If

                needRefresh = False
            Case 5
                If e.Node.Nodes.Count > 0 Then
                    Select Case e.Node.FirstNode.Name
                        Case "表列"
                            Dim node As TreeNode = e.Node
                            Dim nodeColumns As TreeNode = node.FirstNode
                            If nodeColumns.Nodes.Count > 0 AndAlso Not needRefresh Then Exit Select
                            nodeColumns.Nodes.Clear()

                            Dim tableName As String = node.Name
                            Dim dtSchema As DataTable '= Me.CurrDataBase.GetSchema("Columns", New String() {Nothing, Nothing, tableName})
                            Dim tableView As DataView '= dtSchema.DefaultView
                            
                            If Me.CurrDbType = "Access" Then
                                dtSchema = Me.CurrDataBase.GetSchema("Columns", New String() {Nothing, Nothing, tableName})
                                tableView = dtSchema.DefaultView
                                tableView.Sort = "ordinal_position"

                                Dim dtSchemaDataType As DataTable = Me.CurrDataBase.GetSchema("DataTypes", Nothing)
                                Dim dictTypeName As New Dictionary(Of String, String)
                                Dim index As Integer = dtSchemaDataType.Columns.Count - 1

                                For Each item As DataRow In dtSchemaDataType.Rows
                                    Dim key As String = item(index)
                                    If Not dictTypeName.Keys.Contains(key) Then
                                        dictTypeName.Add(key, item(0))
                                    End If
                                Next

                                For Each item As DataRowView In tableView
                                    Dim text As String = item("COLUMN_NAME") & " [" & dictTypeName(item("DATA_TYPE"))
                                    If item("CHARACTER_MAXIMUM_LENGTH") IsNot DBNull.Value Then
                                        Dim length As Long = item("CHARACTER_MAXIMUM_LENGTH")
                                        'length=0 是备注类型 LongText
                                        If length > 0 And length <= 255 Then
                                            text = item("COLUMN_NAME") & "[VarChar"
                                        End If
                                        text = text & "(" & length & ")]"
                                    Else
                                        text = text & "]"
                                    End If
                                    Dim childNode As New TreeNode(text)
                                    childNode.Name = item("COLUMN_NAME")
                                    childNode.SelectedImageKey = "Element"
                                    childNode.ImageKey = "Element"
                                    nodeColumns.Nodes.Add(childNode)
                                Next
                            ElseIf Me.CurrDbType = "Oracle" Then
                                '注意：应该指名 owner的值，否则可能是另外一个表的结构

                                dtSchema = Me.CurrDataBase.GetSchema("Columns", New String() {Me.CurrDataBase.ConnectionUserID, tableName}) 'owner table_name column_name id datatype length precision scale nullable
                                tableView = dtSchema.DefaultView
                                tableView.Sort = "column_name"

                                For Each item As DataRowView In tableView
                                    Dim text As String = item("COLUMN_NAME") & " [" & item("DATATYPE")
                                    If item("length") IsNot DBNull.Value Then
                                        text = text & "(" & item("length") & ")]"
                                    Else
                                        text = text & "]"
                                    End If
                                    Dim childNode As New TreeNode(text)
                                    childNode.Name = item("COLUMN_NAME")
                                    childNode.SelectedImageKey = "Element"
                                    childNode.ImageKey = "Element"
                                    nodeColumns.Nodes.Add(childNode)
                                Next
                            Else
                                dtSchema = Me.CurrDataBase.GetSchema("Columns", New String() {Nothing, Nothing, tableName})
                                tableView = dtSchema.DefaultView
                                tableView.Sort = "ordinal_position"

                                For Each item As DataRowView In tableView
                                    Dim text As String = item("COLUMN_NAME") & " [" & item("DATA_TYPE")
                                    If item("CHARACTER_MAXIMUM_LENGTH") IsNot DBNull.Value Then
                                        text = text & "(" & item("CHARACTER_MAXIMUM_LENGTH") & ")]"
                                    Else
                                        text = text & "]"
                                    End If
                                    Dim childNode As New TreeNode(text)
                                    childNode.Name = item("COLUMN_NAME")
                                    childNode.SelectedImageKey = "Element"
                                    childNode.ImageKey = "Element"
                                    nodeColumns.Nodes.Add(childNode)
                                Next
                            End If

                        Case "视图列"
                            Dim node As TreeNode = e.Node
                            Dim nodeColumns As TreeNode = node.FirstNode
                            If nodeColumns.Nodes.Count > 0 AndAlso Not needRefresh Then Exit Select
                            nodeColumns.Nodes.Clear()


                            Dim viewName As String = node.Name
                            '对于视图，仍然使用Columns 获取视图的列，ViewColumns 获取的则是各表的原始列
                            Dim dtSchema As DataTable = Me.CurrDataBase.GetSchema("Columns", New String() {Nothing, Nothing, viewName})
                            Dim tableView As DataView = dtSchema.DefaultView
                            tableView.Sort = "COLUMN_NAME"


                            For Each item As DataRowView In tableView
                                Dim text As String = item("TABLE_SCHEMA") & "." & item("TABLE_NAME") & "." & item("COLUMN_NAME")
                                Dim childNode As New TreeNode(text)
                                childNode.Name = item("COLUMN_NAME") ' IIf(item("COLUMN_NAME") Is DBNull.Value, "", item("COLUMN_NAME").ToString())
                                childNode.SelectedImageKey = "Element"
                                childNode.ImageKey = "Element"

                                nodeColumns.Nodes.Add(childNode)
                            Next

                            '"原始视图列"
                            Dim nodeColumns2 As TreeNode = node.Nodes(1)
                            nodeColumns2.Nodes.Clear()

                            'Dim viewName As String = node.Name
                            '对于视图，仍然使用Columns 获取视图的列，ViewColumns 获取的则是各表的原始列
                            dtSchema = Me.CurrDataBase.GetSchema("ViewColumns", New String() {Nothing, Nothing, viewName})
                            tableView = dtSchema.DefaultView
                            tableView.Sort = "COLUMN_NAME"

                            For Each item As DataRowView In tableView
                                Dim text As String = item("TABLE_SCHEMA") & "." & item("TABLE_NAME") & "." & item("COLUMN_NAME")
                                Dim childNode As New TreeNode(text)
                                childNode.Name = item("COLUMN_NAME")
                                childNode.SelectedImageKey = "Element"
                                childNode.ImageKey = "Element"

                                nodeColumns2.Nodes.Add(childNode)
                            Next

                        Case "存储过程参数"
                            Dim node As TreeNode = e.Node
                            Dim nodeParas As TreeNode = node.FirstNode
                            If nodeParas.Nodes.Count > 0 AndAlso Not needRefresh Then Exit Select
                            nodeParas.Nodes.Clear()

                            Dim procName As String = node.Name
                            Dim procedureParameters As String = "ProcedureParameters"
                            If Me.CurrDataBase.CurrentDBMSType = PWMIS.Common.DBMSType.MySql Then
                                procedureParameters = "Procedure Parameters"
                            End If
                            Dim dtSchema As DataTable = Me.CurrDataBase.GetSchema(procedureParameters, New String() {Nothing, Nothing, procName}) '
                            Dim tableView As DataView = dtSchema.DefaultView
                            tableView.Sort = "ordinal_position"


                            For Each item As DataRowView In tableView
                                Dim text As String = item("PARAMETER_NAME") & " [" & item("DATA_TYPE") & "," & item("CHARACTER_MAXIMUM_LENGTH") & "," & item("PARAMETER_MODE") & "]"
                                Dim childNode As New TreeNode(text)
                                childNode.Name = item("PARAMETER_NAME")
                                childNode.SelectedImageKey = "Element"
                                childNode.ImageKey = "Element"

                                nodeParas.Nodes.Add(childNode)
                            Next
                        Case "函数参数"
                            Dim node As TreeNode = e.Node
                            Dim nodeParas As TreeNode = node.FirstNode
                            If nodeParas.Nodes.Count > 0 AndAlso Not needRefresh Then Exit Select
                            nodeParas.Nodes.Clear()

                            Dim funName As String = node.Name
                            Dim dtSchema As DataTable = Nothing
                            Try
                                dtSchema = Me.CurrDataBase.GetSchema("ProcedureParameters", New String() {Nothing, Nothing, funName})
                            Catch ex As Exception
                                If Me.CurrDataBase.CurrentDBMSType = PWMIS.Common.DBMSType.PostgreSQL Then
                                    MessageBox.Show("获取函数参数信息失败，请尝试手工执行下面地址的脚本：http://www.alberton.info/postgresql_meta_info.html ，脚本名称：public.function_args")
                                    Process.Start("http://www.alberton.info/postgresql_meta_info.html")
                                Else
                                    MessageBox.Show("获取函数参数信息失败：" & ex.Message)
                                End If
                                Exit Select
                            End Try

                            Dim tableView As DataView = dtSchema.DefaultView
                            tableView.Sort = "ordinal_position"
                            tableView.RowFilter = "IS_RESULT='NO'"

                            For Each item As DataRowView In tableView
                                Dim text As String = item("PARAMETER_NAME") & " [" & item("DATA_TYPE") & "," & item("CHARACTER_MAXIMUM_LENGTH") & "," & item("PARAMETER_MODE") & "]"
                                Dim childNode As New TreeNode(text)
                                childNode.Name = item("PARAMETER_NAME")
                                childNode.SelectedImageKey = "Element"
                                childNode.ImageKey = "Element"

                                nodeParas.Nodes.Add(childNode)
                            Next

                            Dim nodeReturn As TreeNode = node.Nodes(1)
                            tableView = dtSchema.DefaultView
                            tableView.Sort = "ordinal_position"
                            tableView.RowFilter = "IS_RESULT='YES'"

                            nodeReturn.Nodes.Clear()
                            For Each item As DataRowView In tableView
                                Dim text As String = "return " & item("PARAMETER_NAME") & " [" & item("DATA_TYPE") & "," & item("CHARACTER_MAXIMUM_LENGTH") & "," & item("PARAMETER_MODE") & "]"
                                Dim childNode As New TreeNode(text)
                                childNode.Name = item("PARAMETER_NAME")
                                childNode.SelectedImageKey = "Element"
                                childNode.ImageKey = "Element"

                                nodeReturn.Nodes.Add(childNode)
                            Next

                    End Select

                    needRefresh = False
                End If
        End Select


        Me.Cursor = Cursors.Default
    End Sub

    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click
        Me.needRefresh = True

    End Sub

    ''' <summary>
    ''' 创建实体类，打开实体类生成器窗口
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateEntity() As Boolean
        Dim currNode As TreeNode = Me.TreeView1.SelectedNode
        Dim tables As New List(Of String)
        Dim views As New List(Of String)
        Dim dbName As String = "MyDB"
        If currNode IsNot Nothing Then
            If currNode.Nodes.Count > 0 AndAlso currNode.FirstNode.Text = "列" Then
                '当前节点是表或者视图名称
                Dim tableName As String = currNode.Name
                'MessageBox.Show(tableName)

                If currNode.FirstNode.Name = "表列" Then
                    tables.Add(tableName)
                Else
                    views.Add(tableName)
                End If
                dbName = currNode.Parent.Parent.Name
            ElseIf currNode.Text = "表" Then
                '为所有表生成实体类
                'MessageBox.Show("表/视图" + currNode.Text)

                For Each node As TreeNode In currNode.Nodes
                    tables.Add(node.Name)
                Next
                dbName = currNode.Parent.Name
            ElseIf currNode.Text = "视图" Then
                '为所有视图生成实体类
                'MessageBox.Show("表/视图" + currNode.Text)

                For Each node As TreeNode In currNode.Nodes
                    views.Add(node.Name)
                Next

                dbName = currNode.Parent.Name
            ElseIf currNode.Nodes.Count > 0 AndAlso currNode.FirstNode.Text = "表" Then
                '为当前数据库所有表和视图生成实体类
                'MessageBox.Show("数据库：" + currNode.Text)

                For Each node As TreeNode In currNode.Nodes(0).Nodes
                    tables.Add(node.Name)
                Next


                For Each node As TreeNode In currNode.Nodes(1).Nodes
                    views.Add(node.Name)
                Next
                dbName = currNode.Name
            Else

                Return False
            End If

            Dim window As New frmEntityCreate(Me.CurrDataBase, tables, views)
            window.CurrDBName = dbName
            Me.CommandForm.OpenWindow(Me, window, "")
            Return True
        End If
        Return False
    End Function

    Private Sub tsmiCreateEntity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiCreateEntity.Click
        CreateEntity()
    End Sub

    Private Sub tsmiNewQuery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiNewQuery.Click
        If Me.CurrDataBase Is Nothing Then
            MessageBox.Show("请先打开一个数据库连接", "新建表查询", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If

        Dim currNode As TreeNode = Me.TreeView1.SelectedNode
        Dim SQL As String = ""
        If currNode IsNot Nothing Then
            If currNode.Nodes.Count > 0 AndAlso currNode.FirstNode.Text = "列" Then
                '当前节点是表或者视图名称
                Dim tableName As String = currNode.Name

                Dim columns As String = ""
                For Each node As TreeNode In currNode.FirstNode.Nodes
                    columns &= vbTab & dbLeftChar & node.Name & dbRightChar & " ," & vbCrLf
                Next
                If currNode.FirstNode.Nodes.Count = 0 Then
                    columns = " * "
                Else
                    columns = columns.TrimEnd(",", vbCr, vbLf) & vbCrLf
                End If
                SQL = "SELECT" & columns & " FROM " & dbLeftChar & tableName & dbRightChar & vbCrLf

            End If
        End If

        Dim window As New frmDataQuery(Me.CurrDataBase)
        window.rtbQueryText.Text = SQL
        window.CurrDBName = Me.CurrDBName
        window.CurrDBPath = Me.dbNodePathKey & "-" & Me.CurrDBName
        Me.CommandForm.OpenWindow(Me, window, "")
        window.CommandForm = Me.CommandForm

    End Sub


    Private Sub TreeView1_ItemDrag(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles TreeView1.ItemDrag
        Dim node As TreeNode = e.Item
        TreeView1.DoDragDrop(node.Name, DragDropEffects.Copy Or DragDropEffects.Move)

    End Sub

    Private Sub CreatNewConn()
        If TreeView1.SelectedNode IsNot Nothing Then
            If TreeView1.SelectedNode.Level = 1 Then
                Dim groupName As String = TreeView1.SelectedNode.Text
                Dim dbLogin As New frmDBLogin
                dbLogin.ShowDialog()
                If dbLogin.ConnectionString <> "" And dbLogin.ServerName <> "" Then
                    Dim fileName As String = ".\Config\DataConnectionCfg.xml"
                    Dim dbConnCfg As XElement = XElement.Load(fileName)
                    For Each groupElement As XElement In dbConnCfg.Elements("Group")
                        If groupElement.Attribute("Name") = groupName Then
                            Dim ServerName As String = ""
                            Dim DbType As String = dbLogin.DBMSType.ToString()

                            If dbLogin.DBMSType = PWMIS.Common.DBMSType.UNKNOWN Then
                                DbType = InputBox("请输入数据库类型(注意大小写)。" & vbCrLf & "系统支持的类型有：Access,SqlServer,Oracle,DB2,Sysbase,MySql,SQLite,UNKNOWN", "数据连接管理")
                                If DbType = "SQLite" Then
                                    ServerName = "main"
                                Else
                                    ServerName = InputBox("请输入 服务器/连接 的名称", "数据连接管理")
                                End If

                                If ServerName = "" Then
                                    Exit Sub
                                End If
                            Else
                                ServerName = dbLogin.ServerName
                            End If
                            Dim connElement As XElement = _
                       <Connection DbType=<%= DbType %> Name=<%= ServerName %> ConnectionString=<%= dbLogin.ConnectionString %> Provider=<%= dbLogin.Provider %>/>
                            groupElement.Add(connElement)
                            dbConnCfg.Save(fileName)

                            initTreeView()
                            Exit For
                        End If
                    Next
                Else
                    MessageBox.Show("连接字符串为空，请在[新建连接窗体]中单击[高级选项]查看或者设置！")
                End If
            Else
                MessageBox.Show("请先选择或者创建一个组！")
            End If
        Else
            MessageBox.Show("请先选择或者创建一个组！")
        End If
    End Sub

    Private Sub DeleteSelectedConn()
        If TreeView1.SelectedNode IsNot Nothing Then
            If TreeView1.SelectedNode.Level = 2 Then
                Dim at As Integer = InStr(TreeView1.SelectedNode.Text, ":")
                Dim connName As String = Mid(TreeView1.SelectedNode.Text, at + 1)
                Dim groupName As String = TreeView1.SelectedNode.Parent.Text

                Dim fileName As String = ".\Config\DataConnectionCfg.xml"
                Dim dbConnCfg As XElement = XElement.Load(fileName)
                Dim objGroupElement As XElement = dbConnCfg.<Group>.Where(Function(item As XElement) item.@Name = groupName).FirstOrDefault
                If objGroupElement IsNot Nothing Then
                    Dim objConnElement As XElement = objGroupElement.Elements _
                                                     .Where(Function(item As XElement) item.@Name = connName).FirstOrDefault
                    If objConnElement IsNot Nothing Then
                        If MessageBox.Show("确认删除当前连接 [" & connName & "] 吗？", "数据连接管理", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.OK Then
                            objConnElement.Remove()
                            dbConnCfg.Save(fileName)

                            initTreeView()
                        End If
                    End If
                End If
            Else
                MessageBox.Show("请先选择一个连接！")
            End If
        Else
            MessageBox.Show("请先选择一个连接！")
        End If
    End Sub

    Private Sub tsbNewConn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbNewConn.Click
        CreatNewConn()

    End Sub

    Private Sub tsmItemNewConn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmItemNewConn.Click
        CreatNewConn()
    End Sub

    Private Sub ContextMenuStrip1_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening
        If TreeView1.SelectedNode IsNot Nothing Then
            If TreeView1.SelectedNode.Level = 0 Or TreeView1.SelectedNode.Level = 1 Then
                tsmItemDeleGroup.Enabled = True
                tsmItemNewGroup.Enabled = True
                tsmItemEditGroup.Enabled = True
            Else
                tsmItemDeleGroup.Enabled = False
                tsmItemNewGroup.Enabled = False
                tsmItemEditGroup.Enabled = False
            End If
            If TreeView1.SelectedNode.Level = 1 Or TreeView1.SelectedNode.Level = 2 Then
                tsmItemNewConn.Enabled = True
                tsmItemCloseConn.Enabled = True
                tsmItemDeleConn.Enabled = True
            Else
                tsmItemNewConn.Enabled = False
                tsmItemCloseConn.Enabled = False
                tsmItemDeleConn.Enabled = False
            End If
        End If
    End Sub

    Private Sub tsmItemDeleConn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmItemDeleConn.Click
        DeleteSelectedConn()



    End Sub

    Private Sub tsmItemDeleGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmItemDeleGroup.Click
        Dim groupName As String = TreeView1.SelectedNode.Text
        Dim fileName As String = ".\Config\DataConnectionCfg.xml"
        Dim dbConnCfg As XElement = XElement.Load(fileName)
        Dim objElement = dbConnCfg.<Group>.Where(Function(item As XElement) item.@Name = groupName).FirstOrDefault
        If objElement IsNot Nothing Then
            If MessageBox.Show("确认删除当前分组 [" & groupName & "] 吗？如果删除那么下面的所有节点都将删除！", "分组管理", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.OK Then
                objElement.Remove()
                dbConnCfg.Save(fileName)

                initTreeView()
            End If
        End If

    End Sub

    Private Sub tsmItemNewGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmItemNewGroup.Click
        Dim newGroupName As String = InputBox("请输入新建的分组名称：", "分组管理", "新建分组1")
        If newGroupName <> "" Then
            Dim fileName As String = ".\Config\DataConnectionCfg.xml"
            Dim dbConnCfg As XElement = XElement.Load(fileName)
            Dim newGroupElement As XElement = <Group Name=<%= newGroupName %>/>
            dbConnCfg.Add(newGroupElement)
            dbConnCfg.Save(fileName)

            initTreeView()
        End If
    End Sub

    Private Sub tsmItemEditGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmItemEditGroup.Click
        Dim groupName As String = TreeView1.SelectedNode.Text
        Dim fileName As String = ".\Config\DataConnectionCfg.xml"
        Dim dbConnCfg As XElement = XElement.Load(fileName)
        Dim objElement = dbConnCfg.<Group>.Where(Function(item As XElement) item.@Name = groupName).FirstOrDefault
        If objElement IsNot Nothing Then
            Dim newGroupName As String = InputBox("请输入新的分组名称：", "分组管理", groupName)
            If newGroupName <> "" And newGroupName <> groupName Then
                objElement.@Name = newGroupName
                dbConnCfg.Save(fileName)
                MessageBox.Show("修改分组成功！")

                initTreeView()
            End If

        End If
    End Sub

    Private Sub tsmItemCloseConn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmItemCloseConn.Click
        CurrDataBase = Nothing
        CurrDBName = ""
        CurrDbType = ""
    End Sub

    Private Sub tsmiNewGroupQuery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiNewGroupQuery.Click
        Dim currNode As TreeNode = Me.TreeView1.SelectedNode
        Dim SQL As String = ""
        If currNode IsNot Nothing Then
            If currNode.Nodes.Count > 0 AndAlso currNode.FirstNode.Text = "列" Then
                '当前节点是表或者视图名称
                Dim tableName As String = currNode.Name

                Dim columns As String = ""
                For Each node As TreeNode In currNode.FirstNode.Nodes
                    columns &= vbTab & dbLeftChar & node.Name & dbRightChar & " ," & vbCrLf
                Next
                If currNode.FirstNode.Nodes.Count = 0 Then
                    columns = " * "
                Else
                    columns = columns.TrimEnd(",", vbCr, vbLf) & vbCrLf
                End If
                SQL = "SELECT" & columns & " FROM " & dbLeftChar & tableName & dbRightChar & vbCrLf

            End If
        End If

        Dim window As New frmDataQuery(Me.CurrDataBase)
        window.Text = "多数据源查询"
        window.IsGroupQuery = True

        For Each key As String In DBNodePath.Keys
            Dim dc As DataConnection = New DataConnection
            Dim strTemp As String = key.Split("\")(2).Split(":")(0)
            dc.Enabled = True
            'PWMIS.Common.DBMSType.SqlServer 
            dc.DbType = System.Enum.Parse(GetType(PWMIS.Common.DBMSType), strTemp, True)
            dc.ConnectionStrng = DBNodePath(key).ConnectionString
            dc.CurrAdoHelper = DBNodePath(key)

            window.DataConnections.Add(dc)
        Next

        window.rtbQueryText.Text = Sql
        window.CurrDBName = Me.CurrDBName

        Me.CommandForm.OpenWindow(Me, window, "")
        window.CommandForm = Me.CommandForm
    End Sub

    Private Sub tsmiExpTableDataSQL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiExpTableDataSQL.Click
        Dim currNode As TreeNode = Me.TreeView1.SelectedNode

        If currNode IsNot Nothing Then
            Dim window As New frmExportData
            If currNode.Parent IsNot Nothing AndAlso currNode.Parent.Text = "表" Then
                Dim tables As New List(Of String)
                For Each node As TreeNode In currNode.Parent.Nodes
                    tables.Add(node.Name)
                Next
                window.ExportTableName = currNode.Name
                window.ExportTables = tables
            ElseIf currNode.Text = "表" Then
                Dim tables As New List(Of String)
                For Each node As TreeNode In currNode.Nodes
                    tables.Add(node.Name)
                Next
                window.ExportTables = tables
            Else
                MessageBox.Show("请选择一个'表'节点或者具体的表名称！", "数据导出", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            window.CurrDataBase = Me.CurrDataBase
            window.dbLeftChar = Me.dbLeftChar
            window.dbRightChar = Me.dbRightChar
            window.ExportTypeIndex = 2
            window.ShowDialog()

        Else
            MessageBox.Show("请选择一个表名称！", "数据导出", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub
End Class