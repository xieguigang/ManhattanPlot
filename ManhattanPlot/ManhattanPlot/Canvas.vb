Imports Microsoft.VisualBasic.Language

Public Module Canvas

    ''' <summary>
    ''' + https://en.wikipedia.org/wiki/Chromosome_%n_(human) n=[1,22]
    ''' + https://en.wikipedia.org/wiki/X_chromosome
    ''' + https://en.wikipedia.org/wiki/Y_chromosome
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Chromosomes As New Dictionary(Of String, Integer) From {
        {"1", 247249719},
        {"2", 242193529},
        {"3", 198295559},
        {"4", 190214555},
        {"5", 181538259},
        {"6", 170805979},
        {"7", 159345973},
        {"8", 145138636},
        {"9", 138394717},
        {"10", 133797422},
        {"11", 13586622},
        {"12", 133275309},
        {"13", 114364328},
        {"14", 10743718},
        {"15", 101991189},
        {"16", 90338345},
        {"17", 83257441},
        {"18", 80373285},
        {"19", 58617616},
        {"20", 6325520},
        {"21", 48129895},
        {"22", 51304566},
        {"X", 156040895},
        {"Y", 57227415}
    }

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