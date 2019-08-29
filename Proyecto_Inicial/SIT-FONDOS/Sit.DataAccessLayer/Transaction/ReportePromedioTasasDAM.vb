Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class ReportePromedioTasasDAM
    Public Sub New()

    End Sub

    Public Function PromedioTasas(ByVal FechaInicial As Decimal, ByVal FechaFinal As Decimal) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("REPORTE_PROMEDIO_TASAS")

        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, FechaInicial)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, FechaFinal)

        Return db.ExecuteDataSet(dbCommand)

    End Function
End Class
