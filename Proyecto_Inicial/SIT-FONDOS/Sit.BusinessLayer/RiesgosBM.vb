'BPesantes 18-08-2016
Imports Sit.DataAccessLayer

Public Class RiesgosBM

    Public Function ReporteValidacionRating(ByVal FechaOperacion As Decimal, ByVal FechaCadena As String) As DataTable
        Try
            Dim Riesgos As New RiesgosDAM
            Return Riesgos.ReporteValidacionRating(FechaOperacion, FechaCadena)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ReporteValidacionFondos(ByVal FechaOperacion As Decimal, ByVal FechaCadena As String) As DataTable
        Try
            Dim Riesgos As New RiesgosDAM
            Return Riesgos.ReporteValidacionFondos(FechaOperacion, FechaCadena)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
