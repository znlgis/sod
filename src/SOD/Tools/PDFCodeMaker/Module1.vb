'''''''''''''''''''''''''''''''''''''''''''''''''''''
'PDF.NET ����������
'����SqlMap�淶������.NET ҵ�����ݷ��ʴ��롣http://www.pwmis.cn/sqlMap/
'������ƣ�Bluedoctor MSN:bluedoctors@msn.com
'Ver 1.2 Date 2008.4.4
'�汾��ʷ��
'Ver 1.0:֧��VB.NET��������
'Ver 1.1:֧��C# ��������
'Ver 1.2 ֧������ʵ����Ӱ�����
'Ver 1.2.1 �޸�����C# ����ʱ��������ʱ���滻������BUG
'''''''''''''''''''''''''''''''''''''''''''''''''''
Imports System.Xml
Imports System.IO
Imports PWMIS.DataMap.SqlMap
Imports PWMIS.Common
Imports System.Configuration

Module Module1

    Private Const VB_FunParamReplaceText As String = "ByVal #ReplaceableParameter# AS String "
    Private Const CS_FunParamReplaceText As String = "string #ReplaceableParameter# "

    Public Enum CodeLanguage
        VB
        CSharp
    End Enum

    Sub Main()
        Dim VerMsg As String = "PDF.NET Code Maker : " & System.Reflection.Assembly.GetEntryAssembly().FullName & ",http://www.pwmis.com/SqlMap/"
        Console.WriteLine(VerMsg)
        WriteLog("============================================")
        WriteLog(VerMsg)
        WriteLog("Begin:" & DateTime.Now.ToString())

        Dim ConfigFile As String = ConfigurationManager.AppSettings("SqlMapConfigFile")  ' "E:\PWMIS\1.config"
        Dim RootNameSpace As String = ConfigurationManager.AppSettings("RootNameSpace")
        Dim OutPutPath As String = ConfigurationManager.AppSettings("OutPutPath")
        Dim ReWrite As Boolean = Convert.ToBoolean(ConfigurationManager.AppSettings("ReWrite"))
        Dim Language As CodeLanguage = [Enum].Parse(GetType(CodeLanguage), ConfigurationManager.AppSettings("CodeLanguage"))

        Console.WriteLine("�Ѿ���ȡ������Ϣ����ʼ���ɴ��룮����")
        Console.WriteLine()
        Dim DoneMsg As String = String.Empty
        Try
            Dim TStart As DateTime = DateTime.Now
            Dim MakeCount As Integer = MakeCodeString(ConfigFile, RootNameSpace, OutPutPath, ReWrite, Language)
            If MakeCount > 0 Then
                Dim TEnd As DateTime = DateTime.Now
                Dim TS As TimeSpan = TEnd.Subtract(TStart)
                DoneMsg = "����ɹ���ɣ������ļ� " & MakeCount.ToString() & "������ʱ" & TS.Milliseconds.ToString() & "ms���������������"
            Else
                DoneMsg = "δ�ܹ��ɹ�����ȫ���ļ���"
            End If
        Catch ex As Exception
            DoneMsg = ex.ToString()

        End Try

        Console.WriteLine()
        Console.WriteLine(DoneMsg)
        WriteLog(DoneMsg)
        Console.Read()

    End Sub

    Private Function SaveCode(ByVal OutPutFile As String, ByVal CodeString As String) As Boolean
        Try
            Dim writer As StreamWriter = New StreamWriter(OutPutFile, False, System.Text.Encoding.Default)
            writer.Write(CodeString.Replace(vbCrLf, vbLf).Replace(vbLf, vbCrLf))
            writer.Flush()
            writer.Close()
            Dim FullPathMsg As String = Path.GetFullPath(OutPutFile) & "  OK!"
            Console.WriteLine(FullPathMsg)
            WriteLog(FullPathMsg)
            Return True
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            WriteLog(ex.Message)
        End Try
        Return False


    End Function

    Private Sub WriteLog(ByVal Log As String)
        'д����־
        If Not Directory.Exists("LOG") Then
            Directory.CreateDirectory("LOG")
        End If
        Dim writer As StreamWriter = New StreamWriter("LOG\Make.Log", True, System.Text.Encoding.Default)
        writer.WriteLine(Log)
        writer.Flush()
        writer.Close()
    End Sub

    Function MakeCodeString(ByVal SqlMapConfigFile As String, ByVal RootNameSpace As String, ByVal OutPutPath As String, Optional ByVal ReWrite As Boolean = True, Optional ByVal Language As CodeLanguage = CodeLanguage.VB) As Integer
        '�������Ŀ¼
        OutPutPath = OutPutPath.Replace("/", "\")
        If Not OutPutPath.EndsWith("\") Then
            OutPutPath &= "\"
        End If

        Dim OutFileName As String = "#OutPutFile#"
        Dim CodeTemplateFileName As String
        Select Case Language
            Case CodeLanguage.VB
                CodeTemplateFileName = "VBCodeTemplate.txt"
                OutFileName &= ".vb"
            Case CodeLanguage.CSharp
                CodeTemplateFileName = "CSCodeTemplate.txt"
                OutFileName &= ".cs"
            Case Else
                CodeTemplateFileName = "VBCodeTemplate.txt"
                OutFileName &= ".vb"
        End Select
        If Not File.Exists(CodeTemplateFileName) Then
            Throw New Exception("����ģ���ļ�δ�ҵ���")
        End If
        Dim reader As StreamReader = New StreamReader(CodeTemplateFileName, Text.Encoding.Default)
        Dim TemplateString As String = reader.ReadToEnd()
        reader.Close()

        Dim MakeCount As Integer = 0
        Dim EmbedAssemblySource As String = GetEmbedAssemblySource(SqlMapConfigFile)
        Dim Scripts As XmlNodeList = GetScriptNodeList(SqlMapConfigFile)
        If Not Scripts Is Nothing Then
            For Each Script As XmlNode In Scripts
                If Not Script.Attributes("Type") Is Nothing AndAlso Not Script.Attributes("Version") Is Nothing Then
                    '�������ƿռ�
                    Dim TypeName As String = Script.Attributes("Type").Value
                    Dim connectionString As String = ""
                    If Not Script.Attributes("ConnectionString") Is Nothing Then connectionString = Script.Attributes("ConnectionString").Value
                    Dim providerName As String = ""
                    If Not Script.Attributes("ProviderName") Is Nothing Then providerName = Script.Attributes("ProviderName").Value
                    If providerName = "" Then providerName = TypeName 'Ĭ��ʹ��TypeName��Ϊ�ṩ��������

                    'Dim DataBaseType As PWMIS.Common.DBMSType = [Enum].Parse(GetType(PWMIS.Common.DBMSType), TypeName)
                    Dim DB As PWMIS.DataProvider.Data.AdoHelper = PWMIS.DataProvider.Adapter.MyDB.GetDBHelperByProviderString(providerName, connectionString)

                    '��4.0 ��ʼ��ͳһʹ��SQLMAP��ΪDAL���ƿռ䣬��Ϊʵ�ʵ�������ͨ���������ġ�
                    Dim CurrNameSpace As String = RootNameSpace & ".SqlMapDAL"  '& TypeName & "DAL"
                    Dim CurrOutPutPath As String = OutPutPath
                    If Not ReWrite Then '��������д�����ļ�
                        CurrOutPutPath = OutPutPath & TypeName & "\" & DateTime.Now.ToShortDateString & "\"
                    End If
                    If Not Directory.Exists(CurrOutPutPath) Then
                        Directory.CreateDirectory(CurrOutPutPath)
                    End If
                    If Script.HasChildNodes Then
                        '���� ��ӳ��
                        For Each CommandClass As XmlNode In Script.ChildNodes
                            '�����ע�ͣ�����
                            If CommandClass.NodeType <> XmlNodeType.Element Then Continue For

                            Dim CodeString As String = TemplateString
                            GetStringArea(CodeString, "<%R:REM[", "REM]%>", "ʹ�øó���ǰ�������ó��򼯣�PWMIS.Core���������涨������ƿռ�ǰ׺��Ҫʹ�ãУףͣɣӣ�������Ϣ����鿴 http://www.pwmis.com/sqlmap")
                            Dim FunTemplateString As String = GetStringArea(CodeString, "<%R:Function[", "Function]%>", "@FunTemplate") 'Mid(CodeString, ReFunAtB, ReFunAtE - ReFunAtB)

                            Dim CommandClassName As String = CommandClass.Attributes("Name").Value
                            Dim CommandClassMap As String '= CommandClass.Attributes("Class").Value
                            If Not CommandClass.Attributes("Class") Is Nothing AndAlso CommandClass.Attributes("Class").Value <> "" Then
                                CommandClassMap = CommandClass.Attributes("Class").Value
                            Else
                                CommandClassMap = CommandClassName
                            End If
                            '�����ӿ� 2010.5.26
                            Dim CommandClassInterface As String
                            If Not CommandClass.Attributes("Interface") Is Nothing AndAlso CommandClass.Attributes("Interface").Value <> "" Then
                                CommandClassInterface = CommandClass.Attributes("Interface").Value
                            Else
                                CommandClassInterface = ""
                            End If

                            Dim ClassDescription As String '= CommandClass.Attributes("Description").Value
                            If Not CommandClass.Attributes("Description") Is Nothing AndAlso CommandClass.Attributes("Description").Value <> "" Then
                                ClassDescription = CommandClass.Attributes("Description").Value
                            Else
                                ClassDescription = CommandClassName
                            End If

                            '����ӳ��������� CommandClassMap
                            If CommandClassMap = "" Then
                                CommandClassMap = CommandClassName
                            Else
                                '������ӳ�����Ƶĵ�ǰ�޶������ƿռ�
                                Dim tempAt As Integer = InStr(1, CommandClassMap, ".", CompareMethod.Text)
                                If tempAt > 0 Then
                                    CurrNameSpace &= "." & Left(CommandClassMap, tempAt - 1)
                                    CommandClassMap = Mid(CommandClassMap, tempAt + 1)
                                End If
                            End If
                            'ִ�������ı��滻
                            CodeString = CodeString.Replace("#CurrNameSpace#", CurrNameSpace).Replace("#CommandClassMap#", CommandClassMap).Replace("#CommandClassName#", CommandClassName)
                            CodeString = CodeString.Replace("#CreateDateTime#", DateTime.Now.ToString()).Replace("#ClassDescription#", ClassDescription)
                            '�滻�ӿ�
                            If CommandClassInterface <> "" Then
                                CommandClassInterface = "," + CommandClassInterface
                            End If
                            CodeString = CodeString.Replace("#CommandClassInterface#", CommandClassInterface).Replace("#EmbedAssemblySource#", EmbedAssemblySource)


                            If CommandClass.HasChildNodes Then
                                Dim ArrFunString(CommandClass.ChildNodes.Count()) As String
                                Dim nodeCount As Integer = 0
                                For Each node As XmlNode In CommandClass.ChildNodes
                                    '�����ע�ͣ�����
                                    If node.NodeType <> XmlNodeType.Element Then Continue For

                                    '��ȡ�ڵ�����
                                    Dim nodeName As String = node.Name
                                    Dim CommandName As String = node.Attributes("CommandName").Value
                                    Dim CommandType As String = node.Attributes("CommandType").Value
                                    Dim Method As String '= node.Attributes("Method").Value
                                    If Not node.Attributes("Method") Is Nothing AndAlso node.Attributes("Method").Value.Trim() <> "" Then
                                        Method = node.Attributes("Method").Value
                                    Else
                                        Method = CommandName
                                    End If
                                    Dim SqlMapScript As String = node.InnerText
                                    Dim ResultClass As String = String.Empty
                                    Dim CommandDescription As String = String.Empty
                                    Dim ResultMap As String = String.Empty

                                    If Not node.Attributes("ResultClass") Is Nothing AndAlso node.Attributes("ResultClass").Value <> "" Then
                                        ResultClass = node.Attributes("ResultClass").Value
                                        '�������Ϊʵ�����ͷ���ָ�����ӳ������
                                        If ResultClass = PWMIS.Common.enumResultClass.EntityObject.ToString() Or _
                                            ResultClass = PWMIS.Common.enumResultClass.[EntityList].ToString() Or _
                                            ResultClass = PWMIS.Common.enumResultClass.ObjectList.ToString() Then
                                            If Not node.Attributes("ResultMap") Is Nothing AndAlso node.Attributes("ResultClass").Value <> "" Then
                                                ResultMap = node.Attributes("ResultMap").Value
                                            End If
                                        End If
                                    End If

                                    If Not node.Attributes("Description") Is Nothing AndAlso node.Attributes("Description").Value <> "" Then
                                        CommandDescription = node.Attributes("Description").Value
                                    End If
                                    If CommandDescription = "" Then CommandDescription = "Function " & Method & ":"

                                    '��ȡSqlMapper ʵ��
                                    'Dim DataBaseType As PWMIS.Common.DBMSType = [Enum].Parse(GetType(PWMIS.Common.DBMSType), TypeName)
                                    'Dim DB As PWMIS.DataProvider.Data.AdoHelper = PWMIS.DataProvider.Adapter.MyDB.GetDBHelper(DataBaseType, "")
                                    Dim mapper As New SqlMapper
                                    mapper.DataBase = DB

                                    '��ȡ���������Ϣ
                                    Dim cmdInfo As CommandInfo = mapper.GetCommandInfoBySqlMapScript(SqlMapScript)
                                    Dim DataParameters() As IDataParameter = cmdInfo.DataParameters ' mapper.GetParameters(SqlMapScript)
                                    Dim ArrFunTemplateString() As String = FunTemplateString.Split(vbCrLf) '.ToCharArray()

                                    '���ɺ���˵��
                                    ArrFunTemplateString(2) = ArrFunTemplateString(2).Replace("#CommandDescription#", CommandDescription)

                                    '���ɲ���ע��
                                    Dim tempStr As String = String.Empty '/// <param name="#ParameterName#"></param>
                                    Dim ParamsLength As Integer = mapper.ParasLenth - 1
                                    For I As Integer = 0 To ParamsLength
                                        tempStr &= ArrFunTemplateString(5).Replace("#ParameterName#", mapper.ParamsMap(I).ParamName)
                                    Next
                                    ArrFunTemplateString(5) = tempStr

                                    '�滻��������
                                    Dim FunParamsTemplate As String = GetStringArea(ArrFunTemplateString(8), "<%R:Params[", "Params]%>", "@FunParams")
                                    Dim Sepchar As String = GetStringArea(FunParamsTemplate, "[", "]", "")
                                    tempStr = String.Empty
                                    For I As Integer = 0 To ParamsLength
                                        tempStr &= FunParamsTemplate.Replace("#ParameterName#", mapper.ParamsMap(I).ParamName).Replace("#DataType#", mapper.ParamsMap(I).TypeCode.ToString())
                                        If I <> ParamsLength Then tempStr &= Sepchar
                                    Next
                                    '�����б���滻��������
                                    If mapper.ParamsReplaceable.Count > 0 Then
                                        Dim ReplaceCount As Integer = mapper.ParamsReplaceable.Count - 1
                                        If tempStr <> "" Then tempStr &= Sepchar
                                        Dim FunParamReplaceText As String
                                        If Language = CodeLanguage.VB Then
                                            FunParamReplaceText = VB_FunParamReplaceText
                                        Else
                                            FunParamReplaceText = CS_FunParamReplaceText
                                        End If
                                        For I As Integer = 0 To ReplaceCount
                                            tempStr &= FunParamReplaceText.Replace("#ReplaceableParameter#", mapper.ParamsReplaceable(I))
                                            If I <> ReplaceCount Then tempStr &= Sepchar
                                        Next
                                    End If

                                    ArrFunTemplateString(8) = ArrFunTemplateString(8).Replace("@FunParams", tempStr)

                                    '�滻������
                                    ArrFunTemplateString(8) = ArrFunTemplateString(8).Replace("#Method#", Method)
                                    Dim ExecuteType As String = ""
                                    Select Case nodeName
                                        Case "Select", "Read"
                                            Select Case ResultClass
                                                Case "", enumResultClass.Default.ToString(), enumResultClass.DataSet.ToString()
                                                    ExecuteType = "ExecuteDataSet"
                                                    ResultClass = "DataSet"
                                                Case enumResultClass.EntityObject.ToString() 'ʵ������
                                                    If ResultClass = "" Then
                                                        Throw New Exception("��������ֵָ�����󣬲���ָ��һ���յĽ��ʵ�壡��������λ�ã�" & ArrFunTemplateString(8))
                                                    End If
                                                    ExecuteType = "ExecuteReader"
                                                    ResultClass = ResultMap
                                                    '����ʵ����ת��
                                                Case enumResultClass.[EntityList].ToString() 'ʵ������
                                                    If ResultClass = "" Then
                                                        Throw New Exception("��������ֵָ�����󣬲���ָ��һ���յĽ��ʵ�壡��������λ�ã�" & ArrFunTemplateString(8))
                                                    End If
                                                    ExecuteType = "ExecuteReaderList" 'ʵ�弯��
                                                    If Language = CodeLanguage.VB Then
                                                        ResultClass = "List(Of " & ResultMap & ")"
                                                    ElseIf Language = CodeLanguage.CSharp Then
                                                        ResultClass = "List<" & ResultMap & ">"
                                                    End If

                                                    '����ʵ����ת��
                                                Case enumResultClass.ValueType.ToString()  '��ֵ����
                                                    ExecuteType = "ExecuteScalar"
                                                    ResultClass = "System.Object"
                                                Case enumResultClass.ObjectList.ToString() 'һ������б�����DTO�б�
                                                    If ResultClass = "" Then
                                                        Throw New Exception("��������ֵָ�����󣬲���ָ��һ���յĽ��ʵ�壡��������λ�ã�" & ArrFunTemplateString(8))
                                                    End If
                                                    ExecuteType = "ExecuteObjectList" 'һ������б���
                                                    If Language = CodeLanguage.VB Then
                                                        ResultClass = "List(Of " & ResultMap & ")"
                                                    ElseIf Language = CodeLanguage.CSharp Then
                                                        ResultClass = "List<" & ResultMap & ">"
                                                    End If

                                                Case Else
                                                    Throw New Exception("δ֪�Ľ�����ͣ���������λ�ã�" & ArrFunTemplateString(8))
                                            End Select

                                        Case "Update"
                                            If ResultClass = "" Then ResultClass = "Int32"
                                            ExecuteType = "ExecuteNonQuery"
                                        Case "Delete"
                                            If ResultClass = "" Then ResultClass = "Int32"
                                            ExecuteType = "ExecuteNonQuery"
                                        Case "Create", "Insert"
                                            If ResultClass = "" Then ResultClass = "Int32"
                                            ExecuteType = "ExecuteNonQuery"

                                        Case Else
                                            Throw New Exception("��֧�ֵ�CRUD���ͣ���ָ��Create/Insert,Read/Select,Update,Delete ֮һ��")
                                    End Select

                                    '�滻������������
                                    ArrFunTemplateString(8) = ArrFunTemplateString(8).Replace("#ResultClass#", ResultClass)

                                    '�滻������
                                    ArrFunTemplateString(11) = ArrFunTemplateString(11).Replace("#CommandName#", CommandName)

                                    '������ֵ��������ʽ
                                    tempStr = String.Empty
                                    For I As Integer = 0 To ParamsLength
                                        tempStr &= ArrFunTemplateString(14).Replace("#ParamIndex#", I.ToString).Replace("#ParameterName#", mapper.ParamsMap(I).ParamName)
                                    Next
                                    ArrFunTemplateString(14) = tempStr

                                    '������ֵ��������ʽ
                                    tempStr = String.Empty
                                    For I As Integer = 0 To ParamsLength
                                        tempStr &= ArrFunTemplateString(18).Replace("#ParameterName#", mapper.ParamsMap(I).ParamName)
                                    Next
                                    ArrFunTemplateString(18) = tempStr

                                    '�滻�������� ReplaceableParameter
                                    tempStr = String.Empty
                                    If mapper.ParamsReplaceable.Count > 0 Then
                                        For I As Integer = 0 To mapper.ParamsReplaceable.Count - 1
                                            tempStr &= ArrFunTemplateString(21).Replace("#ReplaceableParameter#", mapper.ParamsReplaceable(I))
                                        Next
                                    End If
                                    ArrFunTemplateString(21) = tempStr

                                    ArrFunTemplateString(23) = ArrFunTemplateString(23).Replace("#ExecuteType#", ExecuteType)
                                    '�滻��ѯ��ʽ
                                    If Language = CodeLanguage.VB Then
                                        If ParamsLength < 0 Then
                                            ArrFunTemplateString(23) = ArrFunTemplateString(23).Replace("#DataParameters#", ",Nothing")
                                        Else
                                            ArrFunTemplateString(23) = ArrFunTemplateString(23).Replace("#DataParameters#", ", cmdInfo.DataParameters")
                                        End If
                                        If ExecuteType = "ExecuteReader" Then
                                            ArrFunTemplateString(23) = ArrFunTemplateString(23).Replace("Return", "Return .ResultMapEntity(") + ", New " & ResultClass & " )"
                                        ElseIf ExecuteType = "ExecuteReaderList" Then
                                            ArrFunTemplateString(23) = ArrFunTemplateString(23).Replace("Return", "Return EntityQuery(Of " & ResultMap & ").QueryList(")
                                            ArrFunTemplateString(23) = ArrFunTemplateString(23).Replace("ExecuteReaderList", "ExecuteReader") & ")"
                                        ElseIf ExecuteType = "ExecuteObjectList" Then
                                            ArrFunTemplateString(23) = ArrFunTemplateString(23).Replace("Return", "Return MapObjectList(Of " & ResultMap & ")(")
                                            ArrFunTemplateString(23) = ArrFunTemplateString(23).Replace("ExecuteObjectList", "ExecuteReader") & ")"
                                        End If
                                    ElseIf Language = CodeLanguage.CSharp Then
                                        If ParamsLength < 0 Then
                                            ArrFunTemplateString(23) = ArrFunTemplateString(23).Replace("#DataParameters#", ",null")
                                        Else
                                            ArrFunTemplateString(23) = ArrFunTemplateString(23).Replace("#DataParameters#", ", cmdInfo.DataParameters")
                                        End If
                                        If ExecuteType = "ExecuteReader" Then
                                            ArrFunTemplateString(23) = ArrFunTemplateString(23).Replace("return", "return (" & ResultClass & ")Mapper.ResultMapEntity(") + ", new " & ResultClass & "() );"
                                            ArrFunTemplateString(23) = ArrFunTemplateString(23).Replace(";,", ",")
                                        ElseIf ExecuteType = "ExecuteReaderList" Then
                                            ArrFunTemplateString(23) = ArrFunTemplateString(23).Replace("return", "return EntityQuery<" & ResultMap & ">.QueryList(")
                                            ArrFunTemplateString(23) = ArrFunTemplateString(23).Replace(");", "));").Replace("ExecuteReaderList", "ExecuteReader")
                                        ElseIf ExecuteType = "ExecuteObjectList" Then
                                            ArrFunTemplateString(23) = ArrFunTemplateString(23).Replace("return", "return MapObjectList<" & ResultMap & ">(")
                                            ArrFunTemplateString(23) = ArrFunTemplateString(23).Replace(");", "));").Replace("ExecuteObjectList", "ExecuteReader")
                                        End If
                                    Else
                                        '
                                    End If


                                    '���������ĺ�����
                                    Dim FullFunBody As New System.Text.StringBuilder
                                    With FullFunBody
                                        .Append(ArrFunTemplateString(1)) '.Append(vbCr)
                                        .Append(ArrFunTemplateString(2)) '.Append(vbLf)
                                        .Append(ArrFunTemplateString(3)) '.Append(vbLf)
                                        .Append(ArrFunTemplateString(5)) '.Append(vbLf)
                                        .Append(ArrFunTemplateString(7)) '.Append(vbLf)
                                        .Append(ArrFunTemplateString(8)) '.Append(vbLf)
                                        .Append(ArrFunTemplateString(9)) '.Append(vbLf)
                                        .Append(ArrFunTemplateString(10)) '.Append(vbLf)
                                        .Append(ArrFunTemplateString(11)) '.Append(vbLf)
                                        If ParamsLength >= 0 Then .Append(ArrFunTemplateString(12)) '.Append(vbLf)
                                        .Append(ArrFunTemplateString(14)) '.Append(vbLf)
                                        If ParamsLength >= 0 Then .Append(ArrFunTemplateString(16)) '.Append(vbLf)
                                        .Append(ArrFunTemplateString(18)) '.Append(vbLf)
                                        If mapper.ParamsReplaceable.Count > 0 Then .Append(ArrFunTemplateString(20)) '.Append(vbLf)
                                        .Append(ArrFunTemplateString(21)) '.Append(vbLf)
                                        .Append(ArrFunTemplateString(22)) '.Append(vbLf)
                                        .Append(ArrFunTemplateString(23)) '.Append(vbLf)
                                        .Append(ArrFunTemplateString(24)) '.Append(vbLf)
                                        .Append(ArrFunTemplateString(25)) '.Append(vbLf)
                                    End With

                                    ArrFunString(nodeCount) = FullFunBody.ToString()
                                    nodeCount += 1
                                Next
                                CodeString = CodeString.Replace("@FunTemplate", String.Join(vbCrLf, ArrFunString))
                            End If
                            Dim OutPutFile As String = CurrOutPutPath & OutFileName.Replace("#OutPutFile#", CommandClassName)
                            If SaveCode(OutPutFile, CodeString) Then
                                MakeCount += 1
                            End If
                        Next
                    End If
                End If
            Next
        End If
        Return MakeCount
    End Function



    Private Function GetStringArea(ByRef Source As String, ByVal BeginFlag As String, ByVal EndFlag As String, ByVal ReplacedFlag As String) As String
        Dim ReParamAtB As Integer = InStr(Source, BeginFlag, CompareMethod.Text) '+ Len(BeginFlag)
        Dim ReParamAtE As Integer = InStr(Source, EndFlag, CompareMethod.Text) + Len(EndFlag)
        Dim ParamsTemplate As String = Mid(Source, ReParamAtB, ReParamAtE - ReParamAtB)
        Source = Source.Replace(ParamsTemplate, ReplacedFlag)
        ParamsTemplate = ParamsTemplate.Replace(BeginFlag, "").Replace(EndFlag, "")
        Return ParamsTemplate
    End Function

    Private Function GetScriptNodeList(ByVal ConfigFile As String) As XmlNodeList
        Dim doc As XmlDocument = New XmlDocument
        Dim _ErrDescription As String
        Try
            doc.Load(ConfigFile) '���Կ��ǲ���XML�ļ����ķ�ʽ�ӿ��ȡ

            Dim SqlScripts As XmlNodeList
            Dim root As XmlElement = doc.DocumentElement
            SqlScripts = root.SelectNodes("/SqlMap/Script")
            Return SqlScripts
        Catch ex As Exception
            _ErrDescription = ex.Message
            Console.WriteLine(_ErrDescription)
            Console.Read()
            End
        End Try
    End Function

    Private Function GetEmbedAssemblySource(ByVal ConfigFile As String) As String
        Dim doc As XmlDocument = New XmlDocument
        Dim _ErrDescription As String
        Try
            doc.Load(ConfigFile) '���Կ��ǲ���XML�ļ����ķ�ʽ�ӿ��ȡ
            Dim root As XmlElement = doc.DocumentElement
            If root.Attributes.Count > 0 Then
                Dim source As XmlAttribute = root.Attributes("EmbedAssemblySource")
                If source IsNot Nothing Then
                    Return source.Value.ToString()
                End If
            End If
            Return ""
        Catch ex As Exception
            _ErrDescription = ex.Message
            Console.WriteLine(_ErrDescription)
            Console.Read()
            End
        End Try
    End Function


End Module
