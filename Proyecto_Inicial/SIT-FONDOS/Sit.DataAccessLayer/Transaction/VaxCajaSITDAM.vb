Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class VaxCajaSITDAM

    'RGF 20080625
    Public Function SeleccionarPorCartera(ByVal portafolio As String, ByVal fecha As Decimal, ByVal dataRequest As DataSet) As DataSet

        'Me.llenarTabla_VaxTicus(portafolio, fecha)

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("VaxCajaSIT_Seleccionar")


        db.AddInParameter(dbCommand, "@p_Portafolio", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_FechaVax", DbType.Decimal, fecha)
        Dim oVaxCajaSIT As New DataSet
        db.LoadDataSet(dbCommand, oVaxCajaSIT, "VaxCajaSIT")
        Return oVaxCajaSIT
    End Function

End Class
