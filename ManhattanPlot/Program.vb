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
        Dim data = "C:\Users\xieguigang\Desktop\8.23\Manhattan_Plots.csv".LoadCsv(Of SNP)
        Dim colors As New Dictionary(Of String, String) From {
            {"GMAF", "Crimson"},
            {"AFR vs. AMR", "CadetBlue"},
            {"EAS vs. AFR", "DarkSlateBlue"},
            {"EAS vs. AMR", "Gold"}
        }
        Dim img = data.Plot(equidistant:=True, colors:=colors, ptSize:=25)
        Call img.SaveAs("C:\Users\xieguigang\Desktop\8.23\Manhattan_Plots.png", ImageFormats.Png)

        Call "C:\Users\xieguigang\Desktop\8.23\sp\AFR vs. AMR.csv".LoadCsv(Of SNP).Plot(equidistant:=True, colors:=colors, ptSize:=25, showDebugLabel:=True).SaveAs("C:\Users\xieguigang\Desktop\8.23\sp\AFR vs. AMR.png", ImageFormats.Png)
        Call "C:\Users\xieguigang\Desktop\8.23\sp\EAS vs. AFR.csv".LoadCsv(Of SNP).Plot(equidistant:=True, colors:=colors, ptSize:=25, showDebugLabel:=True).SaveAs("C:\Users\xieguigang\Desktop\8.23\sp\EAS vs. AFR.png", ImageFormats.Png)
        Call "C:\Users\xieguigang\Desktop\8.23\sp\EAS vs. AMR.csv".LoadCsv(Of SNP).Plot(equidistant:=True, colors:=colors, ptSize:=25, showDebugLabel:=True).SaveAs("C:\Users\xieguigang\Desktop\8.23\sp\EAS vs. AMR.png", ImageFormats.Png)
        Call "C:\Users\xieguigang\Desktop\8.23\sp\GMAF.csv".LoadCsv(Of SNP).Plot(equidistant:=True, colors:=colors, ptSize:=25, showDebugLabel:=True).SaveAs("C:\Users\xieguigang\Desktop\8.23\sp\GMAF.png", ImageFormats.Png)

        Pause()
        Return GetType(Program).RunCLI(App.CommandLine)
    End Function
End Module
