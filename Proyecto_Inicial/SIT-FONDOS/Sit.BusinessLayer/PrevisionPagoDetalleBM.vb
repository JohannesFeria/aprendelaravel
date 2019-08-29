Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer

Public Class PrevisionPagoDetalleBM
    Public Shared Function ListarPrevisionPagoDetalle(ByVal FechaPago As String, ByVal IdTipoOperacion As String, ByVal IdEstado As String) As DataSet
        Return PrevisionPagoDetalleDAM.ListarPrevisionPagoDetalle(FechaPago, IdTipoOperacion, IdEstado)
    End Function

    Public Shared Function ListarPrevisionPagoDetalleToExport(ByVal FechaPago As String, ByVal IdTipoOperacion As String, ByVal IdEstado As String) As DataSet
        Return PrevisionPagoDetalleDAM.ListarPrevisionPagoDetalleToExport(FechaPago, IdTipoOperacion, IdEstado)
    End Function

    Public Shared Function ReportePrevisionPagoPorDetalle(ByVal sFechaInicio As String, ByVal sFechaFin As String) As DataSet
        Return PrevisionPagoDetalleDAM.ReportePrevisionPagoPorDetalle(sFechaInicio, sFechaFin)
    End Function

    Public Shared Function PagoDetallePorCuentaCte(ByVal Fecha As String, ByVal CuentaCte As String) As DataSet
        Return PrevisionPagoDetalleDAM.PagoDetallePorCuentaCte(Fecha, CuentaCte)
    End Function

    Public Shared Function ListarCuentaCtePorIdFondo(ByVal IdFondo As String, ByVal Fecha As String) As DataSet
        Return PrevisionPagoDetalleDAM.ListarCuentaCtePorIdFondo(IdFondo, Fecha)
    End Function

    Public Shared Function ListarOperacionxUsuario(ByVal CodUsuario As String) As DataSet
        Return PrevisionPagoDetalleDAM.ListarOperacionxUsuario(CodUsuario)
    End Function

    Public Shared Function ConsultaEntidad_x_TipoMoneda(ByVal IdMoneda As String) As DataSet
        Return PrevisionPagoDetalleDAM.ConsultaEntidad_x_TipoMoneda(IdMoneda)
    End Function
End Class
