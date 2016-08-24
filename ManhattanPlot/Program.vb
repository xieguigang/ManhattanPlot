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
        Dim img = data.Plot(equidistant:=True, colors:=colors)
        Call img.SaveAs("C:\Users\xieguigang\Desktop\8.23\Manhattan_Plots.png", ImageFormats.Png)
        Pause()
        Return GetType(Program).RunCLI(App.CommandLine)
    End Function
End Module
