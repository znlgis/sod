Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports PWMIS.DataMap.Entity

Public Class User
    Inherits EntityBase
    Public Sub New()
        Me.TableName = "User"
        Me.IdentityName = "UserID"
        Me.PrimaryKeys.Add("UserID")
    End Sub

    Protected Overrides Sub SetFieldNames()
        PropertyNames = New String() {"UserID", "UserName", "UserType", "RegisterDate", "Expenditure"}
    End Sub

    Public Property UserID() As Integer
        Get
            Return getProperty(Of Integer)("UserID")
        End Get
        Set(value As Integer)
            setProperty("UserID", value)
        End Set
    End Property

    Public Property UserName() As String
        Get
            Return getProperty(Of String)("UserName")
        End Get
        Set(value As String)
            setProperty("UserName", value, 50)
        End Set
    End Property

    Public Property UserType() As Integer
        Get
            Return getProperty(Of Integer)("UserType")
        End Get
        Set(value As Integer)
            setProperty("UserType", value)
        End Set
    End Property


    Public Property RegisterDate() As DateTime
        Get
            Return getProperty(Of DateTime)("RegisterDate")
        End Get
        Set(value As DateTime)
            setProperty("RegisterDate", value)
        End Set
    End Property

    Public Property Expenditure() As [Single]
        Get
            Return getProperty(Of [Single])("Expenditure")
        End Get
        Set(value As [Single])
            setProperty("Expenditure", value)
        End Set
    End Property
End Class
