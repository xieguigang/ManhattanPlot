Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.DocumentFormat.Csv
Imports Microsoft.VisualBasic.Imaging

Module Program

    Sub New()
        Dim template As String = App.HOME & "/Template.csv"

        If Not template.FileExists Then
            Call {New SNP}.SaveTo(template)
        End If
    End Sub

    Public Function Main() As Integer

        Call "H:\ManhattanPlot\manhattan_plot_test.csv".LoadCsv(Of SNP).Plot.SaveAs("H:\ManhattanPlot\manhattan_plot_test.png", ImageFormats.Png)

        End

        Dim label As Boolean = False
        Dim eq As Boolean = False
        Dim data = "C:\Users\xieguigang\Desktop\8.23\Manhattan_Plots.csv".LoadCsv(Of SNP)
        Dim method As String = "asdadg"
        Dim colors As New Dictionary(Of String, String) From {
            {"GMAF", "Red"},
            {"AFR vs. AMR", "DeepSkyBlue"},
            {"EAS vs. AFR", "Cyan"},
            {"EAS vs. AMR", "Chocolate"}
        }
        Dim img = data.Plot(equidistant:=eq, colors:=colors, ptSize:=25, showDebugLabel:=label, ylog:=method)
        Call img.SaveAs("C:\Users\xieguigang\Desktop\8.23\Manhattan_Plots.png", ImageFormats.Png)

        Call "C:\Users\xieguigang\Desktop\8.23\sp\AFR vs. AMR.csv".LoadCsv(Of SNP).Plot(equidistant:=eq, colors:=colors, ptSize:=25, showDebugLabel:=label, ylog:=method).SaveAs("C:\Users\xieguigang\Desktop\8.23\sp\AFR vs. AMR.png", ImageFormats.Png)
        Call "C:\Users\xieguigang\Desktop\8.23\sp\EAS vs. AFR.csv".LoadCsv(Of SNP).Plot(equidistant:=eq, colors:=colors, ptSize:=25, showDebugLabel:=label, ylog:=method).SaveAs("C:\Users\xieguigang\Desktop\8.23\sp\EAS vs. AFR.png", ImageFormats.Png)
        Call "C:\Users\xieguigang\Desktop\8.23\sp\EAS vs. AMR.csv".LoadCsv(Of SNP).Plot(equidistant:=eq, colors:=colors, ptSize:=25, showDebugLabel:=label, ylog:=method).SaveAs("C:\Users\xieguigang\Desktop\8.23\sp\EAS vs. AMR.png", ImageFormats.Png)
        Call "C:\Users\xieguigang\Desktop\8.23\sp\GMAF.csv".LoadCsv(Of SNP).Plot(equidistant:=eq, colors:=colors, ptSize:=25, showDebugLabel:=label, ylog:=method).SaveAs("C:\Users\xieguigang\Desktop\8.23\sp\GMAF.png", ImageFormats.Png)



        Pause()
        Return GetType(Program).RunCLI(App.CommandLine)
    End Function

    <ExportAPI("/Draw", Usage:="/Draw /in <data.csv> [/out <out.png>]")>
    Public Function Draw(args As CommandLine) As Integer

    End Function
End Module
