Imports System.Xml

Module Module1

    Structure ListItem
        Dim ItemText As String
        Dim ItemValue As String
    End Structure

    Public CurrDataBase As PWMIS.DataProvider.Data.AdoHelper
    Public CurrDataBaseType As PWMIS.Common.DBMSType

    '���������ļ�
    Function CreateConfigFile(ByVal ConfigFile As String) As Boolean
        'strScriptBlock ="<?xml version="1.0" encoding="utf-8"?>" _
        '"<!-- PWMIS SqlMap Ver 1.0.1 ,2006.11.7,http://www.pwmis.cn/SqlMap/ -->" _  
        '"<SqlMap><Script Type="Access" Version="2000,2002,2003"/></SqlMap>"
        Dim _ErrDescription As String
        If ConfigFile = "" Then Exit Function
        Try
            Dim writer As New XmlTextWriter(ConfigFile, System.Text.Encoding.UTF8)
            Dim CommentMsg As String = vbCrLf & "PWMIS SqlMap Ver 1.1.2 ,2006-11-22,http://www.pwmis.com/SqlMap/" & _
            vbCrLf & "Config by SqlMap Builder,Date:" & DateTime.Today.ToShortDateString & vbCrLf
            With writer
                .Formatting = Formatting.Indented
                .Indentation = 4
                .WriteStartDocument()
                .WriteComment(CommentMsg)
                .WriteStartElement("SqlMap")
                .WriteStartElement("Script")
                .WriteAttributeString("Type", "Access")
                .WriteAttributeString("Version", "2000,2002,2003")
                .WriteEndElement()
                .WriteEndElement()
                .Close()
            End With
            Return True
        Catch ex As Exception
            _ErrDescription = ex.Message
        End Try
    End Function

    Function GetCommandClassNode(ByVal ConfigFile As String, ByVal ScriptType As String, ByVal CommandClassName As String) As XmlNode
        Dim doc As XmlDocument = New XmlDocument
        Dim _ErrDescription As String
        Try
            doc.Load(ConfigFile) '���Կ��ǲ���XML�ļ����ķ�ʽ�ӿ��ȡ

            Dim SqlScripts As XmlNode
            Dim root As XmlElement = doc.DocumentElement
            SqlScripts = root.SelectSingleNode("/SqlMap/Script[@Type='" & ScriptType & "']/CommandClass[@Name='" & CommandClassName & "']")
            Return SqlScripts
        Catch ex As Exception
            _ErrDescription = ex.Message
        End Try
    End Function

#Region "�ű������"


    '���ӽű���
    Function AddScriptType(ByVal ConfigFile As String, ByVal ScriptType As String, ByVal Version As String) As Boolean
        If ConfigFile = "" Then Exit Function
        Dim doc As XmlDocument = New XmlDocument
        Dim _ErrDescription As String
        Try
            doc.Load(ConfigFile) '���Կ��ǲ���XML�ļ����ķ�ʽ�ӿ��ȡ
            Dim SqlMap As XmlNode
            Dim root As XmlElement = doc.DocumentElement
            SqlMap = root.SelectSingleNode("/SqlMap")
            If Not SqlMap Is Nothing Then
                Dim ScriptNode As XmlNode = SqlMap.SelectSingleNode("//Script[@Type='" & ScriptType & "']")
                If ScriptNode Is Nothing Then
                    Dim node As XmlElement = doc.CreateElement("Script")
                    node.SetAttribute("Type", ScriptType)
                    node.SetAttribute("Version", Version)
                    node.SetAttribute("ConnectionString", "")
                    SqlMap.AppendChild(node)
                    doc.Save(ConfigFile)
                    Return True
                Else
                    _ErrDescription = "�Ѿ����ڽű�����" & ScriptType
                    Return False
                End If
                
            End If
        Catch ex As Exception
            _ErrDescription = ex.Message
        End Try
    End Function

    ''' <summary>
    ''' ��SQL�����ļ������ȡ�����ַ���
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetConnectionConfig(ByVal ConfigFile As String, ByVal ScriptType As String) As String
        Dim doc As XmlDocument = New XmlDocument
        Dim _ErrDescription As String
        Try
            doc.Load(ConfigFile) '���Կ��ǲ���XML�ļ����ķ�ʽ�ӿ��ȡ
            Dim node As XmlNode
            Dim root As XmlElement = doc.DocumentElement
            node = root.SelectSingleNode("/SqlMap/Script[@Type='" & ScriptType & "']")
            If Not node Is Nothing Then
                If Not node.Attributes("ConnectionString") Is Nothing Then
                    Return node.Attributes("ConnectionString").Value
                Else
                    Return ""
                End If
                
            End If
        Catch ex As Exception
            _ErrDescription = ex.Message
        End Try
        Return ""
    End Function

    ''' <summary>
    '''  ��SQL�����ļ����汣�������ַ���
    ''' </summary>
    ''' <param name="ConfigFile"></param>
    ''' <param name="ScriptType"></param>
    ''' <param name="ConnectionString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function SetConnectionConfig(ByVal ConfigFile As String, ByVal ScriptType As String, ByVal ConnectionString As String) As Boolean
        Dim doc As XmlDocument = New XmlDocument
        Dim _ErrDescription As String
        Try
            doc.Load(ConfigFile) '���Կ��ǲ���XML�ļ����ķ�ʽ�ӿ��ȡ
            Dim node As XmlNode
            Dim root As XmlElement = doc.DocumentElement
            node = root.SelectSingleNode("/SqlMap/Script[@Type='" & ScriptType & "']")
            If Not node Is Nothing Then
                If node.Attributes("ConnectionString") Is Nothing Then
                    node.Attributes.Append(doc.CreateAttribute("ConnectionString"))

                End If
                node.Attributes("ConnectionString").Value = ConnectionString
                doc.Save(ConfigFile)
                Return True
            End If
        Catch ex As Exception
            _ErrDescription = ex.Message
        End Try
        Return False
    End Function

    'ɾ���ű��飬������������нڵ�
    Function DeleteScriptType(ByVal ConfigFile As String, ByVal ScriptType As String) As Boolean
        If ConfigFile = "" Then Exit Function
        Dim doc As XmlDocument = New XmlDocument
        Dim _ErrDescription As String
        Try
            doc.Load(ConfigFile) '���Կ��ǲ���XML�ļ����ķ�ʽ�ӿ��ȡ
            Dim SqlMap As XmlNode
            Dim root As XmlElement = doc.DocumentElement
            SqlMap = root.SelectSingleNode("/SqlMap")
            If Not SqlMap Is Nothing Then
                Dim node As XmlNode = SqlMap.SelectSingleNode("//Script[@Type='" & ScriptType & "']")
                If Not node Is Nothing Then
                    SqlMap.RemoveChild(node)
                    doc.Save(ConfigFile)
                    Return True
                End If
            End If
        Catch ex As Exception
            _ErrDescription = ex.Message
        End Try
    End Function

#End Region

#Region "���Ѿ����ڵĽű����������б�ؼ��б���Ŀ��"


    ''' <summary>
    ''' ��ȡ�Ѿ����ڵĽű�����
    ''' </summary>
    ''' <param name="ConfigFile">�����ļ�</param>
    ''' <param name="cmbScriptType">(���ݿ�)�ű�����</param>
    ''' <remarks></remarks>
    Sub GetExistsScriptType(ByVal ConfigFile As String, ByVal cmbScriptType As System.Windows.Forms.ComboBox)
        If ConfigFile = "" Then Exit Sub
        cmbScriptType.Items.Clear()
        Dim SqlScripts As XmlNodeList = GetScriptNodeList(ConfigFile)
        If Not SqlScripts Is Nothing Then
            For Each node As XmlNode In SqlScripts
                If Not node.Attributes("Type") Is Nothing Then
                    cmbScriptType.Items.Add(node.Attributes("Type").Value)
                End If
            Next
        End If

    End Sub

    ''' <summary>
    ''' ��ȡ�Ѿ����ڵĽű�����
    ''' </summary>
    ''' <param name="ConfigFile">�����ļ�</param>
    ''' <param name="ArrListItem"></param>
    ''' <param name="cmbScriptType">(���ݿ�)�ű�����</param>
    ''' <remarks></remarks>
    Sub GetExistsScriptType(ByVal ConfigFile As String, ByRef ArrListItem As ArrayList, ByVal cmbScriptType As System.Windows.Forms.ListBox)
        If ConfigFile = "" Then Exit Sub
        ArrListItem.Clear()
        cmbScriptType.Items.Clear()
        Dim SqlScripts As XmlNodeList = GetScriptNodeList(ConfigFile)
        If Not SqlScripts Is Nothing Then
            For Each node As XmlNode In SqlScripts
                If Not node.Attributes("Type") Is Nothing AndAlso Not node.Attributes("Version") Is Nothing Then
                    Dim Item As ListItem
                    Item.ItemText = node.Attributes("Type").Value
                    Item.ItemValue = node.Attributes("Version").Value
                    ArrListItem.Add(Item)
                    cmbScriptType.Items.Add(Item.ItemText)
                End If
            Next
        End If

    End Sub

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
        End Try
        Return Nothing

    End Function

#End Region

#Region "���������"

    Sub GetClassNameList(ByVal ConfigFile As String, ByVal ScriptType As String, ByVal lstClassName As System.Windows.Forms.ListBox)
        Dim doc As XmlDocument = New XmlDocument
        Dim _ErrDescription As String
        lstClassName.Items.Clear()
        Try
            doc.Load(ConfigFile) '���Կ��ǲ���XML�ļ����ķ�ʽ�ӿ��ȡ

            Dim SqlScripts As XmlNode
            Dim root As XmlElement = doc.DocumentElement
            SqlScripts = root.SelectSingleNode("/SqlMap/Script[@Type='" & ScriptType & "']")
            If Not SqlScripts Is Nothing AndAlso SqlScripts.HasChildNodes Then
                For Each node As XmlNode In SqlScripts.ChildNodes
                    If node.NodeType = XmlNodeType.Element AndAlso Not node.Attributes("Name") Is Nothing Then
                        lstClassName.Items.Add(node.Attributes("Name").Value)
                    End If
                Next
            End If
        Catch ex As Exception
            _ErrDescription = ex.Message
        End Try
    End Sub

    '���������
    Function AddCommandClass(ByVal ConfigFile As String, ByVal ScriptType As String, ByVal Name As String, ByVal ClassName As String, ByVal Description As String, ByVal CommandInterface As String) As Boolean
        Dim doc As XmlDocument = New XmlDocument
        Dim _ErrDescription As String
        Try
            doc.Load(ConfigFile) '���Կ��ǲ���XML�ļ����ķ�ʽ�ӿ��ȡ
            Dim SqlScripts As XmlNode
            Dim root As XmlElement = doc.DocumentElement
            SqlScripts = root.SelectSingleNode("/SqlMap/Script[@Type='" & ScriptType & "']")
            If Not SqlScripts Is Nothing Then
                Dim node As XmlElement = doc.CreateElement("CommandClass")
                node.SetAttribute("Name", Name)
                node.SetAttribute("Class", ClassName)
                node.SetAttribute("Description", Description)
                node.SetAttribute("Interface", CommandInterface)
                SqlScripts.AppendChild(node)
                doc.Save(ConfigFile)
                Return True
            End If
        Catch ex As Exception
            _ErrDescription = ex.Message
        End Try
    End Function

    '�޸�������
    Function EditCommandClass(ByVal ConfigFile As String, ByVal ScriptType As String, ByVal OldName As String, ByVal NewName As String, ByVal ClassName As String, ByVal Description As String, ByVal CommandInterface As String) As Boolean
        Dim doc As XmlDocument = New XmlDocument
        Dim _ErrDescription As String
        Try
            doc.Load(ConfigFile) '���Կ��ǲ���XML�ļ����ķ�ʽ�ӿ��ȡ
            Dim node As XmlElement
            Dim root As XmlElement = doc.DocumentElement
            node = root.SelectSingleNode("/SqlMap/Script[@Type='" & ScriptType & "']/CommandClass[@Name='" & OldName & "']")
            If Not node Is Nothing Then
                node.SetAttribute("Name", NewName)
                node.SetAttribute("Class", ClassName)
                node.SetAttribute("Description", Description)
                node.SetAttribute("Interface", CommandInterface)
                doc.Save(ConfigFile)
                Return True
            End If
        Catch ex As Exception
            _ErrDescription = ex.Message
        End Try
    End Function

    'ɾ��������
    Function DeleteCommandClass(ByVal ConfigFile As String, ByVal ScriptType As String, ByVal Name As String) As Boolean
        Dim doc As XmlDocument = New XmlDocument
        Dim _ErrDescription As String
        Try
            doc.Load(ConfigFile) '���Կ��ǲ���XML�ļ����ķ�ʽ�ӿ��ȡ
            Dim node As XmlNode
            Dim root As XmlElement = doc.DocumentElement
            node = root.SelectSingleNode("/SqlMap/Script[@Type='" & ScriptType & "']")
            If Not node Is Nothing AndAlso node.HasChildNodes() Then
                For Each nodeTemp As XmlNode In node.ChildNodes
                    If nodeTemp.NodeType = XmlNodeType.Element _
                       AndAlso Not nodeTemp.Attributes("Name") Is Nothing _
                       AndAlso nodeTemp.Attributes("Name").Value = Name Then

                        node.RemoveChild(nodeTemp)
                        Exit For
                    End If
                Next
                doc.Save(ConfigFile)
                Return True
            End If
        Catch ex As Exception
            _ErrDescription = ex.Message
        End Try
    End Function


#End Region

End Module
