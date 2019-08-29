Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class ReporteCalculoInteresDAM
    Public Sub New()

    End Sub

    Public Function CalculoInteres(ByVal FechaInicial As Decimal, ByVal FechaFinal As Decimal) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Reporte_Calculo_Interes")

        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, FechaInicial)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, FechaFinal)

        Return db.ExecuteDataSet(dbCommand)

    End Function

    Public Function CertificadoDeposito(ByVal FechaInicial As Decimal, ByVal FechaFinal As Decimal) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Reporte_Certificado_Depositos")

        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, FechaInicial)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, FechaFinal)

        Return db.ExecuteDataSet(dbCommand)

    End Function

    Public Function FondosMutuos(ByVal FechaInicial As Decimal, ByVal FechaFinal As Decimal) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Reporte_Fondos_Mutuos")

        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, FechaInicial)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, FechaFinal)

        Return db.ExecuteDataSet(dbCommand)

    End Function

    Public Function ValorCuota(ByVal Fecha As Decimal) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Obtener_ValorCuota")

        db.AddInParameter(dbCommand, "@p_fecha", DbType.Decimal, Fecha)

        Return db.ExecuteDataSet(dbCommand)

    End Function
End Class
