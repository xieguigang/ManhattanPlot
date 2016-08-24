Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq

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

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="width"></param>
    ''' <param name="height"></param>
    ''' <param name="colors">colorName/rgb(a,r,g,b)</param>
    ''' <returns></returns>
    <Extension>
    Public Function Plot(data As IEnumerable(Of SNP), Optional width As Integer = 1024, Optional height As Integer = 768, Optional colors As String() = Nothing) As Bitmap
        Dim bmp As New Bitmap(width, height)
        Dim cls As Color() = If(colors.IsNullOrEmpty,
            ColorExtensions.ChartColors.Shuffles,
            colors.ToArray(AddressOf ToColor))

        Using g As Graphics = Graphics.FromImage(bmp)
            Dim serials As IEnumerable(Of String) = data.First.pvalues.Keys   ' 先绘制出系列的名称
            Dim h As Integer = 0
            Dim left As Integer = width - 200
            Dim font As New Font(FontFace.Cambria, 12, FontStyle.Regular)
            Dim fsz As SizeF = g.MeasureString("0", font)

            For Each name As SeqValue(Of String) In serials.SeqIterator
                Call g.FillRectangle(New SolidBrush(cls(name.i)), New Rectangle(left, h, 100, fsz.Height))
                Call g.DrawString(name.obj, font, Brushes.Black, New Point(left + 110, h))
                h += fsz.Height
            Next
        End Using

        Return bmp
    End Function
End Module

Public Class SNP : Inherits ClassObject

    Public Property SNP As String
    Public Property Chr As String
    Public Property Position As Integer
    Public Property Gene As String
    Public Property pvalues As Dictionary(Of String, Double)

End Class