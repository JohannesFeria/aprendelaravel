'Creado por: HDG OT 64769-4 20120404
Option Explicit On
Option Strict On

Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports UIUtility
Imports ParametrosSIT

Partial Class Modulos_Parametria_Tablas_Valores_frmBusquedaHechosImportancia
    Inherits BasePage

#Region "/* Metodos Personalizado */"

    Private Sub CargarPagina()
        If Not Page.IsPostBack Then
            CargarFiltros()
            Buscar()
        End If
    End Sub

    Private Sub CargarGrilla()
        Dim oHechosImportanciaBM As New HechosImportanciaBM
        Dim strSituacion As String, strCodigoPortafolio As String, decFecha As Decimal

        strSituacion = IIf(Me.ddlSituacion.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS), String.Empty, ddlSituacion.SelectedValue).ToString()
        strCodigoPortafolio = IIf(Me.ddlFondo.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS), String.Empty, ddlFondo.SelectedValue).ToString()
        If tbFecha.Text = Constantes.M_STR_TEXTO_INICIAL Then
            decFecha = 0
        Else
            decFecha = UIUtility.ConvertirFechaaDecimal(tbFecha.Text)
        End If

        Dim dtblDatos As DataTable = oHechosImportanciaBM.SeleccionarPorFiltro(strCodigoPortafolio, decFecha, strSituacion, DatosRequest).HechosImportancia

        dgLista.DataSource = dtblDatos
        dgLista.DataBind()
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dtblDatos) + "')")
    End Sub

    Private Sub CargarFiltros()

        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", True)
        Dim oPortafolioBM As New PortafolioBM
        ddlFondo.DataSource = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        ddlFondo.DataValueField = "CodigoPortafolio"
        ddlFondo.DataTextField = "Descripcion"
        ddlFondo.DataBind()
        Dim it As ListItem = New ListItem("Todos", "Todos")
        ddlFondo.Items.Insert(0, it)
        tbFecha.Text = Constantes.M_STR_TEXTO_INICIAL
    End Sub

    Private Sub Buscar()
        dgLista.PageIndex = 0
        CargarGrilla()

        If dgLista.Rows.Count = 0 Then
            AlertaJS(Constantes.M_STR_MENSAJE_NO_EXISTE_DATA)
        End If
    End Sub

    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Response.Redirect("frmHechosImportancia.aspx?codigo=" & e.CommandArgument.ToString())
        Catch ex As Exception
            AlertaJS("Ocurrió un error al modificar el registro")
        End Try
    End Sub

    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim oHechosImportanciaBM As New HechosImportanciaBM
            Dim strCodigoHechos As String

            strCodigoHechos = e.CommandArgument.ToString().Split(","c)(0)
            oHechosImportanciaBM.Eliminar(strCodigoHechos, DatosRequest)
            CargarGrilla()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub Ingresar()
        Response.Redirect("frmHechosImportancia.aspx")
    End Sub

    Private Sub Cancelar()
        Response.Redirect("~/frmDefault.aspx")
    End Sub

    Private Sub EditarNuevoIndice(ByVal nuevoIndice As Integer)
        dgLista.PageIndex = nuevoIndice
        CargarGrilla()
    End Sub

    Private Sub EditarCelda(ByVal oDataGridItem As GridViewRow)
        Dim imgEliminar As ImageButton

        If oDataGridItem.RowType = DataControlRowType.DataRow Then
            imgEliminar = DirectCast(oDataGridItem.FindControl("ibEliminar"), ImageButton)
            imgEliminar.Attributes.Add("onclick", ConfirmJS(Constantes.M_STR_MENSAJE_PREGUNTA_ELIMINAR_ENTIDAD))
        End If
    End Sub

#End Region

#Region "/* Eventos de la Pagina */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            CargarPagina()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Buscar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la búsqueda")
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Ingresar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al ingresar a la página")
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Cancelar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al salir de la página")
        End Try
    End Sub

#End Region

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            EditarNuevoIndice(e.NewPageIndex)
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            EditarCelda(e.Row)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Grilla")
        End Try
    End Sub
End Class
