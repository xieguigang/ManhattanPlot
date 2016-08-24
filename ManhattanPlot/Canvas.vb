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
    Public Function Plot(data As IEnumerable(Of SNP),
                         Optional width As Integer = 3000,
                         Optional height As Integer = 1440,
                         Optional colors As String() = Nothing,
                         Optional margin As Size = Nothing,
                         Optional ptSize As Integer = 10,
                         Optional showDebugLabel As Boolean = False,
                         Optional equidistant As Boolean = False) As Bitmap

        Dim bmp As New Bitmap(width, height)
        Dim cls As Color() = If(colors.IsNullOrEmpty,
            ColorExtensions.ChartColors.Shuffles,
            colors.ToArray(AddressOf ToColor))

        If margin.IsEmpty Then
            margin = New Size(15, 25)
        End If

        Using g As Graphics = Graphics.FromImage(bmp)
            Dim serials As IEnumerable(Of String) = data.First.pvalues.Keys   ' 先绘制出系列的名称
            Dim h As Integer = margin.Height
            Dim left As Integer = width - 300
            Dim font As New Font(FontFace.MicrosoftYaHei, 12, FontStyle.Regular)
            Dim fsz As SizeF = g.MeasureString("0", font)
            Dim sampleBrush As New Dictionary(Of String, SolidBrush)

            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.FillRectangle(Brushes.White, New Rectangle(New Point, New Size(width, height)))

            For Each name As SeqValue(Of String) In serials.SeqIterator   ' legend绘制
                Dim br As SolidBrush = New SolidBrush(cls(name.i))

                Call g.FillRectangle(br, New Rectangle(left, h, 100, fsz.Height))
                Call g.DrawString(name.obj, font, Brushes.Black, New Point(left + 110, h))

                h += fsz.Height + 10
                sampleBrush(name.obj) = br
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

            Dim xLeft As Integer = margin.Width
            Dim maxY As Double = chrData _
                .Select(Function(x) x.x _
                .Select(Function(o) o.pvalues.Values) _
                .MatrixAsIterator) _
                .MatrixAsIterator _
                .Where(Function(n) Not Double.IsNaN(n)).Min  ' pvalue越小则-log越大
            maxY = -Math.Log(maxY) + 1
            chrData = New List(Of NamedValue(Of SNP()))(chrData.OrderBy(Function(x) Val(x.Name)))

            ' 绘制X轴
            Call g.DrawLine(Pens.Black, New Point(margin.Width, height - margin.Height), New Point(width - margin.Width, height - margin.Height))
            ' 绘制y轴
            Call g.DrawLine(Pens.Black, New Point(margin.Width, height - margin.Height), New Point(margin.Width, margin.Height))

            For iy As Double = 0.5 To maxY Step (maxY / 10)
                Dim y As Integer = height - height * ((-Math.Log(iy)) / maxY) - 2 * margin.Height
                Call g.DrawLine(Pens.Black, New Point(margin.Width, y), New Point(margin.Width - 4, y))
            Next

            Dim labelFont As New Font(FontFace.BookmanOldStyle, 4)
            Dim ed As Integer = (width - 2 * margin.Width) / chrData.Count

            For Each chromsome In chrData
                Dim l As Integer = Chromosomes(chromsome.Name)
                Dim max As Integer = If(equidistant, ed, (width - 2 * margin.Width) * (l / total))  '  最大的长度

                g.DrawLine(Pens.Black, New Point(xLeft + max, height - margin.Height), New Point(xLeft + max, height - margin.Height - 5))
                fsz = g.MeasureString(chromsome.Name, font)
                g.DrawString(chromsome.Name, font, Brushes.Black, New Point(xLeft + (max - fsz.Width) / 2, height - margin.Height + 5))

                For Each snp As SNP In chromsome.x
                    Dim x As Integer = max * (snp.Position / l) + xLeft

                    For Each sample In snp.pvalues.Where(Function(n) Not Double.IsNaN(n.Value))
                        Dim y As Integer = height - height * ((-Math.Log(sample.Value)) / maxY) - 2 * margin.Height
                        Dim label As String = $"{snp.Gene} ({sample.Key})"

                        Call g.FillPie(sampleBrush(sample.Key), New Rectangle(x, y, ptSize, ptSize), 0, 360)
                        If showDebugLabel Then
                            Call g.DrawString(label,
                                              labelFont,
                                              Brushes.LightGray,
                                              New Point(x + ptSize + 3, y + 10))
                        End If
                    Next
                Next

                xLeft += max
            Next
        End Using

        Return bmp
    End Function
End Module