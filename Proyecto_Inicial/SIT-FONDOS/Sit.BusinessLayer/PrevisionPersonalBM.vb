Imports Sit.DataAccessLayer

Public Class PrevisionPersonalBM
    Public Function ListarPersonal(ByVal Nombre As String, ByVal Apellido As String, ByVal CodigoInterno As String) As DataSet
        Dim ds As New DataSet()
        ds = PrevisionPersonalDAM.ListarPersonal(Nombre, Apellido, CodigoInterno)
        Return ds
    End Function

End Class
