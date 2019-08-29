Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class PrevisionPagoDAM
    Public Shared Function InsertarPrevisionPago(ByVal CodigoBancoOrigen As Integer, ByVal CodigoBancoDestino As Integer, ByVal oPago As PrevisionPago, ByVal oPagoDetalle1 As PrevisionPagoDetalle, ByVal oPagoDetalle2 As PrevisionPagoDetalle) As Boolean
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("prov.SP_PROV_INS_InsertarPrevisionPago")
            db.AddInParameter(dbCommand, "@FechaPago", DbType.Int32, oPago.FechaPago)
            db.AddInParameter(dbCommand, "@IdTipoOperacion", DbType.String, oPago.IdTipoOperacion)
            db.AddInParameter(dbCommand, "@Usu_Provision", DbType.String, oPago.UsuarioProvision)
            db.AddInParameter(dbCommand, "@TipoMovimiento1", DbType.String, oPagoDetalle1.TipoMovimiento)
            db.AddInParameter(dbCommand, "@TipoMoneda1", DbType.String, oPagoDetalle1.IdTipoMoneda)
            db.AddInParameter(dbCommand, "@Importe1", DbType.String, oPagoDetalle1.Importe)
            db.AddInParameter(dbCommand, "@IdEntidad1", DbType.String, oPagoDetalle1.IdEntidad)
            db.AddInParameter(dbCommand, "@CodigoBancoOrigen", DbType.Int32, CodigoBancoOrigen)
            db.AddInParameter(dbCommand, "@TipoMovimiento2", DbType.String, oPagoDetalle2.TipoMovimiento)
            db.AddInParameter(dbCommand, "@TipoMoneda2", DbType.String, oPagoDetalle2.IdTipoMoneda)
            db.AddInParameter(dbCommand, "@Importe2", DbType.String, oPagoDetalle2.Importe)
            db.AddInParameter(dbCommand, "@IdEntidad2", DbType.String, oPagoDetalle2.IdEntidad)
            db.AddInParameter(dbCommand, "@CodigoBancoDestino", DbType.Int32, CodigoBancoDestino)
            db.ExecuteNonQuery(dbCommand)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function EliminarPrevisionPago(ByVal CodigoPago As String, ByVal IdUsuario As String) As Boolean
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("prov.SP_PROV_UPD_ActualizarEstadoPago")
            db.AddInParameter(dbCommand, "@CodigoPago", DbType.String, CodigoPago)
            db.AddInParameter(dbCommand, "@IdUsuarioAnulacion", DbType.String, IdUsuario)
            db.ExecuteNonQuery(dbCommand)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function ListarPrevisionPago(ByVal FechaPago As String, ByVal IdTipoOperacion As String, ByVal IdEstado As String, ByVal IdUsuario As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("prov.SP_PROV_SEL_ListaPrevisionPago")
        db.AddInParameter(dbCommand, "@FechaPago", DbType.String, FechaPago)
        db.AddInParameter(dbCommand, "@IdTipoOperacion", DbType.String, IdTipoOperacion)
        db.AddInParameter(dbCommand, "@IdEstado", DbType.String, IdEstado)
        db.AddInParameter(dbCommand, "@UsuarioProvision", DbType.String, IdUsuario)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Shared Function ObtenerPrevisionPago(ByVal CodigoPago As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("prov.SP_PROV_SEL_ObtenerPrevisionPago")
        db.AddInParameter(dbCommand, "@CodigoPago", DbType.String, CodigoPago)
        Return db.ExecuteDataSet(dbCommand)
    End Function

End Class
