Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Public Class PrevisionCierreBM
    Public Shared Function ObtenerPrevisionCierre() As DataSet
        Return PrevisionCierreDAM.ObtenerPrevisionCierre()
    End Function
    Public Shared Function ActualizarPrevisionCierre(ByVal HoraCierre1 As String, ByVal TipoCierre1 As String, ByVal HoraCierre2 As String, ByVal TipoCierre2 As String, ByVal UsuarioModificacion As String) As Boolean
        Return PrevisionCierreDAM.ActualizarPrevisionCierre(HoraCierre1, TipoCierre1, HoraCierre2, TipoCierre2, UsuarioModificacion)
    End Function
End Class
