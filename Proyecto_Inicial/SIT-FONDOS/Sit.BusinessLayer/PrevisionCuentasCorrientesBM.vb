Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer

Public Class PrevisionCuentasCorrientesBM

    Public Shared Function ObtenerBanco(ByVal IdEntidad As String) As DataSet
        Return PrevisionCuentasCorrientesDAM.ObtenerBanco(IdEntidad)
    End Function

    Public Shared Function ObtenerBanco_x_IdBanco(ByVal IdEntidad As String, ByVal IdMoneda As String) As DataSet
        Return PrevisionCuentasCorrientesDAM.ObtenerBanco_x_IdBanco(IdEntidad, IdMoneda)
    End Function

End Class
