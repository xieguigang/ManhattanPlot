Imports Microsoft.VisualBasic.Language

Public Class SNP : Inherits ClassObject

    Public Property SNP As String
    Public Property Chr As String
    Public Property Position As Integer
    Public Property Gene As String
    Public Property pvalues As Dictionary(Of String, Double)

End Class

Public Class SampleColor
    Public Property SampleName As String
    Public Property Color As String
End Class