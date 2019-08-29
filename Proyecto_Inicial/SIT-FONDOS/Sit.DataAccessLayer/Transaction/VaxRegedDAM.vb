Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class VaxRegedDAM

    Public Function SeleccionarPorCartera(ByVal portafolio As String, ByVal fecha As Decimal, ByVal dataRequest As DataSet) As DataSet
        Me.llenarTabla_VaxRegdet(portafolio, fecha)

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("VaxRegdet_SeleccionarPorCartera")

        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, portafolio)
        Dim oVaxReged As New DataSet
        db.LoadDataSet(dbCommand, oVaxReged, "VaxReged")

        Return oVaxReged
    End Function

    Public Sub llenarTabla_VaxRegdet(ByVal portafolio As String, ByVal fecha As Decimal)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("llenarTabla_VaxRegdet")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, fecha)
        db.ExecuteNonQuery(dbCommand)
    End Sub

End Class
