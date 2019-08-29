Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class PrevisionUsuarioBM

    Public Function ListarUsuarios(ByVal nombre As String, ByVal situacion As String) As DataSet
        Dim objDAM As PrevisionUsuarioDAM = New PrevisionUsuarioDAM()
        Return objDAM.ListarUsuario(nombre, situacion)
    End Function

    Public Function EliminarUsuario(ByVal codigoUsuario As String) As Integer
        Dim objDAM As PrevisionUsuarioDAM = New PrevisionUsuarioDAM()
        Return objDAM.EliminarUsuario(codigoUsuario)
    End Function

    Function ListarOperacionesUsuario(ByVal codigoUsuario As String) As DataSet
        Dim objDAM As PrevisionUsuarioDAM = New PrevisionUsuarioDAM()
        Return objDAM.ListarOperacionesUsuario(codigoUsuario)
    End Function

    Function RegistrarUsuario(ByVal objUsuarioBE As PrevisionUsuario) As Integer
        Dim objDAM As PrevisionUsuarioDAM = New PrevisionUsuarioDAM()
        Return objDAM.RegistrarUsuario(objUsuarioBE)
    End Function

    Function ActualizarUsuario(ByVal objUsuarioBE As PrevisionUsuario) As Integer
        Dim objDAM As PrevisionUsuarioDAM = New PrevisionUsuarioDAM()
        Return objDAM.ActualizarUsuario(objUsuarioBE)
    End Function

    Sub LimpiarUsuarioDetalle(ByVal objUsuarioBE As PrevisionUsuario)
        Dim objDAM As PrevisionUsuarioDAM = New PrevisionUsuarioDAM()
        objDAM.LimpiarUsuarioDetalle(objUsuarioBE)
    End Sub

    Sub RegistrarUsuarioDetalle(ByVal objDetalleBE As PrevisionDetalleUsuario)
        Dim objDAM As PrevisionUsuarioDAM = New PrevisionUsuarioDAM()
        objDAM.RegistrarUsuarioDetalle(objDetalleBE)
    End Sub

End Class
