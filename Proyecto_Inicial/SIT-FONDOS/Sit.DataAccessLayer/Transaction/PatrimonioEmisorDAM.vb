Imports System.Data
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data.Common

Public Class PatrimonioEmisorDAM

    Public Sub Insertar(ByVal objPE_BE As PatrimonioEmisorBE, ByVal datosRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("PatrimonioEmisor_Insertar")
            db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, objPE_BE.codigoEntidad)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, objPE_BE.codigoTercero)
            db.AddInParameter(dbCommand, "@p_TipoValor", DbType.String, objPE_BE.tipoValor)
            db.AddInParameter(dbCommand, "@p_Valor", DbType.Decimal, objPE_BE.valor)
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Int32, objPE_BE.fecha)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(datosRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(datosRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(datosRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(datosRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub

    Public Sub Actualizar(ByVal objPE_BE As PatrimonioEmisorBE, ByVal datosRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("PatrimonioEmisor_Actualizar")
            db.AddInParameter(dbCommand, "@p_Id", DbType.Int32, objPE_BE.id)
            db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, objPE_BE.codigoEntidad)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, objPE_BE.codigoTercero)
            db.AddInParameter(dbCommand, "@p_TipoValor", DbType.String, objPE_BE.tipoValor)
            db.AddInParameter(dbCommand, "@p_Valor", DbType.Decimal, objPE_BE.valor)
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Int32, objPE_BE.fecha)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(datosRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(datosRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(datosRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(datosRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub

    Public Sub Eliminar(ByVal objPE_BE As PatrimonioEmisorBE)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Eliminar_PatrimonioEmisor")
            db.AddInParameter(dbCommand, "@p_Id", DbType.Int32, objPE_BE.id)
            db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, objPE_BE.codigoEntidad)
            db.AddInParameter(dbCommand, "@p_TipoValor", DbType.String, objPE_BE.tipoValor)
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Int32, objPE_BE.fecha)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub

End Class
