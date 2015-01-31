Imports PWMIS.DataProvider.Data
Imports PWMIS.DataProvider.Adapter
Imports System.ComponentModel
Imports PDFDotNET

Public Class frmEntityCreate
    Public CurrDbHelper As AdoHelper
    Public SourceTables As List(Of String)
    Public SourceViews As List(Of String)
    Dim propWindow As New EntityCreateProperty
    Dim isSqlServer2000 As Boolean
    Dim dataSourceProductVersion, dataSourceProductName As String

    ''' <summary>
    ''' 当前数据库名称
    ''' </summary>
    ''' <remarks></remarks>
    Public CurrDBName As String

    Dim entityClassTemplate As XElement = _
    <EntityTemplate>
        <FileHead>
            <![CDATA[
'''
''本类由PWMIS 实体类生成工具(Ver 4.0)自动生成
''http://www.pwmis.com/sqlmap
''使用前请先在项目工程中引用 PWMIS.Core.dll
''%DateTime%

''''
]]>
        </FileHead>
        <CS>
            <ClassText>
                <![CDATA[
using System;
using PWMIS.DataMap.Entity;

namespace [NameSpace] 
{
  [Serializable()]
  public partial class [ClassName] : EntityBase
  {
    public [ClassName]()
    {
            TableName = "[SqlTableName]";
            //IdentityName = "标识字段名";
%IdentityName%
            //PrimaryKeys.Add("主键字段名");
%PrimaryKey%
            %AddProperty%
    }

%Propertys%

  }
}
]]>
            </ClassText>
            <PropertyText>
                <![CDATA[
      /// <summary>
      /// [FieldDesc]
      /// </summary>
      public <T> [Name]
      {
          get{return getProperty<<T>>("[Name]");}
          set{setProperty("[Name]",value [,Length]);}
      }
]]>
            </PropertyText>
            <AddProperty>
                <![CDATA[
            PropertyNames = new string[] { %PropertyNames% };
            PropertyValues = new object[PropertyNames.Length]; 
]]>
            </AddProperty>
        </CS>
        <VB>
            <ClassText>
                <![CDATA[
Imports System
Imports PWMIS.DataMap.Entity

NameSpace [NameSpace] 
  <Serializable()> _
  Partial Public Class  [ClassName] 
Inherits  EntityBase
 
    Sub New()
            TableName = "[SqlTableName]";
            'IdentityName = "标识字段名"
%IdentityName%
            'PrimaryKeys.Add("主键字段名");
%PrimaryKey%
            %AddProperty%
    End Sub

%Propertys%

  End Class
End NameSpace

]]>
            </ClassText>
            <PropertyText>
                <![CDATA[
    ''' <summary>
    ''' [FieldDesc]
    ''' </summary>
    Public Property [Name]() As <T>
        Get
            Return getProperty(Of <T>)("[Name]")
        End Get
        Set(ByVal Value As <T>)
            setProperty("[Name]",Value [,Length])
        End Set
    End Property
]]>
            </PropertyText>
            <AddProperty>
                <![CDATA[
            PropertyNames = New String() { %PropertyNames% }
            PropertyValues = New Object(PropertyNames.Length)
]]>
            </AddProperty>
        </VB>
    </EntityTemplate>

    Dim dtMapInfo As Data.DataTable


    Public Sub New()

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。

    End Sub

    Public Sub New(ByVal ado As AdoHelper, ByVal tables As List(Of String), ByVal views As List(Of String))

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Me.CurrDbHelper = ado
        Me.SourceTables = tables
        Me.SourceViews = views


    End Sub

    Function MakeClassText(ByVal className As String, ByVal sql As String, ByVal sql_tableName As String) As String
        Dim ds As DataSet = CurrDbHelper.ExecuteDataSetSchema(sql, CommandType.Text, Nothing)
        Dim dt As DataTable = ds.Tables(0)
        Dim str, str2 As String
        Dim classText, propertyText, addProp As String
        Dim isPartial As Boolean = propWindow.UsePartialClass
        Dim langLine As String

        Select Case propWindow.Language
            Case DevelopLanguage.CSharp
                classText = entityClassTemplate.<CS>.<ClassText>.Value
                propertyText = entityClassTemplate.<CS>.<PropertyText>.Value
                addProp = entityClassTemplate.<CS>.<AddProperty>.Value
                If Not isPartial Then classText = classText.Replace("partial", "")
                langLine = ";" & vbCrLf
            Case DevelopLanguage.VB
                classText = entityClassTemplate.<VB>.<ClassText>.Value
                propertyText = entityClassTemplate.<VB>.<PropertyText>.Value
                addProp = entityClassTemplate.<VB>.<AddProperty>.Value
                If Not isPartial Then classText = classText.Replace("partial", "")
                langLine = vbCrLf
            Case Else
                classText = entityClassTemplate.<CS>.<ClassText>.Value
                propertyText = entityClassTemplate.<CS>.<PropertyText>.Value
                addProp = entityClassTemplate.<CS>.<AddProperty>.Value
                If Not isPartial Then classText = classText.Replace("partial", "")
                langLine = ";" & vbCrLf
        End Select

        classText = classText.Replace("[NameSpace]", Me.propWindow.DefaultNamespace) _
        .Replace("[ClassName]", className) _
                .Replace("[SqlTableName]", sql_tableName)

        Dim strPKs As String = "", strIdentity As String = ""
        For Each col As DataColumn In dt.PrimaryKey
            strPKs &= "\t    PrimaryKeys.Add(""" & col.ColumnName & " "")" & langLine
            If col.AutoIncrement Then
                strIdentity = "\t    IdentityName=""" & col.ColumnName & " """ & langLine
            End If
        Next
        classText = classText.Replace("%PrimaryKey%", strPKs).Replace("%IdentityName%", strIdentity)
        Dim dtTableFiledDesc As DataTable
        If sql_tableName <> "" Then
            '生成字段说明
            dtTableFiledDesc = Me.getSqlTableFieldDescription(Me.isSqlServer2000, sql_tableName)

        End If
        Dim perpertyNames As String = ""
        For Each col As DataColumn In dt.Columns
            Dim strLength As String = IIf(col.MaxLength > 0, "," & col.MaxLength, "")
            Dim fieldDesc As String = ""
            If dtTableFiledDesc IsNot Nothing Then
                For Each dr As DataRow In dtTableFiledDesc.Rows
                    If dr(1).ToString() = col.ColumnName Then
                        fieldDesc = dr(2).ToString()
                        Exit For
                    End If
                Next
            End If

            str += propertyText.Replace("[Name]", col.ColumnName.Replace(" ", "")) _
            .Replace("<T>", col.DataType.ToString()) _
            .Replace("[TypeCode]", col.DataType.ToString().Replace("System", "TypeCode")) _
            .Replace("[,Length]", strLength) _
                    .Replace("[FieldDesc]", fieldDesc)

            perpertyNames += """" & col.ColumnName & " "","
        Next

        str2 = addProp.Replace("%PropertyNames%", perpertyNames.TrimEnd(","c))
        classText = classText.Replace("%Propertys%", str).Replace("%AddProperty%", str2)
        Return classText
    End Function

    Private Function getSqlTableFieldDescription(ByVal sqlServer2000 As Boolean, ByVal tableName As String) As DataTable
        Dim sql As String = ""
        Dim sqlDescription As XElement = _
        <DataBase>
            <SqlServer>
                <Ver2000>
                    <![CDATA[
SELECT 
    [Table Name] = i_s.TABLE_NAME, 
    [Column Name] = i_s.COLUMN_NAME, 
    [Description] = s.value 
FROM 
    INFORMATION_SCHEMA.COLUMNS i_s 
LEFT OUTER JOIN 
    sysproperties s 
ON 
    s.id = OBJECT_ID(i_s.TABLE_SCHEMA+'.'+i_s.TABLE_NAME) 
    AND s.smallid = i_s.ORDINAL_POSITION 
    AND s.name = 'MS_Description' 
WHERE 
    OBJECTPROPERTY(OBJECT_ID(i_s.TABLE_SCHEMA+'.'+i_s.TABLE_NAME), 'IsMsShipped')=0 
    AND i_s.TABLE_NAME = '@@table_name'   
ORDER BY 
    i_s.TABLE_NAME, i_s.ORDINAL_POSITION

]]>
                </Ver2000>
                <Ver2005>
                    <![CDATA[
SELECT 
    [Table Name] = OBJECT_NAME(c.object_id), 
    [Column Name] = c.name, 
    [Description] = ex.value 
FROM 
    sys.columns c 
LEFT OUTER JOIN 
    sys.extended_properties ex 
ON 
    ex.major_id = c.object_id 
    AND ex.minor_id = c.column_id 
    AND ex.name = 'MS_Description' 
WHERE 
    OBJECTPROPERTY(c.object_id, 'IsMsShipped')=0 
     AND OBJECT_NAME(c.object_id) = '@@table_name' 
ORDER 
    BY OBJECT_NAME(c.object_id), c.column_id 

]]>
                </Ver2005>
            </SqlServer>
        </DataBase>

        If dataSourceProductName = "Microsoft SQL Server" Then
            If sqlServer2000 Then
                sql = sqlDescription.<SqlServer>.<Ver2000>.Value
            Else
                sql = sqlDescription.<SqlServer>.<Ver2005>.Value
            End If
        End If
        If sql <> "" Then
            sql = sql.Replace("@@table_name", tableName)
            Dim ds As DataSet = Me.CurrDbHelper.ExecuteDataSet(sql)
            Return ds.Tables(0)
        End If
        Return Nothing
    End Function

    Private Function GetFileHeadText()
        Dim headText As String = entityClassTemplate.<FileHead>.Value
        If Me.propWindow.Language = DevelopLanguage.CSharp Then
            headText = headText.Replace("''''", "*/").Replace("'''", "/*").Replace("''", " ")
        End If
        Return headText.Replace("%DateTime%", DateTime.Now.ToString())
    End Function

    Private Sub frmEntityCreate_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim count As Integer
        dtMapInfo = New Data.DataTable("mapInfo")
        dtMapInfo.Columns.Add("Selected", GetType(Boolean))
        dtMapInfo.Columns.Add("TableName", GetType(String))
        dtMapInfo.Columns.Add("TableType", GetType(String))
        dtMapInfo.Columns.Add("MapEntityName", GetType(String))
        dtMapInfo.Columns.Add("OutputFile", GetType(String))
        dtMapInfo.Columns(0).ReadOnly = False
        dtMapInfo.Columns(3).ReadOnly = False
        dtMapInfo.PrimaryKey = New Data.DataColumn() {dtMapInfo.Columns(1)}


        If SourceTables IsNot Nothing Then
            count += SourceTables.Count
            For Each table As String In SourceTables
                Dim dr As Data.DataRow = dtMapInfo.NewRow()
                dr(0) = True
                dr(1) = table
                dr(2) = "表"
                dr(3) = table
                dr(4) = "<默认>"
                dtMapInfo.Rows.Add(dr)
            Next
        End If
        If SourceViews IsNot Nothing Then
            count += SourceViews.Count
            For Each table As String In SourceViews
                Dim dr As Data.DataRow = dtMapInfo.NewRow()
                dr(0) = True
                dr(1) = table
                dr(2) = "视图"
                dr(3) = table
                dr(4) = "<默认>"
                dtMapInfo.Rows.Add(dr)
            Next
        End If
        dtMapInfo.AcceptChanges()


        If count = 1 Then
            rbtnSelectOneTable.Checked = True
        Else
            rbtnSelectAllTable.Checked = True

        End If

        Me.dgMapInfo.DataSource = dtMapInfo

        '属性窗口测试

        'propWindow.OutputPath = "c:\aaa\bbb"
        propWindow.DefaultNamespace = CurrDBName
        Me.PropertyGrid1.SelectedObject = propWindow

        'Dim result As IEnumerable(Of String) = From p In entityClassTemplate.<CS>.<ClassText> _
        '            Select p.Value
        'MessageBox.Show(entityClassTemplate.<CS>.<PropertyText>.Value)
        'For Each i In result
        '    MessageBox.Show(i)
        'Next


    End Sub

    Private Sub dgMapInfo_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgMapInfo.CellContentClick

        Dim dgv As DataGridView = sender
        If dgv.Columns(e.ColumnIndex).Name = "colFirstLook" Then
            Dim rowView As DataRowView = dgv.CurrentRow.DataBoundItem
            Dim tableName As String = rowView(1)
            Dim classText As String = Me.MakeClassText(tableName, "select * from " & tableName, tableName)
            Dim frmCode As New frmCodeFile
            frmCode.Text = "PDF.NET 实体类 预览"
            frmCode.ContentText = GetFileHeadText() & classText
            frmCode.ShowDialog()

        End If
    End Sub

    Private Sub btnMakeFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMakeFile.Click
        If Not System.IO.Directory.Exists(Me.propWindow.OutputPath) Then
            MessageBox.Show("指定的代码输出目录 " & Me.propWindow.OutputPath & " 不存在，请在属性窗口选择有效的路径。", "实体类生成器")
            Exit Sub
        End If
        If Not Me.propWindow.OutputPath.EndsWith("\") Then
            Me.propWindow.OutputPath &= "\"
        End If
        btnMakeFile.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        Me.PrgBarMakeFile.Maximum = dtMapInfo.Rows.Count
        Dim count As Integer = 0
        Me.txtMakeLog.Text = ""

        For Each row As DataRow In dtMapInfo.Rows
            If row(0) = True Then
                '被选中的行
                Dim entityName As String = row("MapEntityName").ToString()
                Dim tableName As String = row("TableName").ToString()
                Dim outputFile As String = row("OutputFile").ToString()
                If outputFile = "<默认>" Then
                    outputFile = Me.propWindow.OutputPath & entityName
                    If Me.propWindow.Language = DevelopLanguage.CSharp Then
                        outputFile = outputFile & ".cs"
                    Else
                        outputFile = outputFile & ".vb"
                    End If
                End If

                count += 1
                Me.txtMakeLog.Text &= "正在生成第" & count & " 个实体类文件： " & outputFile & vbCrLf
                Me.PrgBarMakeFile.Value = count
                Application.DoEvents()

                Dim classText As String = Me.MakeClassText(entityName, "select * from " & tableName, tableName)
                classText = GetFileHeadText() & classText
                My.Computer.FileSystem.WriteAllText(outputFile, classText, False)

            End If
        Next

        MessageBox.Show("成功生成 " & count & " 个实体类文件！", "实体类生成器")
        btnMakeFile.Enabled = True
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub btnSQLtoEntity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSQLtoEntity.Click
        RunProcessByConfig("EntityCodeMakerPath")
    End Sub

    Private Sub RunProcessByConfig(ByVal fileKey As String)
        System.Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory
        Dim fileName As String = Configuration.ConfigurationManager.AppSettings(fileKey)
        If System.IO.File.Exists(fileName) Then
            Try
                System.Diagnostics.Process.Start(fileName)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "打开文件", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try


        Else
            MessageBox.Show("指定的程序未找到，程序名称：" + fileName, "打开文件", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
End Class

Public Class EntityCreateProperty

    Dim _outputPath As String = ".\Entity"
    ''' <summary>
    ''' 代码输出目录
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("常规"), Description("生成的代码文件所在的目录"), DefaultValue(".\"), EditorAttribute(GetType(PDFDotNET.PropertyGridFolderItem), GetType(System.Drawing.Design.UITypeEditor))> _
    Property OutputPath() As String
        Get
            Return _outputPath
        End Get
        Set(ByVal value As String)
            _outputPath = value
        End Set
    End Property

    Dim _defaultNamespace As String = ""

    <Category("代码"), Description("生成的实体类默认的命名空间，通常默认是当前数据库编目名称。")> _
    Property DefaultNamespace() As String
        Get
            Return _defaultNamespace
        End Get
        Set(ByVal value As String)
            _defaultNamespace = value
        End Set
    End Property

    Dim _usePartialClass As Boolean = True
    ''' <summary>
    ''' 是否使用分部类
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("代码"), Description("生成的代码文件是否使用分部类"), DefaultValue(True)> _
    Property UsePartialClass() As Boolean
        Get
            Return _usePartialClass
        End Get
        Set(ByVal value As Boolean)
            _usePartialClass = value
        End Set
    End Property

    Dim _language As DevelopLanguage
    ''' <summary>
    ''' 使用的语言
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("代码"), Description("使用的.NET开发语言"), DefaultValue(GetType(DevelopLanguage), "VB")> _
    Property Language() As DevelopLanguage
        Get
            Return _language
        End Get
        Set(ByVal value As DevelopLanguage)
            _language = value
        End Set
    End Property
End Class

Public Enum DevelopLanguage
    VB
    CSharp
End Enum