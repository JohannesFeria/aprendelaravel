Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports System.Data
Imports System.Web.Configuration

Partial Class Modulos_Menu_frmConsultaMenu
    Inherits BasePage


#Region "/* Metodos Personalizados */"

    Private Sub CargarPagina()
        CargarCombos()
        Buscar()
    End Sub

    Private Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM

        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", True)
    End Sub

    Private Sub Buscar()
        dgLista.PageIndex = 0
        CargarGrilla()
        If dgLista.Rows.Count = 0 Then
            AlertaJS("No se encontraron Registros")
        End If
    End Sub


    Private Sub CargarGrilla()
        
        Dim blMenu As New MnMenuAccesoBM
        Dim dt As DataTable
        Dim eMenuAcceso As New Menu_AccesoBE


        If ddlSituacion.SelectedValue = "Todos" Then
            eMenuAcceso.ESTADO = ""
        Else
            eMenuAcceso.ESTADO = ddlSituacion.SelectedValue
        End If

        eMenuAcceso.NOMBRE_MENU = tbNombreMenu.Text

        dt = blMenu.ListarBandeja(eMenuAcceso)

        dgLista.DataSource = dt
        dgLista.DataBind()

        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(dt.Rows.Count) + "')")
    End Sub

    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim oContactoBM As New ContactoBM
        Dim fila As Integer = e.CommandArgument
        With dgLista.Rows(fila)
            Dim codApl As String = DirectCast(.FindControl("hfCodAplicativo"), HiddenField).Value
            Dim estado As String = DirectCast(.FindControl("hfEstado"), HiddenField).Value
            Response.Redirect("frmRolEdicion.aspx?type=Edit&codRol=" & .Cells(3).Text & "&codApl=" & codApl & "&nomRol=" & .Cells(4).Text & "&est=" & estado)
        End With
    End Sub

    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim rolBM As New MnRolBM
        Dim rolBE As New MnRolBE

        Dim fila As Integer = e.CommandArgument
        With dgLista.Rows(fila)
            rolBE.CODIGO_APLICATIVO = DirectCast(.FindControl("hfCodAplicativo"), HiddenField).Value
            rolBE.CODIGO_ROL = .Cells(3).Text
            rolBE.ESTADO = "I"
            rolBE.NOMBRE_ROL = .Cells(4).Text
        End With

        If rolBM.Actualizar(rolBE, DatosRequest) Then
            AlertaJS("Se actualizó el Rol correctamente")
            CargarGrilla()
        End If

    End Sub

    Public Sub Menu(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim fila As Integer = e.CommandArgument
        With dgLista.Rows(fila)
            Dim codApl As String = DirectCast(.FindControl("hfCodAplicativo"), HiddenField).Value
            Response.Redirect("frmMenuRol.aspx?type=Edit&codRol=" & .Cells(3).Text & "&codApl=" & codApl)
        End With
    End Sub

    Private Sub Actualizar()
        Dim rolBM As New MnRolBM


    End Sub

#End Region

#Region "/* Eventos de la Pagina */"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                CargarPagina()
            End If
        Catch ex As Exception
            AlertaJS(ex.ToString())
        End Try
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Buscar()
        Catch ex As Exception
            AlertaJS(ex.ToString())
        End Try                
    End Sub

    Protected Sub btnAgregar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Response.Redirect("frmRolEdicion.aspx?type=Add&user=" + Usuario & "&codApl=" & WebConfigurationManager.AppSettings("codAplicativo"))
    End Sub

#End Region

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        dgLista.PageIndex = e.NewPageIndex
        CargarGrilla()
    End Sub
End Class
