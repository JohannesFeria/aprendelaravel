Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class OrdenInversionTMPDAM
    Public Sub New()

    End Sub
    Private oOrdenInverversionTMPRow As OrdenInversionTMPBE.OrdenInversionBETMPRow
    'LETV 20091007
    'Se insertar oi desde un excel a una tabla temporal para poder agruparlos posteriormente y hacer validaciones de saldos
    Public Function InsertarOrdenInversionTemporal(ByVal oiTMP As OrdenInversionTMPBE) As Boolean
        oOrdenInverversionTMPRow = CType(oiTMP.OrdenInversionBETMP.Rows(0), OrdenInversionTMPBE.OrdenInversionBETMPRow)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        'Dim dbCommand As dbCommand = db.GetStoredProcCommand("OrdenInversionTMP_InsertarTemporalOIExcel")
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_ins_OrdenInversionTMP_InsertarTemporalOIExcel")
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, oOrdenInverversionTMPRow.CodigoOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, oOrdenInverversionTMPRow.CodigoTercero)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oOrdenInverversionTMPRow.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, oOrdenInverversionTMPRow.CodigoNemonico)
        db.AddInParameter(dbCommand, "@p_CantidadOrdenado", DbType.Decimal, oOrdenInverversionTMPRow.CantidadOrdenado)
        db.AddInParameter(dbCommand, "@p_Guid", DbType.String, oOrdenInverversionTMPRow.Guid)
        db.AddInParameter(dbCommand, "@p_FechaSaldo", DbType.Decimal, oOrdenInverversionTMPRow.FechaSaldo)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function Seleccionar_OrdenInversionTMPValidarSaldo(ByVal codigoOperacion As String, ByVal guid As String) As DataTable

        Dim ds As New DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        'Dim dbCommand As dbCommand = db.GetStoredProcCommand("OrdenInversionTMP_ValidarSaldo")
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_OrdenInversionTMP_ValidarSaldo")
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, codigoOperacion)
        db.AddInParameter(dbCommand, "@p_Guid", DbType.String, guid)
        db.LoadDataSet(dbCommand, ds, "OI")
        Return ds.Tables(0)
    End Function
End Class

