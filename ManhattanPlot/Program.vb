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
        Dim data = "H:\ManhattanPlot\manhattan_plot_test.csv".LoadCsv(Of SNP)
        Dim img = data.Plot(equidistant:=True)
        Call img.SaveAs("H:\ManhattanPlot\manhattan_plot_test.png", ImageFormats.Png)

        Return GetType(Program).RunCLI(App.CommandLine)
    End Function
End Module
