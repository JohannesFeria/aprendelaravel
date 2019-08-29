Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class PrevisionAprobacionBM
    Public Shared Function ListarPagosAprobar(ByVal Fecha As Decimal, ByVal Usuario As String, ByVal idTipoOperacion As String) As DataSet
        Return PrevisionAprobacionDAM.ListarPagosAprobar(Fecha, Usuario, idTipoOperacion)
    End Function

    Public Shared Function ActualizarAprobacion(ByVal CodigoPago As String, ByVal UsuarioAprobacion As String, ByVal Estado As String) As Boolean
        Return PrevisionAprobacionDAM.ActualizarAprobacion(CodigoPago, UsuarioAprobacion, Estado)
    End Function
End Class
