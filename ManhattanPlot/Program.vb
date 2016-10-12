Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Data.csv
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Scripting.MetaData

<PackageNamespace("Manhattan.Plot",
                  Description:="
### ManhattanPlot
Manhattan plot in VisualBasic

##### [Manhattan plot](https://en.wikipedia.org/wiki/Manhattan_plot)

A Manhattan plot is a type of scatter plot, usually used to display data with a large number of data-points - many of non-zero amplitude, and with a distribution of higher-magnitude values, for instance in genome-wide association studies (GWAS).[1] In GWAS Manhattan plots, genomic coordinates are displayed along the X-axis, with the negative logarithm of the association P-value for each single nucleotide polymorphism (SNP) displayed on the Y-axis, meaning that each dot on the Manhattan plot signifies a SNP. Because the strongest associations have the smallest P-values (e.g., 10−15), their negative logarithms will be the greatest (e.g., 15).

It gains its name from the similarity of such a plot to the Manhattan skyline: a profile of skyscrapers towering above the lower level ""buildings"" which vary around a lower height.

##### References
+ Gibson, Greg (2010). ""Hints Of hidden heritability In GWAS"". Nature Genetics. 42 (7): 558–560. doi:10.1038/ng0710-558. PMID 20581876.")>
Module Program

    Sub New()
        Dim template As String = App.HOME & "/Template.csv"

        If Not template.FileExists Then
            Call {New SNP}.SaveTo(template)
        End If

        template = App.HOME & "/SampleColors.csv"
        If Not template.FileExists Then
            Call {New SampleColor}.SaveTo(template)
        End If
    End Sub

    Public Function Main() As Integer

        'Call "H:\ManhattanPlot\manhattan_plot_test.csv".LoadCsv(Of SNP).Plot(colorPattern:="chr").SaveAs("H:\ManhattanPlot\manhattan_plot_test.png", ImageFormats.Png)
        'Call "H:\ManhattanPlot\manhattan_plot_test.csv".LoadCsv(Of SNP).Plot(colorPattern:="sampleName").SaveAs("H:\ManhattanPlot\manhattan_plot_test_sampleName.png", ImageFormats.Png)
        'Call "H:\ManhattanPlot\manhattan_plot_test.csv".LoadCsv(Of SNP).Plot(colorPattern:="interval").SaveAs("H:\ManhattanPlot\manhattan_plot_test_interval.png", ImageFormats.Png)

        'End

        'Dim label As Boolean = False
        'Dim eq As Boolean = False
        'Dim data = "C:\Users\xieguigang\Desktop\8.23\Manhattan_Plots.csv".LoadCsv(Of SNP)
        'Dim method As String = "asdadg"
        'Dim colors As New Dictionary(Of String, String) From {
        '    {"GMAF", "Red"},
        '    {"AFR vs. AMR", "DeepSkyBlue"},
        '    {"EAS vs. AFR", "Cyan"},
        '    {"EAS vs. AMR", "Chocolate"}
        '}
        'Dim img = data.Plot(equidistant:=eq, colors:=colors, ptSize:=25, showDebugLabel:=label, ylog:=method)
        'Call img.SaveAs("C:\Users\xieguigang\Desktop\8.23\Manhattan_Plots.png", ImageFormats.Png)

        'Call "C:\Users\xieguigang\Desktop\8.23\sp\AFR vs. AMR.csv".LoadCsv(Of SNP).Plot(equidistant:=eq, colors:=colors, ptSize:=25, showDebugLabel:=label, ylog:=method).SaveAs("C:\Users\xieguigang\Desktop\8.23\sp\AFR vs. AMR.png", ImageFormats.Png)
        'Call "C:\Users\xieguigang\Desktop\8.23\sp\EAS vs. AFR.csv".LoadCsv(Of SNP).Plot(equidistant:=eq, colors:=colors, ptSize:=25, showDebugLabel:=label, ylog:=method).SaveAs("C:\Users\xieguigang\Desktop\8.23\sp\EAS vs. AFR.png", ImageFormats.Png)
        'Call "C:\Users\xieguigang\Desktop\8.23\sp\EAS vs. AMR.csv".LoadCsv(Of SNP).Plot(equidistant:=eq, colors:=colors, ptSize:=25, showDebugLabel:=label, ylog:=method).SaveAs("C:\Users\xieguigang\Desktop\8.23\sp\EAS vs. AMR.png", ImageFormats.Png)
        'Call "C:\Users\xieguigang\Desktop\8.23\sp\GMAF.csv".LoadCsv(Of SNP).Plot(equidistant:=eq, colors:=colors, ptSize:=25, showDebugLabel:=label, ylog:=method).SaveAs("C:\Users\xieguigang\Desktop\8.23\sp\GMAF.png", ImageFormats.Png)

        'Pause()
        Return GetType(Program).RunCLI(App.CommandLine)
    End Function

    <ExportAPI("/Draw", Usage:="/Draw /in <data.csv> [/out <out.png> /sampleColors <sample_colors.csv> /width 3000 /height 1440 /pt.size 10 /debug.label /equidistant /relative /ylog <ln/log/raw, default:=ln> /colorPattern <chr/sampleName/interval, default:=chr>]",
               Info:="Invoke the Manhattan plots for the SNP sites.",
               Example:="/Draw /in ./manhattan_plot_test.csv /out ./manhattan_plot_test.png")>
    <ParameterInfo("/in", False, AcceptTypes:={GetType(SNP())})>
    <ParameterInfo("/sampleColors", True, AcceptTypes:={GetType(SampleColor())},
                   Description:="Color expression supports both .NET known color name and rgb expression.

+ .NET known color names: https://github.com/xieguigang/VisualBasic_AppFramework/blob/master/VB.NET_Colors.html
+ rgb expressions: ``rgb(r,g,b)`` or ``rgb(a,r,g,b)``, parameters ``a,r,g,b`` each value should less than 256, that is value ranges from 0 to 255")>
    <ParameterInfo("/ylog", True, AcceptTypes:={GetType(String)},
                   Description:="
+ ``ln``, for ``-ln(p-value)``, log value on base ``e``
+ ``log``, for ``-log(p-value, 10)``, log value on base 10
+ ``raw``, for raw value, no transformation.")>
    Public Function Draw(args As CommandLine) As Integer
        Dim [in] As String = args("/in")
        Dim sampleColors As String = args("/sampleColors")
        Dim w As Integer = args.GetValue("/width", 3000)
        Dim h As Integer = args.GetValue("/height", 1440)
        Dim ptSize As Integer = args.GetValue("/pt.size", 10)
        Dim debug As Boolean = args.GetBoolean("/debug.label")
        Dim eqdist As Boolean = args.GetBoolean("/equidistant")
        Dim rel As Boolean = args.GetBoolean("/relative")
        Dim ylog As String = args.GetValue("/ylog", "ln")
        Dim cptn As String = args.GetValue("/colorPattern", "chr")
        Dim out As String = args.GetValue("/out", [in].TrimSuffix & "_" & cptn & ".png")
        Dim data As IEnumerable(Of SNP) = [in].LoadCsv(Of SNP)
        Dim samples As Dictionary(Of String, String) = Nothing
        If sampleColors.FileExists Then
            Dim colors = sampleColors.LoadCsv(Of SampleColor)
            samples = colors.ToDictionary(
                Function(x) x.SampleName,
                Function(x) x.Color)
        End If
        Dim image As Bitmap = data.Plot(w, h, samples,, ptSize, debug, eqdist, rel, ylog, cptn)
        Return image.SaveAs(out, ImageFormats.Png).CLICode
    End Function
End Module
