Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class PrevisionParametriaBM

    Public Shared Function ListarParametria(ByVal Parametro As Int32) As DataSet
        Return PrevisionParametriaDAM.ListarParametria(Parametro)
    End Function

    Public Shared Function InsertarCuentaCorriente(ByVal oCta As PrevisionCuentasCorrientes) As Int32
        Return PrevisionParametriaDAM.InsertarCuentaCorriente(oCta)
    End Function

    Public Shared Function ActualizarCuentaCorriente(ByVal oCta As PrevisionCuentasCorrientes) As Int32
        Return PrevisionParametriaDAM.ActualizarCuentaCorriente(oCta)
    End Function

    Public Shared Function EliminarCuentaCorriente(ByVal Codigo As Integer, ByVal IdUsuario As String) As Boolean
        Return PrevisionParametriaDAM.EliminarCuentaCorriente(Codigo, IdUsuario)
    End Function

    Public Shared Function ListarDetalleCuentasCorrientes(ByVal Banco As String, ByVal Estado As String) As DataSet
        Return PrevisionParametriaDAM.ListarDetalleCuentasCorrientes(Banco, Estado)
    End Function

End Class
