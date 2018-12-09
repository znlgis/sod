'''''''''''''''''''''''''''''''''''''''''''''''''''''
'PDF.NET 代码生成器
'根据SqlMap规范，生成.NET 业务数据访问代码。http://www.pwmis.cn/sqlMap/
'程序设计：Bluedoctor MSN:bluedoctors@msn.com
'Ver 1.2 Date 2008.4.4
'版本历史：
'Ver 1.0:支持VB.NET代码生成
'Ver 1.1:支持C# 代码生成
'Ver 1.2 支持生成实体类影射代码
'Ver 1.2.1 修改生成C# 代码时函数声明时候替换参数的BUG
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
        Dim VerMsg As String = "PDF.NET Code Maker : " & AppDomain.CurrentDomain.GetAssemblies(AppDomain.CurrentDomain.GetAssemblies.Length - 1).FullName & ",http://www.pwmis.com/SqlMap/"
        Console.WriteLine(VerMsg)
        WriteLog("============================================")
        WriteLog(VerMsg)
        WriteLog("Begin:" & DateTime.Now.ToString())

        Dim ConfigFile As String = ConfigurationManager.AppSettings("SqlMapConfigFile")  ' "E:\PWMIS\1.config"
        Dim RootNameSpace As String = ConfigurationManager.AppSettings("RootNameSpace")
        Dim OutPutPath As String = ConfigurationManager.AppSettings("OutPutPath")
        Dim ReWrite As Boolean = Convert.ToBoolean(ConfigurationManager.AppSettings("ReWrite"))
        Dim Language As CodeLanguage = [Enum].Parse(GetType(CodeLanguage), ConfigurationManager.AppSettings("CodeLanguage"))
       
        Console.WriteLine("已经读取配置信息，开始生成代码．．．")
        Console.WriteLine()
        Dim DoneMsg As String = String.Empty
        Try
            Dim TStart As DateTime = DateTime.Now
            Dim MakeCount As Integer = MakeCodeString(ConfigFile, RootNameSpace, OutPutPath, ReWrite, Language)
            If MakeCount > 0 Then
                Dim TEnd As DateTime = DateTime.Now
                Dim TS As TimeSpan = TEnd.Subtract(TStart)
                DoneMsg = "命令成功完成！生成文件 " & MakeCount.ToString() & "个，用时" & TS.Milliseconds.ToString() & "ms（按任意键结束）"
            Else
                DoneMsg = "未能够成功生成全部文件！"
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
        '写入日志
        If Not Directory.Exists("LOG") Then
            Directory.CreateDirectory("LOG")
        End If
        Dim writer As StreamWriter = New StreamWriter("LOG\Make.Log", True, System.Text.Encoding.Default)
        writer.WriteLine(Log)
        writer.Flush()
        writer.Close()
    End Sub

    Function MakeCodeString(ByVal SqlMapConfigFile As String, ByVal RootNameSpace As String, ByVal OutPutPath As String, Optional ByVal ReWrite As Boolean = True, Optional ByVal Language As CodeLanguage = CodeLanguage.VB) As Integer
        '处理输出目录
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
            Throw New Exception("代码模板文件未找到！")
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
                    '处理名称空间
                    Dim TypeName As String = Script.Attributes("Type").Value
                    Dim connectionString As String = ""
                    If Not Script.Attributes("ConnectionString") Is Nothing Then connectionString = Script.Attributes("ConnectionString").Value
                    Dim providerName As String = ""
                    If Not Script.Attributes("ProviderName") Is Nothing Then providerName = Script.Attributes("ProviderName").Value
                    If providerName = "" Then providerName = TypeName '默认使用TypeName作为提供程序名称

                    'Dim DataBaseType As PWMIS.Common.DBMSType = [Enum].Parse(GetType(PWMIS.Common.DBMSType), TypeName)
                    Dim DB As PWMIS.DataProvider.Data.AdoHelper = PWMIS.DataProvider.Adapter.MyDB.GetDBHelperByProviderString(providerName, connectionString)

                    '从4.0 开始，统一使用SQLMAP作为DAL名称空间，因为实际的类型是通过配置来的。
                    Dim CurrNameSpace As String = RootNameSpace & ".SqlMapDAL"  '& TypeName & "DAL"
                    Dim CurrOutPutPath As String = OutPutPath
                    If Not ReWrite Then '不允许重写代码文件
                        CurrOutPutPath = OutPutPath & TypeName & "\" & DateTime.Now.ToShortDateString & "\"
                    End If
                    If Not Directory.Exists(CurrOutPutPath) Then
                        Directory.CreateDirectory(CurrOutPutPath)
                    End If
                    If Script.HasChildNodes Then
                        '处理 类映射
                        For Each CommandClass As XmlNode In Script.ChildNodes
                            '如果是注释，返回
                            If CommandClass.NodeType <> XmlNodeType.Element Then Continue For

                            Dim CodeString As String = TemplateString
                            GetStringArea(CodeString, "<%R:REM[", "REM]%>", "使用该程序前请先引用程序集：PWMIS.Core，并且下面定义的名称空间前缀不要使用ＰＷＭＩＳ，更多信息，请查看 http://www.pwmis.com/sqlmap")
                            Dim FunTemplateString As String = GetStringArea(CodeString, "<%R:Function[", "Function]%>", "@FunTemplate") 'Mid(CodeString, ReFunAtB, ReFunAtE - ReFunAtB)

                            Dim CommandClassName As String = CommandClass.Attributes("Name").Value
                            Dim CommandClassMap As String '= CommandClass.Attributes("Class").Value
                            If Not CommandClass.Attributes("Class") Is Nothing AndAlso CommandClass.Attributes("Class").Value <> "" Then
                                CommandClassMap = CommandClass.Attributes("Class").Value
                            Else
                                CommandClassMap = CommandClassName
                            End If
                            '申明接口 2010.5.26
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

                            '处理映射的类名称 CommandClassMap
                            If CommandClassMap = "" Then
                                CommandClassMap = CommandClassName
                            Else
                                '处理类映射名称的当前限定的名称空间
                                Dim tempAt As Integer = InStr(1, CommandClassMap, ".", CompareMethod.Text)
                                If tempAt > 0 Then
                                    CurrNameSpace &= "." & Left(CommandClassMap, tempAt - 1)
                                    CommandClassMap = Mid(CommandClassMap, tempAt + 1)
                                End If
                            End If
                            '执行主类文本替换
                            CodeString = CodeString.Replace("#CurrNameSpace#", CurrNameSpace).Replace("#CommandClassMap#", CommandClassMap).Replace("#CommandClassName#", CommandClassName)
                            CodeString = CodeString.Replace("#CreateDateTime#", DateTime.Now.ToString()).Replace("#ClassDescription#", ClassDescription)
                            '替换接口
                            If CommandClassInterface <> "" Then
                                CommandClassInterface = "," + CommandClassInterface
                            End If
                            CodeString = CodeString.Replace("#CommandClassInterface#", CommandClassInterface).Replace("#EmbedAssemblySource#", EmbedAssemblySource)


                            If CommandClass.HasChildNodes Then
                                Dim ArrFunString(CommandClass.ChildNodes.Count()) As String
                                Dim nodeCount As Integer = 0
                                For Each node As XmlNode In CommandClass.ChildNodes
                                    '如果是注释，返回
                                    If node.NodeType <> XmlNodeType.Element Then Continue For

                                    '获取节点属性
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
                                        '结果类型为实体类型方可指定结果映射类型
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

                                    '获取SqlMapper 实例
                                    'Dim DataBaseType As PWMIS.Common.DBMSType = [Enum].Parse(GetType(PWMIS.Common.DBMSType), TypeName)
                                    'Dim DB As PWMIS.DataProvider.Data.AdoHelper = PWMIS.DataProvider.Adapter.MyDB.GetDBHelper(DataBaseType, "")
                                    Dim mapper As New SqlMapper
                                    mapper.DataBase = DB

                                    '获取命令参数信息
                                    Dim cmdInfo As CommandInfo = mapper.GetCommandInfoBySqlMapScript(SqlMapScript)
                                    Dim DataParameters() As IDataParameter = cmdInfo.DataParameters ' mapper.GetParameters(SqlMapScript)
                                    Dim ArrFunTemplateString() As String = FunTemplateString.Split(vbCrLf) '.ToCharArray()

                                    '生成函数说明
                                    ArrFunTemplateString(2) = ArrFunTemplateString(2).Replace("#CommandDescription#", CommandDescription)

                                    '生成参数注释
                                    Dim tempStr As String = String.Empty '/// <param name="#ParameterName#"></param>
                                    Dim ParamsLength As Integer = mapper.ParasLenth - 1
                                    For I As Integer = 0 To ParamsLength
                                        tempStr &= ArrFunTemplateString(5).Replace("#ParameterName#", mapper.ParamsMap(I).ParamName)
                                    Next
                                    ArrFunTemplateString(5) = tempStr

                                    '替换函数参数
                                    Dim FunParamsTemplate As String = GetStringArea(ArrFunTemplateString(8), "<%R:Params[", "Params]%>", "@FunParams")
                                    Dim Sepchar As String = GetStringArea(FunParamsTemplate, "[", "]", "")
                                    tempStr = String.Empty
                                    For I As Integer = 0 To ParamsLength
                                        tempStr &= FunParamsTemplate.Replace("#ParameterName#", mapper.ParamsMap(I).ParamName).Replace("#DataType#", mapper.ParamsMap(I).TypeCode.ToString())
                                        If I <> ParamsLength Then tempStr &= Sepchar
                                    Next
                                    '参数列表的替换参数处理
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

                                    '替换函数名
                                    ArrFunTemplateString(8) = ArrFunTemplateString(8).Replace("#Method#", Method)
                                    Dim ExecuteType As String = ""
                                    Select Case nodeName
                                        Case "Select", "Read"
                                            Select Case ResultClass
                                                Case "", enumResultClass.Default.ToString(), enumResultClass.DataSet.ToString()
                                                    ExecuteType = "ExecuteDataSet"
                                                    ResultClass = "DataSet"
                                                Case enumResultClass.EntityObject.ToString() '实体类型
                                                    If ResultClass = "" Then
                                                        Throw New Exception("函数返回值指定错误，不能指定一个空的结果实体！错误所在位置：" & ArrFunTemplateString(8))
                                                    End If
                                                    ExecuteType = "ExecuteReader"
                                                    ResultClass = ResultMap
                                                    '进行实体类转换
                                                Case enumResultClass.[EntityList].ToString() '实体类型
                                                    If ResultClass = "" Then
                                                        Throw New Exception("函数返回值指定错误，不能指定一个空的结果实体！错误所在位置：" & ArrFunTemplateString(8))
                                                    End If
                                                    ExecuteType = "ExecuteReaderList" '实体集合
                                                    If Language = CodeLanguage.VB Then
                                                        ResultClass = "List(Of " & ResultMap & ")"
                                                    ElseIf Language = CodeLanguage.CSharp Then
                                                        ResultClass = "List<" & ResultMap & ">"
                                                    End If

                                                    '进行实体类转换
                                                Case enumResultClass.ValueType.ToString()  '单值类型
                                                    ExecuteType = "ExecuteScalar"
                                                    ResultClass = "System.Object"
                                                Case enumResultClass.ObjectList.ToString() '一般对象列表，比如DTO列表
                                                    If ResultClass = "" Then
                                                        Throw New Exception("函数返回值指定错误，不能指定一个空的结果实体！错误所在位置：" & ArrFunTemplateString(8))
                                                    End If
                                                    ExecuteType = "ExecuteObjectList" '一般对象列表集合
                                                    If Language = CodeLanguage.VB Then
                                                        ResultClass = "List(Of " & ResultMap & ")"
                                                    ElseIf Language = CodeLanguage.CSharp Then
                                                        ResultClass = "List<" & ResultMap & ">"
                                                    End If

                                                Case Else
                                                    Throw New Exception("未知的结果类型！错误所在位置：" & ArrFunTemplateString(8))
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
                                            Throw New Exception("不支持的CRUD类型，请指定Create/Insert,Read/Select,Update,Delete 之一！")
                                    End Select

                                    '替换函数返回类型
                                    ArrFunTemplateString(8) = ArrFunTemplateString(8).Replace("#ResultClass#", ResultClass)

                                    '替换命令名
                                    ArrFunTemplateString(11) = ArrFunTemplateString(11).Replace("#CommandName#", CommandName)

                                    '参数赋值，索引方式
                                    tempStr = String.Empty
                                    For I As Integer = 0 To ParamsLength
                                        tempStr &= ArrFunTemplateString(14).Replace("#ParamIndex#", I.ToString).Replace("#ParameterName#", mapper.ParamsMap(I).ParamName)
                                    Next
                                    ArrFunTemplateString(14) = tempStr

                                    '参数赋值，命名方式
                                    tempStr = String.Empty
                                    For I As Integer = 0 To ParamsLength
                                        tempStr &= ArrFunTemplateString(18).Replace("#ParameterName#", mapper.ParamsMap(I).ParamName)
                                    Next
                                    ArrFunTemplateString(18) = tempStr

                                    '替换参数处理 ReplaceableParameter
                                    tempStr = String.Empty
                                    If mapper.ParamsReplaceable.Count > 0 Then
                                        For I As Integer = 0 To mapper.ParamsReplaceable.Count - 1
                                            tempStr &= ArrFunTemplateString(21).Replace("#ReplaceableParameter#", mapper.ParamsReplaceable(I))
                                        Next
                                    End If
                                    ArrFunTemplateString(21) = tempStr

                                    ArrFunTemplateString(23) = ArrFunTemplateString(23).Replace("#ExecuteType#", ExecuteType)
                                    '替换查询方式
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


                                    '生成完整的函数体
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
            doc.Load(ConfigFile) '可以考虑采用XML文件流的方式加快读取

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
            doc.Load(ConfigFile) '可以考虑采用XML文件流的方式加快读取
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
