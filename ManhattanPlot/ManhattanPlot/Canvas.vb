Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic

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

            Dim gData = From x As SNP
                        In data
                        Select x
                        Group x By x.Chr Into Group
                        Order By Chr Ascending

            Dim total As Long = 0
            Dim chrData As New List(Of NamedValue(Of SNP()))

            For Each chromsome In gData
                Dim id As String = chromsome.Chr.ToUpper

                total += Canvas.Chromosomes(id)
                chrData += New NamedValue(Of SNP()) With {
                    .Name = id,
                    .x = LinqAPI.Exec(Of SNP) <= From x As SNP
                                                 In chromsome.Group
                                                 Select x
                                                 Order By x.Position Ascending
                    }
            Next

            Dim xLeft As Integer = 0
            Dim maxY As Double = chrData _
                .Select(Function(x) x.x _
                .Select(Function(o) o.pvalues.Values) _
                .MatrixAsIterator) _
                .MatrixAsIterator _
                .Where(Function(n) Not Double.IsNaN(n)).Min  ' pvalue越小则-log越大
            maxY = -Math.Log(maxY) + 1.5

            For Each chromsome In chrData
                Dim l As Integer = Chromosomes(chromsome.Name)
                Dim max As Integer = width * (l / total)  '  最大的长度

                For Each snp As SNP In chromsome.x
                    Dim x As Integer = max * (snp.Position / l) + xLeft

                    For Each sample In snp.pvalues.Where(Function(n) Not Double.IsNaN(n.Value))
                        Dim y As Integer = height - height * ((-Math.Log(sample.Value)) / maxY)
                        Call g.FillPie(Brushes.Brown, New Rectangle(x, y, 10, 10), 0, 360)
                    Next
                Next

                xLeft += max
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