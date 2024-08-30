Public Module MR

    ''' <summary>
    ''' IVW计算函数
    ''' </summary>
    ''' <param name="b_i"></param>
    ''' <param name="v_i"></param>
    ''' <returns></returns>
    Function CalculateIVW(ByVal b_i() As Double, ByVal v_i() As Double) As Double
        Dim numerator As Double = 0.0
        Dim denominator As Double = 0.0

        ' 确保b_i和v_i数组长度相同
        If b_i.Length <> v_i.Length Then
            Throw New ArgumentException("The length of b_i and v_i arrays must be the same.")
        End If

        ' 计算分子和分母
        For i As Integer = 0 To b_i.Length - 1
            numerator += b_i(i) / v_i(i)
            denominator += 1 / v_i(i)
        Next

        If numerator = 0.0 Then
            Return 0
        End If

        ' 计算IVW估计值
        If denominator = 0.0 Then
            Throw New DivideByZeroException("The denominator in the IVW calculation is zero.")
        End If

        Return numerator / denominator
    End Function
End Module
