Imports System
Imports System.Data
Imports sit.DataAccessLayer

Public Class ConsultasBM
    Public Function EjecutarDataSet(ByVal query As String) As DataTable
        Try
            Return New ConsultasDAM().EjecutarDataSet(query)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function EjecutarNonQuery(ByVal query As String) As Integer
        Try
            Return New ConsultasDAM().EjecutarNonQuery(query)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
