Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class VaxRegauxDAM

    Public Function SeleccionarPorCartera(ByVal portafolio As String, ByVal fecha As Decimal, ByVal dataRequest As DataSet) As DataSet

        Me.llenarTabla_VaxRegAux(portafolio, fecha)

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("VaxRegaux_SeleccionarPorCartera")

        db.AddInParameter(dbCommand, "@p_Cartera", DbType.String, portafolio)
        Dim oVaxRegaux As New DataSet
        db.LoadDataSet(dbCommand, oVaxRegaux, "VaxRegaux")
        Return oVaxRegaux

    End Function


    Public Sub llenarTabla_VaxRegAux(ByVal portafolio As String, ByVal fecha As Decimal)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("llenarTabla_VaxRegaux")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, fecha)
        db.ExecuteNonQuery(dbCommand)
    End Sub

End Class
