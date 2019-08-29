Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class ReporteGipsaDAM
    Public Sub New()

    End Sub

    Public Function ReporteGipsa(ByVal Fecha As Decimal, ByVal Portafolios As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("REPORTE_GIPSA")

        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, Fecha)
        db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, Portafolios)

        Return db.ExecuteDataSet(dbCommand)

    End Function

    Public Function ReporteGIPSAEmisor(ByVal Fecha As Decimal, ByVal Portafolios As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("REPORTE_GIPSA_EMISOR")

        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, Fecha)
        db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, Portafolios)

        Return db.ExecuteDataSet(dbCommand)

    End Function

    Public Function TablaReporteGipsa(ByVal Fecha As Decimal, ByVal Portafolios As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("REPORTE_GIPSA_TABLA")

        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, Fecha)
        db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, Portafolios)

        Return db.ExecuteDataSet(dbCommand)

    End Function

    Public Function EncajeReporteGipsa() As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("REPORTE_GIPSA_ENCAJE")

        Return db.ExecuteDataSet(dbCommand)

    End Function


End Class
