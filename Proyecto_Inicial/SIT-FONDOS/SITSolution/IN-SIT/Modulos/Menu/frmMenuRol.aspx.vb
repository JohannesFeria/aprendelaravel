Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data

Imports System.Web.Script.Serialization
Imports System.Web.Services
Imports System.Collections.Generic
Imports System.Web.Configuration


Partial Class Modulos_Menu_frmMenuRol
    Inherits BasePage

    Dim menuBM As New MnMenuAccesoBM

#Region "/* Metodos Personalizados */"
    Dim codRol As Integer
    Dim codApl As String
    Dim dt As New DataTable

    Private Sub CargarPagina()
        If Request.QueryString("type") IsNot Nothing Then
            hCodRol.Value = Convert.ToInt32(Request.QueryString("codRol"))
            codApl = Request.QueryString("codApl").ToString
        End If
    End Sub

    'Private Sub CargarMenuAplicacion()
    '    Dim lmenuBE As New List(Of MnMenuAccesoBE)
    '    lmenuBE = menuBM.Listar("", codRol, codApl, DatosRequest)
    '    GenerarArbol(lmenuBE)
    'End Sub

    'Private Sub CargarMenuRol(codRol As Integer, codApl As String)
    '    Dim lmenuBE As New List(Of MnMenuAccesoBE)
    '    lmenuBE = menuBM.Listar("", codRol, codApl, DatosRequest)
    '    GenerarArbol(lmenuBE)
    'End Sub

    'Private Function AprendeOtraVezJC(item As MnMenuAccesoBE) As List(Of MnMenuAccesoBE)

    '    Return Nothing
    'End Function

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

#Region "/* Eventos de la Pagina */"

    Protected Sub Modulos_Menu_frmEdicionRol_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                CargarPagina()
                ListarRolMenu()
            End If
        Catch ex As Exception
            AlertaJS(ex.ToString())
        End Try
    End Sub

#End Region

    Protected Sub ListarRolMenu()
        Try
            Dim oRol_MenuBM As New Rol_MenuBM
            Dim eRol_Menu As New E_Rol_Menu
            Dim dt As DataTable

            eRol_Menu.CODIGO_ROL = hCodRol.Value
            dt = oRol_MenuBM.Listar(eRol_Menu)

            dgLista.DataSource = dt
            dgLista.DataBind()

            Session("datos") = dt
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub ibIngresar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibIngresar.Click

        Dim dr As DataRow
        Dim cod As String
        Dim nombre As String

        dt.Columns.Add(New DataColumn("Codigo", GetType(String)))
        dt.Columns.Add(New DataColumn("Nombre", GetType(String)))

        Dim s As String = hMenu.Value
        Dim words As String() = s.Split(New Char() {"-"})

        cod = words(0).ToString
        nombre = words(1).ToString


        If Session("datos") IsNot Nothing Then
            dt = Session("datos")
        End If

        dr = dt.NewRow()
        dr("Codigo") = cod
        dr("Nombre") = nombre
        dt.Rows.Add(dr)

        dgLista.DataSource = dt
        dgLista.DataBind()

        Session("datos") = dt

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Page.IsCallback = True Then
            Session("datos") = Nothing
            CargarPagina()
        End If

    End Sub

    Protected Sub ibGrabar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibGrabar.Click
        Try
            Dim oRol_MenuBM As New Rol_MenuBM
            Dim eRol_Menu As New E_Rol_Menu

            For i = 0 To dgLista.Rows.Count - 1
                eRol_Menu.CODIGO_ROL = hCodRol.Value
                eRol_Menu.CODIGO_MENU = Convert.ToInt32(dgLista.DataKeys(i)("CODIGO"))
                oRol_MenuBM.InsertarRolMenu(eRol_Menu)
            Next

            AlertaJS("Se Grabo correctamente")
        Catch ex As Exception
            AlertaJS(ex.Message.ToString)
        End Try
    End Sub

    Protected Sub ibCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibCancelar.Click
        Response.Redirect("frmRol.aspx")
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        dgLista.PageIndex = e.NewPageIndex
        ListarRolMenu()
    End Sub
End Class
