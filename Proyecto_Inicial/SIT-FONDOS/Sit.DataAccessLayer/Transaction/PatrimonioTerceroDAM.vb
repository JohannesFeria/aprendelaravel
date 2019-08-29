Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports Sit.BusinessEntities

Public Class PatrimonioTerceroDAM
    Private oPatrimonioTercero As PatrimonioTerceroBE.PatrimonioTerceroRow

    Public Sub New()
    End Sub

    Public Sub Borrar_PatrimonioTercero(ByVal FechaProceso As Decimal, ByVal tipoArchivo As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_delPatrimonioTercero")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, FechaProceso)
        db.AddInParameter(dbCommand, "@p_TipoInformacion", DbType.String, tipoArchivo)
        db.ExecuteNonQuery(dbCommand)
    End Sub

    Public Function Insertar(ByVal orowPatrimonioTercero As PatrimonioTerceroBE.PatrimonioTerceroRow, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_PatrimonioTercero_Insertar")
        dbCommand.CommandTimeout = 1020
        oPatrimonioTercero = CType(orowPatrimonioTercero, PatrimonioTerceroBE.PatrimonioTerceroRow)
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, If(oPatrimonioTercero.IsNull("CodigoTercero"), DBNull.Value, oPatrimonioTercero.CodigoTercero))
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, If(oPatrimonioTercero.IsNull("CodigoMnemonico"), DBNull.Value, oPatrimonioTercero.CodigoMnemonico))
        db.AddInParameter(dbCommand, "@p_PatrimonioMN", DbType.Decimal, oPatrimonioTercero.PatrimonioMN)
        db.AddInParameter(dbCommand, "@p_PasivoMN", DbType.Decimal, oPatrimonioTercero.PasivoMN)
        db.AddInParameter(dbCommand, "@p_ActivoMN", DbType.Decimal, oPatrimonioTercero.ActivoMN)
        db.AddInParameter(dbCommand, "@p_PatrimonioME", DbType.Decimal, oPatrimonioTercero.PatrimonioME)
        db.AddInParameter(dbCommand, "@PasivoME", DbType.Decimal, oPatrimonioTercero.PasivoME)
        db.AddInParameter(dbCommand, "@p_ActivoME", DbType.Decimal, oPatrimonioTercero.ActivoME)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, orowPatrimonioTercero.Fecha)
        db.AddInParameter(dbCommand, "@p_TipoInformacion", DbType.String, orowPatrimonioTercero.TipoInformacion)
        db.AddInParameter(dbCommand, "@p_UsuarioAuditoria", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaAuditoria", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)
        Return ""
    End Function

    Function SeleccionarPorFecha(ByVal fechaProceso As Decimal, ByVal tipoArchivo As String) As PatrimonioTerceroBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_PatrimonioTercero_Seleccionar")

        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, fechaProceso)
        db.AddInParameter(dbCommand, "@p_TipoInformacion", DbType.String, tipoArchivo)
        Dim oPatrimonioTerceroBE As New PatrimonioTerceroBE
        db.LoadDataSet(dbCommand, oPatrimonioTerceroBE, "PatrimonioTercero")
        Return oPatrimonioTerceroBE
    End Function
End Class
