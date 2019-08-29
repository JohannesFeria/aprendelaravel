Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class ControlForwardDAM
    Public Sub New()

    End Sub

    Public Function ControlForward(ByVal FechaInicial As Decimal, ByVal Portafolios As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("REPORTE_CONTROL_FORWARD")

        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, FechaInicial)
        'db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, FechaFinal)
        db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, Portafolios)

        Return db.ExecuteDataSet(dbCommand)

    End Function

End Class
