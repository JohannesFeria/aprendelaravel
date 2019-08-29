Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer

Public Class PrevisionPagoBM
    Public Shared Function InsertarPRevisionPago(ByVal CodigoBancoOrigen As Integer, ByVal CodigoBancoDestino As Integer, ByVal oPago As PrevisionPago, ByVal oPagoDetalle1 As PrevisionPagoDetalle, ByVal oPagoDetalle2 As PrevisionPagoDetalle) As Boolean
        Return PrevisionPagoDAM.InsertarPrevisionPago(CodigoBancoOrigen, CodigoBancoDestino, oPago, oPagoDetalle1, oPagoDetalle2)
    End Function

    Public Shared Function EliminarPrevisionPago(ByVal CodigoPago As String, ByVal IdUsuario As String) As Boolean
        Return PrevisionPagoDAM.EliminarPrevisionPago(CodigoPago, IdUsuario)
    End Function

    Public Shared Function ListarPrevisionPago(ByVal FechaPago As String, ByVal IdTipoOperacion As String, ByVal IdEstado As String, ByVal IdUsuario As String) As DataSet
        Return PrevisionPagoDAM.ListarPrevisionPago(FechaPago, IdTipoOperacion, IdEstado, IdUsuario)
    End Function

    Public Shared Function ObtenerPrevisionPago(ByVal CodigoPago As String) As DataSet
        Return PrevisionPagoDAM.ObtenerPrevisionPago(CodigoPago)
    End Function
End Class
