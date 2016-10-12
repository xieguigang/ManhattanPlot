Imports System.Drawing.Drawing2D
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq

''' <summary>
''' The ``Manhattan Plot`` canvas
''' </summary>
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
        {"X", 156040895}, {"23", 156040895},
        {"Y", 57227415}, {"24", 57227415}
    }

    Public ReadOnly Property ChromosomeColors As New Dictionary(Of String, SolidBrush) From {
        {"1", New SolidBrush(Color.FromArgb(239, 80, 84))},
        {"2", New SolidBrush(Color.FromArgb(80, 94, 169))},
        {"3", New SolidBrush(Color.FromArgb(200, 103, 170))},
        {"4", New SolidBrush(Color.FromArgb(246, 174, 177))},
        {"5", New SolidBrush(Color.FromArgb(132, 132, 132))},
        {"6", New SolidBrush(Color.FromArgb(198, 34, 43))},
        {"7", New SolidBrush(Color.FromArgb(34, 66, 153))},
        {"8", New SolidBrush(Color.FromArgb(39, 180, 76))},
        {"9", New SolidBrush(Color.FromArgb(199, 192, 50))},
        {"10", New SolidBrush(Color.FromArgb(164, 55, 148))},
        {"11", New SolidBrush(Color.FromArgb(9, 187, 191))},
        {"12", New SolidBrush(Color.FromArgb(64, 64, 64))},
        {"13", New SolidBrush(Color.FromArgb(239, 54, 66))},
        {"14", New SolidBrush(Color.FromArgb(71, 88, 168))},
        {"15", New SolidBrush(Color.FromArgb(193, 91, 164))},
        {"16", New SolidBrush(Color.FromArgb(195, 197, 192))},
        {"17", New SolidBrush(Color.FromArgb(128, 22, 24))},
        {"18", New SolidBrush(Color.FromArgb(44, 46, 123))},
        {"19", New SolidBrush(Color.FromArgb(7, 129, 70))},
        {"20", New SolidBrush(Color.FromArgb(123, 135, 53))},
        {"21", New SolidBrush(Color.FromArgb(131, 41, 129))},
        {"22", New SolidBrush(Color.FromArgb(3, 133, 133))},
        {"X", New SolidBrush(Color.SkyBlue)}, {"23", New SolidBrush(Color.SkyBlue)},
        {"Y", New SolidBrush(Color.Lime)}, {"24", New SolidBrush(Color.Lime)}
    }

    ReadOnly __odds As New SolidBrush(Color.Orange)
    ReadOnly __evens As New SolidBrush(Color.Black)

    Public Function nln(x As Double) As Double
        Return -Math.Log(x)
    End Function

    Public Function nlog(x As Double) As Double
        Return -Math.Log(x, 10)
    End Function

    Public Function raw(x As Double) As Double
        Return x
    End Function

    Public Function GetValueMethod(name As String) As Func(Of Double, Double)
        Select Case name.ToLower
            Case "ln" : Return AddressOf nln
            Case "log" : Return AddressOf nlog
            Case Else
                Return AddressOf raw
        End Select
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="width"></param>
    ''' <param name="height"></param>
    ''' <param name="colors">colorName/rgb(a,r,g,b)</param>
    ''' <param name="ylog">``ln``, ``log``, ``raw``</param>
    ''' <param name="colorPattern">``chr``, ``sampleName``, ``interval``</param>
    ''' <returns></returns>
    <Extension>
    Public Function Plot(data As IEnumerable(Of SNP),
                         Optional width As Integer = 3000,
                         Optional height As Integer = 1440,
                         Optional colors As Dictionary(Of String, String) = Nothing,
                         Optional margin As Size = Nothing,
                         Optional ptSize As Integer = 10,
                         Optional showDebugLabel As Boolean = False,
                         Optional equidistant As Boolean = False,
                         Optional relative As Boolean = False,
                         Optional ylog As String = "ln",
                         Optional colorPattern As String = "chr",
                         Optional bg$ = "white") As Bitmap

        Dim log As Func(Of Double, Double) = GetValueMethod(ylog)
        Dim colorList As New Dictionary(Of String, Color)

        data = data.ToArray

        For Each x In data
            If x.pvalues.ContainsKey("") Then
                Call x.pvalues.Remove("")
            End If
        Next

        If colors.IsNullOrEmpty Then
            Dim charts = ChartColors.Shuffles

            For Each tag As SeqValue(Of String) In data.First.pvalues.Keys.SeqIterator
                colorList(tag.obj) = charts(tag.i)
            Next
        Else
            For Each x In colors
                colorList(x.Key) = x.Value.ToColor
            Next
        End If

        Return g.GraphicsPlots(New Size(width, height), margin, "width",
            Sub(ByRef g, region)

                Dim serials As IEnumerable(Of String) = data.First.pvalues.Keys   ' 先绘制出系列的名称
                Dim h As Integer = margin.Height
                Dim left As Integer = width - 300
                Dim font As New Font(FontFace.MicrosoftYaHei, 12, FontStyle.Regular)
                Dim fsz As SizeF = g.MeasureString("0", font)
                Dim sampleBrush As New Dictionary(Of String, SolidBrush)

                colorPattern = colorPattern.ToLower
                g.CompositingQuality = CompositingQuality.HighQuality
                g.FillRectangle(Brushes.White, New Rectangle(New Point, New Size(width, height)))

                If colorPattern <> "samplename" Then ' 不是按照样品标注颜色的，则不再绘制legend

                Else
                    For Each name As String In serials  ' legend绘制
                        Dim br As New SolidBrush(colorList(name))

                        Call g.FillRectangle(br, New Rectangle(left, h, 100, fsz.Height))
                        Call g.DrawString(
                            If(String.IsNullOrEmpty(name), "null", name),
                            font,
                            Brushes.Black,
                            New Point(left + 110, h))

                        h += fsz.Height + 10
                        sampleBrush(name) = br
                    Next
                End If

                Dim gData = From x As SNP
                            In data
                            Select x
                            Group x By x.Chr Into Group
                            Order By Chr Ascending

                Dim total As Long = 0
                Dim chrData As New List(Of NamedValue(Of SNP()))

                For Each chromsome In gData
                    Dim id As String = chromsome.Chr.ToUpper
                    Dim relLen As Integer() = chromsome _
                        .Group _
                        .ToArray(Function(x) x.Position)

                    total += If(relative, relLen.Max - relLen.Min, Canvas.Chromosomes(id))
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
                maxY = log(maxY) + 1
                chrData = New List(Of NamedValue(Of SNP()))(chrData.OrderBy(Function(x) Val(x.Name)))
                font = New Font(FontFace.MicrosoftYaHei, 24, FontStyle.Regular)
                ylog = ylog.ToLower

                ' 绘制X轴
                Call g.DrawLine(Pens.Black, New Point(margin.Width, height - margin.Height), New Point(width - margin.Width, height - margin.Height))
                ' 绘制y轴
                Call g.DrawLine(Pens.Black, New Point(margin.Width, height - margin.Height), New Point(margin.Width, margin.Height))

                If ylog <> "ln" AndAlso ylog <> "log" Then
                    For iy As Double = 0 To maxY Step 0.25
                        Dim y As Integer = height - (height - 2 * margin.Height) * (iy / maxY) - margin.Height

                        fsz = g.MeasureString(iy, font)

                        Call g.DrawLine(Pens.Black, New Point(margin.Width, y), New Point(margin.Width - 4, y))
                        Call g.DrawString(iy, font, Brushes.Black, New Point(15, y - fsz.Height / 2))
                    Next
                Else
                    For iy As Double = 1 To maxY
                        Dim y As Integer = height - (height - 2 * margin.Height) * (iy / maxY) - margin.Height

                        fsz = g.MeasureString(iy, font)

                        Call g.DrawLine(Pens.Black, New Point(margin.Width, y), New Point(margin.Width - 4, y))
                        Call g.DrawString(iy, font, Brushes.Black, New Point(25, y - fsz.Height / 2))
                    Next
                End If

                Dim labelFont As New Font(FontFace.BookmanOldStyle, 16, FontStyle.Bold)
                Dim ed As Integer = (width - 2 * margin.Width) / chrData.Count
                Dim r As Double = ptSize / 2
                Dim odds As Boolean = True

                For Each chromsome In chrData
                    Dim chrName As String = chromsome.Name
                    Dim relLen As Integer() = chromsome.x.ToArray(Function(x) x.Position)
                    Dim l As Integer = If(relative, relLen.Max - relLen.Min, Chromosomes(chrName))
                    Dim max As Integer = If(equidistant, ed, (width - 2 * margin.Width) * (l / total))  '  最大的长度

                    If l = 0 AndAlso relLen.Length = 1 Then
                        l = relLen(Scan0)
                    End If
                    If chrName = "23" Then
                        chrName = "X"
                    ElseIf chrName = "24" Then
                        chrName = "Y"
                    End If

                    g.DrawLine(Pens.Black, New Point(xLeft + max, height - margin.Height), New Point(xLeft + max, height - margin.Height - 5))
                    fsz = g.MeasureString(chrName, font)
                    g.DrawString(chrName, font, Brushes.Black, New Point(xLeft + (max - fsz.Width) / 2, height - margin.Height + 15))

                    For Each snp As SNP In chromsome.x
                        Dim x As Integer = max * (If(relative, snp.Position - relLen.Min, snp.Position) / l) + xLeft

                        If colorPattern <> "chr" Then

                            If snp.Position > l Then
                                Continue For
                            End If
                        End If

                        For Each sample In snp.pvalues.Where(Function(n) Not Double.IsNaN(n.Value))
                            Dim y As Integer = height - (height - 2 * margin.Height) * ((log(sample.Value)) / maxY) - margin.Height
                            Dim label As String = $"{snp.Gene} ({sample.Key})"
                            Dim color As SolidBrush

                            Select Case colorPattern
                                Case "chr"
                                    color = ChromosomeColors(snp.Chr.ToUpper)
                                Case "samplename"
                                    color = sampleBrush(sample.Key)
                                Case Else
                                    If odds Then
                                        color = __odds
                                    Else
                                        color = __evens
                                    End If
                            End Select

                            Call g.FillPie(color, New Rectangle(x - r, y - r, ptSize, ptSize), 0, 360)

                            If showDebugLabel Then
                                Call g.DrawString(label,
                                                  labelFont,
                                                  Brushes.Black,
                                                  New Point(x + ptSize + 3, y + 10))
                            End If
                        Next
                    Next

                    xLeft += max
                    odds = Not odds
                Next
            End Sub)
    End Function
End Module