Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class VectorSwapDAM
    Private oVectorSwapBE As VectorSwapBE.VectorSwapRow

    Public Sub Insertar(ByVal objVectorSwap As VectorSwapBE.VectorSwapRow, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_ins_VectorSwap_Insertar")
        dbCommand.CommandTimeout = 1020  'HDG 20120103
        oVectorSwapBE = CType(objVectorSwap, VectorSwapBE.VectorSwapRow)

        db.AddInParameter(dbCommand, "@p_RefOp", DbType.String, oVectorSwapBE.RefOp)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, oVectorSwapBE.Fecha)
        db.AddInParameter(dbCommand, "@p_Nocional", DbType.Decimal, oVectorSwapBE.Nocional)
        db.AddInParameter(dbCommand, "@p_TipoTasaR", DbType.String, oVectorSwapBE.TipoTasaR)
        db.AddInParameter(dbCommand, "@p_TasaR", DbType.Decimal, oVectorSwapBE.TasaR)
        db.AddInParameter(dbCommand, "@p_TipoTasaP", DbType.String, oVectorSwapBE.TipoTasaP)
        db.AddInParameter(dbCommand, "@p_TasaP", DbType.Decimal, oVectorSwapBE.TasaP)
        db.AddInParameter(dbCommand, "@p_MtmUSD", DbType.Decimal, oVectorSwapBE.MtmUSD)
        db.AddInParameter(dbCommand, "@p_MtmPEN", DbType.Decimal, oVectorSwapBE.MtmPEN)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oVectorSwapBE.Fondo)   'HDG OT 62087 Nro8-R15 20110315
        db.AddInParameter(dbCommand, "@p_Emisor", DbType.String, oVectorSwapBE.Emisor)   'HDG OT 62087 Nro8-R15 20110315
        db.AddInParameter(dbCommand, "@p_FechaVencimiento", DbType.Decimal, oVectorSwapBE.FechaVencimiento) 'HDG OT 65018 20120418
        db.ExecuteNonQuery(dbCommand)

    End Sub
End Class
