Imports System.Windows.Forms

Imports System.Drawing.Design

Imports System.Windows.Forms.Design



Namespace PDFDotNET

    ''' <summary>
    ''' 属性浏览器文件选择项编辑器
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PropertyGridFileItem
        Inherits UITypeEditor



        Public Overrides Function GetEditStyle(ByVal context As System.ComponentModel.ITypeDescriptorContext) As UITypeEditorEditStyle


            Return UITypeEditorEditStyle.Modal

        End Function



        Public Overrides Function EditValue(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal provider As System.IServiceProvider, ByVal value As Object) As Object


            Dim edSvc As IWindowsFormsEditorService = DirectCast(provider.GetService(GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)

            If edSvc IsNot Nothing Then


                ' 可以打开任何特定的对话框

                Dim dialog As New OpenFileDialog()

                dialog.AddExtension = False

                If dialog.ShowDialog().Equals(DialogResult.OK) Then



                    Return dialog.FileName

                End If
            End If

            Return value

        End Function

    End Class

    ''' <summary>
    ''' 属性浏览器目录选择项编辑器
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PropertyGridFolderItem
        Inherits UITypeEditor



        Public Overrides Function GetEditStyle(ByVal context As System.ComponentModel.ITypeDescriptorContext) As UITypeEditorEditStyle


            Return UITypeEditorEditStyle.Modal

        End Function



        Public Overrides Function EditValue(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal provider As System.IServiceProvider, ByVal value As Object) As Object


            Dim edSvc As IWindowsFormsEditorService = DirectCast(provider.GetService(GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)

            If edSvc IsNot Nothing Then


                ' 可以打开任何特定的对话框

                Dim dialog As New FolderBrowserDialog()


                If dialog.ShowDialog().Equals(DialogResult.OK) Then



                    Return dialog.SelectedPath

                End If
            End If

            Return value

        End Function

    End Class

End Namespace