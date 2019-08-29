Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Collections.Generic

Public Class MnMenuAccesoBM
    Dim dam As New MnMenuAccesoDAM

    Public Function Listar(ByVal TokenSesion As String) As DataTable
        Return dam.Listar(TokenSesion)
    End Function

    Public Function InsertarOpcionMenu(ByVal CodOpcionMenu As String _
                                   , ByVal CodAplicativo As String _
                                   , ByVal TituloOpcionMenu As String _
                                   , ByVal Nivel As String _
                                   , ByVal Orden As String _
                                   , ByVal Url As String _
                                   , ByVal CodOpcionMenuPadre As String _
                                   , ByVal Estado As String _
                                   , ByVal Usuario As String) As String

        Return dam.InsertarOpcionMenu(CodOpcionMenu, CodAplicativo, TituloOpcionMenu, Nivel, Orden, Url, CodOpcionMenuPadre, Estado, Usuario)
    End Function

    Public Sub InsertarRolOpcionMenu(ByVal CodRol As String, ByVal CodOpcionMenu As String, ByVal Usuario As String)
        dam.InsertarRolOpcionMenu(CodRol, CodOpcionMenu, Usuario)
    End Sub

    Public Sub InsertarRolUsuario(ByVal CodRol As String, ByVal CodUsuario As String, ByVal Usuario As String)
        dam.InsertarRolUsuario(CodRol, CodUsuario, Usuario)
    End Sub




    Public Function Listar(ByVal codUser As String, ByVal codRol As Integer, ByVal codAplicativo As String, ByVal dataRequest As DataSet) As List(Of MnMenuAccesoBE)
        Return dam.Listar(codUser, codRol, codAplicativo, dataRequest)
    End Function

    Public Function ListarMenu(ByVal codUser As String, ByVal codRol As Integer, ByVal codAplicativo As String, ByVal dataRequest As DataSet) As DataTable
        Return dam.ListarMenu(codUser, codRol, codAplicativo, dataRequest)
    End Function

    Public Function ListarBandeja(ByVal eMenuAplicativo As Menu_AccesoBE) As DataTable
        Return dam.ListarBandeja(eMenuAplicativo)
    End Function

    Public Function ValidarUsuario(ByVal login As String, ByVal password As String) As DataTable
        Return dam.ValidarUsuario(login, password)
    End Function

#Region "Metodos Nuevos de Menu Dinámico"

    Public Function ListarMenuAplicativo(ByVal codAplicativo As String, ByVal codUser As String, ByVal codRol As Integer, ByVal dataRequest As DataSet) As List(Of MnMenuAccesoBE)
        Return dam.ListarMenuAplicativo(codAplicativo, codUser, codRol, dataRequest)
    End Function

#End Region

End Class
