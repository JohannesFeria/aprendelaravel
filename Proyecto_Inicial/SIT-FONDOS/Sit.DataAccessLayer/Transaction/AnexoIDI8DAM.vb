Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class AnexoIDI8DAM
    Public Function SeleccionarPorPortafolioFecha(ByVal portafolioSBS As String, ByVal fecha As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("AnexoIDI8_SeleccionarPorPortafolioFecha")

        db.AddInParameter(dbCommand, "@p_Portafolio", DbType.String, portafolioSBS)
        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, fecha)

        Dim oAnexoIDI8 As New DataSet
        db.LoadDataSet(dbCommand, oAnexoIDI8, "oAnexoIDI8DAM")
        Return oAnexoIDI8

    End Function

End Class
