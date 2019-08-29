Public Class DataUtility
    Public Shared Function ObtenerValorRequest(ByVal dataRequest As DataSet, ByVal nombre As String)
        Dim columnName As String = dataRequest.Tables(0).Columns(0).ColumnName
        Return CType(dataRequest.Tables(0).Select(columnName & "='" & nombre & "'")(0)(1), String)
    End Function

    Public Shared Function ObtenerHora(ByVal fecha As Date) As String
        Return fecha.ToString("HH:mm:ss")
    End Function
    Public Shared Function ObtenerFecha(ByVal fecha As Date) As Decimal
        Return CType(fecha.ToString("yyyyMMdd"), Decimal)
    End Function
End Class
