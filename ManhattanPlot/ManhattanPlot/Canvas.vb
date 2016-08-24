Imports Microsoft.VisualBasic.Language

Public Module Canvas

    Public Function Plot() As Image

    End Function
End Module

Public Class SNP : Inherits ClassObject

    Public Property SNP As String
    Public Property Chr As String
    Public Property Position As Integer
    Public Property Gene As String
    Public Property pvalues As Dictionary(Of String, Double)

End Class