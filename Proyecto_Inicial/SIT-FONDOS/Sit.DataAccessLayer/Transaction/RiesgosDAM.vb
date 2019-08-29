'BPesantes 18-08-2016
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data.Common

Public Class RiesgosDAM

    Public Function ReporteValidacionRating(ByVal FechaOperacion As Decimal, ByVal FechaCadena As String) As DataTable
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
            Dim dbcomand As DbCommand = db.GetStoredProcCommand("sp_SIT_ReporteValidacionRating")
            db.AddInParameter(dbcomand, "@p_FechaProceso", DbType.Decimal, FechaOperacion)
            db.AddInParameter(dbcomand, "@p_FechaCadena", DbType.String, FechaCadena)
            Return db.ExecuteDataSet(dbcomand).Tables(0)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function ReporteValidacionFondos(ByVal FechaOperacion As Decimal, ByVal FechaCadena As String) As DataTable
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
            Dim dbcomand As DbCommand = db.GetStoredProcCommand("sp_SIT_ReporteValidacionFondos")
            db.AddInParameter(dbcomand, "@p_FechaProceso", DbType.Decimal, FechaOperacion)
            db.AddInParameter(dbcomand, "@p_FechaCadena", DbType.String, FechaCadena)
            Return db.ExecuteDataSet(dbcomand).Tables(0)
        Catch ex As Exception
            Throw
        End Try
    End Function

End Class
