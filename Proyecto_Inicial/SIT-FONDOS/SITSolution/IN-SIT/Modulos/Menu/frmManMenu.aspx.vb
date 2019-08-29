Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data

Imports System.Web.Script.Serialization
Imports System.Web.Services
Imports System.Collections.Generic
Imports System.Web.Configuration


Partial Class Modulos_Menu_frmManMenu
    Inherits BasePage

#Region "/* Metodos Personalizados */"

    <WebMethod()>
    Public Shared Function ListarMenu() As Object
        Dim blMenu As New MnMenuAccesoBM
        Dim listaMenu As New List(Of MnMenuAccesoBE)
        Dim entMenu As MnMenuAccesoBE

        Dim dt As DataTable = blMenu.Listar("1D31D67E-7B7B-43D9-918C-3BABB57695A3")

        'Ahora realizamos un mapeo con las entidades que reconocerá el menú dinámico
        For Each row As DataRow In dt.Rows
            entMenu = New MnMenuAccesoBE

            entMenu.id = row("codOpcionMenu")
            entMenu.pId = row("codOpcionMenuPadre")
            entMenu.name = row("tituloOpcionMenu")
            entMenu.file = row("url")
            entMenu.open = row("open")

            entMenu._CodAplicativo = row("CodAplicativo")
            entMenu._Nivel = row("Nivel")
            entMenu._Orden = row("Orden")

            listaMenu.Add(entMenu)
        Next

        Return listaMenu
    End Function

    <WebMethod()>
    Public Shared Function InsertarOpcionMenu(ByVal CodOpcionMenu As String _
                                       , ByVal CodAplicativo As String _
                                       , ByVal TituloOpcionMenu As String _
                                       , ByVal Nivel As String _
                                       , ByVal Orden As String _
                                       , ByVal Url As String _
                                       , ByVal CodOpcionMenuPadre As String) As String
        Dim blMenu As New MnMenuAccesoBM
        Try
            Dim resultCodOpcionMenu As String = blMenu.InsertarOpcionMenu(CodOpcionMenu, CodAplicativo, TituloOpcionMenu, Nivel, Orden, Url, CodOpcionMenuPadre, "A", "ADMIN")

            ' Por default le damos permiso sobre esta nueva opcion al ADMINISTRADOR
            blMenu.InsertarRolOpcionMenu("1", resultCodOpcionMenu, "ADMIN")
        Catch ex As Exception
            Return ex.Message
        End Try

        Return ""
    End Function

#End Region

End Class
