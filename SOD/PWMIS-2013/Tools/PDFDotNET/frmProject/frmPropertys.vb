Public Class frmPropertys

    Public Parameters As Dictionary(Of String, Object)

    Public Sub New(ByVal paras As Dictionary(Of String, Object))
        InitializeComponent()

        Me.Parameters = paras

    End Sub

    Public Sub New()

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。

    End Sub

    Private Sub frmPropertys_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.DataGridView1.Columns(0).DataPropertyName = ""

    End Sub
End Class